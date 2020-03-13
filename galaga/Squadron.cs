using System;
using System.Collections.Generic;
using DIKUArcade.Math;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace galaga.Squadron { 
    public interface ISquadron {
        // Empty
        EntityContainer<Enemy> Enemies { get; }
        // Max amount in rows
        int MaxEnemies { get; }
        // Creating the enemies 
        void CreateEnemies(List<Image> enemyStrides);
        }
    // Making the enemies spawn in a random position. 
    // The position can't be under a Y.postion of 0.6f and the the X.postion can only be in between 0.1f and 0.8f.
    public class formation1 : ISquadron {
        public EntityContainer<Enemy> Enemies { get; }
        public int MaxEnemies { get; }

        public formation1() {
        MaxEnemies = 10;
        Enemies = new EntityContainer<Enemy> (0);
        }
        public void CreateEnemies(List<Image> enemyStrides){
            for (int i= 0; i < MaxEnemies; i++){
            var rand = new Random();
            var randomx = Math.Round((decimal)(rand.NextDouble()*(0.8-0.1)+0.1), 1);
            var randomy = Math.Round((decimal)(rand.NextDouble()*(0.9-0.6)+0.6), 1);
            var enemy = new Enemy(
            new DynamicShape(new Vec2F((float)randomx, (float)randomy), new Vec2F(0.1f, 0.1f)),
            new ImageStride(80,enemyStrides));
            Enemies.AddDynamicEntity(enemy);
            }
        }
    }

    // A formation of the squadrons where they form a line at the top row. The enemies can only be in spawn in a X.position between 0.1f and 0.8f.
    public class formation2 : ISquadron {
        public EntityContainer<Enemy> Enemies { get; }
        public int MaxEnemies { get; }
        public formation2() {
        MaxEnemies = 8;
        Enemies = new EntityContainer<Enemy> (0);
        }
        public void CreateEnemies(List<Image> enemyStrides){
            for (int i= 1; i <= MaxEnemies; i++){
            var enemy = new Enemy(
            new DynamicShape(new Vec2F(0.1f * i, 0.9f), new Vec2F(0.1f, 0.1f)),
            new ImageStride(80,enemyStrides));
            Enemies.AddDynamicEntity(enemy);
            }
        }
    }
    
    // Our third formation set in two rows by intervals. The enemies will spawn at the top row or secound row with a X.position between 0.1f and 0.8f. 
    public class formation3 : ISquadron {
        public EntityContainer<Enemy> Enemies { get; }
        public int MaxEnemies { get; }
        public formation3() {
        MaxEnemies = 8;
        Enemies = new EntityContainer<Enemy> (0);
        }
        public void CreateEnemies(List<Image> enemyStrides){
            for (int i= 1; i <= MaxEnemies; i++){
                if (i%2 == 1){
                var enemy = new Enemy(
                new DynamicShape(new Vec2F(0.1f * i, 0.9f), new Vec2F(0.1f, 0.1f)),
                new ImageStride(80,enemyStrides));
                Enemies.AddDynamicEntity(enemy);
            }
            else {
                var enemy2 = new Enemy(
                new DynamicShape(new Vec2F(0.1f * i, 0.8f), new Vec2F(0.1f, 0.1f)),
                new ImageStride(80,enemyStrides));
                Enemies.AddDynamicEntity(enemy2);
                }
            }
        }
    }    
}