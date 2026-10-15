// 反编译自程序集： Assembly-CSharp.dll
// 基于 ILSpy 反编译（第 2 版）
using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour, IStoreListener
{
    public static StoreManager instance;


    private static IStoreController m_StoreController;

    private static IExtensionProvider m_StoreExtensionProvider;


    private string cachedProductID = "";
    private void Awake()
    {
        StoreManager.instance = this;
    }

    private void Start()
    {
        //if (StoreManager.m_StoreController == null)
        //{
        //    this.InitializePurchasing();
        //}
    }

    //public void InitializePurchasing()
    //{
    //    if (!this.IsInitialized())
    //    {
    //        ConfigurationBuilder configurationBuilder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
    //        configurationBuilder.AddProduct("fist1", ProductType.Consumable, new IDs {
    //            {
    //                "_fist1",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "pfist1",
    //                GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("fist2", ProductType.Consumable, new IDs {
    //            {
    //                "_fist2",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "pfist2",
    //                GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("fist3", ProductType.Consumable, new IDs {
    //            {
    //                "_fist3",
    //                 AppleAppStore.Name
    //            },
    //            {
    //                "pfist3",
    //                GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("fist4", ProductType.Consumable, new IDs {
    //            {
    //                "_fist4",
    //                 AppleAppStore.Name
    //            },
    //            {
    //                "pfist4",
    //                 GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("fist5", ProductType.Consumable, new IDs {
    //            {
    //                "_fist5",
    //                 AppleAppStore.Name
    //            },
    //            {
    //                "pfist5",
    //                GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("fist6", ProductType.Consumable, new IDs {
    //            {
    //                "_fist6",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "pfist6",
    //                GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("fist7", ProductType.Consumable, new IDs {
    //            {
    //                "_fist7",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "pfist7",
    //                GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("dagger0", ProductType.Consumable, new IDs {
    //            {
    //                "_dagger0",
    //               AppleAppStore.Name
    //            },
    //            {
    //                "pdagger0",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("dagger1", ProductType.Consumable, new IDs {
    //            {
    //                "_dagger1",
    //                 AppleAppStore.Name
    //            },
    //            {
    //                "pdagger1",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("dagger2", ProductType.Consumable, new IDs {
    //            {
    //                "_dagger2",
    //                 AppleAppStore.Name
    //            },
    //            {
    //                "pdagger2",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("dagger3", ProductType.Consumable, new IDs {
    //            {
    //                "_dagger3",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "pdagger3",
    //                 GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("dagger4", ProductType.Consumable, new IDs {
    //            {
    //                "_dagger4",
    //                 AppleAppStore.Name
    //            },
    //            {
    //                "pdagger4",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("dagger5", ProductType.Consumable, new IDs {
    //            {
    //                "_dagger5",
    //                 AppleAppStore.Name
    //            },
    //            {
    //                "pdagger5",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("dagger6", ProductType.Consumable, new IDs {
    //            {
    //                "_dagger6",
    //                 AppleAppStore.Name
    //            },
    //            {
    //                "pdagger6",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("dagger7", ProductType.Consumable, new IDs {
    //            {
    //                "_dagger7",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "pdagger7",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("axe0", ProductType.Consumable, new IDs {
    //            {
    //                "_axe0",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "paxe0",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("axe1", ProductType.Consumable, new IDs {
    //            {
    //                "_axe1",
    //                 AppleAppStore.Name
    //            },
    //            {
    //                "paxe1",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("axe2", ProductType.Consumable, new IDs {
    //            {
    //                "_axe2",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "paxe2",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("axe3", ProductType.Consumable, new IDs {
    //            {
    //                "_axe3",
    //                 AppleAppStore.Name
    //            },
    //            {
    //                "paxe3",
    //                GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("axe4", ProductType.Consumable, new IDs {
    //            {
    //                "_axe4",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "paxe4",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("axe5", ProductType.Consumable, new IDs {
    //            {
    //                "_axe5",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "paxe5",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("axe6", ProductType.Consumable, new IDs {
    //            {
    //                "_axe6",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "paxe6",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("axe7", ProductType.Consumable, new IDs {
    //            {
    //                "_axe7",
    //                 AppleAppStore.Name
    //            },
    //            {
    //                "paxe7",
    //              GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("axe8", ProductType.Consumable, new IDs {
    //            {
    //                "_axe8",
    //               AppleAppStore.Name
    //            },
    //            {
    //                "paxe8",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("spear0", ProductType.Consumable, new IDs {
    //            {
    //                "_spear0",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "pspear0",
    //              GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("spear1", ProductType.Consumable, new IDs {
    //            {
    //                "_spear1",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "pspear1",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("spear2", ProductType.Consumable, new IDs {
    //            {
    //                "_spear2",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "pspear2",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("spear3", ProductType.Consumable, new IDs {
    //            {
    //                "_spear3",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "pspear3",
    //                GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("spear4", ProductType.Consumable, new IDs {
    //            {
    //                "_spear4",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "pspear4",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("spear5", ProductType.Consumable, new IDs {
    //            {
    //                "_spear5",
    //               AppleAppStore.Name
    //            },
    //            {
    //                "pspear5",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("spear6", ProductType.Consumable, new IDs {
    //            {
    //                "_spear6",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "pspear6",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("sword1h0", ProductType.Consumable, new IDs {
    //            {
    //                "_sword1h0",
    //               AppleAppStore.Name
    //            },
    //            {
    //                "psword1h0",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("sword1h1", ProductType.Consumable, new IDs {
    //            {
    //                "_sword1h1",
    //               AppleAppStore.Name
    //            },
    //            {
    //                "psword1h1",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("sword1h2", ProductType.Consumable, new IDs {
    //            {
    //                "_sword1h2",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "psword1h2",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("sword1h3", ProductType.Consumable, new IDs {
    //            {
    //                "_sword1h3",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "psword1h3",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("sword1h4", ProductType.Consumable, new IDs {
    //            {
    //                "_sword1h4",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "psword1h4",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("sword1h5", ProductType.Consumable, new IDs {
    //            {
    //                "_sword1h5",
    //                 AppleAppStore.Name
    //            },
    //            {
    //                "psword1h5",
    //                GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("sword1h6", ProductType.Consumable, new IDs {
    //            {
    //                "_sword1h6",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "psword1h6",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("sword1h7", ProductType.Consumable, new IDs {
    //            {
    //                "_sword1h7",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "psword1h7",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("sword2h0", ProductType.Consumable, new IDs {
    //            {
    //                "_sword2h0",
    //               AppleAppStore.Name
    //            },
    //            {
    //                "psword2h0",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("sword2h1", ProductType.Consumable, new IDs {
    //            {
    //                "_sword2h1",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "psword2h1",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("sword2h2", ProductType.Consumable, new IDs {
    //            {
    //                "_sword2h2",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "psword2h2",
    //                GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("sword2h3", ProductType.Consumable, new IDs {
    //            {
    //                "_sword2h3",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "psword2h3",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("sword2h4", ProductType.Consumable, new IDs {
    //            {
    //                "_sword2h4",
    //                 AppleAppStore.Name
    //            },
    //            {
    //                "psword2h4",
    //                GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("sword2h5", ProductType.Consumable, new IDs {
    //            {
    //                "_sword2h5",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "psword2h5",
    //                GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("sword2h6", ProductType.Consumable, new IDs {
    //            {
    //                "_sword2h6",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "psword2h6",
    //                GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("sword2h7", ProductType.Consumable, new IDs {
    //            {
    //                "_sword2h7",
    //              AppleAppStore.Name
    //            },
    //            {
    //                "psword2h7",
    //                GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("wolf", ProductType.Consumable, new IDs {
    //            {
    //                "_wolf",
    //               AppleAppStore.Name
    //            },
    //            {
    //                "pwolf",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("thor", ProductType.Consumable, new IDs {
    //            {
    //                "_thor",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "pthor",
    //                 GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("ironman", ProductType.Consumable, new IDs {
    //            {
    //                "_ironman",
    //                 AppleAppStore.Name
    //            },
    //            {
    //                "pironman",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("blackpanther", ProductType.Consumable, new IDs {
    //            {
    //                "_blackpanther",
    //               AppleAppStore.Name
    //            },
    //            {
    //                "pblackpanther",
    //                GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("americangothic", ProductType.Consumable, new IDs {
    //            {
    //                "_americangothic",
    //               AppleAppStore.Name
    //            },
    //            {
    //                "pamericangothic",
    //                GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("artist", ProductType.Consumable, new IDs {
    //            {
    //                "_artist",
    //               AppleAppStore.Name
    //            },
    //            {
    //                "partist",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("mrofficer", ProductType.Consumable, new IDs {
    //            {
    //                "_mrofficer",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "pmrofficer",
    //                GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("maleficent", ProductType.Consumable, new IDs {
    //            {
    //                "_maleficent",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "pmaleficent",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("chief", ProductType.Consumable, new IDs {
    //            {
    //                "_chief",
    //               AppleAppStore.Name
    //            },
    //            {
    //                "pchief",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("chinese", ProductType.Consumable, new IDs {
    //            {
    //                "_chinese",
    //               AppleAppStore.Name
    //            },
    //            {
    //                "pchinese",
    //                GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("drogon", ProductType.Consumable, new IDs {
    //            {
    //                "_drogon",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "pdrogon",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("dumbo", ProductType.Consumable, new IDs {
    //            {
    //                "_dumbo",
    //                 AppleAppStore.Name
    //            },
    //            {
    //                "pdumbo",
    //                GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("alex", ProductType.Consumable, new IDs {
    //            {
    //                "_alex",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "palex",
    //                GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("fullmetaljacket", ProductType.Consumable, new IDs {
    //            {
    //                "_fullmetaljacket",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "pfullmetaljacket",
    //                GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("gandalf", ProductType.Consumable, new IDs {
    //            {
    //                "_gandalf",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "pgandalf",
    //                GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("gasmask", ProductType.Consumable, new IDs {
    //            {
    //                "_gasmask",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "pgasmask",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("goku", ProductType.Consumable, new IDs {
    //            {
    //                "_goku",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "pgoku",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("walterwhite", ProductType.Consumable, new IDs {
    //            {
    //                "_walterwhite",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "pwalterwhite",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("robbywood", ProductType.Consumable, new IDs {
    //            {
    //                "_robbywood",
    //                 AppleAppStore.Name
    //            },
    //            {
    //                "probbywood",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("kendo", ProductType.Consumable, new IDs {
    //            {
    //                "_kendo",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "pkendo",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("catwoman", ProductType.Consumable, new IDs {
    //            {
    //                "_catwoman",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "pcatwoman",
    //              GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("korean", ProductType.Consumable, new IDs {
    //            {
    //                "_korean",
    //                 AppleAppStore.Name
    //            },
    //            {
    //                "pkorean",
    //               GooglePlay.Name
    //            }
    //        });
            
    //        configurationBuilder.AddProduct("lacucaracha", ProductType.Consumable, new IDs {
    //            {
    //                "_lacucaracha",
    //               AppleAppStore.Name
    //            },
    //            {
    //                "placucaracha",
    //              GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("machinist", ProductType.Consumable, new IDs {
    //            {
    //                "_machinist",
    //                 AppleAppStore.Name
    //            },
    //            {
    //                "pmachinist",
    //               GooglePlay.Name
    //            }
    //        });
        
    //        configurationBuilder.AddProduct("naruto", ProductType.Consumable, new IDs {
    //            {
    //                "_naruto",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "pnaruto",
    //              GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("ninja", ProductType.Consumable, new IDs {
    //            {
    //                "_ninja",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "pninja",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("oktoberfest", ProductType.Consumable, new IDs {
    //            {
    //                "_oktoberfest",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "poktoberfest",
    //              GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("ottoman", ProductType.Consumable, new IDs {
    //            {
    //                "_ottoman",
    //                 AppleAppStore.Name
    //            },
    //            {
    //                "pottoman",
    //              GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("harleyquinn", ProductType.Consumable, new IDs {
    //            {
    //                "_harleyquinn",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "pharleyquinn",
    //              GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("pinkman", ProductType.Consumable, new IDs {
    //            {
    //                "_pinkman",
    //                 AppleAppStore.Name
    //            },
    //            {
    //                "ppinkman",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("pennywise", ProductType.Consumable, new IDs {
    //            {
    //                "_pennywise",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "ppennywise",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("rugby", ProductType.Consumable, new IDs {
    //            {
    //                "_rugby",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "prugby",
    //             GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("sailor", ProductType.Consumable, new IDs {
    //            {
    //                "_sailor",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "psailor",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("samurai", ProductType.Consumable, new IDs {
    //            {
    //                "_samurai",
    //                 AppleAppStore.Name
    //            },
    //            {
    //                "psamurai",
    //               GooglePlay.Name
    //            }
    //        });
           
    //        configurationBuilder.AddProduct("spanish", ProductType.Consumable, new IDs {
    //            {
    //                "_spanish",
    //                 AppleAppStore.Name
    //            },
    //            {
    //                "pspanish",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("jason", ProductType.Consumable, new IDs {
    //            {
    //                "_jason",
    //                 AppleAppStore.Name
    //            },
    //            {
    //                "pjason",
    //                GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("sherlock", ProductType.Consumable, new IDs {
    //            {
    //                "_sherlock",
    //                 AppleAppStore.Name
    //            },
    //            {
    //                "psherlock",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("spikey", ProductType.Consumable, new IDs {
    //            {
    //                "_spikey",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "pspikey",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("vietnamese", ProductType.Consumable, new IDs {
    //            {
    //                "_vietnamese",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "pvietnamese",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("viking", ProductType.Consumable, new IDs {
    //            {
    //                "_viking",
    //                 AppleAppStore.Name
    //            },
    //            {
    //                "pviking",
    //              GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("paperbag", ProductType.Consumable, new IDs {
    //            {
    //                "_paperbag",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "ppaperbag",
    //                GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("graduate", ProductType.Consumable, new IDs {
    //            {
    //                "_graduate",
    //                 AppleAppStore.Name
    //            },
    //            {
    //                "pgraduate",
    //                GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("beertime", ProductType.Consumable, new IDs {
    //            {
    //                "_beertime",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "pbeertime",
    //               GooglePlay.Name
    //            }
    //        });

    //        configurationBuilder.AddProduct("chaplin", ProductType.Consumable, new IDs {
    //            {
    //                "_chaplin",
    //                 AppleAppStore.Name
    //            },
    //            {
    //                "pchaplin",
    //                GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("heisenberg", ProductType.Consumable, new IDs {
    //            {
    //                "_heisenberg",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "pheisenberg",
    //              GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("maskofthedead", ProductType.Consumable, new IDs {
    //            {
    //                "_maskofthedead",
    //                 AppleAppStore.Name
    //            },
    //            {
    //                "pmaskofthedead",
    //                GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("troll", ProductType.Consumable, new IDs {
    //            {
    //                "_troll",
    //                 AppleAppStore.Name
    //            },
    //            {
    //                "ptroll",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("theking", ProductType.Consumable, new IDs {
    //            {
    //                "_theking",
    //                 AppleAppStore.Name
    //            },
    //            {
    //                "ptheking",
    //                GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("slash", ProductType.Consumable, new IDs {
    //            {
    //                "_slash",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "pslash",
    //              GooglePlay.Name
    //            }
    //        });
           
    //        configurationBuilder.AddProduct("caesar", ProductType.Consumable, new IDs {
    //            {
    //                "_caesar",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "pcaesar",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("ragnar", ProductType.Consumable, new IDs {
    //            {
    //                "_ragnar",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "pragnar",
    //               GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("emperor", ProductType.Consumable, new IDs {
    //            {
    //                "_emperor",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "pemperor",
    //                GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("paimei", ProductType.Consumable, new IDs {
    //            {
    //                "_paimei",
    //                 AppleAppStore.Name
    //            },
    //            {
    //                "ppaimei",
    //               GooglePlay.Name
    //            }
    //        });
           
    //        configurationBuilder.AddProduct("unlockall", ProductType.Consumable, new IDs {
    //            {
    //                "_unlockall",
    //                 AppleAppStore.Name
    //            },
    //            {
    //                "punlockall",
    //                GooglePlay.Name
    //            }
    //        });
    //        configurationBuilder.AddProduct("removeads", ProductType.Consumable, new IDs {
    //            {
    //                "_removeads",
    //                AppleAppStore.Name
    //            },
    //            {
    //                "premoveads",
    //                GooglePlay.Name
    //            }
    //        });
    //        UnityPurchasing.Initialize(this, configurationBuilder);
    //    }
    //}

    private bool IsInitialized()
    {
        return StoreManager.m_StoreController != null && StoreManager.m_StoreExtensionProvider != null;
    }

    public void BuyProductID(string productId)
    {
        if (this.IsInitialized())
        {
            //Product product = StoreManager.m_StoreController.products.WithID(productId);
            Product product = null;
            foreach (var tempProduct in m_StoreController.products.all)
            {
                if (tempProduct.definition.storeSpecificId.Contains(productId))
                {
                    product= tempProduct;
                }

            }
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("正在异步购买商品：'{0}'", product.definition.id));
                cachedProductID = productId;
                StoreManager.m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("购买失败：未找到商品或商品当前不可购买");
                AnalyticsManager.instance.IAPBuyFailed(productId, "Product not found or initialized");
                ItemManager.instance.loadingObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log("购买失败：内购系统尚未初始化");
            ItemManager.instance.loadingObject.SetActive(false);
            AnalyticsManager.instance.IAPBuyFailed(productId, "IAPSystem not initialized");
        }
    }

    //public void RestorePurchases()
    //{
    //    if (!this.IsInitialized())
    //    {
    //        ItemManager.instance.loadingObject.SetActive(false);
    //    }
    //    else if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
    //    {
    //        IAppleExtensions extension = StoreManager.m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
    //        extension.RestoreTransactions(delegate
    //        {
    //            ItemManager.instance.loadingObject.SetActive(false);
    //        });
    //    }
    //    else
    //    {
    //        ItemManager.instance.loadingObject.SetActive(false);
    //    }
    //}

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("内购系统初始化成功");
        StoreManager.m_StoreController = controller;
        StoreManager.m_StoreExtensionProvider = extensions;

    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        OnInitializeFailed(error, null);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log("内购系统初始化失败，原因：" + error + (string.IsNullOrEmpty(message) ? string.Empty : "，详情：" + message));
        AnalyticsManager.instance.IAPInitializeFailed();
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Debug.Log("处理购买：" + cachedProductID);

        bool flag = true;
        if (String.Equals(args.purchasedProduct.definition.id, cachedProductID, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("购买处理成功，商品：'{0}'", args.purchasedProduct.definition.id));
            // 消耗型商品购买成功后，发放对应的游戏奖励。
            
        }
        // 若用户购买了未知商品，可在此补充其他商品的处理逻辑。
        else
        {
            Debug.Log(string.Format("购买处理失败，无法识别商品：'{0}'", args.purchasedProduct.definition.id));
            flag = false;
        }

        
        //CrossPlatformValidator crossPlatformValidator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);
        //try
        //{
        //    IPurchaseReceipt[] array = crossPlatformValidator.Validate(args.purchasedProduct.receipt);
        //}
        //catch (IAPSecurityException)
        //{
        //    UnityEngine.Debug.Log("收据无效，无法解锁内容");
        //    flag = false;
        //}
        if (flag)
        {
            Debug.Log("购买成功");
            ItemManager.instance.OnPurchaseCompleted(args.purchasedProduct);
        }
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("购买失败，商品：'{0}'，原因：{1}", product.definition.storeSpecificId, failureReason));
        ItemManager.instance.OnPurchaseFailed(product, failureReason);
    }

    public ProductMetadata GetProductData(string productId)
    {
        if (this.IsInitialized())
        {
            foreach (var product in m_StoreController.products.all)
            {
                if (product.definition.storeSpecificId.Contains(productId))
                {
                    return product.metadata;
                }
               
            }
        }
        return null;
    }
}
