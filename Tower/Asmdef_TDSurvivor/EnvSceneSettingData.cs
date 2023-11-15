using System;
using UnityEngine;

// Token: 0x02000026 RID: 38
[CreateAssetMenu(fileName = "EnvSceneSettingData", menuName = "設定檔/場景環境設定資料 (EnvSceneSettingData)", order = 1)]
[Serializable]
public class EnvSceneSettingData : ScriptableObject
{
	// Token: 0x17000005 RID: 5
	// (get) Token: 0x060000B6 RID: 182 RVA: 0x0000475C File Offset: 0x0000295C
	public Gradient Gradient_LightColor
	{
		get
		{
			return this.gradient_LightColor;
		}
	}

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x060000B7 RID: 183 RVA: 0x00004764 File Offset: 0x00002964
	public Gradient Gradient_LightColor_FirstDay
	{
		get
		{
			return this.gradient_LightColor_FirstDay;
		}
	}

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x060000B8 RID: 184 RVA: 0x0000476C File Offset: 0x0000296C
	public Gradient Gradient_LightColor_FastForward
	{
		get
		{
			return this.gradient_LightColor_FastForward;
		}
	}

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x060000B9 RID: 185 RVA: 0x00004774 File Offset: 0x00002974
	public AnimationCurve Curve_LightIntensity
	{
		get
		{
			return this.curve_LightIntensity;
		}
	}

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x060000BA RID: 186 RVA: 0x0000477C File Offset: 0x0000297C
	public Vector3 LightOrientation_Start
	{
		get
		{
			return this.lightOrientation_Start;
		}
	}

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x060000BB RID: 187 RVA: 0x00004784 File Offset: 0x00002984
	public Vector3 LightOrientation_End
	{
		get
		{
			return this.lightOrientation_End;
		}
	}

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x060000BC RID: 188 RVA: 0x0000478C File Offset: 0x0000298C
	public AnimationCurve Curve_ShadowStrength
	{
		get
		{
			return this.curve_ShadowStrength;
		}
	}

	// Token: 0x04000085 RID: 133
	[SerializeField]
	private Gradient gradient_LightColor;

	// Token: 0x04000086 RID: 134
	[SerializeField]
	private Gradient gradient_LightColor_FirstDay;

	// Token: 0x04000087 RID: 135
	[SerializeField]
	private Gradient gradient_LightColor_FastForward;

	// Token: 0x04000088 RID: 136
	[SerializeField]
	private AnimationCurve curve_LightIntensity;

	// Token: 0x04000089 RID: 137
	[SerializeField]
	private Vector3 lightOrientation_Start;

	// Token: 0x0400008A RID: 138
	[SerializeField]
	private Vector3 lightOrientation_End;

	// Token: 0x0400008B RID: 139
	[SerializeField]
	private AnimationCurve curve_ShadowStrength;
}
