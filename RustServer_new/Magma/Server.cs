using RustExtended;
using System;
using System.Collections.Generic;

namespace Magma
{
	public class Server
	{
		private ItemsBlocks _items;

		public Data data = new Data();

		private List<Player> players = new List<Player>();

		public string server_message_name = Core.ServerName;

		private static Server magmaServer;

		public Util util = new Util();

		public List<string> ChatHistoryMessages
		{
			get
			{
				return Data.GetData().chat_history;
			}
		}

		public List<string> ChatHistoryUsers
		{
			get
			{
				return Data.GetData().chat_history_username;
			}
		}

		public string IP
		{
			get
			{
				return server.ip;
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

		public string Hostname
		{
			get
			{
				return server.hostname;
			}
		}

		public float SendRate
		{
			get
			{
				return server.sendrate;
			}
		}

		public int FrameRate
		{
			get
			{
				return server.framerate;
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

		public List<Player> Players
		{
			get
			{
				return this.players;
			}
		}

		public List<StructureMaster> ServerStructures
		{
			get
			{
				return StructureMaster.AllStructures;
			}
		}

		public void Broadcast(string arg)
		{
			foreach (Player current in this.Players)
			{
				current.Message(arg);
			}
		}

		public void BroadcastFrom(string name, string arg)
		{
			foreach (Player current in this.Players)
			{
				current.MessageFrom(name, arg);
			}
		}

		public void BroadcastNotice(string s)
		{
			foreach (Player current in this.Players)
			{
				current.Notice(s);
			}
		}

		public Player FindPlayer(string s)
		{
			Player player = Player.FindBySteamID(s);
			Player result;
			if (player != null)
			{
				result = player;
			}
			else
			{
				player = Player.FindByGameID(s);
				if (player != null)
				{
					result = player;
				}
				else
				{
					player = Player.FindByName(s);
					if (player != null)
					{
						result = player;
					}
					else
					{
						result = null;
					}
				}
			}
			return result;
		}

		public static Server GetServer()
		{
			if (Server.magmaServer == null)
			{
				Server.magmaServer = new Server();
			}
			return Server.magmaServer;
		}

		public void Save()
		{
			AvatarSaveProc.SaveAll();
			ServerSaveManager.AutoSave();
		}
	}
}
