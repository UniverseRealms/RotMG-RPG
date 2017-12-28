using common;

namespace wServer.networking.packets.incoming
{
    public class IncrementStat : IncomingMessage
    {
        public int StatType { get; set; }

        public override PacketId ID => PacketId.INCREMENTSTAT;
        public override Packet CreateInstance() { return new IncrementStat(); }

        protected override void Read(NReader rdr)
        {
            StatType = rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(StatType);
        }
    }
}
