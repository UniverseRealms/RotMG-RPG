﻿using wServer.realm;
using wServer.realm.entities;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using common.resources;
using System;

namespace wServer.networking.handlers
{
    class UseItemHandler : PacketHandlerBase<UseItem>
    {
        public override PacketId ID => PacketId.USEITEM;

        protected override void HandlePacket(Client client, UseItem packet)
        {
            client.Manager.Logic.AddPendingAction(t => Handle(client.Player, t, packet));
        }

        void Handle(Player player, RealmTime time, UseItem packet)
        {
            if (player?.Owner == null)
                return;
            
            var item = player.Manager.Resources.GameData.Items[(ushort)packet.SlotObject.ObjectId];
 
            player.UseItem(time, packet.SlotObject.ObjectId, packet.SlotObject.SlotId, packet.ItemUsePos);
        }
    }
}
