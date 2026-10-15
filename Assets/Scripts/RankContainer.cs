// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.UI;

public class RankContainer : MonoBehaviour
{
	public int index;

	public new Text name;

	public Text rank;

	public Text score;

	public Text coinCombo;

	public Text shadowBelt;

	public Image belt;

	public Image altBelt;

	public Image image;

	public GameObject editTitle;

	public PlayerData playerData;

	public void Initialize(PlayerData playerData, int index)
	{
		this.index = index;
		this.playerData = playerData;
		this.editTitle.SetActive(false);
		if (playerData.id == SystemInfo.deviceUniqueIdentifier)
		{
			this.image.color = new Color(1f, 0.8156863f, 0.360784322f, 0.5f);
		}
		else if (playerData.ranking % 2 == 0)
		{
			this.image.color = new Color(1f, 1f, 1f, 0.05882353f);
		}
		else
		{
			this.image.color = new Color(1f, 1f, 1f, 0f);
		}
		this.score.text = playerData.score.ToString();
		this.coinCombo.text = playerData.coinCombo.ToString();
		this.rank.text = playerData.ranking.ToString() + ".   ";
		int num = playerData.beltLevel - 1;
		this.name.text = playerData.name;
		if (playerData.beltLevel > 15)
		{
			this.shadowBelt.text = "(" + playerData.beltLevel + ")";
			if (LeaderboardsManager.instance.shadowWarriorData[0] == playerData.id)
			{
				this.name.text = playerData.name + " 影子武士";
			}
		}
		else
		{
			this.shadowBelt.text = string.Empty;
		}
		if (num > 15)
		{
			num = 15;
		}
		if (num < 0)
		{
			this.belt.color = new Color(1f, 1f, 1f, 0f);
			this.altBelt.gameObject.SetActive(false);
		}
		else
		{
			this.belt.color = ItemManager.instance.beltColors[num];
			if (num != 0 && num % 2 == 0)
			{
				this.altBelt.gameObject.SetActive(true);
			}
			else
			{
				this.altBelt.gameObject.SetActive(false);
			}
		}
	}
}
