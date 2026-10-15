// 反编译自程序集： Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
	public static DialogManager instance;

	public CanvasGroup canvas;

	public Text textBox;

	public Image container;

	public List<string> targetString;

	public bool isOpen;

	public int charCount;

	private double delay;

	public double nextLetterTime;

	public RelativeText relativeText;

	public bool timed;

	public float duration;

	public float openTime;

	private bool dot;

	private void Awake()
	{
		DialogManager.instance = this;
	}

	public void Update()
	{
		if (this.isOpen)
		{
			if (this.canvas.alpha < 1f)
			{
				this.canvas.alpha += Time.deltaTime * 5f;
			}
			else if (this.canvas.alpha > 1f)
			{
				this.canvas.alpha = 1f;
			}
			else
			{
				if (Input.GetMouseButtonDown(0))
				{
					this.delay = 0.0099999997764825821;
				}
				if (this.nextLetterTime < (double)Time.time && this.charCount < this.targetString.Count)
				{
					this.nextLetterTime = (double)Time.time + this.delay;
					Text expr_C6 = this.textBox;
					expr_C6.text += this.targetString[this.charCount];
					this.charCount++;
				}
			}
		}
		else if (this.canvas.alpha > 0f)
		{
			this.canvas.alpha -= Time.deltaTime * 5f;
		}
		else if (this.canvas.alpha < 0f)
		{
			this.canvas.alpha = 0f;
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}

	protected void LateUpdate()
	{
		if (this.isOpen)
		{
			this.SetContainerSize();
			if (this.timed && this.openTime + this.duration < Time.time)
			{
				this.Close();
			}
		}
	}

	public void SetContainerSize()
	{
		int num = (int)this.textBox.rectTransform.sizeDelta.x * 2 + 180;
		int num2 = (int)this.textBox.rectTransform.sizeDelta.y * 2 + 180;
		if (num < 250)
		{
			num = 250;
		}
		if (num2 < 250)
		{
			num2 = 250;
		}
		Vector2 vector = new Vector2((float)num, (float)num2);
		if (this.container.rectTransform.sizeDelta != vector)
		{
			this.container.rectTransform.sizeDelta = vector;
		}
	}

	public void PrepareString(string str)
	{
		string[] array = str.Split(new char[]
		{
			' '
		});
		int num = 0;
		this.targetString = new List<string>();
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			string text = array2[i];
			if (num + this.GetWordLength(text) > 30)
			{
				this.targetString.Add("\n");
				num = 0;
			}
			if (text.Contains("#"))
			{
				string text2 = text.Substring(1);
				for (int j = 0; j < text2.Length; j++)
				{
					char c = text2[j];
					this.targetString.Add("<color=#BCFF00FF>" + c + "</color>");
				}
				this.targetString.Add(" ");
				num += text.Length - 1;
			}
			else
			{
				string text3 = text;
				for (int k = 0; k < text3.Length; k++)
				{
					char c2 = text3[k];
					if (c2 == '\n')
					{
						num = 0;
					}
					this.targetString.Add(c2 + string.Empty);
				}
				this.targetString.Add(" ");
				num += text.Length;
			}
		}
	}

	public int GetWordLength(string word)
	{
		if (word.Contains("</color>"))
		{
			return word.Length - 24;
		}
		return word.Length;
	}

	public void Open(string str, bool timed = false, float duration = 0f)
	{
		if (this.isOpen)
		{
			return;
		}
		this.timed = timed;
		this.duration = duration;
		this.openTime = Time.time;
		base.gameObject.SetActive(false);
		this.textBox.text = string.Empty;
		this.PrepareString(str);
		this.SetContainerSize();
		this.delay = 0.059999998658895493;
		this.charCount = 0;
		this.container.rectTransform.sizeDelta = new Vector2(250f, 250f);
		base.gameObject.SetActive(true);
		this.canvas.alpha = 0f;
		this.isOpen = true;
	}

	public void Close()
	{
		if (!this.isOpen)
		{
			return;
		}
		this.isOpen = false;
	}
}
