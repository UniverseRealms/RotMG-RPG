﻿using System;
using System.Collections.Generic;
using System.Linq;
using common;
using common.resources;
using wServer.logic;
using wServer.networking.packets;
using wServer.networking;
using log4net;
using wServer.networking.packets.outgoing;
using wServer.realm.worlds;
using System.Timers;
using System.Diagnostics;

namespace wServer.realm.entities
{
    interface IPlayer
    {
        void Damage(int dmg, Entity src, RealmTime time);
        bool IsVisibleToEnemy();
    }

    public partial class Player : Character, IContainer, IPlayer
    {
        new static readonly ILog Log = LogManager.GetLogger(typeof(Player));

        private readonly Client _client;
        public Client Client => _client;

        //Stats
        private readonly SV<int> _accountId;
        public int AccountId
        {
            get { return _accountId.GetValue(); }
            set { _accountId.SetValue(value); }
        }

        private readonly SV<int> _experience;
        public int Experience
        {
            get { return _experience.GetValue(); }
            set { _experience.SetValue(value); }
        }

        private readonly SV<int> _experienceGoal;
        public int ExperienceGoal
        {
            get { return _experienceGoal.GetValue(); }
            set { _experienceGoal.SetValue(value); }
        }

        private readonly SV<int> _level;
        public int Level
        {
            get { return _level.GetValue(); }
            set { _level.SetValue(value); }
        }

        private readonly SV<int> _statpoint;
        public int StatPoint
        {
            get { return _statpoint.GetValue(); }
            set { _statpoint.SetValue(value);  }
        }

        private readonly SV<int> _currentFame;
        public int CurrentFame
        {
            get { return _currentFame.GetValue(); }
            set { _currentFame.SetValue(value); }
        }

        private readonly SV<int> _fame;
        public int Fame
        {
            get { return _fame.GetValue(); }
            set { _fame.SetValue(value); }
        }

        private readonly SV<int> _fameGoal;
        public int FameGoal
        {
            get { return _fameGoal.GetValue(); }
            set { _fameGoal.SetValue(value); }
        }

        private readonly SV<int> _stars;
        public int Stars
        {
            get { return _stars.GetValue(); }
            set { _stars.SetValue(value); }
        }

        private readonly SV<string> _guild;
        public string Guild
        {
            get { return _guild.GetValue(); }
            set { _guild.SetValue(value); }
        }

        private readonly SV<int> _guildRank;
        public int GuildRank
        {
            get { return _guildRank.GetValue(); }
            set { _guildRank.SetValue(value); }
        }

        private readonly SV<int> _credits;
        public int Credits
        {
            get { return _credits.GetValue(); }
            set { _credits.SetValue(value); }
        }

        private readonly SV<bool> _nameChosen;
        public bool NameChosen
        {
            get { return _nameChosen.GetValue(); }
            set { _nameChosen.SetValue(value); }
        }

        private readonly SV<int> _texture1;
        public int Texture1
        {
            get { return _texture1.GetValue(); }
            set { _texture1.SetValue(value); }
        }

        private readonly SV<int> _texture2;
        public int Texture2
        {
            get { return _texture2.GetValue(); }
            set { _texture2.SetValue(value); }
        }


        private int _originalSkin;
        private readonly SV<int> _skin;
        public int Skin
        {
            get { return _skin.GetValue(); }
            set { _skin.SetValue(value); }
        }

        private readonly SV<int> _glow;
        public int Glow
        {
            get { return _glow.GetValue(); }
            set { _glow.SetValue(value); }
        }

        private readonly SV<int> _mp;
        public int MP
        {
            get { return _mp.GetValue(); }
            set { _mp.SetValue(value); }
        }

        private readonly SV<bool> _hasBackpack;
        public bool HasBackpack
        {
            get { return _hasBackpack.GetValue(); }
            set { _hasBackpack.SetValue(value); }
        }

        private readonly SV<bool> _xpBoosted;
        public bool XPBoosted
        {
            get { return _xpBoosted.GetValue(); }
            set { _xpBoosted.SetValue(value); }
        }
        
        private readonly SV<int> _oxygenBar;
        public int OxygenBar
        {
            get { return _oxygenBar.GetValue(); }
            set { _oxygenBar.SetValue(value); }
        }

        private readonly SV<int> _rank;
        public int Rank
        {
            get { return _rank.GetValue(); }
            set { _rank.SetValue(value); }
        }

