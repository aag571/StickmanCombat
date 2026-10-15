// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;

public class Helpers
{
	public static void UpdateCanvasVisibilty(CanvasGroup cg, float speed, bool isOpen, bool isButton)
	{
		if (isOpen)
		{
			if (!cg.gameObject.activeInHierarchy)
			{
				cg.alpha = 0f;
				cg.gameObject.SetActive(true);
			}
			if (cg.alpha < 1f)
			{
				cg.alpha += Time.unscaledDeltaTime * speed;
			}
			else if (cg.alpha > 1f)
			{
				cg.alpha = 1f;
			}
			if (isButton)
			{
				if (!cg.interactable)
				{
					cg.interactable = true;
				}
				if (!cg.blocksRaycasts)
				{
					cg.blocksRaycasts = true;
				}
			}
		}
		else
		{
			if (!cg.gameObject.activeInHierarchy)
			{
				return;
			}
			if (cg.alpha > 0f)
			{
				cg.alpha -= Time.unscaledDeltaTime * speed;
			}
			else if (cg.alpha < 0f)
			{
				cg.alpha = 0f;
			}
			else if (cg.gameObject.activeInHierarchy)
			{
				cg.gameObject.SetActive(false);
			}
			if (isButton)
			{
				if (cg.interactable)
				{
					cg.interactable = false;
				}
				if (cg.blocksRaycasts)
				{
					cg.blocksRaycasts = false;
				}
			}
		}
	}
}
