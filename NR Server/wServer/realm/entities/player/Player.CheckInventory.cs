using System;
using System.Collections.Generic;
using common.resources;

namespace wServer.realm.entities
{
    partial class Player
    {
        public int TargetInventory;

        private readonly int[] Abilities =
        {
            4,5,11,12,14
        };

        private readonly int[] Rings =
        {
            9
        };

        Dictionary<int, int> LowTierReq = new Dictionary<int, int>()
        {
            { 2 , 5}, { 3, 15 }, { 4, 25 }, { 5, 35 }, { 6, 45}
        };

        Dictionary<int, int> HighTierReq = new Dictionary<int, int>()
        {
            { 3, 5 }, { 4, 10 }, { 5, 15 }, { 6, 20 }, { 7, 25 },
            { 8, 30 }, { 9, 35 }, { 10, 40 }, { 11, 45 }, { 12, 50 },
            { 13, 55 }
        };

        public bool CheckInventory()
        {
            Item item = Inventory[0];

            int i = 0;
            while(i < 4)
            {
                item = Inventory[i];
                TargetInventory = i;
                i++;

                if (item.Soulbound && Level < 50)
                    return false;

                if (IsAbility(item) || IsRing(item))
                    return CheckLowTier(item);
                else
                    return CheckOthers(item);                
            }
            return false;
        }

        private bool IsRing(Item item)
        {
            foreach (var i in Rings)
                if (item.SlotType == i)
                    return true;
            return false;
        }

        private bool IsAbility(Item item)
        {
            foreach (var i in Abilities)
                if (item.SlotType == i)
                    return true;
            return false;
        }

        private bool CheckLowTier(Item item)
        {
            if (LowTierReq.ContainsKey(item.Tier)
                && Level < LowTierReq[item.Tier])
                return false;
            return true;
        }

        private bool CheckOthers(Item item)
        {
            if (HighTierReq.ContainsKey(item.Tier)
                && Level < HighTierReq[item.Tier])
                return false;
            return true;
        }
    }
}
