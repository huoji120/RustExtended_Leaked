using Facepunch.Utility;
using Magma.Events;
using Rust;
using RustExtended;
using System;
using uLink;
using UnityEngine;

namespace Magma
{
	public class Player
	{
		private long connectedAt;

		private PlayerInv inv;

		private bool invError;

		private bool justDied;

		private PlayerClient ourPlayer;

		public bool Admin
		{
			get
			{
				return this.ourPlayer.netUser.admin;
			}
		}

		public string GameID
		{
			get
			{
				return this.ourPlayer.userID.ToString();
			}
		}

		public UserData Data
		{
			get
			{
				return Users.GetBySteamID(this.ourPlayer.netUser.userID);
			}
		}

		public float Health
		{
			get
			{
				return this.PlayerClient.controllable.health;
			}
			set
			{
				this.PlayerClient.controllable.takeDamage.health = value;
				this.PlayerClient.controllable.takeDamage.Heal(this.PlayerClient.controllable, 0f);
			}
		}

		public PlayerInv Inventory
		{
			get
			{
				if (this.invError || this.justDied)
				{
					this.inv = new PlayerInv(this);
					this.invError = false;
					this.justDied = false;
				}
				return this.inv;
			}
		}

		public string IP
		{
			get
			{
				return this.ourPlayer.netPlayer.externalIP;
			}
		}

		public bool IsBleeding
		{
			get
			{
				return this.PlayerClient.controllable.GetComponent<HumanBodyTakeDamage>().IsBleeding();
			}
			set
			{
				this.PlayerClient.controllable.GetComponent<HumanBodyTakeDamage>().SetBleedingLevel((float)Convert.ToInt32(value));
			}
		}

		public bool IsCold
		{
			get
			{
				return this.PlayerClient.controllable.GetComponent<Metabolism>().IsCold();
			}
			set
			{
				this.PlayerClient.controllable.GetComponent<Metabolism>().coreTemperature = (value ? -10f : 10f);
			}
		}

		public bool IsInjured
		{
			get
			{
				return this.PlayerClient.controllable.GetComponent<FallDamage>().GetLegInjury() != 0f;
			}
			set
			{
				this.PlayerClient.controllable.GetComponent<FallDamage>().SetLegInjury((float)Convert.ToInt32(value));
			}
		}

		public Vector3 Location
		{
			get
			{
				return this.ourPlayer.lastKnownPosition;
			}
			set
			{
				this.ourPlayer.transform.position.Set(value.x, value.y, value.z);
			}
		}

		public string Name
		{
			get
			{
				return this.ourPlayer.netUser.user.displayname_;
			}
			set
			{
				this.ourPlayer.netUser.user.displayname_ = value;
				this.ourPlayer.userName = this.ourPlayer.netUser.user.displayname_;
			}
		}

		public int Ping
		{
			get
			{
				return this.ourPlayer.netPlayer.averagePing;
			}
		}

		public PlayerClient PlayerClient
		{
			get
			{
				return this.ourPlayer;
			}
		}

		public string SteamID
		{
			get
			{
				return this.ourPlayer.netUser.userID.ToString();
			}
		}

		public long TimeOnline
		{
			get
			{
				return (DateTime.UtcNow.Ticks - this.connectedAt) / 10000L;
			}
		}

		public float X
		{
			get
			{
				return this.ourPlayer.lastKnownPosition.x;
			}
			set
			{
				this.ourPlayer.transform.position.Set(value, this.Y, this.Z);
			}
		}

		public float Y
		{
			get
			{
				return this.ourPlayer.lastKnownPosition.y;
			}
			set
			{
				this.ourPlayer.transform.position.Set(this.X, value, this.Z);
			}
		}

		public float Z
		{
			get
			{
				return this.ourPlayer.lastKnownPosition.z;
			}
			set
			{
				this.ourPlayer.transform.position.Set(this.X, this.Y, value);
			}
		}

		public Player()
		{
			this.justDied = true;
		}

