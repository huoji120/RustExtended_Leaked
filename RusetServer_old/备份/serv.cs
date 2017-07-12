using RustExtended;
using System;
using UnityEngine;

public class serv : ConsoleSystem
{
    public static EventTimer RestartEvent = null;
    public static UserData servData = new UserData(0L);
    public static EventTimer ShutdownEvent = null;
    public static int ShutdownLeft = 0;
    public static int ShutdownTime = 0;

    [Admin, Help("Avatars management of server", "unused|clear [inventory|blueprint]")]
    public static void avatars(ref ConsoleSystem.Arg arg)
    {
        Commands.Avatars(null, null, "serv.avatars", arg.Args);
    }

    [Help("Sets balance for specified player", "<player>, <value>"), Admin]
    public static void balance(ref ConsoleSystem.Arg arg)
    {
        Economy.Balance(null, servData, "serv.balance", arg.Args);
    }

    [Help("Bans the specified player", "<player> [<reason>] [<period>] [<details>]"), Admin]
    public static void ban(ref ConsoleSystem.Arg arg)
    {
        Commands.Ban(null, servData, "serv.ban", arg.Args);
    }

    [Help("Blocking specified player by IP address", "(<player>|<ip>)"), Admin]
    public static void block(ref ConsoleSystem.Arg arg)
    {
        Commands.Block(null, "serv.block", arg.Args);
    }

    [Help("Clans management of server", "[list|info|edit] <clan name|clan abbr> [name|abbr|motd|level|leader|(money|balance)|(experience|exp)|tax] [<value>]"), Admin]
    public static void clan(ref ConsoleSystem.Arg arg)
    {
        string[] args = arg.Args;
        if ((args == null) || (args.Length == 0))
        {
            args = new string[] { "LIST" };
        }
        Commands.Clan(null, servData, "serv.clan", args);
    }

    [Help("Clans management of server", ""), Admin]
    public static void clans(ref ConsoleSystem.Arg arg)
    {
        Commands.Clanlist(null, servData, "serv.clans", arg.Args);
    }

    [Admin, Help("Display clients in game to server console", "")]
    public static void clients(ref ConsoleSystem.Arg arg)
    {
        Commands.Clients(arg);
    }

    [Help("Config management of server", "<reload>"), Admin]
    public static void config(ref ConsoleSystem.Arg arg)
    {
        Commands.ConfigManage(null, "serv.config", arg.Args);
    }

    [Admin, Help("Sets food for specified player", "<player>, <value>")]
    public static void food(ref ConsoleSystem.Arg arg)
    {
        Commands.Food(null, servData, "serv.food", arg.Args);
    }

    [Admin, Help("Freeze specified player.", "<player>")]
    public static void freeze(ref ConsoleSystem.Arg arg)
    {
        Commands.Freeze(null, servData, "serv.freeze", arg.Args);
    }

    [Admin, Help("Gives an item for specified player", "<player>, <item name>[, <amount>][, <slots>]")]
    public static void give(ref ConsoleSystem.Arg arg)
    {
        Commands.Give(null, servData, "serv.give", arg.Args);
    }

    [Admin, Help("Sets health for specified player", "<player>, <value>")]
    public static void health(ref ConsoleSystem.Arg arg)
    {
        Commands.Health(null, servData, "serv.health", arg.Args);
    }

    [Help("Gives an item for specified player", "<player>, <item name>[, <amount>][, <slots>]"), Admin]
    public static void i(ref ConsoleSystem.Arg arg)
    {
        give(ref arg);
    }

    [Admin, Help("Clear or drop inventory of specified player", "<player> [drop|clear]")]
    public static void inv(ref ConsoleSystem.Arg arg)
    {
        inventory(ref arg);
    }

    [Help("Clear or drop inventory of specified player", "<player> [drop|clear]"), Admin]
    public static void inventory(ref ConsoleSystem.Arg arg)
    {
        Commands.Inv(null, servData, "serv.inv", arg.Args);
    }

    [Admin, Help("Kick the specified player", "<player>")]
    public static void kick(ref ConsoleSystem.Arg arg)
    {
        Commands.Kick(null, servData, "serv.kick", arg.Args);
    }

    [Help("Kick all players from server", ""), Admin]
    public static void kickall(ref ConsoleSystem.Arg arg)
    {
        Commands.KickAll(arg);
    }

    [Help("Give kit for specified player", "<player> <kit>"), Admin]
    public static void kit(ref ConsoleSystem.Arg arg)
    {
        Commands.Kit(null, servData, "serv.kit", arg.Args);
    }

    [Admin, Help("Sets balance for specified player", "<player>, <value>")]
    public static void money(ref ConsoleSystem.Arg arg)
    {
        balance(ref arg);
    }

    [Admin, Help("Mutes a player in chat", "<player>, [<seconds>]")]
    public static void mute(ref ConsoleSystem.Arg arg)
    {
        Commands.Mute(null, "serv.mute", arg.Args);
    }

    [Admin, Help("Display all connected players to server console", "")]
    public static void players(ref ConsoleSystem.Arg arg)
    {
        Commands.Players(null, "serv.players", arg.Args);
    }

