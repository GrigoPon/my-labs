grammar = {"S": ["IA"], "A": ["GF", "e"], "B": ["+"], "C": ["a"], "D": ["b"], "F": ["AD"], "G": ["CC"], "H": ["BC"], "I": ["HA"]}
numer = {"S": [1], "A": [3], "B": [4], "C": [5], "D": [6], "F": [7], "G": [8], "H": [9], "I": [10]}
st = input()
for i in range(1, len(st)-1, 1):
    if st[i] == "b" and st[i-1] == "a":
        st = st[:i] + "e" + st[i:]
        break
print(st)

def cyk(st):
    n = len(st)
    
    t = [[set([]) for j in range(n)] for i in range(n)]
    
    for j in range(0, n):
        for l, rule in grammar.items():
            for r in rule:
                if len(r) == 1 and r[0] == st[j]:
                    t[0][j].add(l)
    
    for i in range(1, n):
        for j in range(0, n-i):
            for k in range(0, i):
                for l, rule in grammar.items():
                    for r in rule:
                        if len(r) == 2 and r[0] in t[k][j] and r[1] in t[k][j+i-k]:
                            t[i][j].add(l)
    if "S" in t[n-1][0]:
        print("consist")
    else:
        print("doesn't consist")
    return t
    
s = cyk(st)

for i in range(len(st)-1, -1, -1):
    for j in range(0, len(st) - i):
        if s[i][j] == set():
            print("     ", end=" ")
        else:
            print(s[i][j], end=" ")
    print()
    
def gen(i, j, A):
    if i == 0:
        for r in grammar[A]:
            if len(r) == 1:
                print(*numer[A], end=" ")
    elif i > 1:
        bol = 0
        for k in range(i-1, -1, -1):
            for r in grammar[A]:
                if len(r) == 2 and r[0] in s[k][j] and r[1] in s[k][j+i-k]:
                    print(*numer[A], end=" ")
                    gen(k, j, r[0])
                    gen(k, j+i-k, r[1])
                    bol = 1
                    break
            if bol: break

gen(len(st)-1, 0, "S")