		public Player(PlayerClient client)
		{
			this.justDied = true;
			this.ourPlayer = client;
			this.connectedAt = DateTime.UtcNow.Ticks;
			this.FixInventoryRef();
		}

		public void Disconnect()
		{
			NetUser netUser = this.ourPlayer.netUser;
			if (netUser.connected && netUser != null)
			{
				netUser.Kick(NetError.NoError, true);
			}
		}

		public Player Find(string search)
		{
			Player player = Player.FindBySteamID(search);
			Player result;
			if (player != null)
			{
				result = player;
			}
			else
			{
				player = Player.FindByGameID(search);
				if (player != null)
				{
					result = player;
				}
				else
				{
					player = Player.FindByName(search);
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

		public static Player FindByGameID(string uid)
		{
			Player result;
			foreach (Player current in Server.GetServer().Players)
			{
				if (current.GameID == uid)
				{
					result = current;
					return result;
				}
			}
			result = null;
			return result;
		}

		public static Player FindByName(string name)
		{
			Player result;
			foreach (Player current in Server.GetServer().Players)
			{
				if (current.Name == name)
				{
					result = current;
					return result;
				}
			}
			result = null;
			return result;
		}

		public static Player FindByNetworkPlayer(uLink.NetworkPlayer np)
		{
			Player result;
			foreach (Player current in Server.GetServer().Players)
			{
				if (current.ourPlayer.netPlayer == np)
				{
					result = current;
					return result;
				}
			}
			result = null;
			return result;
		}

		public static Player FindByPlayerClient(PlayerClient pc)
		{
			Player result;
			foreach (Player current in Server.GetServer().Players)
			{
				if (current.PlayerClient == pc)
				{
					result = current;
					return result;
				}
			}
			result = null;
			return result;
		}

		public static Player FindBySteamID(string uid)
		{
			Player result;
			foreach (Player current in Server.GetServer().Players)
			{
				if (current.SteamID == uid)
				{
					result = current;
					return result;
				}
			}
			result = null;
			return result;
		}

		public void FixInventoryRef()
		{
			Hooks.OnPlayerKilled += new Hooks.KillHandlerDelegate(this.Hooks_OnPlayerKilled);
		}

		private void Hooks_OnPlayerKilled(DeathEvent de)
		{
			try
			{
				Player player = de.Victim as Player;
				if (player.GameID == this.GameID)
				{
					this.justDied = true;
				}
			}
			catch (Exception)
			{
				this.invError = true;
			}
		}

		public void InventoryNotice(string arg)
		{
			Rust.Notice.Inventory(this.ourPlayer.netPlayer, arg);
		}

		public void Kill()
		{
			TakeDamage.KillSelf(this.PlayerClient.controllable.character, null);
		}

		public void Message(string arg)
		{
			this.SendCommand("chat.add " + Facepunch.Utility.String.QuoteSafe(Server.GetServer().server_message_name) + " " + Facepunch.Utility.String.QuoteSafe(arg));
		}

		public void MessageFrom(string playername, string arg)
		{
			this.SendCommand("chat.add " + Facepunch.Utility.String.QuoteSafe(playername) + " " + Facepunch.Utility.String.QuoteSafe(arg));
		}

		public void Notice(string arg)
		{
			Rust.Notice.Popup(this.PlayerClient.netPlayer, "!", arg, 4f);
		}

		public void Notice(string icon, string text, float duration = 4f)
		{
			Rust.Notice.Popup(this.PlayerClient.netPlayer, icon, text, duration);
		}

		public void SendCommand(string cmd)
		{
			ConsoleNetworker.SendClientCommand(this.PlayerClient.netPlayer, cmd);
		}

		public void TeleportTo(Player p)
		{
			this.TeleportTo(p.X, p.Y, p.Z);
		}

		public void TeleportTo(float x, float y, float z)
		{
			RustServerManagement.Get().TeleportPlayerToWorld(this.PlayerClient.netPlayer, new Vector3(x, y, z));
		}
	}
}
