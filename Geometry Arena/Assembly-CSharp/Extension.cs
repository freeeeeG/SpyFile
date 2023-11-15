using System;
using System.Collections.Generic;
using System.Data;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x0200006D RID: 109
public static class Extension
{
	// Token: 0x0600040D RID: 1037 RVA: 0x00019E9C File Offset: 0x0001809C
	public static string Colored(this string str, Color color)
	{
		string text = ColorUtility.ToHtmlStringRGB(color);
		str = string.Concat(new string[]
		{
			"<color=#",
			text,
			">",
			str,
			"</color>"
		});
		return str;
	}

	// Token: 0x0600040E RID: 1038 RVA: 0x00019EDE File Offset: 0x000180DE
	public static string Sized(this string str, float size)
	{
		str = string.Format("<size={0}>{1}</size>", size, str);
		return str;
	}

	// Token: 0x0600040F RID: 1039 RVA: 0x00019EF4 File Offset: 0x000180F4
	public static string Bolded(this string str)
	{
		str = "<b>" + str + "</b>";
		return str;
	}

	// Token: 0x06000410 RID: 1040 RVA: 0x00019F09 File Offset: 0x00018109
	public static string TextSet(this string str, TextSet set)
	{
		str = str.Sized((float)set.size).Colored(set.color);
		if (set.bold)
		{
			str = str.Bolded();
		}
		return str;
	}

	// Token: 0x06000411 RID: 1041 RVA: 0x00019F36 File Offset: 0x00018136
	public static Color GetColor(this EnumRank rank)
	{
		return (new Color[]
		{
			Color.green,
			Color.blue,
			Color.magenta,
			Color.yellow
		})[(int)rank];
	}

	// Token: 0x06000412 RID: 1042 RVA: 0x00019F74 File Offset: 0x00018174
	public static Color SetValue(this Color color, float value)
	{
		if (value < 0f || value > 1f)
		{
			Debug.LogError("Input Error!");
			return color;
		}
		float h;
		float s;
		float num;
		Color.RGBToHSV(color, out h, out s, out num);
		return Color.HSVToRGB(h, s, value);
	}

	// Token: 0x06000413 RID: 1043 RVA: 0x00019FB4 File Offset: 0x000181B4
	public static Color SetSaturation(this Color color, float value)
	{
		if (value < 0f || value > 1f)
		{
			Debug.LogError("Input Error! value=" + value);
			return color;
		}
		float h;
		float num;
		float v;
		Color.RGBToHSV(color, out h, out num, out v);
		return Color.HSVToRGB(h, value, v);
	}

	// Token: 0x06000414 RID: 1044 RVA: 0x00019FFC File Offset: 0x000181FC
	public static Color SetSaturationAndValue(this Color color, float saturation, float value)
	{
		if (saturation < 0f || saturation > 1f)
		{
			Debug.LogError("Saturation Error!");
			return color;
		}
		if (value < 0f || value > 1f)
		{
			Debug.LogError("Value Error!");
			return color;
		}
		float h;
		float num;
		float num2;
		Color.RGBToHSV(color, out h, out num, out num2);
		return Color.HSVToRGB(h, saturation, value);
	}

	// Token: 0x06000415 RID: 1045 RVA: 0x0001A055 File Offset: 0x00018255
	public static Color ApplyColorSet(this Color color, ColorSet set)
	{
		return color.SetSaturationAndValue(set.saturation / 100f, set.value / 100f);
	}

	// Token: 0x06000416 RID: 1046 RVA: 0x0001A078 File Offset: 0x00018278
	public static Color SetHue(this Color color, float value)
	{
		if (value < 0f || value > 1f)
		{
			Debug.LogError("Input Error!");
			return color;
		}
		float num;
		float s;
		float v;
		Color.RGBToHSV(color, out num, out s, out v);
		return Color.HSVToRGB(value, s, v);
	}

	// Token: 0x06000417 RID: 1047 RVA: 0x0001A0B5 File Offset: 0x000182B5
	public static string ReplaceLineBreak(this string str)
	{
		return str.Replace("\\n", "\n");
	}

	// Token: 0x06000418 RID: 1048 RVA: 0x0001A0C7 File Offset: 0x000182C7
	public static string ReplaceTabChar(this string str)
	{
		return str.Replace("\\t", "\t");
	}

	// Token: 0x06000419 RID: 1049 RVA: 0x0001A0DC File Offset: 0x000182DC
	public static float Hue(this Color color)
	{
		float result;
		float num;
		float num2;
		Color.RGBToHSV(color, out result, out num, out num2);
		return result;
	}

