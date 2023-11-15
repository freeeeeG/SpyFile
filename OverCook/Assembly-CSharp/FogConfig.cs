using System;
using UnityEngine;

// Token: 0x02000471 RID: 1137
public class FogConfig : MonoBehaviour
{
	// Token: 0x06001522 RID: 5410 RVA: 0x00072EFE File Offset: 0x000712FE
	private void OnEnable()
	{
		this.ForceUpdate();
	}

	// Token: 0x06001523 RID: 5411 RVA: 0x00072F08 File Offset: 0x00071308
	public void ForceUpdate()
	{
		FogConfig.Kind fogKind = this.m_fogKind;
		if (fogKind != FogConfig.Kind.Distance)
		{
			if (fogKind == FogConfig.Kind.Height)
			{
				Shader.SetGlobalFloat("_Overcooked2FogOffset", -this.m_fogOffset);
				Shader.SetGlobalFloat("_Overcooked2FogNear", this.m_fogNear);
				Shader.SetGlobalFloat("_Overcooked2FogFar", this.m_fogFar);
				Shader.SetGlobalColor("_Overcooked2FogColour", this.m_fogColour);
			}
		}
		else
		{
			Shader.SetGlobalFloat("_Overcooked2FogNear", this.m_fogNear);
			Shader.SetGlobalFloat("_Overcooked2FogFar", this.m_fogFar);
			Shader.SetGlobalColor("_Overcooked2FogColour", this.m_fogColour);
		}
	}

	// Token: 0x06001524 RID: 5412 RVA: 0x00072FA9 File Offset: 0x000713A9
	private void OnDisable()
	{
		Shader.SetGlobalFloat("_Overcooked2FogOffset", 0f);
		Shader.SetGlobalFloat("_Overcooked2FogNear", 0f);
		Shader.SetGlobalFloat("_Overcooked2FogFar", 0f);
		Shader.SetGlobalColor("_Overcooked2FogColour", Color.black);
	}

	// Token: 0x0400103D RID: 4157
	[SerializeField]
	public FogConfig.Kind m_fogKind;

	// Token: 0x0400103E RID: 4158
	[SerializeField]
	public float m_fogOffset;

	// Token: 0x0400103F RID: 4159
	[SerializeField]
	public float m_fogNear;

	// Token: 0x04001040 RID: 4160
	[SerializeField]
	public float m_fogFar = 100f;

	// Token: 0x04001041 RID: 4161
	[SerializeField]
	public Color m_fogColour = Color.grey;

	// Token: 0x02000472 RID: 1138
	public enum Kind
	{
		// Token: 0x04001043 RID: 4163
		Distance,
		// Token: 0x04001044 RID: 4164
		Height
	}
}
