using System;
using DIKUArcade.Math;
using DIKUArcade.Entities;

namespace galaga.MovementStrategy { 
    public interface IMovementStrategy {
        void MoveEnemy(Enemy enemy);
        void MoveEnemies(EntityContainer<Enemy> enemies);
    }
    // Making the enemies stand still.
    public class NoMove : IMovementStrategy {
        public Vec2F direction;
        public NoMove() {
            direction = new Vec2F(0.0f, 0.0f);
        } 
        public void MoveEnemy(Enemy enemy){
            enemy.Shape.AsDynamicShape().Direction = direction; 
            enemy.Shape.Move();
        }
        public void MoveEnemies(EntityContainer<Enemy> enemies){
            foreach (Enemy enemy in enemies)  {
                MoveEnemy(enemy);
            }
        }
    }
    // Making the enemies move downwards.
    public class Down : IMovementStrategy {
        public Vec2F direction;
        public Down() {
            direction = new Vec2F(0.0f, -0.0001f);
        } 
        public void MoveEnemy(Enemy enemy){
            enemy.Shape.AsDynamicShape().Direction = direction; 
            enemy.Shape.Move();
        }
        public void MoveEnemies(EntityContainer<Enemy> enemies){
            foreach (Enemy enemy in enemies)  {
                MoveEnemy(enemy);
            }
        }
    }    
    // Making the enemies move in a zigzag formation.
    public class  Zigzag: IMovementStrategy {
        public Vec2F direction;
        public double speed;
        public double period;
        public double amplitude;
        public Zigzag() {
            speed = 0.0001;
            period = 0.045;
            amplitude = 0.05;
        } 
        public void MoveEnemy(Enemy enemy){
            var Nuposition = enemy.Shape.AsDynamicShape().Position;
            var NewYposition = Nuposition.Y + speed;
            var NewXposition = enemy.StartPosition.X + amplitude * Math.Sin((2 * Math.PI * (enemy.StartPosition.Y - NewYposition)/period));
            var Newposition = new Vec2F((float)NewXposition,(float)NewYposition);
            direction = new Vec2F((float)(NewXposition -Nuposition.X),(float)(Nuposition.Y - NewYposition));
            enemy.Shape.AsDynamicShape().Direction = direction;
            enemy.Shape.Move();
        }
        public void MoveEnemies(EntityContainer<Enemy> enemies){
            foreach (Enemy enemy in enemies)  {
                MoveEnemy(enemy);
            }
        }
    } 
}
