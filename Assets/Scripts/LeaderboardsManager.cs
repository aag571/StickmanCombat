// 反编译自程序集： Assembly-CSharp.dll
using CodeStage.AntiCheat.ObscuredTypes;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class LeaderboardsManager : MonoBehaviour
{
	private sealed class _GetLeaderboardData_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal bool _isPlayer___0;

		internal string id;

		internal string _url___0;

		internal string name;

		internal WWW _www___0;

		internal LeaderboardsManager _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _GetLeaderboardData_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				ItemManager.instance.loadingObject.SetActive(true);
				this._isPlayer___0 = true;
				if (this.id != SystemInfo.deviceUniqueIdentifier)
				{
					this._isPlayer___0 = false;
				}
				this._url___0 = string.Concat(new string[]
				{
					"https://stick-fight-shadow-warrior.appspot.com/getLeaderboardData.php?isPlayer=",
					this._isPlayer___0.ToString().ToLower(),
					"&playerId=",
					this.id,
					"&rankingType=",
					this._this.typeText
				});
				if (this.name != null)
				{
					this._url___0 = this._url___0 + "&name=" + this.name;
				}
				this._www___0 = new WWW(this._url___0);
				this._current = this._www___0;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				ItemManager.instance.loadingObject.SetActive(false);
				if (this._www___0.error != null)
				{
					this._this.Close();
				}
				else
				{
					try
					{
						this._this.rankingInfo = JsonConvert.DeserializeObject<RankingInfo>(this._www___0.text);
						if (this._this.rankingInfo.generalData.nameSet)
						{
							this._this.UpdateLeaderboardData();
						}
						else
						{
							this._this.ShowNameDialog();
						}
					}
					catch
					{
					}
				}
				this._PC = -1;
				break;
			}
			return false;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private sealed class _GetShadowWarriorDataRequest_c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal string _url___0;

		internal WWW _www___0;

		internal LeaderboardsManager _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _GetShadowWarriorDataRequest_c__Iterator1()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._url___0 = "https://stick-fight-shadow-warrior.appspot.com/getShadowWarriorData.php";
				this._www___0 = new WWW(this._url___0);
				this._current = this._www___0;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				if (this._www___0.error != null)
				{
					this._this.Close();
				}
				else
				{
					try
					{
						ShadowWarriorData shadowWarriorData = JsonConvert.DeserializeObject<ShadowWarriorData>(this._www___0.text);
						if ((!shadowWarriorData.result && shadowWarriorData.deviceId == null) || shadowWarriorData.deviceId == string.Empty)
						{
							break;
						}
						if (!SceneManager.instance.gameStarted && shadowWarriorData.deviceId != this._this.shadowWarriorData[0])
						{
							if (!(shadowWarriorData.deviceId == SystemInfo.deviceUniqueIdentifier))
							{
								if (this._this.shadowWarriorData[0] == SystemInfo.deviceUniqueIdentifier)
								{
									TipManager.instance.Open("#" + shadowWarriorData.name + " 已经夺走了你的头衔 #暗影武士 !", true, 5f);
								}
								else if (this._this.shadowWarriorData[0] == null)
								{
									TipManager.instance.Open("#" + shadowWarriorData.name + " 已成为 #暗影武士 !", true, 5f);
								}
								else
								{
									TipManager.instance.Open(string.Concat(new string[]
									{
										"#",
										shadowWarriorData.name,
										" 击败 #",
										this._this.shadowWarriorData[1],
                                        " 并成为了 #暗影武士 !"
                                    }), true, 5f);
								}
							}
						}
						this._this.shadowWarriorData[0] = shadowWarriorData.deviceId;
						this._this.shadowWarriorData[1] = shadowWarriorData.name;
						this._this.shadowWarriorData[2] = shadowWarriorData.weapon;
						this._this.shadowWarriorData[3] = shadowWarriorData.hat;
						this._this.shadowWarriorData[4] = shadowWarriorData.beltLevel.ToString();
						PlayerPrefsX.SetStringArray("shadowWarriorData", this._this.shadowWarriorData);
					}
					catch
					{
					}
				}
				this._PC = -1;
				break;
			}
			return false;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private sealed class _PostUpdatedData_c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal int score;

		internal int coinCombo;

		internal int belt;

		internal string _url___0;

		internal WWW _www___0;

		internal string id;

		internal string name;

		internal LeaderboardsManager _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _PostUpdatedData_c__Iterator2()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._url___0 = string.Concat(new object[]
				{
					"https://stick-fight-shadow-warrior.appspot.com/postUpdatedData.php?playerId=",
					SystemInfo.deviceUniqueIdentifier,
					"&score=",
					this.score,
					"&coinCombo=",
					this.coinCombo,
					"&belt=",
					this.belt
				});
				this._www___0 = new WWW(this._url___0);
				this._current = this._www___0;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				if (this._www___0.error == null)
				{
					string text = this._www___0.text;
					try
					{
						PostData postData = JsonConvert.DeserializeObject<PostResults>(text).data[0];
						if (postData.score > this._this.bestScore)
						{
							this._this.bestScore = postData.score;
                                PlayerPrefs.SetInt("leaderboardsBestScore", this._this.bestScore);
						}
						if (postData.beltLevel > this._this.bestBelt)
						{
							this._this.bestBelt = postData.beltLevel;
                                PlayerPrefs.SetInt("leaderboardsBestBelt", this._this.bestBelt);
						}
						if (postData.coinCombo > this._this.bestCoinCombo)
						{
							this._this.bestCoinCombo = postData.coinCombo;
                                PlayerPrefs.SetInt("leaderboardsBestCoinCombo", this._this.bestCoinCombo);
						}
					}
					catch
					{
					}
				}
				this._this.StartCoroutine(this._this.GetLeaderboardData(this.id, this.name));
				this._PC = -1;
				break;
			}
			return false;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	public static LeaderboardsManager instance;

	public bool isOpen;

	public CanvasGroup menuCanvas;

	public BlurOptimized blur;

	private string typeText;

	public Text ranking1Text;

	public Text ranking2Text;

	public Text ranking3Text;

	public GameObject nameContainer;

	public Text nameInputText;

	public Text nameErrorText;

	public RectTransform scrollStopper;

	public RectTransform contentHolder;

	public List<RankContainer> rankContainers = new List<RankContainer>();

	public RankContainer rankContainerPrefab;

	public GameObject titleMenu;

	public Dropdown titleDropdown;

	private Vector2 defaultContentSize;

	private float itemHeight = 35f;

	private int topContainerMax;

	private int botContainerMax;

	private float containerHeight;

	private int maxItems;

	public Vector3 startingPos;

	private RankContainer currentMidRankContainer;

	private RankContainer actualMidRankContainer;

	private RankingInfo rankingInfo;

	public ObscuredInt bestScore;

	public ObscuredInt bestCoinCombo;

	public ObscuredInt bestBelt;

	public string[] shadowWarriorData;

	protected void Awake()
	{
		LeaderboardsManager.instance = this;
	}

	private void Start()
	{
		this.bestScore = PlayerPrefs.GetInt("leaderboardsBestScore", 0);
		this.bestCoinCombo = PlayerPrefs.GetInt("leaderboardsBestCoinCombo", 0);
		this.bestBelt = PlayerPrefs.GetInt("leaderboardsBestBelt", 0);
		this.startingPos = this.contentHolder.transform.localPosition;
		this.defaultContentSize = this.scrollStopper.sizeDelta;
		this.containerHeight = (float)Screen.height / 2.4f / 1.3f;
		this.maxItems = (int)(this.containerHeight / this.itemHeight) - 1;
		string[] defaultValue = new string[5];
        if (PlayerPrefs.HasKey("shadowWarriorData"))
        {
            this.shadowWarriorData = PlayerPrefsX.GetStringArray("shadowWarriorData");
        } else
        {
            this.shadowWarriorData = defaultValue;
        }
	}

	private void Update()
	{
		if (this.currentMidRankContainer != null)
		{
			RankContainer rankContainer = this.currentMidRankContainer;
			for (int i = 0; i < this.rankContainers.Count; i++)
			{
				if (Mathf.Abs(this.rankContainers[i].transform.position.y - (float)(Screen.height / 2)) < Mathf.Abs(rankContainer.transform.position.y - (float)(Screen.height / 2)))
				{
					rankContainer = this.rankContainers[i];
				}
			}
			this.actualMidRankContainer = rankContainer;
			if (this.actualMidRankContainer.playerData.ranking - this.currentMidRankContainer.playerData.ranking > 40)
			{
				this.currentMidRankContainer = this.actualMidRankContainer;
				base.StopAllCoroutines();
				if (StatManager.instance.stats[0].value > LeaderboardsManager.instance.bestScore || StatManager.instance.stats[9].value > this.bestCoinCombo || StoryManager.instance.level > this.bestBelt)
				{
					base.StartCoroutine(this.PostUpdatedData(StatManager.instance.stats[0].value, StatManager.instance.stats[9].value, StoryManager.instance.level, this.currentMidRankContainer.playerData.id, null));
				}
				else
				{
					base.StartCoroutine(this.GetLeaderboardData(this.currentMidRankContainer.playerData.id, null));
				}
			}
			else if (this.currentMidRankContainer.playerData.ranking - this.actualMidRankContainer.playerData.ranking > 40)
			{
				this.currentMidRankContainer = this.actualMidRankContainer;
				base.StopAllCoroutines();
				if (StatManager.instance.stats[0].value > LeaderboardsManager.instance.bestScore || StatManager.instance.stats[9].value > this.bestCoinCombo || StoryManager.instance.level > this.bestBelt)
				{
					base.StartCoroutine(this.PostUpdatedData(StatManager.instance.stats[0].value, StatManager.instance.stats[9].value, StoryManager.instance.level, this.currentMidRankContainer.playerData.id, null));
				}
				else
				{
					base.StartCoroutine(this.GetLeaderboardData(this.currentMidRankContainer.playerData.id, null));
				}
			}
		}
		Helpers.UpdateCanvasVisibilty(this.menuCanvas, 2f, this.isOpen, true);
		if (this.isOpen)
		{
			if (!this.blur.enabled)
			{
				this.blur.blurSize = 0f;
				this.blur.enabled = true;
			}
			else if (this.blur.blurSize < 10f)
			{
				this.blur.blurSize += Time.unscaledDeltaTime * 20f;
			}
			else if (this.blur.blurSize != 10f)
			{
				this.blur.blurSize = 10f;
			}
		}
		else if (!MenuManager.instance.isOpen && this.blur.enabled)
		{
			if (this.blur.blurSize > 0f)
			{
				this.blur.blurSize -= Time.unscaledDeltaTime * 20f;
			}
			else
			{
				this.blur.blurSize = 0f;
				this.blur.enabled = false;
			}
		}
	}

	public void Open()
	{
		this.nameContainer.SetActive(false);
		this.isOpen = true;
		this.UpdateRankings(2);
	}

	public void Close()
	{
		this.isOpen = false;
	}

	public void UpdateRankings(int type)
	{
		this.ranking1Text.color = Color.white;
		this.ranking2Text.color = Color.white;
		this.ranking3Text.color = Color.white;
		if (type == 0)
		{
			this.ranking1Text.color = new Color(0f, 0.882352948f, 0.443137258f);
			this.typeText = "beltLevel";
		}
		else if (type == 1)
		{
			this.ranking2Text.color = new Color(0f, 0.882352948f, 0.443137258f);
			this.typeText = "coinCombo";
		}
		else
		{
			this.ranking3Text.color = new Color(0f, 0.882352948f, 0.443137258f);
			this.typeText = "score";
		}
		this.ResetLeaderboardData();
		base.StopAllCoroutines();
		if (StatManager.instance.stats[0].value > LeaderboardsManager.instance.bestScore || StatManager.instance.stats[9].value > this.bestCoinCombo || StoryManager.instance.level > this.bestBelt)
		{
			base.StartCoroutine(this.PostUpdatedData(StatManager.instance.stats[0].value, StatManager.instance.stats[9].value, StoryManager.instance.level, SystemInfo.deviceUniqueIdentifier, null));
		}
		else
		{
			base.StartCoroutine(this.GetLeaderboardData(SystemInfo.deviceUniqueIdentifier, null));
		}
	}

	public void RegisterName()
	{
		string text = this.nameInputText.text;
		if (text.Length < 4)
		{
			this.nameErrorText.text = "名字长度需要大于4个字符";
			return;
		}
		if (!Regex.IsMatch(text, "^[a-zA-Z]*$"))
		{
			this.nameErrorText.text = "只允许使用英文字符";
			return;
		}
		this.nameContainer.SetActive(false);
		base.StopAllCoroutines();
		if (StatManager.instance.stats[0].value > LeaderboardsManager.instance.bestScore || StatManager.instance.stats[9].value > this.bestCoinCombo || StoryManager.instance.level > this.bestBelt)
		{
			base.StartCoroutine(this.PostUpdatedData(StatManager.instance.stats[0].value, StatManager.instance.stats[9].value, StoryManager.instance.level, SystemInfo.deviceUniqueIdentifier, text));
		}
		else
		{
			base.StartCoroutine(this.GetLeaderboardData(SystemInfo.deviceUniqueIdentifier, text));
		}
	}

	private IEnumerator GetLeaderboardData(string id, string name = null)
	{
		LeaderboardsManager._GetLeaderboardData_c__Iterator0 _GetLeaderboardData_c__Iterator = new LeaderboardsManager._GetLeaderboardData_c__Iterator0();
		_GetLeaderboardData_c__Iterator.id = id;
		_GetLeaderboardData_c__Iterator.name = name;
		_GetLeaderboardData_c__Iterator._this = this;
		return _GetLeaderboardData_c__Iterator;
	}

	public void GetShadowWarriorData()
	{
		base.StartCoroutine(this.GetShadowWarriorDataRequest());
	}

	private IEnumerator GetShadowWarriorDataRequest()
	{
		LeaderboardsManager._GetShadowWarriorDataRequest_c__Iterator1 _GetShadowWarriorDataRequest_c__Iterator = new LeaderboardsManager._GetShadowWarriorDataRequest_c__Iterator1();
		_GetShadowWarriorDataRequest_c__Iterator._this = this;
		return _GetShadowWarriorDataRequest_c__Iterator;
	}

	public void UpdateLeaderboardData()
	{
		bool flag = false;
		RankContainer rankContainer = null;
		float num = 0f;
		int num2 = 0;
		string b = string.Empty;
		if (this.rankContainers.Count <= 0)
		{
			flag = true;
		}
		if (this.currentMidRankContainer != null)
		{
			b = this.currentMidRankContainer.playerData.id;
			num2 = this.currentMidRankContainer.index - this.rankingInfo.generalData.midPos;
			num = this.currentMidRankContainer.transform.localPosition.y - (float)(-(float)this.rankingInfo.generalData.midPos) * this.itemHeight;
		}
		else
		{
			num = this.rankContainerPrefab.transform.localPosition.y;
		}
		int num3 = num2 + this.rankingInfo.playerData.Count;
		int amount = -num2;
		int num4 = num3 - this.rankContainers.Count;
		if (flag)
		{
			num4 -= this.maxItems;
			if (num4 < 0)
			{
				num4 = 0;
			}
		}
		foreach (RankContainer current in this.rankContainers)
		{
			UnityEngine.Object.Destroy(current.gameObject);
		}
		this.rankContainers.Clear();
		this.EditScrollStopper(true, amount);
		this.EditScrollStopper(false, num4);
		for (int i = 0; i < this.rankingInfo.playerData.Count; i++)
		{
			PlayerData playerData = this.rankingInfo.playerData[i];
			RankContainer rankContainer2 = UnityEngine.Object.Instantiate<RankContainer>(this.rankContainerPrefab, this.rankContainerPrefab.transform.position, this.rankContainerPrefab.transform.rotation, this.rankContainerPrefab.transform.parent);
			if (playerData.id == SystemInfo.deviceUniqueIdentifier)
			{
				rankContainer = rankContainer2;
			}
			if (playerData.id == b)
			{
				this.currentMidRankContainer = rankContainer2;
			}
			rankContainer2.transform.localPosition = new Vector3(rankContainer2.transform.localPosition.x, num - this.itemHeight * (float)i, rankContainer2.transform.localPosition.z);
			rankContainer2.Initialize(playerData, i);
			rankContainer2.gameObject.SetActive(true);
			this.rankContainers.Add(rankContainer2);
		}
		if (flag && rankContainer != null)
		{
			this.scrollStopper.transform.position += new Vector3(0f, (float)(Screen.height / 2) - rankContainer.transform.position.y, 0f);
			this.currentMidRankContainer = rankContainer;
		}
	}

	public void ResetLeaderboardData()
	{
		this.currentMidRankContainer = null;
		foreach (RankContainer current in this.rankContainers)
		{
			UnityEngine.Object.Destroy(current.gameObject);
		}
		this.rankContainers.Clear();
		this.scrollStopper.sizeDelta = this.defaultContentSize;
		this.scrollStopper.localPosition = Vector3.zero;
		this.contentHolder.transform.localPosition = this.startingPos;
	}

	public void ShowNameDialog()
	{
		this.nameContainer.SetActive(true);
		this.nameErrorText.text = string.Empty;
	}

	public void EditScrollStopper(bool isTop, int amount)
	{
		if (isTop)
		{
			Vector3 position = this.contentHolder.transform.position;
			this.contentHolder.anchorMin = new Vector2(0.5f, 0f);
			this.contentHolder.anchorMax = new Vector2(0.5f, 0f);
			this.contentHolder.transform.position = position;
			this.scrollStopper.offsetMax += new Vector2(0f, (float)amount * this.itemHeight);
			this.topContainerMax += amount;
		}
		else
		{
			Vector3 position2 = this.contentHolder.transform.position;
			this.contentHolder.anchorMin = new Vector2(0.5f, 1f);
			this.contentHolder.anchorMax = new Vector2(0.5f, 1f);
			this.contentHolder.transform.position = position2;
			this.scrollStopper.offsetMin += new Vector2(0f, (float)(-(float)amount) * this.itemHeight);
			this.botContainerMax -= amount;
		}
	}

	private IEnumerator PostUpdatedData(int score, int coinCombo, int belt, string id, string name = null)
	{
		LeaderboardsManager._PostUpdatedData_c__Iterator2 _PostUpdatedData_c__Iterator = new LeaderboardsManager._PostUpdatedData_c__Iterator2();
		_PostUpdatedData_c__Iterator.score = score;
		_PostUpdatedData_c__Iterator.coinCombo = coinCombo;
		_PostUpdatedData_c__Iterator.belt = belt;
		_PostUpdatedData_c__Iterator.id = id;
		_PostUpdatedData_c__Iterator.name = name;
		_PostUpdatedData_c__Iterator._this = this;
		return _PostUpdatedData_c__Iterator;
	}

	public void PostShadowData()
	{
		string url = string.Concat(new object[]
		{
			"https://stick-fight-shadow-warrior.appspot.com/postUpdatedData.php?playerId=",
			SystemInfo.deviceUniqueIdentifier,
			"&score=",
			StatManager.instance.stats[0],
			"&coinCombo=",
			StatManager.instance.stats[9],
			"&belt=",
			StoryManager.instance.level
		});
		WWW wWW = new WWW(url);
	}

	public void OpenTitleMenu()
	{
		this.titleMenu.SetActive(true);
	}

	public void CloseTitleMenu()
	{
		this.titleMenu.SetActive(false);
	}

	public void PickTitle()
	{
		ItemManager.instance.loadingObject.SetActive(true);
	}
}
