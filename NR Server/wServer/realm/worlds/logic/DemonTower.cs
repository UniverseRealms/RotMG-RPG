using System.Collections.Generic;
using System.Linq;
using common.resources;
using common;
using wServer.networking;
using wServer.networking.packets.incoming;
using wServer.networking.packets.outgoing;
using wServer.realm.entities;

namespace wServer.realm.worlds.logic
{
    class DemonTower : Tower
    {

        
        public DemonTower(ProtoWorld proto):base (proto, DemonTower, 4) // 5
        {
            
        }

        protected override void Init()
        {
            FloorMaps = new Dictionary<int, string>
            {
                { 0 , "" },
                { 1 , "" },
                { 2 , "" },
                { 3 , "" },
                { 4 , "" }
            };
        }

        public override void Tick(RealmTime time)
        {
            base.Tick(time);
        }
    }
}