        private readonly SV<ushort> _runestone;
        public string RSEffect { get; set; }
        public ushort RuneStone
        {
            get { return _runestone.GetValue(); }
            set { _runestone.SetValue(value); }
        }

        #region rankidentifier
        public Dictionary<string, int> RankIdentifier = new Dictionary<string, int>()
        {
            { "Owner", 100 },
            { "Developer", 80 },
            { "Staff", 60 },
            { "PremiumPlus", 40 },
            { "Premium", 20 },
            { "Default", 0 }
        };
        #endregion

        private readonly SV<int> _admin;
        public int Admin
        {
            get { return _admin.GetValue(); }
            set { _admin.SetValue(value); }
        }

        private readonly SV<int> _tokens; 
        public int Tokens
        {
            get { return _tokens.GetValue(); }
            set { _tokens.SetValue(value); }
        }

        public int XPBoostTime { get; set; }
        public int LDBoostTime { get; set; }
        public int LTBoostTime { get; set; }
        public ushort SetSkin { get; set; }
        public int SetSkinSize { get; set; }
        public int? GuildInvite { get; set; }
        public bool Muted { get; set; }

        public RInventory DbLink { get; private set; }
        public int[] SlotTypes { get; private set; }
        public Inventory Inventory { get; private set; }
        
        public ItemStacker HealthPots { get; private set; }
        public ItemStacker MagicPots { get; private set; }
        public ItemStacker[] Stacks { get; private set; }

        public readonly StatsManager Stats;
        
