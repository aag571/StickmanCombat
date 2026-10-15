// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuItemPanel : MonoBehaviour
{
	public CanvasGroup canvasGroup;

	public Text titleText;

	public int index;

	public bool isOpen;

	public void SetColor()
	{
		float num = 1f;
		if (!this.isOpen)
		{
			num = 0.392156869f;
		}
		if (this.titleText.color.a != num)
		{
			this.titleText.color = new Color(1f, 1f, 1f, Mathf.MoveTowards(this.titleText.color.a, num, Time.deltaTime));
		}
	}
}
