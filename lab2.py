class L1:
    def __init__(self):
        self.state = 'P'  # Начальное состояние

    def reset(self):
        self.state = 'P'

    def process(self, string):
        self.reset()
        for char in string:
            if self.state == 'P':
                if char == 'a':
                    self.state = 'Q'  # Переход из P в Q
                elif char == 'b':
                    self.state = 'P'  # Остаёмся в P
                else:
                    return False  # Неверный символ
            elif self.state == 'Q':
                if char == 'a':
                    self.state = 'P'  # Переход из Q в P
                elif char == 'b':
                    self.state = 'Q'  # Остаёмся в Q
                else:
                    return False  # Неверный символ

        return self.state == 'Q'  # Успешное завершение, если мы в состоянии Q

class Numer1:
    def __init__(self):
        self.state = 'S'  # Начальное состояние

    def reset(self):
        self.state = 'S'

    def process(self, string):
        self.reset()
        for char in string:
            if self.state == 'S':
                if char == 'a':
                    self.state = 'A'  # Переход из S в A
                elif char == 'b':
                    self.state = 'F'  # Переход из S в F (конечное состояние)
                else:
                    return False  # Неверный символ
            elif self.state == 'A':
                if char == 'b':
                    self.state = 'B'  # Переход из A в B
                elif char == 'a':
                    self.state = 'C'  # Переход из A в C
                else:
                    return False  # Неверный символ
            elif self.state == 'B':
                if char == 'b':
                    self.state = 'B'  # Остаётся в B
                elif char == 'c':
                    self.state = 'A'  # Переход из B в A
                else:
                    return False  # Неверный символ
            elif self.state == 'C':
                if char == 'c':
                    self.state = 'A'  # Переход из C в A
                elif char == 'b':
                    self.state = 'F'  # Переход из C в F (конечное состояние)
                else:
                    return False  # Неверный символ

        return self.state == 'F'  # Успешное завершение, если мы в состоянии F

class Num4_2:
    def __init__(self):
        self.state = 'P'  # Начальное состояние

    def reset(self):
        self.state = 'P'

    def process(self, string):
        self.reset()
        for char in string:
            if self.state == 'P':
                if char == 'a':
                    self.state = 'P'  # Остаётся в P
                elif char == 'b':
                    self.state = 'Q'  # Переход в Q (конечное состояние)
                else:
                    return False  # Неверный символ
            elif self.state == 'Q':
                if char == 'a':
                    self.state = 'R'  # Переход из Q в R
                elif char == 'b':
                    self.state = 'Q'  # Остаётся в Q
                else:
                    return False  # Неверный символ
            elif self.state == 'R':
                if char == 'a':
                    self.state = 'R'  # Остаётся в R
                elif char == 'b':
                    self.state = 'P'  # Переход в P
                else:
                    return False  # Неверный символ
        
        # Проверяем, закончили ли мы в конечном состоянии
        return self.state == 'Q'



class UnifiedL1L2:
    def __init__(self):
        # Начальное состояние
        self.state = 'P1'
        self.accept_states = {'Q1', 'Q2'}  # Конечные состояния

    def reset(self):
        self.state = 'P1'

    def process(self, string):
        self.reset()
        for char in string:
            if self.state == 'P1':  # Состояние L1 P
                if char == 'a':
                    self.state = 'Q1'  # Переход в конечное состояние L1 Q
                elif char == 'b':
                    self.state = 'P1'  # Остаемся в состоянии P1

            elif self.state == 'Q1':  # Состояние L1 Q
                if char == 'a':
                    self.state = 'P1'  # Переход в состояние P
                elif char == 'b':
                    self.state = 'Q1'  # Остаемся в состояний Q

            elif self.state == 'P2':  # Состояние L2 P
                if char == 'a':
                    self.state = 'P2'  # Остаемся в состоянии P2
                elif char == 'b':
                    self.state = 'Q2'  # Переход в состояние Q2

            elif self.state == 'Q2':  # Состояние L2 Q
                if char == 'a':
                    self.state = 'R2'  # Переход в состояние R
                elif char == 'b':
                    self.state = 'Q2'  # Остаемся в состоянии Q2

            elif self.state == 'R2':  # Состояние L2 R
                if char == 'a':
                    self.state = 'R2'  # Остаемся в состоянии R2
                elif char == 'b':
                    self.state = 'P2'  # Переход в состояние P

        return self.state in self.accept_states




# Пример использования
if __name__ == "__main__":
    num4 = L1()
    
    test_strings = ["ab", "ba", "aab", "bba", "aa", "ababbb", "bbbaaa"]
    
    for s in test_strings:
        if num4.process(s):
            print(f"Строка '{s}' принимает автомат.")
        else:
            print(f"Строка '{s}' не принимает автомат.")
    print('\n')
    num1 = Numer1()
    
    test_strings = ["ab", "abc", "aab", "b", "bb", "bcb", "ac", "cc"]
    
    for test in test_strings:
        result = num1.process(test)
        print(f"Строка '{test}' {'принимается' if result else 'не принимается'} конечным автоматом.")

    print ('\n')
    num4_2 = Num4_2()

    test_strings = ["b", "aaab", "bba", "ab", "aaaabbb"]

    for test in test_strings:
        result = num4_2.process(test)
        print(f"Строка '{test}' {'принимается' if result else 'не принимается'} конечным автоматом.")

    print('\n')

    numUF = UnifiedL1L2()

    # Тестируем строки
    test_strings = [
        "ab",    # Должно пройти через L1
        "aa",    # Не пройдет, но может быть в L2
        "bba",   # Должно пройти через L2
        "aaa",   # Не пройдет, но может оставаться в L2
        "babab"  # Должно пройти через обе L1 и L2
    ]

    for string in test_strings:
        result = numUF.process(string)
        print(f"Строка '{string}' приводит к результату: {result}")
