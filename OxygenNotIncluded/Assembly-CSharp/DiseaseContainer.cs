using System;
using UnityEngine;

// Token: 0x0200076C RID: 1900
public struct DiseaseContainer
{
	// Token: 0x0600347B RID: 13435 RVA: 0x00119D28 File Offset: 0x00117F28
	public DiseaseContainer(GameObject go, ushort elemIdx)
	{
		this.elemIdx = elemIdx;
		this.isContainer = (go.GetComponent<IUserControlledCapacity>() != null && go.GetComponent<Storage>() != null);
		Conduit component = go.GetComponent<Conduit>();
		if (component != null)
		{
			this.conduitType = component.type;
		}
		else
		{
			this.conduitType = ConduitType.None;
		}
		this.controller = go.GetComponent<KBatchedAnimController>();
		this.overpopulationCount = 1;
		this.instanceGrowthRate = 1f;
		this.accumulatedError = 0f;
		this.visualDiseaseProvider = null;
		this.autoDisinfectable = go.GetComponent<AutoDisinfectable>();
		if (this.autoDisinfectable != null)
		{
			AutoDisinfectableManager.Instance.AddAutoDisinfectable(this.autoDisinfectable);
		}
	}

	// Token: 0x0600347C RID: 13436 RVA: 0x00119DD8 File Offset: 0x00117FD8
	public void Clear()
	{
		this.controller = null;
	}

	// Token: 0x04001FB1 RID: 8113
	public AutoDisinfectable autoDisinfectable;

	// Token: 0x04001FB2 RID: 8114
	public ushort elemIdx;

	// Token: 0x04001FB3 RID: 8115
	public bool isContainer;

	// Token: 0x04001FB4 RID: 8116
	public ConduitType conduitType;

	// Token: 0x04001FB5 RID: 8117
	public KBatchedAnimController controller;

	// Token: 0x04001FB6 RID: 8118
	public GameObject visualDiseaseProvider;

	// Token: 0x04001FB7 RID: 8119
	public int overpopulationCount;

	// Token: 0x04001FB8 RID: 8120
	public float instanceGrowthRate;

	// Token: 0x04001FB9 RID: 8121
	public float accumulatedError;
}
