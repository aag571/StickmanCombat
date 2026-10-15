// 反编译自程序集： Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
	public static StatManager instance;

	public List<Stat> stats;

	public StatContainer statContainerPrefab;

	protected void Awake()
	{
		StatManager.instance = this;
	}

	private void Start()
	{
		this.Initialize();
	}

	private void Initialize()
	{
		this.stats = new List<Stat>();
		this.stats.Add(new Stat(0, "最高得分", "分"));
		this.stats.Add(new Stat(1, "腰带", string.Empty));
		this.stats.Add(new Stat(2, "击败敌人数", string.Empty));
		this.stats.Add(new Stat(3, "击败首领数", string.Empty));
		this.stats.Add(new Stat(4, "击败暗影数", string.Empty));
		this.stats.Add(new Stat(5, "最快击败暗影用时", "秒"));
		this.stats.Add(new Stat(6, "闪避暗影次数", string.Empty));
		this.stats.Add(new Stat(7, "死亡次数", string.Empty));
		this.stats.Add(new Stat(8, "收集灵魂数", "个"));
		this.stats.Add(new Stat(9, "最高灵魂连击", string.Empty));
		this.stats.Add(new Stat(10, "消耗灵魂数", string.Empty));
		this.stats.Add(new Stat(11, "开启礼物数", string.Empty));
		this.stats.Add(new Stat(12, "获得重复物品数", string.Empty));
		bool flag = true;
		int[] defaultValue = new int[this.stats.Count];
        int[] arrayInt;
        if (PlayerPrefs.HasKey("statValues")) {

            arrayInt = PlayerPrefsX.GetIntArray("statValues");
        } else
        {
            arrayInt = defaultValue;
        }
		foreach (Stat current in this.stats)
		{
            if (StickmanCombat.Gameplay.GameFlowController.instance != null)
            {
                current.UpdateStat(current.id < arrayInt.Length ? arrayInt[current.id] : 0);
                continue;
            }
			if ((current.id < 3 || current.id > 6) && StickmanCombat.Gameplay.GameFlowController.instance == null)
			{
				StatContainer statContainer = UnityEngine.Object.Instantiate<StatContainer>(this.statContainerPrefab, this.statContainerPrefab.transform.parent);
				if (!flag)
				{
					statContainer.bg.color = new Color(1f, 1f, 1f, 0.05882353f);
				}
				statContainer.stat = current;
				current.statContainer = statContainer;
				current.UpdateStat(arrayInt[current.id]);
				statContainer.descriptionText.text = current.description;
				statContainer.stat.UpdateStat(statContainer.stat.value);
				flag = !flag;
				statContainer.gameObject.SetActive(true);
			}
		}
	}

	public void UpdateStats()
	{
		int[] array = new int[this.stats.Count];
		for (int i = 0; i < this.stats.Count; i++)
		{
			array[i] = this.stats[i].value;
		}
		PlayerPrefsX.SetIntArray("statValues", array);
	}
}
