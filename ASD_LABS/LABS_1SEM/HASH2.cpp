#include <iostream>
#include <fstream>
#include <vector>
#include <string>
#include <locale>
#include <codecvt>

using namespace std;

class HashTable {
private:
    vector<string> table; // Хранилище для хэш-таблицы
    int size;  // Размер хэш-таблицы
    int count;

    int hashFunction(const string& key) const {
        int hash = 0;
        for (char ch : key) {
            hash += ch;
        }
        return hash % size;
    }

    void rehash() {
        vector<string> oldTable = table;
        size *= 2;
        table.resize(size);
        count = 0;

        /*for (const auto& key : oldTable) {
            if (!key.empty()) {
                add(key);
            }
        }*/
    }

public:
    HashTable(int s) : size(s), count(1) {
        table.resize(size);
    }

    void add(const string& key) {

        if (count > size) {
            rehash();
        }
        int index = hashFunction(key);
        while (!table[index].empty()) {
            index = (index + 1) % size;
        }
        table[index] = key;
    }

    void loadFromFile(const string& filename) {
        ifstream file(filename);
        string word;
        if (!file.is_open()) {
            cerr << "Не удалось открыть файл: " << filename << endl;
            return;
        }
        while (file >> word) {
            add(word);
            count++;
        }
        file.close();
    }

    void display() const {
        for (int i = 0; i < size; ++i) {
            if (!table[i].empty()) {
                cout << i + 1 << ": [" << table[i] << "]" << endl;
            }
            else {
                cout << i + 1 << ": " << "[ пусто ]" << endl;
            }
        }
    }

    
};

int main() {

    setlocale(LC_ALL, "RU");

    HashTable ht(10);

    ht.loadFromFile("Assets\\DATA.txt");

    ht.display();

    return 0;
}