using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.realm.worlds;
using wServer.realm.worlds.logic;

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
            int[,] _size = new int[Size, Size];
        }
    }
}
