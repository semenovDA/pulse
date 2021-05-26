import os, sys
import numpy as np
import argparse, warnings, json
from VSRstats.tools import *
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
obj["peaks"] = getPeaks(np.array(signal)) if(args.hz==None)\
               else getPeaks(np.array(signal), args.hz)
obj["filtered"] = filterdSignal(np.array(signal)) if(args.hz==None)\
                  else filterdSignal(np.array(signal), args.hz)

obj["filtered"] = normalize(obj["filtered"])
obj["normalized"] = normalize(signal)

# Format ndarray to list
obj["peaks"] = list([float(i) for i in obj["peaks"]])
obj["filtered"] = list([float(i) for i in obj["filtered"]])
obj["normalized"] = list([float(i) for i in obj["normalized"]])

filename = os.path.basename(args.i)[:-4]

path = os.getenv('APPDATA')
path = os.path.join(path, 'pulse', "{}_SIGNAL.json".format(filename))
with open(path, "w") as outfile:  
    json.dump(obj, outfile)

print({"signal": path})
