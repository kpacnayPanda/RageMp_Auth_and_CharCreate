using System.Collections.Generic;
using System.Data;
using GTANetworkAPI;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;

namespace Server
{
    public class RemoteEvents : Script
    {
        [RemoteEvent("CLIENT:SERVER::CLIENT_CREATE_WAYPOINT")]
        public void OnClientCreateWaypoint(Player player, float posX, float posY, float posZ)
        {
            player.Position = new Vector3(posX, posY, posZ);
        }

        [RemoteEvent("CLIENT:SERVER::REGISTER_BUTTON_CLICKED")]
        public async void OnCefRegisterButtonClicked(Player player, string username, string password, string email)
        {
            const string selectQuery = "SELECT * FROM users WHERE username = @name";
            MySqlCommand selectCommand = new MySqlCommand(selectQuery);
            selectCommand.Parameters.AddWithValue("@name", username);

            DataTable tb = await MySQL.QueryReadAsync(selectCommand);
            if (tb.Rows.Count > 0)
            {
                NAPI.Task.Run(() =>
                {
                    NAPI.ClientEvent.TriggerClientEvent(player, "SERVER:CLIENT::REGISTER_USER", true);
                }, 1000);
            }
            else
            {
                string hashedPassword = Crypto.HashPassword(password);

                const string insertQuery = "INSERT INTO users (username, password, email) VALUES (@name, @password, @email)";
                MySqlCommand insertCommand = new MySqlCommand(insertQuery);
                insertCommand.Parameters.AddWithValue("@name", username);
                insertCommand.Parameters.AddWithValue("@password", hashedPassword);
                insertCommand.Parameters.AddWithValue("@email", email);
                MySQL.Query(insertCommand);
                NAPI.Task.Run(() =>
                {
                    NAPI.ClientEvent.TriggerClientEvent(player, "SERVER:CLIENT::REGISTER_USER", false);
                    player.Position = new Vector3(-1642.6818, -1092.0542, 13.052933);
                    player.Rotation = new Vector3(0, 0, 50.111305);
                    player.Dimension = player.Id;
                    //SetPersonCustomization(player, 21, 0, 0.5f, 0.5f);
                    player.SetData<string>("player_username", username);
                }, 1000);
            }
        }

        [RemoteEvent("CLIENT:SERVER::LOGIN_BUTTON_CLICKED")]
        public async void OnCefLoginButtonClicked(Player player, string username, string password)
        {
            string selectQuery = "SELECT * FROM users WHERE username = @name";
            MySqlCommand selectCommand = new MySqlCommand(selectQuery);
            selectCommand.Parameters.AddWithValue("@name", username);
            DataTable tb = await MySQL.QueryReadAsync(selectCommand);
            if (tb.Rows.Count == 0)
            {
                NAPI.Task.Run(() =>
                {
                    NAPI.ClientEvent.TriggerClientEvent(player, "SERVER:CLIENT::LOGIN_USER", true);
                }, 1000); 
            }
            else
            {
                string hashedPassword = Crypto.HashPassword(password);
                string outUsername = tb.Rows[0].ItemArray[1].ToString();
                string outHashedPassword = tb.Rows[0].ItemArray[2].ToString();
                string outEmail = tb.Rows[0].ItemArray[3].ToString();
                string outPersonName = tb.Rows[0].ItemArray[4].ToString();

                if (outHashedPassword != hashedPassword)
                {
                    NAPI.Task.Run(() =>
                    {
                        NAPI.ClientEvent.TriggerClientEvent(player, "SERVER:CLIENT::LOGIN_USER", true);
                    }, 1000);
                }
                else
                {
                    if (string.IsNullOrEmpty(outPersonName))
                    {
                        NAPI.Task.Run(() =>
                        {
                            NAPI.ClientEvent.TriggerClientEvent(player, "SERVER:CLIENT::REGISTER_USER", false);
                            player.Position = new Vector3(-1642.6818, -1092.0542, 13.052933);
                            player.Rotation = new Vector3(0, 0, 50.111305);
                            player.Dimension = player.Id;
                            SetPersonCustomization(player, 21, 0, 0.5f, 0.5f);
                            player.SetData<string>("player_username", username);
                        }, 1000);
                    }
                    else
                    {
                        NAPI.Task.Run(() =>
                        {
                            NAPI.ClientEvent.TriggerClientEvent(player, "SERVER:CLIENT::LOGIN_USER", false);
                            player.SetData<string>("player_username", outUsername);

                        }, 1000);
                    }
                }
            }
        }

