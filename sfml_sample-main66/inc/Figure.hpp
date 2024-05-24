#pragma once
#include <string>
using namespace std;

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
protected:
	std::string color;

public:

	Figures() = default;

	Figures(std::string ccolor)
	{
		color = ccolor;
	}
	virtual void Setup(Point Aa, Point Bb, Point Cc, Point Dd, string ccolor) = 0;
	//virtual void SetupS() = 0;
	virtual float Area() = 0;
	virtual string FigureInfo() = 0;
	virtual ~Figures() = default;

};