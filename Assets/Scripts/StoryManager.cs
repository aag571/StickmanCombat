// 反编译自程序集： Assembly-CSharp.dll
using CodeStage.AntiCheat.ObscuredTypes;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
	public static StoryManager instance;

	public ObscuredInt level;

	public Queue<LevelStep> levelSteps;

	public LevelStep currentStep;

	private int stepCount;

	public bool introCompleted;

	public bool levelCompleted;

	public Npc miniboss;

	public int hitsTaken;

	protected void Awake()
	{
		StoryManager.instance = this;
	}

	protected void Start()
	{

        this.level = PlayerPrefs.GetInt("storyLevel", 0);

        if (this.level == 0)
		{
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, -10f);
		}
	}

	protected void Update()
	{
        if (StickmanCombat.Gameplay.GameFlowController.instance != null) return;
		this.UpdateCurrentStep();
		this.UpdateSpawns();
	}

	public void StartLevel()
	{
		this.hitsTaken = 0;
		if (SceneManager.instance.isBeltExamReady)
		{
			SceneManager.instance.isBeltExamReady = false;
            PlayerPrefsX.SetBool("BeltExam", false);
		}
		SceneManager.instance.inputStarted = false;
		this.introCompleted = false;
		this.levelCompleted = false;
		this.InstantiateSteps();
		this.StartNextStep();
		//AnalyticsManager.instance.PlayedBeltExam();
	}

	public void StartNextStep()
	{
		if (this.levelSteps.Count == 0)
		{
			if (!this.introCompleted)
			{
				this.IntroCompleted();
			}
		}
		else
		{
			this.stepCount++;
			this.currentStep = this.levelSteps.Dequeue();
			this.currentStep.Start();
		}
	}

	private void IntroCompleted()
	{
		SceneManager.instance.targetCameraZ = -7f;
		this.introCompleted = true;
		NpcManager.instance.killCount = 0;
		NpcManager.instance.lieutenantKillCount = 0;
		SceneManager.instance.inputStarted = true;
	}

	public void LevelCompleted()
	{
		this.levelCompleted = true;
		this.introCompleted = false;
		this.currentStep = null;
		this.level = ++this.level;
		if (this.level > 15)
		{
			this.level = this.hitsTaken;
		}
		StatManager.instance.stats[1].UpdateStat(this.level);
		PlayerPrefs.SetInt("storyLevel", this.level);
		ItemManager.instance.UpdateBelt();
		//if (this.level > 15)
		//{
		//	LeaderboardsManager.instance.PostShadowData();
		//}
		ScreenshotManager.instance.TakeScreenShot(true);
		SceneManager.instance.GameOver(true);
	}

	private void UpdateCurrentStep()
	{
		if (this.introCompleted || this.currentStep == null)
		{
			return;
		}
		if (this.currentStep.Update())
		{
			this.StartNextStep();
		}
	}

	public void StartFatality()
	{
		AudioManager.instance.StartFatalitySound();
		CharacterManager.instance.runTargetPos = this.miniboss.transform.position - new Vector3((float)this.miniboss.side, 0f, 0f);
		if (CharacterManager.instance.transform.position.x * (float)this.miniboss.side > CharacterManager.instance.runTargetPos.x * (float)this.miniboss.side)
		{
			CharacterManager.instance.transform.position = new Vector3(CharacterManager.instance.runTargetPos.x, CharacterManager.instance.runTargetPos.y, CharacterManager.instance.transform.position.z);
		}
		this.miniboss.weapon1Sprite.sprite = null;
		this.miniboss.weapon2Sprite.sprite = null;
		this.miniboss.weapon2hSprite.sprite = null;
		SceneManager.instance.gameStarted = false;
		SceneManager.instance.chaserObject.transform.localPosition = new Vector3((float)CharacterManager.instance.side, -1.5f, 0f);
		SceneManager.instance.targetCameraZ = -7.5f;
		CharacterManager.instance.spine.state.SetAnimation(0, ItemManager.instance.currentWeapon.category.id + "Fatality", false);
		this.miniboss.spine.state.SetAnimation(0,"FatalityGotHit", false);
	}

	public void EndFatality()
	{
		AudioManager.instance.EndFatalitySound();
		ItemManager.instance.ToggleTrails(false);
		this.miniboss.StartJustBlood();
	}

	public void AfterFatality()
	{
		SceneManager.instance.targetCameraZ = -9f;
		SceneManager.instance.chaserObject.transform.localPosition = Vector3.zero;
		this.miniboss.ChangeState(Npc.State.Dead);
		ItemManager.instance.GotItem(0, string.Empty, true, false);
	}

	private void UpdateSpawns()
	{
		if (!this.introCompleted || this.levelCompleted)
		{
			return;
		}
	}

	public void GameOver()
	{
		if (this.level > 15)
		{
			//if (this.level == int.Parse(LeaderboardsManager.instance.shadowWarriorData[4]))
			//{
			//	this.level = --this.level;
			//}
			if (this.hitsTaken > this.level)
			{
				this.level = this.hitsTaken;
				StatManager.instance.stats[1].UpdateStat(this.level);
				PlayerPrefs.SetInt("storyLevel", this.level);
				//LeaderboardsManager.instance.PostShadowData();
			}
		}
		//AnalyticsManager.instance.KilledByExaminer();
		this.introCompleted = false;
		this.currentStep = null;
	}

	private void InstantiateSteps()
	{
		this.stepCount = 0;
		this.levelSteps = new Queue<LevelStep>();
		this.levelSteps.Enqueue(new LevelStep_Wait(1f));
		this.levelSteps.Enqueue(new LevelStep_Animation(CharacterManager.instance.spine, string.Empty, true, 0f, false));
		this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, Vector3.zero));
		switch (this.level + 1)
		{
		case 1:
			this.levelSteps.Enqueue(new LevelStep_Tip("#班赞", true, 0f, false));
			this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, "fist2", "fullmetaljacket"));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "FistRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0.5f, false));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "FistIdle", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_Speech("让我们开始吧！", this.miniboss.head, 2.5f, false));
            this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "FistRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
			break;
		case 2:
			this.levelSteps.Enqueue(new LevelStep_Tip("#乔·卡曼", true, 0f, false));
			this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, "dagger4", "pennywise"));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "DaggerRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0.5f, false));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "DaggerIdle", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_Speech("想知道我为什么用匕首吗？", this.miniboss.head, 0.5f, false));
