using System;
using System.Collections.Generic;
using System.Linq;
using common.resources;
using wServer.realm.entities;
using wServer.realm.setpieces;
using wServer.realm.terrain;
using log4net;
using wServer.realm.worlds;
using wServer.realm.worlds.logic;

namespace wServer.realm
{
    //The mad god who look after the realm
    class Oryx
    {
        public bool Closing;

        private static readonly ILog Log = LogManager.GetLogger(typeof(Oryx));
        
        private readonly Realm _world;
        private readonly Random _rand = new Random();
        private long _prevTick;
        private int _tenSecondTick;
        private RealmTime dummy_rt = new RealmTime();
        
        struct TauntData
        {
            public string[] Spawn;
            public string[] NumberOfEnemies;
            public string[] Final;
            public string[] Killed;
        }

        private readonly List<Tuple<string, ISetPiece>> _events = new List<Tuple<string, ISetPiece>>()
        {
            //Tuple.Create("Zombie Horde", (ISetPiece) new ZombieHorde())            
        };

        private readonly List<Tuple<string, ISetPiece>> _rareEvents = new List<Tuple<string, ISetPiece>>()
        {
        };

        #region "Taunt data"
        private static readonly Tuple<string, TauntData>[] CriticalEnemies = new Tuple<string, TauntData>[]
        {
            Tuple.Create("Lich", new TauntData()
            {
                NumberOfEnemies = new string[] {
                    "I am invincible while my {COUNT} Liches still stand!",
                    "My {COUNT} Liches will feast on your essence!"
                },
                Final = new string[] {
                    "My final Lich shall consume your souls!",
                    "My final Lich will protect me forever!"
                }
            }),
            Tuple.Create("Ent Ancient", new TauntData()
            {
                NumberOfEnemies = new string[] {
                    "Mortal scum! My {COUNT} Ent Ancients will defend me forever!",
                    "My forest of {COUNT} Ent Ancients is all the protection I need!"
                },
                Final = new string[] {
                    "My final Ent Ancient will destroy you all!",
                    "My final Ent Ancient shall crush you!"
                }
            }),
            Tuple.Create("Oasis Giant", new TauntData()
            {
                NumberOfEnemies = new string[] {
                    "My {COUNT} Oasis Giants will feast on your flesh!",
                    "You have no hope against my {COUNT} Oasis Giants!"
                },
                Final = new string[] {
                    "A powerful Oasis Giant still fights for me!",
                    "You will never defeat me while an Oasis Giant remains!"
                }
            }),
            Tuple.Create("Phoenix Lord", new TauntData()
            {
                NumberOfEnemies = new string[] {
                    "Maggots! My {COUNT} Phoenix Lord will burn you to ash!",
                    "My {COUNT} Phoenix Lords will serve me forever!"
                },
                Final = new string[] {
                    "My final Phoenix Lord will never fall!",
                    "My last Phoenix Lord will blacken your bones!"
                }
            }),
            Tuple.Create("Ghost King", new TauntData()
            {
                NumberOfEnemies = new string[] {
                    "My {COUNT} Ghost Kings give me more than enough protection!",
                    "Pathetic humans! My {COUNT} Ghost Kings shall destroy you utterly!"
                },
                Final = new string[] {
                    "A mighty Ghost King remains to guard me!",
                    "My final Ghost King is untouchable!"
                }
            }),
            Tuple.Create("Cyclops God", new TauntData()
            {
                NumberOfEnemies = new string[] {
                    "Cretins! I have {COUNT} Cyclops Gods to guard me!",
                    "My {COUNT} powerful Cyclops Gods will smash you!"
                },
                Final = new string[] {
                    "My last Cyclops God will smash you to pieces!",
                    "My final Cyclops God shall crush your puny skulls!"
                }
            }),
            Tuple.Create("Red Demon", new TauntData()
            {
                NumberOfEnemies = new string[] {
                    "Fools! There is no escape from my {COUNT} Red Demons!",
                    "My legion of {COUNT} Red Demons live only to serve me!"
                },
                Final = new string[] {
                    "My final Red Demon is unassailable!",
                    "A Red Demon still guards me!"
                }
            }),
            
            Tuple.Create("Skull Shrine", new TauntData()
            {
                Spawn = new string[] {
                    "Your futile efforts are no match for a Skull Shrine!"
                },
                NumberOfEnemies = new string[] {
                    "Insects!  {COUNT} Skull Shrines still protect me",
                    "You hairless apes will never overcome my {COUNT} Skull Shrines!",
                    "You frail humans will never defeat my {COUNT} Skull Shrines!",
                    "Miserable worms like you cannot stand against my {COUNT} Skull Shrines!",
                    "Imbeciles! My {COUNT} Skull Shrines make me invincible!"
                },
                Final = new string[] {
                    "Pathetic fools!  A Skull Shrine guards me!",
                    "Miserable scum!  My Skull Shrine is invincible!"
                },
                Killed = new string[] {
                    "You defaced a Skull Shrine!  Minions, to arms!",
                    "{PLAYER} razed one of my Skull Shrines -- I WILL HAVE MY REVENGE!",
                    "{PLAYER}, you will rue the day you dared to defile my Skull Shrine!",
                    "{PLAYER}, you contemptible pig! Ruining my Skull Shrine will be the last mistake you ever make!",
                    "{PLAYER}, you insignificant cur! The penalty for destroying a Skull Shrine is death!"
                }
            }),
            Tuple.Create("Cube God", new TauntData()
            {
                Spawn = new string[] {
                    "Your meager abilities cannot possibly challenge a Cube God!"
                },
                NumberOfEnemies = new string[] {
                    "Filthy vermin! My {COUNT} Cube Gods will exterminate you!",
                    "Loathsome slugs! My {COUNT} Cube Gods will defeat you!",
                    "You piteous cretins! {COUNT} Cube Gods still guard me!",
                    "Your pathetic rabble will never survive against my {COUNT} Cube Gods!",
                    "You feeble creatures have no hope against my {COUNT} Cube Gods!"
                },
                Final = new string[] {
                    "Worthless mortals! A mighty Cube God defends me!",
                    "Wretched mongrels!  An unconquerable Cube God is my bulwark!"
                },
                Killed = new string[] {
                    "You have dispatched my Cube God, but you will never escape my Realm!",
                    "{PLAYER}, you pathetic swine! How dare you assault my Cube God?",
                    "{PLAYER}, you wretched dog! You killed my Cube God!",
                    "{PLAYER}, you may have destroyed my Cube God but you will never defeat me!",
                    "I have many more Cube Gods, {PLAYER}!",
                }
            }),
            Tuple.Create("Pentaract", new TauntData()
            {
                Spawn = new string[] {
                    "Behold my Pentaract, and despair!"
                },
                NumberOfEnemies = new string[] {
                    "Wretched creatures! {COUNT} Pentaracts remain!",
                    "You detestable humans will never defeat my {COUNT} Pentaracts!",
                    "My {COUNT} Pentaracts will protect me forever!",
                    "Your weak efforts will never overcome my {COUNT} Pentaracts!",
                    "Defiance is useless! My {COUNT} Pentaracts will crush you!"
                },
                Final = new string[] {
                    "I am invincible while my Pentaract stands!",
                    "Ignorant fools! A Pentaract guards me still!"
                },
                Killed = new string[] {
                    "That was but one of many Pentaracts!",
                    "You have razed my Pentaract, but you will die here in my Realm!",
                    "{PLAYER}, you lowly scum!  You'll regret that you ever touched my Pentaract!",
                    "{PLAYER}, you flea-ridden animal! You destoryed my Pentaract!",
                    "{PLAYER}, by destroying my Pentaract you have sealed your own doom!"
                }
            }),
            Tuple.Create("Grand Sphinx", new TauntData()
            {
                Spawn = new string[] {
                    "At last, a Grand Sphinx will teach you to respect!"
                },
                NumberOfEnemies = new string[] {
                    "You dull-spirited apes! You shall pose no challenge for {COUNT} Grand Sphinxes!",
                    "Regret your choices, blasphemers! My {COUNT} Grand Sphinxes will teach you respect!",
                    "My {COUNT} Grand Sphinxes protect my Chamber with their lives!",
                    "My Grand Sphinxes will bewitch you with their beauty!"
                },
                Final = new string[] {
                    "A Grand Sphinx is more than a match for this rabble.",
                    "You festering rat-catchers! A Grand Sphinx will make you doubt your purpose!",
                    "Gaze upon the beauty of the Grand Sphinx and feel your last hopes drain away."
                },
                Killed = new string[] {
                    "The death of my Grand Sphinx shall be avenged!",
                    "My Grand Sphinx, she was so beautiful. I will kill you myself, {PLAYER}!",
                    "My Grand Sphinx had lived for thousands of years! You, {PLAYER}, will not survive the day!",
                    "{PLAYER}, you up-jumped goat herder! You shall pay for defeating my Grand Sphinx!",
                    "{PLAYER}, you pestiferous lout! I will not forget what you did to my Grand Sphinx!",
                    "{PLAYER}, you foul ruffian! Do not think I forget your defiling of my Grand Sphinx!"
                }
            }),
            Tuple.Create("Lord of the Lost Lands", new TauntData()
            {
                Spawn = new string[] {
                    "Cower in fear of my Lord of the Lost Lands!",
                    "My Lord of the Lost Lands will make short work of you!"
                },
                NumberOfEnemies = new string[] {
                    "Cower before your destroyer! You stand no chance against {COUNT} Lords of the Lost Lands!",
                    "Your pathetic band of fighters will be crushed under the might feet of my {COUNT} Lords of the Lost Lands!",
                    "Feel the awesome might of my {COUNT} Lords of the Lost Lands!",
                    "Together, my {COUNT} Lords of the Lost Lands will squash you like a bug!",
                    "Do not run! My {COUNT} Lords of the Lost Lands only wish to greet you!"
                },
                Final = new string[] {
                    "Give up now! You stand no chance against a Lord of the Lost Lands!",
                    "Pathetic fools! My Lord of the Lost Lands will crush you all!",
                    "You are nothing but disgusting slime to be scraped off the foot of my Lord of the Lost Lands!"
                },
                Killed = new string[] {
                    "How dare you foul-mouthed hooligans treat my Lord of the Lost Lands with such indignity!",
                    "What trickery is this?! My Lord of the Lost Lands was invincible!",
                    "You win this time, {PLAYER}, but mark my words:  You will fall before the day is done.",
                    "{PLAYER}, I will never forget you exploited my Lord of the Lost Lands' weakness!",
                    "{PLAYER}, you have done me a service! That Lord of the Lost Lands was not worthy of serving me.",
                    "You got lucky this time {PLAYER}, but you stand no chance against me!",
                }
            }),
            Tuple.Create("Hermit God", new TauntData()
            {
                Spawn = new string[] {
                    "My Hermit God's thousand tentacles shall drag you to a watery grave!"
                },
                NumberOfEnemies = new string[] {
                    "You will make a tasty snack for my Hermit Gods!",
                    "I will enjoy watching my {COUNT} Hermit Gods fight over your corpse!"
                },
                Final = new string[] {
                    "You will be pulled to the bottom of the sea by my mighty Hermit God.",
                    "Flee from my Hermit God, unless you desire a watery grave!",
                    "My Hermit God awaits more sacrifices for the majestic Thessal.",
                    "My Hermit God will pull you beneath the waves!",
                    "You will make a tasty snack for my Hermit God!",
                },
                Killed = new string[] {
                    "This is preposterous!  There is no way you could have defeated my Hermit God!",
                    "You were lucky this time, {PLAYER}!  You will rue this day that you killed my Hermit God!",
                    "You naive imbecile, {PLAYER}! Without my Hermit God, Dreadstump is free to roam the seas without fear!",
                    "My Hermit God was more than you'll ever be, {PLAYER}. I will kill you myself!",
                }
            }),
            Tuple.Create("Ghost Ship", new TauntData()
            {
                Spawn = new string[] {
                    "My Ghost Ship will terrorize you pathetic peasants!",
                    "A Ghost Ship has entered the Realm."
                },
                Final = new string[] {
                    "My Ghost Ship will send you to a watery grave.",
                    "You filthy mongrels stand no chance against my Ghost Ship!",
                    "My Ghost Ship's cannonballs will crush your pathetic Knights!"
                },
                Killed = new string[] {
                    "My Ghost Ship will return!",
                    "Alas, my beautiful Ghost Ship has sunk!",
                    "{PLAYER}, you foul creature.  I shall see to your death personally!",
                    "{PLAYER}, has crossed me for the last time! My Ghost Ship shall be avenged.",
                    "{PLAYER} is such a jerk!",
                    "How could a creature like {PLAYER} defeat my dreaded Ghost Ship?!",
                    "The spirits of the sea will seek revenge on your worthless soul, {PLAYER}!"
                }
            }),
            Tuple.Create("Dragon Head", new TauntData()
            {
                Spawn = new string[] {
                    "The Rock Dragon has been summoned.",
                    "Beware my Rock Dragon. All who face him shall perish.",
                },
                Final = new string[] {
                    "My Rock Dragon will end your pathetic existence!",
                    "Fools, no one can withstand the power of my Rock Dragon!",
                    "The Rock Dragon will guard his post until the bitter end.",
                    "The Rock Dragon will never let you enter the Lair of Draconis.",
                },
                Killed = new string[] {
                    "My Rock Dragon will return!",
                    "The Rock Dragon has failed me!",
                    "{PLAYER} knows not what he has done.  That Lair was guarded for the Realm's own protection!",
                    "{PLAYER}, you have angered me for the last time!",
                    "{PLAYER} will never survive the trials that lie ahead.",
                    "A filthy weakling like {PLAYER} could never have defeated my Rock Dragon!!!",
                    "You shall not live to see the next sunrise, {PLAYER}!",
                }
            }),
            Tuple.Create("shtrs Defense System", new TauntData()
            {
                Spawn = new string[] {
                    "The Shatters has been discovered!?!",
                    "The Forgotten King has raised his Avatar!",
                },
                Final = new string[] {
                    "Attacking the Avatar of the Forgotten King would be...unwise.",
                    "Kill the Avatar, and you risk setting free an abomination.",
                    "Before you enter the Shatters you must defeat the Avatar of the Forgotten King!",
                },
                Killed = new string[] {
                    "The Avatar has been defeated!",
                    "How could simpletons kill The Avatar of the Forgotten King!?",
                    "{PLAYER} has unleashed an evil upon this Realm.",
                    "{PLAYER}, you have awoken the Forgotten King. Enjoy a slow death!",
                    "{PLAYER} will never survive what lies in the depths of the Shatters.",
                    "Enjoy your little victory while it lasts, {PLAYER}!"
                }
            }),
            Tuple.Create("Zombie Horde", new TauntData()
            {
                Spawn = new string[] {
                    "At last, my Zombie Horde will eradicate you like the vermin that you are!",
                    "The full strength of my Zombie Horde has been unleashed!",
                    "Let the apocalypse begin!",
                    "Quiver with fear, peasants, my Zombie Horde has arrived!",
                },
                Final = new string[] {
                    "A small taste of my Zombie Horde should be enough to eliminate you!",
                    "My Zombie Horde will teach you the meaning of fear!",
                },
                Killed = new string[] {
                    "The death of my Zombie Horde is unacceptable! You will pay for your insolence!",
                    "{PLAYER}, I will kill you myself and turn you into the newest member of my Zombie Horde!",
                }
            }),
            Tuple.Create("Boshy", new TauntData()),
            Tuple.Create("The Kid", new TauntData()),
            Tuple.Create("Sanic", new TauntData())
        };
        #endregion

