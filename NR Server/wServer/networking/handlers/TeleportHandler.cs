using wServer.realm;
using wServer.realm.entities;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.networking.packets.outgoing;
using System.Timers;

namespace wServer.networking.handlers
{
    class TeleportHandler : PacketHandlerBase<Teleport>
    {
        public override PacketId ID => PacketId.TELEPORT;

        protected override void HandlePacket(Client client, Teleport packet)
        {
            client.Manager.Logic.AddPendingAction(t => Handle(client.Player, t, packet.ObjectId, packet));
        }

        void Handle(Player player, RealmTime time, int objId, Teleport packet)
        {
            if (player == null || player.Owner == null)
                return;

            if (packet.IsRecon && player.Owner.Name == "Realm")
            {
                Timer timer = new Timer(5000);
                timer.Start();
                player.SendInfo("Teleporting to spawn in 5 seconds...");

                timer.Elapsed += new ElapsedEventHandler(DoEvent);
                    
                void DoEvent(object sender, ElapsedEventArgs e){
                    player.TeleportToSpawn(time);
                    timer.Stop();
                }
            }
            else if (player.Owner.Name != "Realm" && packet.IsRecon)
            {
                player.ReconnectToRealm();
            }
            else
            {
                player.Teleport(time, objId);
            }
        }
    }
}
