import os, sys
import argparse, json
import numpy as np
import warnings
from VSRstats.time_domain import time_domain
from VSRstats.pars_rating import pars_rating
warnings.filterwarnings("ignore")

def openFile(filename):
    try:
        return open(filename, "r").read();
    except Exception as e:
        print('Failed to open ' + filename);
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

obj = {}
obj['stats'] = time_domain(signal).stats if(args.hz==None)\
               else time_domain(signal, args.hz).stats
obj['pars'] = pars_rating(signal).stats if(args.hz==None)\
              else pars_rating(signal, args.hz).stats

for key in obj['stats']:
    obj['stats'][key] = float(obj['stats'][key])

obj['update'] = 'false'

filename = os.path.basename(args.i)[:-4]

path = os.getenv('APPDATA')
path = os.path.join(path, 'pulse', "{}_STATS.json".format(filename))
with open(path, "w") as outfile:  
    json.dump(obj, outfile)

print({"stats": path})
