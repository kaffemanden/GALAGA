using System.IO;
using DIKUArcade.EventBus;
using DIKUArcade.Math;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
    public class Player : IGameEventProcessor<object>
    {
        public Entity Entity {get; private set;}
    
    public Player(DynamicShape shape, IBaseImage image)
    {
        Entity = new Entity(shape, image);
    }  
    public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
        if (eventType == GameEventType.PlayerEvent) {
            switch (gameEvent.Message) {
                case "MOVE_LEFT":
                    Direction (new Vec2F (-0.01f, 0.0f));
                    break; 
                case "MOVE_RIGHT":
                    Direction (new Vec2F (0.01f, 0.0f)); 
                    break; 
                case "KEY_RELEASE":
                    Direction (new Vec2F (0.0f, 0.0f)); 
                    break;
                case "SHOTS":
                    AddShot(); 
                    break; 
            }
        }
    }
    
    private void Direction(Vec2F Dic) {
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
    public void AddShot() {
        Playershot shot = new Playershot(
            new DynamicShape( new Vec2F((Entity.Shape.Position.X + 0.0465f) ,  0.2f),new Vec2F(0.008f,0.027f)), 
            new Image (Path.Combine("Assets", "Images", "BulletRed2.png")));
        Game.playerShots.Add(shot);
    }
}
