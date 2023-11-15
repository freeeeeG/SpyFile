using System;
using UnityEngine;

// Token: 0x020003D5 RID: 981
[RequireComponent(typeof(Light))]
public class MaterialColourLight : LightModifier
{
	// Token: 0x06001221 RID: 4641 RVA: 0x00066F36 File Offset: 0x00065336
	protected override void Awake()
	{
		base.Awake();
		if (this.m_renderer != null)
		{
			this.m_material = this.m_renderer.material;
		}
	}

	// Token: 0x06001222 RID: 4642 RVA: 0x00066F60 File Offset: 0x00065360
	protected override void ModifyLight(Light _light)
	{
		_light.color = this.m_material.color;
	}

	// Token: 0x04000E38 RID: 3640
	[SerializeField]
	private Renderer m_renderer;

	// Token: 0x04000E39 RID: 3641
	private Material m_material;
}
