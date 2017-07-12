using System;
using System.Collections.Generic;
using UnityEngine;

namespace RustExtended
{
	[Serializable]
	public class WorldZone
	{
		public string Name;

		public List<Vector2> Points;

		public List<Vector3> Spawns;

		public List<WorldZone> Internal;

		public Vector2 Center;

		public ZoneFlags Flags;

		public WorldZone WarpZone;

		public string Notice_OnEnter;

		public string Notice_OnLeave;

		public string[] Message_OnEnter;

		public string[] Message_OnLeave;

		public string[] ForbiddenCommand;

		public ClanData Owned;

		public long WarpTime;

		public string Defname
		{
			get
			{
				return Zones.GetDefname(this);
			}
		}

		public bool Radiation
		{
			get
			{
				return (this.Flags & ZoneFlags.radiation) == ZoneFlags.radiation;
			}
		}

		public bool NoSleepers
		{
			get
			{
				return (this.Flags & ZoneFlags.nosleep) == ZoneFlags.nosleep;
			}
		}

		public bool NoBuild
		{
			get
			{
				return (this.Flags & ZoneFlags.nobuild) == ZoneFlags.nobuild;
			}
		}

		public bool NoCraft
		{
			get
			{
				return (this.Flags & ZoneFlags.nocraft) == ZoneFlags.nocraft;
			}
		}

		public bool NoDecay
		{
			get
			{
				return (this.Flags & ZoneFlags.nodecay) == ZoneFlags.nodecay;
			}
		}

		public bool CanTrade
		{
			get
			{
				return (this.Flags & ZoneFlags.trade) == ZoneFlags.trade;
			}
		}

		public bool NoPvP
		{
			get
			{
				return (this.Flags & ZoneFlags.nopvp) == ZoneFlags.nopvp;
			}
		}

		public bool Safe
		{
			get
			{
				return (this.Flags & ZoneFlags.safe) == ZoneFlags.safe;
			}
		}

		public bool Warp
		{
			get
			{
				return this.WarpZone != null;
			}
		}

		public bool NoEnter
		{
			get
			{
				return (this.Flags & ZoneFlags.noenter) == ZoneFlags.noenter;
			}
		}

		public bool NoLeave
		{
			get
			{
				return (this.Flags & ZoneFlags.noleave) == ZoneFlags.noleave;
			}
		}

		public WorldZone(string name = null, ZoneFlags flags = (ZoneFlags)0)
		{
			this.Name = name;
			this.Flags = flags;
			this.Points = new List<Vector2>();
			this.Spawns = new List<Vector3>();
			this.Internal = new List<WorldZone>();
			this.ForbiddenCommand = new string[0];
			this.Message_OnEnter = new string[0];
			this.Message_OnLeave = new string[0];
			this.WarpZone = null;
			this.WarpTime = 0L;
		}

		public void SetFlags(string flags)
		{
			if (!string.IsNullOrEmpty(flags))
			{
				flags = flags.Trim().Replace(" ", "");
				this.Flags = (ZoneFlags)Enum.Parse(typeof(ZoneFlags), flags);
			}
		}
	}
}
