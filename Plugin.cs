using BadJujuRPHUD.Models;
using BadJujuRPHUD.Services;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Framework.Utilities;
using SDG.NetTransport;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Threading;
using UnityEngine;

namespace BadJujuRPHUD
{
    public class Plugin : RocketPlugin<Configuration>
    {

        public static Plugin Instance;
        public static Configuration Conf;
        public string Namee = "BadJujuRPHUD";
        private DateTime lastcalled;
        private DateTime lastcalled2;
        private DateTime lastcalled3;

        protected override void Load()
        {
            Instance = this;
  
            Conf = Configuration.Instance;
            Rocket.Core.Logging.Logger.Log("#      Plugin created by BadJuju       #", ConsoleColor.Yellow);
            Rocket.Core.Logging.Logger.Log("#      Plugin Version: 1.0.0.0         #", ConsoleColor.Yellow);
  
            Rocket.Core.Logging.Logger.Log(Namee + " is successfully loaded!", ConsoleColor.Green);
            U.Events.OnPlayerConnected += OnConnect;
            UnturnedPlayerEvents.OnPlayerUpdateHealth += OnHealth;
            UnturnedPlayerEvents.OnPlayerUpdateFood += OnFood;
            UnturnedPlayerEvents.OnPlayerUpdateWater += OnWater;
            UnturnedPlayerEvents.OnPlayerUpdateVirus += OnVirus;
            UnturnedPlayerEvents.OnPlayerUpdateExperience += OnExperience;
            PlayerVoice.onRelayVoice += OnVoice;
            UnturnedPlayerEvents.OnPlayerUpdateBleeding += OnBleed;
            UnturnedPlayerEvents.OnPlayerUpdateBroken += OnBroken;
            U.Events.OnPlayerDisconnected += OnDisconnect;
            UnturnedPlayerEvents.OnPlayerUpdatePosition += OnPos;
            UnturnedPlayerEvents.OnPlayerRevive += OnRevive;
            VehicleManager.onEnterVehicleRequested += OnVehicleEnter;
            VehicleManager.onExitVehicleRequested += OnVehicleExit;
        

        }

        private void OnVehicleExit(Player player, InteractableVehicle vehicle, ref bool shouldAllow, ref Vector3 pendingLocation, ref float pendingYaw)
        {
            UnturnedPlayer uPlayer = UnturnedPlayer.FromPlayer(player);
            EffectManager.askEffectClearByID(Conf.EffectCar, GetPlayer(uPlayer));

        }

        private void OnVehicleEnter(Player player, InteractableVehicle vehicle, ref bool shouldAllow)
        {
            UnturnedPlayer uPlayer = UnturnedPlayer.FromPlayer(player);
            EffectManager.sendUIEffect(Conf.EffectCar, Conf.EffectKeyCar, GetPlayer(uPlayer), true);
        }

        private void OnPos(UnturnedPlayer player, Vector3 position)
        {
            EffectManager.sendUIEffectText(Conf.EffectKey, GetPlayer(player), true, "in_location", RegionHelper.GetNearestRegion(player).Name);
        }

        private void OnVirus(UnturnedPlayer player, byte virus)
        {
            if (virus < 5)
            {
                NotifyHelper.SetNotify(player, Notify.RADIATION);
            }
            else if (virus > 5)
            {
                EffectManager.sendUIEffectVisibility(Conf.EffectKey, GetPlayer(player), true, "notify_4", false);
            }
        }

        public void SetOnlineUI(UnturnedPlayer uPlayer)
        {
            EffectManager.sendUIEffectText(Conf.EffectKey, GetPlayer(uPlayer), true, "active_online_text", Provider.clients.Count() + "/" + Provider.maxPlayers);
        }
        private void OnDisconnect(UnturnedPlayer player)
        {
            foreach (SteamPlayer client in Provider.clients)
            {
                UnturnedPlayer uPlayer = UnturnedPlayer.FromSteamPlayer(client);
                SetOnlineUI(uPlayer);
            }
          
        }

