#include <SFML/Graphics.hpp>
#include <SFML/Audio.hpp>
#include <string>
#include <iostream>
#include <ctime>

using namespace sf;
using namespace std;

class MyText {
    wstring text;
    wstring anim_text;
    
    Text my_Text;
    Font font;
    SoundBuffer buffer;
    Sound sounds;

    float ShowTime;
    float interval;
    float startPos;
    float Pos;
    float changePos;
    int c;
    bool s;
public:
    
    //Конструктор по умолчанию
    MyText() {
        text = L"standart text!";
        anim_text = text[0];
        font.loadFromFile("calibri.ttf");
        buffer.loadFromFile("sound.ogg");

        my_Text.setFont(font);
        my_Text.setString(text);
        my_Text.setCharacterSize(50);
        my_Text.setColor(Color::Red);
        c = 0;
        sounds.setBuffer(buffer);
    }

    //Конструктор
    void Setup(wstring ss1, float time) {
        font.loadFromFile("calibri.ttf");
        
        startPos = 768 / 2;
        Pos = startPos;
        c = 0;
        text = ss1;
        anim_text = ss1[0];
        ShowTime = time;
        interval = ShowTime / (text.length());

        my_Text.setFont(font);
        my_Text.setString(anim_text);
        my_Text.setCharacterSize(50);
        my_Text.setColor(Color::Black);
        my_Text.setPosition(1366 / 2 - 200, startPos);
        changePos = 0;
        
    }

    //анимация (ну типа)
    void Animation() {
        if (anim_text.length() < text.length()) {
            anim_text += text[anim_text.length()];
            changePos = 0;
            my_Text.setString(anim_text);
            if (c == 2)
                changePos = -1;
            else if (c == -2)
                changePos = 1;
            else
                while (changePos == 0) {
                    changePos = rand() % 3 - 1;
                }
            c += changePos;
            Pos += changePos * 30;
            my_Text.setPosition(1366 / 2 - 200, Pos);
        }
        else {
            //sound
            if (s)
                sounds.play();
            s = false;
        }
    }

    void verbAnimationText() {
        
        RenderWindow window(VideoMode(1366, 768), L"Лаба4", Style::Default);

        Clock clock;
        window.setVerticalSyncEnabled(true);
        
        while (window.isOpen())
        {
            Event event;
            while (window.pollEvent(event))
            {
                if (event.type == Event::Closed)
                    window.close();
            }

            float secundas = clock.getElapsedTime().asSeconds();
            if (secundas > interval) {
                Animation();
                my_Text.setFont(font);
                
                clock.restart();
                
            }
            
            
            window.clear(Color::White);
            window.draw(my_Text);
            window.display();
        }


    }

};





int main()
{
    srand(time(NULL));
    setlocale(LC_ALL, "RU");
    MyText lab4;
    
    lab4.Setup(L"Hello World", 7);
    lab4.verbAnimationText();

    return 0;
}