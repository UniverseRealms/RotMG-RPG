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
                { 0 , "DemonTower1.jm" },
                { 1 , "DemonTower2.jm" },
                { 2 , "DemonTower3.jm" },
                { 3 , "DemonTower4.jm" },
                { 4 , "DemonTower5.jm" }
            };
        }

        public override void Tick(RealmTime time)
        {
            base.Tick(time);
        }
    }
}