//            this.levelSteps.Enqueue(new LevelStep_Speech("The gun is too fast.", this.miniboss.head, 0.5f, false));
//            this.levelSteps.Enqueue(new LevelStep_Speech("You can’t taste all the small emotions.", this.miniboss.head, 0.5f, false));
            this.levelSteps.Enqueue(new LevelStep_Speech("你看……人在生命的最后一刻，才会显露真实的自己。", this.miniboss.head, 1f, false));
            this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "DaggerRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
			break;
		case 3:
			this.levelSteps.Enqueue(new LevelStep_Tip("#酒鬼", true, 0f, false));
			this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, "dagger2", "beertime"));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "DaggerRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0.5f, false));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "DaggerIdle", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_Speech("要不要……嗝……来点啤酒？……嗝……", this.miniboss.head, 2.5f, false));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "DaggerRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
			break;
		case 4:
			this.levelSteps.Enqueue(new LevelStep_Tip("#屠夫", true, 0f, false));
			this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, "axe5", "spikey"));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "AxeRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0.5f, false));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "AxeIdle", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_Speech("靠近点，鲜嫩的猎物！", this.miniboss.head, 2.5f, false));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "AxeRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
			break;
		case 5:
			this.levelSteps.Enqueue(new LevelStep_Tip("#雅科·托诺科梅", true, 0f, false));
			this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, "sword1h2", "maskofthedead"));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword1hRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0.5f, false));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword1hIdle", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_Speech("如果没人打扰，生活本可以很美好。", this.miniboss.head, 1f, false));
//            this.levelSteps.Enqueue(new LevelStep_Speech("I won't hurt you.", this.miniboss.head, 0.5f, false));
//            this.levelSteps.Enqueue(new LevelStep_Speech("I just want to smash your head!", this.miniboss.head, 1f, false));
            this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword1hRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
			break;
		case 6:
			this.levelSteps.Enqueue(new LevelStep_Tip("#海森堡", true, 0f, false));
			this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, "fist4", "troll"));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "FistRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0.5f, false));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "FistIdle", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_Speech("我才是那个叩响死亡之门的人。", this.miniboss.head, 2.5f, false));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "FistRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
                break;
		case 7:
			this.levelSteps.Enqueue(new LevelStep_Tip("#本雷托", true, 0f, false));
			this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, "spear5", "vietnamese"));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "SpearRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0.5f, false));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "SpearIdle", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_Speech("尝尝长矛的威力吧！", this.miniboss.head, 1f, false));
//            this.levelSteps.Enqueue(new LevelStep_Speech("可别哭出来。", this.miniboss.head, 1.5f, false));
            this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "SpearRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
			break;
		case 8:
			this.levelSteps.Enqueue(new LevelStep_Tip("#埃托", true, 0f, false));
			this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, "sword2h2", "viking"));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword2hRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0.5f, false));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword2hIdle", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_Speech("可别哭出来。", this.miniboss.head, 1.5f, false));
