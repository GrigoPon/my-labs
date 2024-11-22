#include <SFML/Graphics.hpp>
#include <cmath>
#include <iostream>
#include <list>

double Pi = acos(-1);
using namespace sf;
using namespace std;


vector<pair<int, int>> edges = {
        {1, 2},
        {1, 3},
        {1, 4},
        {2, 3},
        {3, 2},
        {3, 4},
        {4, 3},
        {4, 5},
        {5, 1}
};


void MatrSmezh()
{
    bool graph[5][5];
    for (int i = 0; i < 5; i++)
        for (int j = 0; j < 5; j++)
            graph[i][j] = 0;
    
    cout << "Матрица смежности графа: " << endl;
    cout << endl;
    for (auto edge : edges)
    {
        int u = edge.first - 1;
        int v = edge.second - 1;

        graph[u][v] = true;
    }
    cout << "   ";
    for (int i = 0; i < 5; i++)
        cout << i + 1 << " ";
    cout << endl;
    cout << "   ---------" << endl;

    for (int i = 0; i < 5; i++)
    {
        cout << i + 1 << ": ";
        for (int j = 0; j < 5; j++)
        {
            cout << graph[i][j] << " ";
        }
        cout << endl;
    }
}

void listSmezh() {
    vector<int> graph[5];

    for (auto edge : edges) {
        int u = edge.first - 1;
        int v = edge.second - 1; 
       
        graph[u].push_back(v);
   
    }

    cout << "------------------------" << endl;
    cout << "Список смежности графа: " << endl;
    
    for (int i = 0; i < 5; i++) {
        cout << i + 1 << ": ";
        for (int v : graph[i]) {
            cout << v + 1 << " ";
        }
        cout << endl;
    }

}

void simplelist() {

    cout << "------------------------------" << endl;
    cout << "Обычный список ребер и вершин:" << endl;

    for (auto edge : edges)
    {
        cout << "{" << edge.first << ", " << edge.second << "} ";
    }


}

void printed()
{
    RenderWindow window(VideoMode(1920, 1080), "Graph");

    const int numSides = 5;
    const float radius = 200.f;
    const float circleRadius = 40.f;


    ConvexShape pentagon;
    pentagon.setPointCount(numSides);

    CircleShape circles[numSides];
    Vector2f positions[numSides];
    Text numbers[numSides];

    Texture arrow;
    if (!arrow.loadFromFile("arroww.png"))
    {
        cerr << "Текстурка не загружена" << endl;
        exit(EXIT_FAILURE);
    }

    Font font;
    if (!font.loadFromFile("font.ttf"))
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
        circles[i].setOutlineThickness(5);
        circles[i].setPosition(x - circleRadius, y - circleRadius);

        //номера кружочков
        numbers[i].setFont(font);
        numbers[i].setString(to_string(i + 1));
        numbers[i].setCharacterSize(48);
        numbers[i].setFillColor(Color::Black);
        numbers[i].setPosition(x - circleRadius / 3, y - circleRadius / 1.5);
    }


    Vector2f start;
    Vector2f end;
    float length;

    // Основной цикл
    while (window.isOpen()) {
        Event event;
        while (window.pollEvent(event)) {
            if (event.type == Event::Closed) {
                window.close();
            }
        }


        window.clear(Color::White);

        //стрелочки
        for (auto edge : edges)
        {
            start = positions[edge.first - 1];
            end = positions[edge.second - 1];
            length = sqrt(pow(end.x - start.x, 2) + pow(end.y - start.y, 2));
            RectangleShape line(Vector2f(length - 40, 70.0f));
            line.setTexture(&arrow);
            line.setPosition(start);
            line.setOrigin(0, 35);
            line.setRotation(atan2(end.y - start.y, end.x - start.x) * 180 / Pi);
            window.draw(line);
        }



        // кружочки
        for (int i = 0; i < numSides; ++i) {
            window.draw(circles[i]);
            window.draw(numbers[i]);
        }

        window.display();
    }
}

int main() {
    setlocale(LC_ALL, "RU");
   

    printed();
    MatrSmezh();
    listSmezh();
    simplelist();

    return 0;
}