// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;

public class KaskolPiece : MonoBehaviour
{
	public GameObject target;

	public float factor;

	public float length;

	public float globalFactor;

	private void Start()
	{
	}

	private void Update()
	{
		float num = this.factor * this.globalFactor;
		base.transform.position = Vector3.Lerp(base.transform.position, this.target.transform.position - this.length * new Vector3(1f - num / 3f + (-1f + Mathf.PerlinNoise(base.transform.position.x, base.transform.position.y)) * num * Mathf.Abs(Mathf.Cos(Time.time)) / 30f, -Mathf.Abs(Mathf.Sin(Time.time) / 10f)), Time.deltaTime * 10f);
	}
}
