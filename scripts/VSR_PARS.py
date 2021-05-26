import os
import argparse
import numpy as np
import warnings
import json
import uuid

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

print({"pars": pars_rating(signal).stats}) if(args.hz==None)\
    else print({"pars": pars_rating(signal, args.hz).stats})
