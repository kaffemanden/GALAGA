using DIKUArcade.Math;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
public class Playershot : Entity {
    public Playershot(DynamicShape shape, IBaseImage image): base(shape, image) {
        shape.AsDynamicShape().Direction = new Vec2F(0.0f, 0.01f); 
        }
    } 
