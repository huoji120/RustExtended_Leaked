using RustExtended;
using System;
using UnityEngine;

public class serv : ConsoleSystem
{
	public static int ShutdownLeft = 0;

	public static int ShutdownTime = 0;

	public static EventTimer RestartEvent = null;

	public static EventTimer ShutdownEvent = null;

	public static UserData servData = new UserData(0uL);

	[ConsoleSystem.Admin, ConsoleSystem.Help("Shutdown a server after 60 seconds, with restart message", "<time>")]
	public static void restart(ref ConsoleSystem.Arg arg)
	{
		Commands.Restart(null, "serv.restart", arg.Args);
	}

	[ConsoleSystem.Admin, ConsoleSystem.Help("Shutdown a server after 60 seconds, with shutdown message", "<time>")]
	public static void shutdown(ref ConsoleSystem.Arg arg)
	{
		Commands.Shutdown(null, "serv.shutdown", arg.Args);
	}

	[ConsoleSystem.Admin, ConsoleSystem.Help("Display all connected players to server console", "")]
	public static void players(ref ConsoleSystem.Arg arg)
	{
		Commands.Players(null, "serv.players", arg.Args);
	}

	[ConsoleSystem.Admin, ConsoleSystem.Help("Display clients in game to server console", "")]
	public static void clients(ref ConsoleSystem.Arg arg)
	{
		Commands.Clients(arg);
	}

	[ConsoleSystem.Admin, ConsoleSystem.Help("Sets or gets user premium days", "<username>, [<days>|disable]")]
	public static void premium(ref ConsoleSystem.Arg arg)
	{
		Commands.Premium(null, serv.servData, "serv.premium", arg.Args);
	}

	[ConsoleSystem.Admin, ConsoleSystem.Help("Give kit for specified player", "<player> <kit>")]
	public static void kit(ref ConsoleSystem.Arg arg)
	{
		Commands.Kit(null, serv.servData, "serv.kit", arg.Args);
	}

	[ConsoleSystem.Admin, ConsoleSystem.Help("Toggle PvP mode for specified player", "<player>")]
	public static void pvp(ref ConsoleSystem.Arg arg)
	{
		Commands.PvP(null, serv.servData, "serv.pvp", arg.Args);
	}

	[ConsoleSystem.Admin, ConsoleSystem.Help("Mutes a player in chat", "<player>, [<seconds>]")]
	public static void mute(ref ConsoleSystem.Arg arg)
	{
		Commands.Mute(null, "serv.mute", arg.Args);
	}

	[ConsoleSystem.Admin, ConsoleSystem.Help("Unmutes a player in chat", "<player>")]
	public static void unmute(ref ConsoleSystem.Arg arg)
	{
		Commands.Unmute(null, "serv.unmute", arg.Args);
	}

	[ConsoleSystem.Admin, ConsoleSystem.Help("Enable/disable truth detector for player", "<player>")]
	public static void truth(ref ConsoleSystem.Arg arg)
	{
		Commands.Truth(null, serv.servData, "serv.truth", arg.Args);
	}

	[ConsoleSystem.Admin, ConsoleSystem.Help("Sets food for specified player", "<player>, <value>")]
	public static void food(ref ConsoleSystem.Arg arg)
	{
		Commands.Food(null, serv.servData, "serv.food", arg.Args);
	}

	[ConsoleSystem.Admin, ConsoleSystem.Help("Sets health for specified player", "<player>, <value>")]
	public static void health(ref ConsoleSystem.Arg arg)
	{
		Commands.Health(null, serv.servData, "serv.health", arg.Args);
	}

	[ConsoleSystem.Admin, ConsoleSystem.Help("Sets a player share for specific player", "<owner>, <player>")]
	public static void share(ref ConsoleSystem.Arg arg)
	{
		Commands.Share(null, serv.servData, "serv.share", arg.Args);
	}

	[ConsoleSystem.Admin, ConsoleSystem.Help("Sets a player unshare for specific player", "<owner>[, <player>]")]
	public static void unshare(ref ConsoleSystem.Arg arg)
	{
		Commands.Unshare(null, serv.servData, "serv.unshare", arg.Args);
	}

