using System.IO;
using System.Linq;
using System.Threading.Tasks;
using common.resources;
using log4net;
using wServer.networking;
using wServer.realm.setpieces;

namespace wServer.realm.worlds.logic
{
    public class Realm : World
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(Realm));

        private Oryx _overseer;

        private readonly bool _oryxPresent;
        private readonly int _mapId;
        private Task _overseerTask;

        public Realm(ProtoWorld proto, Client client = null) : base(proto)
        {
            _oryxPresent = true;
            _mapId = 1;
        }

        public override bool AllowedAccess(Client client)
        {
            return !Closed && base.AllowedAccess(client);
        }

        protected override void Init()
        {
            Log.InfoFormat("Initializing Game World {0}({1}) from map {2}...", Id, Name, _mapId);

            FromWorldMap(new MemoryStream(Manager.Resources.Worlds["Realm"].wmap[_mapId - 1]));
            SetPieces.ApplySetPieces(this);
            SetPieces.GenNexus(this);
            
            if (_oryxPresent)
            {
                _overseer = new Oryx(this);
            }

            Log.Info("Game World initalized.");
        }

        public override void Tick(RealmTime time)
        {
            _overseer.Tick(time);
            base.Tick(time);
        }
    }
}