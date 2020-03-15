using DIKUArcade.Math;
using DIKUArcade.Graphics;

namespace SystemEndScreen {
public class EndScreen{
    private Text display;
    private Text display1;
    public EndScreen() {
        display = new Text("GAMEOVER", new Vec2F(0.30f,0.12f), new Vec2F (0.5f,0.5f));
        display1 = new Text("PRESS R TO RESTART", new Vec2F(0.375f,0.225f), new Vec2F (0.3f,0.3f));
    }
    // The Method that renders the endscreen
    public void RenderEndscreen() {
        display.SetText(string.Format("GAMEOVER")); 
        display.SetColor(new Vec3I(255, 0, 0));
        display.RenderText();
        display1.SetFontSize(28);
        display1.SetText(string.Format("PRESS R TO RESTART")); 
        display1.SetColor(new Vec3I(255, 0, 0));
        display1.RenderText();
        }
    }
}