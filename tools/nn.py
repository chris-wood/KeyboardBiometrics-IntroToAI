import sys
import math


def euclidean_distance(models, query):

	results = []

	for model in models:
		distance = 0
		# go over every feature minus the last one
		# remember that the classifier is last and these are parallel lists
		for i in range(len(model) - 1):
			distance = distance + math.pow( (float(model[i]) - float(query[i])), 2 )
		distance = math.sqrt(distance)
		results.append( (model[-1], distance) )

	closest = results[0]
	for result in results:
		if result[1] < closest[1]:
			print "Lower!"
			closest = result

	return closest

		

def main():
	args = sys.argv
	training_file = open(args[1])
	testing_file = open(args[2])

	training_contents = training_file.read().split('\n')
	testing_contents = testing_file.read().split('\n')

	# Close the files
	training_file.close()
	testing_file.close()

	print "Scrubbing Data..."

	training_features = training_contents[0].split(',')
	testing_features = testing_contents[0].split(',')

	# don't include the feature list in the contents
	training_contents.pop(0)
	testing_contents.pop(0)

	for i in range(len(training_contents)):
		training_contents[i] = training_contents[i].split(',')

	for i in range(len(testing_contents)):
		testing_contents[i] = testing_contents[i].split(',')
	# everything is tokenized at this point



	# this will store indexes for 0-pruning
	training_index_prune = []
	testing_index_prune = []
	
	# prune out the features that have zero as a value for entries from the feature list.
	for i in range(len(training_features)):
		# don't throw away instances of 0 if someone else actually used the key
		used = False
		for row in training_contents:
			if row[i] != '0':
				used = True

		#if training_contents[0][i] == '0':
		if not used:
			training_index_prune.append(i)
			for j in range(len(training_contents)):
				training_contents[j][i] = None

	
	for i in range(len(testing_features)):
			if testing_contents[0][i] == '0':
				testing_index_prune.append(i)
				testing_contents[0][i] == None


	# actually 0-prune the feature lists
	temp = []
	for i in range(len(training_features)):
		if i not in training_index_prune:
			temp.append(training_features[i])
	training_features = temp

	temp = []
	for i in range(len(testing_features)):
		if i not in testing_index_prune:
			temp.append(testing_features[i])
	testing_features = temp


	# actually 0-prune rows in the contents
	for i in range(len(training_contents)):
		training_contents[i] = filter(lambda val: val != None, training_contents[i])
	
	for i in range(len(testing_contents)):
		testing_contents[i] = filter(lambda val: val != None, testing_contents[i])


	# AT THIS POINT, *_contents has non-zero elements, and the feature lists reflect this.
	# Now we want only features that coincide.
	# this will store indexes for non-commonality pruning
	common_features = set(training_features).intersection(testing_features)

	training_index_prune = []
	testing_index_prune = []

	# find the indexes that need to get pruned out for each data set
	# we do it this way to preserve the ordering of the csv
	for i in range(len(training_features)):
		if training_features[i] not in common_features:
			training_index_prune.append(i)
			training_features[i] = None

	for i in range(len(testing_features)):
		if testing_features[i] not in common_features:
			testing_index_prune.append(i)
			testing_features[i] = None

	# actually prune the non-commonalities in features
	training_features = filter(lambda val: val != None, training_features)
	testing_features = filter(lambda val: val != None, testing_features)


	# mark non-commonalities for removal
	for i in range(len(training_contents)):
		for j in training_index_prune:
			training_contents[i][j] = None

	for i in range(len(testing_contents)):
		for j in testing_index_prune:
			testing_contents[i][j] = None


	# actually prune the non-commonalities in contents
	for i in range(len(training_contents)):
		training_contents[i] = filter(lambda val: val != None, training_contents[i])

	for i in range(len(testing_contents)):
		testing_contents[i] = filter(lambda val: val != None, testing_contents[i])

	print "Done."
	print "Running Classifier..."
	print "EUCLIDEAN: " + str(euclidean_distance(training_contents, testing_contents[0]))

if __name__ == '__main__':
	main()