using BadJujuRPHUD.Models;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadJujuRPHUD.Services
{
   public static class NotifyHelper
    {
        public static void SetNotify(UnturnedPlayer uPlayer,Notify not)
        {
         
            string txt = " ";
            if (not == Notify.FOOD)
            {

                txt = Plugin.Instance.Translate("notify_food");
                EffectManager.sendUIEffectVisibility(Plugin.Conf.EffectKey, Plugin.Instance.GetPlayer(uPlayer), true, "notify_3", true);
                EffectManager.sendUIEffectText(Plugin.Conf.EffectKey, Plugin.Instance.GetPlayer(uPlayer), true, "notify_3_txt", txt);
                EffectManager.sendUIEffectImageURL(Plugin.Conf.EffectKey, Plugin.Instance.GetPlayer(uPlayer), true, "notify_3_img", GetURL(Notify.FOOD));
            }
            else if (not == Notify.WATER)
            {
                txt = Plugin.Instance.Translate("notify_water");
                EffectManager.sendUIEffectVisibility(Plugin.Conf.EffectKey, Plugin.Instance.GetPlayer(uPlayer), true, "notify_2", true);
                EffectManager.sendUIEffectText(Plugin.Conf.EffectKey, Plugin.Instance.GetPlayer(uPlayer), true, "notify_2_txt", txt);
                EffectManager.sendUIEffectImageURL(Plugin.Conf.EffectKey, Plugin.Instance.GetPlayer(uPlayer), true, "notify_2_img", GetURL(Notify.WATER));
            }
            else if (not == Notify.OXY)
            {
                txt = Plugin.Instance.Translate("notify_oxy");
                EffectManager.sendUIEffectVisibility(Plugin.Conf.EffectKey, Plugin.Instance.GetPlayer(uPlayer), true, "notify_5", true);
                EffectManager.sendUIEffectText(Plugin.Conf.EffectKey, Plugin.Instance.GetPlayer(uPlayer), true, "notify_5_txt", txt);
                EffectManager.sendUIEffectImageURL(Plugin.Conf.EffectKey, Plugin.Instance.GetPlayer(uPlayer), true, "notify_5_img", GetURL(Notify.OXY));
            }
            else if (not == Notify.RADIATION)
            {
                txt = Plugin.Instance.Translate("notify_radiation");
                EffectManager.sendUIEffectVisibility(Plugin.Conf.EffectKey, Plugin.Instance.GetPlayer(uPlayer), true, "notify_4", true);
                EffectManager.sendUIEffectText(Plugin.Conf.EffectKey, Plugin.Instance.GetPlayer(uPlayer), true, "notify_4_txt", txt);
                EffectManager.sendUIEffectImageURL(Plugin.Conf.EffectKey, Plugin.Instance.GetPlayer(uPlayer), true, "notify_4_img", GetURL(Notify.RADIATION));
            }
            else if (not == Notify.BLOOD)
            {
                txt = Plugin.Instance.Translate("notify_blood");
                EffectManager.sendUIEffectVisibility(Plugin.Conf.EffectKey, Plugin.Instance.GetPlayer(uPlayer), true, "notify_0", true);
                EffectManager.sendUIEffectText(Plugin.Conf.EffectKey, Plugin.Instance.GetPlayer(uPlayer), true, "notify_0_txt", txt);
                EffectManager.sendUIEffectImageURL(Plugin.Conf.EffectKey, Plugin.Instance.GetPlayer(uPlayer), true, "notify_0_img", GetURL(Notify.BLOOD));
            }
            else if (not == Notify.BONE)
            {
                txt = Plugin.Instance.Translate("notify_bone");
                EffectManager.sendUIEffectVisibility(Plugin.Conf.EffectKey, Plugin.Instance.GetPlayer(uPlayer), true, "notify_1", true);
                EffectManager.sendUIEffectText(Plugin.Conf.EffectKey, Plugin.Instance.GetPlayer(uPlayer), true, "notify_1_txt", txt);
                EffectManager.sendUIEffectImageURL(Plugin.Conf.EffectKey, Plugin.Instance.GetPlayer(uPlayer), true, "notify_1_img", GetURL(Notify.BONE));
            }
           
        }
      
        public static string GetURL(Notify not)
        {
            if(not == Notify.FOOD)
            {
                return Plugin.Conf.FoodURL;
            }
            if (not == Notify.WATER)
            {
                return Plugin.Conf.WaterURL;
            }
            if(not == Notify.OXY)
            {
                return Plugin.Conf.OxyURL;
            }
            if (not == Notify.RADIATION)
            {
                return Plugin.Conf.RadiationURL;
            }
            if (not == Notify.BLOOD)
            {
                return Plugin.Conf.BloodURL;
            }
            if (not == Notify.BONE)
            {
                return Plugin.Conf.BoneURL;
            }
            return Plugin.Conf.BoneURL;
        }
    }
}
