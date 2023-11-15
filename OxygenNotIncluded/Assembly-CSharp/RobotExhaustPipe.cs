using System;
using UnityEngine;

// Token: 0x0200093C RID: 2364
[AddComponentMenu("KMonoBehaviour/scripts/RobotExhaustPipe")]
public class RobotExhaustPipe : KMonoBehaviour, ISim4000ms
{
	// Token: 0x060044B3 RID: 17587 RVA: 0x001828DC File Offset: 0x00180ADC
	public void Sim4000ms(float dt)
	{
		Facing component = base.GetComponent<Facing>();
		bool flip = false;
		if (component)
		{
			flip = component.GetFacing();
		}
		CO2Manager.instance.SpawnBreath(Grid.CellToPos(Grid.PosToCell(base.gameObject)), dt * this.CO2_RATE, 303.15f, flip);
	}

	// Token: 0x04002D7A RID: 11642
	private float CO2_RATE = 0.001f;
}
