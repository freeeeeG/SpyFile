using System;

// Token: 0x02000420 RID: 1056
public class PathProberSensor : Sensor
{
	// Token: 0x06001649 RID: 5705 RVA: 0x00074D50 File Offset: 0x00072F50
	public PathProberSensor(Sensors sensors) : base(sensors)
	{
		this.navigator = sensors.GetComponent<Navigator>();
	}

	// Token: 0x0600164A RID: 5706 RVA: 0x00074D65 File Offset: 0x00072F65
	public override void Update()
	{
		this.navigator.UpdateProbe(false);
	}

	// Token: 0x04000C71 RID: 3185
	private Navigator navigator;
}
