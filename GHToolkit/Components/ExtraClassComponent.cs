using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using System;
using System.Linq;
using UnityEngine;

namespace GHToolkit.Components
{
    public abstract class ExtraClassComponent : MonoBehaviour
    {
        protected Player owner;

        public abstract string Name { get; }

        public bool Initialized { get; private set; } = false;

        private int __id = -1;

        public int Id
        {
            get
            {
                try
                {
                    if (__id == -1)
                    {
                        __id = Util.GenerateIdForAlias(Name);
                    }
                }
                catch (Exception)
                {
                    Log.Error($"ExtraClass {Name}'s alias is occupied!");
                    __id = -2;
                }
                return __id;
            }
        }

        private void Start()
        {
            PreSpawnLogic();
            owner = Player.Get(gameObject);
            owner.Role = Role;
            Timing.CallDelayed(0.1f, () =>
            {
                if (Health > 0)
                {
                    owner.Health = Health;
                    owner.MaxHealth = Health;
                }
                if (!Tag.IsEmpty())
                {
                    owner.PlayerInfoArea &= ~PlayerInfoArea.Role;
                    owner.CustomPlayerInfo = Tag;
                }
                if (!Ammo.IsEmpty() && Ammo.Length == 3)
                {
                    owner.Ammo[(int)AmmoType.Nato556] = Ammo[0];
                    owner.Ammo[(int)AmmoType.Nato762] = Ammo[1];
                    owner.Ammo[(int)AmmoType.Nato9] = Ammo[2];
                }
                if (!Inventory.IsEmpty())
                {
                    owner.ClearInventory();
                    foreach (ItemType itm in Inventory)
                    {
                        owner.AddItem(itm);
                    }
                }
                if (Spawnpoint != RoomType.Unknown)
                {
                    owner.Position = Map.Rooms.Where(r => r.Type == Spawnpoint).FirstOrDefault().Position;
                }
                if (!Broadcast.IsEmpty())
                {
                    owner.Broadcast(5, Broadcast);
                }
                if (!ConsoleMessage.IsEmpty())
                {
                    owner.SendConsoleMessage(ConsoleMessage, "yellow");
                }
                if (!GlobalBroadcast.IsEmpty())
                {
                    Map.Broadcast(5, GlobalBroadcast);
                }
                PostSpawnLogic();
                Initialized = true;
            });
        }

        private void OnDestroy()
        {
            if (!tag.IsEmpty())
            {
                owner.CustomPlayerInfo = "";
                owner.PlayerInfoArea |= PlayerInfoArea.Role;
            }
            CleanupLogic();
        }

        protected abstract void CleanupLogic();

        protected abstract void PostSpawnLogic();

        protected abstract void PreSpawnLogic();

        public virtual void OnEscape() { }

        public abstract bool CanRoundBeEnded(RoundSummary.SumInfo_ClassList classes);

        public virtual RoomType Spawnpoint { get; } = RoomType.Unknown;
        public abstract RoleType Role { get; }
        public virtual ItemType[] Inventory { get; } = new ItemType[] { };
        public virtual uint[] Ammo { get; } = new uint[] { };
        public virtual int Health { get; } = -1;
        public virtual string Tag { get; } = "";
        public virtual string Broadcast { get; } = "";
        public virtual string GlobalBroadcast { get; } = "";
        public virtual string ConsoleMessage { get; } = "";
    }
}