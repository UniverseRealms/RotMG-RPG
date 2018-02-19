using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using wServer.realm.entities;

namespace wServer.realm
{
    public class RuneSlot
    {
        public RuneSlot(Player player)
        {
            
        }

        public void HandleEffects(RealmTime time, Player player)
        {
            switch (player.RSEffect)
            {
                case "Heals":
                    IncreaseHealRate(player);
                    break;
                case "Defense":
                    DamageReduction(player);
                    break;
            }
        }

        private void DamageReduction(Player player)
        {
            player.IsDefenseRune = true;
        }

        private void IncreaseHealRate(Player player)
        {
            player.HpIncRate = 750f;
        }
    }
}
