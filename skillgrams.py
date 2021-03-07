import sys, os
from itertools import combinations

level_name = sys.argv[1]
mpl = int(sys.argv[2])

files = []

for f in os.listdir('.'):
	if f.startswith(level_name):
		files.append(f)
print(files)

trajectories = []
for f in files:
	lines = open(f).readlines()
	processed, count = [], 1
	prev_action, prev_context = lines[0].strip().split(' ')
	for line in lines[1:]:
		action, context = line.strip().split(' ')
		if action == 'win':
			break
		if action == prev_action and context == prev_context or prev_action == None:
			count += 1
		else:
			processed.append(prev_action+prev_context)
			count = 1
		prev_action, prev_context = action, context
	processed.append(prev_action+prev_context)
	trajectories.append(processed)

grams = {}
for i in range(1,mpl+1):
	grams[i] = {}
for k, traj in enumerate(trajectories):
	print(traj,'\n')
	for j in range(1,mpl+1):
		for i in range(0,len(traj)-j):
			pat = tuple(traj[i:i+j])
			if pat not in grams[j]:
				grams[j][pat] = [1, {k}]
			else:
				grams[j][pat][0] += 1
				grams[j][pat][1].add(k)

outfile = open('skillgrams_' + level_name + '.txt','w')
for pl in grams:
	print(pl)
	outfile.write('\nN: ' + str(pl) + '\n')
	gram = grams[pl]
	gram_sorted = dict(sorted(gram.items(), key=lambda item: item[1][0], reverse=True))
	for gs in gram_sorted:
		count, trajs = gram_sorted[gs][0], len(gram_sorted[gs][1])
		if count > 1 and trajs > 1:
			print(gs, count, trajs)
			outfile.write(str(gs) + '\t' + str(count) + '\t' + str(trajs) + '\n')
outfile.close()