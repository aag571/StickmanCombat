// 反编译自程序集： Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TerrainGenerator : IEnumerable<Vector3>, IEnumerable
{
	private sealed class _GetEnumerator_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<Vector3>
	{
		internal int _sign___0;

		internal TerrainGenerator _this;

		internal Vector3 _current;

		internal bool _disposing;

		internal int _PC;

		Vector3 IEnumerator<Vector3>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _GetEnumerator_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._sign___0 = -1;
				this._current = new Vector3((float)(-(float)Screen.width), this._this.y, this._this.zPositionOfTerrain);
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				break;
			case 2u:
				break;
			default:
				return false;
			}
			if (this._sign___0 == 1)
			{
				this._this.dx = UnityEngine.Random.Range(this._this.minDeltaX, this._this.rangeDeltaX + this._this.minDeltaX);
				this._this.dy = UnityEngine.Random.Range(0f, this._this.dx / 8f);
			}
			this._this.x += 3f * this._this.dx;
			this._this.y += 10f * this._this.dy * (float)this._sign___0;
			if (this._this.deltaIncline != 0f)
			{
				if (this._this.deltaIncline > 0f)
				{
					this._this.maxHeight += this._this.deltaIncline;
				}
				else if (this._this.deltaIncline < 0f)
				{
					this._this.minHeight += this._this.deltaIncline;
				}
			}
			this._sign___0 *= -1;
			this._current = new Vector3(this._this.x, this._this.y, this._this.zPositionOfTerrain);
			if (!this._disposing)
			{
				this._PC = 2;
			}
			return true;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	public float deltaIncline;

	public float minHeight = 30f;

	public float maxHeight = (float)(Screen.height - 100);

	private float y;

	private float x;

	private float dx;

	private float dy;

	public float minDeltaX;

	public float minDeltaY;

	public float rangeDeltaX;

	public float rangeDeltaY;

	public float zPositionOfTerrain;

	private IEnumerator<Vector3> _internalEnumarator;

	private Vector3[] _terrainKeyPoints = new Vector3[1000];

	private int _lastGeneratedPointIndex = -1;

	public Vector3 this[int index]
	{
		get
		{
			if (index > this._lastGeneratedPointIndex)
			{
				for (int i = 0; i <= 5; i++)
				{
					this._terrainKeyPoints[++this._lastGeneratedPointIndex] = this.nextValue;
				}
			}
			return this._terrainKeyPoints[index];
		}
	}

	public Vector3 nextValue
	{
		get
		{
			Vector3 current = this._internalEnumarator.Current;
			this._internalEnumarator.MoveNext();
			return current;
		}
	}

	public TerrainGenerator()
	{
		this._internalEnumarator = this.GetEnumerator();
		this.minDeltaX = 200f;
		this.minDeltaY = 25f;
		this.rangeDeltaX = 300f;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	public IEnumerator<Vector3> GetEnumerator()
	{
		TerrainGenerator._GetEnumerator_c__Iterator0 _GetEnumerator_c__Iterator = new TerrainGenerator._GetEnumerator_c__Iterator0();
		_GetEnumerator_c__Iterator._this = this;
		return _GetEnumerator_c__Iterator;
	}

	public void Reset()
	{
	}

	public void ResetToLastUsedIndex(int lastUsedIndex)
	{
		if (lastUsedIndex == 0 || this._lastGeneratedPointIndex <= 0)
		{
			return;
		}
		int num = this._lastGeneratedPointIndex - lastUsedIndex + 1;
		for (int i = 0; i < num; i++)
		{
			this._terrainKeyPoints[i] = this._terrainKeyPoints[lastUsedIndex + i];
		}
		this._lastGeneratedPointIndex = num - 1;
	}

	public void TestOtherSettings()
	{
	}
}
