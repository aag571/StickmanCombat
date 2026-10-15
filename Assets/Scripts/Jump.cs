// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;

public class Jump : MonoBehaviour
{
	public float y;

	public float randomTime;

	private void Awake()
	{
		this.randomTime = UnityEngine.Random.Range(0f, 10f);
		this.y = base.transform.localPosition.y;
	}

	private void Update()
	{
		base.transform.localPosition = new Vector3(base.transform.localPosition.x, this.y + Mathf.PingPong((this.randomTime + Time.time) / 15f, 0.015f), base.transform.localPosition.z);
	}
}
