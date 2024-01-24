using CmlLib.Core.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedMinecraftLauncher
{
    internal static class Launcher
    {   
        public static MSession Session { get; private set; }

        public static void LoginOffline(string username)
        {
            Session = MSession.CreateOfflineSession(username);
        }
    }
}
