using DIKUArcade.Math;
using DIKUArcade.Graphics;

namespace ScoreSystem {
public class Score{
    private int score;
    private Text display;
    //Method that places the score
    public Score(Vec2F position, Vec2F extent) {
        score = 0;
        display = new Text(score.ToString(), position, extent);
    }
    // Method that adds points to the score
    public void AddPoint() {
        score += 1;
    }
    public void ResetScore(){
        score = 0; 
    }
    // The Method that renders the score when called in game loop 
    public void RenderScore() {
        display.SetText(string.Format("Score: {0}", score.ToString())); 
        display.SetColor(new Vec3I(255, 0, 0));
        display.RenderText();
        }
    }
}