	// Token: 0x0600041A RID: 1050 RVA: 0x0001A0F8 File Offset: 0x000182F8
	public static float Saturation(this Color color)
	{
		float num;
		float result;
		float num2;
		Color.RGBToHSV(color, out num, out result, out num2);
		return result;
	}

	// Token: 0x0600041B RID: 1051 RVA: 0x0001A114 File Offset: 0x00018314
	public static float Value(this Color color)
	{
		float num;
		float num2;
		float result;
		Color.RGBToHSV(color, out num, out num2, out result);
		return result;
	}

	// Token: 0x0600041C RID: 1052 RVA: 0x0001A12E File Offset: 0x0001832E
	public static Color SetAlpha(this Color color, float value)
	{
		if (value < 0f || value > 1f)
		{
			Debug.LogError("Input Error!");
			return color;
		}
		color = new Color(color.r, color.g, color.b, value);
		return color;
	}

	// Token: 0x0600041D RID: 1053 RVA: 0x0001A168 File Offset: 0x00018368
	public static Vector2 Rotate(this Vector2 vector2, float angelZ)
	{
		float magnitude = vector2.magnitude;
		return MyTool.AngleToVec2(MyTool.Vec2toAngle180(vector2.normalized) + angelZ) * magnitude;
	}

	// Token: 0x0600041E RID: 1054 RVA: 0x0001A196 File Offset: 0x00018396
	public static int RoundToInt(this float value)
	{
		return Mathf.RoundToInt(value);
	}

	// Token: 0x0600041F RID: 1055 RVA: 0x0001A19E File Offset: 0x0001839E
	public static int RoundToInt(this double value)
	{
		return (int)math.round(value);
	}

	// Token: 0x06000420 RID: 1056 RVA: 0x0001A1A8 File Offset: 0x000183A8
	public static int Int(this float value)
	{
		int num = Mathf.RoundToInt(value);
		if ((float)num != value)
		{
			Debug.LogError("Not Int!");
			return 0;
		}
		return num;
	}

	// Token: 0x06000421 RID: 1057 RVA: 0x0001A1D0 File Offset: 0x000183D0
	public static List<T> Shuffle<T>(this List<T> list)
	{
		int count = list.Count;
		int[] array = new int[count];
		List<T> list2 = new List<T>();
		for (int i = 0; i < count; i++)
		{
			array[i] = UnityEngine.Random.Range(0, 100);
		}
		for (int j = 0; j < count; j++)
		{
			int num = -1;
			int num2 = -1;
			for (int k = 0; k < count; k++)
			{
				if (array[k] > num)
				{
					num = array[k];
					num2 = k;
				}
			}
			if (num == -1 || num2 == -1)
			{
				Debug.LogError("ShuffleError! -1!");
				return null;
			}
			array[num2] = -1;
			list2.Add(list[num2]);
		}
		return list2;
	}

	// Token: 0x06000422 RID: 1058 RVA: 0x0001A26C File Offset: 0x0001846C
	public static T GetRandom<T>(this List<T> list)
	{
		int count = list.Count;
		int index = UnityEngine.Random.Range(0, count);
		return list[index];
	}

	// Token: 0x06000423 RID: 1059 RVA: 0x0001A290 File Offset: 0x00018490
	public static Vector2 DirectionApproach(this Vector2 dirCurrent, Vector2 dirTarget, float maxAngle)
	{
		float num = MyTool.Vec2toAngle180(dirCurrent);
		float num2 = MyTool.Vec2toAngle180(dirTarget) - num;
		if (num2 < -180f)
		{
			num2 += 360f;
		}
		if (num2 > 180f)
		{
			num2 -= 360f;
		}
		float num3 = Mathf.Sign(num2) * Mathf.Min(Mathf.Abs(num2), maxAngle);
		return MyTool.AngleToVec2(num + num3);
	}

	// Token: 0x06000424 RID: 1060 RVA: 0x0001A2EC File Offset: 0x000184EC
	public static Vector2 RandomDeltaEuler(this Vector2 vec2, float angleMin, float angleMax)
	{
		float angelZ = UnityEngine.Random.Range(angleMin, angleMax);
		return vec2.Rotate(angelZ);
	}

	// Token: 0x06000425 RID: 1061 RVA: 0x0001A308 File Offset: 0x00018508
	public static AudioClip RandomAC(this AudioClip[] acs)
	{
		int num = UnityEngine.Random.Range(0, acs.Length);
		return acs[num];
	}

