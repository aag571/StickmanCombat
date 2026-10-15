// 反编译自程序集： Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MountainGenerator : MonoBehaviour
{
	public int numberOfHills;

	public float scale;

	public List<Vector2> vertices2D = new List<Vector2>();

	public Material material;

	public Dictionary<int, float> heightList = new Dictionary<int, float>();

	public float yOffset;

	public float lastYTarget;

	private void Start()
	{
		this.yOffset = base.transform.position.y;
		this.Generate();
	}

	public void Generate()
	{
		float num = 0.5f * this.scale;
		float min = 0.3f * this.scale;
		float min2 = 1.5f * this.scale;
		float max = 3f * this.scale;
		this.vertices2D.Clear();
		this.vertices2D.Add(new Vector2(0f, -200f));
		this.vertices2D.Add(new Vector2(0f, 0f));
		this.heightList.Add(0, 0f);
		for (int i = 0; i < this.numberOfHills; i++)
		{
			Vector2 vector = this.vertices2D[this.vertices2D.Count - 1];
			float num2 = UnityEngine.Random.Range(3f, 10f) * this.scale;
			float num3 = UnityEngine.Random.Range(6f, 12f) * this.scale;
			float num4 = UnityEngine.Random.Range(num2 - 0.1f, 0.1f - num2);
			float num5 = num3 / num2 * (num2 - num4);
			Vector2 vector2 = new Vector2(num3 + vector.x, num2 + vector.y);
			Vector2 vector3 = new Vector2(vector.x + num3 + num5, num4 + vector.y);
			int num6 = (int)((vector2.x - vector.x) / num / 2f);
			int num7 = (int)((vector3.x - vector2.x) / num / 2f);
			int num8 = (int)UnityEngine.Random.Range(0f, (float)num6 / 1.5f);
			int num9 = (int)UnityEngine.Random.Range(0f, (float)num7 / 1.5f);
			List<int> list = new List<int>();
			for (int j = 1; j < num6 - 1; j++)
			{
				list.Add(j);
			}
			for (int k = 0; k < num6 - num8 - 2; k++)
			{
				list.RemoveAt(UnityEngine.Random.Range(0, list.Count));
			}
			Vector2 a = (vector2 - vector) / (float)num6;
			Vector2 a2 = a / num;
			for (int l = 0; l < list.Count; l++)
			{
				float d = UnityEngine.Random.Range(min, num);
				Vector2 a3 = vector + a * (float)list[l];
				Vector2 item = a3 - a2 * d;
				Vector2 item2 = a3 + a2 * d;
				Vector2 item3 = a3 + new Vector2(0f, UnityEngine.Random.Range(min2, max));
				this.vertices2D.Add(item);
				this.vertices2D.Add(item3);
				this.vertices2D.Add(item2);
			}
			this.vertices2D.Add(vector2);
			List<int> list2 = new List<int>();
			for (int m = 1; m < num7 - 1; m++)
			{
				list2.Add(m);
			}
			for (int n = 0; n < num7 - num9 - 2; n++)
			{
				list2.RemoveAt(UnityEngine.Random.Range(0, list2.Count));
			}
			a = (vector3 - vector2) / (float)num7;
			a2 = a / num;
			for (int num10 = 0; num10 < list2.Count; num10++)
			{
				float d2 = UnityEngine.Random.Range(min, num);
				Vector2 a4 = vector2 + a * (float)list2[num10];
				Vector2 item4 = a4 - a2 * d2;
				Vector2 item5 = a4 + a2 * d2;
				Vector2 item6 = a4 + new Vector2(0f, UnityEngine.Random.Range(min2, max));
				this.vertices2D.Add(item4);
				this.vertices2D.Add(item6);
				this.vertices2D.Add(item5);
			}
			this.vertices2D.Add(vector3);
			if (!this.heightList.ContainsKey((int)vector2.x))
			{
				this.heightList.Add((int)vector2.x, vector2.y);
			}
			if ((int)vector2.x >= 30 && this.lastYTarget == 0f)
			{
				this.lastYTarget = vector2.y;
			}
		}
		Vector2 vector4 = this.vertices2D[this.vertices2D.Count - 1];
		this.vertices2D.Add(new Vector2(vector4.x, -200f));
		Triangulator triangulator = new Triangulator(this.vertices2D.ToArray());
		int[] triangles = triangulator.Triangulate();
		Vector3[] array = new Vector3[this.vertices2D.Count];
		for (int num11 = 0; num11 < array.Length; num11++)
		{
			array[num11] = new Vector3(this.vertices2D[num11].x, this.vertices2D[num11].y, 0f);
		}
		Mesh mesh = new Mesh();
		mesh.vertices = array;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		MeshRenderer meshRenderer = (MeshRenderer)base.gameObject.AddComponent(typeof(MeshRenderer));
		meshRenderer.material = this.material;
		meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
		meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
		meshRenderer.receiveShadows = false;
		meshRenderer.lightProbeUsage = LightProbeUsage.Off;
		MeshFilter meshFilter = base.gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
		meshFilter.mesh = mesh;
		base.transform.position = new Vector3(base.transform.position.x, this.yOffset - this.lastYTarget, base.transform.position.z);
	}

	public void LateUpdate()
	{
		int key = (int)(Camera.main.transform.position.x - base.transform.position.x);
		if (this.heightList.ContainsKey(key))
		{
			this.lastYTarget = this.yOffset - this.heightList[key];
		}
		base.transform.position = Vector3.Lerp(base.transform.position, new Vector3(base.transform.position.x, this.lastYTarget, base.transform.position.z), Time.deltaTime / 20f);
	}
}
