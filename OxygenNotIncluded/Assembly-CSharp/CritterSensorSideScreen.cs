using System;
using UnityEngine;

// Token: 0x02000C12 RID: 3090
public class CritterSensorSideScreen : SideScreenContent
{
	// Token: 0x060061DC RID: 25052 RVA: 0x0024217E File Offset: 0x0024037E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.countCrittersToggle.onClick += this.ToggleCritters;
		this.countEggsToggle.onClick += this.ToggleEggs;
	}

	// Token: 0x060061DD RID: 25053 RVA: 0x002421B4 File Offset: 0x002403B4
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LogicCritterCountSensor>() != null;
	}

	// Token: 0x060061DE RID: 25054 RVA: 0x002421C4 File Offset: 0x002403C4
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetSensor = target.GetComponent<LogicCritterCountSensor>();
		this.crittersCheckmark.enabled = this.targetSensor.countCritters;
		this.eggsCheckmark.enabled = this.targetSensor.countEggs;
	}

	// Token: 0x060061DF RID: 25055 RVA: 0x00242210 File Offset: 0x00240410
	private void ToggleCritters()
	{
		this.targetSensor.countCritters = !this.targetSensor.countCritters;
		this.crittersCheckmark.enabled = this.targetSensor.countCritters;
	}

	// Token: 0x060061E0 RID: 25056 RVA: 0x00242241 File Offset: 0x00240441
	private void ToggleEggs()
	{
		this.targetSensor.countEggs = !this.targetSensor.countEggs;
		this.eggsCheckmark.enabled = this.targetSensor.countEggs;
	}

	// Token: 0x040042AD RID: 17069
	public LogicCritterCountSensor targetSensor;

	// Token: 0x040042AE RID: 17070
	public KToggle countCrittersToggle;

	// Token: 0x040042AF RID: 17071
	public KToggle countEggsToggle;

	// Token: 0x040042B0 RID: 17072
	public KImage crittersCheckmark;

	// Token: 0x040042B1 RID: 17073
	public KImage eggsCheckmark;
}
