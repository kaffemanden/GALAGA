using System;
using System.IO;
using System.Collections.Generic;
using DIKUArcade;
using DIKUArcade.EventBus;
using DIKUArcade.Timers;
using DIKUArcade.Math;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Physics;
using ScoreSystem;
using galaga.Squadron;
using galaga.MovementStrategy;

public class Game : IGameEventProcessor<object> {
    private Window win;
    private formation1 Formation1;
    private formation2 Formation2;
    private formation3 Formation3;
    private NoMove nomove;
    private Down down;
    private Zigzag zigzag;
    private DIKUArcade.Timers.GameTimer gameTimer;
    public DIKUArcade.Timers.StaticTimer staticTimer;
    private Player player;
    private DIKUArcade.EventBus.GameEventBus<object> eventBus;
    private List<Image> enemyStrides;
    public static List<Playershot> playerShots {get; private set;}
    private List<Image> explosionStrides;
    private AnimationContainer explosions;
    private Score score;
    public Game() {
// TODO: Choose some reasonable values for the window and timer constructor. // For the window, we recommend a 500x500 resolution (a 1:1 aspect ratio). 
    staticTimer = new StaticTimer();
    win = new Window("galaga", 500, 500);
    gameTimer = new GameTimer(60,60);
    
    player = new Player(
        new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
        new Image(Path.Combine("Assets", "Images", "Player.png")));

    eventBus = new GameEventBus<object>(); 
    eventBus.InitializeEventBus(new List<GameEventType>() {
    GameEventType.InputEvent, // key press / key release
    GameEventType.WindowEvent, // messages to the window  
    GameEventType.PlayerEvent }); 
    win.RegisterEventBus(eventBus); 
    eventBus.Subscribe(GameEventType.InputEvent, this); 
    eventBus.Subscribe(GameEventType.WindowEvent, this);
    eventBus.Subscribe(GameEventType.PlayerEvent, player);

    enemyStrides = ImageStride.CreateStrides(4, Path.Combine("Assets", "Images", "BlueMonster.png"));
    playerShots = new List<Playershot>();

    explosionStrides = ImageStride.CreateStrides(8, Path.Combine("Assets", "Images", "Explosion.png"));
    explosions = new AnimationContainer(70);
    score = new Score(new Vec2F(0.05f,-0.15f), new Vec2F (0.2f,0.2f));

    Formation1 = new formation1();
    Formation2 = new formation2();
    Formation3 = new formation3();
    this.Formation2.CreateEnemies(enemyStrides);
    
    nomove = new NoMove(); 
    down = new Down(); 
    zigzag = new Zigzag();
    this.nomove.MoveEnemies(Formation1.Enemies);
    }

    public void AddExplosion(float posX, float posY,float extentX, float extentY) { 
        explosions.AddAnimation(
        new StationaryShape(posX, posY, extentX, extentY), 500,
        new ImageStride(500 / 8, explosionStrides));
    }

