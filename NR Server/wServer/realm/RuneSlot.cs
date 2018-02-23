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
        private int _tick;

        public RuneSlot(Player player)
        {
            
        }

        public void HandleEffects(RealmTime time, Player player)
        {
            if (_tick % 2 == 0)
            {
                switch (player.RSEffect)
                {
                    case "Heals":
                        IncreaseHealRate(player);
                        break;
                    case "Defense":
                        DamageReduction(player);
                        break;
                    case "Damage":
                        Damage(player);
                        break;
                    case "Mp":
                        IncreaseManaRate(player);
                        break;
                }
            }
            _tick++;
        }

        private void Damage(Player player)
        {
            //todo
        }

        private void DamageReduction(Player player)
        {
            player.IsDefenseRune = true;
        }

        private void IncreaseManaRate(Player player)
        {
            player.MpIncRate = 750f;
        }

        private void IncreaseHealRate(Player player)
        {
            player.HpIncRate = 750;
        }
    }
}
