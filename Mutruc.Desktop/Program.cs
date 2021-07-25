using osu.Framework.Platform;
using osu.Framework;
using Mutruc.Game;

namespace Mutruc.Desktop
{
    public static class Program
    {
        public static void Main()
        {
            using (GameHost host = Host.GetSuitableHost(@"Mutruc"))
            using (osu.Framework.Game game = new MutrucGame())
                host.Run(game);
        }
    }
}
