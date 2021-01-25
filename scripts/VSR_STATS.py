import os, sys, getopt
import numpy as np
import warnings
from VSRstats.time_domain import time_domain
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
    signal = []

    for bit in data:
        if not (bit == ''):
            try:
                signal.append(int(bit));
            except Exception:
                pass;

    obj = {}
    obj['stats'] = time_domain(signal).stats
    print(obj)


main(sys.argv[1:])
