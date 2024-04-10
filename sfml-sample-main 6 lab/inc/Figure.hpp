#include <SFML/Graphics.hpp>
#include <string>

class Point {
	int x;
	int y;
public:
	Point() = default;
	Point(int a, int b) {
		x = a;
		y = b;
	}

	void SetX(int xx) {
		x = xx;
	}
	void SetY(int yy) {
		y = yy;
	}

	int GetX() {
		return x;
	}
	int GetY() {
		return y;
	}
};



class Figures {
	std::string color = "Пурпурный";
public:
	
	double LenghtVector(Point a, Point b)
	{
		return sqrt(pow((a.GetX() - b.GetX()), 2) + pow((a.GetY() - b.GetY()), 2));
	}
	
};