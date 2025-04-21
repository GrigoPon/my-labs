#include <iostream>
#include <vector>
#include <climits>

using namespace std;

int eggDrop(int floors, int eggs) {
    // Создаём динамическую таблицу dp[eggs+1][floors+1]
    vector<vector<int>> dp(eggs + 1, vector<int>(floors + 1, 0));

    // Базовые случаи:
    // 1. Если этажей 0 или 1
    for (int e = 1; e <= eggs; e++) {
        dp[e][0] = 0;  // 0 этажей — 0 попыток
        dp[e][1] = 1;  // 1 этаж — 1 попытка
    }
    // 2. Если яйцо одно — проверяем все этажи подряд
    for (int f = 1; f <= floors; f++) {
        dp[1][f] = f;
    }

    // Заполняем dp для 2+ яиц и 2+ этажей
    for (int e = 2; e <= eggs; e++) {
        for (int f = 2; f <= floors; f++) {
            dp[e][f] = INT_MAX;
            // Перебираем все возможные этажи для первого броска
            for (int k = 1; k <= f; k++) {
                int attempts = 1 + max(
                    dp[e - 1][k - 1], // Яйцо разбилось — проверяем этажи ниже
                    dp[e][f - k]      // Яйцо целое — проверяем этажи выше
                );
                if (attempts < dp[e][f]) {
                    dp[e][f] = attempts;
                }
            }
        }
    }

    return dp[eggs][floors];
}

int main() {
    setlocale(LC_ALL, "RU");
    int floors = 100;
    int eggs = 2;
    cout << "Минимальное число попыток: " << eggDrop(floors, eggs) << endl;
    return 0;
}