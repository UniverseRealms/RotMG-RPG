using wServer.realm.worlds;

namespace wServer.realm.setpieces
{
    class SPNexus : ISetPiece
    {
        public int Size //todo
        {
            get { return 80; }
        }


        public void RenderSetPiece(World world, IntPoint intPoint)
        {
            var proto = world.Manager.Resources.Worlds["StrongHold"];
            SetPieces.RenderFromProto(world, intPoint, proto);
        }
    }
}
