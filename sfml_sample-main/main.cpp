#include <SFML/Graphics.hpp>
#include <Circle.hpp>
#include <Game.hpp>




int main()
{
    srand(time(0));
    gp::Game game(1280, 760, "LABA3");
    game.Setup();
    game.GAME();



    return 0;
}