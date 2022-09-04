using BadJujuRPHUD.Models;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BadJujuRPHUD.Services
{
	public static class RegionHelper
	{
		public static Region GetNearestRegion(UnturnedPlayer uPlayer)
		{
			var regions = Plugin.Instance.Configuration.Instance.Regions;


			regions.Sort((a, b) => Vector3.Distance(uPlayer.Position, a.ToVector3()).CompareTo(Vector3.Distance(uPlayer.Position, b.ToVector3())));

			return regions.FirstOrDefault();
		}
	}
}
