using DIKUArcade.Math;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
// The class responsible for the enemy entity
public class Enemy : Entity {
    // A field made to acces the enemys original position. It is later used to make the Zigzag movement.
    public Vec2F StartPosition;
    public Enemy(DynamicShape shape, IBaseImage image): base(shape, image) {
        this.StartPosition = shape.Position;
    } 
}
