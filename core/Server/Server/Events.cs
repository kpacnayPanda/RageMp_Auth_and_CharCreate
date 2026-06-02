using System.Data;
using GTANetworkAPI;
using MySql.Data.MySqlClient;

namespace Server
{
    public class Events : Script
    {
        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart()
        {
            string query = "SELECT * FROM users";
            using MySqlCommand command = new MySqlCommand(query);
            DataTable dt = MySQL.QueryRead(command);

            //NAPI.Util.ConsoleOutput(dt.Rows[0].ItemArray[1].ToString());
        }

        [ServerEvent(Event.PlayerSpawn)]
        public void OnPlayerSpawn(Player player)
        {
            player.Position = new Vector3(-1847.6251, 4571.128, 5.5569506);
            player.Rotation = new Vector3(0, 0, -9.241396);
            player.Dimension = player.Id;
        }
    }
}