// 反编译自程序集： Assembly-CSharp.dll
using Spine.Unity;
using System;
using UnityEngine;

public class KungfuBelt : MonoBehaviour
{
	public SpriteRenderer[] mainSrs;

	public SpriteRenderer[] altSrs;

	public BoneFollower boneFollower;

	public void SetMainColor(Color color)
	{
		for (int i = 0; i < this.mainSrs.Length; i++)
		{
			this.mainSrs[i].color = color;
		}
	}

	public void SetAltColor(Color color)
	{
		for (int i = 0; i < this.altSrs.Length; i++)
		{
			this.altSrs[i].color = color;
		}
	}
}
