#pragma once
#include <SFML/Graphics.hpp>
#include <iostream>


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
		//void changeColor(float t) {
		//	rgbr = 10*t;
		//	rgbg += 10*t;
		//	rgbb += 10*t;
		//	circle.setFillColor(sf::Color(rgbr, rgbg, rgbb, 255));*/
		//}

		
		void MoveC(float t, float width, float height) {
			
			float vx = g_v * cos(g_alfa);
			float vy = g_v * sin(g_alfa);
			float ratiox = 1;
			float ratioy = 1;
			bool istouchhorizont = false;
			bool istouchvertical = false;

			if (vx * t > width - g_x - g_r) {
				ratiox = (width - g_x - g_r) / (vx * t);
				istouchvertical = true;
			}
			else if (vx * t < -g_x + g_r) {
				ratiox = (-g_x + g_r) / (vx * t);
				istouchvertical = true;
			}

			if (vy * t > height - g_y - g_r) {
				ratioy = (height - g_y - g_r) / (vy * t);
				istouchhorizont = true;
			}

			else if (vy * t < -g_y + g_r) {
				ratioy = (-g_y + g_r) / (vy * t);
				istouchhorizont = true;
			}


			if (!istouchvertical && !istouchhorizont) {
				g_x += vx * t;
				g_y += vy * t;
			}
			else {
				if (ratiox < ratioy) {
					g_x += ratiox * vx * t;
					g_y += ratiox * vy * t;
				}
				else {
					g_x += ratioy * vx * t;
					g_y += ratioy * vy * t;
				}
			}
			circle.setPosition(g_x, g_y);
		}
		void changeColorC() {
			circle.setFillColor(sf::Color(rand() % 256, rand() % 256, rand() % 256, 255));
		}

		sf::CircleShape Get() {
			return circle;
		}
	};


	class Triangle {
		float t_x;
		float t_y;
		float t_beta;
		float t_v;
		float t_r;
		sf::CircleShape triangle;
	public:
		Triangle() = default;

		Triangle(float x, float y, float r, float beta, float v) {
			SetupT(x, y, r, beta, v);
		}
		void SetupT(float x, float y, float r, float beta, float v) {
			t_x = x;
			t_y = y;
			t_beta = beta;
			t_v = v;
			t_r = r;
			triangle.setPointCount(3);
			/*triangle.setPoint(0, sf::Vector2f(150, 600));
			triangle.setPoint(1, sf::Vector2f(200, 150));
			triangle.setPoint(2, sf::Vector2f(300, 600));*/
			triangle.setRadius(t_r);
			triangle.setOrigin(t_r, t_r);
			triangle.setPosition(t_x, t_y);
			triangle.setFillColor(sf::Color(0, 255, 0, 255));
		}

		float TX() { return t_x; }
		float TY() { return t_y; }
		float TR() { return t_r; }
		float TV() { return t_v; }

		void Ttouch() {
			t_v = 0;
		}

		void changeColorT() {
			triangle.setFillColor(sf::Color(rand()% 256, rand() % 256, rand() % 256, 255));
			
		}


		void MoveT(float t, float width, float height) {
			float ratiox = 1;
			float ratioy = 1;
			float tvx = t_v * cos(t_beta);
			float tvy = t_v * sin(t_beta);
			bool istouchhorizont = false;
			bool istouchvertical = false;

			if (tvx * t > width - t_x - t_r * (sqrt(3) / 2)) {  
				ratiox = (width - t_x - t_r * (sqrt(3) / 2)) / (tvx * t);
				istouchvertical = true;
			}
			else if (tvx * t < -t_x + t_r * (sqrt(3) / 2)) {  
				ratiox = (-t_x + t_r * (sqrt(3) / 2)) / (tvx * t);
				istouchvertical = true;
			}

			if (tvy * t > height - t_y - t_r/2) {  
				ratioy = (height - t_y - t_r / 2) / (tvy * t);
				istouchhorizont = true;
			}

			else if (tvy * t < -t_y + t_r) {  
				ratioy = (-t_y + t_r) / (tvy * t);
				istouchhorizont = true;
			}


			if (!istouchvertical && !istouchhorizont) {
				t_x += tvx * t;
				t_y += tvy * t;
			}
			else {
				if (ratiox < ratioy) {
					t_x += ratiox * tvx * t;
					t_y += ratiox * tvy * t;
				}
				else {
					t_x += ratioy * tvx * t;
					t_y += ratioy * tvy * t;
				}
			}
			//t_beta += t;
			triangle.setPosition(t_x, t_y);
		}


		sf::CircleShape GetT() {
			return triangle;
		}
	};

	class Square {
		float s_x, s_y, s_v, s_gamma;
		float s_r;
		sf::CircleShape square;
	public:
		Square() = default;

		Square(float x, float y, float r, float v, float gamma) {
			SetupS(x, y, r, v, gamma);
		}

		void SetupS(float x, float y, float r, float v, float gamma) {
			s_x = x;
			s_y = y;
			s_v = v;
			s_r = r;
			s_gamma = gamma;
			square.setPointCount(4);
			square.setRadius(s_r);
			square.setOrigin(s_r, s_r);
			square.setPosition(s_x, s_y);
			square.setFillColor(sf::Color(0, 100, 155, 255));
		}

		float SX() { return s_x; }
		float SY() { return s_y; }
		float SV() { return s_v; }
		float SR() { return s_r; }

		void touchS() {
			s_v = 0;
		}

		void changeColorS() {
			square.setFillColor(sf::Color(rand() % 256, rand() % 256, rand() % 256, 255));
		}


		void MoveS(float t, float width, float height) {
			float svx = s_v * cos(s_gamma);
			float svy = s_v * sin(s_gamma);
			float ratiox = 1;
			float ratioy = 1;
			bool istouchhorizont = false;
			bool istouchvertical = false;

			if (svx * t > width - s_x - s_r) {
				ratiox = (width - s_x - s_r) / (svx * t);
				istouchvertical = true;
			}
			else if (svx * t < -s_x + s_r) {
				ratiox = (-s_x + s_r) / (svx * t);
				istouchvertical = true;
			}

			if (svy * t > height - s_y - s_r) {
				ratioy = (height - s_y - s_r) / (svy * t);
				istouchhorizont = true;
			}

			else if (svy * t < -s_y + s_r) {
				ratioy = (-s_y + s_r) / (svy * t);
				istouchhorizont = true;
			}


			if (!istouchvertical && !istouchhorizont) {
				s_x += svx * t;
				s_y += svy * t;
			}
			else {
				if (ratiox < ratioy) {
					s_x += ratiox * svx * t;
					s_y += ratiox * svy * t;
				}
				else {
					s_x += ratioy * svx * t;
					s_y += ratioy * svy * t;
				}
			}
			//s_gamma += t;
			square.setPosition(s_x, s_y);
		}


		sf::CircleShape GetS() {
			return square;
		}
	};






}