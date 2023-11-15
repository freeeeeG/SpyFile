using System;
using UnityEngine;

// Token: 0x02000521 RID: 1313
public class PowerUseTracker : WorldTracker
{
	// Token: 0x06001F77 RID: 8055 RVA: 0x000A802A File Offset: 0x000A622A
	public PowerUseTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06001F78 RID: 8056 RVA: 0x000A8034 File Offset: 0x000A6234
	public override void UpdateData()
	{
		float num = 0f;
		foreach (UtilityNetwork utilityNetwork in Game.Instance.electricalConduitSystem.GetNetworks())
		{
			ElectricalUtilityNetwork electricalUtilityNetwork = (ElectricalUtilityNetwork)utilityNetwork;
			if (electricalUtilityNetwork.allWires != null && electricalUtilityNetwork.allWires.Count != 0)
			{
				int num2 = Grid.PosToCell(electricalUtilityNetwork.allWires[0]);
				if ((int)Grid.WorldIdx[num2] == base.WorldID)
				{
					num += Game.Instance.circuitManager.GetWattsUsedByCircuit(Game.Instance.circuitManager.GetCircuitID(num2));
				}
			}
		}
		base.AddPoint(Mathf.Round(num));
	}

	// Token: 0x06001F79 RID: 8057 RVA: 0x000A80F4 File Offset: 0x000A62F4
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedWattage(value, GameUtil.WattageFormatterUnit.Automatic, true);
	}
}
