class CFGToChomsky:
    def __init__(self):
        self.rules = {
            'S': [['0S1'], ['A']],
            'A': [['0A1'], ['0.1']]
        }
        self.terminals = ['0', '1', '.']
        self.new_rules = {}

    def convert_to_chomsky(self):
        # Создаем новые нетерминалы для терминалов
        self.new_rules = {
            'S': [['X', 'D'], ['B', 'D']],
            'A': [['E', 'D']],
            'X': [['C', 'S']],
            'B': [['C', 'F']],
            'E': [['C', 'A']],
        }
        # Добавляем преобразующие правила
        self.add_terminal_rules()

    def add_terminal_rules(self):
        terminal_rules = {
            'C': ['0'],
            'D': ['1'],
            'F': ['.']
        }
        for nt, rules in terminal_rules.items():
            self.new_rules[nt] = [rules]

    def print_chomsky(self):
        for nt, productions in self.new_rules.items():
            for production in productions:
                print(f"{nt} -> {' '.join(production)}")

converter = CFGToChomsky()
converter.convert_to_chomsky()
converter.print_chomsky()
