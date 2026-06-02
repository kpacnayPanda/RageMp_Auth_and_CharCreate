using Newtonsoft.Json.Linq;
using RAGE;
using RAGE.Elements;
using RAGE.Game;
using RAGE.Ui;

namespace Client.Handlers
{
    public class ServerEvents : Events.Script
    {
        public ServerEvents() 
        {
            Events.Add("SERVER:CLIENT::REGISTER_USER", OnServerRegisterUser);
            Events.Add("SERVER:CLIENT::LOGIN_USER", OnServerLoginUser);
            Events.Add("SERVER:CLIENT::CREATE_PERSON", OnServerCreatePerson);
            Events.Add("SERVER:CLIENT::UPDATE_SAVED_CUSTOMIZATION", OnUpdateSavedCustomization);
        }

        private static void OnServerRegisterUser(object[] args)
        {
            bool isExists = (bool)args[0];
            if (isExists) Main.OpenedWindow.ExecuteJs("document.dispatchEvent(new Event('registerUserExists'))");
            else
            {
                Main.OpenedWindow.Destroy();

                Main.CurrentCamera = new Camera((ushort)Cam.CreateCameraWithParams(Misc.GetHashKey("DEFAULT_SCRIPTED_CAMERA"), -1643.6074f, -1091.1317f, 13.568433f, 0f, 0f, 68.35428f, 70.0f, true, 2), 0);
                Cam.PointCamAtCoord(Main.CurrentCamera.Id, -1642.6818f, -1092.0542f, 13.52933f);
                Cam.SetCamActive(Main.CurrentCamera.Id, true);
                Cam.RenderScriptCams(true, false, 0, true, false, 0);

                Main.OpenedWindow = new HtmlWindow("package://cef/person_creater/index.html");
                Main.OpenedWindow.Active = true;
                Cursor.ShowCursor(true, true);
                
                RAGE.Elements.Player.LocalPlayer.SetData("CLIENT_CUSTOMIZATION_DATA_GENDER", "male");
                Customization.ResetLocalPlayerCustomization();
            }
        }

        private static void OnServerLoginUser(object[] args)
        {
            bool notExists = (bool)args[0];
            if (notExists) Main.OpenedWindow.ExecuteJs("document.dispatchEvent(new Event('loginNotValidData'))");
            else
            {
                Main.OpenedWindow.Destroy();
                Cursor.ShowCursor(false, false);
            }
        }

        private static void OnServerCreatePerson(object[] args)
        {
            if (Main.OpenedWindow != null) Main.OpenedWindow.Destroy();
            Cursor.ShowCursor(false, false);
            Ui.DisplayRadar(true);
            Chat.Show(true);

            Cam.RenderScriptCams(false, false, 0, true, false, 0);
            Main.CurrentCamera = null;
        }

        private static void OnUpdateSavedCustomization(object[] args)
        {
            string gender = args[0].ToString();
            RAGE.Elements.Player.LocalPlayer.SetData("CLIENT_CUSTOMIZATION_DATA_GENDER", gender);
            
            if (!RAGE.Elements.Player.LocalPlayer.HasData("CLIENT_CUSTOMIZATION_DATA"))
            {
                Customization.ResetLocalPlayerCustomization(gender);
                return;
            }
            
            string json = RAGE.Elements.Player.LocalPlayer.GetData<string>("CLIENT_CUSTOMIZATION_DATA");
            
            if (string.IsNullOrEmpty(json)) return;
            dynamic customizationInfo = JObject.Parse(json);
            
            Customization.SetLocalPlayerCustomization(customizationInfo, gender);
        }
    }
}