        private void OnVoice(PlayerVoice speaker, bool wantsToUseWalkieTalkie, ref bool shouldAllow, ref bool shouldBroadcastOverRadio, ref PlayerVoice.RelayVoiceCullingHandler cullingHandler)
        {
          
            UnturnedPlayer uPlayer = UnturnedPlayer.FromPlayer(speaker.player);
      
             
               
                EffectManager.sendUIEffectVisibility(Conf.EffectKey, GetPlayer(uPlayer), true, "mcr_online", true);
                EffectManager.sendUIEffectVisibility(Conf.EffectKey, GetPlayer(uPlayer), true, "mcr_offline", false);
            
            
        }
        public void VehicleUpdate()
        {
            if ((DateTime.Now - this.lastcalled).TotalSeconds <= 0.1)
                return;
            this.lastcalled = DateTime.Now;
            try
            {
                foreach (SteamPlayer client in Provider.clients)
                {
                    UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromSteamPlayer(client);
                    if (!unturnedPlayer.Player.voice.isTalking && (DateTime.Now - this.lastcalled3).TotalSeconds >= 0.5)
                    {
                        EffectManager.sendUIEffectVisibility(Conf.EffectKey, GetPlayer(unturnedPlayer), true, "mcr_online", false);
                        EffectManager.sendUIEffectVisibility(Conf.EffectKey, GetPlayer(unturnedPlayer), true, "mcr_offline", true);
                        lastcalled3 = DateTime.Now;
                    }
                    if (!unturnedPlayer.IsInVehicle)
                        continue;
                    InteractableVehicle veh = unturnedPlayer.CurrentVehicle;
                    EffectManager.sendUIEffectText(Conf.EffectKeyCar, GetPlayer(unturnedPlayer), true, "speedometer", veh.speed.ToString()+"KM/H");
                    EffectManager.sendUIEffectText(Conf.EffectKeyCar, GetPlayer(unturnedPlayer), true, "quality_text", ((veh.health/veh.asset.health)*100).ToString()+"%");
                    EffectManager.sendUIEffectText(Conf.EffectKeyCar, GetPlayer(unturnedPlayer), true, "gas_text", ((veh.fuel / veh.asset.fuel) * 100).ToString() + "%");
                    EffectManager.sendUIEffectText(Conf.EffectKeyCar, GetPlayer(unturnedPlayer), true, "charge_text", ((veh.batteryCharge/10000*100).ToString() + "%"));
               
                    if (veh.isLocked)
                    {
                        EffectManager.sendUIEffectVisibility(Conf.EffectKey, GetPlayer(unturnedPlayer), true, "key", true);
                    }
                    else
                    {
                        EffectManager.sendUIEffectVisibility(Conf.EffectKey, GetPlayer(unturnedPlayer), true, "key", false);
                    }
                    if (veh.taillightsOn)
                    {
                        EffectManager.sendUIEffectVisibility(Conf.EffectKey, GetPlayer(unturnedPlayer), true, "battery", true);
                    }
                    else
                    {
                        EffectManager.sendUIEffectVisibility(Conf.EffectKey, GetPlayer(unturnedPlayer), true, "battery", false);
                    }
                }

                
            }
            catch
            {
            }


        }
        public void TimeUpdate()
        {
            if ((DateTime.Now - this.lastcalled2).TotalSeconds <= 1.0)
                return;
            this.lastcalled2 = DateTime.Now;
            try
            {
                foreach (SteamPlayer client in Provider.clients)
                {
                    UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromSteamPlayer(client);
                    EffectManager.sendUIEffectText(Conf.EffectKey, GetPlayer(unturnedPlayer), true, "datetimenow", DateTime.Now.ToString());
                }
            }
            catch
            {

            }
        }
                    public void FixedUpdate()
        {
            VehicleUpdate();
            TimeUpdate();
        }

        private void OnExperience(UnturnedPlayer player, uint experience)
        {
            EffectManager.sendUIEffectText(Conf.EffectKey, GetPlayer(player), true, "player_balance", "$"+ " " +player.Experience.ToString());
        }

        private void OnBroken(UnturnedPlayer player, bool broken)
        {
            if (broken)
            {
                NotifyHelper.SetNotify(player, Notify.BONE);
            }
            else
            {
                EffectManager.sendUIEffectVisibility(Conf.EffectKey, GetPlayer(player), true, "notify_1", false);
            }
        }

        private void OnBleed(UnturnedPlayer player, bool bleeding)
        {
            if (bleeding)
            {
                NotifyHelper.SetNotify(player, Notify.BLOOD);
            }
            else
            {
                EffectManager.sendUIEffectVisibility(Conf.EffectKey, GetPlayer(player), true, "notify_0", false);
            }
        }

        private void OnConnect(UnturnedPlayer player)
        {
            EffectManager.sendUIEffect(Conf.EffectID, Conf.EffectKey, this.GetPlayer(player), true);
            PlayerLife life = player.Player.life;
            life.onOxygenUpdated = (OxygenUpdated)Delegate.Combine(life.onOxygenUpdated, new OxygenUpdated(delegate (byte oxygen)
            {
                this.OnPlayerOxygenUpdated(oxygen, player);
            }));
            player.Player.setPluginWidgetFlag((EPluginWidgetFlags)2016, false);
            player.Player.setPluginWidgetFlag((EPluginWidgetFlags)2048, false);
            player.Player.setPluginWidgetFlag(EPluginWidgetFlags.ShowVehicleStatus, false);
            this.UpdaterHud(player, "heal", player.Player.life.health);
            this.UpdaterHud(player, "food", player.Player.life.food);
            this.UpdaterHud(player, "water", player.Player.life.water);
            EffectManager.sendUIEffectText(Conf.EffectKey, GetPlayer(player), true, "player_balance", "$" + " "+  player.Experience.ToString());
            EffectManager.sendUIEffectText(Conf.EffectKey, GetPlayer(player), true, "player_name", player.DisplayName);
            EffectManager.sendUIEffectText(Conf.EffectKey, GetPlayer(player), true, "in_location", RegionHelper.GetNearestRegion(player).Name);
            EffectManager.sendUIEffectText(Conf.EffectKey, GetPlayer(player), true, "datetimenow", DateTime.Now.ToString());
         for(int i = 0; i<5; i++)
            {
                EffectManager.sendUIEffectVisibility(Conf.EffectKey, GetPlayer(player), true, $"notify_{i}", false);
            }
        
            SetOnlineUI(player);
            foreach(SteamPlayer client in Provider.clients)
            {
                UnturnedPlayer uPlayer = UnturnedPlayer.FromSteamPlayer(client);
                SetOnlineUI(uPlayer);
            }


        }