    private int Move;
    private int diff;
    public void IterateShots()
     {
        explosions.RenderAnimations();
        score.RenderScore();
        int counter1 = Formation1.Enemies.CountEntities();
        int counter2 = Formation2.Enemies.CountEntities();
        int counter3 = Formation3.Enemies.CountEntities();
        int allenemies = counter1 + counter2 + counter3;
        if (counter1 > 0) {
            if (Move == 1){
                for(int i = 0; i < diff; i++){
                this.nomove.MoveEnemies(Formation1.Enemies);
                }
            }
            else if(Move == 2){
                for(int i = 0; i < diff; i++){
                this.down.MoveEnemies(Formation1.Enemies);
                }
            }
            else if(Move == 3){
                for(int i = 0; i < diff; i++){
                this.zigzag.MoveEnemies(Formation1.Enemies);
                }
            }
        }
        if (counter2 > 0) {
            if (Move == 1){
                for(int i = 0; i < diff; i++){
                this.nomove.MoveEnemies(Formation2.Enemies);
                }
            }
            else if(Move == 2){
                for(int i = 0; i < diff; i++){
                this.down.MoveEnemies(Formation2.Enemies);
                }
            }
            else if(Move == 3){
                for(int i = 0; i < diff; i++){
                this.zigzag.MoveEnemies(Formation2.Enemies);
                }
            }
        }
        if (counter3 > 0) {
            if (Move == 1){
                for(int i = 0; i < diff; i++){
                this.nomove.MoveEnemies(Formation3.Enemies);
                }
            }
            else if(Move == 2){
                for(int i = 0; i < diff; i++){
                this.down.MoveEnemies(Formation3.Enemies);
                }
            }
            else if(Move == 3){
                for(int i = 0; i < diff; i++){
                this.zigzag.MoveEnemies(Formation3.Enemies);
                }
            }
        }
        if (allenemies == 0){
            if (new Random().Next(1,4) == 1) {
                this.Formation1.CreateEnemies(enemyStrides);
                Move = new Random().Next(1,4);
                diff++;
                }
            else if (new Random().Next(1,4) == 2){
                this.Formation2.CreateEnemies(enemyStrides);
                Move = new Random().Next(1,4);
                diff++;
                }
            else if (new Random().Next(1,4) == 3){
                this.Formation3.CreateEnemies(enemyStrides);
                Move = new Random().Next(1,4);
                diff++;
                }
            }     
        foreach (var shot in playerShots) {
            shot.Shape.Move();
            if (shot.Shape.Position.Y > 1.0f) {
                shot.DeleteEntity(); 
                } 
            else {
                void checking1(Enemy enemy1) {
                    foreach (Enemy enemy in Formation1.Enemies){
                        if (CollisionDetection.Aabb(shot.Shape.AsDynamicShape(), enemy.Shape).Collision == true)
                        {
                            shot.DeleteEntity();
                            enemy.DeleteEntity();
                            AddExplosion(enemy.Shape.AsDynamicShape().Position.X,enemy.Shape.AsDynamicShape().Position.Y,enemy.Shape.AsDynamicShape().Extent.X,enemy.Shape.AsDynamicShape().Extent.Y);
                            score.AddPoint();
                        }
                    }
                }
                void checking2(Enemy enemy2) {
                    foreach (Enemy enemy in Formation2.Enemies){
                        if (CollisionDetection.Aabb(shot.Shape.AsDynamicShape(), enemy.Shape).Collision == true)
                        {
                            shot.DeleteEntity();
                            enemy.DeleteEntity();
                            AddExplosion(enemy.Shape.AsDynamicShape().Position.X,enemy.Shape.AsDynamicShape().Position.Y,enemy.Shape.AsDynamicShape().Extent.X,enemy.Shape.AsDynamicShape().Extent.Y);
                            score.AddPoint();
                        }
                    }
                }
                void checking3(Enemy enemy3) {
                    foreach (Enemy enemy in Formation3.Enemies){
                        if (CollisionDetection.Aabb(shot.Shape.AsDynamicShape(), enemy.Shape).Collision == true)
                        {
                            shot.DeleteEntity();
                            enemy.DeleteEntity();
                            AddExplosion(enemy.Shape.AsDynamicShape().Position.X,enemy.Shape.AsDynamicShape().Position.Y,enemy.Shape.AsDynamicShape().Extent.X,enemy.Shape.AsDynamicShape().Extent.Y);
                            score.AddPoint();
                        }
                    }
                } 
                Formation1.Enemies.Iterate(checking1);
                Formation2.Enemies.Iterate(checking2);
                Formation3.Enemies.Iterate(checking3);
            }
        List<Playershot> newShots = new List<Playershot>();
        foreach (Playershot shott in playerShots) {
            if (!shott.IsDeleted()) { 
                newShots.Add(shott);
                } 
            }
        playerShots = newShots;   
        } 
    }
    public void GameLoop() {
        while(win.IsRunning()) { 
            gameTimer.MeasureTime();
            while (gameTimer.ShouldUpdate()) {
                win.PollEvents();
            // Update game logic here
            }
            if (gameTimer.ShouldRender()) { 
                win.Clear();
            // Render gameplay entities here
                player.Entity.RenderEntity();
                player.Move();
                eventBus.ProcessEvents();
                Formation1.Enemies.RenderEntities();
                Formation2.Enemies.RenderEntities();
                Formation3.Enemies.RenderEntities();
                foreach(Playershot shot in playerShots) {
                    shot.RenderEntity();
                }
                IterateShots();
                win.SwapBuffers(); 
            }

            if (gameTimer.ShouldReset()) {
                // 1 second has passed - display last captured ups and fps 
                win.Title = "Galaga | UPS: " + gameTimer.CapturedUpdates +
                            ", FPS: " + gameTimer.CapturedFrames;
            }
        }
    }

    private void KeyPress(string key) {
            switch(key) {
            case "KEY_ESCAPE": eventBus.RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                GameEventType.WindowEvent, this, "CLOSE_WINDOW", "", "")); 
                break;
            case "KEY_A": eventBus.RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                GameEventType.PlayerEvent, this, "MOVE_LEFT", "", ""));
                break;
            case "KEY_D": eventBus.RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                GameEventType.PlayerEvent, this, "MOVE_RIGHT", "",""));
                break;
            case "KEY_SPACE": eventBus.RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                GameEventType.PlayerEvent, this, "SHOTS", "",""));            
                break;
            }
        }
    
    public void KeyRelease(string key) {
        if (key != "KEY_ESCAPE"){
        switch(key) {
            case "KEY_A": eventBus.RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                GameEventType.PlayerEvent, this, "KEY_RELEASE", "", ""));
            break;
            case "KEY_D": eventBus.RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                GameEventType.PlayerEvent, this, "KEY_RELEASE", "", ""));
            break;
            }
        }    
    }

    public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
        if (eventType == GameEventType.WindowEvent) {
            switch (gameEvent.Message) { 
            case "CLOSE_WINDOW":
                win.CloseWindow();
                break;
            default:
                break;
            }   
        } 
        else if (eventType == GameEventType.InputEvent) {
        switch (gameEvent.Parameter1) { 
            case "KEY_PRESS":
                KeyPress(gameEvent.Message);
                break;
            case "KEY_RELEASE":
                KeyRelease(gameEvent.Message); 
                break;
            } 
        }
    }
}