        protected override void ImportStats(StatsType stats, object val)
        {
            var items = Manager.Resources.GameData.Items;
            base.ImportStats(stats, val);
            switch (stats)
            {
                case StatsType.AccountId: AccountId = ((string)val).ToInt32(); break;
                case StatsType.Experience: Experience = (int)val; break;
                case StatsType.ExperienceGoal: ExperienceGoal = (int)val; break;
                case StatsType.Level: Level = (int)val; break;
                case StatsType.StatPoint: StatPoint = (int)val; break;
                case StatsType.RuneStone: RuneStone = (ushort)(int)val; break;
                case StatsType.Fame: Fame = (int)val; break;
                case StatsType.CurrentFame: CurrentFame = (int)val; break;
                case StatsType.FameGoal: FameGoal = (int)val; break;
                case StatsType.Stars: Stars = (int)val; break;
                case StatsType.Guild: Guild = (string)val; break;
                case StatsType.GuildRank: GuildRank = (int)val; break;
                case StatsType.Credits: Credits = (int)val; break;
                case StatsType.NameChosen: NameChosen = (int)val != 0; break;
                case StatsType.Texture1: Texture1 = (int)val; break;
                case StatsType.Texture2: Texture2 = (int)val; break;
                case StatsType.Skin: Skin = (int)val; break;
                case StatsType.Glow: Glow = (int)val; break;
                case StatsType.MP: MP = (int)val; break;
                case StatsType.Inventory0: Inventory[0] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.Inventory1: Inventory[1] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.Inventory2: Inventory[2] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.Inventory3: Inventory[3] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.Inventory4: Inventory[4] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.Inventory5: Inventory[5] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.Inventory6: Inventory[6] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.Inventory7: Inventory[7] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.Inventory8: Inventory[8] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.Inventory9: Inventory[9] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.Inventory10: Inventory[10] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.Inventory11: Inventory[11] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.BackPack0: Inventory[12] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.BackPack1: Inventory[13] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.BackPack2: Inventory[14] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.BackPack3: Inventory[15] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.BackPack4: Inventory[16] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.BackPack5: Inventory[17] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.BackPack6: Inventory[18] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.BackPack7: Inventory[19] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.MaximumHP: Stats.Base[0] = (int)val; break;
                case StatsType.MaximumMP: Stats.Base[1] = (int)val; break;
                case StatsType.Attack: Stats.Base[2] = (int)val; break;
                case StatsType.Defense: Stats.Base[3] = (int)val; break;
                case StatsType.Speed: Stats.Base[4] = (int)val; break;
                case StatsType.Dexterity: Stats.Base[5] = (int)val; break;
                case StatsType.Vitality: Stats.Base[6] = (int)val; break;
                case StatsType.Wisdom: Stats.Base[7] = (int)val; break;
                case StatsType.DamageMin: Stats.Base[8] = (int)val; break;
                case StatsType.DamageMax: Stats.Base[9] = (int)val; break;
                case StatsType.Luck: Stats.Base[10] = (int)val; break;
                case StatsType.HealthStackCount: HealthPots.Count = (int)val; break;
                case StatsType.MagicStackCount: MagicPots.Count = (int)val; break;
                case StatsType.HasBackpack: HasBackpack = (int)val == 1; break;
                case StatsType.XPBoostTime: XPBoostTime = (int)val * 1000; break;
                case StatsType.LDBoostTime: LDBoostTime = (int)val * 1000; break;
                case StatsType.LTBoostTime: LTBoostTime = (int)val * 1000; break;
                case StatsType.Rank: Rank = (int)val; break;
                case StatsType.Admin: Admin = (int)val; break;
                case StatsType.Tokens: Tokens = (int)val; break;
            }
        }

        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            base.ExportStats(stats);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            stats[StatsType.AccountId] = AccountId.ToString();
            stats[StatsType.Experience] = Experience - GetLevelExp(Level);
            stats[StatsType.ExperienceGoal] = ExperienceGoal;
            stats[StatsType.Level] = Level;
            stats[StatsType.StatPoint] = StatPoint;
            stats[StatsType.RuneStone] = RuneStone;
            stats[StatsType.CurrentFame] = CurrentFame;
            stats[StatsType.Fame] = Fame;
            stats[StatsType.FameGoal] = FameGoal;
            stats[StatsType.Stars] = Stars;
            stats[StatsType.Guild] = Guild;
            stats[StatsType.GuildRank] = GuildRank;
            stats[StatsType.Credits] = Credits;
            stats[StatsType.NameChosen] = 
                (_client.Account?.NameChosen ?? NameChosen) ? 1 : 0;
            stats[StatsType.Texture1] = Texture1;
            stats[StatsType.Texture2] = Texture2;
            stats[StatsType.Skin] = Skin;
            stats[StatsType.Glow] = Glow;
            stats[StatsType.MP] = MP;
            stats[StatsType.Inventory0] = Inventory[0]?.ObjectType ?? -1;
            stats[StatsType.Inventory1] = Inventory[1]?.ObjectType ?? -1;
            stats[StatsType.Inventory2] = Inventory[2]?.ObjectType ?? -1;
            stats[StatsType.Inventory3] = Inventory[3]?.ObjectType ?? -1;
            stats[StatsType.Inventory4] = Inventory[4]?.ObjectType ?? -1;
            stats[StatsType.Inventory5] = Inventory[5]?.ObjectType ?? -1;
            stats[StatsType.Inventory6] = Inventory[6]?.ObjectType ?? -1;
            stats[StatsType.Inventory7] = Inventory[7]?.ObjectType ?? -1;
            stats[StatsType.Inventory8] = Inventory[8]?.ObjectType ?? -1;
            stats[StatsType.Inventory9] = Inventory[9]?.ObjectType ?? -1;
            stats[StatsType.Inventory10] = Inventory[10]?.ObjectType ?? -1;
            stats[StatsType.Inventory11] = Inventory[11]?.ObjectType ?? -1;
            stats[StatsType.BackPack0] = Inventory[12]?.ObjectType ?? -1;
            stats[StatsType.BackPack1] = Inventory[13]?.ObjectType ?? -1;
            stats[StatsType.BackPack2] = Inventory[14]?.ObjectType ?? -1;
            stats[StatsType.BackPack3] = Inventory[15]?.ObjectType ?? -1;
            stats[StatsType.BackPack4] = Inventory[16]?.ObjectType ?? -1;
            stats[StatsType.BackPack5] = Inventory[17]?.ObjectType ?? -1;
            stats[StatsType.BackPack6] = Inventory[18]?.ObjectType ?? -1;
            stats[StatsType.BackPack7] = Inventory[19]?.ObjectType ?? -1;
            stats[StatsType.MaximumHP] = Stats[0];
            stats[StatsType.MaximumMP] = Stats[1];
            stats[StatsType.Attack] = Stats[2];
            stats[StatsType.Defense] = Stats[3];
            stats[StatsType.Speed] = Stats[4];
            stats[StatsType.Dexterity] = Stats[5];
            stats[StatsType.Vitality] = Stats[6];
            stats[StatsType.Wisdom] = Stats[7];
            stats[StatsType.DamageMin] = Stats[8];
            stats[StatsType.DamageMax] = Stats[9];
            stats[StatsType.Luck] = Stats[10];
            stats[StatsType.HPBoost] = Stats.Boost[0];
            stats[StatsType.MPBoost] = Stats.Boost[1];
            stats[StatsType.AttackBonus] = Stats.Boost[2];
            stats[StatsType.DefenseBonus] = Stats.Boost[3];
            stats[StatsType.SpeedBonus] = Stats.Boost[4];
            stats[StatsType.DexterityBonus] = Stats.Boost[5];
            stats[StatsType.VitalityBonus] = Stats.Boost[6];
            stats[StatsType.WisdomBonus] = Stats.Boost[7];
            stats[StatsType.DamageMinBonus] = Stats.Boost[8];
            stats[StatsType.DamageMaxBonus] = Stats.Boost[9];
            stats[StatsType.LuckBonus] = Stats.Boost[10];
            stats[StatsType.HealthStackCount] = HealthPots.Count;
            stats[StatsType.MagicStackCount] = MagicPots.Count;
            stats[StatsType.HasBackpack] = (HasBackpack) ? 1 : 0;
            stats[StatsType.XPBoost] = (XPBoostTime != 0) ? 1 : 0;
            stats[StatsType.XPBoostTime] = XPBoostTime / 1000;
            stats[StatsType.LDBoostTime] = LDBoostTime / 1000;
            stats[StatsType.LTBoostTime] = LTBoostTime / 1000;
            stats[StatsType.OxygenBar] = OxygenBar;
            stats[StatsType.Rank] = Rank;
            stats[StatsType.Admin] = Admin;
            stats[StatsType.Tokens] = Tokens;
            Log.Warn(stopwatch.ElapsedMilliseconds);
            stopwatch.Stop();
        }

