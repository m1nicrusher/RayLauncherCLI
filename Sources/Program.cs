using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Diagnostics;

namespace RayLauncherCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to RayLauncher!");
            Console.WriteLine();
            Launch(Environment.CurrentDirectory + "/bin/Debug/netcoreapp3.0/TestEnv", "1.12.2", "Test", "2048",
             Environment.CurrentDirectory + "/bin/Debug/netcoreapp3.0/TestEnv/runtimes/jre1.8.0_221/bin/java", "1920", "1080");
        }
        private static readonly string forgeVer = "1.12.2-forge1.12.2-14.23.4.2759";
        private static void Launch(string gamePath, string version, string playerName, string maxMemory, string javaPath, string rx, string ry, string uuid = "628628", string token = "88888888", string userType = "legacy")
        {
            string PATH = $@"{gamePath}/libraries";
            string value = File.ReadAllText($@"{gamePath}/versions/1.12.2/1.12.2.json");
            JObject a = (JObject)JsonConvert.DeserializeObject(value);//result
            JArray arr = (JArray)a["libraries"];
            string ylx = "";
            foreach (JObject item in arr)
            {
                string ori = (string)item["name"];
                string left = ori.Substring(0, ori.LastIndexOf(":"));
                string path = left.Replace('.', '/').Replace(':', '/');
                left = left.Substring(left.LastIndexOf(":") + 1);
                string right = ori.Substring(ori.LastIndexOf(":") + 1);
                path += "/" + right;
                string full = $"{PATH}/{path}/{left}-{right}";
                string add = "-natives-windows";
                if (!File.Exists(full + ".jar"))
                    full += add;
                //Console.WriteLine(full + ".jar");
                if (File.Exists(full + ".jar"))
                    ylx += ";" + full + ".jar";
            }

            string valueForge = File.ReadAllText(($@"{gamePath}/versions/{forgeVer}/{forgeVer}.json"));
            JObject aForge = (JObject)JsonConvert.DeserializeObject(valueForge);//result
            JArray arrForge = (JArray)aForge["libraries"];
            foreach (JObject item in arrForge)
            {
                string ori = (string)item["name"];
                string left = ori.Substring(0, ori.LastIndexOf(":"));
                string path = ori.Substring(0, ori.IndexOf(":")).Replace('.', '/');
                path += ori.Substring(ori.IndexOf(":")).Replace(':', '/');
                left = left.Substring(left.LastIndexOf(":") + 1);
                string right = ori.Substring(ori.LastIndexOf(":") + 1);
                string full = $"{PATH}/{path}/{left}-{right}";
                string add = "-natives-windows";
                if (!File.Exists(full + ".jar"))
                    full += add;
                //Console.WriteLine(full + ".jar");
                //Console.WriteLine(File.Exists(full + ".jar") ? "存在" : "不存在");
                if (File.Exists(full + ".jar"))
                    ylx += ":" + full + ".jar";
            }


            ylx = ylx.Substring(1);
            //Console.WriteLine(ylx);
            string arg = $"{javaPath} -Xmx{maxMemory}m -Dfml.ignoreInvalidMinecraftCertificates=true -Dfml.ignorePatchDiscrepancies=true -Djava.library.path=\"natives\" ";
            arg += $"-cp \"versions/{version}/{version}.jar;{ylx}\" ";
            arg += $"{aForge["mainClass"]} ";
            arg += $@"--username {playerName} --version {version} --gameDir / --assetsDir assets --assetIndex 1.12 --uuid " + uuid + " --accessToken " + token + " --userType " + userType + " --tweakClass net.minecraftforge.fml.common.launcher.FMLTweaker --versionType Forge" + 
                $" --height {ry} --width {rx}";
            Console.WriteLine("arg: " + arg);
            Process p = new Process();
            p.StartInfo.FileName = "bash";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = false;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine($@"cd {gamePath}");

            p.StandardInput.WriteLine(arg);
            p.StandardInput.WriteLine("exit");
            Environment.Exit(0);
        }
    }
}
