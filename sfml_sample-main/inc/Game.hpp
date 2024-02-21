#pragma once
#include <SFML/Graphics.hpp>
#include <Circle.hpp>
#include <string>
#include <thread>

using namespace std;


namespace gp {

    class Game {
        int width;
        int height;
        string name;
        sf::RenderWindow window;
        gp::Circle fig1;
    public:

        Game(int w, int h, string n) {
            width = w;
            height = h;
            name = n;
        }
        Game() = default;
        void Setup() {
            window.create(sf::VideoMode(width, height), name);

            fig1.Stup(600, 500, 100, 100, 4.5, 255, 0, 0);
        }

        void TouchBorder(Circle& obj) {
            sf::Clock clock;
            float x = obj.X();
            float y = obj.Y();
            float r = obj.R();
            float v = obj.V();
            float t = clock.getElapsedTime().asSeconds();
            if (x + r >= width || x - r <= 0 || y + r >= height || y - r <= 0) {
                fig1.touch();
                fig1.changeColor(t);
            }
            

        }


        void GAME() {
            sf::Clock clock;
            

            while (window.isOpen())
            {
                sf::Event event;
                while (window.pollEvent(event))
                {
                    if (event.type == sf::Event::Closed)
                        window.close();
                }
                //this_thread::sleep_for(chrono::milliseconds(1000));
                //Организация движения
                float t = clock.getElapsedTime().asSeconds();
                clock.restart();
                fig1.Move(t);

                TouchBorder(fig1);




                window.clear();
                window.draw(fig1.Get());
                window.display();
            }
        }
    };

}