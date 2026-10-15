// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;

[AddComponentMenu("")]
public class ImageEffects
{
	public static void RenderDistortion(Material material, RenderTexture source, RenderTexture destination, float angle, Vector2 center, Vector2 radius)
	{
		bool flag = source.texelSize.y < 0f;
		if (flag)
		{
			center.y = 1f - center.y;
			angle = -angle;
		}
		Matrix4x4 value = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, angle), Vector3.one);
		material.SetMatrix("_RotationMatrix", value);
		material.SetVector("_CenterRadius", new Vector4(center.x, center.y, radius.x, radius.y));
		material.SetFloat("_Angle", angle * 0.0174532924f);
		Graphics.Blit(source, destination, material);
	}

	[Obsolete("请改用 Graphics.Blit(source,dest)")]
	public static void Blit(RenderTexture source, RenderTexture dest)
	{
		Graphics.Blit(source, dest);
	}

	[Obsolete("请改用 Graphics.Blit(source, destination, material)")]
	public static void BlitWithMaterial(Material material, RenderTexture source, RenderTexture dest)
	{
		Graphics.Blit(source, dest, material);
	}
}