        private void OnPlayerOxygenUpdated(byte oxygen, UnturnedPlayer player)
        {
            if (oxygen < 5)
            {
                NotifyHelper.SetNotify(player, Notify.WATER);
            }
            else if (oxygen > 5)
            {
                EffectManager.sendUIEffectVisibility(Conf.EffectKey, GetPlayer(player), true, "notify_5", false);
            }
        }

        private void OnHealth(UnturnedPlayer player, byte health)
        {
            this.UpdaterHud(player, "heal", health);
        }

        private void OnFood(UnturnedPlayer player, byte food)
        {
            this.UpdaterHud(player, "food", food);
            if (food < 5)
            {
                NotifyHelper.SetNotify(player, Notify.FOOD);
            }
            else if (food > 5)
            {
                EffectManager.sendUIEffectVisibility(Conf.EffectKey, GetPlayer(player), true, "notify_3", false);
            }
        }

        private void OnWater(UnturnedPlayer player, byte water)
        {
            this.UpdaterHud(player, "water", water);
            if(water < 5)
            {
                NotifyHelper.SetNotify(player, Notify.WATER);
            }
            else if( water>5)
            {
                EffectManager.sendUIEffectVisibility(Conf.EffectKey, GetPlayer(player), true, "notify_2", false);
            }
        }

     

        private void OnRevive(UnturnedPlayer player, Vector3 position, byte angle)
        {
            this.UpdaterHud(player, "heal", player.Player.life.health);
            this.UpdaterHud(player, "food", player.Player.life.food);
            this.UpdaterHud(player, "water", player.Player.life.water);
       


        }

        public ITransportConnection GetPlayer(UnturnedPlayer _uplayer)
        {
            return _uplayer.Player.channel.GetOwnerTransportConnection();
        }

        private void UpdaterHud(UnturnedPlayer _player, string _name, byte _volume)
        {
            for (int i = 0; i <= 100; i++)
            {
                bool flag = i != (int)_volume;
                if (flag)
                {
                    EffectManager.sendUIEffectVisibility(Conf.EffectKey, this.GetPlayer(_player), true, string.Format("{0}_slider_{1}", _name, i), false);
                }
                else
                {
             
                    EffectManager.sendUIEffectVisibility(Conf.EffectKey, this.GetPlayer(_player), true, string.Format("{0}_slider_{1}", _name, _volume), true);
                }
            }
        }

        protected override void Unload()
        {



            U.Events.OnPlayerConnected -= OnConnect;
            UnturnedPlayerEvents.OnPlayerUpdateHealth -= OnHealth;
            UnturnedPlayerEvents.OnPlayerUpdateFood -= OnFood;
            UnturnedPlayerEvents.OnPlayerUpdateWater -= OnWater;
            UnturnedPlayerEvents.OnPlayerUpdateVirus -= OnVirus;
            UnturnedPlayerEvents.OnPlayerUpdateExperience -= OnExperience;
            PlayerVoice.onRelayVoice -= OnVoice;
            UnturnedPlayerEvents.OnPlayerUpdateBleeding -= OnBleed;
            UnturnedPlayerEvents.OnPlayerUpdateBroken -= OnBroken;
            U.Events.OnPlayerDisconnected -= OnDisconnect;
            UnturnedPlayerEvents.OnPlayerUpdatePosition -= OnPos;
            UnturnedPlayerEvents.OnPlayerRevive -= OnRevive;
            VehicleManager.onEnterVehicleRequested -= OnVehicleEnter;
            VehicleManager.onExitVehicleRequested -= OnVehicleExit;
            Rocket.Core.Logging.Logger.Log(Namee + " is successfully unloaded!", ConsoleColor.Green);
        }
        public override TranslationList DefaultTranslations => new TranslationList
    {
          {"notify_oxy", "Вы задыхаетесь"},
            {"notify_water", "Вы мучаетесь от жажды"},
            {"notify_radiation", "Вы отравились"},
            {"notify_blood", "У вас кровотечение"},
            {"notify_bone", "Вы сломали ногу"},
            {"notify_food", "Вы проголодались"},
            { "syntax", "Используйте данный синтаксис: {0}" },
            { "incorrect", "Некорректно введён [{0}]." },
            { "region_not_found", "Регион с таким regionId: [{0}] не найден." },
            { "remove_region", "Вы удалили регион: [{0}/{1}]." },
            { "region_overlap", "Регион, что вы пытаетесь создать пересекается с регионом: [{0}/{1}]." },
            { "add_region", "Вы создали регион: [{0}/{1}], его радиус: [{2}]m." },
            { "dont_have_perm_to_subcommand", "Вы не имеете разрешения для использования данной подкоманды." }

    };

    }
}



