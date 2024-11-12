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
                    print ("Переход P->aQ")
                elif char == 'b':
                    self.state = 'P'  # Остаёмся в P
                    print ("Переход P->bP")
                else:
                    print ("Путь никуда не ведет")
                    return False  # Неверный символ
            elif self.state == 'Q':
                if char == 'a':
                    self.state = 'P'  # Переход из Q в P
                    print ("Переход Q->aP")
                elif char == 'b':
                    self.state = 'Q'  # Остаёмся в Q
                    print ("Переход Q->bQ")
                else:
                    print ("Путь никуда не ведет")
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
                    print ("Переход S->aA")
                elif char == 'b':
                    self.state = 'F'  # Переход из S в F (конечное состояние)
                    print ("Переход S->bF")
                else:
                    return False  # Неверный символ
            elif self.state == 'A':
                if char == 'b':
                    self.state = 'B'  # Переход из A в B
                    print ("Переход A->bB")
                elif char == 'a':
                    self.state = 'C'  # Переход из A в C
                    print ("Переход A->aC")
                else:
                    print ("Путь никуда не ведет")
                    return False  # Неверный символ
            elif self.state == 'B':
                if char == 'b':
                    self.state = 'B'  # Остаётся в B
                    print ("Переход B->bB")
                elif char == 'c':
                    self.state = 'A'  # Переход из B в A
                    print ("Переход B->cA")
                else:
                    print ("Путь никуда не ведет")
                    return False  # Неверный символ
            elif self.state == 'C':
                if char == 'c':
                    self.state = 'A'  # Переход из C в A
                    print ("Переход C->cA")
                elif char == 'b':
                    self.state = 'F'  # Переход из C в F (конечное состояние)
                    print ("Переход C->bF")
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
                    print ("Переход P->aP")
                elif char == 'b':
                    self.state = 'Q'  # Переход в Q (конечное состояние)
                    print ("Переход P->bQ")
                else:
                    return False  # Неверный символ
            elif self.state == 'Q':
                if char == 'a':
                    self.state = 'R'  # Переход из Q в R
                    print ("Переход Q->aR")
                elif char == 'b':
                    self.state = 'Q'  # Остаётся в Q
                    print ("Переход Q->bQ")
                else:
                    return False  # Неверный символ
            elif self.state == 'R':
                if char == 'a':
                    self.state = 'R'  # Остаётся в R
                    print ("Переход R->aR")
                elif char == 'b':
                    self.state = 'P'  # Переход в P
                    print ("Переход R->bP")
                else:
                    return False  # Неверный символ
        
        # Проверяем, закончили ли мы в конечном состоянии
        return self.state == 'Q'

class CombinedL1L2:
    def __init__(self):
        self.state = 'P1'  # Начальное состояние
        self.accept_states = {'Q2'}  # Конечное состояние

    def reset(self):
        self.state = 'P1'  # Сбрасываем состояние в начальное

    def process(self, input_string):
        for symbol in input_string:
            if self.state == 'P1':
                if symbol == 'a':
                    self.state = 'Q1'
                    print("Переход P1->aQ1")
                elif symbol == 'b':
                    self.state = 'P1'
                    print("Переход P1->bP1")
            elif self.state == 'Q1':
                if symbol == 'a':
                    self.state = 'P1'
                    print("Переход Q1->aP1")
                elif symbol == 'b':
                    self.state = 'P2'
                    print("Переход Q1->bP2")
            elif self.state == 'Q2':
                if symbol == 'a':
                    self.state = 'R'
                    print("Переход Q2->aR")
                elif symbol == 'b':
                    self.state = 'Q2'
            elif self.state == 'R':
                if symbol == 'a':
                    self.state = 'R'
                    print("Переход R->aR")
                elif symbol == 'b':
                    self.state = 'P2'
                    print("Переход R->bP2")
            if self.state == 'P2':
                if symbol == 'a':
                    self.state = 'P2'
                    print("Переход P2->aP2")
                elif symbol == 'b':
                    self.state = 'Q2'
                    print("Переход P2->bQ2")

    def is_accepted(self):
        return self.state in self.accept_states

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
                    print ("Переход P1->aQ1")
                elif char == 'b':
                    self.state = 'P1'  # Остаемся в состоянии P1
                    print ("Переход P1->bP1")

            elif self.state == 'Q1':  # Состояние L1 Q
                if char == 'a':
                    self.state = 'P1'  # Переход в состояние P
                    print ("Переход Q1->aP1")
                elif char == 'b':
                    self.state = 'Q1'  # Остаемся в состояний Q
                    print ("Переход Q1->bQ1")

            elif self.state == 'P2':  # Состояние L2 P
                if char == 'a':
                    self.state = 'P2'  # Остаемся в состоянии P2
                    print ("Переход P2->aP2")
                elif char == 'b':
                    self.state = 'Q2'  # Переход в состояние Q2
                    print ("Переход P2->bQ2")

            elif self.state == 'Q2':  # Состояние L2 Q
                if char == 'a':
                    self.state = 'R2'  # Переход в состояние R
                    print ("Переход Q2->aR2")
                elif char == 'b':
                    self.state = 'Q2'  # Остаемся в состоянии Q2
                    print ("Переход Q2->bQ2")

            elif self.state == 'R2':  # Состояние L2 R
                if char == 'a':
                    self.state = 'R2'  # Остаемся в состоянии R2
                    print ("Переход R2->aR2")
                elif char == 'b':
                    self.state = 'P2'  # Переход в состояние P
                    print ("Переход R2->bP2")

        return self.state in self.accept_states




# Пример использования
if __name__ == "__main__":
    num4 = L1()
    
    test_strings = ["ab", "ba", "aab", "bba", "aa", "ababbb", "bbbaaa"]
    print ("АВТОМАТ НОМЕР 1")
    for s in test_strings:
        if num4.process(s):
            print(f"Автомат принимает строку '{s}'.")
            print ('\n')
        else:
            print(f"Автомат не принимает строку '{s}'.")
            print ('\n')
    print('\n')
    num1 = Numer1()
    
    test_strings = ["ab", "abc", "aab", "b", "bb", "bcb", "ac", "cc"]
    print ("АВТОМАТ НОМЕР 2")
    for test in test_strings:
        result = num1.process(test)
        print(f"Автомат {'принимает строку' if result else 'не принимает строку'} '{test}'")
        print ('\n')

    print ('\n')
    num4_2 = Num4_2()

    test_strings = ["b", "aaab", "bba", "ab", "aaaabbb"]
    print ("АВТОМАТ НОМЕР 3")
    for test in test_strings:
        result = num4_2.process(test)
        print(f"Автомат {'принимает строку' if result else 'не принимает строку'} '{test}' ")
        print ('\n')

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
    print ("АВТОМАТ НОМЕР 4")
    for string in test_strings:
        result = numUF.process(string)
        print(f"Автомат {'принимает строку' if result else 'не принимает строку'} '{string}' ")
        print ('\n')


    num5_2 = CombinedL1L2()

    strings_to_test = ['abbaabb', 'b', 'aab', 'abb', 'ba', 'bb']
    for s in strings_to_test:
        num5_2.reset()
        num5_2.process(s)
        result = "Автомат принимает строку" if num5_2.is_accepted() else "Автомат НЕ принимает строку"
        print(f"Строка '{s}': {result}")

        
