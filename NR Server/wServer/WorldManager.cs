using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using wServer.networking;
using wServer.realm.entities;

namespace wServer
{
    public partial class WorldManager : Form
    {
        Dictionary<Player, string> players = new Dictionary<Player, string>();

        public WorldManager(Client client)
        {
            InitializeComponent();

            foreach (var i in client.Manager.Worlds.Values)
                foreach (var w in i.Players.Values)
                    players[w] = w.Name;
        }

        private void btnGetList_Click(object sender, EventArgs e)
        {
            foreach (var i in players)
                txtPlayerList.Text += i.Value + "\n";
            
        }
    }
}
