#include <iostream>
#include <list>
#include <vector>
#include <algorithm>

using namespace std;

class Graph {
    int V; // Количество вершин
    list<int>* adj; // Список смежности

public:
    Graph(int V) {
        this->V = V;
        adj = new list<int>[V];
    }

    ~Graph() {
        delete[] adj;
    }

    // Добавление ребра в граф
    void addEdge(int v, int w) {
        adj[v].push_back(w);
        adj[w].push_back(v); // Граф неориентированный
    }

    // Жадный алгоритм раскраски графа
    void greedyColoring() {
        vector<int> result(V, -1); // Инициализация цветов (-1 - нет цвета)
        vector<bool> available(V, false); // Доступные цвета

        // Первая вершина получает первый цвет
        result[0] = 0;

        // Назначение цветов оставшимся V-1 вершинам
        for (int u = 1; u < V; u++) {
            // Помечаем цвета всех смежных вершин как недоступные
            for (auto i = adj[u].begin(); i != adj[u].end(); ++i) {
                if (result[*i] != -1) {
                    available[result[*i]] = true;
                }
            }

            // Находим первый доступный цвет
            int cr;
            for (cr = 0; cr < V; cr++) {
                if (!available[cr]) {
                    break;
                }
            }

            result[u] = cr; // Назначаем найденный цвет

            // Сбрасываем значения обратно для следующей итерации
            for (auto i = adj[u].begin(); i != adj[u].end(); ++i) {
                if (result[*i] != -1) {
                    available[result[*i]] = false;
                }
            }
        }

        // Выводим результат
        for (int u = 0; u < V; u++) {
            cout << "Вершина " << u << " ---> Цвет " << result[u] << endl;
        }

        // Находим количество использованных цветов
        int max_color = *max_element(result.begin(), result.end()) + 1;
        cout << "Всего использовано цветов: " << max_color << endl;
    }
};

int main() {
    setlocale(LC_ALL, "RU");

    int V;
    cout << "Введите количество вершин в вашем графе: ";
    cin >> V;

    Graph graph(V);
    int a;
    int b;
    cout << "Теперь введите ваш список ребер в формате (a b). Введите (-1 -1) для завершения заполнения: " << endl;

    while(cin >> a >> b)
    {
        if (a == -1 || b == -1) break;
        else if (a >= V || b >= V) cout << "Вы ввели вершину, которой не существует, попробуйте снова!" << endl;
        else graph.addEdge(a, b);
    }
    

    cout << "Раскраска графа:\n";
    graph.greedyColoring();

    return 0;
}