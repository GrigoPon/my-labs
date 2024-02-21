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
        Circle fig1;
        Triangle fig2;
        Square fig3;
        bool toucherC = false;
        bool toucherT = false;
        bool toucherS = false;
    public:
        
        Game(int w, int h, string n) {
            width = w;
            height = h;
            name = n;
        }
        Game() = default;
        void Setup() {
            window.create(sf::VideoMode(width, height), name);
            fig1.Stup(600, 500, 100, 100, 4.25, 255, 0, 0);
            fig2.SetupT(300, 400, 80, 4, 150);
            fig3.SetupS(800, 500, 120, 80, 3.8);
            
        }

        void TouchBorderC(Circle& obj) {
            sf::Clock clock;
            float x = obj.X();
            float y = obj.Y();
            float r = obj.R();
            float v = obj.V();
            float t = clock.getElapsedTime().asSeconds();
            if (x + r >= width || x - r <= 0 || y + r >= height || y - r <= 0) {
                fig1.touch();
                //fig1.changeColor(t);
                toucherC = true;
            }
        }
        void TouchBorderT(Triangle& obj) {
            sf::Clock clockT;
            float tx = obj.TX();
            float ty = obj.TY();
            float tr = obj.TR();
            float tv = obj.TV();
            float t = clockT.getElapsedTime().asSeconds();
            if (tx + (tr*(sqrt(3)/2)) >= width || tx - (tr * (sqrt(3) / 2)) <= 0 || ty + tr/2 >= height || ty - tr <= 0) {
                fig2.Ttouch();
                toucherT = true;
            }
            
        }

        void TouchBorderS(Square& obj) {
            sf::Clock clockS;
            float sx = obj.SX();
            float sy = obj.SY();
            float sr = obj.SR();
            float sv = obj.SV();

            float t = clockS.getElapsedTime().asSeconds();
            if (sx + sr >= width || sx - sr <= 0 || sy + sr >= height || sy - sr <= 0) {
                fig3.touchS();
                toucherS = true;
            }
            
        }



        void GAME() {
            sf::Clock clock;
            
            window.clear();
            window.draw(fig1.Get());
            window.draw(fig2.GetT());
            window.draw(fig3.GetS());
            window.display();
            this_thread::sleep_for(chrono::seconds(1));

            sf::Clock timer;

            while (window.isOpen())
            {
                sf::Event event;
                while (window.pollEvent(event))
                {
                    if (event.type == sf::Event::Closed)
                        window.close();
                }
                
                //Организация движения
                if (timer.getElapsedTime().asSeconds() > 0.5) {
                    if(toucherC)
                        fig1.changeColorC();
                    if(toucherT)
                        fig2.changeColorT();
                    if(toucherS)
                        fig3.changeColorS();
                    timer.restart();
                }
                float t = clock.getElapsedTime().asSeconds();
                clock.restart();
                fig1.MoveC(t);
                fig2.MoveT(t);
                fig3.MoveS(t);
                
                    
                TouchBorderC(fig1);
                TouchBorderT(fig2);
                TouchBorderS(fig3);




                window.clear();
                window.draw(fig1.Get());
                window.draw(fig2.GetT());
                window.draw(fig3.GetS());
                window.display();
            }
        }
    };

}