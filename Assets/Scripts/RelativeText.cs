// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;

public class RelativeText : MonoBehaviour
{
	public GameObject target;

	public Vector3 offset;

	public float followSpeed;

	public bool onlyY;

	public bool fullChase;

	public void Start()
	{
		base.transform.position = Camera.main.WorldToScreenPoint(this.target.transform.position) + this.offset;
	}

	private void LateUpdate()
	{
		if (this.target == null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else if (this.onlyY)
		{
			if (this.fullChase)
			{
				base.transform.position = new Vector3(base.transform.position.x, (Camera.main.WorldToScreenPoint(this.target.transform.position) + this.offset).y, base.transform.position.z);
			}
			else
			{
				base.transform.position = Vector3.Lerp(base.transform.position, new Vector3(base.transform.position.x, (Camera.main.WorldToScreenPoint(this.target.transform.position) + this.offset).y, base.transform.position.z), Time.fixedDeltaTime * this.followSpeed);
			}
		}
		else if (this.fullChase)
		{
			base.transform.position = Camera.main.WorldToScreenPoint(this.target.transform.position) + this.offset;
		}
		else
		{
			base.transform.position = Vector3.Lerp(base.transform.position, Camera.main.WorldToScreenPoint(this.target.transform.position) + this.offset, Time.fixedDeltaTime * this.followSpeed);
		}
	}
}
