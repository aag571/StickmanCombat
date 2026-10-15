// 反编译自程序集： Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class KaskolMain : MonoBehaviour
{
	public List<GameObject> pieces;

	public LineRenderer lineRenderer;

	public GameObject target;

	private void Start()
	{
	}

	private void Update()
	{
		this.lineRenderer.SetPosition(0, this.target.transform.position);
		for (int i = 0; i < this.pieces.Count; i++)
		{
			this.lineRenderer.SetPosition(i + 1, this.pieces[i].transform.position);
		}
	}
}
