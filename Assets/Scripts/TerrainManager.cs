// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
	public PhysicMaterial physicsMaterial;

	public Material terrainMaterial;

	private Terrain2D _terrain;

	private Camera _camera;

	private Transform _cameraTransform;

	private GameObject[] _terrainGameObjects = new GameObject[2];

	private int _currentTerrainGameObjectIndex = -1;

	private float _meshMaxX;

	private void Awake()
	{
		base.transform.position = Vector3.zero;
		this._camera = Camera.main;
		this._cameraTransform = this._camera.transform;
		this._terrain = new Terrain2D(base.gameObject);
		this._terrain.terrainGenerator.TestOtherSettings();
		this.terrainMaterial.mainTexture = (Resources.Load("testTexture", typeof(Texture)) as Texture);
		for (int i = 0; i <= 1; i++)
		{
			GameObject gameObject = new GameObject("mesh" + i);
			gameObject.transform.parent = base.transform;
			gameObject.AddComponent<MeshFilter>();
			MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
			meshCollider.material = this.physicsMaterial;
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = this.terrainMaterial;
			this._terrainGameObjects[i] = gameObject;
		}
	}

	private void Update()
	{
		float num = this._cameraTransform.position.x + this._camera.orthographicSize * 2f;
		if (num > this._meshMaxX)
		{
			this._currentTerrainGameObjectIndex++;
			if (this._currentTerrainGameObjectIndex > 1)
			{
				this._currentTerrainGameObjectIndex = 0;
			}
			GameObject gameObject = this._terrainGameObjects[this._currentTerrainGameObjectIndex];
			this._meshMaxX += 3500f;
			this._terrain.GenerateMeshWithWidth(this._meshMaxX, gameObject.GetComponent<MeshFilter>());
		}
	}
}
