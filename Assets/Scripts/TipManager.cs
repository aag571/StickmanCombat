// 反编译自程序集： Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipManager : MonoBehaviour
{
	public static TipManager instance;

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
		TipManager.instance = this;
	}

	public void Update()
	{
		if (this.isOpen)
		{
			if (this.canvas.alpha < 1f)
			{
				this.canvas.alpha += Time.unscaledDeltaTime * 5f;
			}
			else if (this.canvas.alpha > 1f)
			{
				this.canvas.alpha = 1f;
			}
			else if (this.nextLetterTime < (double)Time.unscaledTime && this.charCount < this.targetString.Count)
			{
				this.nextLetterTime = (double)Time.unscaledTime + this.delay;
				Text expr_AC = this.textBox;
				expr_AC.text += this.targetString[this.charCount];
				this.charCount++;
			}
		}
		else if (this.canvas.alpha > 0f)
		{
			this.canvas.alpha -= Time.unscaledDeltaTime * 5f;
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
			if (this.timed && this.openTime + this.duration < Time.unscaledTime)
			{
				this.Close(false);
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
			if (num + this.GetWordLength(text) > 100)
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
		this.openTime = Time.unscaledTime;
		base.gameObject.SetActive(false);
		this.textBox.text = string.Empty;
		this.PrepareString(str);
		this.SetContainerSize();
		this.delay = 0.02500000037252903;
		this.charCount = 0;
		this.canvas.alpha = 0f;
		this.container.rectTransform.sizeDelta = new Vector2(250f, 250f);
		base.gameObject.SetActive(true);
		this.isOpen = true;
	}

	public void Close(bool fastClose = false)
	{
		if (!this.isOpen)
		{
			return;
		}
		if (fastClose)
		{
			base.gameObject.SetActive(false);
		}
		this.isOpen = false;
	}
}
