// 反编译自程序集： Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRepeater : MonoBehaviour
{
	public List<GameObject> objects;

	public float minDist;

	public float maxDist;

	public float minYOffset;

	public float maxYOffset;

	private void Start()
	{
		foreach (GameObject current in this.objects)
		{
			current.transform.localPosition = new Vector3(-current.transform.parent.transform.position.x + UnityEngine.Random.Range(-this.minDist, this.maxDist), UnityEngine.Random.Range(this.minYOffset, this.maxYOffset), 0f);
		}
	}

	private void Update()
	{
		foreach (GameObject current in this.objects)
		{
			if (current.transform.position.x < Camera.main.transform.position.x - 15f)
			{
				current.transform.localPosition = new Vector3(Camera.main.transform.position.x - current.transform.parent.transform.position.x + UnityEngine.Random.Range(this.minDist, this.maxDist), UnityEngine.Random.Range(this.minYOffset, this.maxYOffset), 0f);
			}
		}
	}

	public void ResetPosition()
	{
		base.transform.position = new Vector3(0f, base.transform.position.y, base.transform.position.z);
		this.Start();
	}
}
