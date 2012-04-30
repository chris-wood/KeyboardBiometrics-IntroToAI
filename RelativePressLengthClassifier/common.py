#!/usr/bin/python

import re
from collections import deque

FEATURES_FILE = "key_features.dat"  # file to save the extracted stats in
AVERAGE_WINDOW = 50         # number of most recent key presses to average
OUTLIER_SCALE = 8.0         # scale factor used to define the outlier range
MIN_SAMPLES_TO_COMPARE = 2  # number of samples required to allow a sample to be used

# a class for storing stats per character
class StatObj(object):
    count = 0
    mean = 0
    m2 = 0  # second moment ?
    variance = None
    std_dev = None

    # update the stats with a new value
    def update(self, x):
        self.count += 1
        delta = x - self.mean
        self.mean += delta/self.count
        self.m2 += delta * (x - self.mean)
    
    # derive other stats
    def finalize(self):
        # estimated population variance/stddev
        if self.count > 1:
            self.variance = self.m2/(self.count - 1)
            self.std_dev = self.variance ** 0.5
        else:
            # technically not correct but it seems to make sense in this context
            self.std_dev = self.variance = float('inf')
        
        # sample variance/stddev
        #self.variance_n = self.m2/self.count
        #self.std_dev_n = self.varience_n ** 0.5
        

# returns a number indicating the similarity between two feature sets.
# it is assumed that there is significant overlap between the sets so that
# missing keys can be safely ignored
def compare_feature_sets(a, b):
    feature_overlap = 0.0     # number of overlapping features
    total = 0.0
    
    for key, stats_a in a.iteritems():
        try:
            stats_b = b[key]
        except KeyError:
            continue
        
        if min(stats_a.count, stats_b.count) < MIN_SAMPLES_TO_COMPARE:
            continue
        
        feature_overlap += 1
        mean_diff = abs(stats_a.mean - stats_b.mean)
        combined_count = max(stats_a.count, stats_b.count)
        total += mean_diff*combined_count
    
    if feature_overlap > 0:
        return total/feature_overlap
    else:
        return float('inf')


# reads a log and returns an object containing features
def process_log(f, verbose = False):
    # define the regex used to split log lines
    line_re = re.compile(r"^(\d+)\s+(\d+)\s+KEY_(DOWN|UP)\s*$")
    
    states = {}     # map of key codes to the last seen state/time
    stats = {}      # map of key codes to statistics
    
    recent_press_lengths = deque()
    average_press_length = 0.0
    
    # iterate through lines in the log
    for line in f:
        # split the line
        m = line_re.match(line)
        if m is None:
            if verbose:
                print "invalid log line"
            continue    # invalid lines are ignored silently
        ts, code, down = m.groups()
        ts = int(ts)
        code = int(code)
        down = (down == "DOWN")
        
        try:
            last_ts, last_state = states[code]
            if down != last_state:  # state is changing
                if down:        # key is being pressed
                    pass
                else:           # key is being released
                    press_length = float(ts - last_ts)
                    
                    # if ready, find the relative press length
                    if len(recent_press_lengths) == AVERAGE_WINDOW:
                        relative_length = press_length / average_press_length
                        
                        # update the average (excluding outliers)
                        if relative_length > 1.0/OUTLIER_SCALE and relative_length < OUTLIER_SCALE:
                            # update the key stats
                            try:
                                stats[code].update(relative_length)
                            except KeyError:
                                stats[code] = stats_obj = StatObj()
                                stats_obj.update(relative_length)
                        
                            # remove the oldest value
                            average_press_length -= recent_press_lengths.popleft()
                            
                            # add the next value to the average
                            recent_press_lengths.append(press_length/AVERAGE_WINDOW)
                            average_press_length += recent_press_lengths[-1]
                        else:
                            if verbose:
                                print "  outlier:", relative_length, code, ts
                    else:
                        # average is not yet stable (keep adding values)
                        recent_press_lengths.append(press_length/AVERAGE_WINDOW)
                        average_press_length += recent_press_lengths[-1]
                # update the state map with the new state
                states[code] = (ts, down)
        except KeyError:
            # never seen before - always add to the map
            states[code] = (ts, down)
        
    # all data had been added, compute derived stats
    for code, stats_obj in stats.iteritems():
        stats_obj.finalize()
        
    return stats
