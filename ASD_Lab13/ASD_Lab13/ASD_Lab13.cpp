#include <iostream>
#include <vector>
#include <algorithm>

using namespace std;

int firstFit(vector<double> items, double binCapacity) {
    vector<double> bins;

    for (double item : items) {
        bool placed = false;
        // Пытаемся поместить в существующий ящик
        for (double& bin : bins) {
            if (bin + item <= binCapacity) {
                bin += item;
                placed = true;
                break;
            }
        }
        // Если не поместился - создаем новый ящик
        if (!placed) {
            bins.push_back(item);
        }
    }

    return bins.size();
}

int firstFitDecreasing(vector<double> items, double binCapacity) {
    // Сортируем предметы по убыванию
    sort(items.begin(), items.end(), greater<double>());

    return firstFit(items, binCapacity);
}

int main() {
    setlocale(LC_ALL, "RU");
    vector<double> items = { 0.5, 0.7, 0.3, 0.9, 0.6, 0.8, 0.1, 0.4, 0.2, 0.5 };
    double binCapacity = 1.0;

    cout << "Количество использованных ящиков: " << firstFitDecreasing(items, binCapacity) << endl;

    return 0;
}
