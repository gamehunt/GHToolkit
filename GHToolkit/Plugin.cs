using Exiled.API.Features;
using HarmonyLib;
using System;
using Handlers = Exiled.Events.Handlers;

namespace GHToolkit
{
    public class Plugin : Plugin<Config>
    {
        public EventHandlers EventHandlers;

        public static Plugin Instance { get; private set; }
        public static Harmony Harmony { get; private set; }

        public override string Author { get; } = "gamehunt";
        public override string Name { get; } = "GHToolkit";
        public override string Prefix { get; } = "GHToolkit";
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(2, 1, 12);

        public override void OnEnabled()
        {
            try
            {
                Instance = this;

                Harmony = new Harmony("ghtoolkit.instance");

                Harmony.PatchAll();

                EventHandlers = new EventHandlers();

                Handlers.Server.RoundStarted += EventHandlers.OnRoundStart;
                Handlers.Server.RoundEnded += EventHandlers.OnRoundEnd;
                Handlers.Server.EndingRound += EventHandlers.OnRoundEnding;

                Handlers.Player.Died += EventHandlers.OnDead;
                Handlers.Player.ChangingRole += EventHandlers.OnRoleChanged;
                Handlers.Player.Left += EventHandlers.OnLeft;

                Log.Info($"GHToolkit plugin loaded. @gamehunt @Arith");
            }
            catch (Exception e)
            {
                Log.Error($"There was an error loading the plugin: {e}");
            }
        }

        public override void OnDisabled()
        {

            Harmony.UnpatchAll();

            Handlers.Server.RoundStarted -= EventHandlers.OnRoundStart;
            Handlers.Server.RoundEnded -= EventHandlers.OnRoundEnd;
            Handlers.Server.EndingRound -= EventHandlers.OnRoundEnding;

            Handlers.Player.Died -= EventHandlers.OnDead;
            Handlers.Player.ChangingRole -= EventHandlers.OnRoleChanged;
            Handlers.Player.Left -= EventHandlers.OnLeft;

            Instance = null;
            Harmony = null;
            EventHandlers = null;
        }

        public override void OnReloaded()
        {
        }
    }
}