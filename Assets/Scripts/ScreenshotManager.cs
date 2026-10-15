// 反编译自程序集： Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScreenshotManager : MonoBehaviour
{
	

	public static ScreenshotManager instance;

	public RawImage ssThumbnail;

	public GameObject screenshotItems;

	public Texture2D screenshotTexture;

	public Text scoreText;

	public Text coinComboText;

	public Text beltText;

	private void Awake()
	{
		ScreenshotManager.instance = this;
	}

	private void Update()
	{
	}

	public void TakeScreenShot(bool beltWon = false)
	{
        if (StickmanCombat.Gameplay.GameFlowController.instance != null) return;
        //UnityEngine.Debug.Log("截取屏幕");
		base.StartCoroutine(this.CaptureScreenShot(beltWon));
	}

    IEnumerator CaptureScreenShot(bool beltwon = false)
    {
        this.screenshotItems.SetActive(true);
        if (beltwon)
        {
            this.beltText.text = string.Empty;
            this.scoreText.text = string.Empty;
            this.coinComboText.text = string.Empty;
        }
        else
        {
            if (StoryManager.instance.level == 0)
            {
                this.beltText.text = "无腰带";
            }
            else
            {
                this.beltText.text = ItemManager.instance.LevelToBeltString(StoryManager.instance.level).ToUpper() + "带";
            }
            this.scoreText.text = EndlessManager.instance.point.ToString();
            this.coinComboText.text = MonetizationManager.instance.highestCoinCombo.ToString();
            
        }
        StartCoroutine(captureScreenshot());
        string imageName = "Screenshots.png";

        yield return new WaitForEndOfFrame();

        byte[] data = File.ReadAllBytes(Application.persistentDataPath + "/" + imageName);

       
        screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.DXT1, false);

        screenshotTexture.LoadImage(data);
        ssThumbnail.texture = screenshotTexture;
        this.screenshotItems.SetActive(false);
    }

    IEnumerator captureScreenshot()
    {
        yield return new WaitForEndOfFrame();

        string path = Application.persistentDataPath + "/" + "Screenshots.png";

        Texture2D screenImage = new Texture2D(Screen.width, Screen.height);
        //Get Image from screen
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();
        //Convert to png
        byte[] imageBytes = screenImage.EncodeToPNG();

        //Save image to file
        System.IO.File.WriteAllBytes(path, imageBytes);
    }

   
}
