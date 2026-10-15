using CodeStage.AntiCheat.ObscuredTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

public class MonetizationManager : MonoBehaviour
{
	public static MonetizationManager instance;

	private const int factor = 2;

	private ObscuredInt freeGiftCount;

	private ObscuredInt gotItemCount;

	private DateTime lastGiftTime;

	public GameObject tipObject;

	public Text tipText;

	public Text giftNotReady;

	public Text giftClaim;

	public Text watchToWin;

	public Text coinNotReady;

	public Text coinClaim;

	public Text beltExam;

	public GameObject giftObject;

	public GameObject itemObject;

	public GameObject topObject;

	public ObscuredInt coins;

	public GameObject coinObject;

    public GameObject coinEffect;

    public SkeletonAnimation soulAnim;

	public Text comboText;


	private bool willShowItem;

	private bool willShowGift;

	public double coinSpawnTime;

	public bool coinSpawned;

	public int coinSide;

	private int targetMinutes;

	private int lastCoinSide;

	public ObscuredInt comboCount;

	public ObscuredInt interstitialShowCount;

	public int gameOverCount;

	public int watchToWinAmount;

	public ObscuredInt highestCoinCombo;

	public Image beltImage;

	public Image beltAltImage;

	public GameObject loadingObject;

    public GameObject gameOverMenu;

	public MenuToggle notifToggle;

	private void Awake()
	{
		MonetizationManager.instance = this;
	}

	private void Start()
	{
		this.gameOverCount = PlayerPrefs.GetInt("gameOverCount", 0);
		this.coins = PlayerPrefs.GetInt("coins", 0);
        this.freeGiftCount = PlayerPrefs.GetInt("freeGiftCount", 0);
		this.gotItemCount = PlayerPrefs.GetInt("gotItemCount", 0);
		this.lastGiftTime = DateTime.Parse(PlayerPrefs.GetString("lastGiftTime", DateTime.Now.ToString()));
		this.interstitialShowCount = PlayerPrefs.GetInt("interstitialShowCount", 0);
		if (this.interstitialShowCount == 0)
		{
			this.interstitialShowCount = this.gameOverCount + 1;
            PlayerPrefs.SetInt("interstitialShowCount", this.interstitialShowCount);
		}
	}

	public void OnGameStart()
	{
		this.coinSpawnTime = (double)(Time.time - 10f);
		this.highestCoinCombo = 0;
		
	}

    public void OnGameOver()
    {
		this.SaveCoins();
        this.coinSpawned = false;
        this.willShowGift = false;
        this.willShowItem = false;







		if (this.CheckForFreeGift())
		{
			this.willShowGift = true;
		}
		else
		{
			TimeSpan timeSpan = DateTime.Now - this.lastGiftTime;
			double num2 = (double)this.targetMinutes - timeSpan.TotalMinutes;
			if (num2 < 10.0)
			{
				if (this.gameOverCount % 3 == 0)
				{
					this.willShowGift = true;
				}
			}
			else if (this.gameOverCount % 6 == 0)
			{
				this.willShowGift = true;
			}
		}
		this.beltExam.gameObject.SetActive(false);
		if (!SceneManager.instance.isEndless && !StoryManager.instance.levelCompleted)
		{
			this.beltExam.gameObject.SetActive(true);
			this.coinClaim.gameObject.SetActive(false);
			this.coinNotReady.gameObject.SetActive(false);
			int num3 = StoryManager.instance.level;
			if (num3 > 15)
			{
				num3 = 15;
			}
			this.beltImage.color = ItemManager.instance.beltColors[num3];
			if (num3 != 0 && num3 % 2 == 0)
			{
				this.beltAltImage.gameObject.SetActive(true);
			}
			else
			{
				this.beltAltImage.gameObject.SetActive(false);
			}
			int num4 = StoryManager.instance.level * 50;
			if (StoryManager.instance.level == 0)
			{
				num4 = 0;
			}
			else if (num4 < 200)
			{
				num4 = 200;
			}
			else if (num4 > 600)
			{
				num4 = 600;
			}
			this.beltExam.text = "再次挑战，消耗 " + num4 + " 个灵魂";
			this.willShowItem = true;
		}
		else if (this.CheckForCoinGift())
		{
			this.willShowItem = true;
		}
		else if (this.coins >= 180)
		{
			if (this.gameOverCount % 2 == 0)
			{
				this.willShowItem = true;
			}
		}
		else if (this.gameOverCount % 4 == 0)
		{
			this.willShowItem = true;
		}
		this.gameOverCount++;
        PlayerPrefs.SetInt("gameOverCount", this.gameOverCount);
		GameOverTipManager.instance.Save();
	}

