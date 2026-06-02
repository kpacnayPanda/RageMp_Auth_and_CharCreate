using RAGE;
using RAGE.Elements;
using RAGE.Game;
using RAGE.Ui;

namespace Client
{
    public class Main : Events.Script
    {
        public static HtmlWindow OpenedWindow;
        public static Camera CurrentCamera;
        public Main()
        {
            Events.OnPlayerReady += OnPlayerReady;
            Events.OnPlayerSpawn += OnPlayerSpawn;
            Events.OnPlayerCreateWaypoint += OnPlayerCreateWaypoint;
            Events.Add("closeBrowser", OnCloseBrowserMessage);
        }

        private static void OnPlayerReady()
        {

        }

        private static void OnPlayerCreateWaypoint(Vector3 position)
        {
            Events.CallRemote("CLIENT:SERVER::CLIENT_CREATE_WAYPOINT", position.X, position.Y, position.Z);
        }

        private static void OnCloseBrowserMessage(object[] args)
        {
            OpenedWindow.Destroy();
            Cursor.ShowCursor(false, false);
        }

        private static void OnPlayerSpawn(Events.CancelEventArgs cancel)
        {
            OpenedWindow = new HtmlWindow("package://cef/auth/index.html")
            {
                Active = true
            };
            OpenedWindow.ExecuteJs("document.dispatchEvent(new Event('showLogin'))");
            Cursor.ShowCursor(true, true);
            Ui.DisplayRadar(false);
            Chat.Show(false);
        }
    }
}