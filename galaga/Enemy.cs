using DIKUArcade.Math;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;

public class Enemy : Entity {
    public Vec2F StartPosition;

    public Enemy(DynamicShape shape, IBaseImage image): base(shape, image) {
        this.StartPosition = shape.Position;
    } 
}
