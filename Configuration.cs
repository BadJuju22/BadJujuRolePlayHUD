using BadJujuRPHUD.Models;
using Rocket.API;
using System.Collections.Generic;

namespace BadJujuRPHUD
{
    public class Configuration : IRocketPluginConfiguration
    {
        public ushort EffectID = 21451;
        public short EffectKey = 21451;
        public ushort EffectCar = 21452;
        public short EffectKeyCar = 21452;
       
        public string FoodURL;
        public string WaterURL;
        public string RadiationURL;
        public string OxyURL;
        public string BoneURL;
        public string BloodURL;
        public List<Region> Regions;
        public void LoadDefaults()
        {
            FoodURL = "www";
            WaterURL = "ddd";
            RadiationURL = "dffg";
            OxyURL = "‡‡‡";
            BoneURL = "Ddd";
            BloodURL = "gkg";

            Regions = new List<Region>();
        }
    }
}