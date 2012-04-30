#!/usr/bin/python

import sys
import pickle

import common


if __name__=='__main__':
    # print usage statement if requested
    if len(sys.argv) == 2 and sys.argv[1].lower() in ('-h', '--help'):
        print "python {0} [log1 log2 ... logN]".format(sys.argv[0])
        exit(1)

    # process any logs
    stats = {}
    for arg in sys.argv[1:]:
        try:
            print "Processing:", arg
            with open(arg, 'rb') as f:
                log_stats = common.process_log(f)
                stats[arg] = log_stats
        except IOError as e:
            print >>sys.stderr, e
    
    print "Writing extracted features to:", common.FEATURES_FILE
    try:
        with open(common.FEATURES_FILE, 'wb') as f:
            pickle.dump(stats, f, -1)
    except IOError as e:
        print >>sys.stderr, e
        exit(-1)
