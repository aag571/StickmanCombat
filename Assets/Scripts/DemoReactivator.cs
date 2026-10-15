// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;

public class DemoReactivator : MonoBehaviour
{
	public float TimeDelayToReactivate = 3f;

	private void Start()
	{
		base.InvokeRepeating("Reactivate", this.TimeDelayToReactivate, this.TimeDelayToReactivate);
	}

	private void Reactivate()
	{
		base.gameObject.SetActive(false);
		base.gameObject.SetActive(true);
	}
}
