using wServer.realm.entities;
using wServer.networking.packets;
using wServer.networking.packets.incoming;

namespace wServer.networking.handlers
{
    class IncrementStatHandler: PacketHandlerBase<IncrementStat>
    {
        public override PacketId ID => PacketId.INCREMENTSTAT;

        protected override void HandlePacket(Client client, IncrementStat packet)
        {
            switch(packet.StatType)
            {
                case 0:
                    IncreaseStat(client.Player, packet.StatType, 5);
                    break;
                default:
                    client.Player.SendInfo("Error!");
                    break;
            }
        }

        private void IncreaseStat(Player player, int stat, int amount)
        {
            player.Client.Character.Stats[stat] += amount;
            player.StatPoint -= 1;
            player.Client.Character.FlushAsync();
            player.SendInfo("Success!");
        }
    }
}
