#include <SFML/Graphics.hpp>
#include <Figure.hpp>



class Oval : public Figures
{
	Point O, A, B;
	double oa, ob;
	sf::CircleShape Ellipse;

public:
	Oval() = default;

	Oval(Point oo, Point aa, Point bb) {
		O = oo;
		A = aa;
		B = bb;
		oa = LenghtVector(O, A);
		ob = LenghtVector(O, B);
	}



};