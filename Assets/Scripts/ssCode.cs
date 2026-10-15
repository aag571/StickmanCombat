// 反编译自程序集： Assembly-CSharp.dll
using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ssCode : MonoBehaviour
{
	public AmbientLight ambi;

	public GameObject chaser;

	public MountainGenerator m;

	public MountainGenerator m2;

	public TestMovement mover;

	public GameObject[] mode1Objects;

	public GameObject[] mode2Objects;

	public GameObject itemPrefab;

	public Image imagePrefab;

	public Collider groundCollider;

	public int ssNo;

	public SkeletonAnimation spine;

	private void Start()
	{
		this.ssNo = 0;
		base.Invoke("StartNow", 3f);
	}

	private void StartNow()
	{
		this.mover.enabled = false;
		Behaviour arg_21_0 = this.m;
		bool enabled = false;
		this.m2.enabled = enabled;
		arg_21_0.enabled = enabled;
		this.ambi.t = 0f;
		this.chaser.transform.localPosition = new Vector3(0f, -0.6f, 0f);
		this.StorySS();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F12))
		{
			ScreenCapture.CaptureScreenshot("Screenshot" + this.ssNo + ".png");
			this.ssNo++;
		}
		if (!this.groundCollider.enabled)
		{
			this.groundCollider.enabled = true;
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(0);
		}
		if (Input.GetKeyDown(KeyCode.K))
		{
			NpcManager.instance.leftNpcs.First.Value.Die(new Vector2((float)UnityEngine.Random.Range(-10, 10), (float)UnityEngine.Random.Range(10, 20)));
			NpcManager.instance.rightNpcs.First.Value.Die(new Vector2((float)UnityEngine.Random.Range(-10, 10), (float)UnityEngine.Random.Range(10, 20)));
		}
		if (Input.GetKeyDown(KeyCode.Return))
		{
			if (Time.timeScale == 1f)
			{
				Time.timeScale = 0f;
			}
			else
			{
				Time.timeScale = 1f;
			}
		}
		float num;
		if (Input.GetKey(KeyCode.LeftShift))
		{
			num = 4f;
		}
		else
		{
			num = 1f;
		}
		this.ambi.speed = 0f;
		if (Input.GetKeyDown(KeyCode.L))
		{
			this.ambi.t += 0.1f;
		}
		if (Input.GetKey(KeyCode.A))
		{
			this.m.transform.position -= new Vector3(Time.unscaledDeltaTime * num, 0f, 0f);
		}
		if (Input.GetKey(KeyCode.D))
		{
			this.m.transform.position += new Vector3(Time.unscaledDeltaTime * num, 0f, 0f);
		}
		if (Input.GetKey(KeyCode.W))
		{
			this.m.transform.position += new Vector3(0f, Time.unscaledDeltaTime * num, 0f);
		}
		if (Input.GetKey(KeyCode.S))
		{
			this.m.transform.position -= new Vector3(0f, Time.unscaledDeltaTime * num, 0f);
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			this.m2.transform.position -= new Vector3(Time.unscaledDeltaTime * num, 0f, 0f);
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			this.m2.transform.position += new Vector3(Time.unscaledDeltaTime * num, 0f, 0f);
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			this.m2.transform.position += new Vector3(0f, Time.unscaledDeltaTime * num, 0f);
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			this.m2.transform.position -= new Vector3(0f, Time.unscaledDeltaTime * num, 0f);
		}
		if (Input.GetKeyDown(KeyCode.F1))
		{
			GameObject[] array = this.mode2Objects;
			for (int i = 0; i < array.Length; i++)
			{
				GameObject gameObject = array[i];
				gameObject.SetActive(true);
			}
			GameObject[] array2 = this.mode1Objects;
			for (int j = 0; j < array2.Length; j++)
			{
				GameObject gameObject2 = array2[j];
				gameObject2.SetActive(false);
			}
		}
		if (Input.GetKeyDown(KeyCode.F2))
		{
			GameObject[] array3 = this.mode1Objects;
			for (int k = 0; k < array3.Length; k++)
			{
				GameObject gameObject3 = array3[k];
				gameObject3.SetActive(true);
			}
			GameObject[] array4 = this.mode2Objects;
			for (int l = 0; l < array4.Length; l++)
			{
				GameObject gameObject4 = array4[l];
				gameObject4.SetActive(false);
			}
		}
		if (Input.GetKeyDown(KeyCode.F3))
		{
			GameObject[] array5 = this.mode1Objects;
			for (int m = 0; m < array5.Length; m++)
			{
				GameObject gameObject5 = array5[m];
				gameObject5.SetActive(true);
			}
			GameObject[] array6 = this.mode2Objects;
			for (int n = 0; n < array6.Length; n++)
			{
				GameObject gameObject6 = array6[n];
				gameObject6.SetActive(true);
			}
		}
		if (Input.GetKey(KeyCode.Z))
		{
			Camera.main.transform.position += new Vector3(0f, 0f, Time.deltaTime * num);
		}
		if (Input.GetKey(KeyCode.X))
		{
			Camera.main.transform.position -= new Vector3(0f, 0f, Time.deltaTime * num);
		}
	}

	private void ItemsSS()
	{
		List<Sprite> list = new List<Sprite>();
		List<Sprite> list2 = new List<Sprite>();
		foreach (KeyValuePair<string, Hat> current in ItemManager.instance.hatsById)
		{
			if (current.Key != "hat0")
			{
				list.Add(current.Value.sprite);
			}
		}
		foreach (KeyValuePair<string, Weapon> current2 in ItemManager.instance.weaponsById)
		{
			if (current2.Key != "fist0")
			{
				list2.Add(current2.Value.sprite);
			}
		}
		bool flag = true;
		List<Sprite> list3 = new List<Sprite>();
		while (list2.Count > 0 && list.Count > 0)
		{
			flag = !flag;
			if (flag)
			{
				list3 = list;
			}
			else
			{
				list3 = list2;
			}
			int index = UnityEngine.Random.Range(0, list3.Count);
			this.imagePrefab.sprite = list3[index];
			list3.RemoveAt(index);
			this.imagePrefab.SetNativeSize();
			this.imagePrefab.rectTransform.sizeDelta = this.imagePrefab.rectTransform.sizeDelta / 4f;
			UnityEngine.Object.Instantiate<GameObject>(this.itemPrefab, this.itemPrefab.transform.parent).SetActive(true);
		}
	}

	private void StorySS()
	{
		this.chaser.transform.SetParent(null);
		CharacterManager.instance.transform.localPosition = new Vector3(-14f, CharacterManager.instance.transform.localPosition.y, CharacterManager.instance.transform.localPosition.z);
	}
}
