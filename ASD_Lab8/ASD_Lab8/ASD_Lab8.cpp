#include <iostream>
#include <vector>
#include <set>

using namespace std;

int countWaysToMakeChange(const set<int>& coins, int amount) {
    vector<int> dp(amount + 1, 0);
    dp[0] = 1; // Базовый случай: 1 способ разменять 0 рублей

    for (int coin : coins) {
        for (int i = coin; i <= amount; ++i) {
            dp[i] += dp[i - coin];
        }
    }

    return dp[amount];
}

int main() {
    setlocale(LC_ALL, "RU");
    int x;
    vector<int> coins;
    cout << "Какие у вас купюры? (номинал, кратный 50р) - 0, чтобы завершить набор" << endl;
    while (cin >> x)
    {
        if (x == 0) break;
        if (x % 50 != 0) cout << "Некорректная купюра! Попробуй еще раз: ";
        else coins.push_back(x);
        
    }

    set<int> coin_set(coins.begin(), coins.end());
    int amount; // Сумма для размена
    cout << "Какую сумму хотите разменять? (кратное 50р)" << endl;
    cin >> amount;

    cout << "Количество способов разменять " << amount
        << " рублей: " << countWaysToMakeChange(coin_set, amount) << endl;

    return 0;
}