	public void SpawnCoin(int side = 0)
	{
		this.coinSpawnTime = (double)Time.time;
		if (side == 0)
		{
			this.coinSide = -CharacterManager.instance.side;
		}
		else
		{
			this.coinSide = side;
		}
		if (UnityEngine.Random.Range(0, 2) == 0)
		{
			this.coinSide = UnityEngine.Random.Range(0, 2) * 2 - 1;
		}
		float num = UnityEngine.Random.Range(5f, 7f);
		this.lastCoinSide = this.coinSide;
		this.coinObject.transform.localScale = Vector3.zero;
		this.coinObject.transform.position = new Vector3(CharacterManager.instance.transform.position.x + (float)this.coinSide * num, 1, this.coinObject.transform.position.z);
		this.coinSpawned = true;
	}

	public void HideCoin()
	{
		this.coinSpawned = false;
		this.comboCount = 0;
	}

	public void NextTip()
	{
		this.tipText.text = GameOverTipManager.instance.GetNextTip();
	}

	private void Update()
	{
		if (this.comboText != null && this.comboCount > 1)
		{
			if (!this.comboText.gameObject.activeInHierarchy)
			{
				this.comboText.gameObject.SetActive(true);
			}
			string text = "灵魂连击 × " + this.comboCount;
			if (this.comboText.text != text)
			{
				this.comboText.text = text;
			}
		}
		else if (this.comboText != null && this.comboText.gameObject.activeInHierarchy)
		{
			this.comboText.gameObject.SetActive(false);
		}
		if (this.coinSpawned)
		{
			if (!this.coinObject.activeInHierarchy)
			{
				this.coinObject.SetActive(true);
                soulAnim.AnimationState.SetAnimation(0, "start", false);
                soulAnim.AnimationState.AddAnimation(0, "idle", true,0);
            }
			if (this.coinObject.transform.localScale.x < 0.07f)
			{
				this.coinObject.transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime) / 3f;
			}
			else
			{
				this.coinObject.transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
			}
			float num;
			if (this.coinSide == 1)
			{
				num = CharacterManager.instance.transform.position.x - this.coinObject.transform.position.x;
			}
			else
			{
				num = this.coinObject.transform.position.x - CharacterManager.instance.transform.position.x;
			}
			if (num > -0.3f)
			{
				this.CoinCollected();
			}
		}
		else if (this.coinObject.transform.localScale.x > 0f)
		{
			this.coinObject.transform.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime) / 3f;
		}
		else
		{
			this.coinObject.transform.localScale = new Vector3(0f, 0f, 0f);
			this.coinObject.SetActive(false);
		}
		if (SceneManager.instance.gameStarted && TutorialManager.instance.tutorialIndex >= 4 && SceneManager.instance.isEndless && TutorialManager.instance.tutorialIndex > 4)
		{
			if (this.coinSpawnTime + 10.0 < (double)Time.time && NpcManager.instance.killCount > 0)
			{
				this.SpawnCoin(0);
			}
			if (this.coinObject.activeInHierarchy && this.coinSpawnTime + 4.0 < (double)Time.time)
			{
				if (!SceneManager.instance.isEndless && !StoryManager.instance.introCompleted)
				{
					return;
				}
				this.HideCoin();
			}
		}
		else
		{
			this.HideCoin();
		}
		if (SceneManager.instance.currentState == SceneManager.State.gameOver && StickmanCombat.Gameplay.GameFlowController.instance == null)
		{
			this.CheckForFreeGift();
			this.CheckForCoinGift();
            if (!this.topObject.activeInHierarchy)
            {
                this.topObject.SetActive(true);
            }
            if (!this.tipObject.activeInHierarchy)
            {
                this.NextTip();
                this.tipObject.SetActive(true);
            }





            if (this.willShowItem)
			{
				if (!this.itemObject.activeInHierarchy)
				{
					this.itemObject.SetActive(true);
				}
			}
			else if (this.itemObject.activeInHierarchy)
			{
				this.itemObject.SetActive(false);
			}
			if (this.willShowGift)
			{
				if (!this.giftObject.activeInHierarchy)
				{
					this.giftObject.SetActive(true);
				}
			}
			else if (this.giftObject.activeInHierarchy)
			{
				this.giftObject.SetActive(false);
			}
		}
	}

	public void CoinCollected()
	{
        AudioManager.instance.CoinCollectSound();
        this.coinEffect.SetActive(false);
        this.coinEffect.transform.SetParent(this.coinObject.transform);
        this.coinEffect.transform.localPosition = Vector3.zero;
        this.coinEffect.SetActive(true);
        this.coinEffect.transform.SetParent(null);
        soulAnim.AnimationState.SetAnimation(0, "end", false);
		int num = 1; //自定义
		this.comboCount = ++this.comboCount;
		if (this.comboCount > this.highestCoinCombo)
		{
			this.highestCoinCombo = this.comboCount;
		}
		if (this.comboCount > StatManager.instance.stats[9].value)
		{
			StatManager.instance.stats[9].UpdateStat(this.comboCount);
		}
		this.SpawnCoin(-this.lastCoinSide);
		
		this.coins += num;
		StatManager.instance.stats[8].UpdateStat(StatManager.instance.stats[8].value + num);
	}

	public int GetFreeGift()
	{
		int num;
		if (this.freeGiftCount == 0 || this.freeGiftCount == 1)
		{
			num = 100;
		}
		else if (this.freeGiftCount == 2)
		{
			num = 120;
		}
		else
		{
			int num2 = UnityEngine.Random.Range(0, 101);
			if (num2 == 100)
			{
				num = UnityEngine.Random.Range(4, 101) * 10;
			}
			else if (num2 > 90)
			{
				num = UnityEngine.Random.Range(4, 51) * 10;
			}
			else if (num2 > 80)
			{
				num = UnityEngine.Random.Range(4, 21) * 10;
			}
			else if (num2 > 50)
			{
				num = UnityEngine.Random.Range(4, 13) * 10;
			}
			else
			{
				num = UnityEngine.Random.Range(4, 8) * 10;
			}
		}
		num *= 2;
		this.freeGiftCount = ++this.freeGiftCount;
        PlayerPrefs.SetInt("freeGiftCount", this.freeGiftCount);
		this.CheckForFreeGift();
		this.ResetFreeGiftTime();
		return num;
	}

	public bool CheckForFreeGift()
	{
		TimeSpan t = DateTime.Now - this.lastGiftTime;
		string text = string.Empty;
		bool flag = false;
		if (t.Days > 0)
		{
			flag = true;
		}
		else
		{
			double totalMinutes = t.TotalMinutes;
			switch (this.freeGiftCount)
			{
			case 0:
				this.targetMinutes = 0;
				break;
			case 1:
				this.targetMinutes = 3;
				break;
			case 2:
				this.targetMinutes = 6;
				break;
			case 3:
				this.targetMinutes = 30;
				break;
			case 4:
				this.targetMinutes = 60;
				break;
			case 5:
				this.targetMinutes = 180;
				break;
			default:
				this.targetMinutes = 360;
				break;
			}
			if (totalMinutes > (double)this.targetMinutes)
			{
				flag = true;
			}
		}
        if (!flag)
		{
			TimeSpan timeSpan = TimeSpan.FromMinutes((double)this.targetMinutes) - t;
			text = string.Concat(new object[]
			{
                "免费礼物倒计时：",
				timeSpan.Hours,
                "小时 ",
				timeSpan.Minutes + 1,
                "分钟"
            });
			if (this.giftNotReady.text != text)
			{
				this.giftNotReady.text = text;
			}
		}
		if (this.giftClaim.gameObject.activeInHierarchy == !flag)
		{
			this.giftClaim.gameObject.SetActive(flag);
		}
		if (this.giftNotReady.gameObject.activeInHierarchy == flag)
		{
			this.giftNotReady.gameObject.SetActive(!flag);
		}
       
		return flag;
	}

	public bool CheckForCoinGift()
	{
		bool flag = false;
		if (!SceneManager.instance.isEndless && !StoryManager.instance.levelCompleted)
		{
			return false;
		}
		if (this.coins >= 200)
		{
			flag = true;
		}
		if (!flag)
		{
			string text = "还需 " + (200 - this.coins) + " 个灵魂即可兑换";
			if (this.coinNotReady.text != text)
			{
				this.coinNotReady.text = text;
			}
		}
		if (this.coinClaim.gameObject.activeInHierarchy == !flag)
		{
			this.coinClaim.gameObject.SetActive(flag);
		}
		if (this.coinNotReady.gameObject.activeInHierarchy == flag)
		{
			this.coinNotReady.gameObject.SetActive(!flag);
		}
		return flag;
	}

	public void ResetFreeGiftTime()
	{
#if UNITY_ANDROID
#endif
        this.lastGiftTime = DateTime.Now;
        PlayerPrefs.SetString("lastGiftTime", DateTime.Now.ToString());
	}

	public void ClearAndScheduleNotifications()
	{
    }

    public void OnBeltExamAgainPressed()
	{
		int num = StoryManager.instance.level * 50;
		if (StoryManager.instance.level == 0)
		{
			num = 0;
		}
		else if (num < 200)
		{
			num = 200;
		}
		else if (num > 600)
		{
			num = 600;
		}
		if (this.coins >= num)
		{
			this.coins -= num;
			this.SaveCoins();
			SceneManager.instance.isBeltExamReady = true;
			SceneManager.instance.StartGame();
		}
		else
		{
			MenuManager.instance.Open();
			MenuManager.instance.ChangePanel(2);
		}
	}

	public void OnClaimPressed()
	{
		if (SceneManager.instance.gettingItem)
		{
			return;
		}
		if (this.CheckForFreeGift())
		{
			if (SceneManager.instance.currentState == SceneManager.State.gameOver)
			{
				SceneManager.instance.previousState = SceneManager.instance.currentState;
				SceneManager.instance.currentState = SceneManager.State.gotItem;
				MenuUIController.instance.CloseGameOver();
				int freeGift = this.GetFreeGift();
				StatManager.instance.stats[11].UpdateStat(StatManager.instance.stats[11].value + 1);
				StatManager.instance.UpdateStats();
				ItemManager.instance.GotCoin(freeGift);
				this.GetCoins(freeGift, true);
				if (this.coins >= 200)
				{
					this.willShowItem = true;
					if (!this.itemObject.activeInHierarchy)
					{
						this.itemObject.SetActive(true);
					}
				}
			}
		}
	}


	public void OnClaimWithCoinPressed()
	{
		if (SceneManager.instance.gettingItem)
		{
			return;
		}
		if (this.CheckForCoinGift())
		{
            if (SceneManager.instance.currentState == SceneManager.State.gameOver)
            {
                SceneManager.instance.previousState = SceneManager.instance.currentState;
                SceneManager.instance.currentState = SceneManager.State.gotItem;
                MenuUIController.instance.CloseGameOver();
                StatManager.instance.stats[10].UpdateStat(StatManager.instance.stats[10].value + 200);
                StatManager.instance.UpdateStats();
                this.coins -= 200;
                this.SaveCoins();
                if (this.gotItemCount == 0)
                {
                    this.gotItemCount = ++this.gotItemCount;
                    PlayerPrefs.SetInt("gotItemCount", this.gotItemCount);
                    ItemManager.instance.GotItem(2, "viking", false, false);
                }
                else if (this.gotItemCount == 1)
                {
                    this.gotItemCount = ++this.gotItemCount;
                    PlayerPrefs.SetInt("gotItemCount", this.gotItemCount);
                    ItemManager.instance.GotItem(1, "sword1h0", false, true);
                }
                else if (this.gotItemCount == 2)
                {
                    this.gotItemCount = ++this.gotItemCount;
                    PlayerPrefs.SetInt("gotItemCount", this.gotItemCount);
                    ItemManager.instance.GotItem(1, "dagger2", false, false);
                }
                else
                {
                    ItemManager.instance.GotItem();
                }
            }
		}
	}

	public void OnWatchPressed()
	{
		return;
	}

	public void GetCoins(int amt, bool save = false)
	{
		this.coins += amt;
		if (save)
		{
			this.SaveCoins();
		}
	}

	public void SaveCoins()
	{
        PlayerPrefs.SetInt("coins", this.coins);
	}

	public void ShowAd()
	{
		return;
    }

	public void OnSharePressed()
	{
    }



}