        public void SaveToCharacter()
        {
            var chr = _client.Character;
            chr.Level = Level;
            chr.StatPoint = StatPoint;
            chr.Experience = Experience;
            chr.Fame = Fame;
            chr.HP = Math.Max(1, HP);
            chr.MP = MP;
            chr.Stats = Stats.Base.GetStats();
            chr.Tex1 = Texture1;
            chr.Tex2 = Texture2;
            chr.Skin = _originalSkin;
            chr.FameStats = FameCounter.Stats.Write();
            chr.LastSeen = DateTime.Now;
            chr.HealthStackCount = HealthPots.Count;
            chr.MagicStackCount = MagicPots.Count;
            chr.HasBackpack = HasBackpack;
            chr.XPBoostTime = XPBoostTime;
            chr.LDBoostTime = LDBoostTime;
            chr.LTBoostTime = LTBoostTime;
            chr.Items = Inventory.GetItemTypes();
            chr.RuneStone = RuneStone;
        }

        public Player(Client client, bool saveInventory = true)
            : base(client.Manager, client.Character.ObjectType)
        {
            var settings = Manager.Resources.Settings;
            var gameData = Manager.Resources.GameData;

            _client = client;

            // found in player.update partial
            Sight = new Sight(this);
            _clientEntities = new UpdatedSet(this);
            
            _accountId = new SV<int>(this, StatsType.AccountId, client.Account.AccountId, true);
            _experience = new SV<int>(this, StatsType.Experience, client.Character.Experience, true);
            _experienceGoal = new SV<int>(this, StatsType.ExperienceGoal, 0, true);
            _level = new SV<int>(this, StatsType.Level, client.Character.Level);
            _statpoint = new SV<int>(this, StatsType.StatPoint, client.Character.StatPoint);
            _runestone = new SV<ushort>(this, StatsType.RuneStone, client.Character.RuneStone);
            
            _currentFame = new SV<int>(this, StatsType.CurrentFame, client.Account.Fame, true);
            _fame = new SV<int>(this, StatsType.Fame, client.Character.Fame, true);
            _fameGoal = new SV<int>(this, StatsType.FameGoal, 0, true);
            _stars = new SV<int>(this, StatsType.Stars, 0);
            _guild = new SV<string>(this, StatsType.Guild, "");
            _guildRank = new SV<int>(this, StatsType.GuildRank, -1);
            _credits = new SV<int>(this, StatsType.Credits, client.Account.Credits, true);
            _nameChosen = new SV<bool>(this, StatsType.NameChosen, client.Account.NameChosen, false, v => _client.Account?.NameChosen ?? v);
            _texture1 = new SV<int>(this, StatsType.Texture1, client.Character.Tex1);
            _texture2 = new SV<int>(this, StatsType.Texture2, client.Character.Tex2);
            _skin = new SV<int>(this, StatsType.Skin, 0);
            _glow = new SV<int>(this, StatsType.Glow, 0);
            _mp = new SV<int>(this, StatsType.MP, client.Character.MP);
            _hasBackpack = new SV<bool>(this, StatsType.HasBackpack, client.Character.HasBackpack, true);
            _xpBoosted = new SV<bool>(this, StatsType.XPBoost, client.Character.XPBoostTime != 0, true);
            _oxygenBar = new SV<int>(this, StatsType.OxygenBar, -1, true);
            _rank = new SV<int>(this, StatsType.Rank, client.Account.Rank);
            _admin = new SV<int>(this, StatsType.Admin, client.Account.Admin ? 1 : 0);
            _tokens = new SV<int>(this, StatsType.Tokens, client.Account.Tokens, true);

            Name = client.Account.Name;
            HP = client.Character.HP;
            ConditionEffects = 0;

            XPBoostTime = client.Character.XPBoostTime;
            LDBoostTime = client.Character.LDBoostTime;
            LTBoostTime = client.Character.LTBoostTime;

            var s = (ushort) client.Character.Skin;
            if (gameData.Skins.Keys.Contains(s))
            {
                SetDefaultSkin(s);
                SetDefaultSize(gameData.Skins[s].Size);
            }

            var guild = Manager.Database.GetGuild(client.Account.GuildId);
            if (guild?.Name != null)
            {
                Guild = guild.Name;
                GuildRank = client.Account.GuildRank;
            }
            
            HealthPots = new ItemStacker(this, 254, 0x0A22, 
                client.Character.HealthStackCount, settings.MaxStackablePotions);
            MagicPots = new ItemStacker(this, 255, 0x0A23, 
                client.Character.MagicStackCount, settings.MaxStackablePotions);
            Stacks = new ItemStacker[] {HealthPots, MagicPots};

            // inventory setup
            DbLink = new DbCharInv(Client.Account, Client.Character.CharId);
            Inventory = new Inventory(this, 
                Utils.ResizeArray(
                    (DbLink as DbCharInv).Items
                        .Select(_ => (_ == 0xffff || !gameData.Items.ContainsKey(_)) ? null : gameData.Items[_])
                        .ToArray(), 
                    settings.InventorySize)
                );
            if (!saveInventory)
                DbLink = null;

            Inventory.InventoryChanged += (sender, e) => Stats.ReCalculateValues(e);
            SlotTypes = Utils.ResizeArray(
                gameData.Classes[ObjectType].SlotTypes,
                settings.InventorySize);
            Stats = new StatsManager(this);

            // set size of player if using set skin
            var skin = (ushort) Skin;
            if (gameData.SkinTypeToEquipSetType.ContainsKey(skin))
            {
                var setType = gameData.SkinTypeToEquipSetType[skin];
                var ae = gameData.EquipmentSets[setType].ActivateOnEquipAll
                    .SingleOrDefault(e => e.SkinType == skin);

                if (ae != null)
                    Size = ae.Size;
            }

            // override size
            if (Client.Account.Size > 0)
                Size = Client.Account.Size;
            
            Manager.Database.IsMuted(client.IP)
                .ContinueWith(t =>
                {
                    Muted = !Client.Account.Admin && t.IsCompleted && t.Result;
                });

            Manager.Database.IsLegend(AccountId)
                .ContinueWith(t =>
                {
                    Glow = t.Result && client.Account.GlowColor == 0
                        ? 0xFF0000
                        : client.Account.GlowColor;
                });
        }

