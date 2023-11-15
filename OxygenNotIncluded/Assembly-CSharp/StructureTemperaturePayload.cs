using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009F3 RID: 2547
public struct StructureTemperaturePayload
{
	// Token: 0x170005A1 RID: 1441
	// (get) Token: 0x06004C04 RID: 19460 RVA: 0x001AA449 File Offset: 0x001A8649
	// (set) Token: 0x06004C05 RID: 19461 RVA: 0x001AA451 File Offset: 0x001A8651
	public PrimaryElement primaryElement
	{
		get
		{
			return this.primaryElementBacking;
		}
		set
		{
			if (this.primaryElementBacking != value)
			{
				this.primaryElementBacking = value;
				this.overheatable = this.primaryElementBacking.GetComponent<Overheatable>();
			}
		}
	}

	// Token: 0x06004C06 RID: 19462 RVA: 0x001AA47C File Offset: 0x001A867C
	public StructureTemperaturePayload(GameObject go)
	{
		this.simHandleCopy = -1;
		this.enabled = true;
		this.bypass = false;
		this.overrideExtents = false;
		this.overriddenExtents = default(Extents);
		this.primaryElementBacking = go.GetComponent<PrimaryElement>();
		this.overheatable = ((this.primaryElementBacking != null) ? this.primaryElementBacking.GetComponent<Overheatable>() : null);
		this.building = go.GetComponent<Building>();
		this.operational = go.GetComponent<Operational>();
		this.pendingEnergyModifications = 0f;
		this.maxTemperature = 10000f;
		this.energySourcesKW = null;
		this.isActiveStatusItemSet = false;
	}

	// Token: 0x170005A2 RID: 1442
	// (get) Token: 0x06004C07 RID: 19463 RVA: 0x001AA51C File Offset: 0x001A871C
	public float TotalEnergyProducedKW
	{
		get
		{
			if (this.energySourcesKW == null || this.energySourcesKW.Count == 0)
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 0; i < this.energySourcesKW.Count; i++)
			{
				num += this.energySourcesKW[i].value;
			}
			return num;
		}
	}

	// Token: 0x06004C08 RID: 19464 RVA: 0x001AA575 File Offset: 0x001A8775
	public void OverrideExtents(Extents newExtents)
	{
		this.overrideExtents = true;
		this.overriddenExtents = newExtents;
	}

	// Token: 0x06004C09 RID: 19465 RVA: 0x001AA585 File Offset: 0x001A8785
	public Extents GetExtents()
	{
		if (!this.overrideExtents)
		{
			return this.building.GetExtents();
		}
		return this.overriddenExtents;
	}

	// Token: 0x170005A3 RID: 1443
	// (get) Token: 0x06004C0A RID: 19466 RVA: 0x001AA5A1 File Offset: 0x001A87A1
	public float Temperature
	{
		get
		{
			return this.primaryElement.Temperature;
		}
	}

	// Token: 0x170005A4 RID: 1444
	// (get) Token: 0x06004C0B RID: 19467 RVA: 0x001AA5AE File Offset: 0x001A87AE
	public float ExhaustKilowatts
	{
		get
		{
			return this.building.Def.ExhaustKilowattsWhenActive;
		}
	}

	// Token: 0x170005A5 RID: 1445
	// (get) Token: 0x06004C0C RID: 19468 RVA: 0x001AA5C0 File Offset: 0x001A87C0
	public float OperatingKilowatts
	{
		get
		{
			if (!(this.operational != null) || !this.operational.IsActive)
			{
				return 0f;
			}
			return this.building.Def.SelfHeatKilowattsWhenActive;
		}
	}

	// Token: 0x040031A1 RID: 12705
	public int simHandleCopy;

	// Token: 0x040031A2 RID: 12706
	public bool enabled;

	// Token: 0x040031A3 RID: 12707
	public bool bypass;

	// Token: 0x040031A4 RID: 12708
	public bool isActiveStatusItemSet;

	// Token: 0x040031A5 RID: 12709
	public bool overrideExtents;

	// Token: 0x040031A6 RID: 12710
	private PrimaryElement primaryElementBacking;

	// Token: 0x040031A7 RID: 12711
	public Overheatable overheatable;

	// Token: 0x040031A8 RID: 12712
	public Building building;

	// Token: 0x040031A9 RID: 12713
	public Operational operational;

	// Token: 0x040031AA RID: 12714
	public List<StructureTemperaturePayload.EnergySource> energySourcesKW;

	// Token: 0x040031AB RID: 12715
	public float pendingEnergyModifications;

	// Token: 0x040031AC RID: 12716
	public float maxTemperature;

	// Token: 0x040031AD RID: 12717
	public Extents overriddenExtents;

	// Token: 0x02001876 RID: 6262
	public class EnergySource
	{
		// Token: 0x060091D0 RID: 37328 RVA: 0x0032A895 File Offset: 0x00328A95
		public EnergySource(float kj, string source)
		{
			this.source = source;
			this.kw_accumulator = new RunningAverage(float.MinValue, float.MaxValue, Mathf.RoundToInt(186f), true);
		}

		// Token: 0x170009A6 RID: 2470
		// (get) Token: 0x060091D1 RID: 37329 RVA: 0x0032A8C4 File Offset: 0x00328AC4
		public float value
		{
			get
			{
				return this.kw_accumulator.AverageValue;
			}
		}

		// Token: 0x060091D2 RID: 37330 RVA: 0x0032A8D1 File Offset: 0x00328AD1
		public void Accumulate(float value)
		{
			this.kw_accumulator.AddSample(value);
		}

		// Token: 0x04007218 RID: 29208
		public string source;

		// Token: 0x04007219 RID: 29209
		public RunningAverage kw_accumulator;
	}
}
