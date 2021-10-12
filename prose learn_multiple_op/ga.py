import random
import pandas as pd
from math import ceil,floor

def create_individual(operations, size):
    rand_ops = []
    for i in range(size):
        rand_ops.append(operations[random.randint(0, 3)])
    return rand_ops

def initial_pop(operations, pop_size, size):
    pop = []
#     print(pop_size)
    for i in range(pop_size):
        ind = create_individual(operations, size)
        pop.append(ind)
    return pop

def value(inp, ind):
    value = inp[0]
    for i in range(len(ind)):
        value = calc(value, inp[i+1], ind[i])
    return value

def calc(a, b, op):
    if(op=='+'):
        return a+b
    elif(op=='-'):
        return a-b
    elif(op=='*'):
        return a*b
    elif(op=='/'):
        ans = a/b
        if(int(ans)==ans):
            return ans
        else:
            return int(ans)+100
        
def check_mutate():
    return random.randint(0, 1)

def mutation(c, operations):
    mut = check_mutate()
    if(mut==1):
        for i in range(len(c)):
            m = check_mutate()
            if(m==1):
                newop = operations[random.randint(0, 3)]
                c[i] = newop
    return c
    
def crossover(parent1, parent2, inp):
    cut = int((len(inp)-1)/2)
    # each pair of parents give 2 children
    child1 = []
    child2 = []
    for i in range(cut):
        child1.append(parent1[i])
        child2.append(parent2[i])

    for i in range(cut, len(inp)-1):
        child1.append(parent2[i])
        child2.append(parent1[i])
    return child1, child2

def next_pop(old_pop, children, fit_size, inp, operations):
    
    new_pop = []
    # add top n fittest to new_pop
    fittest = old_pop.nsmallest(fit_size, 'Fitness')
    # select random parents for crossover
    parents = random.sample(list(old_pop['Sequence']), children)
    for i in fittest['Sequence']:
        new_pop.append(i)
    for i in range(int(len(parents)/2)):
        c1, c2 = crossover(parents[i], parents[len(parents)-1-i], inp)
        c1 = mutation(c1, operations)
        c2 = mutation(c2, operations)
        new_pop.append(c1)
        new_pop.append(c2)
    return new_pop

def make_df(pop, inp, out):
    old_pop = []
    for ind in pop:
        row = []
        op = value(inp, ind)
        row.append(op)
        row.append(abs(out-op))
        row.append(ind)
        old_pop.append(row)
    old_pop = pd.DataFrame(old_pop, columns=['Output', 'Fitness', 'Sequence'])
    return old_pop

def get_candidate_space(inp):
    return (pow(4,len(inp)-1))

def get_population_size(perc, n):
    return ceil(perc/100 * n)

def get_children_size(perc, n):
    perc = perc/100
    if(ceil(perc*n)%2):
        return floor(perc*n)
    return ceil(perc*n)

def fittest_size(pop_size, children):
    return pop_size - children

def ga(inp, out):
    operations = ['+', '-', '*', '/']
    itr = (len(inp)-1 )* 50
    candidate = get_candidate_space(inp)
    pop_size = get_population_size(50, candidate)
    children = get_children_size(60, pop_size)
    fit_size = fittest_size(pop_size, children)
    
    p = initial_pop(operations, pop_size, len(inp)-1)
    p = make_df(p, inp, out)
    p = p.sort_values('Fitness')
#    print('INITIAL POPULATION')
#    print(p)
    
    for i in range(itr):
#         print('ITERATION ', i+1)
        n = next_pop(p, children, fit_size, inp, operations)
        n = make_df(n, inp, out)
        n = n.sort_values('Fitness')
#        print(n)
        p = n
#         print('-------------------------------------------')
    return p


import sys
# print (type(sys.argv[1]))
o = int(sys.argv[1])
# print(o, type(o))

inp = [] 
for i in range(2, len(sys.argv)):
    inp.append(int(sys.argv[i]))
# print(inp)

p = ga(inp, o)
#print('FINAL ITERATION')
#print(p)

res = set()
for row in p.iterrows():
    if(row[1][1]==0):
        res.add(tuple(row[1][2]))
    
print(list(res))
