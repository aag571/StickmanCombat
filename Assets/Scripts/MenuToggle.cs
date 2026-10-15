// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuToggle : MonoBehaviour
{
	public bool isOn;

	public Text stateText;

	public Image ball;

	public string playerPrefString;

	public MenuToggle parentToggle;

	public bool startingState = true;

	private void Start()
	{
		this.isOn = PlayerPrefsX.GetBool(this.playerPrefString, this.startingState);
	}

	private void Update()
	{
        if (this.ball == null || this.stateText == null) return;
		if (this.parentToggle != null)
		{
			this.isOn = this.parentToggle.isOn;
		}
		if (this.isOn)
		{
			if (this.ball.rectTransform.localPosition != new Vector3(20f, 0f, 0f))
			{
				this.ball.rectTransform.localPosition = Vector3.MoveTowards(this.ball.rectTransform.localPosition, new Vector3(20f, 0f, 0f), Time.unscaledDeltaTime * 350f);
			}
			if (this.stateText.text != "开")
			{
				this.stateText.text = "开";
			}
		}
		else
		{
			if (this.ball.rectTransform.localPosition != new Vector3(-20f, 0f, 0f))
			{
				this.ball.rectTransform.localPosition = Vector3.MoveTowards(this.ball.rectTransform.localPosition, new Vector3(-20f, 0f, 0f), Time.unscaledDeltaTime * 350f);
			}
			if (this.stateText.text != "关")
			{
				this.stateText.text = "关";
			}
		}
	}

	public void OnToggle()
	{
		this.isOn = !this.isOn;
		if (this.parentToggle != null)
		{
			this.parentToggle.isOn = this.isOn;
		}
		PlayerPrefsX.SetBool(this.playerPrefString, this.isOn);
	}

	public void UpdateNotifs()
	{
		//NPBinding.NotificationService.RegisterNotificationTypes(NotificationType.Badge | NotificationType.Sound | NotificationType.Alert);
		MonetizationManager.instance.ClearAndScheduleNotifications();
	}
}
