#include <iostream>
#include <vector>
#include <list>
#include <string>
#include <fstream>
#include <locale>
#include <codecvt>

using namespace std;

class HashTable {
private:
    int size;  // Размер хеш-таблицы
    vector<list<wstring>> table;  // Вектор списков для хранения элементов

    // Простая хеш-функция
    int hashFunction(const wstring& key) {
        int hash = 0;
        for (char c : key) {
            hash += (int)(c);  // Сумма ASCII-кодов символов
        }
        return hash % size;  // Возвращаем индекс в пределах размера таблицы
    }

public:
    // Конструктор
    HashTable(int s) : size(s), table(s) {}

    // Метод вставки
    void insert(const wstring& key) {
        int index = hashFunction(key);
        // Проверка, существует ли уже ключ
        for (const wstring& item : table[index]) {
            if (item == key) {
                return;  // Ключ уже существует
            }
        }
        table[index].push_back(key);  // Добавление ключа в корзину
    }

    // Метод для отображения хеш-таблицы
    void display() {
        for (int i = 0; i < size; i++) {
            if (!table[i].empty()) {
                wcout << "Index " << i << ": ";
                for (const wstring& item : table[i]) {
                    wcout << item << " -> ";
                }
                wcout << "[end]" << endl;  // Конец списка
                
            }
        }
    }
};

// Функция для создания хеш-таблицы из файла
void createHashTableFromFile(const string& filename) {
    setlocale(LC_ALL, "RU");
    HashTable hashTable(10);  // Укажите необходимый размер таблицы
    wifstream file(filename);
    file.imbue(locale(file.getloc(), new codecvt_utf8<wchar_t>));

    if (!file.is_open()) {
        wcerr << L"Не удалось открыть файл!" << endl;
        return;
    }

    wstring word;
    while (file >> word) {
        hashTable.insert(word);
    }

    file.close();
    hashTable.display();  // Вывод хеш-таблицы
}

int main() {
    locale::global(locale(""));
    createHashTableFromFile("Assets\\input.txt");  // Укажите имя вашего файла
    return 0;
}