#pragma once
#include "Figure.hpp"
#include <string>
using namespace std;

class Oval : public Figures
{
	Point O, A, B;

public:
	Oval()
	{
		Point O(0, 0);
		Point A(1, 0);
		Point B(0, 1);
		color = "white";
	};

	Oval(Point Aa, Point Bb, Point Cc, Point Dd, string ccolor) {
		Setup(Aa, Bb, Cc, Dd, ccolor);
	}
	void Setup(Point Aa, Point Bb, Point Cc, Point Dd, string ccolor)
	{
		A = Aa;
		B = Bb;
		O = Cc;
		color = ccolor;
	}

	float Area()
	{
		float oa = sqrt(pow((A.GetX() - O.GetX()), 2) + pow((A.GetY() - O.GetY()), 2));
		float ob = sqrt(pow((B.GetX() - O.GetX()), 2) + pow((B.GetY() - O.GetY()), 2));
		return acos(-1) * oa * ob;
	}

	string FigureInfo() override
	{
		string name = "Oval";
		string inf = "Type: " + name + ", Color: " + color + ", Coordinates: ";
		inf += "A = ( " + to_string(A.GetX()) + "; " + to_string(A.GetY()) + " ), B = ( " + to_string(B.GetX()) + "; " + to_string(B.GetY()) + " ), " + "O = ( " + to_string(O.GetX()) + "; " + to_string(O.GetY()) + " ), ";
		inf += "Площадь: " + to_string(Area());
		return inf;
	}

	~Oval() = default;


};

