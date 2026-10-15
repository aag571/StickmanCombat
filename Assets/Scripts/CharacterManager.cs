// 反编译自程序集： Assembly-CSharp.dll
using Spine;
using Spine.Unity;
using Spine.Unity.Modules;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
	public enum State
	{
		Deactive,
		Idle,
		Running,
		Recovering,
		Falling
	}

	public SkeletonAnimation spine;

	public NpcManager npcManager;

	public static CharacterManager instance;

	public Transform npcChaseTarget;

	public Transform chaseTarget;

	public CombatCameraFollow proCam;

	public bool isZooming;

	public SkeletonRagdoll ragdoll;

	public GameObject head;

	public float cameraZoomOffset;

	public ParticleSystem blood1;

	public ParticleSystem blood2;

	private double lastZoomTime;

	public CharacterManager.State currentState;

	private const double zoomDuration = 1.0;

	public const float runTime = 0.1f;

	public float baseRange = 3f;

	private const float comboDuration = 3f;

	private const float comboSize = 4f;

	private const float baseRecoveryTime = 0.2f;

	private int comboIndex;

	private double runStartTime;

	private double recoveryStartTime;

	private Vector2 runStartPlayerPos;

	public Vector2 runTargetPos;

	private Npc markedNpc;

	public int side = 1;

	private bool willHit;
    private bool autoDodging;

	public Queue<int> inputQueue = new Queue<int>();

	private void Awake()
	{
		CharacterManager.instance = this;
        
    }

	protected void OnEnable()
	{
		InputManager.OnAttackPressed += new InputManager.AttackAction(this.OnAttackPressed);
       
	}

	protected void OnDisable()
	{
		InputManager.OnAttackPressed -= new InputManager.AttackAction(this.OnAttackPressed);
	}

	private void Start()
	{
		this.spine.state.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.OnSpineEvent);
		this.currentState = CharacterManager.State.Idle;

    }

	private void OnAttackPressed(int attackSide)
	{
        if (autoDodging) return;
		if (this.currentState == CharacterManager.State.Idle || this.currentState == CharacterManager.State.Running)
		{
			this.inputQueue.Enqueue(attackSide);
		}
	}

	private void Update()
	{
        if (autoDodging) return;
		if (SceneManager.instance.gameStarted && (SceneManager.instance.isEndless || StoryManager.instance.introCompleted) && this.isZooming && this.lastZoomTime + 1.0 < (double)Time.time)
		{
			this.isZooming = false;
		}
		switch (this.currentState)
		{
		case CharacterManager.State.Idle:
			this.ProcessInput();
			break;
		case CharacterManager.State.Running:
		{
			double num = (double)Time.time - this.runStartTime;
			Vector2 a = this.runTargetPos - this.runStartPlayerPos;
			float num2 = (float)(num / 0.10000000149011612);
			if (num2 >= 1f)
			{
				num2 = 1f;
			}
			Vector2 vector = this.runStartPlayerPos + a * num2;
			base.transform.position = new Vector3(vector.x, vector.y, base.transform.position.z);
			if (num2 == 1f)
			{
				if (this.willHit)
				{
					this.ChangeState(CharacterManager.State.Idle);
					if (!SceneManager.instance.isEndless && StoryManager.instance.introCompleted && StoryManager.instance.miniboss.hitSides.Count == 0)
					{
						return;
					}
					Vector3 v = ItemManager.instance.GetImpactVector(this.comboIndex);
					if (this.markedNpc != null)
					{
                        var flow = StickmanCombat.Gameplay.GameFlowController.instance;
                        bool defeated = this.markedNpc.hitSides.Count == 0;
                        this.markedNpc.GotHit(v);
                        if (flow != null) flow.ResolveWeaponHit(this.markedNpc, this.side, v, defeated);
					}
				}
				else
				{
					this.ChangeState(CharacterManager.State.Recovering);
				}
			}
			break;
		}
		case CharacterManager.State.Recovering:
			if ((double)Time.time - this.recoveryStartTime >= (double)this.GetRecoveryTime())
			{
				this.ChangeState(CharacterManager.State.Idle);
				this.spine.state.AddAnimation(0, ItemManager.instance.GetIdleString(), true, 0f);
			}
			break;
		}
	}

	public void ChangeState(CharacterManager.State newState)
	{
		this.spine.state.TimeScale = 1f;
		if (newState != CharacterManager.State.Idle)
		{
			if (newState != CharacterManager.State.Running)
			{
				if (newState == CharacterManager.State.Recovering)
				{
					ItemManager.instance.ToggleTrails(false);
					this.inputQueue.Clear();
					this.recoveryStartTime = (double)Time.time;
				}
			}
			else
			{
				if (this.markedNpc != null)
				{
					ItemManager.instance.ToggleTrails(true);
				}
				this.runStartTime = (double)Time.time;
				this.runStartPlayerPos = new Vector2(base.transform.position.x, base.transform.position.y);
			}
		}
		else
		{
			ItemManager.instance.ToggleTrails(false);
		}
		this.currentState = newState;
	}

	public void Activate()
	{
		this.ChangeState(CharacterManager.State.Idle);
	}

	private void ProcessInput()
	{
		if (this.inputQueue.Count < 1)
		{
			return;
		}
		this.side = this.inputQueue.Dequeue();
		this.spine.skeleton.flipX = (this.side == -1);
		this.StartAttack();
	}

	private float GetReach()
	{
		return StickmanCombat.Gameplay.WeaponCombatProfile.For(ItemManager.instance.currentWeapon).reach;
	}

	private float GetRange()
	{
		return this.baseRange;
	}

	private float GetRecoveryTime()
	{
		return StickmanCombat.Gameplay.WeaponCombatProfile.For(ItemManager.instance.currentWeapon).recovery;
	}

	public float GetThreatXPos(int side)
	{
		return base.transform.position.x + this.GetRange() * (float)side + this.GetReach() * (float)side;
	}

	private void StartAttack()
	{
        bool isCoin = false;
        bool isFist = ItemManager.instance.currentWeapon.category.id == "Fist";
        this.markedNpc = this.npcManager.PlayerAttackCheck(this.side, this.GetThreatXPos(this.side));
        if (StickmanCombat.Gameplay.GameFlowController.instance != null)
            StickmanCombat.Gameplay.GameFlowController.instance.PrepareWeaponHit(this.markedNpc, this.side, this.GetThreatXPos(this.side));
        this.spine.state.SetAnimation(0, ItemManager.instance.GetAttackString(this.comboIndex), false);
        
        this.comboIndex++;
        if ((float)this.comboIndex >= 4f)
        {
            this.comboIndex = 0;
        }
        if ((UnityEngine.Object)this.markedNpc == (UnityEngine.Object)null)
        {
            this.willHit = false;
            Vector3 position = base.transform.position;
            float x = position.x + (float)this.side * this.GetRange();
            Vector3 position2 = base.transform.position;
            this.runTargetPos = new Vector2(x, position2.y);
            if (MonetizationManager.instance.coinSpawned)
            {
                float num = 0f;
                float threatXPos = this.GetThreatXPos(this.side);
                if (MonetizationManager.instance.coinSide == 1)
                {
                    float num2 = threatXPos;
                    Vector3 position3 = MonetizationManager.instance.coinObject.transform.position;
                    num = num2 - position3.x;
                }
                else
                {
                    Vector3 position4 = MonetizationManager.instance.coinObject.transform.position;
                    num = position4.x - threatXPos;
                }
                if (num > 0f)
                {
                    isCoin = true;
                    this.willHit = true;
                    Vector3 position5 = MonetizationManager.instance.coinObject.transform.position;
                    float x2 = position5.x;
                    Vector3 position6 = base.transform.position;
                    this.runTargetPos = new Vector2(x2, position6.y);
                }
            }
        }
        else
        {
            this.willHit = true;
            Vector3 position7 = this.markedNpc.transform.position;
            float x3 = position7.x - this.GetReach() * (float)this.side;
            Vector3 position8 = this.markedNpc.transform.position;
            this.runTargetPos = new Vector2(x3, position8.y);
            this.markedNpc.WaitToGetHit();
            float num3 = this.runTargetPos.x * (float)this.side;
            Vector3 position9 = base.transform.position;
            if (num3 < position9.x * (float)this.side)
            {
                Vector3 position10 = base.transform.position;
                float x4 = position10.x;
                Vector3 position11 = this.markedNpc.transform.position;
                this.runTargetPos = new Vector2(x4, position11.y);
            }
        }
        bool isMiss = !this.willHit;
        if (this.willHit)
        {
            this.isZooming = true;
            this.lastZoomTime = (double)Time.time;
            this.spine.state.AddAnimation(0, ItemManager.instance.GetIdleString(), true, 0.5f);
        }
        if (this.willHit && !SceneManager.instance.isEndless && !StoryManager.instance.levelCompleted && StoryManager.instance.introCompleted && StoryManager.instance.miniboss.hitSides.Count == 0)
        {
            isMiss = true;
            StoryManager.instance.StartFatality();
        }
        if (!this.willHit && !SceneManager.instance.isEndless && !StoryManager.instance.levelCompleted && StoryManager.instance.introCompleted && StoryManager.instance.miniboss.markedForKill)
        {
            if (!SceneManager.instance.isEndless)
            {
                StoryManager.instance.miniboss.Hit();
            }
            else if (NpcManager.instance.leftNpcs.Count > 0)
            {
                NpcManager.instance.leftNpcs.First.Value.Hit();
            }
            else
            {
                NpcManager.instance.rightNpcs.First.Value.Hit();
            }
            this.willHit = true;
        }
        if (this.willHit && (UnityEngine.Object)this.markedNpc != (UnityEngine.Object)null && this.markedNpc.hitSides.Count > 0)
        {
            isMiss = true;
        }
        AudioManager.instance.HitSound(isFist, isMiss, isCoin, ItemManager.instance.currentWeapon.category.id);
        this.ChangeState(State.Running);
    }

	public bool CanBeHit()
	{
		return !autoDodging && this.currentState != CharacterManager.State.Running;
	}

    public void BeginAutoDodge(int attackerSide)
    {
        if (autoDodging) return;
        StartCoroutine(AutoDodge(attackerSide));
    }

    private System.Collections.IEnumerator AutoDodge(int attackerSide)
    {
        autoDodging = true;
        inputQueue.Clear();
        markedNpc = null;
        willHit = false;
        ItemManager.instance.ToggleTrails(false);
        Vector3 start = transform.position;
        float distance = 2.5f;
        var retreatEnemies = attackerSide == 1 ? npcManager.leftNpcs : npcManager.rightNpcs;
        if (retreatEnemies.First != null)
            distance = Mathf.Min(distance, Mathf.Max(0f, Mathf.Abs(retreatEnemies.First.Value.transform.position.x - start.x) - 0.9f));
        Vector3 destination = start + Vector3.left * attackerSide * distance;
        spine.state.SetAnimation(0, "Block0", false);
        float elapsed = 0f;
        while (elapsed < 0.24f)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / 0.24f);
            transform.position = Vector3.Lerp(start, destination, Mathf.SmoothStep(0f, 1f, progress)) + Vector3.up * Mathf.Sin(progress * Mathf.PI) * 0.65f;
            yield return null;
        }
        transform.position = destination;
        runTargetPos = destination;
        inputQueue.Clear();
        autoDodging = false;
        ChangeState(State.Idle);
        spine.state.SetAnimation(0, ItemManager.instance.GetIdleString(), true);
    }

	public void Reset()
	{
		if (this.currentState == CharacterManager.State.Falling)
		{
			this.blood1.Stop();
			this.blood2.Stop();
			this.ChangeState(CharacterManager.State.Idle);
			CharacterManager.instance.GetComponent<MeshRenderer>().enabled = true;
			this.ragdoll.SmoothMix(0f, 0f);
			try
			{
				this.ragdoll.Remove();
			}
			catch
			{
			}
			base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, -1.22f);
			ItemManager.instance.RecoverItems();
			this.spine.state.SetAnimation(0, ItemManager.instance.GetAttackString(UnityEngine.Random.Range(0, 4)), false);
			this.spine.state.AddAnimation(0, ItemManager.instance.GetIdleString(), true, 0f);
			ItemManager.instance.weaponSpriteRenderers[0].transform.localScale = new Vector2(Mathf.Abs(ItemManager.instance.weaponSpriteRenderers[0].transform.localScale.x), Mathf.Abs(ItemManager.instance.weaponSpriteRenderers[0].transform.localScale.y));
			ItemManager.instance.weaponSpriteRenderers[1].transform.localScale = new Vector2(Mathf.Abs(ItemManager.instance.weaponSpriteRenderers[1].transform.localScale.x), Mathf.Abs(ItemManager.instance.weaponSpriteRenderers[1].transform.localScale.y));
			ItemManager.instance.weaponSpriteRenderers[2].transform.localScale = new Vector2(Mathf.Abs(ItemManager.instance.weaponSpriteRenderers[2].transform.localScale.x), Mathf.Abs(ItemManager.instance.weaponSpriteRenderers[2].transform.localScale.y));
		}
		ItemManager.instance.UpdateBelt();
	}

	public void GotHit(Vector2 iVector, bool applyRagdoll = true)
	{
        if (autoDodging) return;
		if (NpcManager.instance.bloodToggler.isOn)
		{
			this.blood1.Play();
			this.blood2.Play();
		}
		StatManager.instance.stats[7].UpdateStat(StatManager.instance.stats[7].value + 1);
		if (applyRagdoll)
		{
			this.ragdoll.Apply();
			this.ragdoll.RootRigidbody.velocity = iVector;
			ItemManager.instance.DropItems(this.ragdoll.RootRigidbody.velocity / 4f);
		}
		this.ChangeState(CharacterManager.State.Falling);
		SceneManager.instance.Shake();
	}

	private void OnSpineEvent(TrackEntry entry, Spine.Event e)
	{
        if (e.data.name == "Speed" && e.floatValue > 0f)
		{
			this.spine.state.TimeScale = e.floatValue;
		}
		if (e.data.name == "Fatality" && entry.animation.Name.Contains("Fatality"))
		{
			if (e.floatValue == 1)
			{
                //Debug.Log("终结技结束");
				StoryManager.instance.EndFatality();
				SceneManager.instance.ShakePreset();
			}
			else if (e.floatValue == 2)
			{
				ItemManager.instance.ToggleTrails(true);
			}
			else
			{
				StoryManager.instance.AfterFatality();
			}
		}
	}
}
