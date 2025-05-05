#include <iostream>
#include <string>
#include <vector>

using namespace std;

const int PRIME = 101; // Простое число для хеширования
const int ALPHABET_SIZE = 256; // Размер алфавита (ASCII)

// Функция для вычисления хеша строки
long calculateHash(const string& str, int length) {
    long hash = 0;
    for (int i = 0; i < length; ++i) {
        hash = (ALPHABET_SIZE * hash + str[i]) % PRIME;
    }
    return hash;
}

// Функция для пересчета хеша при сдвиге окна
long recalculateHash(long oldHash, char oldChar, char newChar, int patternLength) {
    // Предварительно вычисляем ALPHABET_SIZE^(patternLength-1) % PRIME
    long power = 1;
    for (int i = 0; i < patternLength - 1; ++i) {
        power = (power * ALPHABET_SIZE) % PRIME;
    }

    long newHash = (oldHash - oldChar * power) % PRIME;
    newHash = (newHash * ALPHABET_SIZE + newChar) % PRIME;
    return (newHash < 0) ? (newHash + PRIME) : newHash;
}

// Функция поиска по алгоритму Рабина-Карпа
vector<int> rabinKarpSearch(const string& text, const string& pattern) {
    vector<int> matches;
    int textLength = text.length();
    int patternLength = pattern.length();

    if (patternLength == 0 || textLength < patternLength) {
        return matches;
    }

    // Вычисляем хеш для шаблона и первого окна текста
    long patternHash = calculateHash(pattern, patternLength);
    long textHash = calculateHash(text, patternLength);

    // Проходим по тексту
    for (int i = 0; i <= textLength - patternLength; ++i) {
        // Если хеши совпадают, проверяем символы по одному
        if (patternHash == textHash) {
            bool match = true;
            for (int j = 0; j < patternLength; ++j) {
                if (text[i + j] != pattern[j]) {
                    match = false;
                    break;
                }
            }
            if (match) {
                matches.push_back(i);
            }
        }

        // Вычисляем хеш для следующего окна
        if (i < textLength - patternLength) {
            textHash = recalculateHash(textHash, text[i], text[i + patternLength], patternLength);
        }
    }

    return matches;
}

int main() {
    string text, pattern;

    cout << "Введите текст: ";
    getline(cin, text);

    cout << "Введите шаблон для поиска: ";
    getline(cin, pattern);

    vector<int> matches = rabinKarpSearch(text, pattern);

    if (matches.empty()) {
        cout << "Шаблон не найден в тексте." << endl;
    }
    else {
        cout << "Шаблон найден на позициях: ";
        for (int pos : matches) {
            cout << pos << " ";
        }
        cout << endl;
    }

    return 0;
}