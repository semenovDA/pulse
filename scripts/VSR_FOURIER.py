import os, sys
import argparse
import numpy as np
import warnings
import json
import uuid

warnings.filterwarnings("ignore")

def fourier_transform(signal):
    ft_ = np.fft.fft(signal)
    ft = 2.0 * abs(ft_ / len(ft_))

    freq = np.fft.fftfreq(len(ft_))
    
    return freq[freq > 0], ft[freq > 0]
    
def extract_bands(ft, freq):
    ulf = np.where(freq <= 0.015)[0].max()
    vlf = np.where(freq <= 0.04)[0].max()
    lf = np.where(freq <= 0.15)[0].max()
    hf = np.where(freq <= 0.4)[0].max()
    
    return ft[:ulf],\
           ft[ulf:vlf],\
           ft[vlf:lf],\
           ft[lf:hf]

def openFile(filename):
    try:
        return open(filename, "r").read();
    except Exception as e:
        print('Failed to open ' + filename);
        print(str(e))

parser = argparse.ArgumentParser()
parser.add_argument('-i', type=str, action='store')
args = parser.parse_args()

data = openFile(args.i).split('\n');
signal = []

for bit in data:
    if not (bit == ''):
        try:
            signal.append(int(bit));
        except Exception:
            pass;

freq, ft = fourier_transform(arr)
arr = extract_bands(ft, freq)

obj = { "count": sum([len(v) for v in arr]),
        "ulf": list(arr[0]),
        "vlf": list(arr[1]),
        "lf": list(arr[2]),
        "hf": list(arr[3]) }

obj['update'] = 'false'    

filename = os.path.basename(args.i)[:-4]

path = os.getenv('APPDATA')
path = os.path.join(path, 'pulse', "{}.json".format(filename))
with open(path, "w") as outfile:  
    json.dump(obj, outfile)
    
print({"fourier": path})
