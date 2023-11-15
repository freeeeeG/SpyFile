using System;

// Token: 0x02000421 RID: 1057
public class PickupableSensor : Sensor
{
	// Token: 0x0600164B RID: 5707 RVA: 0x00074D73 File Offset: 0x00072F73
	public PickupableSensor(Sensors sensors) : base(sensors)
	{
		this.worker = base.GetComponent<Worker>();
		this.pathProber = base.GetComponent<PathProber>();
	}

	// Token: 0x0600164C RID: 5708 RVA: 0x00074D94 File Offset: 0x00072F94
	public override void Update()
	{
		GlobalChoreProvider.Instance.UpdateFetches(this.pathProber);
		Game.Instance.fetchManager.UpdatePickups(this.pathProber, this.worker);
	}

	// Token: 0x04000C72 RID: 3186
	private PathProber pathProber;

	// Token: 0x04000C73 RID: 3187
	private Worker worker;
}
