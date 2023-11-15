using System;
using UnityEngine;

// Token: 0x02000522 RID: 1314
public class BatteryTracker : WorldTracker
{
	// Token: 0x06001F7A RID: 8058 RVA: 0x000A80FE File Offset: 0x000A62FE
	public BatteryTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06001F7B RID: 8059 RVA: 0x000A8108 File Offset: 0x000A6308
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
					ushort circuitID = Game.Instance.circuitManager.GetCircuitID(num2);
					foreach (Battery battery in Game.Instance.circuitManager.GetBatteriesOnCircuit(circuitID))
					{
						num += battery.JoulesAvailable;
					}
				}
			}
		}
		base.AddPoint(Mathf.Round(num));
	}

	// Token: 0x06001F7C RID: 8060 RVA: 0x000A8214 File Offset: 0x000A6414
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedJoules(value, "F1", GameUtil.TimeSlice.None);
	}
}
