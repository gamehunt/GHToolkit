using Exiled.Events.EventArgs;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using UnityEngine;
using MEC;

namespace GHToolkit
{
    public class EventHandlers
    {

        private CoroutineHandle EscapeCoro;

        private IEnumerator<float> CheckEscape()
        {
            yield return Timing.WaitForSeconds(1f);
            Vector3 escape = UnityEngine.Object.FindObjectOfType<Escape>().worldPosition;
            for (; ; )
            {
                foreach (Player p in Player.List)
                {
                    if (Vector3.Distance(p.Position, escape) < Escape.radius)
                    {
                        if (p.HasExtraClass())
                        {
                            p.GetExtraClass().OnEscape();
                        }
                    }
                }
                yield return Timing.WaitForSeconds(0.5f);
            }
        }

        public void OnRoundStart()
        {
            EscapeCoro = Timing.RunCoroutine(CheckEscape());
        }

        public void OnRoundEnd(RoundEndedEventArgs ev)
        {
            Timing.KillCoroutines(EscapeCoro);
            foreach (Player p in Player.List)
            {
                p.RemoveExtraClass();
            }
        }

        public void OnDead(DiedEventArgs ev)
        {
            ev.Target.RemoveExtraClass();
        }

        public void OnRoleChanged(ChangingRoleEventArgs ev)
        {
            if (ev.Player.HasExtraClass() && ev.Player.GetExtraClass().Initialized)
            {
                ev.Player.RemoveExtraClass();
            }
        }

        public void OnLeft(LeftEventArgs ev)
        {
            ev.Player.RemoveExtraClass();
        }

        public void OnRoundEnding(EndingRoundEventArgs ev)
        {
            foreach(Player player in Player.List)
            {
                if (player.HasExtraClass())
                {
                    if (!player.GetExtraClass().CanRoundBeEnded(ev.ClassList))
                    {
                        ev.IsRoundEnded = false;
                        break;
                    }
                }
            }
        }
    }
}