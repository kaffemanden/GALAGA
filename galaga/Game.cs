using System;
using System.IO;
using System.Collections.Generic;
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

public class Game : IGameEventProcessor<object> {
    private Window win;
    private DIKUArcade.Timers.GameTimer gameTimer;
    private Player player;
    private DIKUArcade.EventBus.GameEventBus<object> eventBus;
    private List<Image> enemyStrides;
    private List<Enemy> enemies;
    private Enemy enemy;
    public Game() {
// TODO: Choose some reasonable values for the window and timer constructor. // For the window, we recommend a 500x500 resolution (a 1:1 aspect ratio). 
    
    win = new Window("galaga", 500, 500);
    gameTimer = new GameTimer();
    
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
    }
    public void AddEnemies(){
        enemy = new Enemy(
            new DynamicShape(new Vec2F(0.1f, 0.9f), new Vec2F(0.1f, 0.1f)),
            new ImageStride(80,enemyStrides));
        enemies.Add(enemy);
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
                foreach(Enemy element in enemies) {
                    if (enemy.Shape.Position == )
                    enemy.RenderEntity();
                }
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
            }
        }
    public void KeyRelease(string key) {
        if (key != "KEY_ESCAPE") {
        Vec2F nodic = new Vec2F();
        nodic.X = 0f;
        nodic.Y = 0f;
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
