using System;
using UnityEngine;

// Token: 0x0200051E RID: 1310
public class BreathabilityTracker : WorldTracker
{
	// Token: 0x06001F6C RID: 8044 RVA: 0x000A7EAF File Offset: 0x000A60AF
	public BreathabilityTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06001F6D RID: 8045 RVA: 0x000A7EB8 File Offset: 0x000A60B8
	public override void UpdateData()
	{
		float num = 0f;
		int count = Components.LiveMinionIdentities.GetWorldItems(base.WorldID, false).Count;
		if (count == 0)
		{
			base.AddPoint(0f);
			return;
		}
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.GetWorldItems(base.WorldID, false))
		{
			OxygenBreather component = minionIdentity.GetComponent<OxygenBreather>();
			OxygenBreather.IGasProvider gasProvider = component.GetGasProvider();
			if (!component.IsSuffocating)
			{
				num += 100f;
				if (gasProvider.IsLowOxygen())
				{
					num -= 50f;
				}
			}
		}
		num /= (float)count;
		base.AddPoint((float)Mathf.RoundToInt(num));
	}

	// Token: 0x06001F6E RID: 8046 RVA: 0x000A7F78 File Offset: 0x000A6178
	public override string FormatValueString(float value)
	{
		return value.ToString() + "%";
	}
}
