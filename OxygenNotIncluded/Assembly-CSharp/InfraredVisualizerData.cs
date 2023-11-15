using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020004BD RID: 1213
public struct InfraredVisualizerData
{
	// Token: 0x06001B8D RID: 7053 RVA: 0x0009377C File Offset: 0x0009197C
	public void Update()
	{
		float num = 0f;
		if (this.temperatureAmount != null)
		{
			num = this.temperatureAmount.value;
		}
		else if (this.structureTemperature.IsValid())
		{
			num = GameComps.StructureTemperatures.GetPayload(this.structureTemperature).Temperature;
		}
		else if (this.primaryElement != null)
		{
			num = this.primaryElement.Temperature;
		}
		else if (this.temperatureVulnerable != null)
		{
			num = this.temperatureVulnerable.InternalTemperature;
		}
		if (num < 0f)
		{
			return;
		}
		Color32 c = SimDebugView.Instance.NormalizedTemperature(num);
		this.controller.OverlayColour = c;
	}

	// Token: 0x06001B8E RID: 7054 RVA: 0x00093830 File Offset: 0x00091A30
	public InfraredVisualizerData(GameObject go)
	{
		this.controller = go.GetComponent<KBatchedAnimController>();
		if (this.controller != null)
		{
			this.temperatureAmount = Db.Get().Amounts.Temperature.Lookup(go);
			this.structureTemperature = GameComps.StructureTemperatures.GetHandle(go);
			this.primaryElement = go.GetComponent<PrimaryElement>();
			this.temperatureVulnerable = go.GetComponent<TemperatureVulnerable>();
			return;
		}
		this.temperatureAmount = null;
		this.structureTemperature = HandleVector<int>.InvalidHandle;
		this.primaryElement = null;
		this.temperatureVulnerable = null;
	}

	// Token: 0x04000F53 RID: 3923
	public KAnimControllerBase controller;

	// Token: 0x04000F54 RID: 3924
	public AmountInstance temperatureAmount;

	// Token: 0x04000F55 RID: 3925
	public HandleVector<int>.Handle structureTemperature;

	// Token: 0x04000F56 RID: 3926
	public PrimaryElement primaryElement;

	// Token: 0x04000F57 RID: 3927
	public TemperatureVulnerable temperatureVulnerable;
}
