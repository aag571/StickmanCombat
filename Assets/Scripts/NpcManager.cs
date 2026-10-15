// 反编译自程序集： Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
	private List<Npc> npcPool = new List<Npc>();

	public LinkedList<Npc> leftNpcs = new LinkedList<Npc>();

	public LinkedList<Npc> rightNpcs = new LinkedList<Npc>();

    //public LinkedList<Npc> listNpcsRemove = new LinkedList<Npc>();

	private Npc npcPrefab;

	public Transform poolTransform;

	public Transform activeTransform;

	public Material npcMaterial;

	public static NpcManager instance;

	private const int poolSize = 20;
    private float nextSpawnTime;

	private const float baseSpawnDistance = 10f;

	public const float minSpeed = 3f;

	public const float maxSpeed = 6f;

	public int progress;

	public int targetProgress;

	public int diffTier;

	public float speed = 3f;

	public Transform uiTransform;

	public bool paused;

	public int killCount;

	public int lieutenantKillCount;

	public double gameStartTime;

	public bool spawnBreak;

	public MenuToggle bloodToggler;

	private void Awake()
	{
		NpcManager.instance = this;
	}

	private void Start()
	{
		this.Initialize();
	}

	private void Update()
	{
        if (paused) return;
		this.NpcMarkCheck(-1);
		this.NpcMarkCheck(1);
		this.NpcAttackCheck(-1);
		this.NpcAttackCheck(1);
        var flow = StickmanCombat.Gameplay.GameFlowController.instance;
        if (flow != null)
        {
            if (flow.AllowsContinuousSpawning && Time.timeScale > 0f) SpawnProgressiveEnemies(flow);
            return;
        }
		if (SceneManager.instance.gameStarted && SceneManager.instance.isEndless && !this.paused && (StickmanCombat.Gameplay.GameFlowController.instance == null || StickmanCombat.Gameplay.GameFlowController.instance.AllowsContinuousSpawning))
		{
			int num = this.leftNpcs.Count + this.rightNpcs.Count;
			if (TutorialManager.instance.tutorialIndex < 4)
			{
				if (num == 0)
				{
					switch (TutorialManager.instance.tutorialIndex)
					{
					case 0:
						TutorialManager.instance.ActivateTutorial(0);
						this.SpawnEnemy(1, 12f, 2f, 0);
						break;
					case 1:
						TutorialManager.instance.ActivateTutorial(1);
						this.SpawnEnemy(-1, 12f, 2f, 0);
						break;
					case 2:
						TutorialManager.instance.ActivateTutorial(2);
						this.SpawnEnemy(-1, 12f, 2f, 2);
						break;
					case 3:
						TutorialManager.instance.ActivateTutorial(3);
						this.SpawnEnemy(1, 12f, 2f, 3);
						break;
					}
				}
			}
			else if (num < 10)
			{
				this.PrepareSpawn(-1f, -1f, -1, -1, -1f);
			}
		}
	}

	private void Initialize()
	{
		this.speed = 3f;
		this.npcPrefab = Resources.Load<Npc>("Prefabs/Enemy");
		for (int i = 0; i < poolSize; i++)
		{
			Npc npc = UnityEngine.Object.Instantiate<Npc>(this.npcPrefab);
			npc.manager = this;
			npc.transform.SetParent(this.poolTransform);
			this.npcPool.Add(npc);
		}
	}

	public void OnGameStart()
	{
        nextSpawnTime = Time.time + 0.8f;
		this.gameStartTime = (double)Time.time;
		this.progress = 0;
		this.targetProgress = 30;
		this.diffTier = 0;
		this.killCount = 0;
		this.lieutenantKillCount = 0;
		this.progress = 0;
		this.spawnBreak = false;
	}

	public void PrepareSpawn(float minGap = -1f, float maxGap = -1f, int typeL = -1, int typeR = -1, float spd = -1f)
	{
		if (this.diffTier > 80)
		{
			if (this.spawnBreak)
			{
				if (this.leftNpcs.Count + this.rightNpcs.Count > 0)
				{
					return;
				}
				this.spawnBreak = false;
			}
			else if (this.progress > this.targetProgress)
			{
				this.progress = 0;
				this.targetProgress = UnityEngine.Random.Range(25, 40);
				this.spawnBreak = true;
			}
			this.progress++;
		}
		this.diffTier = (int)(((double)Time.time - this.gameStartTime - 5.0) / 1.0);
		if (this.diffTier < 0)
		{
			this.diffTier = 0;
		}
		if (this.diffTier >= 90)
		{
			this.diffTier = 90;
		}
		float num = (float)this.diffTier;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		if (this.leftNpcs.Count == 0)
		{
			num5 = 10f;
		}
		if (this.rightNpcs.Count == 0)
		{
			num6 = 10f;
		}
		if (minGap == -1f || maxGap == -1f)
		{
			num2 = Mathf.Sqrt(num / 8f);
			if (num2 > 5f)
			{
				num2 = 5f;
			}
			if (num2 < 2f)
			{
				num2 = 2f;
			}
			num3 = Mathf.Sqrt(9f * num / 50f);
			if (num3 > 3f)
			{
				num3 = 3f;
			}
		}
		if (minGap == -1f)
		{
			minGap = UnityEngine.Random.Range(2f, 10f - num2) + 3f - num3;
		}
		if (maxGap == -1f)
		{
			maxGap = UnityEngine.Random.Range(2f, 10f - num2) + 3f - num3;
		}
		if (minGap > maxGap)
		{
			float num7 = minGap;
			minGap = maxGap;
			maxGap = num7;
		}
		bool flag;
		if (this.leftNpcs.Count > 0 && this.rightNpcs.Count > 0)
		{
			flag = (Mathf.Abs(this.leftNpcs.Last.Value.transform.position.x - CharacterManager.instance.transform.position.x) > Mathf.Abs(this.rightNpcs.Last.Value.transform.position.x - CharacterManager.instance.transform.position.x));
		}
		else
		{
			flag = (this.leftNpcs.Count <= 0);
		}
		if (flag)
		{
			num5 += minGap;
			num6 += maxGap;
		}
		else
		{
			num5 += maxGap;
			num6 += minGap;
		}
		if (spd == -1f)
		{
			spd = Mathf.Sqrt(9f + 9f * num / 50f);
			if (spd > 6f)
			{
				spd = 6f;
			}
		}
		if (typeL == -1 || typeR == -1)
		{
			num4 = 1f + num * num / 39900f;
			if (num4 > 2f)
			{
				num4 = 2f;
			}
		}
		if (typeL == -1)
		{
			if (UnityEngine.Random.Range(0f, 4f) <= num4)
			{
				typeL = UnityEngine.Random.Range(2, 4);
			}
			else
			{
				typeL = 0;
			}
		}
		if (typeR == -1)
		{
			if (UnityEngine.Random.Range(0f, 4f) <= num4)
			{
				typeR = UnityEngine.Random.Range(2, 4);
			}
			else
			{
				typeR = 0;
			}
		}
		if (this.diffTier == 0)
		{
			typeR = 0;
			typeL = 0;
		}
		if (this.rightNpcs.Count > 5)
		{
			this.SpawnEnemy(-1, num5, spd, typeL);
			this.SpawnEnemy(-1, num6, spd, typeL);
		}
		else if (this.leftNpcs.Count > 5)
		{
			this.SpawnEnemy(1, num6, spd, typeL);
			this.SpawnEnemy(1, num5, spd, typeL);
		}
		else
		{
			this.SpawnEnemy(-1, num5, spd, typeL);
			this.SpawnEnemy(1, num6, spd, typeR);
		}
	}

	public void CheckAndDestroyExcessEnemy()
	{
        if (StickmanCombat.Gameplay.GameFlowController.instance != null) return;
        //if (this.listNpcsRemove.Count >= 10)
        //{
        //    this.listNpcsRemove.Last.Value.Reset();
        //    this.listNpcsRemove.RemoveLast();
        //}


        if (this.rightNpcs.Count > 6)
		{
			this.ResetAndRemove(1);
		}
		if (this.leftNpcs.Count > 6)
		{
			this.ResetAndRemove(-1);
		}
    }

	public Npc SpawnEnemy(int side, float gap, float spd, int type = -1)
	{
        if (npcPool.Count == 0)
        {
            Npc extra = UnityEngine.Object.Instantiate<Npc>(npcPrefab);
            extra.manager = this;
            extra.transform.SetParent(poolTransform);
            npcPool.Add(extra);
        }
		Npc npc = this.npcPool[0];
		this.npcPool.RemoveAt(0);
		npc.transform.SetParent(this.activeTransform);
        npc.hitSides.Clear();
        npc.isSpecial = false;
		npc.hitSides.Add(side);
		if (type == 2)
		{
			npc.hitSides.Add(side);
			npc.isSpecial = true;
		}
		else if (type == 3)
		{
			npc.hitSides.Add(-side);
			npc.isSpecial = true;
		}
		npc.type = 0;
		if (gap > 6f)
		{
			npc.gap = UnityEngine.Random.Range(3f, 6f);
		}
		else
		{
			npc.gap = gap;
		}
		Vector3 position = CharacterManager.instance.transform.position;
		if (side == -1)
		{
			if (this.leftNpcs.Count == 0)
			{
				npc.transform.position = new Vector3(position.x + (float)side * gap, CharacterManager.instance.transform.position.y, position.z);
			}
			else
			{
				float num = this.leftNpcs.Last.Value.transform.position.x + (float)side * gap;
				if (position.x - num < 10f)
				{
					num = position.x - 10f;
				}
				npc.transform.position = new Vector3(num, CharacterManager.instance.transform.position.y, position.z);
			}
		}
		else if (this.rightNpcs.Count == 0)
		{
			npc.transform.position = new Vector3(position.x + (float)side * gap, CharacterManager.instance.transform.position.y, position.z);
		}
		else
		{
			float num2 = this.rightNpcs.Last.Value.transform.position.x + (float)side * gap;
			if (num2 - position.x < 10f)
			{
				num2 = position.x + 10f;
			}
			npc.transform.position = new Vector3(num2, CharacterManager.instance.transform.position.y, position.z);
		}
		npc.side = side;
		if (spd > 6f)
		{
			spd = 6f;
		}
		npc.speed = spd;
		npc.hitMarkers.transform.SetParent(this.uiTransform);
		npc.hitMarkers.transform.localScale = Vector3.one;
		npc.hitMarkers.transform.SetAsFirstSibling();
		npc.Activate();
		if (side == -1)
		{
			npc.node = this.leftNpcs.AddLast(npc);
		}
		else
		{
            npc.node = this.rightNpcs.AddLast(npc);
		}
		return npc;
	}

    private void SpawnProgressiveEnemies(StickmanCombat.Gameplay.GameFlowController flow)
    {
        if (Time.time < nextSpawnTime) return;
        var difficulty = flow.CurrentDifficulty;
        if (leftNpcs.Count + rightNpcs.Count >= difficulty.MaxEnemies) return;
        nextSpawnTime = Time.time + difficulty.SpawnInterval;
        int side = leftNpcs.Count < rightNpcs.Count ? -1 : rightNpcs.Count < leftNpcs.Count ? 1 : UnityEngine.Random.value < 0.5f ? -1 : 1;
        bool advanced = difficulty.AdvancedChance >= 1f || UnityEngine.Random.value < difficulty.AdvancedChance;
        int type = advanced ? UnityEngine.Random.Range(2, 4) : 0;
        Npc enemy = SpawnEnemy(side, (side == -1 ? leftNpcs.Count : rightNpcs.Count) == 0 ? 10f : difficulty.Gap, difficulty.Speed, type);
        if (advanced)
        {
            if (difficulty.ExtraHits > 0)
            {
                enemy.hitSides.Add(type == 3 ? side : -side);
                enemy.hitMarkers.Initialize();
            }
            var weapons = new List<Weapon>();
            foreach (Weapon weapon in ItemManager.instance.weaponsById.Values)
                if (weapon.id != "fist0" && StickmanCombat.Gameplay.WeaponCombatProfile.IsSelectable(weapon) && weapon.category.index <= difficulty.WeaponTier) weapons.Add(weapon);
            if (weapons.Count > 0)
            {
                enemy.currentWeapon = weapons[UnityEngine.Random.Range(0, weapons.Count)];
                enemy.EquipWeapon();
                enemy.ChangeState(Npc.State.Running);
            }
        }
        enemy.SetCombatColor(advanced ? new Color(0.85f, 0.2f, 0.28f, 1f) : new Color(0.62f, 0.38f, 0.16f, 1f));
    }

	public void SpawnEnemyStarting(int side)
	{
		Npc npc = this.npcPool[0];
		this.npcPool.RemoveAt(0);
		npc.transform.SetParent(this.activeTransform);
		npc.transform.position = new Vector3(Camera.main.transform.position.x + (float)side * 10f, CharacterManager.instance.transform.position.y, CharacterManager.instance.transform.position.z);
		npc.side = side;
		npc.speed = 5f;
		npc.Activate();
		if (side == -1)
		{
			npc.node = this.leftNpcs.AddLast(npc);
		}
		else
		{
            npc.node = this.rightNpcs.AddLast(npc);
		}

    }

	private void NpcAttackCheck(int side)
	{
		Npc npc = null;
		if (side == -1)
		{
			if (this.leftNpcs.Count > 0)
			{
				npc = this.leftNpcs.First.Value;
			}
		}
		else if (this.rightNpcs.Count > 0)
		{
			npc = this.rightNpcs.First.Value;
		}
		if (npc == null)
		{
			return;
		}
		if (npc.CanHit() && CharacterManager.instance.CanBeHit())
		{
			npc.Hit();
		}
	}

	private void NpcMarkCheck(int side)
	{
		if (side == 1)
		{
			if (this.rightNpcs.Count < 1)
			{
				return;
			}
			if (this.rightNpcs.First.Value.markedForKill)
			{
				if (this.rightNpcs.First.Value.transform.position.x - CharacterManager.instance.transform.position.x > 7f)
				{
					this.rightNpcs.First.Value.UnMarkForKill();
				}
				return;
			}
			if (this.rightNpcs.First.Value.transform.position.x < CharacterManager.instance.GetThreatXPos(1))
			{
				this.rightNpcs.First.Value.MarkForKill();
			}
		}
		else
		{
			if (this.leftNpcs.Count < 1)
			{
				return;
			}
			if (this.leftNpcs.First.Value.markedForKill)
			{
				if (CharacterManager.instance.transform.position.x - this.leftNpcs.First.Value.transform.position.x > 7f)
				{
					this.leftNpcs.First.Value.UnMarkForKill();
				}
				return;
			}
			if (this.leftNpcs.First.Value.transform.position.x > CharacterManager.instance.GetThreatXPos(-1))
			{
				this.leftNpcs.First.Value.MarkForKill();
			}
		}
	}

	public Npc PlayerAttackCheck(int side, float xPosition)
	{
		if (side == 1)
		{
			if (this.rightNpcs.Count < 1)
			{
				return null;
			}
			if (this.rightNpcs.First.Value.markedForKill || this.rightNpcs.First.Value.transform.position.x < xPosition)
			{
				return this.rightNpcs.First.Value;
			}
		}
		else
		{
			if (this.leftNpcs.Count < 1)
			{
				return null;
			}
			if (this.leftNpcs.First.Value.markedForKill || this.leftNpcs.First.Value.transform.position.x > xPosition)
			{
				return this.leftNpcs.First.Value;
			}
		}
		return null;
	}

	public void RemoveNpc(int side)
	{
		if (side == 1)
		{
            //this.rightNpcs.First.Value.Reset();
            //listNpcsRemove.AddFirst(this.rightNpcs.First.Value);
			this.rightNpcs.RemoveFirst();
		}
		else
		{
            //this.leftNpcs.First.Value.Reset();
            //listNpcsRemove.AddFirst(this.leftNpcs.First.Value);
            this.leftNpcs.RemoveFirst();
		}
	}

	public void ResetAndRemove(int side)
	{
		if (side == 1)
		{
			this.rightNpcs.Last.Value.Reset();
			this.rightNpcs.RemoveLast();
		}
		else
		{
			this.leftNpcs.Last.Value.Reset();
			this.leftNpcs.RemoveLast();
		}
	}

	public void PauseAll()
	{
		this.paused = true;
		foreach (Npc current in this.leftNpcs)
		{
			current.Pause();
		}
		foreach (Npc current2 in this.rightNpcs)
		{
			current2.Pause();
		}
	}

	public void ContinueAll()
	{
		this.paused = false;
		foreach (Npc current in this.leftNpcs)
		{
			current.Continue();
		}
		foreach (Npc current2 in this.rightNpcs)
		{
			current2.Continue();
		}
	}

	public void ResetNpc(Npc npcToReset)
	{

		this.npcPool.Add(npcToReset);
		npcToReset.transform.SetParent(this.poolTransform);
	}

	public void KillAll(Vector2 impactVector, Vector2 randomAmount, Npc keepAlive = null)
	{
		while (this.leftNpcs.Count > 0)
		{
			if (this.leftNpcs.First.Value != keepAlive)
			{
				this.leftNpcs.First.Value.Die(new Vector2(impactVector.x + UnityEngine.Random.Range(0f, randomAmount.x), impactVector.y + UnityEngine.Random.Range(0f, randomAmount.y)));
			}
			else
			{
				this.leftNpcs.RemoveFirst();
			}
		}
		while (this.rightNpcs.Count > 0)
		{
			if (this.rightNpcs.First.Value != keepAlive)
			{
				this.rightNpcs.First.Value.Die(new Vector2(impactVector.x + UnityEngine.Random.Range(0f, randomAmount.x), impactVector.y + UnityEngine.Random.Range(0f, randomAmount.y)));
			}
			else
			{
				this.rightNpcs.RemoveFirst();
			}
		}
	}

	public void SilentKillAll()
	{
		base.enabled = false;
		while (this.leftNpcs.Count > 0)
		{
			this.leftNpcs.First.Value.SilentDie();
		}
		while (this.rightNpcs.Count > 0)
		{
			this.rightNpcs.First.Value.SilentDie();
		}
	}

	public void OnGameOver()
	{
		this.PauseAll();
		StatManager.instance.stats[2].UpdateStat(StatManager.instance.stats[2].value + this.killCount);
	}
}
