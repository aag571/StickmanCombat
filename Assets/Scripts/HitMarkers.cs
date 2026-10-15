// 反编译自程序集： Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitMarkers : MonoBehaviour
{
	public CanvasGroup cGroup;

	public Image leftPrefab;

	public Image rightPrefab;

	public Image circle;

	public Queue<Image> hitImages = new Queue<Image>();

	public RelativeText relativeText;

	private void Start()
	{
	}

	public void AddHitImage(int side)
	{
		Image image;
		if (side == -1)
		{
			image = UnityEngine.Object.Instantiate<Image>(this.leftPrefab, this.leftPrefab.transform.parent);
		}
		else
		{
			image = UnityEngine.Object.Instantiate<Image>(this.rightPrefab, this.leftPrefab.transform.parent);
		}
		this.hitImages.Enqueue(image);
		image.gameObject.SetActive(true);
	}

	public void RemoveHitImage()
	{
		UnityEngine.Object.Destroy(this.hitImages.Dequeue().gameObject);
	}

	public void InRange()
	{
		this.circle.color = new Color(0.8745098f, 0.8745098f, 0.8745098f, 1f);
	}

	public void OutOfRange()
	{
		this.circle.color = new Color(0f, 0f, 0f, 1f);
	}

	public void Initialize()
	{
		this.cGroup.alpha = 1f;
		this.relativeText.Start();
		this.circle.color = Color.black;
	}
}
