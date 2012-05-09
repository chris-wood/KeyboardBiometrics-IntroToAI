import sys

"""
This script just pulls out features that I don't want to use with my classifier.
It also puts the classifier at the end, not at the start.
"""

if len(sys.argv) != 3:
    print 'Usage: python prune.py [full_csv] [pruned_csv]'
    sys.exit()

f = open("zebra-files")
contents = f.read()
f.close()
contents = contents.split('\n')
features = contents[0].split(',')
contents.pop(0)

combos = ['8427', '7269', '7378', '6978', '7884', '8269', '6982', '6578', '8473', '6983', '7978', '6584', '8472']
inds = []
pruned_features = ''
for i in range(len(features)):
    # This is the main line that filters features
    if (features[i].find("avg") != -1):
        good = True
        if good:
            pruned_features = pruned_features + features[i] + ','
            inds.append(i)
                
pruned_features = pruned_features + '\n'

inds.insert(0, 0)

pruned = []
for row in contents:
    l = []
    i = 0
    items = row.split(',')
    for item in items:
        if i in inds:
            l.append(item)
        i = i + 1
    pruned.append(l)

f = open("pruned.csv", "w")

s = pruned_features
for row in pruned:
    for item in row:
        s = s + item + ','
    s = s + '\n'
f.write(s)
f.close()