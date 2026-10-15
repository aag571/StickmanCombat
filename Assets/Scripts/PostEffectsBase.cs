// 反编译自程序集： Assembly-UnityScript.dll
using System;
using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(Camera))]
[Serializable]
public class PostEffectsBase : MonoBehaviour
{
	protected bool supportHDRTextures;

	protected bool supportDX11;

	protected bool isSupported;

	public PostEffectsBase()
	{
		this.supportHDRTextures = true;
		this.isSupported = true;
	}

	public Material CheckShaderAndCreateMaterial(Shader s, Material m2Create)
	{
		Material arg_D0_0;
		if (!s)
		{
			UnityEngine.Debug.Log("缺少着色器：" + this.ToString());
			this.enabled = false;
			arg_D0_0 = null;
		}
		else if (s.isSupported && m2Create && m2Create.shader == s)
		{
			arg_D0_0 = m2Create;
		}
		else if (!s.isSupported)
		{
			this.NotSupported();
			UnityEngine.Debug.Log("着色器 " + s.ToString() + " 所属特效：" + this.ToString() + "，当前平台不支持该着色器！");
			arg_D0_0 = null;
		}
		else
		{
			m2Create = new Material(s);
			m2Create.hideFlags = HideFlags.DontSave;
			arg_D0_0 = ((!m2Create) ? null : m2Create);
		}
		return arg_D0_0;
	}

	public Material CreateMaterial(Shader s, Material m2Create)
	{
		Material arg_8F_0;
		if (!s)
		{
			UnityEngine.Debug.Log("缺少着色器：" + this.ToString());
			arg_8F_0 = null;
		}
		else if (m2Create && m2Create.shader == s && s.isSupported)
		{
			arg_8F_0 = m2Create;
		}
		else if (!s.isSupported)
		{
			arg_8F_0 = null;
		}
		else
		{
			m2Create = new Material(s);
			m2Create.hideFlags = HideFlags.DontSave;
			arg_8F_0 = ((!m2Create) ? null : m2Create);
		}
		return arg_8F_0;
	}

	public void OnEnable()
	{
		this.isSupported = true;
	}

	public bool CheckSupport()
	{
		return this.CheckSupport(false);
	}

	public bool CheckResources()
	{
		UnityEngine.Debug.LogWarning("资源检查 CheckResources()：" + this.ToString() + "，需要重写此方法。");
		return this.isSupported;
	}

	public void Start()
	{
		this.CheckResources();
	}

	public bool CheckSupport(bool needDepth)
	{
		this.isSupported = true;
		this.supportHDRTextures = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf);
		bool arg_2C_1;
		if (arg_2C_1 = (SystemInfo.graphicsShaderLevel >= 50))
		{
			arg_2C_1 = SystemInfo.supportsComputeShaders;
		}
		this.supportDX11 = arg_2C_1;
		bool arg_6D_0;
		if (needDepth && !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
		{
			this.NotSupported();
			arg_6D_0 = false;
		}
		else
		{
			if (needDepth)
			{
				this.GetComponent<Camera>().depthTextureMode = (this.GetComponent<Camera>().depthTextureMode | DepthTextureMode.Depth);
			}
			arg_6D_0 = true;
		}
		return arg_6D_0;
	}

	public bool CheckSupport(bool needDepth, bool needHdr)
	{
		bool arg_30_0;
		if (!this.CheckSupport(needDepth))
		{
			arg_30_0 = false;
		}
		else if (needHdr && !this.supportHDRTextures)
		{
			this.NotSupported();
			arg_30_0 = false;
		}
		else
		{
			arg_30_0 = true;
		}
		return arg_30_0;
	}

	public bool Dx11Support()
	{
		return this.supportDX11;
	}

	public void ReportAutoDisable()
	{
		UnityEngine.Debug.LogWarning("图像特效 " + this.ToString() + " 已禁用，因为当前平台不支持此特效。");
	}

	public bool CheckShader(Shader s)
	{
		UnityEngine.Debug.Log("着色器 " + s.ToString() + " 所属特效：" + this.ToString() + " 已不属于 Unity 3.2 及更高版本的特效套件。为获得最佳性能和画质，请使用最新的 Standard Assets Image Effects 资源包（仅限专业版）。");
		bool arg_51_0;
		if (!s.isSupported)
		{
			this.NotSupported();
			arg_51_0 = false;
		}
		else
		{
			arg_51_0 = false;
		}
		return arg_51_0;
	}

	public void NotSupported()
	{
		this.enabled = false;
		this.isSupported = false;
	}

	public void DrawBorder(RenderTexture dest, Material material)
	{
		float x = 0f;
		float x2 = 0f;
		float y = 0f;
		float y2 = 0f;
		RenderTexture.active = dest;
		bool flag = true;
		GL.PushMatrix();
		GL.LoadOrtho();
		for (int i = 0; i < material.passCount; i++)
		{
			material.SetPass(i);
			float y3 = 0f;
			float y4 = 0f;
			if (flag)
			{
				y3 = 1f;
				y4 = (float)0;
			}
			else
			{
				y3 = (float)0;
				y4 = 1f;
			}
			x = (float)0;
			x2 = (float)0 + 1f / ((float)dest.width * 1f);
			y = (float)0;
			y2 = 1f;
			GL.Begin(7);
			GL.TexCoord2((float)0, y3);
			GL.Vertex3(x, y, 0.1f);
			GL.TexCoord2(1f, y3);
			GL.Vertex3(x2, y, 0.1f);
			GL.TexCoord2(1f, y4);
			GL.Vertex3(x2, y2, 0.1f);
			GL.TexCoord2((float)0, y4);
			GL.Vertex3(x, y2, 0.1f);
			x = 1f - 1f / ((float)dest.width * 1f);
			x2 = 1f;
			y = (float)0;
			y2 = 1f;
			GL.TexCoord2((float)0, y3);
			GL.Vertex3(x, y, 0.1f);
			GL.TexCoord2(1f, y3);
			GL.Vertex3(x2, y, 0.1f);
			GL.TexCoord2(1f, y4);
			GL.Vertex3(x2, y2, 0.1f);
			GL.TexCoord2((float)0, y4);
			GL.Vertex3(x, y2, 0.1f);
			x = (float)0;
			x2 = 1f;
			y = (float)0;
			y2 = (float)0 + 1f / ((float)dest.height * 1f);
			GL.TexCoord2((float)0, y3);
			GL.Vertex3(x, y, 0.1f);
			GL.TexCoord2(1f, y3);
			GL.Vertex3(x2, y, 0.1f);
			GL.TexCoord2(1f, y4);
			GL.Vertex3(x2, y2, 0.1f);
			GL.TexCoord2((float)0, y4);
			GL.Vertex3(x, y2, 0.1f);
			x = (float)0;
			x2 = 1f;
			y = 1f - 1f / ((float)dest.height * 1f);
			y2 = 1f;
			GL.TexCoord2((float)0, y3);
			GL.Vertex3(x, y, 0.1f);
			GL.TexCoord2(1f, y3);
			GL.Vertex3(x2, y, 0.1f);
			GL.TexCoord2(1f, y4);
			GL.Vertex3(x2, y2, 0.1f);
			GL.TexCoord2((float)0, y4);
			GL.Vertex3(x, y2, 0.1f);
			GL.End();
		}
		GL.PopMatrix();
	}

	public void Main()
	{
	}
}
