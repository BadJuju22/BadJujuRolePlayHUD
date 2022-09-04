using BadJujuRPHUD.Models;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BadJujuRPHUD.Commands
{
    class CommandReg : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "loc";

        public string Help => "";

        public string Syntax => "/loc <add> <regionRadius> <regionName> | /reg <remove> <regionId>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { "command.loc" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var uPlayer = (UnturnedPlayer)caller;

            if (command.Length < 2 || command.Length > 3 || command[0].ToLower() != "add" && command[0].ToLower() != "remove")
            {
                UnturnedChat.Say(uPlayer, Plugin.Instance.Translate("syntax", Syntax), Color.red);
                return;
            }

            if (command[0].ToLower() == "remove")
            {
                if (!uPlayer.HasPermission("reg.remove"))
                {
                    UnturnedChat.Say(uPlayer, Plugin.Instance.Translate("dont_have_perm_to_subcommand"), Color.red);
                    return;
                }

                if (!uint.TryParse(command[1], out uint regionId))
                {
                    UnturnedChat.Say(uPlayer, Plugin.Instance.Translate("incorrect", "regionId"), Color.red);
                    return;
                }

                var region = Plugin.Instance.Configuration.Instance.Regions.Find(x => x.Id == regionId);

                if (region == null)
                {
                    UnturnedChat.Say(uPlayer, Plugin.Instance.Translate("region_not_found", regionId), Color.red);
                    return;
                }

                UnturnedChat.Say(uPlayer, Plugin.Instance.Translate("remove_region", region.Id, region.Name), Color.yellow);
                Plugin.Instance.Configuration.Instance.Regions.Remove(region);
                Plugin.Instance.Configuration.Save();
            }
            else
            {
                if (!uPlayer.HasPermission("reg.add"))
                {
                    UnturnedChat.Say(uPlayer, Plugin.Instance.Translate("dont_have_perm_to_subcommand"), Color.red);
                    return;
                }

                var region = Plugin.Instance.Configuration.Instance.Regions.Find(x => Vector3.Distance(x.ToVector3(), uPlayer.Position) <= x.Radius);

                if (region != null)
                {
                    UnturnedChat.Say(uPlayer, Plugin.Instance.Translate("region_overlap", region.Id, region.Name), Color.red);
                    return;
                }

                if (!float.TryParse(command[1], out float regionRadius) || regionRadius < 1)
                {
                    UnturnedChat.Say(uPlayer, Plugin.Instance.Translate("incorrect", "radius"), Color.red);
                    return;
                }

                region = new Region
                {
                    Id = (uint)Plugin.Instance.Configuration.Instance.Regions.Count,
                    Name = command[2],
                    Center_X = uPlayer.Position.x,
                    Center_Y = uPlayer.Position.y,
                    Center_Z = uPlayer.Position.z,
                    Radius = regionRadius,

                };

                Plugin.Instance.Configuration.Instance.Regions.Add(region);
                Plugin.Instance.Configuration.Save();

                UnturnedChat.Say(uPlayer, Plugin.Instance.Translate("add_region", region.Id, region.Name, region.Radius), Color.yellow);
            }
        }
    }
}