	[ConsoleSystem.Admin, ConsoleSystem.Help("Sets balance for specified player", "<player>, <value>")]
	public static void money(ref ConsoleSystem.Arg arg)
	{
		serv.balance(ref arg);
	}

	[ConsoleSystem.Admin, ConsoleSystem.Help("Sets balance for specified player", "<player>, <value>")]
	public static void balance(ref ConsoleSystem.Arg arg)
	{
		Economy.Balance(null, serv.servData, "serv.balance", arg.Args);
	}

	[ConsoleSystem.Admin, ConsoleSystem.Help("Gives an item for specified player", "<player>, <item name>[, <amount>][, <slots>]")]
	public static void i(ref ConsoleSystem.Arg arg)
	{
		serv.give(ref arg);
	}

	[ConsoleSystem.Admin, ConsoleSystem.Help("Gives an item for specified player", "<player>, <item name>[, <amount>][, <slots>]")]
	public static void give(ref ConsoleSystem.Arg arg)
	{
		Commands.Give(null, serv.servData, "serv.give", arg.Args);
	}

	[ConsoleSystem.Admin, ConsoleSystem.Help("Toggle safety boxes for specified player", "<player>")]
	public static void safebox(ref ConsoleSystem.Arg arg)
	{
		Commands.Safebox(null, serv.servData, "serv.savebox", arg.Args);
	}

	[ConsoleSystem.Admin, ConsoleSystem.Help("Clear or drop inventory of specified player", "<player> [drop|clear]")]
	public static void inv(ref ConsoleSystem.Arg arg)
	{
		serv.inventory(ref arg);
	}

	[ConsoleSystem.Admin, ConsoleSystem.Help("Clear or drop inventory of specified player", "<player> [drop|clear]")]
	public static void inventory(ref ConsoleSystem.Arg arg)
	{
		Commands.Inv(null, serv.servData, "serv.inv", arg.Args);
	}

	[ConsoleSystem.Admin, ConsoleSystem.Help("Freeze specified player.", "<player>")]
	public static void freeze(ref ConsoleSystem.Arg arg)
	{
		Commands.Freeze(null, serv.servData, "serv.freeze", arg.Args);
	}

	[ConsoleSystem.Admin, ConsoleSystem.Help("Teleport specified player", "<player> [, <player>]")]
	public static void tele(ref ConsoleSystem.Arg arg)
	{
		serv.teleport(ref arg);
	}

	[ConsoleSystem.Admin, ConsoleSystem.Help("Teleport specified player", "<player> [, <player>]")]
	public static void teleport(ref ConsoleSystem.Arg arg)
	{
		if (arg.Args.Length >= 2)
		{
			string[] array = arg.Args;
			if (array[0].Contains(","))
			{
				array = string.Join(" ", array).Replace(",", " ").Split(new string[]
				{
					" "
				}, StringSplitOptions.RemoveEmptyEntries);
			}
			PlayerClient playerClient = Helper.GetPlayerClient(array[0]);
			if (playerClient == null)
			{
				arg.ReplyWith("Player with name " + Helper.QuoteSafe(array[0]) + " not found.");
			}
			else
			{
				Vector3 vector = Vector3.zero;
				if (array.Length > 2)
				{
					float x = 0f;
					float y = 0f;
					float z = 0f;
					if (!float.TryParse(array[1], out x) || !float.TryParse(array[2], out y) || !float.TryParse(array[3], out z))
					{
						arg.ReplyWith("Invalid syntax for execute command");
						return;
					}
					vector = new Vector3(x, y, z);
					arg.ReplyWith(Helper.QuoteSafe(playerClient.userName) + " teleported to " + vector.AsString());
				}
				else
				{
					PlayerClient playerClient2 = Helper.GetPlayerClient(array[1]);
					if (playerClient2 == null)
					{
						arg.ReplyWith(Config.GetMessage("Command.PlayerNoFound", null, array[1]));
						return;
					}
					if (playerClient == playerClient2 || !ServerManagement.GetOrigin(playerClient2.netPlayer, true, out vector))
					{
						arg.ReplyWith(playerClient.userName + " cannot be teleported to " + playerClient2.userName);
						return;
					}
					arg.ReplyWith(Helper.QuoteSafe(playerClient.userName) + " teleported to " + Helper.QuoteSafe(playerClient2.userName));
				}
				Helper.TeleportTo(playerClient.netUser, vector);
			}
		}
	}

