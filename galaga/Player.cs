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

    public class Player : IGameEventProcessor<object>
    {
        public Entity Entity {get; private set;}

    
    public Player(DynamicShape shape, IBaseImage image)
    {
        Entity = new Entity(shape, image);
    }  
    public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
        }
    public void Direction(Vec2F Dic) {
        Entity.Shape.AsDynamicShape().ChangeDirection(Dic);
    }   
    public void Move() {
        if (Entity.Shape.Position.X > 0.9) {
            if (Entity.Shape.AsDynamicShape().Direction.X < 0) {
            Entity.Shape.Move();
            }
            

        }
        else if (Entity.Shape.Position.X < 0) {
            if (Entity.Shape.AsDynamicShape().Direction.X > 0) {
                Entity.Shape.Move();
            }
        }
        else if  (Entity.Shape.Position.X < 0.9f)  {
            Entity.Shape.Move();
        }   
        
    }
    }