        #region "Spawn data"
        private static readonly Dictionary<ushort, Tuple<int, Tuple<string, double>[]>> RegionMobs = 
            new Dictionary<ushort, Tuple<int, Tuple<string, double>[]>>()
        {
            { 0xbe , Tuple.Create(//0xbe is tiletype, 3000 is amount, Tuple(entityname, chance of spawning)
                2000, new []
                {
                    Tuple.Create("Pirate", 0.3),
                    Tuple.Create("Piratess", 0.1),
                    Tuple.Create("Snake", 0.2),
                    Tuple.Create("Scorpion Queen", 0.4),
                    Tuple.Create("Bandit Leader", 0.4),
                    Tuple.Create("Red Gelatinous Cube", 0.2),
                    Tuple.Create("Purple Gelatinous Cube", 0.2),
                    Tuple.Create("Green Gelatinous Cube", 0.2),
                })
            },
            {  0x60 , Tuple.Create(
                2000, new []
                {
                    Tuple.Create("White Demon", 0.1),
                    Tuple.Create("Sprite God", 0.11),
                    Tuple.Create("Medusa", 0.1),
                    Tuple.Create("Ent God", 0.1),
                    Tuple.Create("Beholder", 0.1),
                    Tuple.Create("Flying Brain", 0.1),
                    Tuple.Create("Slime God", 0.09),
                    Tuple.Create("Ghost God", 0.09),
                    Tuple.Create("Rock Bot", 0.05),
                    Tuple.Create("Djinn", 0.09),
                    Tuple.Create("Leviathan", 0.09),
                    Tuple.Create("Arena Headless Horseman", 0.04)
                })
            },
        };
        #endregion

