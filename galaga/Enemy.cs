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
using galaga;

public class Enemy : Entity {
    public Enemy enemy;
    public Enemy(DynamicShape shape, IBaseImage image): base(shape, image) {

    } 

    public void Move() {
        void position(float x) {
        if (enemy.Shape.Position.X < x && enemy.Shape.Position.Y < 0.9f) {
        enemy.Shape.Move();

    
    }

        }
    
    }
}