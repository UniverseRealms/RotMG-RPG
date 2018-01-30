using common;

namespace wServer.networking.packets.outgoing
{
    class RewardPacket : OutgoingMessage
    {
        public uint ItemId { get; set; }

        public override PacketId ID => PacketId.REWARD;
        public override Packet CreateInstance() { return new RewardPacket(); }

        protected override void Read(NReader rdr)
        {
            ItemId = rdr.ReadUInt16();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(ItemId);
        }
    }
}
