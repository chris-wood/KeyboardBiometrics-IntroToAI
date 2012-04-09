import operator
import sys


def print_dict_sorted(d, zero_only, filter_zero, show_coverage):
    c = 0
    for pair in sorted(d.iteritems(), key=operator.itemgetter(1), reverse=True):
        if pair[1] > 0:
            c += 1
        if zero_only and pair[1] > 0:
            continue
        if filter_zero and pair[1] == 0:
            continue
        print pair

    if show_coverage:
        p = 100 * c / len(d.values())
        print "Coverage: " + str(c) + "/" + str(len(d.values())) + " " + str(p) + "%"
    

if len(sys.argv) < 2:
    sys.exit('''\
Usage: text_analysis [-[options]] filename [flags]
or more information: text_analysis --help\
''')

if sys.argv[1] == '--help':
    print '''\
Usage: text_analysis [-[options]] filename [flags]
options
    d - show digraphs
    t - show trigraphs
    r - show doubles


flags:
    --filter-zero   - filter out instances with frequency < 0
                            (conflicts with --zero-only)
    --zero-only     - print out only entries with frequency == 0
                            (conflicts with --filter-zero)
    --show-coverage - print out a coverage percentage for each stat\
'''

    sys.exit()

options = sys.argv[2:]
filter_zero = '--filter-zero' in options
zero_only = '--zero-only' in options
show_coverage = '--show-coverage' in options

if filter_zero and zero_only:
    sys.exit("Error: conflicting flags")

s = ''
s_upper = ''
try:
    s = file(sys.argv[1]).read()
    s_upper = s.upper()
except IOError:
    sys.exit("Error: Invalid file name")

letter_freq = {}
for i in range(65, 123):
    letter_freq[chr(i)] = 0
letter_freq[' '] = 0
letter_freq[')'] = 0
letter_freq['('] = 0
letter_freq[','] = 0
letter_freq['.'] = 0
letter_freq['-'] = 0
letter_freq[';'] = 0
letter_freq['\n'] = 0

digraphs = []
for i in range(65, 91):
    for j in range(65, 91):
        digraphs.append(chr(i) + chr(j))

common_digraphs = ['TH', 'HE', 'AN', 'IN', 'ER', 'ON', 'RE', 'ED', 'ND', 'HA', 'AT', 'EN', 'ES', 'OF', 'NT', 'EA', 'TI', 'TO', 'IO', 'LE', 'IS', 'OU', 'AR', 'AS', 'DE', 'RT', 'VE']
common_trigraphs = ['THE', 'AND', 'THA', 'ENT', 'ION', 'TIO', 'FOR', 'NDE', 'HAS', 'NCE', 'TIS', 'OFT', 'MEN']
common_doubles = ['SS', 'EE', 'TT', 'FF', 'LL', 'MM', 'OO']

digraph_freq = {}
common_digraph_freq = {}
common_trigraph_freq = {}
double_freq = {}

print "Checking common digraphs..."
for digraph in common_digraphs:
    count = s_upper.count(digraph)
    common_digraph_freq[digraph] = count

print "Checking common trigraphs..."
for trigraph in common_trigraphs:
    count = s_upper.count(trigraph)
    common_trigraph_freq[trigraph] = count

print "Checking common doubles..."
for double in common_doubles:
    count = s_upper.count(double)
    double_freq[double] = count

print "Checking all digraphs..."
for digraph in digraphs:
    count = s_upper.count(digraph)
    digraph_freq[digraph] = count

print "Done"


print "========Common Digraphs========"
print_dict_sorted(common_digraph_freq, zero_only, filter_zero, show_coverage)

print "========Common Trigraphs========"
print_dict_sorted(common_trigraph_freq, zero_only, filter_zero, show_coverage)

print "========Common Doubles========"
print_dict_sorted(double_freq, zero_only, filter_zero, show_coverage)

print "========All Digraphs========"
print_dict_sorted(digraph_freq, zero_only, filter_zero, show_coverage)