        byte[,] tiles;
        public FameCounter FameCounter { get; private set; }

        public Entity SpectateTarget { get; set; }
        public bool IsControlling => SpectateTarget != null && !(SpectateTarget is Player);
        
        public void ResetFocus(object target, EventArgs e)
        {
            var entity = target as Entity;
            entity.FocusLost -= ResetFocus;
            entity.Controller = null;

            if (Owner == null)
                return;

            SpectateTarget = null;
            Owner.Timers.Add(new WorldTimer(3000, (w, t) =>
                ApplyConditionEffect(ConditionEffectIndex.Paused, 0)));
            Client.SendPacket(new SetFocus()
            {
                ObjectId = Id
            });
        }
        
        public override void Init(World owner)
        {
            var x = 0;
            var y = 0;
            var spawnRegions = owner.Name == "Realm" ? owner.GetSpawnPoints(true) 
                : owner.GetSpawnPoints(false);
            if (spawnRegions.Any())
            {
                var rand = new System.Random();
                var sRegion = spawnRegions.ElementAt(rand.Next(0, spawnRegions.Length));
                x = sRegion.Key.X;
                y = sRegion.Key.Y;
            }
            Move(x + 0.5f, y + 0.5f);
            tiles = new byte[owner.Map.Width, owner.Map.Height];
            
            FameCounter = new FameCounter(this);
            FameGoal = GetFameGoal(FameCounter.ClassStats[ObjectType].BestFame);
            ExperienceGoal = GetExpGoal(_client.Character.Level);
            Stars = GetStars();

            if (owner.Name.Equals("OceanTrench"))
                OxygenBar = 100;

            SetNewbiePeriod();

            base.Init(owner);
        }

