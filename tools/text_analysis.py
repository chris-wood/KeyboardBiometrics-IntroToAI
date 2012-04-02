import operator
import sys

if len(sys.argv) < 2:
    sys.exit("Usage: python text_analysis filename")

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
common_double = ['SS', 'EE', 'TT', 'FF', 'LL', 'MM', 'OO']

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
for double in common_double:
    count = s_upper.count(double)
    double_freq[double] = count

print "Checking all digraphs..."
for digraph in digraphs:
    count = s_upper.count(digraph)
    digraph_freq[digraph] = count

print "Computing letter frequency..."
for word in s:
    for letter in word:
        letter_freq[letter] += 1

print "Done!"



# print common digraphs
print "========Common Digraphs========"
for pair in sorted(common_digraph_freq.iteritems(), key=operator.itemgetter(1), reverse=True):
    if pair[1] > 0:
        print pair


# print common trigraphs
print "========Common Trigraphs========"
for pair in sorted(common_trigraph_freq.iteritems(), key=operator.itemgetter(1), reverse=True):
    if pair[1] > 0:
        print pair


# print doubles
print "========Common Doubles========"
for pair in sorted(double_freq.iteritems(), key=operator.itemgetter(1), reverse=True):
    if pair[1] > 0:
        print pair


# print all digraphs
print "========All Digraphs========"
for pair in sorted(digraph_freq.iteritems(), key=operator.itemgetter(1), reverse=True):
    print pair


# print letter frequency
print "========Letter Frequency========"
for pair in sorted(letter_freq.iteritems(), key=operator.itemgetter(1), reverse=True):
    print pair