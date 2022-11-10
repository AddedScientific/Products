import os
import re
from string import printable


dirname = os.path.dirname(__file__)

input_file = dirname + "\\main\\A_600_SerialFunctions.ino"

output_file = dirname + "/Serial commands.csv"

search_exp = ".*if.*\(.*controlByte.*==.*'.'"

coll = dict()

with open(input_file) as f:
    lines = f.readlines()



line_count = 0
found_start = -1
found_finish = 0
found_label = ''
first_code_line = 0
last_code_line = 0

for line in lines:
    out = re.search(search_exp, line)
    if(out):
        #first finish the last one
        if(found_start > 0):
            found_finish = line_count - 1
            coll[found_label] = (found_start, found_finish)        
        
        out = re.match(search_exp, line)
        found_start = line_count
        found_label = out.group()[-2]
        #print("%03.0d\t%s"%(line_count, found_label))

    line_count+=1


found_finish = line_count - 2
coll[found_label] = (found_start, found_finish)  

#print(coll)
#print()

list_of_chars = printable

out_file = []

for char in list_of_chars[0:-6]:
    if char in '"':
        print("skipping")
    elif char in coll:
        desc = lines[1+coll[char][0]][0:-1]
        desc = re.split('//', desc)
        desc = desc[1]
        cmd_arg = lines[2+coll[char][0]][0:-1]
        if len(cmd_arg) > 12:
            if '//' in cmd_arg:
                search_comment = '//'
                cmd_arg = re.split(search_comment, cmd_arg)
                cmd_arg = cmd_arg[1]
            else:
                cmd_arg = "No arguments accepted."
        else:
            cmd_arg = "No arguments accepted."

        
        out_file.append("%s \t %s \t %s"%(char, desc, cmd_arg))
    else:
        out_file.append("%s \t Unused \t No arguments accepted"%char)


with open(output_file, 'w') as f:
    for line in out_file:
        f.write(line)
        f.write("\n")
    
