#include <iostream>
#include <vector>
#include <algorithm>
#include <climits>

using namespace std;

const int INF = INT_MAX;

int tsp(const vector<vector<int>>& dist) {
    int n = dist.size();
    if (n == 0) return 0;

    // dp[mask][i] — минимальная стоимость пути, который посещает все города в `mask` и заканчивается в `i`
    vector<vector<int>> dp(1 << n, vector<int>(n, INF));

    // Базовый случай: посещение только стартового города (обычно 0)
    dp[1][0] = 0;

    // Перебираем все возможные подмножества городов
    for (int mask = 1; mask < (1 << n); ++mask) {
        for (int i = 0; i < n; ++i) {
            // Если город `i` не посещён в `mask`, пропускаем
            if (!(mask & (1 << i))) continue;

            // Перебираем предыдущий город `j`
            for (int j = 0; j < n; ++j) {
                if (i == j || !(mask & (1 << j))) continue;
                int prev_mask = mask ^ (1 << i);
                if (dp[prev_mask][j] != INF) {
                    dp[mask][i] = min(dp[mask][i], dp[prev_mask][j] + dist[j][i]);
                }
            }
        }
    }

    // Находим минимальный цикл, возвращающийся в стартовый город (0)
    int final_mask = (1 << n) - 1;
    int min_cost = INF;
    for (int i = 1; i < n; ++i) {
        if (dp[final_mask][i] != INF && dist[i][0] != INF) {
            min_cost = min(min_cost, dp[final_mask][i] + dist[i][0]);
        }
    }

    return (min_cost == INF) ? -1 : min_cost;
}

int main() {
    // Пример матрицы расстояний (INF — если города не связаны)
    setlocale(LC_ALL, "RU");
    vector<vector<int>> dist = {
        {0, 10, 15, 20},
        {10, 0, 35, 25},
        {15, 35, 0, 30},
        {20, 25, 30, 0}
    };

    int min_path = tsp(dist);
    cout << "Минимальная длина маршрута коммивояжёра: " << min_path << endl;

    return 0;
}