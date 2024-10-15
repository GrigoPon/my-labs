import re

def precedence(op):
    if op == '+' or op == '-':
        return 1
    if op == '*' or op == '/':
        return 2
    if op == '^':
        return 3
    return 0

def infix_to_postfix(expression):
    output = []
    stack = []
    
    tokens = re.findall(r'[+\-*/^()]|\d+', expression)
    
    for token in tokens:
        if token.isdigit():  # Если это число
            output.append(token)
        elif token == '(':  # Если это открывающая скобка
            stack.append(token)
        elif token == ')':  # Если это закрывающая скобка
            while stack and stack[-1] != '(':
                output.append(stack.pop())
            stack.pop()  # Удаляем '(' из стека
        else:  # Если это оператор
            while (stack and precedence(stack[-1]) >= precedence(token)):
                output.append(stack.pop())
            stack.append(token)

    # Добавляем оставшиеся операторы из стека
    while stack:
        output.append(stack.pop())
    
    return output

def evaluate_postfix(postfix):
    stack = []
    
    for token in postfix:
        if token.isdigit():  # Если это число
            stack.append(int(token))
        else:  # Это оператор
            b = stack.pop()
            a = stack.pop()
            if token == '+':
                stack.append(a + b)
            elif token == '-':
                stack.append(a - b)
            elif token == '*':
                stack.append(a * b)
            elif token == '/':
                stack.append(a / b)
            elif token == '^':
                stack.append(a ** b)

    return stack.pop()

def evaluate_expression(expression):
    postfix = infix_to_postfix(expression)
    result = evaluate_postfix(postfix)
    return result

input_expression = input('Введите выражение: ')
result = evaluate_expression(input_expression)
print('ОПЗ: ', infix_to_postfix(input_expression))
print(f"Результат выражения '{input_expression}' равен: {result}")
