// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;

public class BackgroundLayer : MonoBehaviour
{
	public GameObject mover;

	public Vector3 offset;

	public Vector3 camOffset;

	public float originalY;

	public float parallaxFactor;

	public bool zoomEffectOn;

	public bool bgTrees;

	public float zoomOffset;

	public Vector3 startingOffset;

	private void Start()
	{
		this.zoomOffset = Camera.main.transform.position.z;
		this.camOffset = this.mover.transform.position;
		this.offset = base.transform.localPosition;
		this.originalY = this.camOffset.y;
	}

	private void LateUpdate()
	{
		//if (this.bgTrees)
		//{
		//	float num = this.originalY - (Camera.main.transform.position.z + 9f) / 2f;
		//	num = Mathf.Min(num, this.originalY);
		//	this.camOffset = new Vector3(this.camOffset.x, num, this.camOffset.z);
		//}
		//base.transform.localPosition = this.startingOffset + this.offset - this.parallaxFactor * (this.mover.transform.position - this.camOffset);
		//if (this.zoomEffectOn)
		//{
		//	float num2 = Camera.main.transform.position.z - this.zoomOffset;
		//	base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, this.offset.z - num2 * ((this.parallaxFactor + 0.1f) * (this.parallaxFactor + 0.1f)) * 20f);
		//}
	}
}
