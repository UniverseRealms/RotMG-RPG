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
            
            var monitor = Manager.Monitor;
            foreach (var i in Manager.Worlds.Values)
            {
                if (i is Realm)
                {
                    monitor.AddPortal(i.Id);
                    continue;
                }
            }
        }
    }
}
