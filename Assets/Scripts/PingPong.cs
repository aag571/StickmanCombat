// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;

public class PingPong : MonoBehaviour
{
	public float speed;

	public float minSize;

	public float maxSize;

	private void Start()
	{
	}

	private void Update()
	{
		float num = this.minSize + Mathf.PingPong(Time.unscaledTime * this.speed, this.maxSize - this.minSize);
		base.transform.localScale = new Vector3(num, num, num);
	}
}
