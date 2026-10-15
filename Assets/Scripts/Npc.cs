// 反编译自程序集： Assembly-CSharp.dll
using Spine;
using Spine.Unity;
using Spine.Unity.Modules;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
	public enum State
	{
		Deactive,
		Running,
		Hitting,
		Waiting,
		Triggered,
		Falling,
		Dead
	}

	public SkeletonAnimation spine;

	public SkeletonRagdoll2D ragdoll;

	public NpcManager manager;

	public ParticleSystem blood;

	public ParticleSystem bloodBurst;

	public ParticleSystem fatalityBlood1;

	public ParticleSystem fatalityBlood2;

	public MeshRenderer meshRenderer;

	public GameObject head;

	public float speed = 3f;

	public HitMarkers hitMarkers;

	public bool markedForKill;

	private float pausedSpeed;

	private Color defaultColor = Color.white;

	public List<int> hitSides = new List<int>();

	public SpriteRenderer hatSprite;

	public SpriteRenderer weapon2hSprite;

	public SpriteRenderer weapon1Sprite;

	public SpriteRenderer weapon2Sprite;

    public SpriteRenderer fistweapon1Sprite;

    public SpriteRenderer fistweapon2Sprite;

    public Weapon currentWeapon;

	public Hat currentHat;

	public GameObject hatGlow;

	public int type;

	private float slideSpeed;

	private float slideTarget;

	private double waitEndTime;

	private bool dontReset;

	public bool isLieutenant;

	public bool isSpecial;

	private Vector2 playerImpactVector;

	public float gap;

	private bool ragdollApplied;

	public LinkedListNode<Npc> node;

	public float hitDistance = 0.4f;

	public Npc.State currentState;

	public int side = 1;

    private Transform baseTransform;
    private Material baseMaterial;
	private void Start()
	{
		this.spine.state.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.OnSpineEvent);
        baseTransform = base.transform;
        baseMaterial = this.meshRenderer.material;

    }

    private float num;
    private float num2 = 1.3f;

	private void Update()
	{
		switch (this.currentState)
		{
		case Npc.State.Running:
			if (this.node != null && this.node.Previous != null)
			{
				num = Mathf.Abs(baseTransform.position.x - this.node.Previous.Value.transform.position.x);
				if (num < this.gap)
				{
					this.node.Previous.Value.speed = this.speed;
				}
				num2 = 1.3f;
				if (this.node.Previous.Previous != null)
				{
					num2 = this.gap;
				}
				if (num < num2)
				{
                        baseTransform.position = new Vector3(this.node.Previous.Value.transform.position.x + num2 * (float)this.side, baseTransform.position.y, baseTransform.position.z);
				}
			}
                baseTransform.position += new Vector3((float)(-(float)this.side) * Time.deltaTime * this.speed, 0f, 0f);
			if (baseMaterial.color != this.defaultColor)
			{
                    baseMaterial.SetColor("_Color", this.defaultColor);

            }
			break;
		case Npc.State.Hitting:
			if (this.hitMarkers.cGroup.alpha > 0f)
			{
				this.hitMarkers.cGroup.alpha -= Time.deltaTime * 3f;
			}
			else if (this.hitMarkers.cGroup.alpha < 0f)
			{
				this.hitMarkers.cGroup.alpha = 0f;
			}
			//base.transform.position = Vector3.MoveTowards(base.transform.position, new Vector3(CharacterManager.instance.transform.position.x + (float)this.side * this.hitDistance, base.transform.position.y, base.transform.position.z), Time.deltaTime * 30f);
			break;
		case Npc.State.Waiting:
			if (this.hitSides.Count > 0)
			{
				if ((float)this.side * baseTransform.position.x < (float)this.side * this.slideTarget)
				{
                        baseTransform.position += new Vector3((float)this.side * Time.deltaTime * this.slideSpeed, 0f, 0f);
				}
				if ((float)this.side * baseTransform.position.x > (float)this.side * this.slideTarget)
				{
                        baseTransform.position = new Vector3(this.slideTarget, baseTransform.position.y, baseTransform.position.z);
				}
				if ((double)Time.time > this.waitEndTime + 0.20000000298023224 - (double)(0.2f * ((this.speed - 3f) / 3f)))
				{
					this.ChangeState(Npc.State.Running);
				}
			}
			break;
		case Npc.State.Triggered:
            baseTransform.position += new Vector3((float)(-(float)this.side) * Time.deltaTime * this.speed, 0f, 0f);
                if (this.hitMarkers.cGroup.alpha > 0f)
                {
                    this.hitMarkers.cGroup.alpha -= Time.deltaTime * 3f;
                }
                else if (this.hitMarkers.cGroup.alpha < 0f)
                {
                    this.hitMarkers.cGroup.alpha = 0f;
                }
                this.CanReset();
			break;
		case Npc.State.Falling:
                //base.transform.position = Vector3.Lerp(base.transform.position, new Vector3(base.transform.position.x, base.transform.position.y, -2f), Time.deltaTime * 10f);
                if (this.hitMarkers.cGroup.alpha > 0f)
                {
                	this.hitMarkers.cGroup.alpha -= Time.deltaTime * 3f;
                }
                else if (this.hitMarkers.cGroup.alpha < 0f)
                {
                	this.hitMarkers.cGroup.alpha = 0f;
                }
                this.CanReset();
			break;
		case Npc.State.Dead:
                this.CanReset();
                if (this.hitMarkers.cGroup.alpha > 0f)
                {
                    this.hitMarkers.cGroup.alpha -= Time.deltaTime * 3f;
                }
                else if (this.hitMarkers.cGroup.alpha < 0f)
                {
                    this.hitMarkers.cGroup.alpha = 0f;
                }
                break;
		}
	}


	public void ChangeState(Npc.State newState)
	{
		if (newState != Npc.State.Running)
		{
			if (newState == Npc.State.Waiting)
			{
				if (this.hitSides.Count > 0)
				{
                    //this.spine.state.SetAnimation(0, "Block" + UnityEngine.Random.Range(0, 2), false);
                    this.spine.state.SetAnimation(0, "Block0", false);
                    if (this.side != this.hitSides[0])
					{
						if (this.side == -1)
						{
							NpcManager.instance.leftNpcs.RemoveFirst();
							this.node = NpcManager.instance.rightNpcs.AddFirst(this);
						}
						else
						{
							NpcManager.instance.rightNpcs.RemoveFirst();
							this.node = NpcManager.instance.leftNpcs.AddFirst(this);
						}
						this.side = this.hitSides[0];
						this.spine.skeleton.flipX = (this.side == 1);
					}
					this.slideTarget = CharacterManager.instance.runTargetPos.x + (float)this.side * 1.8f;
					this.slideSpeed = Mathf.Abs(this.slideTarget - base.transform.position.x) / 0.1f;
					this.waitEndTime = (double)(Time.time + 0.1f);
				}
			}
		}
		else if (this.currentWeapon.category.id == "Fist")
		{
			this.spine.state.SetAnimation(0, "FistRun", true);
		}
		else if (this.currentWeapon.category.id == "Axe")
        {
			this.spine.state.SetAnimation(0, "AxeRun", true);
		}
        else if (this.currentWeapon.category.id == "Dagger")
        {
            this.spine.state.SetAnimation(0, "DaggerRun", true);
        }
        else if (this.currentWeapon.category.id == "Spear")
        {
            this.spine.state.SetAnimation(0, "SpearRun", true);
        }
        else if (this.currentWeapon.category.id == "Sword1h")
        {
            this.spine.state.SetAnimation(0, "Sword1hRun", true);
        }
        else if (this.currentWeapon.category.id == "Sword2h")
        {
            this.spine.state.SetAnimation(0, "Sword2hRun", true);
        }
        this.currentState = newState;
	}

	public void Activate()
	{
		base.gameObject.SetActive(true);
		if (this.hitSides.Count < 2)
		{
			this.hatSprite.color = Color.black;
			this.currentWeapon = ItemManager.instance.weaponsById["fist0"];
		}
		else if (this.hitSides.Count > 2)
		{
			this.hatSprite.color = Color.black;
			this.weapon1Sprite.color = Color.black;
			this.weapon2Sprite.color = Color.black;
			this.weapon2hSprite.color = Color.black;
		}
		else if (this.hitSides[0] != this.hitSides[1])
		{
			this.currentHat = ItemManager.instance.hatsById["spikey"];
			this.EquipHat();
			this.currentWeapon = ItemManager.instance.weaponsById["dagger" + UnityEngine.Random.Range(1, 5)];
			this.EquipWeapon();
			this.hatSprite.color = Color.blue;
			this.weapon1Sprite.color = Color.blue;
			this.weapon2Sprite.color = Color.blue;
		}
		else
		{
			this.currentHat = ItemManager.instance.hatsById["jason"];
			this.EquipHat();
			this.currentWeapon = ItemManager.instance.weaponsById["dagger" + UnityEngine.Random.Range(1, 5)];
			this.EquipWeapon();
			this.hatSprite.color = Color.red;
			this.weapon1Sprite.color = Color.red;
			this.weapon2Sprite.color = Color.red;
		}
		this.markedForKill = false;
		if (SceneManager.instance.gameStarted && SceneManager.instance.isEndless)
		{
			this.hitMarkers.Initialize();
		}
		if (this.hitSides.Count > 2)
		{
			this.isLieutenant = true;
			this.hatGlow.SetActive(true);
		}
		else
		{
			this.isLieutenant = false;
		}
		if (!SceneManager.instance.isEndless)
		{
			this.hitMarkers.cGroup.alpha = 0f;
		}
		this.pausedSpeed = this.speed;
		this.spine.state.TimeScale = 1f;
		this.spine.skeleton.flipX = (this.side == 1);
		this.ChangeState(Npc.State.Running);
        if (StickmanCombat.Gameplay.GameFlowController.instance != null)
            SetCombatColor(isSpecial ? new Color(0.85f, 0.2f, 0.28f, 1f) : new Color(0.62f, 0.38f, 0.16f, 1f));
	}

    public void SetCombatColor(Color color)
    {
        defaultColor = color;
        meshRenderer.material.SetColor("_Color", color);
        spine.skeleton.SetColor(Color.white);
        hatSprite.color = color;
        weapon1Sprite.color = Color.white;
        weapon2Sprite.color = Color.white;
        weapon2hSprite.color = Color.white;
        fistweapon1Sprite.color = Color.white;
        fistweapon2Sprite.color = Color.white;
    }

	public void MarkForKill()
	{
		this.markedForKill = true;
		this.hitMarkers.InRange();
	}

	public void UnMarkForKill()
	{
		this.markedForKill = false;
		this.hitMarkers.OutOfRange();
	}

	public void Hit()
	{
        var flow = StickmanCombat.Gameplay.GameFlowController.instance;
        if (flow != null && flow.TryDodge(this)) return;
		this.hitDistance = 1f;
		SceneManager.instance.gameStarted = false;
		SceneManager.instance.targetCameraZ = -7f;
		SceneManager.instance.chaserObject.transform.SetParent(CharacterManager.instance.head.transform);
		SceneManager.instance.chaserObject.transform.localPosition = Vector3.zero;
		CombatCameraFollow component = Camera.main.GetComponent<CombatCameraFollow>();
		component.OverallOffset.y = -0.1f;
		this.manager.PauseAll();
		this.Continue();
		CharacterManager.instance.inputQueue.Clear();
		SceneManager.instance.inputStarted = false;
		this.spine.state.TimeScale = 1f;
		this.ChangeState(Npc.State.Hitting);
		CharacterManager.instance.ChangeState(CharacterManager.State.Falling);
		CharacterManager.instance.spine.skeleton.flipX = (this.side == -1);
		CharacterManager.instance.side = -this.side;
		this.CompleteHit();
	}

	private void CompleteHit()
	{
        int num = UnityEngine.Random.Range(0, 4);
		this.spine.state.SetAnimation(0, this.currentWeapon.category.id + "Fatality", false);
		AudioManager.instance.StartFatalitySound();
		ItemManager.instance.DropItems(new Vector2(0f, 4f));
        //CharacterManager.instance.spine.state.SetAnimation(0, this.currentWeapon.category.id + "FatalityGotHit", false);
        CharacterManager.instance.spine.state.TimeScale = 0.75f;
        CharacterManager.instance.spine.state.SetAnimation(0,"FatalityGotHit", false);
        num++;
		if (num > 3)
		{
			num = 0;
		}
		this.playerImpactVector = new Vector2((float)(this.currentWeapon.category.hitVectors[num].x * -(float)this.side), (float)this.currentWeapon.category.hitVectors[num].y);


      
    }

	public void Reset()
	{
		this.hitSides.Clear();
		this.hatGlow.SetActive(false);
		this.currentHat = ItemManager.instance.hatsById["hat0"];
		this.currentWeapon = ItemManager.instance.weaponsById["fist0"];
		this.EquipHat();
		this.EquipWeapon();
        baseTransform.localScale = new Vector3(1f, 1f, 1f);
		this.hitMarkers.transform.SetParent(base.transform);
        this.blood.Stop();
        this.bloodBurst.Stop();
		this.fatalityBlood1.Stop();
		this.fatalityBlood2.Stop();
		if (this.ragdollApplied)
		{
			this.ragdoll.SmoothMix(0f, 0f);
			this.ragdoll.Remove();
			this.ragdollApplied = false;
		}
		this.ChangeState(Npc.State.Deactive);
        //base.gameObject.SetActive(false);
        base.gameObject.transform.position = new Vector3(1000, 1000, 1000);
        this.manager.ResetNpc(this);
		this.isLieutenant = false;
		this.isSpecial = false;
		this.hitDistance = 0.4f;
	}

	public bool CanBeHit()
	{
		return this.currentState == Npc.State.Running || this.currentState == Npc.State.Hitting || this.currentState == Npc.State.Triggered;
	}

	public bool CanHit()
	{
		return this.currentState != Npc.State.Hitting && this.currentState != Npc.State.Dead && this.currentState != Npc.State.Deactive && this.currentState != Npc.State.Waiting && this.currentState != Npc.State.Falling && (float)(-(float)this.side) * CharacterManager.instance.transform.position.x - (float)this.side * -base.transform.position.x <= this.hitDistance;
	}

	public void WaitToGetHit()
	{
		if (SceneManager.instance.gameStarted)
		{
			this.hitSides.RemoveAt(0);
			if (this.hitSides.Count == 1 && !this.isLieutenant)
			{
				this.weapon1Sprite.color = Color.white;
				this.weapon2Sprite.color = Color.white;
				this.weapon2Sprite.color = Color.white;
				this.hatSprite.color = Color.white;
			}
		}
		this.ChangeState(Npc.State.Waiting);
	}

	public void GotHit(Vector2 hitVector)
	{
    
        if (this.isLieutenant)
		{
			StoryManager.instance.hitsTaken++;
		}
		if (this.hitSides.Count <= 0)
		{
			if (SceneManager.instance.gameStarted)
			{
				NpcManager.instance.killCount++;
				if (this.isLieutenant)
				{
					NpcManager.instance.lieutenantKillCount++;
				}
				else if (this.isSpecial)
				{
					if (SceneManager.instance.isEndless)
					{
						EndlessManager.instance.OnSpecialEnemyKilled();

					}
				}
				else if (SceneManager.instance.isEndless)
				{
					EndlessManager.instance.OnNormalEnemyKilled();
				}
			}
            AudioManager.instance.GotHit();
            this.Die(hitVector);
            if (StickmanCombat.Gameplay.GameFlowController.instance != null)
            {
                bool endless = StickmanCombat.Gameplay.GameFlowController.instance.Session != null
                    && StickmanCombat.Gameplay.GameFlowController.instance.Session.Mode == StickmanCombat.Gameplay.GameMode.Endless;
                StickmanCombat.Gameplay.GameFlowController.instance.EnemyDefeated(endless ? (this.isSpecial ? 2 : 1) : 0);
            }
		}
        NpcManager.instance.CheckAndDestroyExcessEnemy();
	}

	public void SilentDie()
	{
		this.dontReset = true;
		this.manager.RemoveNpc(this.side);
		this.ChangeState(Npc.State.Dead);
	}

	public void Die(Vector2 hitVector)
	{
        if (this.currentState != Npc.State.Falling)
		{
			if (this.currentWeapon != null && this.currentWeapon.category.twoHanded)
			{
				this.weapon2hSprite.sprite = null;
			}
			if (NpcManager.instance.bloodToggler.isOn)
			{
                ParticleSystem.VelocityOverLifetimeModule velocityModule;
                velocityModule = this.blood.velocityOverLifetime;
                velocityModule.x= (float)(-(float)this.side);;
                //this.blood.velocityOverLifetime.x = (float)(-(float)this.side);
                this.blood.Play();
				this.bloodBurst.Play();
               
            }
			this.ragdoll.Apply();
			this.ragdollApplied = true;
			this.ragdoll.RootRigidbody.velocity = UnityEngine.Random.Range(0.8f, 1.2f) * new Vector2((float)this.side * hitVector.x, hitVector.y);
			this.ChangeState(Npc.State.Falling);
			this.manager.RemoveNpc(this.side);
		}
	}

	public void StartJustBlood()
	{
		if (NpcManager.instance.bloodToggler.isOn)
		{
            //Debug.Log("播放血液特效");
            this.fatalityBlood1.Play();
			this.fatalityBlood2.Play();
			this.bloodBurst.Play();
		}
	}

	public void Fell()
	{
		this.blood.Stop();
		this.bloodBurst.Stop();
		this.ChangeState(Npc.State.Dead);
	}

	public void Pause()
	{
		this.speed = 0f;
		this.spine.state.TimeScale = 0f;
	}

	public void Continue()
	{
		this.speed = this.pausedSpeed;
		this.spine.state.TimeScale = 1f;
	}

	public void CanReset()
	{
		if (this.dontReset)
		{
			return;
		}
		if (Mathf.Abs(Camera.main.transform.position.x - this.head.transform.position.x) > 10f)
		{
			this.Reset();
		}
		else if (Camera.main.transform.position.y - this.head.transform.position.y > 7f)
		{
			this.Reset();
		}
	}

	private void OnSpineEvent(TrackEntry entry, Spine.Event e)
	{
		if (e.data.name == "Speed")
		{
			if (e.floatValue == 1f)
			{
				CharacterManager.instance.GotHit(this.playerImpactVector, false);
				SceneManager.instance.chaserObject.transform.localPosition += new Vector3(this.playerImpactVector.x / 20f, this.playerImpactVector.y / 20f, 0f);
			}
		}
		else if (e.data.name == "Fatality" && entry.animation.Name.Contains("Fatality"))
		{
            if (e.floatValue == 1)
			{
				AudioManager.instance.EndFatalitySound();
				CharacterManager.instance.GotHit(this.playerImpactVector, false);
			}
			if (e.floatValue == 3)
			{
                this.spine.state.TimeScale = 0.5f;
                CharacterManager.instance.spine.state.TimeScale = 1f;
                ScreenshotManager.instance.TakeScreenShot(false);
				SceneManager.instance.GameOver(false);
				if (!SceneManager.instance.isEndless)
				{
					StoryManager.instance.GameOver();
				}
			}
		}
	}



	public void EquipWeapon()
	{

        Vector3 rightPos = Vector3.zero;
        float rightRotZ = 0;
        Vector3 rightScale = Vector3.zero;
        Vector3 leftPos = Vector3.zero;
        float leftRotZ = 0;
        Vector3 leftScale = Vector3.zero;

        var N = SimpleJSON.JSON.Parse(Resources.Load<TextAsset>("ItemPos").text);
        for (int i = 0; i < N["weaponCategories"].Count; i++)
        {
            if (this.currentWeapon.id.Contains(N["weaponCategories"][i]["id"].Value.ToLower()))
            {
                if (this.currentWeapon.EnglishName != "Unarmed")
                {
                    for (int j = 0; j < N["weaponCategories"][i]["weapons"].Count; j++)
                    {
                        if (this.currentWeapon.EnglishName == N["weaponCategories"][i]["weapons"][j]["EnglishName"].Value)
                        {

                            if (this.currentWeapon.id.Contains("fist") || this.currentWeapon.id.Contains("dagger"))
                            {
                                rightPos = new Vector3(N["weaponCategories"][i]["weapons"][j]["rightPos"]["x"].AsFloat, N["weaponCategories"][i]["weapons"][j]["rightPos"]["y"].AsFloat, 0f);
                                rightRotZ = N["weaponCategories"][i]["weapons"][j]["rightRotZ"].AsFloat;
                                rightScale = new Vector3(N["weaponCategories"][i]["weapons"][j]["rightLocalScale"]["x"].AsFloat, N["weaponCategories"][i]["weapons"][j]["rightLocalScale"]["y"].AsFloat, 1f);
                                leftPos = new Vector3(N["weaponCategories"][i]["weapons"][j]["leftPos"]["x"].AsFloat, N["weaponCategories"][i]["weapons"][j]["leftPos"]["y"].AsFloat, 0f);
                                leftRotZ = N["weaponCategories"][i]["weapons"][j]["leftRotZ"].AsFloat;
                                leftScale = new Vector3(N["weaponCategories"][i]["weapons"][j]["leftLocalScale"]["x"].AsFloat, N["weaponCategories"][i]["weapons"][j]["leftLocalScale"]["y"].AsFloat, 1f);
                            }
                            else
                            {
                                rightPos = new Vector3(N["weaponCategories"][i]["weapons"][j]["Pos"]["x"].AsFloat, N["weaponCategories"][i]["weapons"][j]["Pos"]["y"].AsFloat, 0f);
                                rightRotZ = N["weaponCategories"][i]["weapons"][j]["RotZ"].AsFloat;
                                rightScale = new Vector3(N["weaponCategories"][i]["weapons"][j]["LocalScale"]["x"].AsFloat, N["weaponCategories"][i]["weapons"][j]["LocalScale"]["y"].AsFloat, 1f);
                            }
                        }
                    }
                }
            }
        }

        this.weapon1Sprite.transform.parent.gameObject.SetActive(false);
		this.weapon2Sprite.transform.parent.gameObject.SetActive(false);
		this.weapon2hSprite.transform.parent.gameObject.SetActive(false);
        this.fistweapon1Sprite.transform.parent.gameObject.SetActive(false);
        this.fistweapon2Sprite.transform.parent.gameObject.SetActive(false);
        if (!(this.currentWeapon.id == "fist0"))
		{
			if (this.currentWeapon.category.twoHanded)
			{
               
                    this.weapon2hSprite.transform.parent.gameObject.SetActive(true);
                    this.weapon2hSprite.sprite = this.currentWeapon.sprite;
                    this.weapon2hSprite.transform.localPosition = rightPos;
                    this.weapon2hSprite.transform.localEulerAngles = new Vector3(0, 0, rightRotZ);
                    this.weapon2hSprite.transform.localScale = rightScale;
                
            }
			else if (this.currentWeapon.category.dualWield)
			{
                if (this.currentWeapon.id.Contains("fist"))
                {
                    this.fistweapon1Sprite.transform.parent.gameObject.SetActive(true);
                    this.fistweapon2Sprite.transform.parent.gameObject.SetActive(true);

                    this.fistweapon1Sprite.sprite = this.currentWeapon.sprite;
                    this.fistweapon2Sprite.sprite = this.currentWeapon.sprite;

                    this.fistweapon1Sprite.transform.localPosition = rightPos;
                    this.fistweapon1Sprite.transform.localEulerAngles = new Vector3(0, 0, rightRotZ);
                    this.fistweapon1Sprite.transform.localScale = rightScale;

                    this.fistweapon2Sprite.transform.localPosition = leftPos;
                    this.fistweapon2Sprite.transform.localEulerAngles = new Vector3(0, 0, leftRotZ);
                    this.fistweapon2Sprite.transform.localScale = leftScale;
                }
                else
                {
                    this.weapon1Sprite.transform.parent.gameObject.SetActive(true);
                    this.weapon2Sprite.transform.parent.gameObject.SetActive(true);

                    this.weapon1Sprite.sprite = this.currentWeapon.sprite;
                    this.weapon2Sprite.sprite = this.currentWeapon.sprite;

                    this.weapon1Sprite.transform.localPosition = rightPos;
                    this.weapon1Sprite.transform.localEulerAngles = new Vector3(0, 0, rightRotZ);
                    this.weapon1Sprite.transform.localScale = rightScale;

                    this.weapon2Sprite.transform.localPosition = leftPos;
                    this.weapon2Sprite.transform.localEulerAngles = new Vector3(0, 0, leftRotZ);
                    this.weapon2Sprite.transform.localScale = leftScale;
                }

            }
			else
			{
				this.weapon1Sprite.transform.parent.gameObject.SetActive(true);
				this.weapon1Sprite.sprite = this.currentWeapon.sprite;
                this.weapon1Sprite.transform.localPosition = rightPos;
                this.weapon1Sprite.transform.localEulerAngles = new Vector3(0, 0, rightRotZ);
                this.weapon1Sprite.transform.localScale = rightScale;
            }
		}
	}

	public void EquipHat()
	{


        if (this.currentHat.id == "hat0")
		{
            this.hatSprite.transform.parent.gameObject.SetActive(false);
        }
		else
		{
            this.hatSprite.transform.parent.gameObject.SetActive(true);
            this.hatSprite.sprite = this.currentHat.sprite;
		}
        var N = SimpleJSON.JSON.Parse(Resources.Load<TextAsset>("ItemPos").text);
        for (int i = 0; i < N["hats"].Count; i++)
        {
            if (this.currentHat.id == N["hats"][i]["id"].Value)
            {
                Vector3 localPos = new Vector3(N["hats"][i]["Pos"]["x"].AsFloat, N["hats"][i]["Pos"]["y"].AsFloat, 0f);
                float rotationZ = N["hats"][i]["RotZ"].AsFloat;
                Vector3 localScale = new Vector3(N["hats"][i]["LocalScale"]["x"].AsFloat, N["hats"][i]["LocalScale"]["y"].AsFloat, 1f);
                this.hatSprite.transform.localScale = localScale;
                this.hatSprite.transform.localPosition = localPos;
                this.hatSprite.transform.localEulerAngles = new Vector3(0, 0, rotationZ);
               
               
            }
        }

    }
}
