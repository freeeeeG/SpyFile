using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200090D RID: 2317
public class FossilDigsiteLampLight : Light2D
{
	// Token: 0x170004A9 RID: 1193
	// (get) Token: 0x0600432C RID: 17196 RVA: 0x001782E0 File Offset: 0x001764E0
	// (set) Token: 0x0600432B RID: 17195 RVA: 0x001782D7 File Offset: 0x001764D7
	public bool independent { get; private set; }

	// Token: 0x0600432D RID: 17197 RVA: 0x001782E8 File Offset: 0x001764E8
	protected override void OnPrefabInit()
	{
		base.Subscribe<FossilDigsiteLampLight>(-592767678, FossilDigsiteLampLight.OnOperationalChangedDelegate);
		base.IntensityAnimation = 1f;
	}

	// Token: 0x0600432E RID: 17198 RVA: 0x00178308 File Offset: 0x00176508
	public void SetIndependentState(bool isIndependent, bool checkOperational = true)
	{
		this.independent = isIndependent;
		Operational component = base.GetComponent<Operational>();
		if (component != null && this.independent && checkOperational && base.enabled != component.IsOperational)
		{
			base.enabled = component.IsOperational;
		}
	}

	// Token: 0x0600432F RID: 17199 RVA: 0x00178353 File Offset: 0x00176553
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		if (this.independent || base.enabled)
		{
			return base.GetDescriptors(go);
		}
		return new List<Descriptor>();
	}

	// Token: 0x04002BC8 RID: 11208
	private static readonly EventSystem.IntraObjectHandler<FossilDigsiteLampLight> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<FossilDigsiteLampLight>(delegate(FossilDigsiteLampLight light, object data)
	{
		if (light.independent)
		{
			light.enabled = (bool)data;
		}
	});
}
