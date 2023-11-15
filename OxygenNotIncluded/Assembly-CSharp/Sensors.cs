using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000424 RID: 1060
[AddComponentMenu("KMonoBehaviour/scripts/Sensors")]
public class Sensors : KMonoBehaviour
{
	// Token: 0x0600165D RID: 5725 RVA: 0x00074FD9 File Offset: 0x000731D9
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<Brain>().onPreUpdate += this.OnBrainPreUpdate;
	}

	// Token: 0x0600165E RID: 5726 RVA: 0x00074FF8 File Offset: 0x000731F8
	public SensorType GetSensor<SensorType>() where SensorType : Sensor
	{
		foreach (Sensor sensor in this.sensors)
		{
			if (typeof(SensorType).IsAssignableFrom(sensor.GetType()))
			{
				return (SensorType)((object)sensor);
			}
		}
		global::Debug.LogError("Missing sensor of type: " + typeof(SensorType).Name);
		return default(SensorType);
	}

	// Token: 0x0600165F RID: 5727 RVA: 0x00075090 File Offset: 0x00073290
	public void Add(Sensor sensor)
	{
		this.sensors.Add(sensor);
		sensor.Update();
	}

	// Token: 0x06001660 RID: 5728 RVA: 0x000750A4 File Offset: 0x000732A4
	public void UpdateSensors()
	{
		foreach (Sensor sensor in this.sensors)
		{
			sensor.Update();
		}
	}

	// Token: 0x06001661 RID: 5729 RVA: 0x000750F4 File Offset: 0x000732F4
	private void OnBrainPreUpdate()
	{
		this.UpdateSensors();
	}

	// Token: 0x06001662 RID: 5730 RVA: 0x000750FC File Offset: 0x000732FC
	public void ShowEditor()
	{
		foreach (Sensor sensor in this.sensors)
		{
			sensor.ShowEditor();
		}
	}

	// Token: 0x04000C7B RID: 3195
	public List<Sensor> sensors = new List<Sensor>();
}
