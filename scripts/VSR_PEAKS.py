import os, sys, getopt
import numpy as np
import warnings
from VSRstats import getPeaks
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
      print('usage: VSR_PEAKS.py -i <inputfile>')
      sys.exit(2)
    for opt, arg in opts:
      if opt == '-h':
         print('usage: VSR_PEAKS.py -i <inputfile>')
         sys.exit()
      elif opt in ("-i", "--ifile"):
         inputfile = arg

    data = openFile(inputfile).split('\n');
    arr = []

    for bit in data:
        if not (bit == ''):
            try:
                arr.append(int(bit));
            except Exception:
                pass;

    arr = getPeaks(np.array(arr));
            
    print('{' + 'peaks: {0}'.format(arr.tolist()) + '}')

main(sys.argv[1:])

