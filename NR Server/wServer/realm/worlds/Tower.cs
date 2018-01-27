using System.Collections.Generic;
using System.Linq;
using common.resources;
using common;
using wServer.networking;
using wServer.networking.packets.incoming;
using wServer.networking.packets.outgoing;
using wServer.realm.entities;

namespace wServer.realm.worlds
{
    public class Tower : World
    {
        #region _towers
        public readonly int DemonTower = 0;
        #endregion

        public Dictionary<int, string> floormaps = new Dictionary<int, string>();
        ProtoWorld _poto;

        private bool floorCleared;

        private int _maxfloor;
        public int CurrentFloor
        {
            get { return 0; }
            set
            {
                if (value > _maxfloor)
                    value -= (value - _maxfloor);
            }
        }

        public Tower(ProtoWorld proto, int id, int maxfloor) : base(proto)
        {
            _poto = proto;
            Manager.Database.CreateTower(id, maxfloor);

            var currenttower = Manager.Database.GetTower(id);

            _maxfloor = maxfloor;
            CurrentFloor = currenttower.CurrentFloor;

            proto = _poto;
        }

        protected override void Init()
        {
            floorCleared = false;
            _poto.isTower = true;
            _poto.towerStarted = true;
        }

        public override void Tick(RealmTime time)
        {
            int istrue = 0;
            if (_poto.towerStarted)
            {
                istrue++;
                if (istrue == 1)
                {
                    foreach (var i in Players.Values)
                        i.Reconnect(this);
                    AddMaps();
                }

                if (IsCleared())
                    ToNexus();
            }
        }

        private void AddMaps()
        {
            int worldcount = 0;
            foreach (var i in _poto.towermaps)
            {
                worldcount++;
                floormaps.Add(worldcount, i.ToString());
            }

            for (var i = 0; i < _maxfloor; i++)
            {
                if (i == CurrentFloor)
                    _poto.towermap = floormaps[i];
            }
        }

        private bool IsCleared()
        {
            if (Enemies.Count < 1)
                return true;
            return false;
        }

        private void ToNexus()
        {
            foreach (var i in Players.Values)
            {
                i.Client.Reconnect(new Reconnect()
                {
                    Host = "",
                    Port = 2050,
                    GameId = World.Nexus,
                    Name = "Nexus"
                });
            }
        }
    }
}