	// Token: 0x06000426 RID: 1062 RVA: 0x0001A324 File Offset: 0x00018524
	public static void PlayRandom(this Sound[] sds)
	{
		int num = UnityEngine.Random.Range(0, sds.Length);
		sds[num].Play();
	}

	// Token: 0x06000427 RID: 1063 RVA: 0x0001A343 File Offset: 0x00018543
	public static string GetString(this DataRow d, int num)
	{
		return d[num].ToString();
	}

	// Token: 0x06000428 RID: 1064 RVA: 0x0001A351 File Offset: 0x00018551
	public static int GetInt(this DataRow d, int num)
	{
		return int.Parse(d[num].ToString());
	}

	// Token: 0x06000429 RID: 1065 RVA: 0x0001A364 File Offset: 0x00018564
	public static float GetFloat(this DataRow d, int num)
	{
		return float.Parse(d[num].ToString());
	}

	// Token: 0x0600042A RID: 1066 RVA: 0x0001A377 File Offset: 0x00018577
	public static long GetLong(this DataRow d, int num)
	{
		return long.Parse(d[num].ToString());
	}

	// Token: 0x0600042B RID: 1067 RVA: 0x0001A38C File Offset: 0x0001858C
	public static void MyAddForce(this Rigidbody2D rb, float bodySize, Vector2 force)
	{
		float x = bodySize * 50f * 60f;
		float magnitude = force.magnitude;
		float d = math.min(x, magnitude);
		rb.AddForce(force.normalized * d);
	}

	// Token: 0x0600042C RID: 1068 RVA: 0x0001A3C8 File Offset: 0x000185C8
	public static string ToSmartString(this double num)
	{
		if (num >= 10000000000.0)
		{
			return num.ToString("#.##E+0");
		}
		return num.ToString("0.0");
	}

	// Token: 0x0600042D RID: 1069 RVA: 0x0001A3EF File Offset: 0x000185EF
	public static string ToSmartString_Percent(this double num)
	{
		if (num >= 1000000000.0)
		{
			return num.ToString("#.##E+0%");
		}
		return num.ToString("0%");
	}

	// Token: 0x0600042E RID: 1070 RVA: 0x0001A416 File Offset: 0x00018616
	public static string ToSmartString_Int(this double num)
	{
		if (num >= 10000000000.0)
		{
			return num.ToString("#.##E+0");
		}
		return num.ToString("0");
	}

	// Token: 0x0600042F RID: 1071 RVA: 0x0001A440 File Offset: 0x00018640
	public static SpriteRenderer SetMaterialAndIntensity(this SpriteRenderer renderer, float intensity, bool ifBullet, Material mat = null)
	{
		if (mat == null)
		{
			mat = ResourceLibrary.Inst.matGlowDefault;
		}
		renderer.material = mat;
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		materialPropertyBlock.SetFloat("_Intensity", intensity * ResourceLibrary.Inst.GetFloat_GlowMulti());
		if (ifBullet && Battle.inst.DIY_BulletAlpha_BoolFlag)
		{
			float diy_BulletAlpha_Float = Battle.inst.DIY_BulletAlpha_Float;
			materialPropertyBlock.SetFloat("_Alpha", diy_BulletAlpha_Float);
		}
		if (renderer.sprite == null)
		{
			return null;
		}
		materialPropertyBlock.SetTexture("_MainTex", renderer.sprite.texture);
		renderer.SetPropertyBlock(materialPropertyBlock);
		return renderer;
	}

	// Token: 0x06000430 RID: 1072 RVA: 0x0001A4DC File Offset: 0x000186DC
	public static ParticleSystemRenderer SetMaterialAndIntensity(this ParticleSystemRenderer renderer, float intensity, bool ifBullet, Material mat = null)
	{
		if (mat == null)
		{
			mat = ResourceLibrary.Inst.matGlowDefault;
		}
		renderer.material = mat;
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		materialPropertyBlock.SetFloat("_Intensity", intensity * ResourceLibrary.Inst.GetFloat_GlowMulti());
		if (ifBullet && Battle.inst.DIY_BulletAlpha_BoolFlag)
		{
			float diy_BulletAlpha_Float = Battle.inst.DIY_BulletAlpha_Float;
			materialPropertyBlock.SetFloat("_Alpha", diy_BulletAlpha_Float);
		}
		renderer.SetPropertyBlock(materialPropertyBlock);
		return renderer;
	}
}
