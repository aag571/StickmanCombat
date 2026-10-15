// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;

public class ScrollItem
{
	public Weapon weapon;

	public Hat hat;

	public CanvasGroup canvasGroup;

	public int index;

	public ScrollItem(Hat hat, CanvasGroup canvasGroup, int ind)
	{
		this.index = ind;
		this.hat = hat;
		this.canvasGroup = canvasGroup;
	}

	public ScrollItem(Weapon weapon, CanvasGroup canvasGroup, int ind)
	{
		this.index = ind;
		this.weapon = weapon;
		this.canvasGroup = canvasGroup;
	}
}
