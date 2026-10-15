// 反编译自程序集： Assembly-CSharp.dll
using Newtonsoft.Json;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using XftWeapon;
using DG.Tweening;
using UnityEngine.UI.Extensions;

public class ItemManager : MonoBehaviour
{
	public static ItemManager instance;


    public GameObject _itemPanel, _buyallPanel,_buyAllObj,_confirmObj;

	public bool itemMenuOpen;

	public SkeletonAnimation spine;

	public SpriteRenderer hatRenderer;

	public Animator radialOpenAnim;

	public CanvasGroup gotItem;

	public bool gotItemOpen;

	public Image gotWeaponImage;

	public Image gotBeltImage;

	public Image gotBeltAltImage;

	public Image gotHatImage;

	public Text gotWeaponText;

	public Text gotBeltText;

	public Text youEarnedText;

	public Text gotHatText;

	public SpriteRenderer droppingItem;

	public XWeaponTrail[] weaponTrails;

	public SpriteRenderer[] weaponSpriteRenderers;

	public SpriteRenderer[] weaponGlows;

	public Rigidbody2D[] rigidBodies;

	public Transform[] itemParents;

	public Dictionary<string, Weapon> weaponsById;

	public Dictionary<string, Hat> hatsById;

    public GameObject _prefabItem;

    public GameObject _listWeapon, _listHat;

    public VerticalScrollSnap verticalScrollSnapHat;
	public HorizontalScrollSnap horizontalScrollSnapHat;

    public RectTransform contentItemHat;

    public VerticalScrollSnap verticalScrollSnapEquipment;
	public HorizontalScrollSnap horizontalScrollSnapEquipment;

	public RectTransform contentItemWeapon;

  

	public Text itemCountText;

	public int itemCount;

	private int scrollerType;

	public CanvasGroup itemsMenuCanvas;

	public CanvasGroup scoreCanvas;

	public GameObject loadingObject;

	public Weapon currentWeapon;

	public Weapon previousWeapon;

	public Hat currentHat;

	public Hat previousHat;

	

	//public Renderer coinRenderer;

	public bool allItemsBought;

	private bool isLastItemBelt;

	public Text lockedText;

	public GameObject beltKnot;

	public Rigidbody2D beltRb;

	public KungfuBelt kungfuBelt;

	public KungfuBelt beltPrefab;

	public Color[] beltColors;

	public Text hatDuplicateText;

	public Text weaponDuplicateText;

	public Sprite beltSprite;

    private bool isWeaponList = true;

	private bool registerNotifs;

	private bool isDuplicate;

	private bool endStory;

	public List<Hat> npcHats = new List<Hat>();

	public List<Weapon> npcWeapons = new List<Weapon>();

	private string currentIapId;

	public Text itemPriceText;

	private static Action<GameObject> __f__am_cache0;

	private void Awake()
	{
	}

	private void Start()
	{
		
		ItemManager.instance = this;
		this.weaponsById = new Dictionary<string, Weapon>();
		this.hatsById = new Dictionary<string, Hat>();
		ConfigRoot configRoot = JsonConvert.DeserializeObject<ConfigRoot>(Resources.Load<TextAsset>("ItemConfig").text);
		List<WeaponCategory> weaponCategories = configRoot.weaponCategories;
		int num = 0;
		int num2 = 0;
		foreach (WeaponCategory current in weaponCategories)
		{
			current.index = num;
			num++;
			int num3 = 0;
			foreach (Weapon current2 in current.weapons)
			{
				if (current2.unlockLevel == 0 && !current.twoHanded)
				{
					this.npcWeapons.Add(current2);
				}
				current2.Instantiate(current, num3);
				current2.totalIndex = num2;
				this.weaponsById.Add(current2.id, current2);
				num3++;
				num2++;
			}
			current.weaponCount = num3;
		}
		int num4 = 0;
		foreach (Hat current3 in configRoot.hats)
		{
			current3.Instantiate(num4);
			this.hatsById.Add(current3.id, current3);
			if (current3.unlockLevel == 0)
			{
				this.npcHats.Add(current3);
			}
			num4++;
		}
		this.allItemsBought = PlayerPrefsX.GetBool("allItemsBought", false);
		this.spine.state.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.OnSpineEvent);
		this.currentWeapon = this.weaponsById[PlayerPrefs.GetString("currentWeapon", "fist0")];
        if (StickmanCombat.Gameplay.GameFlowController.instance != null && !StickmanCombat.Gameplay.WeaponCombatProfile.IsSelectable(this.currentWeapon))
        {
            var oldProfile = StickmanCombat.Gameplay.WeaponCombatProfile.For(this.currentWeapon);
            var oldRank = StickmanCombat.Gameplay.WeaponCombatProfile.RequiredRank(this.currentWeapon);
            foreach (Weapon candidate in this.weaponsById.Values)
            {
                if (StickmanCombat.Gameplay.WeaponCombatProfile.IsSelectable(candidate)
                    && StickmanCombat.Gameplay.WeaponCombatProfile.RequiredRank(candidate) == oldRank
                    && StickmanCombat.Gameplay.WeaponCombatProfile.For(candidate).effect == oldProfile.effect)
                {
                    this.currentWeapon = candidate;
                    PlayerPrefs.SetString("currentWeapon", candidate.id);
                    break;
                }
            }
        }
		this.currentHat = this.hatsById[PlayerPrefs.GetString("currentHat", "hat0")];

        this.CheckIfCurrentItemsOwned();
		this.InitWeaponTrails();
		this.EquipCurrentWeapon(false);
		this.EquipCurrentHat(false);
		if (this.currentWeapon.id == "fist0" && this.currentHat.id == "hat0")
		{
			CharacterManager.instance.spine.state.SetAnimation(0, "meditation", true);
		}
		this.UpdateBelt();