        private RuneSlot runeSlot;
        public override void Tick(RealmTime time)
        {
            if (!KeepAlive(time))
                return;

            CheckTradeTimeout(time);
            HandleQuest(time);

            runeSlot = new RuneSlot(this);
            
            if (!HasConditionEffect(ConditionEffects.Paused))
            {
                HandleRegen(time);
                HandleEffects(time);
                HandleOceanTrenchGround(time);
                TickActivateEffects(time);
                FameCounter.Tick(time);
                if (RuneStone != 0x00)
                    runeSlot.HandleEffects(time, this);
            }

            base.Tick(time);
            
            SendUpdate(time);
            SendNewTick(time);

            if (HP <= 0)
            {
                Death("Unknown", time);
                return;
            }
        }

        void TickActivateEffects(RealmTime time)
        {
            var dt = time.ElaspedMsDelta;
            
            if (XPBoostTime > 0)
                XPBoostTime = Math.Max(XPBoostTime - dt, 0);
            if (XPBoostTime == 0)
                XPBoosted = false;
            
            if (LDBoostTime > 0)
                LDBoostTime = Math.Max(LDBoostTime - dt, 0);

            if (LTBoostTime > 0)
                LTBoostTime = Math.Max(LTBoostTime - dt, 0);
        }

        float _hpRegenCounter;
        float _mpRegenCounter;
        public float HpIncRate = 1000f;
        public float MpIncRate = 1000f;

        void HandleRegen(RealmTime time)
        {
            // hp regen
            if (HP == Stats[0] || !CanHpRegen())
                _hpRegenCounter = 0;
            else
            {
                _hpRegenCounter += Stats.GetHPRegen() * time.ElaspedMsDelta / HpIncRate;
                var regen = (int)_hpRegenCounter;
                if (regen > 0)
                {
                    HP = Math.Min(Stats[0], HP + regen);
                    _hpRegenCounter -= regen;
                }
            }

            // mp regen
            if (MP == Stats[1] || !CanMpRegen())
                _mpRegenCounter = 0;
            else
            {
                _mpRegenCounter += Stats.GetMPRegen() * time.ElaspedMsDelta / MpIncRate;
                var regen = (int)_mpRegenCounter;
                if (regen > 0)
                {
                    MP = Math.Min(Stats[1], MP + regen);
                    _mpRegenCounter -= regen;
                }
            }

            // hp pot regen
            /*if (Stacks[0].Count < Stacks[0].MaxCount)
            {
                _hpPotRegenCounter += Stacks[0].MaxCount * time.ElaspedMsDelta / 120000f;
                var potRegen = (int)_hpPotRegenCounter;
                if (potRegen > 0)
                {
                    Stacks[0].Put(Stacks[0].Item);
                    _hpPotRegenCounter -= potRegen;
                    UpdateCount++;
                }
            }*/
        }

        public void TeleportPosition(RealmTime time, float x, float y, bool ignoreRestrictions = false)
        {
            TeleportPosition(time, new Position() { X = x, Y = y }, ignoreRestrictions);
        }

        public void TeleportPosition(RealmTime time, Position position, bool ignoreRestrictions = false)
        {
            if (!ignoreRestrictions)
            {
                if (!TPCooledDown())
                {
                    SendError("Too soon to teleport again!");
                    return;
                }

                SetTPDisabledPeriod();
                SetNewbiePeriod();
                FameCounter.Teleport();
            }
            
            HandleQuest(time, true, position);

            var id = (IsControlling) ? SpectateTarget.Id : Id;
            var tpPkts = new Packet[]
            {
                new Goto()
                {
                    ObjectId = id,
                    Pos = position
                },
                new ShowEffect()
                {
                    EffectType = EffectType.Teleport,
                    TargetObjectId = id,
                    Pos1 = position,
                    Color = new ARGB(0xFFFFFFFF)
                }
            };
            foreach (var plr in Owner.Players.Values)
            {
                plr.AwaitGotoAck(time.TotalElapsedMs);
                plr.Client.SendPackets(tpPkts, PacketPriority.Low);
            }
        }

