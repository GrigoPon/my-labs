#include <SFML/Graphics.hpp>
#include <cmath>
#include <iostream>
#include <string>
#include <fstream>
#include <locale>
#include <codecvt>

double Pi = acos(-1);
using namespace sf;
using namespace std;

wstring CheckPoints[10] = { L"Калининград", L"Гурьевск", L"Гвардейск", L"Зеленоградск", L"Пионерский", L"Светлогорск", L"Колосовка", L"Янтарный", L"Приморск", L"Балтийск" };

vector<vector<int>> edges = {
        {1, 2, 13}, {1, 3, 53}, {1, 4, 33},
        {1, 5, 44}, {1, 7, 24}, {1, 9, 40},
        {1, 10, 45}, {2, 3, 37}, {2, 4, 25},
        {4, 5, 24}, {5, 6, 5},
        {6, 7, 27}, {6, 8, 24}, {7, 9, 24},
        {8, 9, 20}, {9, 10, 14}
};


void output_to_file(vector<vector<int>> M, vector<int> route)
{
    wofstream out("Route.txt");
    out.imbue(locale(out.getloc(), new codecvt_utf8<wchar_t>));
    if (!out.is_open())
    {
        wcerr << L"Ошибка создания файла!!!" << endl;
        exit(EXIT_FAILURE);
    }
    out << "Список вершин: " << endl;
    for (int i = 0; i < route.size(); i++)
        out << CheckPoints[route[i]] << " ";
    out << endl;
    out << "Список ребер:" << endl;

    for (int i = 0; i < route.size() - 1; i++)
    {
        out << "{" << route[i] + 1 << ", " << route[i + 1] + 1 << ", " << M[route[i]][route[i + 1]] << "}" << endl;
    }

    out.close();
}

vector<vector<int>> Matr()
{
    int vertex = 10;
    vector<vector<int>> Matr(vertex, vector<int>(vertex, 0));

    for (const auto& edge : edges) {
        int u = edge[0] - 1;
        int v = edge[1] - 1;
        int weight = edge[2];

        Matr[u][v] = weight;
        Matr[v][u] = weight;
    }

    return Matr;
}



vector<int> dijkstra(const vector<vector<int>>& Matr, int start, int end) {
    
    const int INF = 1e9;
    int n = 10;
    vector<int> distance(n, INF);
    vector<int> previous(n, -1);
    vector<bool> visited(n, false);

    // Начальная точка
    distance[start] = 0;


    for (int i = 0; i < n; ++i) {
        // Находим текущую вершину с минимальным расстоянием
        int v = -1;
        for (int j = 0; j < n; ++j) {
            if (!visited[j] && (v == -1 || distance[j] < distance[v])) {
                v = j;
            }
        }

        if (distance[v] == INF)
            break;

        visited[v] = true; // Если вершина v выбрана, она помечается как посещённая

        // Обновление расстояния до соседних вершин
        for (int u = 0; u < n; ++u) {
            if (Matr[v][u] > 0 && distance[v] + Matr[v][u] < distance[u]) {
                distance[u] = distance[v] + Matr[v][u]; // Присваиваем меньшее расстояние (вес вершины)
                previous[u] = v;
            }
        }
    }

    // Восстановление пути
    vector<int> Way;
    for (int at = end; at != -1; at = previous[at]) {
        Way.push_back(at);
    }
    reverse(Way.begin(), Way.end()); // Переворачиваем массив


    if (Way.size() == 1 && Way[0] != start) {
        return {};
    }

    return Way;
}


vector<int> ABC(const vector<vector<int>>& Matr, int start, int via, int end) {
    vector<vector<int>> Temp = Matr;

    
    for (size_t i = 0; i < Matr.size(); ++i) {
        Temp[i][end] = 0;
        Temp[end][i] = 0;
    }

    vector<int> AC = dijkstra(Temp, start, via);


    if (AC.empty()) {
        return {};
    }


    Temp = Matr;

 
    vector<int> CB = dijkstra(Temp, via, end);


    if (CB.empty()) {
        return {};
    }

    if (!AC.empty() && !CB.empty()) {
        CB.erase(CB.begin());
    }

 
    AC.insert(AC.end(), CB.begin(), CB.end());

    return AC;
}

