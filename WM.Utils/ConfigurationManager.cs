using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace WM.Utils
{
    public class ConfigurationManager
    {
        private static string config = "ricerc";
        private static Dictionary<Int64, string> _runMap = new Dictionary<Int64, string>();
        private static Dictionary<Int64, int> _workspaceMap = new Dictionary<Int64, int>();

//        public static List<string, string> BindsListing = ConfigurationManager.BindsListing;
        public ConfigurationManager()
        {
            var reader = File.OpenText(config);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                //split the line by spaces
                uint hotKeyId = 0;
                var parts = Regex.Split(line, "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*) (?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                if (parts[0] == "bind")
                {
                    if (parts.Length < 3)
                    {
                        Console.Out.WriteLine("Bind must have 3 components, skipping");
                        continue;
                    }
                    string[] keys = parts[1].Split('+');
                    if (keys.Length < 2)
                    {
                        Console.Out.WriteLine("Key bind must have a modifier and a key, skipping");
                        continue;
                    }

                    var command = Regex.Split(parts[2], "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*):(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                    if (command.Length > 0)
                    {
                        if (command[0].ToLower() == "run")
                        {
                            _runMap.Add(hotKeyId, command[1]);
                        }
                        else if (command[0].ToLower() == "workspace")
                        {
                            int wsNum = 0;
                            Int32.TryParse(command[1], out wsNum);
                            _workspaceMap.Add(hotKeyId, wsNum);
                        }
                    }
                }
            }

        }

    }
}
