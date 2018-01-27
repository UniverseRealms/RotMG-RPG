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
            if (packet.Reset == true)
            {
                if (client.Player.Credits >= 500)
                    Reset(client.Player);
                else
                    client.Player.SendError("Not enough gold!");                
                return;
            } else {
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
        }

        private void Reset(Player player)
        {
            var statinfo = player.Manager.Resources.GameData.Classes[player.ObjectType].Stats;
            int statamount = 0;

            for (var i = 0; i < 8; i++)
            {
                int diff = player.Stats.Base[i] - statinfo[i].StartingValue;

                player.Stats.Base[i] = statinfo[i].StartingValue;
                statamount += diff;
            }
            player.Credits = player.Client.Account.Credits -= 500;
            player.StatPoint += statamount;
            player.Client.Character.FlushAsync();
            player.SendInfo("Succesfully Reseted!");
        }

        private void IncreaseStat(Player player, int stat, int amount)
        {
            var statinfo = player.Manager.Resources.GameData.Classes[player.ObjectType].Stats;

            if (player.Stats.Base[stat] + 5 >= statinfo[stat].MaxValue)
            {
                int difference = (player.Stats.Base[stat] + 5) - statinfo[stat].MaxValue;
                amount -= difference;
            }
            if (player.Stats.Base[stat] >= statinfo[stat].MaxValue)
            {
                player.SendError("Stat already maxed!");
                return;
            }
            player.Stats.Base[stat] += amount;
            player.StatPoint -= 1;
            player.Client.Character.FlushAsync();
            player.SendInfo("Successfully increased " + StatName(stat) 
                + " by " + amount + ".");
        }

        private string StatName(int stattype)
        {
            switch (stattype)
            {
                case 0: return "Hp";
                case 1: return "Mp";
                case 2: return "Attack";
                case 3: return "Defense";
                case 4: return "Speed";
                case 5: return "Dexterity";
                case 6: return "Vitality";
                case 7: return "Wisdom";
                default: return "Unknown";
            }
        }
    }
}
