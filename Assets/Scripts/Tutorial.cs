// 反编译自程序集： Assembly-CSharp.dll
using Spine.Unity;
using System;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
	public int id;

	public SkeletonGraphic playerSpine;

	public SkeletonGraphic enemySpine;

	public int stateNo;

	public int hitStateNo;

	public double stateStartTime;

	public double hitStateChangeTime;

	public GameObject step1;

	public GameObject step2;

	public CanvasGroup canvasGroup;

	public Npc npc;

	private void Start()
	{
	}

	public void Initialize()
	{
		this.ChangeState(0);
		this.hitStateNo = 0;
		this.stateNo = 0;
	}

	private void Update()
	{
		if (Input.GetKeyDown("t"))
		{
			Time.timeScale = 0f;
		}
		switch (this.id)
		{
		case 0:
			if (this.npc == null && NpcManager.instance.rightNpcs.Count > 0)
			{
				this.npc = NpcManager.instance.rightNpcs.First.Value;
			}
			if (this.npc != null && this.npc.markedForKill)
			{
				if (this.stateNo == 0)
				{
					this.ChangeState(1);
					Time.timeScale = 0f;
                        string tempStr = "点击";

                    TutorialManager.instance.SetTapState(1, tempStr);
				}
				else if (this.stateNo == 1)
				{
                        Touch[] touches = Input.touches;
                        for (int i = 0; i < touches.Length; i++)
                        {
                            Touch touch = touches[i];
                            if (touch.phase == TouchPhase.Began && touch.position.x > (float)(Screen.width / 2))
                            {
                                Time.timeScale = 1f;
                                CharacterManager.instance.inputQueue.Enqueue(1);
                                this.ChangeState(2);
                                TutorialManager.instance.TutorialCompleted();
                                TutorialManager.instance.SetTapState(0, string.Empty);
                            }
                        }
						//if (Input.GetMouseButtonDown(0))
						//{
						//    Vector2 position = Input.mousePosition;
						//    if (position.x > (float)(Screen.width / 2))
						//    {
						//        Time.timeScale = 1f;
						//        CharacterManager.instance.inputQueue.Enqueue(1);
						//        this.ChangeState(2);
						//        TutorialManager.instance.TutorialCompleted();
						//        TutorialManager.instance.SetTapState(0, string.Empty);
						//    }
						//}
						if (Application.isEditor)
						{
							if (Input.GetKeyDown("space"))
							{
								Time.timeScale = 1f;
								CharacterManager.instance.inputQueue.Enqueue(1);
								this.ChangeState(2);
								TutorialManager.instance.TutorialCompleted();
								TutorialManager.instance.SetTapState(0, string.Empty);
							}
						}
					}
                }
			break;
		case 1:
			if (this.npc == null && NpcManager.instance.leftNpcs.Count > 0)
			{
				this.npc = NpcManager.instance.leftNpcs.First.Value;
			}
                if (this.npc != null && this.npc.markedForKill)
                {
                    if (this.stateNo == 0)
                    {
                        this.ChangeState(1);
                        Time.timeScale = 0f;
                        TutorialManager.instance.SetTapState(-1, "点击");
                    }
                    else if (this.stateNo == 1)
                    {
                        Touch[] touches2 = Input.touches;
                        for (int j = 0; j < touches2.Length; j++)
                        {
                            Touch touch2 = touches2[j];
                            if (touch2.phase == TouchPhase.Began && touch2.position.x < (float)(Screen.width / 2))
                            {
                                Time.timeScale = 1f;
                                CharacterManager.instance.inputQueue.Enqueue(-1);
                                this.ChangeState(2);
                                TutorialManager.instance.TutorialCompleted();
                                TutorialManager.instance.SetTapState(0, string.Empty);
                            }

                        }
						//if (Input.GetMouseButtonDown(0))
						//{
						//    Vector2 position = Input.mousePosition;
						//    if (position.x < (float)(Screen.width / 2))
						//    {
						//        Time.timeScale = 1f;
						//        CharacterManager.instance.inputQueue.Enqueue(-1);
						//        this.ChangeState(2);
						//        TutorialManager.instance.TutorialCompleted();
						//        TutorialManager.instance.SetTapState(0, string.Empty);
						//    }
						//}
						if (Application.isEditor)
						{
							if (Input.GetKeyDown("space"))
							{
								Time.timeScale = 1f;
								CharacterManager.instance.inputQueue.Enqueue(-1);
								this.ChangeState(2);
								TutorialManager.instance.TutorialCompleted();
								TutorialManager.instance.SetTapState(0, string.Empty);
							}
						}
					}
                }
			break;
		case 2:
		{
			if (this.npc == null && NpcManager.instance.leftNpcs.Count > 0)
			{
				this.npc = NpcManager.instance.leftNpcs.First.Value;
			}
			if (this.npc != null && this.npc.markedForKill)
			{
				if (this.hitStateNo == 0)
				{
					this.hitStateNo = 1;
					Time.timeScale = 0f;
					TutorialManager.instance.SetTapState(-1, "点击");
				}
				else if (this.hitStateNo == 1)
				{
                            Touch[] touches3 = Input.touches;
                            for (int k = 0; k < touches3.Length; k++)
                            {
                                Touch touch3 = touches3[k];
                                if (touch3.phase == TouchPhase.Began && touch3.position.x < (float)(Screen.width / 2))
                                {
                                    Time.timeScale = 1f;
                                    CharacterManager.instance.inputQueue.Enqueue(-1);
                                    this.hitStateNo = 2;
                                    this.hitStateChangeTime = (double)Time.time;
                                    TutorialManager.instance.SetTapState(0, string.Empty);
                                }

                            }
							//if (Input.GetMouseButtonDown(0))
							//{
							//    Vector2 position = Input.mousePosition;
							//    if (position.x < (float)(Screen.width / 2))
							//    {
							//        Time.timeScale = 1f;
							//        CharacterManager.instance.inputQueue.Enqueue(-1);
							//        this.hitStateNo = 2;
							//        this.hitStateChangeTime = (double)Time.time;
							//        TutorialManager.instance.SetTapState(0, string.Empty);
							//    }
							//}
							if (Application.isEditor)
							{
								if (Input.GetKeyDown("space"))
								{
									Time.timeScale = 1f;
									CharacterManager.instance.inputQueue.Enqueue(-1);
									this.hitStateNo = 2;
									this.hitStateChangeTime = (double)Time.time;
									TutorialManager.instance.SetTapState(0, string.Empty);
								}
							}
						}
				else if (this.hitStateNo == 2)
				{
					if ((double)Time.time > this.hitStateChangeTime + 0.40000000596046448)
					{
						this.hitStateNo = 3;
						Time.timeScale = 0f;
						TutorialManager.instance.SetTapState(-1, "再次点击");
					}
				}
				else if (this.hitStateNo == 3)
				{
                            Touch[] touches4 = Input.touches;
                            for (int l = 0; l < touches4.Length; l++)
                            {
                                Touch touch4 = touches4[l];
                                if (touch4.phase == TouchPhase.Began && touch4.position.x < (float)(Screen.width / 2))
                                {
                                    Time.timeScale = 1f;
                                    CharacterManager.instance.inputQueue.Enqueue(-1);
                                    TutorialManager.instance.TutorialCompleted();
                                    this.hitStateNo = 4;
                                    TutorialManager.instance.SetTapState(0, string.Empty);
                                }

                            }
							//if (Input.GetMouseButtonDown(0))
							//{
							//    Vector2 position = Input.mousePosition;
							//    if (position.x < (float)(Screen.width / 2))
							//    {
							//        Time.timeScale = 1f;
							//        CharacterManager.instance.inputQueue.Enqueue(-1);
							//        TutorialManager.instance.TutorialCompleted();
							//        this.hitStateNo = 4;
							//        TutorialManager.instance.SetTapState(0, string.Empty);
							//    }
							//}
							if (Application.isEditor)
							{
								if (Input.GetKeyDown("space"))
								{
									Time.timeScale = 1f;
									CharacterManager.instance.inputQueue.Enqueue(-1);
									TutorialManager.instance.TutorialCompleted();
									this.hitStateNo = 4;
									TutorialManager.instance.SetTapState(0, string.Empty);
								}
							}
						}
			}
			int num = this.stateNo;
			if (num != 0)
			{
				if (num == 1)
				{
					this.playerSpine.gameObject.transform.localPosition = Vector3.MoveTowards(this.playerSpine.gameObject.transform.localPosition, new Vector3(0f, this.playerSpine.gameObject.transform.localPosition.y, this.playerSpine.gameObject.transform.localPosition.z), Time.unscaledDeltaTime * 1000f);
					this.enemySpine.gameObject.transform.localPosition = Vector3.MoveTowards(this.enemySpine.gameObject.transform.localPosition, new Vector3(200f, this.enemySpine.gameObject.transform.localPosition.y, this.enemySpine.gameObject.transform.localPosition.z), Time.unscaledDeltaTime * 1000f);
					if (this.stateStartTime + 1.0 < (double)Time.unscaledTime)
					{
                                this.ChangeState(0);
					}
				}
			}
			else
			{
				this.enemySpine.gameObject.transform.localPosition = Vector3.MoveTowards(this.enemySpine.gameObject.transform.localPosition, new Vector3(100f, this.enemySpine.gameObject.transform.localPosition.y, this.enemySpine.gameObject.transform.localPosition.z), Time.unscaledDeltaTime * 150f);
				if (this.stateStartTime + 1.0 < (double)Time.unscaledTime)
				{
					this.ChangeState(1);
				}
			}
			break;
		}
		case 3:
		{
			if (this.npc == null && NpcManager.instance.rightNpcs.Count > 0)
			{
				this.npc = NpcManager.instance.rightNpcs.First.Value;
			}
			if (this.npc != null && this.npc.markedForKill)
			{
				if (this.hitStateNo == 0)
				{
					this.hitStateNo = 1;
					Time.timeScale = 0f;
					TutorialManager.instance.SetTapState(1, "点击");
				}
				else if (this.hitStateNo == 1)
				{
                            Touch[] touches5 = Input.touches;
                            for (int m = 0; m < touches5.Length; m++)
                            {
                                Touch touch5 = touches5[m];
                                if (touch5.phase == TouchPhase.Began && touch5.position.x > (float)(Screen.width / 2))
                                {
                                    Time.timeScale = 1f;
                                    CharacterManager.instance.inputQueue.Enqueue(1);
                                    this.hitStateNo = 2;
                                    this.hitStateChangeTime = (double)Time.time;
                                    TutorialManager.instance.SetTapState(0, string.Empty);
                                }

                            }
							//if (Input.GetMouseButtonDown(0))
							//{
							//    Vector2 position = Input.mousePosition;
							//    if (position.x > (float)(Screen.width / 2))
							//    {
							//        Time.timeScale = 1f;
							//        CharacterManager.instance.inputQueue.Enqueue(1);
							//        this.hitStateNo = 2;
							//        this.hitStateChangeTime = (double)Time.time;
							//        TutorialManager.instance.SetTapState(0, string.Empty);
							//    }
							//}
							if (Application.isEditor)
							{
								if (Input.GetKeyDown("space"))
								{
									Time.timeScale = 1f;
									CharacterManager.instance.inputQueue.Enqueue(1);
									this.hitStateNo = 2;
									this.hitStateChangeTime = (double)Time.time;
									TutorialManager.instance.SetTapState(0, string.Empty);
								}
							}
						}
				else if (this.hitStateNo == 2)
				{
					if ((double)Time.time > this.hitStateChangeTime + 0.40000000596046448)
					{
						this.hitStateNo = 3;
						Time.timeScale = 0f;
						TutorialManager.instance.SetTapState(-1, "点击");
					}
				}
				else if (this.hitStateNo == 3)
				{
                            Touch[] touches6 = Input.touches;
                            for (int n = 0; n < touches6.Length; n++)
                            {
                                Touch touch6 = touches6[n];
                                if (touch6.phase == TouchPhase.Began && touch6.position.x < (float)(Screen.width / 2))
                                {
                                    Time.timeScale = 1f;
                                    CharacterManager.instance.inputQueue.Enqueue(-1);
                                    TutorialManager.instance.TutorialCompleted();
                                    this.hitStateNo = 4;
                                    TutorialManager.instance.SetTapState(0, string.Empty);
                                    TutorialManager.instance.activeTutorial = null;
                                    SceneManager.instance.inputStarted = true;
                                    NpcManager.instance.gameStartTime = (double)(Time.time + 15f);
                                }

                            }
							//if (Input.GetMouseButtonDown(0))
							//{
							//    Time.timeScale = 1f;
							//    CharacterManager.instance.inputQueue.Enqueue(-1);
							//    TutorialManager.instance.TutorialCompleted();
							//    this.hitStateNo = 4;
							//    TutorialManager.instance.SetTapState(0, string.Empty);
							//    TutorialManager.instance.activeTutorial = null;
							//    SceneManager.instance.inputStarted = true;
							//    NpcManager.instance.gameStartTime = (double)(Time.time + 15f);
							//}
							if (Application.isEditor)
							{
								if (Input.GetKeyDown("space"))
								{
									Time.timeScale = 1f;
									CharacterManager.instance.inputQueue.Enqueue(-1);
									TutorialManager.instance.TutorialCompleted();
									this.hitStateNo = 4;
									TutorialManager.instance.SetTapState(0, string.Empty);
									TutorialManager.instance.activeTutorial = null;
									SceneManager.instance.inputStarted = true;
									NpcManager.instance.gameStartTime = (double)(Time.time + 15f);
								}
							}
						}
			}
			int num2 = this.stateNo;
			if (num2 != 0)
			{
				if (num2 == 1)
				{
					this.playerSpine.gameObject.transform.localPosition = Vector3.MoveTowards(this.playerSpine.gameObject.transform.localPosition, new Vector3(100f, this.playerSpine.gameObject.transform.localPosition.y, this.playerSpine.gameObject.transform.localPosition.z), Time.unscaledDeltaTime * 1000f);
					this.enemySpine.gameObject.transform.localPosition = Vector3.MoveTowards(this.enemySpine.gameObject.transform.localPosition, new Vector3(-100f, this.enemySpine.gameObject.transform.localPosition.y, this.enemySpine.gameObject.transform.localPosition.z), Time.unscaledDeltaTime * 1000f);
					if (this.stateStartTime + 1.0 < (double)Time.unscaledTime)
					{
                                
						this.ChangeState(0);
					}
				}
			}
			else
			{
				this.enemySpine.gameObject.transform.localPosition = Vector3.MoveTowards(this.enemySpine.gameObject.transform.localPosition, new Vector3(100f, this.enemySpine.gameObject.transform.localPosition.y, this.enemySpine.gameObject.transform.localPosition.z), Time.unscaledDeltaTime * 150f);
				if (this.stateStartTime + 1.0 < (double)Time.unscaledTime)
				{
					this.ChangeState(1);
				}
			}
			break;
		}
		}
	}

	public void ChangeState(int newState)
	{
		this.stateNo = newState;
		this.stateStartTime = (double)Time.unscaledTime;
		if (this.id > 1)
		{
			if (newState != 0)
			{
				if (newState == 1)
				{
					if (this.id == 3)
					{
						this.enemySpine.transform.localScale = new Vector3(1f, 1f, 1f);
					}
					this.playerSpine.AnimationState.SetAnimation(0, "FistAttack1", false);
					this.enemySpine.AnimationState.SetAnimation(0, "Block0", false);
                }
			}
			else
			{
				this.enemySpine.transform.localScale = new Vector3(-1f, 1f, 1f);
				this.playerSpine.gameObject.transform.localPosition = new Vector3(-100f, this.playerSpine.gameObject.transform.localPosition.y, this.playerSpine.gameObject.transform.localPosition.z);
				this.enemySpine.gameObject.transform.localPosition = new Vector3(250f, this.enemySpine.gameObject.transform.localPosition.y, this.enemySpine.gameObject.transform.localPosition.z);
				this.playerSpine.AnimationState.SetAnimation(0, "FistIdle", true);
				this.enemySpine.AnimationState.SetAnimation(0, "DaggerRun", true);
			}
		}
		else if (newState == 0)
		{
			this.step1.SetActive(true);
			this.step2.SetActive(false);
		}
		else if (newState == 1)
		{
			this.step1.SetActive(false);
			this.step2.SetActive(true);
		}
	}
}
