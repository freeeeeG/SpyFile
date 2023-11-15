using System;
using System.Collections.Generic;
using System.IO;
using AllIn1SpriteShader;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000002 RID: 2
[ExecuteInEditMode]
[DisallowMultipleComponent]
[AddComponentMenu("AllIn1SpriteShader/AddAllIn1Shader")]
public class AllIn1Shader : MonoBehaviour
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public void MakeNewMaterial(bool getShaderTypeFromPrefs, string shaderName = "AllIn1SpriteShader")
	{
		this.SetMaterial(AllIn1Shader.AfterSetAction.Clear, getShaderTypeFromPrefs, shaderName);
	}

	// Token: 0x06000002 RID: 2 RVA: 0x0000205B File Offset: 0x0000025B
	public void MakeCopy()
	{
		this.SetMaterial(AllIn1Shader.AfterSetAction.CopyMaterial, false, this.GetStringFromShaderType());
	}

	// Token: 0x06000003 RID: 3 RVA: 0x0000206B File Offset: 0x0000026B
	private void ResetAllProperties(bool getShaderTypeFromPrefs, string shaderName)
	{
		this.SetMaterial(AllIn1Shader.AfterSetAction.Reset, getShaderTypeFromPrefs, shaderName);
	}

	// Token: 0x06000004 RID: 4 RVA: 0x00002078 File Offset: 0x00000278
	private string GetStringFromShaderType()
	{
		if (this.shaderTypes == AllIn1Shader.ShaderTypes.Default)
		{
			return "AllIn1SpriteShader";
		}
		if (this.shaderTypes == AllIn1Shader.ShaderTypes.ScaledTime)
		{
			return "AllIn1SpriteShaderScaledTime";
		}
		if (this.shaderTypes == AllIn1Shader.ShaderTypes.MaskedUI)
		{
			return "AllIn1SpriteShaderUiMask";
		}
		if (this.shaderTypes == AllIn1Shader.ShaderTypes.Urp2dRenderer)
		{
			return "AllIn1Urp2dRenderer";
		}
		return "AllIn1SpriteShader";
	}

	// Token: 0x06000005 RID: 5 RVA: 0x000020C8 File Offset: 0x000002C8
	private void SetMaterial(AllIn1Shader.AfterSetAction action, bool getShaderTypeFromPrefs, string shaderName)
	{
		Shader shader = Resources.Load(shaderName, typeof(Shader)) as Shader;
		if (getShaderTypeFromPrefs)
		{
			int @int = PlayerPrefs.GetInt("allIn1DefaultShader");
			if (@int == 1)
			{
				shader = (Resources.Load("AllIn1SpriteShaderScaledTime", typeof(Shader)) as Shader);
			}
			else if (@int == 2)
			{
				shader = (Resources.Load("AllIn1SpriteShaderUiMask", typeof(Shader)) as Shader);
			}
			else if (@int == 3)
			{
				shader = (Resources.Load("AllIn1Urp2dRenderer", typeof(Shader)) as Shader);
			}
		}
		if (Application.isPlaying || !Application.isEditor || !(shader != null))
		{
			if (shader == null)
			{
				Debug.LogError("Make sure the AllIn1SpriteShader shader variants are inside the Resource folder!");
			}
			return;
		}
		bool flag = false;
		if (base.GetComponent<Renderer>() != null)
		{
			flag = true;
			this.prevMaterial = new Material(base.GetComponent<Renderer>().sharedMaterial);
			this.currMaterial = new Material(shader);
			base.GetComponent<Renderer>().sharedMaterial = this.currMaterial;
			base.GetComponent<Renderer>().sharedMaterial.hideFlags = HideFlags.None;
			this.matAssigned = true;
			this.DoAfterSetAction(action);
		}
		else
		{
			Graphic component = base.GetComponent<Graphic>();
			if (component != null)
			{
				flag = true;
				this.prevMaterial = new Material(component.material);
				this.currMaterial = new Material(shader);
				component.material = this.currMaterial;
				component.material.hideFlags = HideFlags.None;
				this.matAssigned = true;
				this.DoAfterSetAction(action);
			}
		}
		if (!flag)
		{
			this.MissingRenderer();
			return;
		}
		this.SetSceneDirty();
	}

	// Token: 0x06000006 RID: 6 RVA: 0x00002256 File Offset: 0x00000456
	private void DoAfterSetAction(AllIn1Shader.AfterSetAction action)
	{
		if (action == AllIn1Shader.AfterSetAction.Clear)
		{
			this.ClearAllKeywords();
			return;
		}
		if (action != AllIn1Shader.AfterSetAction.CopyMaterial)
		{
			return;
		}
		this.currMaterial.CopyPropertiesFromMaterial(this.prevMaterial);
	}

	// Token: 0x06000007 RID: 7 RVA: 0x00002278 File Offset: 0x00000478
	public void TryCreateNew()
	{
		bool flag = false;
		Renderer component = base.GetComponent<Renderer>();
		if (component != null)
		{
			flag = true;
			if (component != null && component.sharedMaterial != null && component.sharedMaterial.name.Contains("AllIn1"))
			{
				this.ResetAllProperties(false, this.GetStringFromShaderType());
				this.ClearAllKeywords();
			}
			else
			{
				this.CleanMaterial();
				this.MakeNewMaterial(false, this.GetStringFromShaderType());
			}
		}
		else
		{
			Graphic component2 = base.GetComponent<Graphic>();
			if (component2 != null)
			{
				flag = true;
				if (component2.material.name.Contains("AllIn1"))
				{
					this.ResetAllProperties(false, this.GetStringFromShaderType());
					this.ClearAllKeywords();
				}
				else
				{
					this.MakeNewMaterial(false, this.GetStringFromShaderType());
				}
			}
		}
		if (!flag)
		{
			this.MissingRenderer();
		}
		this.SetSceneDirty();
	}

	// Token: 0x06000008 RID: 8 RVA: 0x0000234C File Offset: 0x0000054C
	public void ClearAllKeywords()
	{
		this.SetKeyword("RECTSIZE_ON", false);
		this.SetKeyword("OFFSETUV_ON", false);
		this.SetKeyword("CLIPPING_ON", false);
		this.SetKeyword("POLARUV_ON", false);
		this.SetKeyword("TWISTUV_ON", false);
		this.SetKeyword("ROTATEUV_ON", false);
		this.SetKeyword("FISHEYE_ON", false);
		this.SetKeyword("PINCH_ON", false);
		this.SetKeyword("SHAKEUV_ON", false);
		this.SetKeyword("WAVEUV_ON", false);
		this.SetKeyword("ROUNDWAVEUV_ON", false);
		this.SetKeyword("DOODLE_ON", false);
		this.SetKeyword("ZOOMUV_ON", false);
		this.SetKeyword("FADE_ON", false);
		this.SetKeyword("TEXTURESCROLL_ON", false);
		this.SetKeyword("GLOW_ON", false);
		this.SetKeyword("OUTBASE_ON", false);
		this.SetKeyword("ONLYOUTLINE_ON", false);
		this.SetKeyword("OUTTEX_ON", false);
		this.SetKeyword("OUTDIST_ON", false);
		this.SetKeyword("DISTORT_ON", false);
		this.SetKeyword("WIND_ON", false);
		this.SetKeyword("GRADIENT_ON", false);
		this.SetKeyword("GRADIENT2COL_ON", false);
		this.SetKeyword("RADIALGRADIENT_ON", false);
		this.SetKeyword("COLORSWAP_ON", false);
		this.SetKeyword("HSV_ON", false);
		this.SetKeyword("HITEFFECT_ON", false);
		this.SetKeyword("PIXELATE_ON", false);
		this.SetKeyword("NEGATIVE_ON", false);
		this.SetKeyword("GRADIENTCOLORRAMP_ON", false);
		this.SetKeyword("COLORRAMP_ON", false);
		this.SetKeyword("GREYSCALE_ON", false);
		this.SetKeyword("POSTERIZE_ON", false);
		this.SetKeyword("BLUR_ON", false);
		this.SetKeyword("MOTIONBLUR_ON", false);
		this.SetKeyword("GHOST_ON", false);
		this.SetKeyword("ALPHAOUTLINE_ON", false);
		this.SetKeyword("INNEROUTLINE_ON", false);
		this.SetKeyword("ONLYINNEROUTLINE_ON", false);
		this.SetKeyword("HOLOGRAM_ON", false);
		this.SetKeyword("CHROMABERR_ON", false);
		this.SetKeyword("GLITCH_ON", false);
		this.SetKeyword("FLICKER_ON", false);
		this.SetKeyword("SHADOW_ON", false);
		this.SetKeyword("SHINE_ON", false);
		this.SetKeyword("CONTRAST_ON", false);
		this.SetKeyword("OVERLAY_ON", false);
		this.SetKeyword("OVERLAYMULT_ON", false);
		this.SetKeyword("ALPHACUTOFF_ON", false);
		this.SetKeyword("ALPHAROUND_ON", false);
		this.SetKeyword("CHANGECOLOR_ON", false);
		this.SetKeyword("CHANGECOLOR2_ON", false);
		this.SetKeyword("CHANGECOLOR3_ON", false);
		this.SetKeyword("FOG_ON", false);
		this.SetSceneDirty();
	}

	// Token: 0x06000009 RID: 9 RVA: 0x000025F4 File Offset: 0x000007F4
	private void SetKeyword(string keyword, bool state = false)
	{
		if (this.destroyed)
		{
			return;
		}
		if (this.currMaterial == null)
		{
			this.FindCurrMaterial();
			if (this.currMaterial == null)
			{
				this.MissingRenderer();
				return;
			}
		}
		if (!state)
		{
			this.currMaterial.DisableKeyword(keyword);
			return;
		}
		this.currMaterial.EnableKeyword(keyword);
	}

	// Token: 0x0600000A RID: 10 RVA: 0x00002650 File Offset: 0x00000850
	private void FindCurrMaterial()
	{
		if (base.GetComponent<Renderer>() != null)
		{
			this.currMaterial = base.GetComponent<Renderer>().sharedMaterial;
			this.matAssigned = true;
			return;
		}
		Graphic component = base.GetComponent<Graphic>();
		if (component != null)
		{
			this.currMaterial = component.material;
			this.matAssigned = true;
		}
	}

	// Token: 0x0600000B RID: 11 RVA: 0x000026A8 File Offset: 0x000008A8
	public void CleanMaterial()
	{
		Renderer component = base.GetComponent<Renderer>();
		if (component != null)
		{
			component.sharedMaterial = new Material(Shader.Find("Sprites/Default"));
			this.matAssigned = false;
		}
		else
		{
			Graphic component2 = base.GetComponent<Graphic>();
			if (component2 != null)
			{
				component2.material = new Material(Shader.Find("Sprites/Default"));
				this.matAssigned = false;
			}
		}
		this.SetSceneDirty();
	}

	// Token: 0x0600000C RID: 12 RVA: 0x00002715 File Offset: 0x00000915
	public void SaveMaterial()
	{
	}

	// Token: 0x0600000D RID: 13 RVA: 0x00002718 File Offset: 0x00000918
	private void SaveMaterialWithOtherName(string path, int i = 1)
	{
		int num = i;
		string fileName = path + "_" + num.ToString() + ".mat";
		if (File.Exists(fileName))
		{
			num++;
			this.SaveMaterialWithOtherName(path, num);
			return;
		}
		this.DoSaving(fileName);
	}

	// Token: 0x0600000E RID: 14 RVA: 0x00002760 File Offset: 0x00000960
	private void DoSaving(string fileName)
	{
	}

	// Token: 0x0600000F RID: 15 RVA: 0x00002762 File Offset: 0x00000962
	public void SetSceneDirty()
	{
	}

	// Token: 0x06000010 RID: 16 RVA: 0x00002764 File Offset: 0x00000964
	private void MissingRenderer()
	{
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00002768 File Offset: 0x00000968
	public void ToggleSetAtlasUvs(bool activate)
	{
		SetAtlasUvs setAtlasUvs = base.GetComponent<SetAtlasUvs>();
		if (activate)
		{
			if (setAtlasUvs == null)
			{
				setAtlasUvs = base.gameObject.AddComponent<SetAtlasUvs>();
			}
			setAtlasUvs.GetAndSetUVs();
			this.SetKeyword("ATLAS_ON", true);
		}
		else
		{
			if (setAtlasUvs != null)
			{
				setAtlasUvs.ResetAtlasUvs();
				Object.DestroyImmediate(setAtlasUvs);
			}
			this.SetKeyword("ATLAS_ON", false);
		}
		this.SetSceneDirty();
	}

	// Token: 0x06000012 RID: 18 RVA: 0x000027D0 File Offset: 0x000009D0
	public void ApplyMaterialToHierarchy()
	{
		Renderer component = base.GetComponent<Renderer>();
		Graphic component2 = base.GetComponent<Graphic>();
		Material material;
		if (component != null)
		{
			material = component.sharedMaterial;
		}
		else
		{
			if (!(component2 != null))
			{
				this.MissingRenderer();
				return;
			}
			material = component2.material;
		}
		List<Transform> list = new List<Transform>();
		this.GetAllChildren(base.transform, ref list);
		foreach (Transform transform in list)
		{
			component = transform.gameObject.GetComponent<Renderer>();
			if (component != null)
			{
				component.material = material;
			}
			else
			{
				component2 = transform.gameObject.GetComponent<Graphic>();
				if (component2 != null)
				{
					component2.material = material;
				}
			}
		}
	}

	// Token: 0x06000013 RID: 19 RVA: 0x000028A4 File Offset: 0x00000AA4
	public void CheckIfValidTarget()
	{
		Object component = base.GetComponent<Renderer>();
		Graphic component2 = base.GetComponent<Graphic>();
		if (component == null && component2 == null)
		{
			this.MissingRenderer();
		}
	}

	// Token: 0x06000014 RID: 20 RVA: 0x000028D8 File Offset: 0x00000AD8
	private void GetAllChildren(Transform parent, ref List<Transform> transforms)
	{
		foreach (object obj in parent)
		{
			Transform transform = (Transform)obj;
			transforms.Add(transform);
			this.GetAllChildren(transform, ref transforms);
		}
	}

	// Token: 0x06000015 RID: 21 RVA: 0x00002938 File Offset: 0x00000B38
	public void RenderToImage()
	{
	}

	// Token: 0x06000016 RID: 22 RVA: 0x0000293A File Offset: 0x00000B3A
	public void RenderAndSaveTexture(Material targetMaterial, Texture targetTexture)
	{
	}

	// Token: 0x06000017 RID: 23 RVA: 0x0000293C File Offset: 0x00000B3C
	private string GetNewValidPath(string path, int i = 1)
	{
		int num = i;
		string result = path + "_" + num.ToString() + ".png";
		if (File.Exists(result))
		{
			num++;
			result = this.GetNewValidPath(path, num);
		}
		return result;
	}

	// Token: 0x06000018 RID: 24 RVA: 0x0000297E File Offset: 0x00000B7E
	protected virtual void OnEnable()
	{
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00002980 File Offset: 0x00000B80
	protected virtual void OnDisable()
	{
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00002984 File Offset: 0x00000B84
	protected virtual void OnEditorUpdate()
	{
		if (this.computingNormal)
		{
			if (this.needToWait)
			{
				this.waitingCycles++;
				if (this.waitingCycles > 5)
				{
					this.needToWait = false;
					this.timesWeWaited++;
					return;
				}
			}
			else
			{
				if (this.timesWeWaited == 1)
				{
					this.SetNewNormalTexture2();
				}
				if (this.timesWeWaited == 2)
				{
					this.SetNewNormalTexture3();
				}
				if (this.timesWeWaited == 3)
				{
					this.SetNewNormalTexture4();
				}
				this.needToWait = true;
			}
		}
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00002A02 File Offset: 0x00000C02
	public void CreateAndAssignNormalMap()
	{
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00002A04 File Offset: 0x00000C04
	private void SetNewNormalTexture()
	{
		this.computingNormal = false;
	}

	// Token: 0x0600001D RID: 29 RVA: 0x00002A0D File Offset: 0x00000C0D
	private void SetNewNormalTexture2()
	{
	}

	// Token: 0x0600001E RID: 30 RVA: 0x00002A0F File Offset: 0x00000C0F
	private void SetNewNormalTexture3()
	{
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00002A11 File Offset: 0x00000C11
	private void SetNewNormalTexture4()
	{
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00002A14 File Offset: 0x00000C14
	private Texture2D CreateNormalMap(Texture2D t, float normalMult = 5f, int normalSmooth = 0)
	{
		Color[] array = new Color[t.width * t.height];
		Texture2D texture2D = new Texture2D(t.width, t.height, TextureFormat.RGB24, false, false);
		Vector3 rhs = new Vector3(0.3333f, 0.3333f, 0.3333f);
		for (int i = 0; i < t.height; i++)
		{
			for (int j = 0; j < t.width; j++)
			{
				Color pixel = t.GetPixel(j - 1, i - 1);
				Vector3 lhs = new Vector3(pixel.r, pixel.g, pixel.g);
				pixel = t.GetPixel(j, i - 1);
				Vector3 lhs2 = new Vector3(pixel.r, pixel.g, pixel.g);
				pixel = t.GetPixel(j + 1, i - 1);
				Vector3 lhs3 = new Vector3(pixel.r, pixel.g, pixel.g);
				pixel = t.GetPixel(j - 1, i);
				Vector3 lhs4 = new Vector3(pixel.r, pixel.g, pixel.g);
				pixel = t.GetPixel(j + 1, i);
				Vector3 lhs5 = new Vector3(pixel.r, pixel.g, pixel.g);
				pixel = t.GetPixel(j - 1, i + 1);
				Vector3 lhs6 = new Vector3(pixel.r, pixel.g, pixel.g);
				pixel = t.GetPixel(j, i + 1);
				Vector3 lhs7 = new Vector3(pixel.r, pixel.g, pixel.g);
				pixel = t.GetPixel(j + 1, i + 1);
				Vector3 lhs8 = new Vector3(pixel.r, pixel.g, pixel.g);
				float num = Vector3.Dot(lhs, rhs);
				float num2 = Vector3.Dot(lhs2, rhs);
				float num3 = Vector3.Dot(lhs3, rhs);
				float num4 = Vector3.Dot(lhs4, rhs);
				float num5 = Vector3.Dot(lhs5, rhs);
				float num6 = Vector3.Dot(lhs6, rhs);
				float num7 = Vector3.Dot(lhs7, rhs);
				float num8 = Vector3.Dot(lhs8, rhs);
				float x = (num - num3) * 0.25f + (num4 - num5) * 0.5f + (num6 - num8) * 0.25f;
				float y = (num - num6) * 0.25f + (num2 - num7) * 0.5f + (num3 - num8) * 0.25f;
				Vector2 vector = new Vector2(x, y) * normalMult;
				Vector3 normalized = new Vector3(vector.x, vector.y, 1f).normalized;
				Color color = new Color(normalized.x * 0.5f + 0.5f, normalized.y * 0.5f + 0.5f, normalized.z * 0.5f + 0.5f, 1f);
				array[j + i * t.width] = color;
			}
		}
		if ((float)normalSmooth > 0f)
		{
			for (int k = 0; k < t.height; k++)
			{
				for (int l = 0; l < t.width; l++)
				{
					float num9 = 0f;
					Color a = array[l + k * t.width];
					num9 += 1f;
					if (l - normalSmooth > 0)
					{
						if (k - normalSmooth > 0)
						{
							a += array[l - normalSmooth + (k - normalSmooth) * t.width];
							num9 += 1f;
						}
						a += array[l - normalSmooth + k * t.width];
						num9 += 1f;
						if (k + normalSmooth < t.height)
						{
							a += array[l - normalSmooth + (k + normalSmooth) * t.width];
							num9 += 1f;
						}
					}
					if (k - normalSmooth > 0)
					{
						a += array[l + (k - normalSmooth) * t.width];
						num9 += 1f;
					}
					if (k + normalSmooth < t.height)
					{
						a += array[l + (k + normalSmooth) * t.width];
						num9 += 1f;
					}
					if (l + normalSmooth < t.width)
					{
						if (k - normalSmooth > 0)
						{
							a += array[l + normalSmooth + (k - normalSmooth) * t.width];
							num9 += 1f;
						}
						a += array[l + normalSmooth + k * t.width];
						num9 += 1f;
						if (k + normalSmooth < t.height)
						{
							a += array[l + normalSmooth + (k + normalSmooth) * t.width];
							num9 += 1f;
						}
					}
					array[l + k * t.width] = a / num9;
				}
			}
		}
		texture2D.SetPixels(array);
		texture2D.Apply();
		return texture2D;
	}

	// Token: 0x04000001 RID: 1
	public AllIn1Shader.ShaderTypes shaderTypes = AllIn1Shader.ShaderTypes.Invalid;

	// Token: 0x04000002 RID: 2
	private Material currMaterial;

	// Token: 0x04000003 RID: 3
	private Material prevMaterial;

	// Token: 0x04000004 RID: 4
	private bool matAssigned;

	// Token: 0x04000005 RID: 5
	private bool destroyed;

	// Token: 0x04000006 RID: 6
	[Range(1f, 20f)]
	public float normalStrenght = 5f;

	// Token: 0x04000007 RID: 7
	[Range(0f, 3f)]
	public int normalSmoothing = 1;

	// Token: 0x04000008 RID: 8
	[HideInInspector]
	public bool computingNormal;

	// Token: 0x04000009 RID: 9
	private bool needToWait;

	// Token: 0x0400000A RID: 10
	private int waitingCycles;

	// Token: 0x0400000B RID: 11
	private int timesWeWaited;

	// Token: 0x0400000C RID: 12
	private SpriteRenderer normalMapSr;

	// Token: 0x0400000D RID: 13
	private Renderer normalMapRenderer;

	// Token: 0x0400000E RID: 14
	private bool isSpriteRenderer;

	// Token: 0x0400000F RID: 15
	private string path;

	// Token: 0x04000010 RID: 16
	private string subPath;

	// Token: 0x020002D1 RID: 721
	public enum ShaderTypes
	{
		// Token: 0x040009C7 RID: 2503
		Default,
		// Token: 0x040009C8 RID: 2504
		ScaledTime,
		// Token: 0x040009C9 RID: 2505
		MaskedUI,
		// Token: 0x040009CA RID: 2506
		Urp2dRenderer,
		// Token: 0x040009CB RID: 2507
		Invalid
	}

	// Token: 0x020002D2 RID: 722
	private enum AfterSetAction
	{
		// Token: 0x040009CD RID: 2509
		Clear,
		// Token: 0x040009CE RID: 2510
		CopyMaterial,
		// Token: 0x040009CF RID: 2511
		Reset
	}
}
