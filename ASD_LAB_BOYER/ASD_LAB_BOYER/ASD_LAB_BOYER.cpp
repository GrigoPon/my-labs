#include <iostream>
#include <vector>
#include <string>
#include <algorithm>
#include <unordered_map>

using namespace std;

unordered_map<char, int> buildBadCharTable(const string& pattern) {
    unordered_map<char, int> badCharTable;
    int patternLength = pattern.length();

    for (int i = 0; i < patternLength; ++i) {
        // Сохраняем самое правое вхождение каждого символа
        badCharTable[pattern[i]] = i;
    }

    return badCharTable;
}

vector<int> buildGoodSuffixTable(const string& pattern) {
    int m = pattern.length();
    vector<int> goodSuffixTable(m, 0);
    vector<int> borderPos(m + 1, 0);

    // Этап 1: стандартный препроцессинг
    int i = m, j = m + 1;
    borderPos[i] = j;

    while (i > 0) {
        while (j <= m && pattern[i - 1] != pattern[j - 1]) {
            if (goodSuffixTable[j - 1] == 0) {
                goodSuffixTable[j - 1] = j - i;
            }
            j = borderPos[j];
        }
        i--; j--;
        borderPos[i] = j;
    }

    // Этап 2: дополнительный препроцессинг
    j = borderPos[0];
    for (i = 0; i < m; i++) {  // Изменено i <= m на i < m
        if (goodSuffixTable[i] == 0) {
            goodSuffixTable[i] = j;
        }
        if (i == j && j < m) {  // Добавлена проверка j < m
            j = borderPos[j];
        }
    }

    return goodSuffixTable;
}

vector<int> boyerMooreSearch(const string& text, const string& pattern) {
    vector<int> matches;
    int n = text.length();
    int m = pattern.length();

    if (m == 0 || n < m) {
        return matches;
    }

    unordered_map<char, int> badCharTable = buildBadCharTable(pattern);
    vector<int> goodSuffixTable = buildGoodSuffixTable(pattern);

    int shift = 0;
    while (shift <= n - m) {
        int j = m - 1;

        while (j >= 0 && pattern[j] == text[shift + j]) {
            j--;
        }

        if (j < 0) {
            matches.push_back(shift);
            shift += (m > 1) ? goodSuffixTable[1] : 1;
        }
        else {
            int badCharShift = (badCharTable.count(text[shift + j])) ?
                max(1, j - badCharTable[text[shift + j]]) :
                max(1, j + 1);

            int goodSuffixShift = (j < m - 1) ? goodSuffixTable[j + 1] : 1;

            shift += max(badCharShift, goodSuffixShift);
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

    vector<int> matches = boyerMooreSearch(text, pattern);

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