using System;

// Token: 0x0200041C RID: 1052
public class BreathableAreaSensor : Sensor
{
	// Token: 0x0600163C RID: 5692 RVA: 0x00074A28 File Offset: 0x00072C28
	public BreathableAreaSensor(Sensors sensors) : base(sensors)
	{
	}

	// Token: 0x0600163D RID: 5693 RVA: 0x00074A34 File Offset: 0x00072C34
	public override void Update()
	{
		if (this.breather == null)
		{
			this.breather = base.GetComponent<OxygenBreather>();
		}
		bool flag = this.isBreathable;
		this.isBreathable = (this.breather.IsBreathableElement || this.breather.HasTag(GameTags.InTransitTube));
		if (this.isBreathable != flag)
		{
			if (this.isBreathable)
			{
				base.Trigger(99949694, null);
				return;
			}
			base.Trigger(-1189351068, null);
		}
	}

	// Token: 0x0600163E RID: 5694 RVA: 0x00074AB2 File Offset: 0x00072CB2
	public bool IsBreathable()
	{
		return this.isBreathable;
	}

	// Token: 0x0600163F RID: 5695 RVA: 0x00074ABA File Offset: 0x00072CBA
	public bool IsUnderwater()
	{
		return this.breather.IsUnderLiquid;
	}

	// Token: 0x04000C65 RID: 3173
	private bool isBreathable;

	// Token: 0x04000C66 RID: 3174
	private OxygenBreather breather;
}
