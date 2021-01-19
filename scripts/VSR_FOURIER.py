import os, sys, getopt
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

    freq, ft = fourier_transform(arr)
    arr = extract_bands(ft, freq)

    obj = { "count": sum([len(v) for v in arr]),
            "ulf": list(arr[0]),
            "vlf": list(arr[1]),
            "lf": list(arr[2]),
            "hf": list(arr[3]) }

    path = os.getenv('APPDATA')
    path = os.path.join(path, 'pulse', "{}.json".format(filename))
    with open(path, "w") as outfile:  
        json.dump(obj, outfile)
    
    print({"path": path})


main(sys.argv[1:])
