import sys, os
from itertools import combinations
from graphviz import Digraph

#level_name = sys.argv[1]
#mpl = int(sys.argv[2])

mgl = 5   # max gram length to check till
min_thresh = 1  # min number of occurrences of skill+context atom must be greater than min_thresh to be considered
num_traj = 5  # number of different trajectories an atom must appear in to be considered
eng = 'dot'
level_grams = {}
levels = ['Level_00_3','Level_00_7','Level_01_3','Level_01_7','Level_02_3','Level_02_7','Level_03_3','Level_03_7']
for level_name in levels:
	files = []
	level_grams[level_name] = set()
	for f in os.listdir('.'):
		if f.startswith(level_name):
			files.append(f)
	print(files)

	trajectories = []
	for f in files:
		lines = open(f).readlines()
		processed, occ_count = [], 1  # processed keeps track of current trajectory
		prev_action, prev_context = lines[0].strip().split(' ')
		for line in lines[1:]:
			action, context = line.strip().split(' ')
			if action == 'win':
				break
			if action == prev_action and context == prev_context or prev_action == None:
				occ_count += 1
			else:
				processed.append(prev_action+prev_context) 
				occ_count = 1
			prev_action, prev_context = action, context
		processed.append(prev_action+prev_context)
		trajectories.append(processed)  # append trajectory in current file to list of trajectories

	#print(processed[0], len(processed))
	#print(trajectories[0], len(trajectories))
	#sys.exit()
	
	#num_traj = len(trajectories)
	

	g = Digraph('skillgrams', filename='skillgrams_' + 'mpl=' + str(mgl) + '_thresh=' + str(min_thresh) + '_nt=' + str(num_traj) + '_' + eng,engine=eng)
	
	grams = {}
	for i in range(1,mgl+1):
		grams[i] = {}
	for k, traj in enumerate(trajectories):
		#print(traj,'\n')
		#sys.exit()
		for j in range(1,mgl+1):
			for i in range(0,len(traj)-j):
				gram = tuple(traj[i:i+j])  # extract gram
				if gram not in grams[j]: 
					grams[j][gram] = [1, {k}]   # index 0: number of occurrences, index 1: set of trajectories in which present
				else:
					grams[j][gram][0] += 1
					grams[j][gram][1].add(k)

	#print(grams[4])
	#sys.exit()
	outfile = open('skillgrams_' + level_name + '.txt','w')
	print(level_name)
	for gr in grams:
		outfile.write('\nN: ' + str(gr) + '\n')
		gram = grams[gr]
		#print(gr)
		#print(gram)
		gram_sorted = dict(sorted(gram.items(), key=lambda item: item[1][0], reverse=True))
		#print('\n',gram_sorted)
		#sys.exit()
		for gs in gram_sorted:
			occ_count, traj_count = gram_sorted[gs][0], len(gram_sorted[gs][1])
			if occ_count > min_thresh and traj_count == num_traj:
				#print(gs, count, trajs)
				outfile.write(str(gs) + '\t' + str(occ_count) + '\t' + str(traj_count) + '\n')
				level_grams[level_name].add(gs)
	outfile.close()

	"""
	print('\n\n\n\n',level_name)
	for gram in level_grams[level_name]:
		print(gram)
	"""
#sys.exit()
#print('\n')
#print(level_grams)

out_file = open('fuzzy_' + 'mgl=' + str(mgl) + '_thresh=' + str(min_thresh) + '_nt=' + str(num_traj) + '.csv','w')
out_file.write('N=' + str(mgl) + ',')
for level_name in levels:
	out_file.write(level_name[6:] + ',')
out_file.write('\n')
for i, level_name in enumerate(levels):
	this_grams = level_grams[level_name]
	print('\nthis: ', level_name, len(this_grams))
	out_file.write(level_name[6:] + ',')
	#print(this_grams)
	for j, other in enumerate(levels):
		if other == level_name:
			out_file.write('X,')
			continue
		this_in_other_count = 0
		other_grams = level_grams[other]
		#print(other_grams)
		for tg in this_grams:
			if tg in other_grams:
				this_in_other_count += 1
		print('other: ', other, this_in_other_count, ((this_in_other_count*100)/len(this_grams)))
		fuzz = '{:.2f}'.format((this_in_other_count*100)/len(this_grams))
		out_file.write(fuzz + ',')
	out_file.write('\n')
out_file.close()

g_fuzz = Digraph('fuzzgrams', filename='fuzzgrams_' + 'mpl=' + str(mgl) + '_thresh=' + str(min_thresh) + '_nt=' + str(num_traj) + '_' + eng,engine=eng)
level_pairs = list(combinations(levels,2))

for l1,l2 in level_pairs:
	print('\n',l1)
	A = level_grams[l1]
	#print(A)
	#others = [l for l in levels if l is not level]
	#for other in others:
	B = level_grams[l2]
	#print(B)

	num_b_in_a, num_a_in_b = 0, 0
	for b in B:
		if b in A:
			num_b_in_a += 1
	for a in A:
		if a in B:
			num_a_in_b += 1
	
	a_fuzz = '{:.2f}'.format((num_a_in_b)*100/len(A))
	b_fuzz = '{:.2f}'.format((num_b_in_a)*100/len(B))
	if a_fuzz < b_fuzz:
		g_fuzz.edge(l1,l2)
	elif a_fuzz > b_fuzz:
		g_fuzz.edge(l2,l1)
	else:
		g_fuzz.edge(l1,l2)
		g_fuzz.edge(l2,l1)

g_fuzz.view()


sys.exit()
delta = -3
colors = {
	'00': {'3':'red','7':'darkred'},
	'01': {'3':'green','7':'darkgreen'},
	'02': {'3':'blue','7':'darkblue'},
	'03': {'3':'orange','7':'darkorange'},
}

x, y = delta, 0
prev_level_id = '00'
"""
for level in levels:
	new_level_id = level[6:8]
	if new_level_id != prev_level_id:
		y -= delta
		x = delta
	x -= delta
	prev_level_id = new_level_id
	pos_string = str(x) + ',' + str(y) + '!'
	#g.node(name=level,label='',pos = pos_string,color=colors[new_level_id][level[-1]],style='bold,filled',colorscheme='svg',penwidth='2.0')
	g.node(level,pos = pos_string,style='bold',penwidth='2.0')
"""

level_pairs = list(combinations(levels,2))

for l1,l2 in level_pairs:
	print('\n',l1)
	A = level_grams[l1]
	#print(A)
	#others = [l for l in levels if l is not level]
	#for other in others:
	B = level_grams[l2]
	#print(other, ': ', B.issubset(A))
	prop = len(B.intersection(A))/len(A)
	prop = '{:.2f}'.format(prop*100)
	AB = A.difference(B)
	BA = B.difference(A)
	print(l2)
	print(len(AB), len(BA))
	if len(AB) < len(BA):
		g.edge(l1,l2)
	elif len(BA) < len(AB):
		g.edge(l2,l1)

	#g.edge(level,other,prop)

g.view()