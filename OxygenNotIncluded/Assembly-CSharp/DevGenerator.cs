using System;
using UnityEngine;

// Token: 0x020005EA RID: 1514
public class DevGenerator : Generator
{
	// Token: 0x060025C5 RID: 9669 RVA: 0x000CD760 File Offset: 0x000CB960
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		ushort circuitID = base.CircuitID;
		this.operational.SetFlag(Generator.wireConnectedFlag, circuitID != ushort.MaxValue);
		if (!this.operational.IsOperational)
		{
			return;
		}
		float num = this.wattageRating;
		if (num > 0f)
		{
			num *= dt;
			num = Mathf.Max(num, 1f * dt);
			base.GenerateJoules(num, false);
		}
	}

	// Token: 0x04001590 RID: 5520
	public float wattageRating = 100000f;
}