        if (StickmanCombat.Gameplay.GameFlowController.instance == null)
        {
            InstanWeaponList();
            InstanHatList();
        }
    }

	public void UpdateBelt()
	{
        if (StickmanCombat.Gameplay.GameFlowController.instance != null) { kungfuBelt.gameObject.SetActive(false); return; }
		SkeletonRenderer skeletonRenderer = this.kungfuBelt.boneFollower.skeletonRenderer;
		UnityEngine.Object.Destroy(this.kungfuBelt.gameObject);
		this.kungfuBelt = UnityEngine.Object.Instantiate<KungfuBelt>(this.beltPrefab);
		this.kungfuBelt.boneFollower.skeletonRenderer = skeletonRenderer;
		int num = StoryManager.instance.level;
		if (num == 0)
		{
			this.kungfuBelt.gameObject.SetActive(false);
		}
		else
		{
			int num2 = num - 1;
			if (num2 > 15)
			{
				num2 = 15;
			}
			this.kungfuBelt.gameObject.SetActive(true);
			this.kungfuBelt.SetMainColor(this.beltColors[num2]);
			if (num2 != 0 && num2 % 2 == 0)
			{
				this.kungfuBelt.SetAltColor(Color.black);
			}
			else
			{
				this.kungfuBelt.SetAltColor(this.beltColors[num2]);
			}
		}
	}

	public void Update()
	{
        if (StickmanCombat.Gameplay.GameFlowController.instance != null) return;
        if ((SceneManager.instance.gameStarted && (SceneManager.instance.isEndless || StoryManager.instance.introCompleted)) || (SceneManager.instance.currentState == SceneManager.State.gameOver && !this.gotItemOpen && !this.itemMenuOpen && !SceneManager.instance.gettingItem))
		{
			if (!this.scoreCanvas.gameObject.activeInHierarchy)
			{
				this.scoreCanvas.gameObject.SetActive(true);
			}
			if (this.scoreCanvas.alpha < 1f)
			{
				this.scoreCanvas.alpha += Time.deltaTime;
			}
			else
			{
				this.scoreCanvas.alpha = 1f;
			}
		}
		else if (this.scoreCanvas.alpha > 0f)
		{
			this.scoreCanvas.alpha -= Time.deltaTime;
		}
		else
		{
			this.scoreCanvas.alpha = 0f;
			this.scoreCanvas.gameObject.SetActive(false);
		}
		if (!this.gotItemOpen)
		{
			//if (this.coinRenderer.transform.localScale.x > 0f)
			//{
			//	this.coinRenderer.transform.localScale -= new Vector3(14f, 14f, 2.1f) * Time.deltaTime * 3f;
			//}
			//else
			//{
			//	this.coinRenderer.transform.localScale = Vector3.zero;
			//	this.coinRenderer.gameObject.SetActive(false);
			//}
		}
		else
		{
			if (CharacterManager.instance.spine.timeScale != 1f)
			{
				CharacterManager.instance.spine.timeScale = 1f;
			}
			if (Time.timeScale != 1f)
			{
				Time.timeScale = 1f;
			}
			//if (this.coinRenderer.transform.localScale.x < 14f)
			//{
			//	this.coinRenderer.transform.localScale += new Vector3(14f, 14f, 2.1f) * Time.deltaTime * 3f;
			//}
			//else
			//{
			//	this.coinRenderer.transform.localScale = new Vector3(14f, 14f, 2.1f);
			//}
		}
		if (this.itemMenuOpen)
		{
			if (CharacterManager.instance.spine.timeScale != 1f)
			{
				CharacterManager.instance.spine.timeScale = 1f;
			}
			if (Time.timeScale != 1f)
			{
				Time.timeScale = 1f;
			}
			if (this.itemsMenuCanvas.alpha < 1f)
			{
				this.itemsMenuCanvas.alpha += Time.deltaTime / 2f;
			}
			else
			{
				this.itemsMenuCanvas.alpha = 1f;
			}

            string text = "";
            if (isWeaponList)
            {
                text = (verticalScrollSnapEquipment.CurrentPage + 1) + "/" + this.weaponsById.Count;
            } else
            {
                text = (verticalScrollSnapHat.CurrentPage + 1) + "/" + this.hatsById.Count;
            }
			if (this.itemCountText.text != text)
			{
				this.itemCountText.text = text;
			}
            if (isWeaponList)
            {
                if (this.currentWeapon != verticalScrollSnapEquipment.CurrentPageObject().GetComponent<ItemEquipment>()._weapon)
                {
                    this.currentWeapon = verticalScrollSnapEquipment.CurrentPageObject().GetComponent<ItemEquipment>()._weapon;
                    verticalScrollSnapEquipment.CurrentPageObject().GetComponent<ItemEquipment>().UpdateBuyButton();
                    if (verticalScrollSnapEquipment.CurrentPageObject().GetComponent<ItemEquipment>()._isBought)
                    {

                       
                        verticalScrollSnapEquipment.CurrentPageObject().GetComponent<ItemEquipment>()._continue.GetComponent<Button>().interactable = true;
                        verticalScrollSnapEquipment.CurrentPageObject().GetComponent<ItemEquipment>()._buyBtn.GetComponent<Button>().interactable = false;
                    }
                    else
                    {
                        verticalScrollSnapEquipment.CurrentPageObject().GetComponent<ItemEquipment>()._continue.GetComponent<Button>().interactable = false;
                        verticalScrollSnapEquipment.CurrentPageObject().GetComponent<ItemEquipment>()._buyBtn.GetComponent<Button>().interactable = true;
                    }
                    if (verticalScrollSnapEquipment.NextPageObject() != null)
                    {
                        verticalScrollSnapEquipment.NextPageObject().GetComponent<ItemEquipment>().UpdateBuyButton();
                        if (verticalScrollSnapEquipment.NextPageObject().GetComponent<ItemEquipment>()._isBought)
                        {
                          
                            verticalScrollSnapEquipment.NextPageObject().GetComponent<ItemEquipment>()._continue.GetComponent<Button>().interactable = false;
                            verticalScrollSnapEquipment.NextPageObject().GetComponent<ItemEquipment>()._buyBtn.GetComponent<Button>().interactable = false;
                        }
                        else
                        {
                        
                            verticalScrollSnapEquipment.NextPageObject().GetComponent<ItemEquipment>()._continue.GetComponent<Button>().interactable = false;
                            verticalScrollSnapEquipment.NextPageObject().GetComponent<ItemEquipment>()._buyBtn.GetComponent<Button>().interactable = false;
                        }
                    }
                    if (verticalScrollSnapEquipment.PreviousPageObject() != null)
                    {
                        verticalScrollSnapEquipment.PreviousPageObject().GetComponent<ItemEquipment>().UpdateBuyButton();
                        if (verticalScrollSnapEquipment.PreviousPageObject().GetComponent<ItemEquipment>()._isBought)
                        {
                          
                            verticalScrollSnapEquipment.PreviousPageObject().GetComponent<ItemEquipment>()._continue.GetComponent<Button>().interactable = false;
                            verticalScrollSnapEquipment.PreviousPageObject().GetComponent<ItemEquipment>()._buyBtn.GetComponent<Button>().interactable = false;
                        }
                        else
                        {
                        
                            verticalScrollSnapEquipment.PreviousPageObject().GetComponent<ItemEquipment>()._continue.GetComponent<Button>().interactable = false;
                            verticalScrollSnapEquipment.PreviousPageObject().GetComponent<ItemEquipment>()._buyBtn.GetComponent<Button>().interactable = false;
                        }
                    }
                    this.EquipCurrentWeapon(false);
                }
            }
            else
            {
                if (this.currentHat != verticalScrollSnapHat.CurrentPageObject().GetComponent<ItemEquipment>()._hat)
                {
                    this.currentHat = verticalScrollSnapHat.CurrentPageObject().GetComponent<ItemEquipment>()._hat;
                    verticalScrollSnapHat.CurrentPageObject().GetComponent<ItemEquipment>().UpdateBuyButton();
                    if (verticalScrollSnapHat.CurrentPageObject().GetComponent<ItemEquipment>()._isBought)
                    {
                      
                        verticalScrollSnapHat.CurrentPageObject().GetComponent<ItemEquipment>()._continue.GetComponent<Button>().interactable = true;
                        verticalScrollSnapHat.CurrentPageObject().GetComponent<ItemEquipment>()._buyBtn.GetComponent<Button>().interactable = false;
                    }
                    else
                    {
                        verticalScrollSnapHat.CurrentPageObject().GetComponent<ItemEquipment>()._continue.GetComponent<Button>().interactable = false;
                        verticalScrollSnapHat.CurrentPageObject().GetComponent<ItemEquipment>()._buyBtn.GetComponent<Button>().interactable = true;
                    }
                    if (verticalScrollSnapHat.NextPageObject() != null)
                    {
                        verticalScrollSnapHat.NextPageObject().GetComponent<ItemEquipment>().UpdateBuyButton();
                        if (verticalScrollSnapHat.NextPageObject().GetComponent<ItemEquipment>()._isBought)
                        {
                           
                            verticalScrollSnapHat.NextPageObject().GetComponent<ItemEquipment>()._continue.GetComponent<Button>().interactable = false;
                            verticalScrollSnapHat.NextPageObject().GetComponent<ItemEquipment>()._buyBtn.GetComponent<Button>().interactable = false;
                        }
                        else
                        {
                          
                            verticalScrollSnapHat.NextPageObject().GetComponent<ItemEquipment>()._continue.GetComponent<Button>().interactable = false;
                            verticalScrollSnapHat.NextPageObject().GetComponent<ItemEquipment>()._buyBtn.GetComponent<Button>().interactable = false;
                        }
                    }
                    if (verticalScrollSnapHat.PreviousPageObject() != null)
                    {
                        verticalScrollSnapHat.PreviousPageObject().GetComponent<ItemEquipment>().UpdateBuyButton();
                        if (verticalScrollSnapHat.PreviousPageObject().GetComponent<ItemEquipment>()._isBought)
                        {
                            verticalScrollSnapHat.PreviousPageObject().GetComponent<ItemEquipment>()._continue.GetComponent<Button>().interactable = false;
                            verticalScrollSnapHat.PreviousPageObject().GetComponent<ItemEquipment>()._buyBtn.GetComponent<Button>().interactable = false;
                        }
                        else
                        {
                            verticalScrollSnapHat.PreviousPageObject().GetComponent<ItemEquipment>()._continue.GetComponent<Button>().interactable = false;
                            verticalScrollSnapHat.PreviousPageObject().GetComponent<ItemEquipment>()._buyBtn.GetComponent<Button>().interactable = false;
                        }
                    }
                    this.EquipCurrentHat(false);
                }
            }
			
			
		}
		else if (this.itemsMenuCanvas.alpha > 0f)
		{
			this.itemsMenuCanvas.alpha -= Time.deltaTime * 2f;
		}
		else
		{
			this.itemsMenuCanvas.alpha = 0f;
			this.itemsMenuCanvas.gameObject.SetActive(false);
		}
		if (!this.gotItemOpen)
		{
			if (this.gotItem.alpha > 0f)
			{
				this.gotItem.alpha -= Time.deltaTime * 3f;
			}
			else
			{
				this.gotItem.gameObject.SetActive(false);
			}
		}
		else if (this.gotItem.alpha < 1f)
		{
			this.gotItem.alpha += Time.deltaTime * 3f;
		}
	}

	public string LevelToBeltString(int level)
	{
		string result;
		switch (level)
		{
		case 0:
			result = "无腰";
			break;
		case 1:
			result = "白";
			break;
		case 2:
			result = "黄";
			break;
		case 3:
			result = "黄黑";
			break;
		case 4:
			result = "绿";
			break;
		case 5:
			result = "绿黑";
			break;
		case 6:
			result = "紫";
			break;
		case 7:
			result = "紫黑";
			break;
		case 8:
			result = "橙";
			break;
		case 9:
			result = "橙黑";
			break;
		case 10:
			result = "蓝";
			break;
		case 11:
			result = "蓝黑";
			break;
		case 12:
			result = "棕";
			break;
		case 13:
			result = "棕黑";
			break;
		case 14:
			result = "红";
			break;
		case 15:
			result = "红黑";
			break;
		case 16:
			result = "黑";
			break;
		default:
			result = "暗影";
			break;
		}
		return result;
	}

	public void BuyButtonPressed(string itemId)
	{
		if (itemId == null || itemId == string.Empty)
		{
			itemId = this.currentIapId;
		}
		this.loadingObject.SetActive(true);
		AnalyticsManager.instance.IapButtonPressed(itemId);
		StoreManager.instance.BuyProductID(itemId);
	}

	public void RestorePurchasesButtonPressed()
	{
		this.loadingObject.SetActive(true);
		//StoreManager.instance.RestorePurchases();
	}

	public void GotItem()
	{
		int num = UnityEngine.Random.Range(1, 3);
		string id = string.Empty;
		if (num == 1)
		{
			List<Weapon> list = new List<Weapon>();
			foreach (KeyValuePair<string, Weapon> current in this.weaponsById)
			{
				if (current.Value.id != "fist0" && (current.Value.unlockLevel <= 0 || current.Value.unlockLevel <= StoryManager.instance.level))
				{
					list.Add(current.Value);
				}
			}
			id = list[UnityEngine.Random.Range(0, list.Count)].id;
		}
		else
		{
			List<Hat> list2 = new List<Hat>();
			foreach (KeyValuePair<string, Hat> current2 in this.hatsById)
			{
				if (current2.Value.id != "hat0" && (current2.Value.unlockLevel <= 0 || current2.Value.unlockLevel <= StoryManager.instance.level))
				{
					list2.Add(current2.Value);
				}
			}
			id = list2[UnityEngine.Random.Range(0, list2.Count)].id;
		}
		this.GotItem(num, id, false, false);
	}

	public void GotItem(int type, string id, bool isBelt = false, bool registerNotifs = false)
	{
        if (StickmanCombat.Gameplay.GameFlowController.instance != null)
        {
            if (type == 1 && weaponsById.ContainsKey(id))
            {
                if (!weaponsById[id].isOwned) weaponsById[id].Own();
                StickmanCombat.Gameplay.GameFlowController.instance.QueueUnlock(weaponsById[id].GetName());
            }
            else if (type == 2 && hatsById.ContainsKey(id))
            {
                if (!hatsById[id].isOwned) hatsById[id].Own();
                StickmanCombat.Gameplay.GameFlowController.instance.QueueUnlock(hatsById[id].GetName());
            }
            return;
        }
        if(SceneManager.instance.currentState != SceneManager.State.gotItem)
        {
            SceneManager.instance.ChangeState(SceneManager.State.gotItem);
            //SceneManager.instance.previousState = SceneManager.instance.currentState;
            //SceneManager.instance.currentState = SceneManager.State.gotItem;
        }
		if (registerNotifs)
		{
			this.registerNotifs = registerNotifs;
		}
		this.isLastItemBelt = false;
		this.isDuplicate = false;
		SceneManager.instance.gettingItem = true;
		this.CloseItemMenu();
		this.DeactivateAllTrails();
		NpcManager.instance.PauseAll();
		this.weaponSpriteRenderers[0].gameObject.SetActive(false);
		this.weaponSpriteRenderers[1].gameObject.SetActive(false);
		this.weaponSpriteRenderers[2].gameObject.SetActive(false);
        this.weaponSpriteRenderers[3].gameObject.SetActive(false);
        this.weaponSpriteRenderers[4].gameObject.SetActive(false);
        this.droppingItem.gameObject.SetActive(true);
        this.droppingItem.transform.DOMoveX(CharacterManager.instance.transform.position.x, 1f);
        this.droppingItem.transform.DOMoveY(CharacterManager.instance.transform.position.y+3, 1f).OnComplete(()=> { this.droppingItem.transform.localPosition = Vector3.zero; });
        this.gotWeaponImage.gameObject.SetActive(false);
		this.gotHatImage.gameObject.SetActive(false);
		this.gotBeltImage.gameObject.SetActive(false);
        this.spine.state.TimeScale = 0.5f;
		this.spine.state.SetAnimation(0, "CatchingBox", false);
		if (isBelt)
		{
			this.isLastItemBelt = true;
			int num = StoryManager.instance.level;
			if (num > 15)
			{
				num = 15;
			}
			this.droppingItem.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
			this.droppingItem.color = this.beltColors[num];
			this.droppingItem.gameObject.SetActive(true);
			this.droppingItem.sprite = this.beltSprite;
			this.gotBeltImage.gameObject.SetActive(true);
			this.gotBeltImage.color = this.beltColors[num];
			if (num != 0 && num % 2 == 0)
			{
				this.gotBeltAltImage.gameObject.SetActive(true);
			}
			else
			{
				this.gotBeltAltImage.gameObject.SetActive(false);
			}
            if (StoryManager.instance.level > 16 )
            {
                this.youEarnedText.text = "恭喜！";
                this.gotBeltText.text = "你已成为暗影武士";
            }
            else
            {
                this.youEarnedText.text = "你获得了";
                this.gotBeltText.text = this.LevelToBeltString(num + 1) + "带";
            }
            this.endStory = true;
			return;
		}
		this.endStory = false;
		this.droppingItem.color = Color.white;
		if (type == 1)
		{
			this.previousWeapon = this.currentWeapon;
			this.currentWeapon = this.weaponsById[id];
			if (this.weaponsById[id].IsOwned())
			{
				StatManager.instance.stats[12].UpdateStat(StatManager.instance.stats[12].value + 1);
				StatManager.instance.UpdateStats();
				this.isDuplicate = true;
				this.weaponDuplicateText.text = "已拥有";
			}
			else
			{
				this.weaponDuplicateText.text = "新获得";
				this.currentWeapon.Own();
			}
			this.gotWeaponImage.gameObject.SetActive(true);
			this.gotWeaponImage.sprite = this.currentWeapon.sprite;
			this.gotWeaponImage.SetNativeSize();
			this.gotWeaponImage.rectTransform.sizeDelta = this.gotWeaponImage.rectTransform.sizeDelta / 2f;
			this.droppingItem.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -45f));
			this.droppingItem.sprite = this.currentWeapon.sprite;
			this.gotWeaponText.text = this.currentWeapon.GetName();
			if (this.isDuplicate)
			{
				this.currentWeapon = this.previousWeapon;
			}
		}
		else
		{
			this.previousHat = this.currentHat;
			this.currentHat = this.hatsById[id];
			if (this.currentHat.IsOwned())
			{
				StatManager.instance.stats[12].UpdateStat(StatManager.instance.stats[12].value + 1);
				StatManager.instance.UpdateStats();
				this.isDuplicate = true;
				this.hatDuplicateText.text = "已拥有";
			}
			else
			{
				this.hatDuplicateText.text = "新获得";
				this.currentHat.Own();
			}
			this.gotHatImage.gameObject.SetActive(true);
			this.gotHatImage.sprite = this.currentHat.sprite;
			this.gotHatImage.SetNativeSize();
			this.gotHatImage.rectTransform.sizeDelta = this.gotHatImage.rectTransform.sizeDelta / 2f;
			this.droppingItem.transform.rotation = Quaternion.identity;
			this.droppingItem.sprite = this.currentHat.sprite;
			this.gotHatText.text = this.currentHat.GetName();
			if (this.isDuplicate)
			{
				this.currentHat = this.previousHat;
			}
		}
		this.droppingItem.gameObject.SetActive(true);
	}

	public void GotCoin(int amount)
	{
		AudioManager.instance.GotItemCoinSound();
		this.endStory = false;
		SceneManager.instance.gettingItem = true;
        NpcManager.instance.PauseAll();
        //this.coinRenderer.gameObject.SetActive(true);
        this.gotWeaponImage.gameObject.SetActive(true);
		this.gotHatImage.gameObject.SetActive(false);
		this.gotBeltImage.gameObject.SetActive(false);
        this.gotWeaponImage.transform.GetChild(0).gameObject.SetActive(true);
		this.gotWeaponImage.sprite = this.hatsById["hat0"].sprite;
		this.gotWeaponImage.rectTransform.sizeDelta = new Vector2(250f, 250f);
		this.gotWeaponText.text = amount + " 个灵魂";
		this.weaponDuplicateText.text = string.Empty;
		this.gotItemOpen = true;
		this.gotItem.alpha = 0f;
		this.gotItem.gameObject.SetActive(true);
		this.radialOpenAnim.Play("radialOpen");
	}

	private void OnSpineEvent(TrackEntry entry, Spine.Event e)
	{
		if (e.data.name == "Cought")
		{
			if (this.isLastItemBelt)
			{
				AudioManager.instance.GotItemBeltSound();
			}
			else
			{
				AudioManager.instance.GotItemItemSound();
			}
			this.gotItemOpen = true;
			this.gotItem.alpha = 0f;
			this.gotItem.gameObject.SetActive(true);
			this.radialOpenAnim.Play("radialOpen");
            this.droppingItem.transform.localPosition = Vector3.zero;
            this.spine.state.TimeScale = 1f;
            this.droppingItem.gameObject.SetActive(false);
			this.EquipCurrentWeapon(true);
			this.EquipCurrentHat(true);
		}
	}

	public void EquipCurrentWeapon(bool save = false)
	{
        Vector3 rightPos = Vector3.zero;
        float rightRotZ = 0;
        Vector3 rightScale = Vector3.zero;
        Vector3 leftPos = Vector3.zero;
        float leftRotZ = 0;
        Vector3 leftScale = Vector3.zero;

        var N = SimpleJSON.JSON.Parse(Resources.Load<TextAsset>("ItemPos").text);
        for (int i = 0; i < N["weaponCategories"].Count; i++)
        {
            if (this.currentWeapon.id.Contains(N["weaponCategories"][i]["id"].Value.ToLower()))
            {
                if (this.currentWeapon.EnglishName != "Unarmed")
                {
                    for (int j = 0; j < N["weaponCategories"][i]["weapons"].Count; j++)
                    {
                        if (this.currentWeapon.EnglishName == N["weaponCategories"][i]["weapons"][j]["EnglishName"].Value)
                        {
                            if (this.currentWeapon.id.Contains("fist") || this.currentWeapon.id.Contains("dagger"))
                            {
                                rightPos = new Vector3(N["weaponCategories"][i]["weapons"][j]["rightPos"]["x"].AsFloat, N["weaponCategories"][i]["weapons"][j]["rightPos"]["y"].AsFloat, 0f);
                                rightRotZ = N["weaponCategories"][i]["weapons"][j]["rightRotZ"].AsFloat;
                                rightScale = new Vector3(N["weaponCategories"][i]["weapons"][j]["rightLocalScale"]["x"].AsFloat, N["weaponCategories"][i]["weapons"][j]["rightLocalScale"]["y"].AsFloat, 1f);
                                leftPos = new Vector3(N["weaponCategories"][i]["weapons"][j]["leftPos"]["x"].AsFloat, N["weaponCategories"][i]["weapons"][j]["leftPos"]["y"].AsFloat, 0f);
                                leftRotZ = N["weaponCategories"][i]["weapons"][j]["leftRotZ"].AsFloat;
                                leftScale = new Vector3(N["weaponCategories"][i]["weapons"][j]["leftLocalScale"]["x"].AsFloat, N["weaponCategories"][i]["weapons"][j]["leftLocalScale"]["y"].AsFloat, 1f);

                            } else
                            {
                                rightPos = new Vector3(N["weaponCategories"][i]["weapons"][j]["Pos"]["x"].AsFloat, N["weaponCategories"][i]["weapons"][j]["Pos"]["y"].AsFloat, 0f);
                                rightRotZ = N["weaponCategories"][i]["weapons"][j]["RotZ"].AsFloat;
                                rightScale = new Vector3(N["weaponCategories"][i]["weapons"][j]["LocalScale"]["x"].AsFloat, N["weaponCategories"][i]["weapons"][j]["LocalScale"]["y"].AsFloat, 1f);
                            }
                        }
                    }
                }
            }
        }

        this.DeactivateAllTrails();
		if (this.spine.state.GetCurrent(0).animation.name != ItemManager.instance.GetIdleString())
		{
			this.spine.state.SetAnimation(0, ItemManager.instance.GetIdleString(), true);
		}
		if (this.currentWeapon.category.twoHanded)
		{
			this.weaponSpriteRenderers[0].gameObject.SetActive(false);
			this.weaponSpriteRenderers[1].gameObject.SetActive(false);
			this.weaponSpriteRenderers[2].gameObject.SetActive(true);
            this.weaponSpriteRenderers[3].gameObject.SetActive(false);
            this.weaponSpriteRenderers[4].gameObject.SetActive(false);
            this.weaponGlows[2].transform.localPosition = this.currentWeapon.startPos + (this.currentWeapon.endPos - this.currentWeapon.startPos) / 2f;
			this.weaponTrails[2].PointStart.localPosition = this.currentWeapon.startPos;
			this.weaponTrails[2].PointEnd.localPosition = this.currentWeapon.endPos;
			this.weaponTrails[2].Init(true);
			this.weaponSpriteRenderers[2].sprite = this.currentWeapon.sprite;
            this.weaponSpriteRenderers[2].transform.localPosition = rightPos;
            this.weaponSpriteRenderers[2].transform.localEulerAngles = new Vector3(0, 0, rightRotZ);
            this.weaponSpriteRenderers[2].transform.localScale = rightScale;
        }
		else if (!this.currentWeapon.category.dualWield)
		{
            this.weaponSpriteRenderers[0].gameObject.SetActive(false);
			this.weaponSpriteRenderers[1].gameObject.SetActive(true);
			this.weaponSpriteRenderers[2].gameObject.SetActive(false);
            this.weaponSpriteRenderers[3].gameObject.SetActive(false);
            this.weaponSpriteRenderers[4].gameObject.SetActive(false);
            this.weaponGlows[1].transform.localPosition = this.currentWeapon.startPos + (this.currentWeapon.endPos - this.currentWeapon.startPos) / 2f;
			this.weaponTrails[1].PointStart.localPosition = this.currentWeapon.startPos;
			this.weaponTrails[1].PointEnd.localPosition = this.currentWeapon.endPos;
			this.weaponTrails[1].Init(true);
			this.weaponSpriteRenderers[1].sprite = this.currentWeapon.sprite;
            this.weaponSpriteRenderers[1].transform.localPosition = rightPos;
            this.weaponSpriteRenderers[1].transform.localEulerAngles = new Vector3(0, 0, rightRotZ);
            this.weaponSpriteRenderers[1].transform.localScale = rightScale;
        }
		else
        {
            
            if (this.currentWeapon.id.Contains("fist"))
            {
                this.weaponSpriteRenderers[0].gameObject.SetActive(false);
                this.weaponSpriteRenderers[1].gameObject.SetActive(false);
                this.weaponSpriteRenderers[3].gameObject.SetActive(true);
                this.weaponSpriteRenderers[4].gameObject.SetActive(true);
                this.weaponGlows[3].transform.localPosition = this.currentWeapon.startPos + (this.currentWeapon.endPos - this.currentWeapon.startPos) / 2f;
                this.weaponGlows[4].transform.localPosition = this.currentWeapon.startPos + (this.currentWeapon.endPos - this.currentWeapon.startPos) / 2f;
                this.weaponTrails[3].PointStart.localPosition = this.currentWeapon.startPos;
                this.weaponTrails[3].PointEnd.localPosition = this.currentWeapon.endPos;
                this.weaponTrails[4].PointStart.localPosition = this.currentWeapon.startPos;
                this.weaponTrails[4].PointEnd.localPosition = this.currentWeapon.endPos;
                this.weaponTrails[3].Init(true);
                this.weaponTrails[4].Init(true);
                this.weaponSpriteRenderers[2].gameObject.SetActive(false);
                this.weaponSpriteRenderers[3].sprite = this.currentWeapon.sprite;
                this.weaponSpriteRenderers[4].sprite = this.currentWeapon.sprite;
                this.weaponSpriteRenderers[3].transform.localPosition = leftPos;
                this.weaponSpriteRenderers[3].transform.localEulerAngles = new Vector3(0, 0, leftRotZ);
                this.weaponSpriteRenderers[3].transform.localScale = leftScale;
                this.weaponSpriteRenderers[4].transform.localPosition = rightPos;
                this.weaponSpriteRenderers[4].transform.localEulerAngles = new Vector3(0, 0, rightRotZ);
                this.weaponSpriteRenderers[4].transform.localScale = rightScale;
            } else
            {

                this.weaponSpriteRenderers[0].gameObject.SetActive(true);
                this.weaponSpriteRenderers[1].gameObject.SetActive(true);
                this.weaponSpriteRenderers[3].gameObject.SetActive(false);
                this.weaponSpriteRenderers[4].gameObject.SetActive(false);
                this.weaponGlows[0].transform.localPosition = this.currentWeapon.startPos + (this.currentWeapon.endPos - this.currentWeapon.startPos) / 2f;
                this.weaponGlows[1].transform.localPosition = this.currentWeapon.startPos + (this.currentWeapon.endPos - this.currentWeapon.startPos) / 2f;
                this.weaponTrails[0].PointStart.localPosition = this.currentWeapon.startPos;
                this.weaponTrails[0].PointEnd.localPosition = this.currentWeapon.endPos;
                this.weaponTrails[1].PointStart.localPosition = this.currentWeapon.startPos;
                this.weaponTrails[1].PointEnd.localPosition = this.currentWeapon.endPos;
                this.weaponTrails[0].Init(true);
                this.weaponTrails[1].Init(true);
                this.weaponSpriteRenderers[2].gameObject.SetActive(false);
                this.weaponSpriteRenderers[0].sprite = this.currentWeapon.sprite;
                this.weaponSpriteRenderers[1].sprite = this.currentWeapon.sprite;
                this.weaponSpriteRenderers[0].transform.localPosition = leftPos;
                this.weaponSpriteRenderers[0].transform.localEulerAngles = new Vector3(0, 0, leftRotZ);
                this.weaponSpriteRenderers[0].transform.localScale = leftScale;
                this.weaponSpriteRenderers[1].transform.localPosition = rightPos;
                this.weaponSpriteRenderers[1].transform.localEulerAngles = new Vector3(0, 0, rightRotZ);
                this.weaponSpriteRenderers[1].transform.localScale = rightScale;
            }
			
		}
		this.DeactivateAllTrails();
		if (save)
		{
			PlayerPrefs.SetString("currentWeapon", this.currentWeapon.id);
		}
	}

	public void EquipCurrentHat(bool save = false)
	{
		this.hatRenderer.sprite = this.currentHat.sprite;
		if (save)
		{
			PlayerPrefs.SetString("currentHat", this.currentHat.id);
		}
        var N = SimpleJSON.JSON.Parse(Resources.Load<TextAsset>("ItemPos").text);
        for (int i = 0; i < N["hats"].Count; i++)
        {
            if (this.currentHat.id == N["hats"][i]["id"].Value)
            {
                Vector3 localPos = new Vector3(N["hats"][i]["Pos"]["x"].AsFloat, N["hats"][i]["Pos"]["y"].AsFloat, 0f);
                float rotationZ = N["hats"][i]["RotZ"].AsFloat;
                Vector3 localScale = new Vector3(N["hats"][i]["LocalScale"]["x"].AsFloat, N["hats"][i]["LocalScale"]["y"].AsFloat, 1f);
                this.hatRenderer.transform.localPosition = localPos;
                this.hatRenderer.transform.localEulerAngles = new Vector3(0, 0, rotationZ);
                this.hatRenderer.transform.localScale = localScale;
            }
        }
    }

    public void CloseGotItem()
    {
        //SceneManager.instance.currentState == SceneManager.State.gotItem && //会出bug
        if (!MenuUIController.instance.isOpenGameOver)
        {
            this.spine.timeScale = 1;
            this.gotWeaponImage.transform.GetChild(0).gameObject.SetActive(false);
            if (this.registerNotifs)
            {
                this.registerNotifs = false;
                if (!MonetizationManager.instance.notifToggle.isOn)
                {
                    MonetizationManager.instance.notifToggle.OnToggle();
                }
            }


            this.droppingItem.gameObject.SetActive(false);

            if (this.gotItem.gameObject.activeInHierarchy)
            {
                this.radialOpenAnim.Play("radialClose");
            }
            //&& !MenuUIController.instance.isOpenGameOver //多余
            if (SceneManager.instance.currentState == SceneManager.State.gotItem && SceneManager.instance.previousState == SceneManager.State.gameOver)
            {
                MenuUIController.instance.OpenGameOver();
            }
            //!MenuUIController.instance.isOpenGameOver && //多余
            else if (SceneManager.instance.currentState != SceneManager.State.gameOver && SceneManager.instance.previousState != SceneManager.State.gameOver)
            {
                NpcManager.instance.ContinueAll();

                MenuUIController.instance.OpenMenu();
            }
            else
            {
                //瞎几把点导致状态发生意外变化，强制修改状态
                SceneManager.instance.currentState = SceneManager.State.gotItem;
                MenuUIController.instance.OpenGameOver();
            }
            if (this.endStory)
            {
                NpcManager.instance.ContinueAll();

                StoryManager.instance.LevelCompleted();
            }
        }
	}

	public string GetIdleString()
	{
		return this.currentWeapon.category.id + "Idle";
	}

	public string GetAttackString(int comboIndex)
	{
      
           return this.currentWeapon.category.id + "Attack" + (comboIndex);

	}

	public Vector2 GetImpactVector(int comboNo)
	{
		return new Vector2((float)this.currentWeapon.category.hitVectors[comboNo].x, (float)this.currentWeapon.category.hitVectors[comboNo].y);
	}

	public void ToggleTrails(bool on)
	{
		if (!on)
		{
			XWeaponTrail[] array = this.weaponTrails;
			for (int i = 0; i < array.Length; i++)
			{
				XWeaponTrail xWeaponTrail = array[i];
				if (xWeaponTrail.transform.parent.gameObject.activeInHierarchy)
				{
					xWeaponTrail.StopSmoothly(0.5f);
				}
			}
		}
		else
		{
			XWeaponTrail[] array2 = this.weaponTrails;
			for (int j = 0; j < array2.Length; j++)
			{
				XWeaponTrail xWeaponTrail2 = array2[j];
				if (xWeaponTrail2.transform.parent.gameObject.activeInHierarchy)
				{
					xWeaponTrail2.Activate();
				}
			}
		}
	}

	public void DeactivateAllTrails()
	{
		if (this.weaponTrails[0].isActiveAndEnabled)
		{
			this.weaponTrails[0].StopSmoothly(0f);
			this.weaponTrails[0].UpdateFade();
			this.weaponTrails[0].Deactivate();
		}
		if (this.weaponTrails[1].isActiveAndEnabled)
		{
			this.weaponTrails[1].StopSmoothly(0f);
			this.weaponTrails[1].UpdateFade();
			this.weaponTrails[1].Deactivate();
		}
		if (this.weaponTrails[2].isActiveAndEnabled)
		{
			this.weaponTrails[2].StopSmoothly(0f);
			this.weaponTrails[2].UpdateFade();
			this.weaponTrails[2].Deactivate();
		}
        if (this.weaponTrails[3].isActiveAndEnabled)
        {
            this.weaponTrails[3].StopSmoothly(0f);
            this.weaponTrails[3].UpdateFade();
            this.weaponTrails[3].Deactivate();
        }
        if (this.weaponTrails[4].isActiveAndEnabled)
        {
            this.weaponTrails[4].StopSmoothly(0f);
            this.weaponTrails[4].UpdateFade();
            this.weaponTrails[4].Deactivate();
        }
    }

	public void InitWeaponTrails()
	{
		this.weaponTrails[0].Init(false);
		this.weaponTrails[1].Init(false);
		this.weaponTrails[2].Init(false);
        this.weaponTrails[3].Init(false);
        this.weaponTrails[4].Init(false);
    }

	public void RecoverItems()
	{
        Vector3 rightPos = Vector3.zero;
        float rightRotZ = 0;
        Vector3 rightScale = Vector3.zero;
        Vector3 leftPos = Vector3.zero;
        float leftRotZ = 0;
        Vector3 leftScale = Vector3.zero;

        var N = SimpleJSON.JSON.Parse(Resources.Load<TextAsset>("ItemPos").text);
        for (int i = 0; i < N["weaponCategories"].Count; i++)
        {
            if (this.currentWeapon.id.Contains(N["weaponCategories"][i]["id"].Value.ToLower()))
            {
                if (this.currentWeapon.EnglishName != "Unarmed")
                {
                    for (int j = 0; j < N["weaponCategories"][i]["weapons"].Count; j++)
                    {
                        if (this.currentWeapon.EnglishName == N["weaponCategories"][i]["weapons"][j]["EnglishName"].Value)
                        {

                            if (this.currentWeapon.id.Contains("fist") || this.currentWeapon.id.Contains("dagger"))
                            {
                                rightPos = new Vector3(N["weaponCategories"][i]["weapons"][j]["rightPos"]["x"].AsFloat, N["weaponCategories"][i]["weapons"][j]["rightPos"]["y"].AsFloat, 0f);
                                rightRotZ = N["weaponCategories"][i]["weapons"][j]["rightRotZ"].AsFloat;
                                rightScale = new Vector3(N["weaponCategories"][i]["weapons"][j]["rightLocalScale"]["x"].AsFloat, N["weaponCategories"][i]["weapons"][j]["rightLocalScale"]["y"].AsFloat, 1f);
                                leftPos = new Vector3(N["weaponCategories"][i]["weapons"][j]["leftPos"]["x"].AsFloat, N["weaponCategories"][i]["weapons"][j]["leftPos"]["y"].AsFloat, 0f);
                                leftRotZ = N["weaponCategories"][i]["weapons"][j]["leftRotZ"].AsFloat;
                                leftScale = new Vector3(N["weaponCategories"][i]["weapons"][j]["leftLocalScale"]["x"].AsFloat, N["weaponCategories"][i]["weapons"][j]["leftLocalScale"]["y"].AsFloat, 1f);

                            }
                            else
                            {
                                rightPos = new Vector3(N["weaponCategories"][i]["weapons"][j]["Pos"]["x"].AsFloat, N["weaponCategories"][i]["weapons"][j]["Pos"]["y"].AsFloat, 0f);
                                rightRotZ = N["weaponCategories"][i]["weapons"][j]["RotZ"].AsFloat;
                                rightScale = new Vector3(N["weaponCategories"][i]["weapons"][j]["LocalScale"]["x"].AsFloat, N["weaponCategories"][i]["weapons"][j]["LocalScale"]["y"].AsFloat, 1f);
                            }
                        }
                    }
                }
            }
        }

        if (this.currentWeapon.category.twoHanded)
		{
			this.rigidBodies[2].simulated = false;
			this.rigidBodies[2].transform.SetParent(this.itemParents[2]);
			this.rigidBodies[2].transform.localPosition = rightPos;
            this.rigidBodies[2].transform.localRotation = Quaternion.Euler(new Vector3(0, 0, rightRotZ));
            this.rigidBodies[2].transform.localScale= rightScale;

         
        }
        else if (!this.currentWeapon.category.dualWield) 
        {
            this.rigidBodies[1].simulated = false;
            this.rigidBodies[1].transform.SetParent(this.itemParents[1]);
            this.rigidBodies[1].transform.localPosition = rightPos;
            this.rigidBodies[1].transform.localRotation = Quaternion.Euler(new Vector3(0, 0, rightRotZ));
            this.rigidBodies[1].transform.localScale = rightScale;
        }
        else
		{
            if (!this.currentWeapon.id.Contains("fist"))
            {
                this.rigidBodies[0].simulated = false;
                this.rigidBodies[0].transform.SetParent(this.itemParents[0]);
                this.rigidBodies[0].transform.localPosition = leftPos;
                this.rigidBodies[0].transform.localRotation = Quaternion.Euler(new Vector3(0, 0, leftRotZ));
                this.rigidBodies[0].transform.localScale = leftScale;

                this.rigidBodies[1].simulated = false;
                this.rigidBodies[1].transform.SetParent(this.itemParents[1]);
                this.rigidBodies[1].transform.localPosition = rightPos;
                this.rigidBodies[1].transform.localRotation = Quaternion.Euler(new Vector3(0, 0, rightRotZ));
                this.rigidBodies[1].transform.localScale = rightScale;
            } else
            {
                this.rigidBodies[4].simulated = false;
                this.rigidBodies[4].transform.SetParent(this.itemParents[4]);
                this.rigidBodies[4].transform.localPosition = leftPos;
                this.rigidBodies[4].transform.localRotation = Quaternion.Euler(new Vector3(0, 0, leftRotZ));
                this.rigidBodies[4].transform.localScale = leftScale;

                this.rigidBodies[5].simulated = false;
                this.rigidBodies[5].transform.SetParent(this.itemParents[5]);
                this.rigidBodies[5].transform.localPosition = rightPos;
                this.rigidBodies[5].transform.localRotation = Quaternion.Euler(new Vector3(0, 0, rightRotZ));
                this.rigidBodies[5].transform.localScale = rightScale;
            }
		}
		
	}

	public void DropItems(Vector2 impactVector)
	{
		if (this.currentWeapon.category.twoHanded)
		{
			this.rigidBodies[2].simulated = true;
			this.rigidBodies[2].velocity = impactVector + new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
			this.rigidBodies[2].angularVelocity = (float)(-(float)UnityEngine.Random.Range(50, 200));
			this.rigidBodies[2].transform.SetParent(null);
		}
        else if (!this.currentWeapon.category.dualWield)
        {

            this.rigidBodies[1].simulated = true;
            this.rigidBodies[1].velocity = impactVector + new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
            this.rigidBodies[1].angularVelocity = (float)(-(float)UnityEngine.Random.Range(50, 200));
            this.rigidBodies[1].transform.SetParent(null);

        } else
        {
            if (!this.currentWeapon.id.Contains("fist"))

            {
                this.rigidBodies[0].simulated = true;
                this.rigidBodies[0].velocity = impactVector + new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
                this.rigidBodies[0].angularVelocity = (float)(-(float)UnityEngine.Random.Range(50, 200));
                this.rigidBodies[0].transform.SetParent(null);

                this.rigidBodies[1].simulated = true;
                this.rigidBodies[1].velocity = impactVector + new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
                this.rigidBodies[1].angularVelocity = (float)(-(float)UnityEngine.Random.Range(50, 200));
                this.rigidBodies[1].transform.SetParent(null);
            }
            else
            {
                this.rigidBodies[4].simulated = true;
                this.rigidBodies[4].velocity = impactVector + new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
                this.rigidBodies[4].angularVelocity = (float)(-(float)UnityEngine.Random.Range(50, 200));
                this.rigidBodies[4].transform.SetParent(null);

                this.rigidBodies[5].simulated = true;
                this.rigidBodies[5].velocity = impactVector + new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
                this.rigidBodies[5].angularVelocity = (float)(-(float)UnityEngine.Random.Range(50, 200));
                this.rigidBodies[5].transform.SetParent(null);
            }
        }
	}

	public void ToggleItemMenu()
	{
		this.itemMenuOpen = !this.itemMenuOpen;
		if (this.itemMenuOpen)
		{
			this.OpenItemMenu();
		}
		else
		{
			this.CloseItemMenu();
		}
	}

	public void CheckIfCurrentItemsOwned()
	{
		if (!this.currentHat.IsOwned())
		{
			if (this.previousHat == null || !this.previousHat.IsOwned())
			{
				this.previousHat = (this.currentHat = this.hatsById["hat0"]);
			}
			else
			{
				this.currentHat = this.previousHat;
			}
		}
		if (!this.currentWeapon.IsOwned())
		{
			if (this.previousWeapon == null || !this.previousWeapon.IsOwned())
			{
				this.previousWeapon = (this.currentWeapon = this.weaponsById["fist0"]);
			}
			else
			{
				this.currentWeapon = this.previousWeapon;
			}
		}
	}

	public void CloseItemMenu()
	{
		if (!this.itemMenuOpen)
		{
			return;
		}
		this.CheckIfCurrentItemsOwned();
        this.EquipCurrentWeapon(true);
		this.EquipCurrentHat(true);
		this.itemMenuOpen = false;
		SceneManager.instance.chaserObject.transform.localPosition = new Vector3(0f, -0.8f, 0f);
		NpcManager.instance.ContinueAll();
		SceneManager.instance.targetCameraZ = -9f;
        Camera.main.GetComponent<CombatCameraFollow>().OverallOffset.x = 0f;
        Camera.main.GetComponent<CombatCameraFollow>().OverallOffset.y = 0f;
        if (!SceneManager.instance.gettingItem)
		{
			if (SceneManager.instance.currentState == SceneManager.State.gameOver)
			{
                //SceneManager.instance.menu.Play("menuGameOver");
                MenuUIController.instance.OpenGameOver();
            }
			else
			{
                //SceneManager.instance.headingAnim.Play("headingStart");
                MenuUIController.instance.ShowLogo();
                MenuUIController.instance.OpenMenu();
                //SceneManager.instance.menu.Play("menuOpen");
            }
		}
	}

	public void OpenItemMenu()
	{
        if (StickmanCombat.Gameplay.GameFlowController.instance != null) { StickmanCombat.Gameplay.GameFlowController.instance.OpenEquipment(); return; }
		if (SceneManager.instance.gettingItem)
		{
			return;
		}
		if (this.itemMenuOpen)
		{
			return;
		}
		this.itemsMenuCanvas.gameObject.SetActive(true);
		CharacterManager.instance.side = 1;
		CharacterManager.instance.spine.skeleton.flipX = false;
		this.previousWeapon = this.currentWeapon;
		this.previousHat = this.currentHat;
		this.itemMenuOpen = true;
		SceneManager.instance.chaserObject.transform.localPosition = new Vector3(0f, -1f, 0f);
		NpcManager.instance.KillAll(new Vector2(-1f, 10f), new Vector2(2f, 10f), null);
		NpcManager.instance.PauseAll();
		SceneManager.instance.targetCameraZ = -5f;
        Camera.main.GetComponent<CombatCameraFollow>().OverallOffset.x = 2.5f;
        Camera.main.GetComponent<CombatCameraFollow>().OverallOffset.y= -1.3f;


        OpenWeaponList();
      
        if (SceneManager.instance.currentState == SceneManager.State.gameOver)
		{
            MenuUIController.instance.CloseGameOver();
            //SceneManager.instance.menu.Play("closeGameOver");
        }
		else
		{
            //SceneManager.instance.headingAnim.Play("headingClose");
            MenuUIController.instance.HideLogo();
            MenuUIController.instance.CloseMenu();
            //SceneManager.instance.menu.Play("menuClose");
        }
		//TNCPManager.Hide();
	}

    public void OpenWeaponList()
    {
        
        _listWeapon.SetActive(true);
        _listHat.SetActive(false);
        int currentPageWeapon = 0;
        int index = 0;
        if (contentItemWeapon.childCount > 0) {
            foreach (Transform child in contentItemWeapon)
            {
                if (child.GetComponent<ItemEquipment>() != null)
                {
                    ProductMetadata productData = StoreManager.instance.GetProductData(child.GetComponent<ItemEquipment>()._weapon.id);
                    string price = "";
                    if (productData != null)
                    {
                        price = productData.localizedPriceString;
                    }
                    else
                    {
                        price = "Lock";
                    }
                    child.GetComponent<ItemEquipment>().UpdatePrice(price);
                    child.GetComponent<ItemEquipment>().UpdateBuyButton();
                   
                }
                if (this.currentWeapon.EnglishName == child.GetComponent<ItemEquipment>()._weapon.EnglishName)
                {
                    currentPageWeapon = index;
                }
                index++;
            }
        }
        verticalScrollSnapEquipment.GoToScreen(currentPageWeapon);

        verticalScrollSnapEquipment.CurrentPageObject().GetComponent<ItemEquipment>()._continue.GetComponent<Button>().interactable = true;
        verticalScrollSnapEquipment.CurrentPageObject().GetComponent<ItemEquipment>()._buyBtn.GetComponent<Button>().interactable = false;
        isWeaponList = true;

    }
    public void OpenHatList()
    {
        
        _listWeapon.SetActive(false);
        _listHat.SetActive(true);
        int currentPageHat = 0;
        int index = 0;
        if (contentItemHat.childCount > 0)
        {
            foreach (Transform child in contentItemHat)
            {
                if (child.GetComponent<ItemEquipment>() != null)
                {
                    ProductMetadata productData = StoreManager.instance.GetProductData(child.GetComponent<ItemEquipment>()._hat.id);
                    string price = "";
                    if (productData != null)
                    {
                        price = productData.localizedPriceString;
                    }
                    else
                    {
                        price = "Lock";
                    }
                    child.GetComponent<ItemEquipment>().UpdatePrice(price);
                    child.GetComponent<ItemEquipment>().UpdateBuyButton();
                }
                if (this.currentHat.EnglishName == child.GetComponent<ItemEquipment>()._hat.EnglishName)
                {
                    currentPageHat = index;
                }
                index++;
            }
        }
        verticalScrollSnapHat.GoToScreen(currentPageHat);
      
       
        verticalScrollSnapHat.CurrentPageObject().GetComponent<ItemEquipment>()._continue.GetComponent<Button>().interactable = true;
        verticalScrollSnapHat.CurrentPageObject().GetComponent<ItemEquipment>()._buyBtn.GetComponent<Button>().interactable = false;
        isWeaponList = false;


    }
    public void UpdateBuyInteractableWeapon()
    {
        if (verticalScrollSnapEquipment.CurrentPageObject().GetComponent<ItemEquipment>()._isBought)
        {


            verticalScrollSnapEquipment.CurrentPageObject().GetComponent<ItemEquipment>()._continue.GetComponent<Button>().interactable = true;
            verticalScrollSnapEquipment.CurrentPageObject().GetComponent<ItemEquipment>()._buyBtn.GetComponent<Button>().interactable = false;
        }
    }

    public void UpdateBuyInteractableHat()
    {
        if (verticalScrollSnapHat.CurrentPageObject().GetComponent<ItemEquipment>()._isBought)
        {


            verticalScrollSnapHat.CurrentPageObject().GetComponent<ItemEquipment>()._continue.GetComponent<Button>().interactable = true;
            verticalScrollSnapHat.CurrentPageObject().GetComponent<ItemEquipment>()._buyBtn.GetComponent<Button>().interactable = false;
        }
    }


    public void InstanWeaponList()
    {
        int index = 0;
        verticalScrollSnapEquipment.ChildObjects= new GameObject[this.weaponsById.Count];
        int currentPageWeapon = 0;
        foreach (KeyValuePair<string, Weapon> current in this.weaponsById)
        {

            GameObject tempWeapon = Instantiate(_prefabItem, Vector3.zero, Quaternion.identity);
            string spriteItemName = current.Value.id;
            string nameItem = current.Value.GetName();

            bool isBought = current.Value.IsOwned();
            ProductMetadata productData = StoreManager.instance.GetProductData(current.Value.id);
            string price = "";
            if (productData != null)
            {
                price = productData.localizedPriceString;
            }
            else
            {
                price = "";
            }
            tempWeapon.GetComponent<ItemEquipment>().SetupDataItem(spriteItemName, nameItem, isBought, price, current.Value, null);
            tempWeapon.transform.SetParent(contentItemWeapon);
            tempWeapon.transform.localPosition = Vector3.zero;
            tempWeapon.transform.localScale = Vector3.one;
            verticalScrollSnapEquipment.AddChild(tempWeapon);
            if (this.currentWeapon.EnglishName == current.Value.EnglishName)
            {
                currentPageWeapon = index;
            }
            index++;
        }
        verticalScrollSnapEquipment.StartingScreen= currentPageWeapon;
        verticalScrollSnapEquipment.GoToScreen(currentPageWeapon);
        if (verticalScrollSnapEquipment.CurrentPageObject().GetComponent<ItemEquipment>()._isBought)
        {
            verticalScrollSnapEquipment.CurrentPageObject().GetComponent<ItemEquipment>()._continue.SetActive(true);
            verticalScrollSnapEquipment.CurrentPageObject().GetComponent<ItemEquipment>()._continue.GetComponent<Button>().interactable = true;
            verticalScrollSnapEquipment.CurrentPageObject().GetComponent<ItemEquipment>()._buyBtn.SetActive(false);
            verticalScrollSnapEquipment.CurrentPageObject().GetComponent<ItemEquipment>()._buyBtn.GetComponent<Button>().interactable = false;
        }
        else
        {
            verticalScrollSnapEquipment.CurrentPageObject().GetComponent<ItemEquipment>()._continue.SetActive(false);
            verticalScrollSnapEquipment.CurrentPageObject().GetComponent<ItemEquipment>()._continue.GetComponent<Button>().interactable = false;
            verticalScrollSnapEquipment.CurrentPageObject().GetComponent<ItemEquipment>()._buyBtn.SetActive(true);
            verticalScrollSnapEquipment.CurrentPageObject().GetComponent<ItemEquipment>()._buyBtn.GetComponent<Button>().interactable = true;
        }
    }

    public void InstanHatList()
    {
        int index = 0;
        verticalScrollSnapHat.ChildObjects = new GameObject[this.hatsById.Count];
        int currentPageHat = 0;
        foreach (KeyValuePair<string, Hat> current in this.hatsById)
        {

            GameObject tempHat = Instantiate(_prefabItem, Vector3.zero, Quaternion.identity);
            string spriteItemName = current.Value.id;
            string nameItem = current.Value.GetName();

            bool isBought = current.Value.IsOwned();
            ProductMetadata productData = StoreManager.instance.GetProductData(current.Value.id);
            string price = "";
            if (productData != null)
            {
                price = productData.localizedPriceString;
            }
            else
            {
                price = "";
            }
            tempHat.GetComponent<ItemEquipment>().SetupDataItem(spriteItemName, nameItem, isBought, price, null, current.Value);
            tempHat.transform.SetParent(contentItemHat);
            tempHat.transform.localPosition = Vector3.zero;
            tempHat.transform.localScale = Vector3.one;
            verticalScrollSnapHat.AddChild(tempHat);
            if (this.currentHat.EnglishName == current.Value.EnglishName)
            {
                currentPageHat = index;
            }
            index++;
        }
        verticalScrollSnapHat.StartingScreen = currentPageHat;
        verticalScrollSnapHat.GoToScreen(currentPageHat);
        if (verticalScrollSnapHat.CurrentPageObject().GetComponent<ItemEquipment>()._isBought)
        {
            verticalScrollSnapHat.CurrentPageObject().GetComponent<ItemEquipment>()._continue.SetActive(true);
            verticalScrollSnapHat.CurrentPageObject().GetComponent<ItemEquipment>()._continue.GetComponent<Button>().interactable = true;
            verticalScrollSnapHat.CurrentPageObject().GetComponent<ItemEquipment>()._buyBtn.SetActive(false);
            verticalScrollSnapHat.CurrentPageObject().GetComponent<ItemEquipment>()._buyBtn.GetComponent<Button>().interactable = false;
        }
        else
        {
            verticalScrollSnapHat.CurrentPageObject().GetComponent<ItemEquipment>()._continue.SetActive(false);
            verticalScrollSnapHat.CurrentPageObject().GetComponent<ItemEquipment>()._continue.GetComponent<Button>().interactable = false;
            verticalScrollSnapHat.CurrentPageObject().GetComponent<ItemEquipment>()._buyBtn.SetActive(true);
            verticalScrollSnapHat.CurrentPageObject().GetComponent<ItemEquipment>()._buyBtn.GetComponent<Button>().interactable = true;
        }
    }

   
	public void OnPurchaseCompleted(Product product)
	{
		this.loadingObject.SetActive(false);
		string id = product.definition.id;
		if (this.weaponsById.ContainsKey(id))
		{
			int type = 1;
			this.GotItem(type, id, false, false);
		}
		else if (this.hatsById.ContainsKey(id))
		{
			int type = 2;
			this.GotItem(type, id, false, false);
		}
		//else if (id == "unlockall")
		//{
  //          this.unlockAll = true;
  //          PlayerPrefsX.SetBool("unlockAllItem", this.unlockAll);

  //          foreach (KeyValuePair<string, Weapon> current in ItemManager.instance.weaponsById)
  //          {
  //              current.Value.Own();
  //          }
  //          foreach (KeyValuePair<string, Hat> current2 in ItemManager.instance.hatsById)
  //          {
  //              current2.Value.Own();
  //          }
  //          OpenConfirmUnlockedPanel();
  //      }
		
		AnalyticsManager.instance.BoughtItem(id);
	}

	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		this.loadingObject.SetActive(false);
		if (product != null)
		{
			AnalyticsManager.instance.IAPBuyFailed(product.definition.id, failureReason.ToString());
		}
		else
		{
			AnalyticsManager.instance.IAPBuyFailed("null", "nullProduct");
		}
	}

    public void OpenBuyAllPanel()
    {
        _itemPanel.SetActive(false);
        _buyallPanel.SetActive(true);
        _buyAllObj.SetActive(true);
        _confirmObj.SetActive(false);
    }

    public void CloseBuyAllPanel()
    {
        _itemPanel.SetActive(true);
        _buyallPanel.SetActive(false);
    }

    //public void OpenConfirmUnlockedPanel()
    //{
    //    _buyAllObj.SetActive(false);
    //    _confirmObj.SetActive(true);
    //}

    public void CloseConfirmUnlockedAndReloadItem()
    {
        _itemPanel.SetActive(true);
        _buyallPanel.SetActive(false);
        OpenWeaponList();
    }
}
