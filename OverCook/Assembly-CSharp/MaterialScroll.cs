using System;
using UnityEngine;

// Token: 0x02000A28 RID: 2600
[Serializable]
public class MaterialScroll
{
	// Token: 0x0600337F RID: 13183 RVA: 0x000F2738 File Offset: 0x000F0B38
	public void SetMaterialScrollToZero()
	{
		this.m_material.SetFloat("_ScrollSpeed", 0f);
	}

	// Token: 0x06003380 RID: 13184 RVA: 0x000F274F File Offset: 0x000F0B4F
	public void SetMaterialScrollToValue()
	{
		this.m_material.SetFloat("_ScrollSpeed", this.m_value);
	}

	// Token: 0x04002973 RID: 10611
	[SerializeField]
	private Material m_material;

	// Token: 0x04002974 RID: 10612
	[SerializeField]
	private float m_value;
}
