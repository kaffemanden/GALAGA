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

public class Playershot : Entity {
    private Playershot playershot;

    private Game game;
    public Playershot(DynamicShape shape, IBaseImage image): base(shape, image) {
        Vec2F dic = new Vec2F();
        dic.X = 0.0f;
        dic.Y = 0.1f;
        playershot.Shape.AsDynamicShape().ChangeDirection(dic);}
    } 