    [Help("Sets or gets user premium days", "<username>, [<days>|disable]"), Admin]
    public static void premium(ref ConsoleSystem.Arg arg)
    {
        Commands.Premium(null, servData, "serv.premium", arg.Args);
    }

    [Help("Toggle PvP mode for specified player", "<player>"), Admin]
    public static void pvp(ref ConsoleSystem.Arg arg)
    {
        Commands.PvP(null, servData, "serv.pvp", arg.Args);
    }

    [Admin, Help("Removing objects by name", "<name>")]
    public static void remove(ref ConsoleSystem.Arg arg)
    {
        Commands.Remove(null, null, "serv.remove", arg.Args);
    }

    [Help("Shutdown a server after 60 seconds, with restart message", "<time>"), Admin]
    public static void restart(ref ConsoleSystem.Arg arg)
    {
        Commands.Restart(null, "serv.restart", arg.Args);
    }

    [Help("Toggle safety boxes for specified player", "<player>"), Admin]
    public static void safebox(ref ConsoleSystem.Arg arg)
    {
        Commands.Safebox(null, servData, "serv.savebox", arg.Args);
    }

    [Admin, Help("Sets a player share for specific player", "<owner>, <player>")]
    public static void share(ref ConsoleSystem.Arg arg)
    {
        Commands.Share(null, servData, "serv.share", arg.Args);
    }

    [Admin, Help("Shutdown a server after 60 seconds, with shutdown message", "<time>")]
    public static void shutdown(ref ConsoleSystem.Arg arg)
    {
        Commands.Shutdown(null, "serv.shutdown", arg.Args);
    }

    [Admin, Help("Teleport specified player", "<player> [, <player>]")]
    public static void tele(ref ConsoleSystem.Arg arg)
    {
        teleport(ref arg);
    }

    [Admin, Help("Teleport specified player", "<player> [, <player>]")]
    public static void teleport(ref ConsoleSystem.Arg arg)
    {
        if (arg.Args.Length >= 2)
        {
            string[] args = arg.Args;
            if (args[0].Contains(","))
            {
                args = string.Join(" ", args).Replace(",", " ").Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            }
            PlayerClient playerClient = Helper.GetPlayerClient(args[0]);
            if (playerClient == null)
            {
                arg.ReplyWith("Player with name " + Helper.QuoteSafe(args[0]) + " not found.");
            }
            else
            {
                Vector3 zero = Vector3.zero;
                if (args.Length > 2)
                {
                    float result = 0f;
                    float num2 = 0f;
                    float num3 = 0f;
                    if ((!float.TryParse(args[1], out result) || !float.TryParse(args[2], out num2)) || !float.TryParse(args[3], out num3))
                    {
                        arg.ReplyWith("Invalid syntax for execute command");
                        return;
                    }
                    zero = new Vector3(result, num2, num3);
                    arg.ReplyWith(Helper.QuoteSafe(playerClient.userName) + " teleported to " + zero.AsString());
                }
                else
                {
                    PlayerClient client2 = Helper.GetPlayerClient(args[1]);
                    if (client2 == null)
                    {
                        arg.ReplyWith(Config.GetMessage("Command.PlayerNoFound", null, args[1]));
                        return;
                    }
                    if ((playerClient == client2) || !ServerManagement.GetOrigin(client2.netPlayer, true, out zero))
                    {
                        arg.ReplyWith(playerClient.userName + " cannot be teleported to " + client2.userName);
                        return;
                    }
                    arg.ReplyWith(Helper.QuoteSafe(playerClient.userName) + " teleported to " + Helper.QuoteSafe(client2.userName));
                }
                Helper.TeleportTo(playerClient.netUser, zero);
            }
        }
    }

    [Help("Enable/disable truth detector for player", "<player>"), Admin]
    public static void truth(ref ConsoleSystem.Arg arg)
    {
        Commands.Truth(null, servData, "serv.truth", arg.Args);
    }

    [Help("Unbans the specified player", "<player>"), Admin]
    public static void unban(ref ConsoleSystem.Arg arg)
    {
        Commands.UnBan(null, "serv.unban", arg.Args);
    }

    [Help("Unblocking specified IP address", "<ip>"), Admin]
    public static void unblock(ref ConsoleSystem.Arg arg)
    {
        Commands.Unblock(null, "serv.unblock", arg.Args);
    }

    [Admin, Help("Unmutes a player in chat", "<player>")]
    public static void unmute(ref ConsoleSystem.Arg arg)
    {
        Commands.Unmute(null, "serv.unmute", arg.Args);
    }

    [Admin, Help("Sets a player unshare for specific player", "<owner>[, <player>]")]
    public static void unshare(ref ConsoleSystem.Arg arg)
    {
        Commands.Unshare(null, servData, "serv.unshare", arg.Args);
    }

    [Help("Users management of server", "<player>|save|load|export|import|unused> [remove|username|password|rank|flags|flag] [<value>]"), Admin]
    public static void users(ref ConsoleSystem.Arg arg)
    {
        Commands.UserManage(null, null, "serv.users", arg.Args);
    }
}

