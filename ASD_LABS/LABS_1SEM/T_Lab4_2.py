global gram, st

gram = {"S": ["+aaaaAbb"], "A": ["aaAb", "e"]}
st = input()
for i in range(0, len(st)-1):
   if st[i] == "a" and st[i+1] == "b":
      st = st[:i+1] + "e" + st[i+1:]
      break
st = st + " "

def scan(d, j):
   for situation in d[j-1]:
      if situation[1][situation[2]] == st[j-1]:
         d[j].append([situation[0], situation[1], situation[2]+1, situation[3]])
         
def complete(d, j):
   for numer, situation in enumerate(d[j]):
      if situation[2] >= len(situation[1]):
         for situa in d[situation[3]]:
            if situation[0] == situa[1][situa[2]]:
               d[j][numer] = [situa[0], situa[1], situa[2]+1, situa[3]]

def predict(d, j):
   for situation in d[j]:
      for l in gram.keys():
         for r in gram[l]:
            if situation[1][situation[2]] == l and [l, r, 0, j] not in d[j]:
               d[j].append([l, r, 0, j])

def early(st):
   n = len(st)
   d = [[] for x in range(n)]
   d[0].append(["s", "S ", 0, 0])
   print(d)      

   for j in range(0, n):
      print(j, " ", st[j])
      scan(d, j)
      oldlen = len(d[j])
      complete(d, j)
      predict(d, j)
      newlen = len(d[j])
      while newlen != oldlen:
         oldlen = newlen
         complete(d, j)
         predict(d, j)
         newlen = len(d[j])
      print(d)
      
   for i in range(len(d[n-1])):
      if d[n-1][i] == ["s", "S ", 1, 0] and i == len(d[n-1]) - 1:
         print("consist")
         break
   else:
      print("doesn't consist")

early(st)
'''d = [[["A", "e ", 1]], []]
complete(d,0)
print(d)'''
