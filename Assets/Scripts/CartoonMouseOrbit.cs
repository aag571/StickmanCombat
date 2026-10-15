// 反编译自程序集： Assembly-UnityScript.dll
using System;
using UnityEngine;

[AddComponentMenu("相机控制/鼠标环绕")]
[Serializable]
public class CartoonMouseOrbit : MonoBehaviour
{
	public Transform target;

	public float distance;

	public float xSpeed;

	public float ySpeed;

	public int yMinLimit;

	public int yMaxLimit;

	private float x;

	private float y;

	public float dpiScale;

	public CartoonMouseOrbit()
	{
		this.distance = 10f;
		this.xSpeed = 250f;
		this.ySpeed = 120f;
		this.yMinLimit = -20;
		this.yMaxLimit = 80;
	}

	public void Start()
	{
		if (Screen.dpi < (float)1)
		{
			this.dpiScale = (float)1;
		}
		if (Screen.dpi < (float)200)
		{
			this.dpiScale = (float)1;
		}
		else
		{
			this.dpiScale = Screen.dpi / 200f;
		}
		Vector3 eulerAngles = this.transform.eulerAngles;
		this.x = eulerAngles.y;
		this.y = eulerAngles.x;
		if (this.GetComponent<Rigidbody>())
		{
			this.GetComponent<Rigidbody>().freezeRotation = true;
		}
	}

	public void LateUpdate()
	{
		if (this.target && (Input.GetMouseButton(0) || Input.GetMouseButton(1)))
		{
			Vector3 mousePosition = Input.mousePosition;
			if (mousePosition.x >= (float)300 * this.dpiScale || (float)Screen.height - mousePosition.y >= (float)200 * this.dpiScale)
			{
				Cursor.visible = false;
				this.x += Input.GetAxis("Mouse X") * this.xSpeed * 0.02f;
				this.y -= Input.GetAxis("Mouse Y") * this.ySpeed * 0.02f;
				this.y = CartoonMouseOrbit.ClampAngle(this.y, (float)this.yMinLimit, (float)this.yMaxLimit);
				Quaternion rotation = Quaternion.Euler(this.y, this.x, (float)0);
				Vector3 position = rotation * new Vector3((float)0, (float)0, -this.distance) + this.target.position;
				this.transform.rotation = rotation;
				this.transform.position = position;
			}
		}
		else
		{
			Cursor.visible = true;
		}
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < (float)(-360))
		{
			angle += (float)360;
		}
		if (angle > (float)360)
		{
			angle -= (float)360;
		}
		return Mathf.Clamp(angle, min, max);
	}

	public void Main()
	{
	}
}
