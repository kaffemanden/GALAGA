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

namespace ScoreSystem {
public class Score{
    private int score;
private Text display;
public Score(Vec2F position, Vec2F extent) {
       score = 0;
       display = new Text(score.ToString(), position, extent);
 }
public void AddPoint() {
    score = score + 200;
}
public void RenderScore() {
    display.SetText(string.Format("Score: {0}", score.ToString())); 
    display.SetColor(new Vec3I(255, 0, 0));
    display.RenderText();
}
}
}