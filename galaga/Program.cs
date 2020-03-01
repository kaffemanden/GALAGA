


namespace galaga
{
    class Program
    {
        static void Main(string[] args)
        {
           Game game = new Game();
            game.AddEnemies(10);
            game.GameLoop();
        }
    }
}