        public void TeleportToSpawn(RealmTime time)
        {
            var x = 0;
            var y = 0;

            var spawnRegions = Owner.Name == "Realm" ? Owner.GetSpawnPoints(true)
                : Owner.GetSpawnPoints(false);
            if (spawnRegions.Any())
            {
                var rand = new System.Random();
                var sRegion = spawnRegions.ElementAt(rand.Next(0, spawnRegions.Length));
                x = sRegion.Key.X;
                y = sRegion.Key.Y;
            }

            TeleportPosition(time, x, y, false);
        }

        public void Teleport(RealmTime time, int objId, bool ignoreRestrictions = false)
        {
            var obj = Owner.GetEntity(objId);
            if (obj == null)
            {
                SendError("Target does not exist.");
                return;
            }

            if (!ignoreRestrictions)
            {
                if (Id == objId)
                {
                    SendInfo("You are already at yourself, and always will be!");
                    return;
                }

                if (!Owner.AllowTeleport)
                {
                    SendError("Cannot teleport here.");
                    return;
                }

                if (HasConditionEffect(ConditionEffects.Paused))
                {
                    SendError("Cannot teleport while paused.");
                    return;
                }
                
                if (!(obj is Player))
                {
                    SendError("Can only teleport to players.");
                    return;
                }

                if (obj.HasConditionEffect(ConditionEffects.Invisible))
                {
                    SendError("Cannot teleport to an invisible player.");
                    return;
                }

                if (obj.HasConditionEffect(ConditionEffects.Paused))
                {
                    SendError("Cannot teleport to a paused player.");
                    return;
                }
            }

            TeleportPosition(time, obj.X, obj.Y, ignoreRestrictions);
        }

        public bool IsInvulnerable()
        {
            if (HasConditionEffect(ConditionEffects.Paused) ||
                HasConditionEffect(ConditionEffects.Stasis) ||
                HasConditionEffect(ConditionEffects.Invincible) ||
                HasConditionEffect(ConditionEffects.Invulnerable))
                return true;
            return false;
        }

        public bool IsDefenseRune = false;
        public override bool HitByProjectile(Projectile projectile, RealmTime time)
        {
            if (projectile.ProjectileOwner is Player ||
                IsInvulnerable())
                return false;

            var dmg = (int)Stats.GetDefenseDamage(projectile.Damage, projectile.ProjDesc.ArmorPiercing);
            if (IsDefenseRune)
                dmg = dmg - (int)(dmg / 9);
            if (!HasConditionEffect(ConditionEffects.Invulnerable))
                HP -= dmg;
            ApplyConditionEffect(projectile.ProjDesc.Effects);
            Owner.BroadcastPacketNearby(new Damage()
            {
                TargetId = this.Id,
                Effects = HasConditionEffect(ConditionEffects.Invincible) ? 0 : projectile.ConditionEffects,
                DamageAmount = (ushort)dmg,
                Kill = HP <= 0,
                BulletId = projectile.ProjectileId,
                ObjectId = projectile.ProjectileOwner.Self.Id
            }, this, this, PacketPriority.Low);

            if (HP <= 0)
                Death(projectile.ProjectileOwner.Self.ObjectDesc.DisplayId ??
                      projectile.ProjectileOwner.Self.ObjectDesc.ObjectId, time);

            return base.HitByProjectile(projectile, time);
        }

        public void Damage(int dmg, Entity src, RealmTime time)
        {
            if (IsInvulnerable())
                return;

            dmg = (int)Stats.GetDefenseDamage(dmg, false);
            if (!HasConditionEffect(ConditionEffects.Invulnerable))
                HP -= dmg;
            Owner.BroadcastPacketNearby(new Damage()
            {
                TargetId = Id,
                Effects = 0,
                DamageAmount = (ushort)dmg,
                Kill = HP <= 0,
                BulletId = 0,
                ObjectId = src.Id
            }, this, this, PacketPriority.Low);

            if (HP <= 0)
                Death(src.ObjectDesc.DisplayId ?? 
                      src.ObjectDesc.ObjectId, time);
        }

        bool _dead;
        bool Resurrection()
        {
            for (int i = 0; i < 4; i++)
            {
                var item = Inventory[i];

                if (item == null || !item.Resurrects)
                    continue;

                Inventory[i] = null;
                foreach (var player in Owner.Players.Values)
                    player.SendInfo($"{Name}'s {item.DisplayName} breaks and he disappears");

                ReconnectToRealm();
                return true;
            }
            return false;
        }

