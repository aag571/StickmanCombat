// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
	public Tutorial[] tutorials;

	public Tutorial activeTutorial;

	public static TutorialManager instance;

	public int tutorialIndex;

	public Text tapLeft;

	public Text tapRight;

	private void Awake()
	{
		TutorialManager.instance = this;
	}

	private void Start()
	{
        if (StickmanCombat.Gameplay.GameFlowController.instance != null) { tutorialIndex = 5; return; }
        this.tutorialIndex = PlayerPrefs.GetInt("tutorialIndex", 0);
       
    }

	private void Update()
	{
        if (StickmanCombat.Gameplay.GameFlowController.instance != null) return;
		int num = this.tutorials.Length;
		if (this.tutorialIndex < 4 && SceneManager.instance.inputStarted)
		{
			SceneManager.instance.inputStarted = false;
		}
		for (int i = 0; i < num; i++)
		{
			Helpers.UpdateCanvasVisibilty(this.tutorials[i].canvasGroup, 3f, this.tutorials[i] == this.activeTutorial && SceneManager.instance.gameStarted, false);
		}
	}

	public void ActivateTutorial(int tutorialId)
	{
		this.tutorials[tutorialId].Initialize();
		this.activeTutorial = this.tutorials[tutorialId];
	}

	public void CloseTutorial()
	{
		this.activeTutorial = null;
	}

	public void IncreaseTutorialStep()
	{
		if (this.activeTutorial.stateNo == 0)
		{
			this.activeTutorial.ChangeState(1);
		}
	}

	public void SetTapState(int tapState, string str = "")
	{
		if (tapState == 0)
		{
			this.tapRight.gameObject.SetActive(false);
			this.tapLeft.gameObject.SetActive(false);
		}
		else if (tapState == 1)
		{
			this.tapRight.gameObject.SetActive(true);
			this.tapLeft.gameObject.SetActive(false);
			if (str != string.Empty && this.tapRight.text != str)
			{
				this.tapRight.text = str;
			}
		}
		else if (tapState == -1)
		{
			this.tapRight.gameObject.SetActive(false);
			this.tapLeft.gameObject.SetActive(true);
			if (str != string.Empty && this.tapLeft.text != str)
			{
				this.tapLeft.text = str;
			}
		}
	}

	public void TutorialCompleted()
	{
		AnalyticsManager.instance.TutorialPassed(this.tutorialIndex);
		this.tutorialIndex++;
		if (this.tutorialIndex == 1)
		{
			this.tutorialIndex++;
		}
        PlayerPrefs.SetInt("tutorialIndex", this.tutorialIndex);
	}
}
