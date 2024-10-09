import math
def find_numbers(x):
    results = set()  # Используем множество, чтобы избежать дубликатов
    K = 0
    while 3**K <= x:
        L = 0
        while 5**L <= x:
            M = 0
            while 7**M <= x:
                current_value = 3**K * 5**L * 7**M
                if current_value <= x:
                    results.add(current_value)
                M += 1
            L += 1
        K += 1
    
    return sorted(results)

# Пример использования функции
x = int(input("Введите число x: "))
numbers = find_numbers(x)
print("Подходящие числа от 1 до", x, ":", numbers)
