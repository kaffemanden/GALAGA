using DIKUArcade.Math;
using DIKUArcade.Graphics;

namespace SystemStartScreen {
public class StartScreen{
    private Text display;
    private Text display1;
    public StartScreen() {
        display = new Text("HALFLIFE 3", new Vec2F(0.30f,0.12f), new Vec2F (0.5f,0.5f));
        display1 = new Text("PRESS ENTER TO START", new Vec2F(0.355f,0.225f), new Vec2F (0.3f,0.3f));
    }
    // The Method that renders the endscreen
    public void RenderStartScreen() {
        display.SetText(string.Format("HALF-LIFE 3")); 
        display.SetColor(new Vec3I(255, 0, 0));
        display.RenderText();
        display1.SetFontSize(28);
        display1.SetText(string.Format("PRESS ENTER TO START")); 
        display1.SetColor(new Vec3I(255, 0, 0));
        display1.RenderText();
        }
    }
}