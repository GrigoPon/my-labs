#include <string>
#include <iostream>
#include "Figure.hpp"
#include "Oval.hpp"
#include "Square.hpp"

using namespace std;

void FigureInfo(Figures& fig)
{
	cout << fig.FigureInfo() << endl;
	//cout << fig.Area() << endl;
}


int main()
{
	setlocale(LC_ALL, "RU");
	int x, y, ox, oy;
	x = y = ox = oy = 0;
	int C = 2;
	int K = 1;
	Figures** figs = new Figures * [C];
	figs[0] = new Oval[K];
	figs[1] = new Square[K];
	string color = "Red";

	for (int i = 0; i < C; i++)
	{
		for (int j = 0; j < K; j++)
		{
			figs[i][j].Setup(Point(ox, oy), Point(x + 25, y), Point(x, y+25), Point(x + 25, y + 25), color);
			x += 15;
			y += 30;
			color = "Blue";
		}
	}

	for (int i = 0; i < C; i++)
	{
		for (int j = 0; j < K; j++)
		{
			FigureInfo(figs[i][j]);
		}
		
	}

	for (int i = 0; i < C; i++)
	{
		delete[] figs[i];
	}
	delete figs;

	return 0;
	
}