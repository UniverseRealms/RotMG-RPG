using System;
using System.Collections.Generic;
using System.Linq;
using common;
using common.resources;
using log4net;
using wServer.realm.terrain;

namespace wServer.realm.entities.vendors
{
    public class ShopItem : ISellableItem
    {
        public ushort ItemId { get; private set; }
        public int Price { get; }
        public int Count { get; }
        public string Name { get; }

        public ShopItem(string name, ushort price, int count = -1)
        {
            ItemId = ushort.MaxValue;
            Price = price;
            Count = count;
            Name = name;
        }

        public void SetItem(ushort item)
        {
            if (ItemId != ushort.MaxValue)
                throw new AccessViolationException("Can't change item after it has been set.");

            ItemId = item;
        }
    }
    
    internal static class MerchantLists
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MerchantLists));

        private static readonly List<ISellableItem> Weapons = new List<ISellableItem>
        {
            new ShopItem("Agateclaw Dagger", 300),
            new ShopItem("Bow of Innocent Blood", 300),
            new ShopItem("Staff of Astral Knowledge", 300),
            new ShopItem("Wand of Ancient Warning", 300), 
            new ShopItem("Skysplitter Sword", 300),
            new ShopItem("Muramasa", 300) 
        };

        private static readonly List<ISellableItem> Abilities = new List<ISellableItem>
        {
            new ShopItem("Cloak of Endless Twilight", 300),
            new ShopItem("Golden Quiver", 300),  
            new ShopItem("Magic Nova Spell", 300),
            new ShopItem("Tome of Divine Favor", 300),
            new ShopItem("Golden Helm", 300),
            new ShopItem("Mithril Shield", 300), 
            new ShopItem("Seal of the Holy Warrior", 300),
            new ShopItem("Nightwing Venom", 300),
            new ShopItem("Lifedrinker Skull", 300),
            new ShopItem("Dragonstalker Trap", 300),
            new ShopItem("Banishment Orb", 300),
            new ShopItem("Prism of Phantoms", 300),
            new ShopItem("Scepter of Skybolts", 300),
            new ShopItem("Ice Star", 300)
        };

        private static readonly List<ISellableItem> Armor = new List<ISellableItem>
        {
            new ShopItem("Robe of the Elder Warlock", 300),
            new ShopItem("Griffon Hide Armor", 300),
            new ShopItem("Abyssal Armor", 300)
        };

        private static readonly List<ISellableItem> Rings = new List<ISellableItem>
        {
            new ShopItem("Ring of Exalted Attack", 300),
            new ShopItem("Ring of Exalted Defense", 300),
            new ShopItem("Ring of Exalted Speed", 300),
            new ShopItem("Ring of Exalted Dexterity", 300),
            new ShopItem("Ring of Exalted Vitality", 300),
            new ShopItem("Ring of Exalted Wisdom", 300),
            new ShopItem("Ring of Exalted Health", 300),
            new ShopItem("Ring of Exalted Magic", 300)
        };

        private static readonly List<ISellableItem> Keys = new List<ISellableItem>
        {
            new ShopItem("Undead Lair Key", 40),
            new ShopItem("Sprite World Key", 40),
            new ShopItem("Davy's Key", 150),
            new ShopItem("The Crawling Depths Key", 200),
            new ShopItem("Candy Key", 80),
            new ShopItem("Abyss of Demons Key", 40),
            new ShopItem("Totem Key", 50),
            new ShopItem("Pirate Cave Key", 15),
            new ShopItem("Shatters Key", 200),
            new ShopItem("Beachzone Key", 30),
            new ShopItem("Ivory Wyvern Key", 150),
            new ShopItem("Lab Key", 40),
            new ShopItem("Manor Key", 60),
            new ShopItem("Cemetery Key", 100),
            new ShopItem("Ocean Trench Key", 200),
            new ShopItem("Snake Pit Key", 30),
            new ShopItem("Bella's Key", 150),
            new ShopItem("Shaitan's Key", 200),
            new ShopItem("Spider Den Key", 20),
            new ShopItem("Tomb of the Ancients Key", 200),
            new ShopItem("Battle Nexus Key", 150),
            new ShopItem("Woodland Labyrinth Key", 200),
            new ShopItem("Theatre Key", 80),
            new ShopItem("Ice Cave Key", 150)
        };

        private static readonly List<ISellableItem> Consumables = new List<ISellableItem>
        {
            new ShopItem("Saint Patty's Brew", 80),
            new ShopItem("Mad God Ale", 25),
            new ShopItem("XP Booster", 35),
            new ShopItem("Backpack", 300),
            new ShopItem("Amulet of Resurrection", 10000)
        };
        
        public static readonly Dictionary<TileRegion, Tuple<List<ISellableItem>, CurrencyType, /*Rank Req*/int>> Shops = 
            new Dictionary<TileRegion, Tuple<List<ISellableItem>, CurrencyType, int>>()
        {
            { TileRegion.Store_1, new Tuple<List<ISellableItem>, CurrencyType, int>(Weapons, CurrencyType.Fame, 0) },
            { TileRegion.Store_2, new Tuple<List<ISellableItem>, CurrencyType, int>(Abilities, CurrencyType.Fame, 0) },
            { TileRegion.Store_3, new Tuple<List<ISellableItem>, CurrencyType, int>(Armor, CurrencyType.Fame, 0) },
            { TileRegion.Store_4, new Tuple<List<ISellableItem>, CurrencyType, int>(Rings, CurrencyType.Fame, 0) },
            { TileRegion.Store_5, new Tuple<List<ISellableItem>, CurrencyType, int>(Keys, CurrencyType.Fame, 0) },
            { TileRegion.Store_6, new Tuple<List<ISellableItem>, CurrencyType, int>(Consumables, CurrencyType.Fame, 5) }
        };
        
        public static void Init(RealmManager manager)
        {
            foreach (var shop in Shops)
                foreach (var shopItem in shop.Value.Item1.OfType<ShopItem>())
                {
                    ushort id;
                    if (!manager.Resources.GameData.IdToObjectType.TryGetValue(shopItem.Name, out id))
                        Log.WarnFormat("Item name: {0}, not found.", shopItem.Name);
                    else
                        shopItem.SetItem(id);
                }
        }
    }
}