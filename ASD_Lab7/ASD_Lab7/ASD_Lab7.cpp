#include <iostream>
#include <vector>
#include <climits>

using namespace std;

void maxSubArray(vector<int>& nums) {
    int max_current = nums[0];
    int max_global = nums[0];
    int start = 0, end = 0;
    int temp_start = 0;

    for (int i = 1; i < nums.size(); ++i) {
        if (nums[i] > max_current + nums[i]) {
            max_current = nums[i];
            temp_start = i;
        }
        else {
            max_current += nums[i];
        }

        if (max_current > max_global) {
            max_global = max_current;
            start = temp_start;
            end = i;
        }
    }

    cout << "Максимальный подмассив: " << max_global << endl;
    cout << "Подмассив начинается с индекса: " << start << ", а заканчивается на индексе: " << end << endl;
}

int main() {
    setlocale(LC_ALL, "RU");

    int n;
    cout << "Введите размер массива: ";
    cin >> n;
    vector<int> nums(n);
    cout << "Введите элементы массива: " << endl;
    for (int i = 0; i < n; i++) {
        cin >> nums[i];
    }

    cout << "Ваш массив: " << endl;
    for (auto num : nums) {
        cout << num << " ";
    }
    maxSubArray(nums);
    return 0;
}