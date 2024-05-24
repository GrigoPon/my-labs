#pragma once
#include "Figure.hpp"
using namespace std;

class Square : public Figures
{
	Point A, B, C, D;
	//float ab, ad, bc, cd;
public:

	Square()
	{
		Point A(0, 0);
		Point B(0, 1);
		Point C(1, 1);
		Point D(1, 0);

	}

	Square(Point Aa, Point Bb, Point Cc, Point Dd, string ccolor)
	{
		Setup(Aa, Bb, Cc, Dd, ccolor);
	}
	void Setup(Point Aa, Point Bb, Point Cc, Point Dd, string ccolor)
	{
		A = Aa;
		B = Bb;
		C = Cc;
		D = Dd;
		color = ccolor;
	}
	float Area()
	{
		float AB = sqrt(pow((B.GetX() - A.GetX()), 2) + pow((B.GetY() - A.GetY()), 2));
		//float BC = sqrt(pow((C.GetX() - B.GetX()), 2) + pow((C.GetY() - B.GetY()), 2));
		return pow(AB,2);
	}

	string FigureInfo() override
	{
		string name = "Square";
		string inf = "Type: " + name + ", Color: " + color + ", Coordinates: A = ( ";
		inf += to_string(A.GetX()) + "; " + to_string(A.GetY()) + " ), B = ( " + to_string(B.GetX()) + "; " + to_string(B.GetY()) + " ), C = ( ";
		inf += to_string(C.GetX()) + "; " + to_string(C.GetY()) + " ), D = ( " + to_string(D.GetX()) + "; " + to_string(D.GetY()) + " ), Square: " + to_string(Area());
		return inf;
	}

	~Square() = default;

};