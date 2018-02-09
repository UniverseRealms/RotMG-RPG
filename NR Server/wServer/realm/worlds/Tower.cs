using System.Collections.Generic;
using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.worlds
{
    public class Tower : World
    {
        #region _towers
        public const int DemonTower = 0;
        #endregion

        public Dictionary<int, string> FloorMaps = new Dictionary<int, string>();

        ProtoWorld _proto;

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

        private int towerId;
        public string FloorName;
        public string Clearer;

        public Tower(ProtoWorld proto, int id, int maxfloor) : base(proto)
        {
            _proto = proto;
            towerId = id;

            if (Manager.Database.TowerExist(id) == false)
            {
                Manager.Database.CreateTower(id, maxfloor);
                CurrentFloor = 0;
            } else
            {
                var currenttower = Manager.Database.GetTower(id);
                
                CurrentFloor = currenttower.CurrentFloor;
            }

            _maxfloor = maxfloor;
            proto = _proto;
        }

        protected override void Init()
        {
            _proto.isTower = true;
            _proto.towerStarted = true;
        }

        public override void Tick(RealmTime time)
        {
            base.Tick(time);

            int istrue = 0;
            if (_proto.towerStarted)
            {
                istrue++;
                if (istrue == 1)
                {
                    AddMaps();

                    foreach (var i in Players.Values)
                        i.Reconnect(this);
                }

                if (IsCleared())
                    OnClear();
            }
        }

        private void AddMaps()
        {
            if (CurrentFloor + 1 < _maxfloor)
                _proto.towermap = FloorMaps[CurrentFloor + 1];
        }

        private bool IsCleared()
        {
            if (Enemies.Count < 1)
                return true;
            return false;
        }

        private void OnClear()
        {
            foreach (var i in Manager.Worlds.Values)
                foreach (var e in i.Players.Values)
                    e.SendInfo($"{Clearer} has cleared floor:" + CurrentFloor + " of the " + FloorName + "."); //todo:add clearer

            Manager.Database.IncreaseFloor(CurrentFloor + 1, towerId);
            Manager.Database.SetClearer(towerId, "Player");//for now

            ToNexus();
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
