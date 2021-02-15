import os, sys, getopt
import numpy as np
import warnings
import json
from VSRstats.nonlinear import nonlinear

warnings.filterwarnings("ignore")

def openFile(filename):
    try:
        filename = filename[1:]
        f = open(filename, "r")
        return f.read();
    except Exception as e:
        print('Failed to open ' + filename);
        print(str(e))

def main(argv):
    inputfile = ''
    try:
      opts, args = getopt.getopt(argv, "hi:",["ifile="])
    except getopt.GetoptError:
      print('usage: test.py -i <inputfile>')
      sys.exit(2)
    for opt, arg in opts:
      if opt == '-h':
         print('usage: test.py -i <inputfile>')
         sys.exit()
      elif opt in ("-i", "--ifile"):
         inputfile = arg

    data = openFile(inputfile).split('\n');
    filename = os.path.basename(inputfile)
    filename = filename[:-4]
    signal = []

    for bit in data:
        if not (bit == ''):
            try:
                signal.append(int(bit));
            except Exception:
                pass;

    signal = np.array(signal)

    obj = nonlinear(signal).stats

    path = os.getenv('APPDATA')
    path = os.path.join(path, 'pulse', "{}_NONLINEAR.json".format(filename))
    with open(path, "w") as outfile:  
        json.dump(obj, outfile)

    print({"nonlinear": path})

main(sys.argv[1:])
