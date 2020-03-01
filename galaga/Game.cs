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

public class Game : IGameEventProcessor<object> {
    private Window win;
    private DIKUArcade.Timers.GameTimer gameTimer;

    public DIKUArcade.Timers.StaticTimer staticTimer;
    private Player player;
    private DIKUArcade.EventBus.GameEventBus<object> eventBus;
    private List<Image> enemyStrides;
    private List<Enemy> enemies;
    private Enemy enemy;
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
    GameEventType.WindowEvent, }); // messages to the window 
    win.RegisterEventBus(eventBus); 
    eventBus.Subscribe(GameEventType.InputEvent, this); 
    eventBus.Subscribe(GameEventType.WindowEvent, this);

    enemyStrides = ImageStride.CreateStrides(4, Path.Combine("Assets", "Images", "BlueMonster.png"));
    enemies = new List<Enemy>();

    playerShots = new List<Playershot>();

    explosionStrides = ImageStride.CreateStrides(8, Path.Combine("Assets", "Images", "Explosion.png"));
    explosions = new AnimationContainer(70);

    score = new Score(new Vec2F(0.05f,-0.15f), new Vec2F (0.2f,0.2f));

    }
    public void AddEnemies(int aoe){
        for (int i= 0; i < aoe; i++){
        var rand = new Random();
        var randomx = Math.Round((decimal)(rand.NextDouble()*(0.9-0.3)+0.3), 1);
        var randomy = Math.Round((decimal)(rand.NextDouble()*(0.9-0.3)+0.3), 1);
        enemy = new Enemy(
            new DynamicShape(new Vec2F((float)randomx, (float)randomy), new Vec2F(0.1f, 0.1f)),
            new ImageStride(80,enemyStrides));
        enemies.Add(enemy);}
    }

    public void AddExplosion(float posX, float posY,float extentX, float extentY) { 
        explosions.AddAnimation(
        new StationaryShape(posX, posY, extentX, extentY), 500,
        new ImageStride(500 / 8, explosionStrides));
    }

    public void IterateShots()
     {
        int counter = enemies.Count; 
        explosions.RenderAnimations();
        score.RenderScore();
        foreach (var shot in playerShots) {
            shot.Shape.Move();
            if (shot.Shape.Position.Y > 1.0f) {
                shot.DeleteEntity(); 
                } 
            else {
                foreach (var enemy in enemies){
                    if (CollisionDetection.Aabb(shot.Shape.AsDynamicShape(), enemy.Shape).Collision == true)
                    {
                        shot.DeleteEntity();
                        enemy.DeleteEntity();
                        AddExplosion(enemy.Shape.AsDynamicShape().Position.X,enemy.Shape.AsDynamicShape().Position.Y,enemy.Shape.AsDynamicShape().Extent.X,enemy.Shape.AsDynamicShape().Extent.Y);
                        score.AddPoint();
                        counter = counter - 1;
                    }
                }
                if (counter == 0)
                {
                    AddEnemies(4);
                }
                }
        List<Enemy> newEnemies = new List<Enemy>();
        foreach (Enemy enemy in enemies) {
            if (!enemy.IsDeleted()) { 
                newEnemies.Add(enemy);
                } 
            }
        enemies = newEnemies;
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
                foreach(Enemy enemy in enemies) {
                    enemy.RenderEntity();
                }
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
        Vec2F newDirection = new Vec2F();
            switch(key) {
            case "KEY_ESCAPE": eventBus.RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                GameEventType.WindowEvent, this, "CLOSE_WINDOW", "", "")); 
                break;
            case "KEY_A":
                newDirection.X = -0.01f;
                newDirection.Y = 0.0f;
                player.Direction(newDirection);
                break;
            case "KEY_D":
                newDirection.X = 0.01f;
                newDirection.Y = 0.0f;
                player.Direction(newDirection);
                break;
            case "KEY_SPACE":
                player.AddShot();            
                break;
            case "KEY_SPACE" + "KEY_A":
                newDirection.X = -0.01f;
                newDirection.Y = 0.0f;
                player.Direction(newDirection);
                player.AddShot();
                break;

            case "KEY_SPACE" + "KEY_D":
                newDirection.X = 0.01f;
                newDirection.Y = 0.0f;
                player.Direction(newDirection);
                player.AddShot();
                break; 
            }
        }
    public void KeyRelease(string key) {
        if (key != "KEY_ESCAPE") {
        Vec2F nodic = new Vec2F();
        nodic.X = 0.0f;
        nodic.Y = 0.0f;
        player.Direction(nodic);
        }    
    }

    
    public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
        if (eventType == GameEventType.WindowEvent) {
            switch (gameEvent.Message) { case "CLOSE_WINDOW":
                win.CloseWindow();
                break;
            default:
                break;
    }
} else if (eventType == GameEventType.InputEvent) {
    switch (gameEvent.Parameter1) { 
        case "KEY_PRESS":
            KeyPress(gameEvent.Message);
            break;
        case "KEY_RELEASE":
            KeyRelease(gameEvent.Message); 
            break;

  } }
}
}
