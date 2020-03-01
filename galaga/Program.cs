using System;
using System.IO;
using DIKUArcade;
using DIKUArcade.EventBus;
using DIKUArcade.Timers;
using DIKUArcade.Math;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Input;
using DIKUArcade.Physics;
using DIKUArcade.State;
using DIKUArcade.Utilities;


namespace galaga
{
    class Program
    {
        static void Main(string[] args)
        {
           Game game = new Game();
            game.AddEnemies(5);
            game.GameLoop();
        }
    }
}
