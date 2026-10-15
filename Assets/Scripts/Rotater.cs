// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;

public class Rotater : MonoBehaviour
{
	public Vector3 speed;

	private void Update()
	{
		base.transform.rotation = Quaternion.Euler(new Vector3(base.transform.rotation.eulerAngles.x + Time.deltaTime * this.speed.x, base.transform.rotation.eulerAngles.y + Time.deltaTime * this.speed.y, base.transform.rotation.eulerAngles.z + Time.deltaTime * this.speed.z));
	}
}
