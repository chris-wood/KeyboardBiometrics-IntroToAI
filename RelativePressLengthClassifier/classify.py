#!/usr/bin/python

import sys
import pickle

import common

if __name__=='__main__':
    # print usage statement if requested
    if len(sys.argv) == 2 and sys.argv[1].lower() in ('-h', '--help'):
        print "python {0} [log1 log2 ... logN]".format(sys.argv[0])
        exit(1)

    # load the known features
    try:
        with open(common.FEATURES_FILE, 'rb') as f:
            stats = pickle.load(f)
    except IOError as e:
        print >>sys.stderr, e
        exit(-1)
    print "Loaded {0} feature set(s)".format(len(stats))
    
    # process any logs
    for arg in sys.argv[1:]:
        try:
            print "Processing:", arg
            with open(arg, 'rb') as f:
                log_stats = common.process_log(f)
                
                # match againts the known features
                best_match = (float('inf'), "Unknown")
                for name, known in stats.iteritems():
                    match_val = common.compare_feature_sets(known, log_stats)
                    
                    #print "   ", match_val, name
                    
                    if match_val < best_match[0]:
                        best_match = (match_val, name)
                print "  Best match:", best_match[1], best_match[0]

        except IOError as e:
            print >>sys.stderr, e
    
