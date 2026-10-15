// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;

public class FallTest : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	protected void OnCollisionEnter(Collision collisionInfo)
	{
		if (collisionInfo.transform.parent != null && collisionInfo.transform.parent.gameObject.tag == "Npc")
		{
            collisionInfo.transform.parent.gameObject.GetComponent<Npc>().Fell();
            collisionInfo.transform.parent.SetParent(base.transform);
        }
	}
  
}
