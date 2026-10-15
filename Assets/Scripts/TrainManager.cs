// 反编译自程序集： Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class TrainManager : MonoBehaviour
{
	public static TrainManager instance;

	private const float vagonDistance = 5.1f;

	public GameObject trainHead;

	public GameObject[] trainVagonObjects;

	public List<Vagon> vagons = new List<Vagon>();

	public GameObject centerVagonObject;

	public GameObject frontVagonObject;

	public GameObject playerObject;

	private Vagon leftMostVagon;

	private Vagon rightMostVagon;

	private Vagon centerVagon;

	public void Awake()
	{
		TrainManager.instance = this;
		int num = 0;
		this.centerVagon = new Vagon(this.centerVagonObject);
		this.vagons.Add(this.centerVagon);
		Vagon vagon = this.centerVagon;
		GameObject[] array = this.trainVagonObjects;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObj = array[i];
			Vagon vagon2 = new Vagon(gameObj);
			this.vagons.Add(vagon2);
			if (num < 3)
			{
				vagon.previous = vagon2;
				vagon2.next = vagon;
				vagon2.go.transform.position = new Vector3(vagon.go.transform.position.x - 5.1f, vagon2.go.transform.position.y, vagon2.go.transform.position.z);
			}
			else
			{
				vagon.next = vagon2;
				vagon2.previous = vagon;
				vagon2.go.transform.position = new Vector3(vagon.go.transform.position.x + 5.1f, vagon2.go.transform.position.y, vagon2.go.transform.position.z);
			}
			vagon = vagon2;
			num++;
			if (num == 3)
			{
				this.leftMostVagon = vagon2;
				vagon = this.centerVagon;
			}
			else if (num == 6)
			{
				this.rightMostVagon = vagon2;
			}
		}
	}

	public void Update()
	{
		float num = this.centerVagon.go.transform.position.x - 2.55f - this.playerObject.transform.position.x;
		if (num > 2.55f)
		{
			this.SetCenterVagon(-1, Mathf.Abs(num));
		}
		else if (num < -2.55f)
		{
			this.SetCenterVagon(1, Mathf.Abs(num));
		}
		this.CheckVagonPositions();
	}

	public void SetCenterVagon(int side, float distance)
	{
		Vagon vagon = this.centerVagon;
		if (side == -1)
		{
			while (Mathf.Abs(vagon.go.transform.position.x - 2.55f - this.playerObject.transform.position.x) >= distance)
			{
				if (vagon.previous == null)
				{
					break;
				}
				vagon = vagon.previous;
			}
		}
		else if (side == 1)
		{
			while (Mathf.Abs(vagon.go.transform.position.x - 2.55f - this.playerObject.transform.position.x) >= distance)
			{
				if (vagon.next == null)
				{
					break;
				}
				vagon = vagon.next;
			}
		}
		this.centerVagon = vagon;
	}

	public void CheckVagonPositions()
	{
		if (this.leftMostVagon == this.centerVagon || this.rightMostVagon == this.centerVagon)
		{
		}
		if (this.leftMostVagon.go.transform.position.x - 2.55f - Camera.main.transform.position.x < -20.05f)
		{
			Vagon vagon = new Vagon(this.leftMostVagon.go);
			vagon.previous = this.rightMostVagon;
			this.rightMostVagon.next = vagon;
			this.rightMostVagon = vagon;
			this.leftMostVagon = this.leftMostVagon.next;
			this.leftMostVagon.previous = null;
			this.rightMostVagon.go.transform.position = this.rightMostVagon.previous.go.transform.position + new Vector3(5.1f, 0f, 0f);
		}
		else if (this.rightMostVagon.go.transform.position.x - 2.55f - Camera.main.transform.position.x > 17.23f)
		{
			Vagon vagon2 = new Vagon(this.rightMostVagon.go);
			vagon2.next = this.leftMostVagon;
			this.leftMostVagon.previous = vagon2;
			this.leftMostVagon = vagon2;
			this.rightMostVagon = this.rightMostVagon.previous;
			this.rightMostVagon.next = null;
			this.leftMostVagon.go.transform.position = this.leftMostVagon.next.go.transform.position - new Vector3(5.1f, 0f, 0f);
		}
	}

	public void PrintVagons()
	{
		Vagon next = this.leftMostVagon;
		string str = string.Empty;
		while (next != null)
		{
			str = str + next.go.name + "-";
			next = next.next;
		}
		UnityEngine.Debug.Log(str + "(! " + this.centerVagon.go.name);
	}
}
