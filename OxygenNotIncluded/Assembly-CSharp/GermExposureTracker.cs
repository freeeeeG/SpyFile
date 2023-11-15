using System;
using System.Collections.Generic;
using KSerialization;
using ProcGen;
using UnityEngine;

// Token: 0x020007E0 RID: 2016
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/GermExposureTracker")]
public class GermExposureTracker : KMonoBehaviour
{
	// Token: 0x06003904 RID: 14596 RVA: 0x0013CDE0 File Offset: 0x0013AFE0
	protected override void OnPrefabInit()
	{
		global::Debug.Assert(GermExposureTracker.Instance == null);
		GermExposureTracker.Instance = this;
	}

	// Token: 0x06003905 RID: 14597 RVA: 0x0013CDF8 File Offset: 0x0013AFF8
	protected override void OnSpawn()
	{
		this.rng = new SeededRandom(GameClock.Instance.GetCycle());
	}

	// Token: 0x06003906 RID: 14598 RVA: 0x0013CE0F File Offset: 0x0013B00F
	protected override void OnForcedCleanUp()
	{
		GermExposureTracker.Instance = null;
	}

	// Token: 0x06003907 RID: 14599 RVA: 0x0013CE18 File Offset: 0x0013B018
	public void AddExposure(ExposureType exposure_type, float amount)
	{
		float num;
		this.accumulation.TryGetValue(exposure_type.germ_id, out num);
		float num2 = num + amount;
		if (num2 > 1f)
		{
			using (List<MinionIdentity>.Enumerator enumerator = Components.LiveMinionIdentities.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MinionIdentity cmp = enumerator.Current;
					GermExposureMonitor.Instance smi = cmp.GetSMI<GermExposureMonitor.Instance>();
					if (smi.GetExposureState(exposure_type.germ_id) == GermExposureMonitor.ExposureState.Exposed)
					{
						float exposureWeight = cmp.GetSMI<GermExposureMonitor.Instance>().GetExposureWeight(exposure_type.germ_id);
						if (exposureWeight > 0f)
						{
							this.exposure_candidates.Add(new GermExposureTracker.WeightedExposure
							{
								weight = exposureWeight,
								monitor = smi
							});
						}
					}
				}
				goto IL_F8;
			}
			IL_AF:
			num2 -= 1f;
			if (this.exposure_candidates.Count > 0)
			{
				GermExposureTracker.WeightedExposure weightedExposure = WeightedRandom.Choose<GermExposureTracker.WeightedExposure>(this.exposure_candidates, this.rng);
				this.exposure_candidates.Remove(weightedExposure);
				weightedExposure.monitor.ContractGerms(exposure_type.germ_id);
			}
			IL_F8:
			if (num2 > 1f)
			{
				goto IL_AF;
			}
		}
		this.accumulation[exposure_type.germ_id] = num2;
		this.exposure_candidates.Clear();
	}

	// Token: 0x040025C5 RID: 9669
	public static GermExposureTracker Instance;

	// Token: 0x040025C6 RID: 9670
	[Serialize]
	private Dictionary<HashedString, float> accumulation = new Dictionary<HashedString, float>();

	// Token: 0x040025C7 RID: 9671
	private SeededRandom rng;

	// Token: 0x040025C8 RID: 9672
	private List<GermExposureTracker.WeightedExposure> exposure_candidates = new List<GermExposureTracker.WeightedExposure>();

	// Token: 0x02001598 RID: 5528
	private class WeightedExposure : IWeighted
	{
		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x06008821 RID: 34849 RVA: 0x0030DE0A File Offset: 0x0030C00A
		// (set) Token: 0x06008822 RID: 34850 RVA: 0x0030DE12 File Offset: 0x0030C012
		public float weight { get; set; }

		// Token: 0x04006931 RID: 26929
		public GermExposureMonitor.Instance monitor;
	}
}