        [RemoteEvent("CLIENT:SERVER::PERSON_CREATE_BUTTON_CLICKED")]
        public void OnCefPersonCreateButtonClicked(Player player, string name, string secondName, string age, string gender)
        {
            if(player.HasData("player_username"))
            {       
                string username = player.GetData<string>("player_username");

                string updateQuery = "UPDATE users SET name = @name, sName = @sName, age = @age, gender = @gender WHERE username = @username";
                MySqlCommand updateCommand = new MySqlCommand(updateQuery);

                updateCommand.Parameters.AddWithValue("@name", name);
                updateCommand.Parameters.AddWithValue("@sName", secondName);
                updateCommand.Parameters.AddWithValue("@age", age);
                updateCommand.Parameters.AddWithValue("@gender", gender);
                updateCommand.Parameters.AddWithValue("@username", username);

                MySQL.Query(updateCommand);

                NAPI.Task.Run(() =>
                {
                    NAPI.ClientEvent.TriggerClientEvent(player, "SERVER:CLIENT::CREATE_PERSON");
                }, 1000);
            }
            else
            {
                NAPI.Util.ConsoleOutput("not has");
            }
        }

        [RemoteEvent("CLIENT:SERVER::PERSON_CREATE_GENDER_SWITCH_BUTTON_CLICKED")]
        public void OnCefPersonCreateGenderSwitchButtonClicked(Player player, string gender)
        {
            NAPI.Player.SetPlayerSkin(player, gender.ToLower() == "male" ? PedHash.FreemodeMale01 : PedHash.FreemodeFemale01);
            NAPI.Task.Run(() =>
            {
                NAPI.ClientEvent.TriggerClientEvent(player, "SERVER:CLIENT::UPDATE_SAVED_CUSTOMIZATION", gender);
            }, 10);
        }

        [RemoteEvent("CLIENT:SERVER::PERSON_CREATE_UPDATE_CUSTOMIZATION")]
        public void OnCefPersonCreateUpdateCustomization(Player player, string jsonString)
        {
            dynamic customizationInfo = JObject.Parse(jsonString);

            byte first = (byte)customizationInfo.firstParent;
            byte second = (byte)customizationInfo.secondParent;
            float shapeMix = (float)customizationInfo.shapeMix;
            float skinMix = (float)customizationInfo.skinMix;
            SetPersonCustomization(player, first, second, shapeMix, skinMix);
        }

        private static void SetPersonCustomization(Player player, byte first, byte second, float shapeMix, float skinMix)
        {
            HeadBlend headBlend = new HeadBlend()
            {
                ShapeFirst = first,
                ShapeSecond = second,
                ShapeThird = 0,
                SkinFirst = first,
                SkinSecond = second,
                SkinThird = 0,
                ShapeMix = shapeMix,
                SkinMix = skinMix,
                ThirdMix = 0
            };

            float[] faceFeatures = new float[20]
            {
                0, 0, 0, 0, 0,
                0, 0, 0, 0, 0,
                0, 0, 0, 0, 0,
                0, 0, 0, 0, 0
            };

            Dictionary<int, HeadOverlay> headOverlays = new Dictionary<int, HeadOverlay>();

            player.SetCustomization(true, headBlend, byte.MinValue, byte.MinValue, byte.MinValue, faceFeatures, headOverlays, new Decoration[] { });
        }
    }
}