        public Oryx(Realm world)
        {
            _world = world;
            Init();
        }

        private static double GetUniform(Random rand)
        {
            // 0 <= u < 2^32
            var u = (uint)(rand.NextDouble() * uint.MaxValue);
            return (u + 1.0) * 2.328306435454494e-10;
        }

        private static double GetNormal(Random rand)
        {
            // Use Box-Muller algorithm
            var u1 = GetUniform(rand);
            var u2 = GetUniform(rand);
            var r = Math.Sqrt(-2.0 * Math.Log(u1));
            var theta = 2.0 * Math.PI * u2;
            return r * Math.Sin(theta);
        }

        private static double GetNormal(Random rand, double mean, double standardDeviation)
        {
            return mean + standardDeviation * GetNormal(rand);
        }

        private ushort GetRandomObjType(IEnumerable<Tuple<string, double>> dat)
        {
            double p = _rand.NextDouble();
            double n = 0;
            ushort objType = 0;
            foreach (var k in dat)
            {
                n += k.Item2;
                if (n > p)
                {
                    objType = _world.Manager.Resources.GameData.IdToObjectType[k.Item1];
                    break;
                }
            }
            return objType;
        }

        private string GetRandomName(IEnumerable<Tuple<string, double>> dat)
        {
            double p = _rand.NextDouble();
            double n = 0;
            string name = "Pirate";
            foreach (var k in dat)
            {
                n += k.Item2;
                if (n > p)
                {
                    name = k.Item1;
                    break;
                }
            }
            return name;
        }

