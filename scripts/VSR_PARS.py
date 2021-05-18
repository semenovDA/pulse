import os, sys, getopt
import numpy as np
import warnings
import json
import uuid

from VSRstats.pars_rating import pars_rating
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
    arr = []

    for bit in data:
        if not (bit == ''):
            try:
                arr.append(int(bit));
            except Exception:
                pass;

    print({"pars": pars_rating(arr).stats})


main(sys.argv[1:])
