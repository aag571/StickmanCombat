// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;

public class UpAndDown : MonoBehaviour
{
	public float speed;

	public float minY;

	public float maxY;

	private void Start()
	{
	}

	private void Update()
	{
		float y = this.minY + Mathf.PingPong(Time.time * this.speed, this.maxY - this.minY);
		base.transform.localPosition = new Vector3(base.transform.localPosition.x, y, base.transform.localPosition.z);
	}
}
