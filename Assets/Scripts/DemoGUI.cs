// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;

public class DemoGUI : MonoBehaviour
{
	public Texture HUETexture;

	public Material mat;

	public Position[] Positions;

	public GameObject[] Prefabs;

	private int currentNomber;

	private GameObject currentInstance;

	private GUIStyle guiStyleHeader = new GUIStyle();

	private float colorHUE;

	private float dpiScale;

	private void Start()
	{
		if (Screen.dpi < 1f)
		{
			this.dpiScale = 1f;
		}
		if (Screen.dpi < 200f)
		{
			this.dpiScale = 1f;
		}
		else
		{
			this.dpiScale = Screen.dpi / 200f;
		}
		this.guiStyleHeader.fontSize = (int)(15f * this.dpiScale);
		this.guiStyleHeader.normal.textColor = new Color(1f, 1f, 1f);
		this.currentInstance = UnityEngine.Object.Instantiate<GameObject>(this.Prefabs[this.currentNomber], base.transform.position, default(Quaternion));
	}

	private void OnGUI()
	{
		if (GUI.Button(new Rect(10f * this.dpiScale, 15f * this.dpiScale, 105f * this.dpiScale, 30f * this.dpiScale), "上一个特效"))
		{
			this.ChangeCurrent(-1);
		}
		if (GUI.Button(new Rect(130f * this.dpiScale, 15f * this.dpiScale, 105f * this.dpiScale, 30f * this.dpiScale), "下一个特效"))
		{
			this.ChangeCurrent(1);
		}
		GUI.Label(new Rect(300f * this.dpiScale, 15f * this.dpiScale, 100f * this.dpiScale, 20f * this.dpiScale), "预制体名称：\"" + this.Prefabs[this.currentNomber].name + "\"  \r\n按住任意鼠标按键移动相机", this.guiStyleHeader);
		GUI.DrawTexture(new Rect(12f * this.dpiScale, 80f * this.dpiScale, 220f * this.dpiScale, 15f * this.dpiScale), this.HUETexture, ScaleMode.StretchToFill, false, 0f);
		float num = this.colorHUE;
		this.colorHUE = GUI.HorizontalSlider(new Rect(12f * this.dpiScale, 105f * this.dpiScale, 220f * this.dpiScale, 15f * this.dpiScale), this.colorHUE, 0f, 1530f);
		if ((double)Mathf.Abs(num - this.colorHUE) > 0.001)
		{
			this.ChangeColor();
		}
		GUI.Label(new Rect(240f * this.dpiScale, 105f * this.dpiScale, 30f * this.dpiScale, 30f * this.dpiScale), "特效颜色", this.guiStyleHeader);
	}

	private void ChangeColor()
	{
		Color color = this.Hue(this.colorHUE / 255f);
		Renderer[] componentsInChildren = this.currentInstance.GetComponentsInChildren<Renderer>();
		Renderer[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			Renderer renderer = array[i];
			Material material = renderer.material;
			if (!(material == null) && material.HasProperty("_TintColor"))
			{
				color.a = material.GetColor("_TintColor").a;
				material.SetColor("_TintColor", color);
			}
		}
		Light componentInChildren = this.currentInstance.GetComponentInChildren<Light>();
		if (componentInChildren != null)
		{
			componentInChildren.color = color;
		}
	}

	private Color Hue(float H)
	{
		Color result = new Color(1f, 0f, 0f);
		if (H >= 0f && H < 1f)
		{
			result = new Color(1f, 0f, H);
		}
		if (H >= 1f && H < 2f)
		{
			result = new Color(2f - H, 0f, 1f);
		}
		if (H >= 2f && H < 3f)
		{
			result = new Color(0f, H - 2f, 1f);
		}
		if (H >= 3f && H < 4f)
		{
			result = new Color(0f, 1f, 4f - H);
		}
		if (H >= 4f && H < 5f)
		{
			result = new Color(H - 4f, 1f, 0f);
		}
		if (H >= 5f && H < 6f)
		{
			result = new Color(1f, 6f - H, 0f);
		}
		return result;
	}

	private void ChangeCurrent(int delta)
	{
		this.currentNomber += delta;
		if (this.currentNomber > this.Prefabs.Length - 1)
		{
			this.currentNomber = 0;
		}
		else if (this.currentNomber < 0)
		{
			this.currentNomber = this.Prefabs.Length - 1;
		}
		if (this.currentInstance != null)
		{
			UnityEngine.Object.Destroy(this.currentInstance);
		}
		Vector3 position = base.transform.position;
		if (this.Positions[this.currentNomber] == Position.Bottom)
		{
			position.y -= 1f;
		}
		if (this.Positions[this.currentNomber] == Position.Bottom02)
		{
			position.y -= 0.8f;
		}
		this.currentInstance = UnityEngine.Object.Instantiate<GameObject>(this.Prefabs[this.currentNomber], position, default(Quaternion));
	}
}