	[ConsoleSystem.Admin, ConsoleSystem.Help("Kick the specified player", "<player>")]
	public static void kick(ref ConsoleSystem.Arg arg)
	{
		Commands.Kick(null, serv.servData, "serv.kick", arg.Args);
	}

	[ConsoleSystem.Admin, ConsoleSystem.Help("Kick all players from server", "")]
	public static void kickall(ref ConsoleSystem.Arg arg)
	{
		Commands.KickAll(arg);
	}

	[ConsoleSystem.Admin, ConsoleSystem.Help("Bans the specified player", "<player> [<reason>] [<period>] [<details>]")]
	public static void ban(ref ConsoleSystem.Arg arg)
	{
		Commands.Ban(null, serv.servData, "serv.ban", arg.Args);
	}

	[ConsoleSystem.Admin, ConsoleSystem.Help("Unbans the specified player", "<player>")]
	public static void unban(ref ConsoleSystem.Arg arg)
	{
		Commands.UnBan(null, "serv.unban", arg.Args);
	}

	[ConsoleSystem.Admin, ConsoleSystem.Help("Blocking specified player by IP address", "(<player>|<ip>)")]
	public static void block(ref ConsoleSystem.Arg arg)
	{
		Commands.Block(null, "serv.block", arg.Args);
	}

	[ConsoleSystem.Admin, ConsoleSystem.Help("Unblocking specified IP address", "<ip>")]
	public static void unblock(ref ConsoleSystem.Arg arg)
	{
		Commands.Unblock(null, "serv.unblock", arg.Args);
	}

	[ConsoleSystem.Admin, ConsoleSystem.Help("Removing objects by name", "<name>")]
	public static void remove(ref ConsoleSystem.Arg arg)
	{
		Commands.Remove(null, null, "serv.remove", arg.Args);
	}

	[ConsoleSystem.Admin, ConsoleSystem.Help("Avatars management of server", "unused|clear [inventory|blueprint]")]
	public static void avatars(ref ConsoleSystem.Arg arg)
	{
		Commands.Avatars(null, null, "serv.avatars", arg.Args);
	}

	[ConsoleSystem.Admin, ConsoleSystem.Help("Users management of server", "<player>|save|load|export|import|unused> [remove|username|password|rank|flags|flag] [<value>]")]
	public static void users(ref ConsoleSystem.Arg arg)
	{
		Commands.UserManage(null, null, "serv.users", arg.Args);
	}

	[ConsoleSystem.Admin, ConsoleSystem.Help("Clans management of server", "[list|info|edit] <clan name|clan abbr> [name|abbr|motd|level|leader|(money|balance)|(experience|exp)|tax] [<value>]")]
	public static void clan(ref ConsoleSystem.Arg arg)
	{
		string[] array = arg.Args;
		if (array == null || array.Length == 0)
		{
			array = new string[]
			{
				"LIST"
			};
		}
		Commands.Clan(null, serv.servData, "serv.clan", array);
	}

	[ConsoleSystem.Admin, ConsoleSystem.Help("Clans management of server", "")]
	public static void clans(ref ConsoleSystem.Arg arg)
	{
		Commands.Clanlist(null, serv.servData, "serv.clans", arg.Args);
	}

	[ConsoleSystem.Admin, ConsoleSystem.Help("Config management of server", "<reload>")]
	public static void config(ref ConsoleSystem.Arg arg)
	{
		Commands.ConfigManage(null, "serv.config", arg.Args);
	}
}
