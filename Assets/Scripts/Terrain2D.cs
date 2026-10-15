// 反编译自程序集： Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class Terrain2D
{
	public TerrainGenerator terrainGenerator;

	public List<Vector3> borderVertices = new List<Vector3>();

	public List<Vector3> bottomVertices = new List<Vector3>();

	public float startX;

	public float endX;

	private int _fromKeyPointIndex;

	private int _toKeyPointIndex;

	public int terrainSegmentWidth = 20;

	public int textureSize = 10;

	public Terrain2D(GameObject originGameObject)
	{
		this.terrainGenerator = new TerrainGenerator();
	}

	public void GenerateMeshWithWidth(float width, MeshFilter meshFilter)
	{
		this.terrainGenerator.ResetToLastUsedIndex(this._toKeyPointIndex);
		this._fromKeyPointIndex = 0;
		this._toKeyPointIndex = 0;
		while (this.terrainGenerator[++this._toKeyPointIndex].x < width)
		{
		}
		this.DrawMesh(meshFilter);
	}

	public void DrawMesh(MeshFilter meshFilter)
	{
		List<Vector3> list = new List<Vector3>();
		List<Vector3> list2 = new List<Vector3>();
		List<Vector3> list3 = new List<Vector3>();
		List<Vector2> list4 = new List<Vector2>();
		List<int> list5 = new List<int>();
		int num = -2;
		Vector3 vector = new Vector3(0f, 0f, this.terrainGenerator.zPositionOfTerrain);
		Vector3 vector2 = this.terrainGenerator[this._fromKeyPointIndex];
		for (int i = this._fromKeyPointIndex + 1; i <= this._toKeyPointIndex; i++)
		{
			Vector3 vector3 = this.terrainGenerator[i];
			int num2 = Mathf.CeilToInt((vector3.x - vector2.x) / (float)this.terrainSegmentWidth);
			float num3 = (vector3.x - vector2.x) / (float)num2;
			float num4 = 3.14159274f / (float)num2;
			float num5 = (vector2.y + vector3.y) / 2f;
			float num6 = (vector2.y - vector3.y) / 2f;
			Vector3 vector4 = vector2;
			if (i == this._toKeyPointIndex)
			{
				num2++;
			}
			for (int j = 0; j <= num2; j++)
			{
				vector.x = vector2.x + (float)j * num3;
				vector.y = num5 + num6 * Mathf.Cos(num4 * (float)j);
				Vector3 vector5 = new Vector3(vector4.x, vector4.y, this.terrainGenerator.zPositionOfTerrain);
				list.Add(vector5);
				list2.Add(vector5 - new Vector3(0f, (float)this.textureSize, 0f));
				list3.Add(vector5);
				list4.Add(new Vector2(vector4.x / (float)this.textureSize, 1f));
				list3.Add(new Vector3(vector4.x, vector4.y - (float)this.textureSize, this.terrainGenerator.zPositionOfTerrain));
				list4.Add(new Vector2(vector4.x / (float)this.textureSize, 0f));
				if (num >= 0)
				{
					list5.Add(num + 2);
					list5.Add(num + 1);
					list5.Add(num);
					list5.Add(num + 3);
					list5.Add(num + 1);
					list5.Add(num + 2);
				}
				num += 2;
				vector4 = vector;
			}
			vector2 = vector3;
		}
		Mesh mesh = meshFilter.mesh;
		mesh.Clear();
		mesh.vertices = list3.ToArray();
		mesh.uv = list4.ToArray();
		mesh.triangles = list5.ToArray();
		mesh.RecalculateBounds();
		this.AddMeshCollider(meshFilter, list, false);
	}

	public void AddMeshCollider(MeshFilter meshFilter, List<Vector3> borderVerts, bool isTop = false)
	{
		if (isTop)
		{
			borderVerts.Insert(0, new Vector3(borderVerts[0].x, borderVerts[0].y + 5000f + this.terrainGenerator.deltaIncline, borderVerts[0].z));
			borderVerts.Add(new Vector3(borderVerts[borderVerts.Count - 1].x, borderVerts[borderVerts.Count - 1].y + 5000f + this.terrainGenerator.deltaIncline, borderVerts[0].z));
		}
		else
		{
			borderVerts.Insert(0, new Vector3(borderVerts[0].x, borderVerts[0].y - 5000f + this.terrainGenerator.deltaIncline, borderVerts[0].z));
			borderVerts.Add(new Vector3(borderVerts[borderVerts.Count - 1].x, borderVerts[borderVerts.Count - 1].y - 5000f + this.terrainGenerator.deltaIncline, borderVerts[0].z));
		}
		List<Vector3> list = new List<Vector3>(borderVerts);
		List<int> list2 = new List<int>();
		for (int i = 0; i < borderVerts.Count; i++)
		{
			list.Add(new Vector3(borderVerts[i].x, borderVerts[i].y, this.terrainGenerator.zPositionOfTerrain + 100f));
		}
		int count = borderVerts.Count;
		for (int j = 0; j < borderVerts.Count; j++)
		{
			int num = j;
			int num2 = (num + 1) % count;
			int item = num + count;
			int item2 = num2 + count;
			if (isTop)
			{
				list2.Add(num);
				list2.Add(num2);
				list2.Add(item2);
				list2.Add(num);
				list2.Add(item);
				list2.Add(item2);
			}
			else
			{
				list2.Add(num);
				list2.Add(item);
				list2.Add(item2);
				list2.Add(num);
				list2.Add(item2);
				list2.Add(num2);
			}
		}
		Mesh mesh = new Mesh();
		mesh.vertices = list.ToArray();
		mesh.triangles = list2.ToArray();
		mesh.RecalculateBounds();
		meshFilter.GetComponent<MeshCollider>().sharedMesh = mesh;
	}
}
