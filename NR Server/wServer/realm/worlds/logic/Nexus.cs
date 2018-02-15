using System.Linq;
using common.resources;
using wServer.networking;
using wServer.realm.entities;
using wServer.realm.terrain;

namespace wServer.realm.worlds.logic
{
    class Nexus : World
    {
        public static ProtoWorld Proto;

        public Nexus(ProtoWorld proto, Client client = null) : base(proto)
        {
            Proto = proto;
        }

        protected override void Init()
        {
            base.Init();
            foreach (var i in Players.Values)
                i.Client.Reconnect(new networking.packets.outgoing.Reconnect
                {
                    Host = "",
                    Port = 2050,
                    GameId = World.Realm,
                    Name = "Realm",
                    IsFromArena = false 
                });
        }
    }
}
