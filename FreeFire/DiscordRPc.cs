using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordRPC;
using System.Web.UI.WebControls;
using KeyAuth;
using free;
namespace Loader
{
    internal class DiscordRPc
    {
        public static DiscordRpcClient client;
        public static Timestamps rpctimestamp { get; set; }
        private static RichPresence presence;

        public static void InitializeRPC()
        {
            client = new DiscordRpcClient("YOUR_DISCORD_ID");
            client.Initialize();

            // Set up buttons with appropriate URLs
            DiscordRPC.Button[] buttons = {
                new DiscordRPC.Button() { Label = "Free Panel", Url = "https://discord.gg/zkqXxAhNpq" },
                new DiscordRPC.Button() { Label = "Youtube", Url = "" }
            };

            // Set up the initial presence
            presence = new RichPresence()
            {
                Buttons = buttons,
                Timestamps = rpctimestamp,

                Assets = new Assets()
                {
                    LargeImageKey = "https://i.postimg.cc/021nmx1G/FLUNIX.png",
                    LargeImageText = "FLL",
                    SmallImageKey = "https://cdn3.emoji.gg/emojis/7177-red-verify.gif",
                    SmallImageText = ""
                }
            };

            client.SetPresence(presence);
            UpdateDiscordPresence();
        }

        public static void SetState(string state, bool watching = false)
        {
            if (watching)
                state = "Looking at " + state;

            presence.State = state;
            client.SetPresence(presence);
        }

        private static void UpdateDiscordPresence()
        {
            // Check if the user is logged in before accessing the username and expiry date
            if (Form1.KeyAuthApp.user_data != null)
            {
                string username = Form1.KeyAuthApp.user_data.username;
                DateTime expiryDateTime = UnixTimeToDateTime(long.Parse(Form1.KeyAuthApp.user_data.subscriptions[0].expiry));

                presence.Details = $"User: {username}";
                presence.State = $"Expiry Date: {expiryDateTime:yyyy-MM-dd HH:mm}";
            }
            else
            {
                presence.Details = "USER";
                presence.State = "";
            }

            client.SetPresence(presence);
        }

        private static DateTime UnixTimeToDateTime(long unixTime)
        {
            DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return unixStart.AddSeconds(unixTime).ToLocalTime();
        }
    }
}

