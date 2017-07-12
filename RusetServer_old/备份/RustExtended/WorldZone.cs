namespace RustExtended
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [Serializable]
    public class WorldZone
    {
        public Vector2 Center;
        public ZoneFlags Flags;
        public string[] ForbiddenCommand;
        public List<WorldZone> Internal;
        public string[] Message_OnEnter;
        public string[] Message_OnLeave;
        public string Name;
        public string Notice_OnEnter;
        public string Notice_OnLeave;
        public ClanData Owned;
        public List<Vector2> Points;
        public List<Vector3> Spawns;
        public long WarpTime;
        public WorldZone WarpZone;

        public WorldZone(string name, ZoneFlags flags)
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
                this.Flags = (ZoneFlags) Enum.Parse(typeof(ZoneFlags), flags);
            }
        }

        public bool CanTrade
        {
            get
            {
                return ((this.Flags & ZoneFlags.trade) == ZoneFlags.trade);
            }
        }

        public string Defname
        {
            get
            {
                return Zones.GetDefname(this);
            }
        }

        public bool NoBuild
        {
            get
            {
                return ((this.Flags & ZoneFlags.nobuild) == ZoneFlags.nobuild);
            }
        }

        public bool NoCraft
        {
            get
            {
                return ((this.Flags & ZoneFlags.nocraft) == ZoneFlags.nocraft);
            }
        }

        public bool NoDecay
        {
            get
            {
                return ((this.Flags & ZoneFlags.nodecay) == ZoneFlags.nodecay);
            }
        }

        public bool NoEnter
        {
            get
            {
                return ((this.Flags & ZoneFlags.noenter) == ZoneFlags.noenter);
            }
        }

        public bool NoLeave
        {
            get
            {
                return ((this.Flags & ZoneFlags.noleave) == ZoneFlags.noleave);
            }
        }

        public bool NoPvP
        {
            get
            {
                return ((this.Flags & ZoneFlags.nopvp) == ZoneFlags.nopvp);
            }
        }

        public bool NoSleepers
        {
            get
            {
                return ((this.Flags & ZoneFlags.nosleep) == ZoneFlags.nosleep);
            }
        }

        public bool Radiation
        {
            get
            {
                return ((this.Flags & ZoneFlags.radiation) == ZoneFlags.radiation);
            }
        }

        public bool Safe
        {
            get
            {
                return ((this.Flags & ZoneFlags.safe) == ZoneFlags.safe);
            }
        }

        public bool Warp
        {
            get
            {
                return (this.WarpZone != null);
            }
        }
    }
}

