import os, sys
import argparse
import numpy as np
import warnings
import json
from VSRstats.frequency_domain import frequency_domain

warnings.filterwarnings("ignore")

def openFile(filename):
    try:
        return open(filename, "r").read()
    except Exception as e:
        print('Failed to open ' + filename)
        print(str(e))

parser = argparse.ArgumentParser()
parser.add_argument('-i', type=str, action='store')
parser.add_argument('-hz', type=int, action='store')
args = parser.parse_args()

data = openFile(args.i).split('\n');
signal = []

for bit in data:
    if not (bit == ''):
        try:
            signal.append(int(bit));
        except Exception:
            pass;

obj = frequency_domain(signal).stats if(args.hz==None)\
      else frequency_domain(signal, args.hz).stats
obj['update'] = 'false'

filename = os.path.basename(args.i)[:-4]

path = os.getenv('APPDATA')
path = os.path.join(path, 'pulse', "{}_FREQUENCY.json".format(filename ))
with open(path, "w") as outfile:  
    json.dump(obj, outfile)
    
print({"frequency": path})
