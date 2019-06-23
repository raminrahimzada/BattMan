using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace BattMan
{
    public class Setting
    {
        private const string ConfigFile = "config.json";

        public static Setting Instance
        {
            get
            {
                try
                {
                    var json = File.ReadAllText(ConfigFile);
                    return JsonConvert.DeserializeObject<Setting>(json);
                }
                catch
                {
                    var result=new Setting()
                    {
                        MinBatteryLevel = 26,
                        MaxBatteryLevel = 96,
                        MinBatteryAlertMessage= "Adapteri Qoşun",
                        MaxBatteryAlertMessage = "Adapteri çıxardın"
                    };
                    var json = JsonConvert.SerializeObject(result,Formatting.Indented);
                    File.WriteAllText(ConfigFile, json);
                    return result;
                }
            }
        }

        public int MinBatteryLevel { get; set; }
        public int MaxBatteryLevel { get; set; }
        public string MinBatteryAlertMessage { get; set; }
        public string MaxBatteryAlertMessage { get; set; }
    }
}