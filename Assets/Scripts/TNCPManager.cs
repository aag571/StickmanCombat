// 反编译自程序集： Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TNCPManager : MonoBehaviour
{
	private sealed class _SendStartMessage_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal string _url___0;

		internal WWW _www___0;

		internal TNCPParsedJson _jsonObj___1;

		internal TNCPManager _this;

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

		public _SendStartMessage_c__Iterator0()
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
					"https://tncrosspromotion.appspot.com/registerApp.php?bundleId=",
					WWW.EscapeURL(Application.identifier),
					"&adShowCount=",
					this._this.adShowCount,
					"&test=",
					(!Application.isEditor) ? "0" : "1",
					"&platform=",
					(Application.platform != RuntimePlatform.Android) ? "0" : "1"
				});
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
					this._this.cooldownEndTime = (double)(Time.unscaledTime + 60f);
					UnityEngine.Debug.Log("数据下载失败：" + this._www___0.error);
				}
				else
				{
					this._jsonObj___1 = JsonUtility.FromJson<TNCPParsedJson>(this._www___0.text);
					if (this._jsonObj___1.result)
					{
						this._this.adData = this._jsonObj___1.data;
						this._url___0 = this._this.adData.buttonVisualLink;
						this._www___0 = new WWW(this._url___0);
						this._current = this._www___0;
						if (!this._disposing)
						{
							this._PC = 2;
						}
						return true;
					}
					this._this.cooldownEndTime = (double)(Time.unscaledTime + 300f);
					UnityEngine.Debug.Log("服务器返回失败结果");
				}
				break;
			case 2u:
				this._this.isStarting = false;
				if (this._www___0.error != null)
				{
					this._this.cooldownEndTime = (double)(Time.unscaledTime + 60f);
					UnityEngine.Debug.Log("纹理下载失败：" + this._www___0.error);
				}
				else
				{
					this._this.buttonImage.texture = this._www___0.texture;
					this._this.canvasScaler.referenceResolution = new Vector2((float)this._this.adData.relativeScreenSizeX, (float)this._this.adData.relativeScreenSizeY);
					RectTransform arg_2F9_0 = this._this.buttonImage.rectTransform;
					Vector2 vector = new Vector2(0.5f, 0.5f) + new Vector2((float)this._this.adData.horizontalOrientation, (float)this._this.adData.verticalOrientation) / 2f;
					this._this.buttonImage.rectTransform.anchorMin = vector;
					vector = vector;
					this._this.buttonImage.rectTransform.anchorMax = vector;
					arg_2F9_0.pivot = vector;
					RectTransform arg_352_0 = this._this.buttonImage.rectTransform;
					vector = new Vector3((float)this._this.adData.offsetX, (float)this._this.adData.offsetY);
					this._this.buttonImage.rectTransform.offsetMin = vector;
					arg_352_0.offsetMax = vector;
					this._this.buttonImage.rectTransform.sizeDelta = new Vector2((float)this._this.adData.adSizeX, (float)this._this.adData.adSizeY);
					this._this.adShowCount++;
					PlayerPrefs.SetInt("TNCPprefsAdShowCount", this._this.adShowCount);
					this._this.isStarted = true;
					if (this._this.showWhenLoaded)
					{
						this._this.TNCPShow();
					}
					this._PC = -1;
				}
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

	private static TNCPManager instance;

	private bool isStarted;

	private bool isStarting;

	private bool isShown;

	private bool showWhenLoaded;

	private int adShowCount;

	private double totalVisibleTime;

	private double cooldownEndTime;

	private TNCPAdData adData;

	private Canvas canvas;

	private CanvasScaler canvasScaler;

	private RawImage buttonImage;

	private Button button;

	private CanvasGroup canvasGroup;

	private static UnityAction __f__am_cache0;

	public static void Start()
	{
		try
		{
			TNCPManager.CreateIfDoesntExist();
			TNCPManager.instance.TNCPStart();
		}
		catch
		{
		}
	}

	public static void Stop()
	{
		try
		{
			TNCPManager.CreateIfDoesntExist();
			TNCPManager.instance.TNCPStop(0);
		}
		catch
		{
		}
	}

	public static void Show()
	{
		try
		{
			TNCPManager.CreateIfDoesntExist();
			TNCPManager.instance.TNCPShow();
		}
		catch
		{
		}
	}

	public static void Hide()
	{
		try
		{
			TNCPManager.CreateIfDoesntExist();
			TNCPManager.instance.TNCPHide();
		}
		catch
		{
		}
	}

	private static void CreateIfDoesntExist()
	{
		if (TNCPManager.instance == null || TNCPManager.instance.gameObject == null)
		{
			GameObject gameObject = new GameObject("TNCPCanvas");
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			TNCPManager.instance = gameObject.AddComponent<TNCPManager>();
			TNCPManager.instance.canvas = gameObject.AddComponent<Canvas>();
			TNCPManager.instance.canvas.sortingOrder = 32767;
			TNCPManager.instance.canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			TNCPManager.instance.canvasScaler = gameObject.AddComponent<CanvasScaler>();
			TNCPManager.instance.canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			gameObject.AddComponent<GraphicRaycaster>();
			GameObject gameObject2 = new GameObject("TNCPButton");
			TNCPManager.instance.buttonImage = gameObject2.AddComponent<RawImage>();
			gameObject2.transform.SetParent(gameObject.transform);
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject2.transform.localScale = Vector3.one;
			TNCPManager.instance.button = gameObject2.AddComponent<Button>();
			TNCPManager.instance.button.onClick.AddListener(delegate
			{
				TNCPManager.instance.OnButtonClicked();
			});
			TNCPManager.instance.buttonImage.color = new Color(1f, 1f, 1f, 0f);
			TNCPManager.instance.button.enabled = false;
			TNCPManager.instance.adShowCount = PlayerPrefs.GetInt("TNCPprefsAdShowCount", 0);
		}
	}

	private void OnButtonClicked()
	{
		this.TNCPStop(1);
		Application.OpenURL("market://details?id=" + this.adData.bundeId);
	}

	private void SendAction(int actionType)
	{
		string text = (Application.platform != RuntimePlatform.Android) ? "0" : "1";
		new WWW(string.Concat(new object[]
		{
			"https://tncrosspromotion.appspot.com/sendAction.php?appId=",
			this.adData.appId,
			"&adAppId=",
			this.adData.adAppId,
			"&actionType=",
			actionType,
			"&lifetime=",
			this.adData.lifeTime,
			"&platform=",
			text,
			string.Empty
		}));
	}

	private void Update()
	{
		bool flag = this.isStarted && this.isShown;
		if (this.button.enabled != flag)
		{
			this.button.enabled = flag;
			this.buttonImage.raycastTarget = flag;
		}
		float num = this.buttonImage.color.a;
		if (flag)
		{
			if (this.adData.scaleAnimationSpeed != 0f)
			{
				float num2 = this.adData.scaleAnimationMinScale + Mathf.PingPong(Time.unscaledTime * this.adData.scaleAnimationSpeed, this.adData.scaleAnimationMaxScale - this.adData.scaleAnimationMinScale);
				this.buttonImage.gameObject.transform.localScale = new Vector3(num2, num2, num2);
			}
			this.totalVisibleTime += (double)(Time.unscaledDeltaTime * 3f);
			if (this.totalVisibleTime > (double)this.adData.lifeTime)
			{
				this.TNCPStop(0);
			}
			if (num < 1f)
			{
				num += Time.unscaledDeltaTime;
			}
			else if (num != 1f)
			{
				num = 1f;
			}
		}
		else if (num > 0f)
		{
			num -= Time.unscaledDeltaTime * 3f;
		}
		else if (num != 0f)
		{
			num = 0f;
		}
		if (this.buttonImage.color.a != num)
		{
			this.buttonImage.color = new Color(1f, 1f, 1f, num);
		}
	}

	public void TNCPStart()
	{
		if (this.isStarted || this.isStarting)
		{
			return;
		}
		if ((double)Time.unscaledTime < this.cooldownEndTime)
		{
			this.showWhenLoaded = false;
			return;
		}
		this.totalVisibleTime = 0.0;
		this.isStarting = true;
		base.StartCoroutine(this.SendStartMessage());
	}

	private IEnumerator SendStartMessage()
	{
		TNCPManager._SendStartMessage_c__Iterator0 _SendStartMessage_c__Iterator = new TNCPManager._SendStartMessage_c__Iterator0();
		_SendStartMessage_c__Iterator._this = this;
		return _SendStartMessage_c__Iterator;
	}

	public void TNCPStop(int actionType = 0)
	{
		this.SendAction(actionType);
		this.cooldownEndTime = (double)(Time.unscaledTime + (float)this.adData.cooldown);
		base.StopAllCoroutines();
		this.showWhenLoaded = false;
		this.isStarted = false;
		this.isStarting = false;
		this.isShown = false;
	}

	public void TNCPShow()
	{
		if (this.isShown)
		{
			return;
		}
		if (this.isStarted)
		{
			this.isShown = true;
			return;
		}
		this.showWhenLoaded = true;
		if (this.isStarting)
		{
			return;
		}
		TNCPManager.Start();
	}

	public void TNCPHide()
	{
		this.showWhenLoaded = false;
		this.isShown = false;
	}
}
