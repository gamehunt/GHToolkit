using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using RemoteAdmin;
using System;
using System.Collections.Generic;

namespace GHToolkit.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class MakeCommand : ICommand
    {
        public string Command => "make";

        public string[] Aliases => new string[] { };

        public string Description => "GHToolkit root class-spawn command";

        private static Dictionary<string, Type> registry = new Dictionary<string, Type>();

        public static void RegisterType(string alias, Type type)
        {
            Log.Info($"Registered ExtraClass type {type.Name} with alias {alias}");
            registry.Add(alias, type);
        }

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count < 1)
            {
                response = "usage: make <class-name> [player-id]";
                return false;
            }
            if (sender is PlayerCommandSender pl)
            {
                Player player = null;
                if (arguments.Count < 2)
                {
                    player = Player.Get(pl.PlayerId);
                }
                else
                {
                    try
                    {
                        player = Player.Get(int.Parse(arguments.At(1)));
                    }
                    catch (Exception) { }
                }
                if (player == null)
                {
                    response = "Can't find a player!";
                    return false;
                }
                if (registry.ContainsKey(arguments.At(0)))
                {
                    if (!sender.CheckPermission("gh.make." + arguments.At(0)))
                    {
                        response = "Access denied!";
                        return false;
                    }
                    player.AssignExtraClass(registry[arguments.At(0)]);
                    response = "Class assigned!";
                    return true;
                }
                else
                {
                    response = "Unknown class!";
                    return false;
                }
            }
            else
            {
                response = "Only players can use this!";
                return false;
            }
        }
    }
}