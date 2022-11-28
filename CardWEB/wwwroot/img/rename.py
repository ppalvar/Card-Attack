from operator import ne
import os

files = os.listdir()

i = 0

for file in reversed(files):
    if 'card' not in file:
        continue
    
    new_name = f'card_{i}.png'
    i += 1
    os.rename(file, new_name)
