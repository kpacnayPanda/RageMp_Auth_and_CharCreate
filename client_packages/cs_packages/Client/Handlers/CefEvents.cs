using Newtonsoft.Json.Linq;
using RAGE;

namespace Client.Handlers
{
    public class CefEvents : Events.Script
    {
        public CefEvents() 
        {
            Events.Add("CEF:CLIENT::REGISTER_BUTTON_CLICKED", OnCefRegisterButtonClicked);
            Events.Add("CEF:CLIENT::LOGIN_BUTTON_CLICKED", OnCefLoginButtonClicked);
            Events.Add("CEF:CLIENT::PERSON_CREATE_BUTTON_CLICKED", OnCefPersonCreateButtonClicked);
            Events.Add("CEF:CLIENT::PERSON_CREATE_GENDER_SWITCH_BUTTON_CLICKED", OnCefPersonCreateGenderSwitchButtonClicked);
            Events.Add("CEF:CLIENT::PERSON_CREATE_UPDATE_CUSTOMIZATION", OnCefPersonCreateUpdateCustomization);
        }

        private static void OnCefLoginButtonClicked(object[] args)
        {
            string username = args[0].ToString();
            string password = args[1].ToString();
            Events.CallRemote("CLIENT:SERVER::LOGIN_BUTTON_CLICKED", username, password);
        }

        private static void OnCefRegisterButtonClicked(object[] args)
        {
            string username = args[0].ToString();
            string password = args[1].ToString();
            string email = args[2].ToString();

            Events.CallRemote("CLIENT:SERVER::REGISTER_BUTTON_CLICKED", username, password, email);
        }

        private static void OnCefPersonCreateButtonClicked(object[] args)
        {
            string name = args[0].ToString();
            string secondName = args[1].ToString();
            string age = args[2].ToString();
            string gender = args[3].ToString();

            Events.CallRemote("CLIENT:SERVER::PERSON_CREATE_BUTTON_CLICKED", name, secondName, age, gender);
        }

        private static void OnCefPersonCreateGenderSwitchButtonClicked(object[] args)
        {
            string gender = args[0].ToString();
            Events.CallRemote("CLIENT:SERVER::PERSON_CREATE_GENDER_SWITCH_BUTTON_CLICKED", gender);
        }

        private static void OnCefPersonCreateUpdateCustomization(object[] args) 
        {
            string jsonString = args[0].ToString();

            if (string.IsNullOrEmpty(jsonString)) return;
            dynamic customizationInfo = JObject.Parse(jsonString);

            string gender = "male";
            if (RAGE.Elements.Player.LocalPlayer.HasData("CLIENT_CUSTOMIZATION_DATA_GENDER")) 
                gender = RAGE.Elements.Player.LocalPlayer.GetData<string>("CLIENT_CUSTOMIZATION_DATA_GENDER");
            
            Customization.SetLocalPlayerCustomization(customizationInfo, gender);
            
            RAGE.Elements.Player.LocalPlayer.SetData("CLIENT_CUSTOMIZATION_DATA", jsonString);
            //Events.CallRemote("CLIENT:SERVER::PERSON_CREATE_UPDATE_CUSTOMIZATION", jsonString);
        }
    }
}
