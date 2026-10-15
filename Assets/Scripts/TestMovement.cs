// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
	public float speed = 1f;

	public bool rotate;

	public bool move;

	public double nextRotationTime;

	public float targetRotation;

	public float rotationSpeed = 1f;

	private void Start()
	{
		this.nextRotationTime = (double)(Time.time + 5f);
	}

	private void LateUpdate()
	{
		if (this.move)
		{
			base.transform.position += new Vector3(Time.deltaTime * 10f * this.speed, 0f, 0f);
		}
		if (this.rotate)
		{
			if (this.nextRotationTime < (double)Time.time)
			{
				this.rotationSpeed = UnityEngine.Random.Range(1f, 2f);
				this.nextRotationTime = (double)(Time.time + (float)UnityEngine.Random.Range(8, 20));
				int num = -1;
				if (this.targetRotation < 0f)
				{
					num = 1;
				}
				this.targetRotation = (float)UnityEngine.Random.Range(0, num * 10);
			}
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, Quaternion.Euler(new Vector3(0f, 0f, this.targetRotation)), Time.deltaTime * this.rotationSpeed / 10f);
		}
	}
}
