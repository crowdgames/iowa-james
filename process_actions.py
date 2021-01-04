import sys

file_name = sys.argv[1]

lines = open(file_name).readlines()
count = 1
prev_action, prev_context = lines[0].strip().split(' ')
for line in lines[1:]:
    #print line
    action, context = line.strip().split(' ')
    if action == prev_action and context == prev_context or prev_action == None:
        count += 1
    else:
        print prev_action, prev_context, count
        count = 1
    prev_action, prev_context = action, context
print prev_action, prev_context, count
