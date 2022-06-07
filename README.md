## Description
Galaga is a 1981 fixed shooter arcade video game developed and published by Namco. 
Galaga is a fixed shooter. The player mans a lone starfighter at the bottom of the screen, which must prevent the Galaga forces from destroying all of mankind. The objective of each stage is to defeat all of the Galaga aliens, which will fly into formation from the top and sides of the screen. Similar to Galaxian, aliens will dive towards the player while shooting down projectiles; colliding with either projectiles or aliens will result in a life being lost.

The main idea of a design concept is minimalizm. But it is still video game so i have to use some small effects and animation. And it should be fun and challenging to play.
The configuaration of entire game balance is avalable via Game.prefab. There is GameProcessor script that controls flow of one game session. Configs are provided by json files. I provided 3 level configs, each new wave(after clearing screen) takes next config file. 
I added health to enemy becouse they die too fast. If you dont like it just set big damage in ConfigShip.txt.  
StateGameplay owns Game and link it with GUI. StateGameplay controls creating of game session (Game). But Game can work independantly on the scene. You can even put several copies of a game prefab on the scene and it will work.  
For monster grid configuration there is an array of strings:
    "MonsterGrid": [
            "ggggg",
            "rrrrr",
            "rrrrr",
            "bbbbb",
            ".bbb."
        ]
    * 'g' - Green monster
    * 'r' - Red monster 
    * '.' - Empty slot    
Speed of monster is defined in a range [0,1], where 1 is a max available speed.
    
    
## If you want to contribute
There are several things to do:  

* Faster appearing. Every new raw starts not waiting for previous one.
* True Laser-beam monster logic

Happy to see your pull request! :)