        IntPoint randomPosition(List<IntPoint> positionlist)
        {
            try
            {
                int count = _rand.Next(0, positionlist.Count);
                return positionlist[count];
            }
            catch
            {
                return new IntPoint(0, 0);
            }
        }

        public void Init()
        {
            Log.Info("Initializing Realm...");

            int w = _world.Map.Width;
            int h = _world.Map.Height;
            AddPositions(w, h);
            SpawnEnemies();
        }
        
        private Dictionary<ushort, List<IntPoint>> positions = new Dictionary<ushort, List<IntPoint>>();
        private void AddPositions(int w, int h)
        {
            foreach (var i in RegionMobs)
            {
                positions.Add(i.Key, new List<IntPoint>());
                for (var e = 0; e < w; e++)
                    for (var a = 0; a < h; a++)
                        if (i.Key == _world.Map[e, a].TileDesc.ObjectType)
                            positions[i.Key].Add(new IntPoint(e, a));
            }
            Log.Info("Realm positions initialized.");
        }

        private int MaxCount()
        {
            int amount = 0;
            foreach (var i in RegionMobs)
                amount += i.Value.Item1;
            return amount;
        }

        private void SpawnEnemies()
        {
            foreach (var i in RegionMobs)
            {
                int count = 0;
                while (count < i.Value.Item1)
                {
                    string name = GetRandomName(RegionMobs[i.Key].Item2);
                    if (_world.Map[randomPosition(positions[i.Key]).X, randomPosition(positions[i.Key]).Y].TileDesc.ObjectType == i.Key)
                    {
                        SpawnEntity(name, randomPosition(positions[i.Key]).X, randomPosition(positions[i.Key]).Y);
                        count++;
                    }
                }
                Log.Info($"Generated tile:[{i.Key}] entities! Amount: {count}");
            }
            Log.Info("Succesfully finished spawning!");
        }
        
        public void SpawnEntity(string name, int x, int y)
        {
            Enemy entity = Entity.Resolve(_world.Manager, name) as Enemy;
            entity.Move(x, y);

            _world.EnterWorld(entity);
        }

        public void Tick(RealmTime time)
        {
            if (time.TotalElapsedMs - _prevTick <= 10000)
                return;

            if (_tenSecondTick % 6 == 0)
            {
                Log.Info("Controlling population...");
                //RespawnEnemies();
            }

            _tenSecondTick++;
            _prevTick = time.TotalElapsedMs;
        }

        private ushort IdtoOdjType(string id)
        {
            return _world.Manager.Resources.GameData.IdToObjectType[id];
        }
    }
}
