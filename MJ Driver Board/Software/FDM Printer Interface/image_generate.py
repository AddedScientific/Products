import numpy as np
from PIL import Image


dpiy = 4

spacx = 30
spacy = spacx * dpiy


lengt = 128*dpiy*3

array = np.ones([128, lengt], dtype=np.uint8)*255

for x in range(128):
    if x % spacx == 0:
        array[x,:] = 0
        
for y in range(lengt):
    if y % spacy == 0:
        array[:,y] = 0

img = Image.fromarray(array)
img.save('verylowdense.bmp')
