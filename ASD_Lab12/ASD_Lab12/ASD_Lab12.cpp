#include <iostream>
#include <vector>
#include <algorithm>

using namespace std;

int knapsackOptimized(int W, const vector<int>& wt, const vector<int>& val, int n) {
    vector<int> dp(W + 1, 0);

    for (int i = 0; i < n; i++) {
        for (int w = W; w >= wt[i]; w--) {
            dp[w] = max(dp[w], val[i] + dp[w - wt[i]]);
        }
    }

    return dp[W];
}

int main() {
    setlocale(LC_ALL, "RU");
    cout << "Какая вместимость (кг) вашего рюкзака?: ";
    int W;
    cin >> W;

    cout << "Вещи какого веса вы хотите взять? (введите вес вещей) введите -1 чтобы закончить набор: ";
    int ob;
    vector<int> wt;
    while (cin >> ob)
    {
        if (ob == -1) break;
        else if (ob == 0) {
            cout << "Предмет не может не иметь веса! Попробуйте еще раз: ";
            continue;
        }
        wt.push_back(ob);
    }

    int v;
    cout << "Какова полезность ваших вещей (стоимость) соответственно? (-1 для завершения): ";
    vector<int> val;
    while (cin >> v)
    {
        if (v == -1) break;
        val.push_back(v);
    }

    
    
    
    int n = val.size();

    cout << "Максимальная стоимость: " << knapsackOptimized(W, wt, val, n) << endl;

    return 0;
}