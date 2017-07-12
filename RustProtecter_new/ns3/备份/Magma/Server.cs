namespace Magma
{
    using System;
    using System.Collections.Generic;
    using RustExtended;

    public class Server
    {
        private ItemsBlocks _items;
        public Magma.Data data = new Magma.Data();
        private static Magma.Server magmaServer;
        private System.Collections.Generic.List<Magma.Player> players = new System.Collections.Generic.List<Magma.Player>();
        public string server_message_name = Core.ServerName;
        public Util util = new Util();

        public void Broadcast(string arg)
        {
            foreach (Magma.Player player in this.Players)
            {
                player.Message(arg);
            }
        }

        public void BroadcastFrom(string name, string arg)
        {
            foreach (Magma.Player player in this.Players)
            {
                player.MessageFrom(name, arg);
            }
        }

        public void BroadcastNotice(string s)
        {
            foreach (Magma.Player player in this.Players)
            {
                player.Notice(s);
            }
        }

        public Magma.Player FindPlayer(string s)
        {
            Magma.Player player = Magma.Player.FindBySteamID(s);
            if (player != null)
            {
                return player;
            }
            player = Magma.Player.FindByGameID(s);
            if (player != null)
            {
                return player;
            }
            player = Magma.Player.FindByName(s);
            if (player != null)
            {
                return player;
            }
            return null;
        }

        public static Magma.Server GetServer()
        {
            if (magmaServer == null)
            {
                magmaServer = new Magma.Server();
            }
            return magmaServer;
        }

        public void Save()
        {
            AvatarSaveProc.SaveAll();
            ServerSaveManager.AutoSave();
        }

        public System.Collections.Generic.List<string> ChatHistoryMessages
        {
            get
            {
                return Magma.Data.GetData().chat_history;
            }
        }

        public System.Collections.Generic.List<string> ChatHistoryUsers
        {
            get
            {
                return Magma.Data.GetData().chat_history_username;
            }
        }

        public int FrameRate
        {
            get
            {
                return server.framerate;
            }
        }

        public string Hostname
        {
            get
            {
                return server.hostname;
            }
        }

        public string IP
        {
            get
            {
                return server.ip;
            }
        }

        public ItemsBlocks Items
        {
            get
            {
                return this._items;
            }
            set
            {
                this._items = value;
            }
        }

        public string Map
        {
            get
            {
                return server.map;
            }
        }

        public int MaxPlayers
        {
            get
            {
                return server.maxplayers;
            }
        }

        public System.Collections.Generic.List<Magma.Player> Players
        {
            get
            {
                return this.players;
            }
        }

        public int Port
        {
            get
            {
                return server.port;
            }
        }

        public bool PvP
        {
            get
            {
                return server.pvp;
            }
        }

        public float SendRate
        {
            get
            {
                return server.sendrate;
            }
        }

        public System.Collections.Generic.List<StructureMaster> ServerStructures
        {
            get
            {
                return StructureMaster.AllStructures;
            }
        }
    }
}

