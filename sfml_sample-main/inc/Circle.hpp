#pragma once
#include <SFML/Graphics.hpp>

namespace gp {
	class Circle {
		float g_x;
		float g_y;
		float g_r;
		float g_alfa;
		float g_v;
		int rgbr, rgbg, rgbb;
		sf::CircleShape circle;
	public:
		Circle() = default;

		Circle(float x, float y, float r, float v, float alfa, int c1, int c2, int c3) {
			Stup(x, y, r, v, alfa, c1, c2, c3);

		}
		void Stup(float x, float y, float r, float v, float alfa, int c1, int c2, int c3) {
			g_x = x;
			g_y = y;
			g_r = r;
			g_v = v;
			g_alfa = alfa;
			rgbr = c1;
			rgbg = c2;
			rgbb = c3;
			circle.setOrigin(g_r, g_r);
			circle.setRadius(g_r);
			circle.setPosition(g_x, g_y);
			circle.setFillColor(sf::Color(rgbr, rgbg, rgbb, 255));
		}

		float X() { return g_x; }
		float Y() { return g_y; }
		float R() { return g_r; }
		float V() { return g_v; }

		void touch() {
			g_v = 0;
		}
		void changeColor(float t) {
			rgbr = 10*t;
			rgbg += 10*t;
			rgbb += 10*t;
			circle.setFillColor(sf::Color(rgbr, rgbg, rgbb, 255));
		}

		
		void Move(float t) {
			
			float vx = g_v * cos(g_alfa);
			float vy = g_v * sin(g_alfa);
			g_x += vx * t;
			g_y += vy * t;
			//g_alfa += t;
			circle.setPosition(g_x, g_y);
		}

		sf::CircleShape Get() {
			return circle;
		}
	};






}