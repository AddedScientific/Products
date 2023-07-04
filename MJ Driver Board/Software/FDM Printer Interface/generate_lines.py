import numpy as np
from PIL import Image


#dpi = 0.03 #30 microns

dist = 0.030 #30 microns


lengt = round(50 / dist) + 25
print(lengt)
array = np.ones([128, lengt], dtype=np.uint8)*255

divider = 1

for x in range(128):
    if x % 10 == 0:
        array[x,:] = 0

one = round(lengt * 750 / 1025)
two = round(lengt * 800 / 1025)
thr = round(lengt * 850 / 1025)

for y in [one, two, thr]:
    for x in range(128):
        if(array[x,y] == 0):
            array[x,y] = 255
        else:
            array[x,y] = 0

array[:,0:9] = 0
array[:,-35:] = 0
array[0,:] = 0
array[-1,:] = 0

img = Image.fromarray(array)
img.save('Lines_%d_microns.bmp'%(dist*1000))
