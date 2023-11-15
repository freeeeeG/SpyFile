using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020000B0 RID: 176
public class BuffableTurret : MonoBehaviour
{
	// Token: 0x0600045C RID: 1116 RVA: 0x0000BE1A File Offset: 0x0000A01A
	private void Awake()
	{
		this.TurretContent = base.GetComponent<ConcreteContent>();
	}

	// Token: 0x0600045D RID: 1117 RVA: 0x0000BE28 File Offset: 0x0000A028
	public void TimeTick()
	{
		foreach (TurretBuff turretBuff in this.TurretBuffs.ToList<TurretBuff>())
		{
			turretBuff.Tick(Time.deltaTime);
			if (turretBuff.IsFinished)
			{
				this.TurretBuffs.Remove(turretBuff);
			}
		}
	}

	// Token: 0x0600045E RID: 1118 RVA: 0x0000BE9C File Offset: 0x0000A09C
	public void AddBuff(TurretBuffInfo buffInfo)
	{
		TurretBuff buff = TurretBuffFactory.GetBuff((int)buffInfo.BuffName);
		foreach (TurretBuff turretBuff in this.TurretBuffs)
		{
			if (turretBuff.TBuffName == buff.TBuffName)
			{
				turretBuff.ApplyBuff(this.TurretContent.Strategy, buffInfo.Stacks, buffInfo.Duration);
				return;
			}
		}
		buff.ApplyBuff(this.TurretContent.Strategy, buffInfo.Stacks, buffInfo.Duration);
		this.TurretBuffs.Add(buff);
	}

	// Token: 0x0600045F RID: 1119 RVA: 0x0000BF4C File Offset: 0x0000A14C
	public void ClearBuffs()
	{
		this.TurretBuffs.Clear();
	}

	// Token: 0x040001A0 RID: 416
	public List<TurretBuff> TurretBuffs = new List<TurretBuff>();

	// Token: 0x040001A1 RID: 417
	private ConcreteContent TurretContent;
}