void printed()
{
    RenderWindow window(VideoMode(1920, 1080), L"Окно");

    setlocale(LC_ALL, "RU");
    const int numSides = 10;
    const float radius = 400.f;
    const float circleRadius = 40.f;


    ConvexShape pentagon;
    pentagon.setPointCount(numSides);

    CircleShape circles[numSides];
    Vector2f positions[numSides];


    Text numbers[numSides];

    Font font;
    if (!font.loadFromFile("calibrib.ttf"))
    {
        cerr << "Шрифт не загружен" << endl;
        exit(EXIT_FAILURE);
    }

    for (int i = 0; i < numSides; ++i) {
        float angle = (2 * Pi / numSides) * i - 1.55;
        //правильный n-угольник (пятиугольник)
        float x = 950 + radius * cos(angle);
        float y = 500 + radius * sin(angle);
        pentagon.setPoint(i, Vector2f(x, y));
        positions[i] = Vector2f(x, y);

        //кружочки
        circles[i].setRadius(circleRadius);
        circles[i].setOutlineColor(Color::Black);
        circles[i].setFillColor(Color::Red);
        circles[i].setOutlineThickness(5);
        circles[i].setPosition(x - circleRadius, y - circleRadius);

        //номера кружочков
        numbers[i].setFont(font);
        numbers[i].setString(CheckPoints[i]);
        numbers[i].setCharacterSize(28);
        numbers[i].setFillColor(Color::Black);
        numbers[i].setPosition(x - circleRadius - 40, y - circleRadius + 80);
    }

    vector<vector<int>> M;
    M = Matr();

    Vector2f start;
    Vector2f end;
    float length;
    Text weight[16];
    int startP = 1; int endP = 0; int viaP = 6;
    vector<int> route = ABC(M, startP, viaP, endP);


    bool Space_Pressed = false;
    bool console_work = true;
    // Основной цикл
    while (window.isOpen()) {
        Event event;
        while (window.pollEvent(event)) {
            if (event.type == Event::Closed)
                window.close();

            
            if (event.type == Event::KeyPressed && event.key.code == Keyboard::Space) {
                Space_Pressed = !Space_Pressed;
            }
        }
        window.clear(Color::White);

        for (int i = 0; i < numSides; i++)
            circles[i].setFillColor(Color::Red);

        for (auto edge : edges) {
            start = positions[edge[0] - 1];
            end = positions[edge[1] - 1];
            float length = sqrt(pow(end.x - start.x, 2) + pow(end.y - start.y, 2));
            float center_x = (start.x + end.x) / 2;
            float center_y = (start.y + end.y) / 2;
            
            
            RectangleShape line(Vector2f(length - 20, 5.0f));
            line.setFillColor(Color(128, 128, 128));

            
            if (Space_Pressed)
            {
                output_to_file(M, route);
                bool isInRoute = false;
                for (size_t i = 0; i < route.size() - 1; i++) {
                    if ((route[i] == edge[0] - 1 && route[i+1] == edge[1] - 1) ||
                        (route[i] == edge[1] - 1 && route[i+1] == edge[0] - 1)) {
                        isInRoute = true;
                        break;
                    }


                }

                if (isInRoute) {
                    line.setFillColor(Color(192, 255, 0)); // Закрашиваем в зеленый, если это часть маршрута
                }

                for (int i = 0; i < numSides; i++)
                {
                    if (i == startP) {
                        circles[i].setFillColor(sf::Color::Green); // Начальная вершина
                    }
                    else if (i == viaP) {
                        circles[i].setFillColor(sf::Color::Yellow); //Особая
                    }
                    else if (i == endP) {
                        circles[i].setFillColor(sf::Color::Black); // Конечная вершина
                    }
                    else if (find(route.begin(), route.end(), i) != route.end()) {
                        circles[i].setFillColor(sf::Color::Blue); // Промежуточные вершины
                    }
                    
                }



            }

            line.setPosition(start.x, start.y);
            line.setOrigin(0, 2.5);
            line.setRotation(atan2(end.y - start.y, end.x - start.x) * 180 / 3.14159);

            // Вес
            weight[edge[0]].setFont(font);
            weight[edge[0]].setString(to_string(edge[2]));
            weight[edge[0]].setCharacterSize(20);
            weight[edge[0]].setFillColor(Color::Black);
            weight[edge[0]].setPosition(center_x - 30, center_y + 5);

            window.draw(weight[edge[0]]);
            window.draw(line);
        }




            for (int i = 0; i < numSides; ++i) {
                window.draw(circles[i]);
                window.draw(numbers[i]);
                //window.draw(weight[i]);
            }

            window.display();
            
            if (console_work)
            {
                vector<vector<int>> M;
                M = Matr();
                int start = 4;
                int end = 3;
                int via = 1;
                cout << "Матрица смежности: " << endl;
                int n = 10;
                cout << "\t\t";
                for (int i = 0; i < n; i++)

                    wcout << CheckPoints[i] << " ";
                cout << endl;
                cout << "   ------------------------------------------------------------------------------------------------" << endl;

                for (int i = 0; i < M.size(); i++)
                {
                    wcout << CheckPoints[i] << ":\t";
                    for (int j = 0; j < M.size(); j++)
                    {
                        cout << M[i][j] << "\t";
                    }
                    cout << endl;
                }

                cout << endl;
                cout << "Список ребер: " << endl;
                for (auto edge : edges) {
                    cout << "{" << edge[0] << ", " << edge[1] << ", " << edge[2] << "}";
                    cout << endl;
                }
            }
            console_work = false;
        }
    }
int main() {
    setlocale(LC_ALL, "RU");
    locale::global(locale(""));


    printed();


    return 0;
}