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
using SystemEndScreen;
using SystemBackGround;
using SystemStartScreen;

public class Game : IGameEventProcessor<object> {
    private Window win;
    private formation1 Formation1;
    private formation2 Formation2;
    private formation3 Formation3;
    private Boss boss; 
    private NoMove nomove;
    private Down down;
    private Zigzag zigzag;
    private DIKUArcade.Timers.GameTimer gameTimer;
    public DIKUArcade.Timers.StaticTimer staticTimer;
    private Player player;
    private DIKUArcade.EventBus.GameEventBus<object> eventBus;
    private List<Image> enemyStrides;
    private List<Image> enemyStrides1;
    private List<Image> BossStrides;
    public static List<Playershot> playerShots {get; private set;}
    private List<Image> explosionStrides;
    private AnimationContainer explosions;
    private Score score;
    private EndScreen endscreen;
    private StartScreen startscreen;
    private BackGround background;
    public Game() {
// TODO: Choose some reasonable values for the window and timer constructor. // For the window, we recommend a 500x500 resolution (a 1:1 aspect ratio). 
    staticTimer = new StaticTimer();
    win = new Window("galaga", 500, 500);
    gameTimer = new GameTimer(60,60);
    
    background = new BackGround(
        new DynamicShape(new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)),
        new Image(Path.Combine("Assets", "Images", "SpaceBackground.png")));

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
    enemyStrides1 = ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "GreenMonster.png"));
    BossStrides = ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "RedMonster.png")); 

    playerShots = new List<Playershot>();

    explosionStrides = ImageStride.CreateStrides(8, Path.Combine("Assets", "Images", "Explosion.png"));
    explosions = new AnimationContainer(70);

    score = new Score(new Vec2F(0.05f,-0.15f), new Vec2F (0.2f,0.2f));
    endscreen = new EndScreen();
    startscreen = new StartScreen();

    Formation1 = new formation1();
    Formation2 = new formation2();
    Formation3 = new formation3();
    boss = new Boss{};
    
    nomove = new NoMove(); 
    down = new Down(); 
    zigzag = new Zigzag();


    }
    // Method that adds the explosion animation and determines its animation speed
    public void AddExplosion(float posX, float posY,float extentX, float extentY) { 
        explosions.AddAnimation(
        new StationaryShape(posX, posY, extentX, extentY), 500,
        new ImageStride(500 / 8, explosionStrides));
    }

    public void IterateShots()
    {    
        foreach (var shot in playerShots) {
            shot.Shape.Move();
            if (shot.Shape.Position.Y > 1.0f) {
                shot.DeleteEntity(); 
                } 
            else {
                // checks for a collistion between the shot and enenmy.
                void ChecksCollision (EntityContainer<Enemy> Formation){
                    foreach (Enemy enemy in Formation){
                        if (CollisionDetection.Aabb(shot.Shape.AsDynamicShape(), enemy.Shape).Collision){
                            shot.DeleteEntity();
                            enemy.DeleteEntity();
                            AddExplosion(enemy.Shape.AsDynamicShape().Position.X,enemy.Shape.AsDynamicShape().Position.Y,enemy.Shape.AsDynamicShape().Extent.X,enemy.Shape.AsDynamicShape().Extent.Y);
                            score.AddPoint();
                        }
                        else if(CollisionDetection.Aabb(enemy.Shape.AsDynamicShape(), shot.Shape).Collision){
                            shot.DeleteEntity();
                            enemy.DeleteEntity();
                            AddExplosion(enemy.Shape.AsDynamicShape().Position.X,enemy.Shape.AsDynamicShape().Position.Y,enemy.Shape.AsDynamicShape().Extent.X,enemy.Shape.AsDynamicShape().Extent.Y);
                            score.AddPoint();
                        }
                    }
                }
                void Checking(Enemy enemy){
                    ChecksCollision(Formation1.Enemies);
                    ChecksCollision(Formation2.Enemies);
                    ChecksCollision(Formation3.Enemies);
                }
                void BossCollision(Enemy enemy1){
                    foreach (Enemy enemy in boss.Enemies){
                        if (CollisionDetection.Aabb(shot.Shape.AsDynamicShape(), enemy.Shape).Collision){
                            shot.DeleteEntity();
                            enemy.Health -=20;
                            if(enemy.Health <= 0){
                                enemy.DeleteEntity();
                                AddExplosion(enemy.Shape.AsDynamicShape().Position.X,enemy.Shape.AsDynamicShape().Position.Y,enemy.Shape.AsDynamicShape().Extent.X,enemy.Shape.AsDynamicShape().Extent.Y);
                                score.AddPoint();
                            }
                        }
                    }                    
                }
                Formation1.Enemies.Iterate(Checking);
                Formation2.Enemies.Iterate(Checking);
                Formation3.Enemies.Iterate(Checking);
                boss.Enemies.Iterate(BossCollision);
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
    // Field that make the enemy do a random movement. 
    private int Move;
    // Field that incease the difficulty.
    private int diff;
    private void EnemySpawner(){
        var rand = new Random().Next(1,4);
        int counter1 = Formation1.Enemies.CountEntities();
        int counter2 = Formation2.Enemies.CountEntities();
        int counter3 = Formation3.Enemies.CountEntities();
        int bosscount = boss.Enemies.CountEntities();
        int allenemies = counter1 + counter2 + counter3 + bosscount;
        // Checking if there are no enemies in the formation array. If true then it spawns a random enemy formation and update the fields Move and diff, so.
        if (allenemies == 0){
            if (diff % 10 ==9){
                this.boss.CreateEnemies(BossStrides);
                foreach(Enemy enemy in boss.Enemies){
                    enemy.Health *= diff;
                }
                Move = 4;
                diff++;
            }
            else{
                var randomstrides = new List<Image>();
                if(new Random().Next(1,3) == 2){
                    randomstrides = enemyStrides1;
                }
                else {
                    randomstrides = enemyStrides;
                }
                if (rand == 1) {
                    this.Formation1.CreateEnemies(randomstrides);
                    Move = new Random().Next(1,4);
                    diff++;
                }
                else if (rand == 2){
                    this.Formation2.CreateEnemies(randomstrides);
                    Move = new Random().Next(1,4);
                    diff++;
                }
                else if (rand == 3){
                    this.Formation3.CreateEnemies(randomstrides);
                    Move = new Random().Next(1,4);
                    diff++;
                }
            }
        } 
        // Making the enemies of formation1 do a random movement and increaseing the difficulty depending on the diff field.
        void WhatMove(EntityContainer<Enemy> Formation){
            if (Formation.CountEntities() > 0) {
                for(int i = 0; i < diff; i++){
                    if (Move == 1){
                        this.nomove.MoveEnemies(Formation);
                        }
                    else if(Move == 2){
                        this.down.MoveEnemies(Formation);
                        }
                    else if(Move == 3){
                        this.zigzag.MoveEnemies(Formation);
                        }
                }
                if(Move == 4){
                    for(int i = 0; i < 5; i++){
                        this.down.MoveEnemies(Formation);
                    }
                }
            }
        }
        void Moves(){
            WhatMove(Formation1.Enemies);
            WhatMove(Formation2.Enemies);
            WhatMove(Formation3.Enemies);
            WhatMove(boss.Enemies);
        }
        Moves();
    }

    // Decide if the game is over
    private bool GameOver = false;
    
    //Checks if the game is over. While the game is not over, the game countinues else the game window is cleared and
    // only shows the score. 
    public void GameOverz(){
        void Checking(EntityContainer<Enemy> Formation){
            foreach (Enemy enemy in Formation){
                if (enemy.Shape.AsDynamicShape().Position.Y < 0.01f){
                    GameOver = true;
                }
                else if (CollisionDetection.Aabb(enemy.Shape.AsDynamicShape(), player.Entity.Shape).Collision){
                    GameOver = true;
                }
                else if (CollisionDetection.Aabb(player.Entity.Shape.AsDynamicShape(), enemy.Shape).Collision){
                    GameOver = true;
                }
                if(GameOver ==true){
                    enemy.DeleteEntity();
                    AddExplosion(enemy.Shape.AsDynamicShape().Position.X,enemy.Shape.AsDynamicShape().Position.Y,enemy.Shape.AsDynamicShape().Extent.X,enemy.Shape.AsDynamicShape().Extent.Y);
                    AddExplosion(player.Entity.Shape.AsDynamicShape().Position.X,player.Entity.Shape.AsDynamicShape().Position.Y,player.Entity.Shape.AsDynamicShape().Extent.X,player.Entity.Shape.AsDynamicShape().Extent.Y);

                }
            }
        }
        void CheckGameOver(Enemy enemy){
            Checking(Formation1.Enemies);
            Checking(Formation2.Enemies);
            Checking(Formation3.Enemies);
            Checking(boss.Enemies);
        }
        Formation1.Enemies.Iterate(CheckGameOver);
        Formation2.Enemies.Iterate(CheckGameOver);
        Formation3.Enemies.Iterate(CheckGameOver);
        boss.Enemies.Iterate(CheckGameOver);        
    }

    //Method that restarts the game
    public void Restart (){
        void Remove(EntityContainer<Enemy> Formation){
            foreach (Enemy enemy in Formation){
                enemy.DeleteEntity();
            }
        }
        foreach (var shot in playerShots) {
            shot.DeleteEntity();
        }
        Remove(Formation1.Enemies);
        Remove(Formation2.Enemies);
        Remove(Formation3.Enemies);
        GameOver = false;
        diff = 1;
        player.Entity.Shape.AsDynamicShape().Position = new Vec2F(0.45f, 0.1f);
        score.ResetScore();
    }
    private bool StartGame = false;

    // The Game Loop function that constantly checks for updates and keeps the game running.
    public void GameLoop() {
        while(win.IsRunning()) { 
            gameTimer.MeasureTime();
            while (gameTimer.ShouldUpdate()) {
                win.PollEvents();
            // Update game logic here
            }
            if (gameTimer.ShouldRender()) { 
                win.Clear();
                if(StartGame == false){
                    startscreen.RenderStartScreen();
                    eventBus.ProcessEvents();
                }
                else{
            // Render gameplay entities here
                    if (GameOver == false){
                        background.RenderEntity();
                        foreach(Playershot shot in playerShots) {
                            shot.RenderEntity();
                        }         
                        player.Entity.RenderEntity();
                        player.Move();
                        eventBus.ProcessEvents();
                        Formation1.Enemies.RenderEntities();
                        Formation2.Enemies.RenderEntities();
                        Formation3.Enemies.RenderEntities();
                        boss.Enemies.RenderEntities();
                        IterateShots();
                        explosions.RenderAnimations();
                        score.RenderScore();
                        EnemySpawner();
                        GameOverz();
                    }
                    else
                    {
                        explosions.RenderAnimations();
                        endscreen.RenderEndscreen();
                        score.RenderScore();   
                        eventBus.ProcessEvents();
                        
                    }
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
    // A method that registers the key input to the playerevent. 
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
            case "KEY_R": eventBus.RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                GameEventType.WindowEvent, this, "RESTART", "", "")); 
                break;
            case "KEY_ENTER": eventBus.RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                GameEventType.WindowEvent, this, "START", "", "")); 
                break;
            }
        }
    // A method that registers the key input to the playerevent. 
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
            case "RESTART":
                Restart();
                break;
            case "START":
                StartGame = true;
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
