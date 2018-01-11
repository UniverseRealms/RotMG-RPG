using common;

namespace wServer.networking.packets.incoming
{
    public class IncrementStat : IncomingMessage
    {
        public int StatType { get; set; }
        public bool Reset { get; set; }

        public override PacketId ID => PacketId.STATINCREMENT;
        public override Packet CreateInstance() { return new IncrementStat(); }

        protected override void Read(NReader rdr)
        {
            StatType = rdr.ReadInt32();
            Reset = rdr.ReadBoolean();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(StatType);
        }
    }
}
