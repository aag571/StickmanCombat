// 反编译自程序集： Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsManager : MonoBehaviour
{
	public static AnalyticsManager instance;

	protected void Awake()
	{
		AnalyticsManager.instance = this;
	}

	public void OnItemsMenuOpened()
	{
		Analytics.CustomEvent("ItemsMenuOpened", new Dictionary<string, object>
		{
			{
				"belt",
				ItemManager.instance.LevelToBeltString(StoryManager.instance.level) + " Belt"
			},
			{
				"totalCoins",
				MonetizationManager.instance.coins
			},
			{
				"currentWeapon",
				ItemManager.instance.currentWeapon.GetName()
			},
			{
				"currentHat",
				ItemManager.instance.currentHat.GetName()
			}
		});
	}

	public void OnWeaponHatToggle()
	{
		Analytics.CustomEvent("WeaponHatToggled", new Dictionary<string, object>
		{
			{
				"belt",
				ItemManager.instance.LevelToBeltString(StoryManager.instance.level) + " Belt"
			},
			{
				"totalCoins",
				MonetizationManager.instance.coins
			},
			{
				"currentWeapon",
				ItemManager.instance.currentWeapon.GetName()
			},
			{
				"currentHat",
				ItemManager.instance.currentHat.GetName()
			}
		});
	}

	public void RewardedVideoAvailable()
	{
		Analytics.CustomEvent("RewardedVideoAvailable", new Dictionary<string, object>
		{
			{
				"belt",
				ItemManager.instance.LevelToBeltString(StoryManager.instance.level) + " Belt"
            },
			{
				"totalCoins",
				MonetizationManager.instance.coins
			},
			
			{
				"coinsWon",
				MonetizationManager.instance.watchToWinAmount
			},
			{
				"currentWeapon",
				ItemManager.instance.currentWeapon.GetName()
			},
			{
				"currentHat",
				ItemManager.instance.currentHat.GetName()
			}
		});
	}

	public void RewardedVideoStarted()
	{
		Analytics.CustomEvent("RewardedVideoStarted", new Dictionary<string, object>
		{
			{
				"belt",
				ItemManager.instance.LevelToBeltString(StoryManager.instance.level) + " Belt"
            },
			{
				"totalCoins",
				MonetizationManager.instance.coins
			},
			{
				"coinsWon",
				MonetizationManager.instance.watchToWinAmount
			},
			{
				"currentWeapon",
				ItemManager.instance.currentWeapon.GetName()
			},
			{
				"currentHat",
				ItemManager.instance.currentHat.GetName()
			}
		});
	}

	public void RewardedVideoWatched()
	{
		Analytics.CustomEvent("RewardedVideoWatched", new Dictionary<string, object>
		{
			{
				"belt",
				ItemManager.instance.LevelToBeltString(StoryManager.instance.level) + " Belt"
            },
			{
				"totalCoins",
				MonetizationManager.instance.coins
			},
			{
				"coinsWon",
				MonetizationManager.instance.watchToWinAmount
			},
			{
				"currentWeapon",
				ItemManager.instance.currentWeapon.GetName()
			},
			{
				"currentHat",
				ItemManager.instance.currentHat.GetName()
			}
		});
	}

	public void RewardedVideoSkipped()
	{
		Analytics.CustomEvent("RewardedVideoSkipped", new Dictionary<string, object>
		{
			{
				"belt",
				ItemManager.instance.LevelToBeltString(StoryManager.instance.level) + " Belt"
            },
			{
				"totalCoins",
				MonetizationManager.instance.coins
			},
			{
				"coinsWon",
				MonetizationManager.instance.watchToWinAmount
			},
			{
				"currentWeapon",
				ItemManager.instance.currentWeapon.GetName()
			},
			{
				"currentHat",
				ItemManager.instance.currentHat.GetName()
			}
		});
	}

	public void GiftClaimed()
	{
		Analytics.CustomEvent("GiftClaimed", new Dictionary<string, object>
		{
			{
				"belt",
				ItemManager.instance.LevelToBeltString(StoryManager.instance.level) + " Belt"
            },
			{
				"totalCoins",
				MonetizationManager.instance.coins
			},
			{
				"currentWeapon",
				ItemManager.instance.currentWeapon.GetName()
			},
			{
				"currentHat",
				ItemManager.instance.currentHat.GetName()
			}
		});
	}

	public void RandomItemBought()
	{
		Analytics.CustomEvent("RandomItemBought", new Dictionary<string, object>
		{
			{
				"belt",
				ItemManager.instance.LevelToBeltString(StoryManager.instance.level) + " Belt"
            },
			{
				"totalCoins",
				MonetizationManager.instance.coins
			},
			{
				"currentWeapon",
				ItemManager.instance.currentWeapon.GetName()
			},
			{
				"currentHat",
				ItemManager.instance.currentHat.GetName()
			}
		});
	}

	public void RetakeBeltExam()
	{
		Analytics.CustomEvent("BeltExamRetaken", new Dictionary<string, object>
		{
			{
				"belt",
				ItemManager.instance.LevelToBeltString(StoryManager.instance.level) + " Belt"
            },
			{
				"totalCoins",
				MonetizationManager.instance.coins
			},
			{
				"currentWeapon",
				ItemManager.instance.currentWeapon.GetName()
			},
			{
				"currentHat",
				ItemManager.instance.currentHat.GetName()
			}
		});
	}

	public void ShareButtonPressed()
	{
		Analytics.CustomEvent("ShareButtonPressed", new Dictionary<string, object>
		{
			{
				"belt",
				ItemManager.instance.LevelToBeltString(StoryManager.instance.level) + " Belt"
            },
			{
				"totalCoins",
				MonetizationManager.instance.coins
			},
			{
				"currentWeapon",
				ItemManager.instance.currentWeapon.GetName()
			},
			{
				"currentHat",
				ItemManager.instance.currentHat.GetName()
			}
		});
	}

	public void KilledByExaminer()
	{
		Analytics.CustomEvent("KilledByExaminer", new Dictionary<string, object>
		{
			{
				"level",
				ItemManager.instance.LevelToBeltString(StoryManager.instance.level) + " Belt"
            },
			{
				"totalCoins",
				MonetizationManager.instance.coins
			},
			{
				"currentWeapon",
				ItemManager.instance.currentWeapon.GetName()
			},
			{
				"currentHat",
				ItemManager.instance.currentHat.GetName()
			}
		});
	}

	public void FirstStart()
	{
		Analytics.CustomEvent("FirstStart", new Dictionary<string, object>());
	}

	public void TutorialPassed(int tutorialNo)
	{
		Analytics.CustomEvent("TutorialPassed", new Dictionary<string, object>
		{
			{
				"tutorialNo",
				"tutorial" + tutorialNo
			}
		});
	}

	public void PlayedFirstGame()
	{
		Analytics.CustomEvent("PlayedFirstGame", new Dictionary<string, object>());
	}

	public void PlayedBeltExam()
	{
		Analytics.CustomEvent("PlayedBeltExam", new Dictionary<string, object>
		{
			{
				"belt",
				ItemManager.instance.LevelToBeltString(StoryManager.instance.level) + " Belt"
            },
			{
				"totalCoins",
				MonetizationManager.instance.coins
			},
			{
				"currentWeapon",
				ItemManager.instance.currentWeapon.GetName()
			},
			{
				"currentHat",
				ItemManager.instance.currentHat.GetName()
			}
		});
	}

	public void SettingsPressed()
	{
		Analytics.CustomEvent("SettingsPressed", new Dictionary<string, object>
		{
			{
				"belt",
				ItemManager.instance.LevelToBeltString(StoryManager.instance.level) + " Belt"
            },
			{
				"totalCoins",
				MonetizationManager.instance.coins
			},
			{
				"currentWeapon",
				ItemManager.instance.currentWeapon.GetName()
			},
			{
				"currentHat",
				ItemManager.instance.currentHat.GetName()
			}
		});
	}

	public void ShopPressed()
	{
		Analytics.CustomEvent("ShopPressed", new Dictionary<string, object>
		{
			{
				"belt",
				ItemManager.instance.LevelToBeltString(StoryManager.instance.level) + " Belt"
            },
			{
				"totalCoins",
				MonetizationManager.instance.coins
			},
			{
				"currentWeapon",
				ItemManager.instance.currentWeapon.GetName()
			},
			{
				"currentHat",
				ItemManager.instance.currentHat.GetName()
			}
		});
	}

	public void IapButtonPressed(string id)
	{
		Analytics.CustomEvent("IapButtonPressed", new Dictionary<string, object>
		{
			{
				"belt",
				ItemManager.instance.LevelToBeltString(StoryManager.instance.level) + " Belt"
            },
			{
				"totalCoins",
				MonetizationManager.instance.coins
			},
			{
				"currentWeapon",
				ItemManager.instance.currentWeapon.GetName()
			},
			{
				"currentHat",
				ItemManager.instance.currentHat.GetName()
			},
			{
				"boughtItemId",
				id
			}
		});
	}

	public void BoughtItem(string id)
	{
		Analytics.CustomEvent("BoughtItem", new Dictionary<string, object>
		{
			{
				"belt",
				ItemManager.instance.LevelToBeltString(StoryManager.instance.level) + " Belt"
            },
			{
				"totalCoins",
				MonetizationManager.instance.coins
			},
			{
				"currentWeapon",
				ItemManager.instance.currentWeapon.GetName()
			},
			{
				"currentHat",
				ItemManager.instance.currentHat.GetName()
			},
			{
				"boughtItemId",
				id
			}
		});
	}

	public void IAPBuyFailed(string id, string reason)
	{
		Analytics.CustomEvent("IAPBuyFailed", new Dictionary<string, object>
		{
			{
				"belt",
				ItemManager.instance.LevelToBeltString(StoryManager.instance.level) + " Belt"
            },
			{
				"totalCoins",
				MonetizationManager.instance.coins
			},
			{
				"currentWeapon",
				ItemManager.instance.currentWeapon.GetName()
			},
			{
				"currentHat",
				ItemManager.instance.currentHat.GetName()
			},
			{
				"boughtItemId",
				id
			},
			{
				"reason",
				reason
			}
		});
	}

	public void CoinDoublerShown()
	{
		Analytics.CustomEvent("CoinDoublerShown", new Dictionary<string, object>
		{
			{
				"belt",
				ItemManager.instance.LevelToBeltString(StoryManager.instance.level) + " Belt"
            },
			{
				"totalCoins",
				MonetizationManager.instance.coins
			},
			{
				"currentWeapon",
				ItemManager.instance.currentWeapon.GetName()
			},
			{
				"currentHat",
				ItemManager.instance.currentHat.GetName()
			}
		});
	}

	public void CoinDoublerPressed()
	{
		Analytics.CustomEvent("CoinDoublerPressed", new Dictionary<string, object>
		{
			{
				"belt",
				ItemManager.instance.LevelToBeltString(StoryManager.instance.level) + " Belt"
            },
			{
				"totalCoins",
				MonetizationManager.instance.coins
			},
			{
				"currentWeapon",
				ItemManager.instance.currentWeapon.GetName()
			},
			{
				"currentHat",
				ItemManager.instance.currentHat.GetName()
			}
		});
	}

	public void IAPInitializeFailed()
	{
		Analytics.CustomEvent("IAPInitializeFailed", new Dictionary<string, object>
		{
			{
				"belt",
				ItemManager.instance.LevelToBeltString(StoryManager.instance.level) + " Belt"
            },
			{
				"totalCoins",
				MonetizationManager.instance.coins
			},
			{
				"currentWeapon",
				ItemManager.instance.currentWeapon.GetName()
			},
			{
				"currentHat",
				ItemManager.instance.currentHat.GetName()
			}
		});
	}
}