        public void ReconnectToRealm()
        {
            HP = 1;
            _client.Reconnect(new Reconnect()
            {
                Host = "",
                Port = 2050,
                GameId = World.Realm,
                Name = "Realm"
            });
        }

        private void AnnounceDeath(string killer)
        {
            foreach (var w in Manager.Worlds.Values)
                foreach (var i in w.Players.Values)
                    i.SendInfo(Name + " was killed by:" + killer + " at level:" + Level); 
        }

        public void Death(string killer, RealmTime time)
        {
            if (_client.State == ProtocolState.Disconnected || _dead)
                return;

            if (Resurrection())
            {
                RemoveItem(4, 3);
                return;
            }
           
            RemoveItem(10, 3);
            AnnounceDeath(killer);

            ApplyConditionEffect(new ConditionEffect()
            {
                Effect = ConditionEffectIndex.Paused,
                DurationMS = 5000,
            });
            SendInfo("5 seconds until respawn...");

            if (Owner.Name == "Realm")
            {
                TeleportToSpawn(time);
            }
            else
            {
                Timer timer = new Timer(5000);
                timer.Start();

                timer.Elapsed += new ElapsedEventHandler(ElapedFunction);

                void ElapedFunction(object o, ElapsedEventArgs e)
                {
                    ReconnectToRealm();
                    timer.Stop();
                }
            }

            HP = 10;
        }
        
        private void RemoveItem(int amount, int chance)
        {
            Random inv = new Random();
            Random c1 = new Random();

            RuneStone = 0x00;
            for (var i = 0; i < amount; i++)
            {
                int invnum = inv.Next(0, Inventory.Length);
                int removeamount = c1.Next(0, amount);

                if (removeamount % chance != 0 && Inventory[invnum] != null)
                    Inventory[invnum] = null;
            }
        }

        public void Reconnect(World world)
        {
            Client.Reconnect(new Reconnect()
            {
                Host = "",
                Port = 2050,
                GameId = world.Id,
                Name = world.Name
            });
        }

        public void Reconnect(object portal, World world)
        {
            ((Portal) portal).WorldInstanceSet -= Reconnect;

            if (world == null)
                SendError("Portal Not Implemented!");
            else
                Client.Reconnect(new Reconnect()
                {
                    Host = "",
                    Port = 2050,
                    GameId = world.Id,
                    Name = world.Name
                });
        }

        public int GetCurrency(CurrencyType currency)
        {
            switch (currency)
            {
                case CurrencyType.Gold:
                    return Credits;
                case CurrencyType.Fame:
                    return CurrentFame;
                case CurrencyType.Tokens:
                    return Tokens;
                default:
                    return 0;
            }
        }

        public void SetCurrency(CurrencyType currency, int amount)
        {
            switch (currency)
            {
                case CurrencyType.Gold:
                    Credits = amount; break;
                case CurrencyType.Fame:
                    CurrentFame = amount; break;
                case CurrencyType.Tokens:
                    Tokens = amount; break;
            }
        }

        public override void Move(float x, float y)
        {
            if (SpectateTarget != null && !(SpectateTarget is Player))
            {
                SpectateTarget.MoveEntity(x, y);
            }
            else
            {
                base.Move(x, y);
            }

            if ((int)X != Sight.LastX || (int)Y != Sight.LastY)
            {
                if (IsNoClipping())
                {
                    _client.Manager.Logic.AddPendingAction(t => _client.Disconnect());
                }

                Sight.UpdateCount++;
            }
        }
        
        public override void Dispose()
        {
            base.Dispose();
            if (SpectateTarget != null)
            {
                SpectateTarget.FocusLost -= ResetFocus;
                SpectateTarget.Controller = null;
            }
            _clientEntities.Dispose();
        }

        // allow other admins to see hidden people
        public override bool CanBeSeenBy(Player player)
        {
            if (Client?.Account != null && Client.Account.Hidden)
            {
                return player.Admin != 0;
            } 
            else
            {
                return true;
            }
        }

        public void SetDefaultSkin(int skin)
        {
            _originalSkin = skin;
            Skin = skin;
        }

        public void RestoreDefaultSkin()
        {
            Skin = _originalSkin;
        }

        public void DropNextRandom()
        {
            Client.Random.NextInt();
        }
    }
}