//                this.levelSteps.Enqueue(new LevelStep_Speech("Take the bridal lane. This is Kalksa.", this.miniboss.head, 1f, false));
                this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword2hRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
			break;
		case 9:
			this.levelSteps.Enqueue(new LevelStep_Tip("#库巴杜", true, 0f, false));
			this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, "sword1h6", "spanish"));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword1hRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0.5f, false));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword1hIdle", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_Speech("诸神又一次眷顾了这个该死的混蛋！", this.miniboss.head, 2.5f, false));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword1hRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
			break;
		case 10:
			this.levelSteps.Enqueue(new LevelStep_Tip("#国王", true, 0f, false));
			this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, "spear3", "theking"));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "SpearRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0.5f, false));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "SpearIdle", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_Speech("智者说，只有愚者才会鲁莽行事。", this.miniboss.head, 2.5f, false));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "SpearRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
			break;
		case 11:
			this.levelSteps.Enqueue(new LevelStep_Tip("#梅格", true, 0f, false));
			this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, "axe7", "ottoman"));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "AxeRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0.5f, false));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "AxeIdle", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_Speech("杀女人和杀男人可不一样。", this.miniboss.head, 1.5f, false));
            this.levelSteps.Enqueue(new LevelStep_Speech("你得换一种方式扣动扳机。", this.miniboss.head, 1f, false));
                this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "AxeRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
			break;
		case 12:
			this.levelSteps.Enqueue(new LevelStep_Tip("#乔纳·林德", true, 0f, false));
			this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, "axe8", "dumbo"));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "AxeRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0.5f, false));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "AxeIdle", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_Speech("想知道眼球被刺穿会怎样吗？", this.miniboss.head, 1f, false));
            this.levelSteps.Enqueue(new LevelStep_Speech("知道喉咙被割开后，脖子会流多少血吗？", this.miniboss.head, 1.5f, false));
            this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "AxeRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
			break;
		case 13:
			this.levelSteps.Enqueue(new LevelStep_Tip("#凯撒", true, 0f, false));
			this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, "dagger6", "caesar"));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "DaggerRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0.5f, false));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "DaggerIdle", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_Speech("我来，我见，我征服。", this.miniboss.head, 2.5f, false));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "DaggerRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
			break;
		case 14:
			this.levelSteps.Enqueue(new LevelStep_Tip("#拉格纳", true, 0f, false));
			this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, "axe1", "ragnar"));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "AxeRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0.5f, false));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "AxeIdle", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_Speech("我不会因疲惫而停下，只有完成目标才会停下。", this.miniboss.head, 2.5f, false));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "AxeRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
			break;
		case 15:
			this.levelSteps.Enqueue(new LevelStep_Tip("#皇帝", true, 0f, false));
			this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, "sword2h6", "emperor"));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword2hRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0.5f, false));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword2hIdle", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_Speech("武士，你离终点已经很近了。", this.miniboss.head, 1f, false));
            this.levelSteps.Enqueue(new LevelStep_Speech("但我会享受击杀你的乐趣！", this.miniboss.head, 1.5f, false));
            this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword2hRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
			break;
		case 16:
			this.levelSteps.Enqueue(new LevelStep_Tip("#白眉大师", true, 0f, false));
			this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, "sword2h5", "paimei"));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword2hRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0.5f, false));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword2hIdle", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_Speech("干得好，武士。\n你已超越我所有的弟子。", this.miniboss.head, 2.5f, false));
			this.levelSteps.Enqueue(new LevelStep_Speech("暗影大师？", CharacterManager.instance.head, 1f, false));
			this.levelSteps.Enqueue(new LevelStep_Speech("没错，我就是暗影大师白眉。\n击败我，这个称号就是你的！", this.miniboss.head, 2.5f, false));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword2hRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
			break;
            case 17:
                this.levelSteps.Enqueue(new LevelStep_Tip("#暗影武士", true, 0f, false));
                this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, "sword2h2", "thor"));
                this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
                this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword2hRun", true, 0f, true));
                this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0.5f, false));
                this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
                this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword2hIdle", true, 0f, true));
                this.levelSteps.Enqueue(new LevelStep_Speech("你以为能夺走我的称号？哈！", this.miniboss.head, 2.5f, false));
                this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword2hRun", true, 0f, true));
                this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
                this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
                break;
            default:
				this.levelSteps.Enqueue(new LevelStep_Tip("#自我", true, 0f, false));
				this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, ItemManager.instance.currentWeapon.id, ItemManager.instance.currentHat.id));
				this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
				this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, ItemManager.instance.currentWeapon.category.id + "Run", true, 0f, true));
				this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0f, false));
				this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
				this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, ItemManager.instance.currentWeapon.category.id + "Idle", true, 0f, true));
				this.levelSteps.Enqueue(new LevelStep_Speech("当只剩下你自己时，你必须面对最强大的敌人。", this.miniboss.head, 2.5f, false));
				this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, ItemManager.instance.currentWeapon.category.id + "Run", true, 0f, true));
				this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
				this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
			
			break;
		}
		this.levelSteps.Enqueue(new LevelStep_Tip(string.Empty, false, 0f, false));
	}

	public int GetCoinStageCount(int l)
	{
		int num = 3;
		num += this.level / 150 * 7;
		if (num > 10)
		{
			num = 10;
		}
		return num;
	}

	public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition)
	{
		Ray ray = Camera.main.ScreenPointToRay(screenPosition);
		Plane plane = new Plane(Vector3.forward, new Vector3(0f, 0f, CharacterManager.instance.transform.position.z));
		float distance;
		plane.Raycast(ray, out distance);
		return ray.GetPoint(distance);
	}
}
