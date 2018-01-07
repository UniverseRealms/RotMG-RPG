using wServer.realm.entities;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using System.Collections.Generic;

namespace wServer.networking.handlers
{
    class IncrementStatHandler: PacketHandlerBase<IncrementStat>
    {
        public override PacketId ID => PacketId.STATINCREMENT;

        Dictionary<int, int> statdic = new Dictionary<int, int>
        {
            { 0,5 }, { 1,5 }, { 2,1 }, { 3,1 }, { 4,1 },
            { 5,1 }, { 6,1 }, { 7,1 }, { 8,1 }
        };

        protected override void HandlePacket(Client client, IncrementStat packet)
        {
            if (client.Player.StatPoint < 1)
            {
                client.Player.SendInfo("Not enough points!");
                return;
            }

            if (statdic.ContainsKey(packet.StatType))
                IncreaseStat(client.Player, packet.StatType, statdic[packet.StatType]);
            else
                client.Player.SendInfo("Could not indicate stat!");
        }

        private void IncreaseStat(Player player, int stat, int amount)
        {
            var statinfo = player.Manager.Resources.GameData.Classes[player.ObjectType].Stats;

            if (player.Stats.Base[stat] >= statinfo[stat].MaxValue)
            {
                player.SendError("Stat already maxed!");
                return;
            }
            player.Stats.Base[stat] += amount;
            player.StatPoint -= 1;
            player.Client.Character.FlushAsync();
            player.SendInfo("Success!");
        }
    }
}
