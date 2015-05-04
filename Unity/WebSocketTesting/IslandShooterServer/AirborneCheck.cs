using System;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace IslandShooterServer
{
    public class AirborneCheck : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            var name = Context.QueryString["name"];
            var msg = !name.IsNullOrEmpty() ? String.Format("'{0}' to {1}", e.Data, name) : e.Data;
            
            string s = Encoding.UTF8.GetString(e.RawData);
            if (s == "YES") msg = "PLAYER IS AIRBORNE";
            else if (s == "NO") msg = "PLAYER IS GROUNDED";
            
            Send(msg);
        }
    }
}
