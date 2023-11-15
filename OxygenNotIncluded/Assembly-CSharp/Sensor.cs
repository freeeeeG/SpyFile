using System;
using UnityEngine;

// Token: 0x02000423 RID: 1059
public class Sensor
{
	// Token: 0x17000099 RID: 153
	// (get) Token: 0x06001654 RID: 5716 RVA: 0x00074F6E File Offset: 0x0007316E
	// (set) Token: 0x06001655 RID: 5717 RVA: 0x00074F76 File Offset: 0x00073176
	public string Name { get; private set; }

	// Token: 0x06001656 RID: 5718 RVA: 0x00074F7F File Offset: 0x0007317F
	public Sensor(Sensors sensors)
	{
		this.sensors = sensors;
		this.Name = base.GetType().Name;
	}

	// Token: 0x06001657 RID: 5719 RVA: 0x00074F9F File Offset: 0x0007319F
	public ComponentType GetComponent<ComponentType>()
	{
		return this.sensors.GetComponent<ComponentType>();
	}

	// Token: 0x1700009A RID: 154
	// (get) Token: 0x06001658 RID: 5720 RVA: 0x00074FAC File Offset: 0x000731AC
	public GameObject gameObject
	{
		get
		{
			return this.sensors.gameObject;
		}
	}

	// Token: 0x1700009B RID: 155
	// (get) Token: 0x06001659 RID: 5721 RVA: 0x00074FB9 File Offset: 0x000731B9
	public Transform transform
	{
		get
		{
			return this.gameObject.transform;
		}
	}

	// Token: 0x0600165A RID: 5722 RVA: 0x00074FC6 File Offset: 0x000731C6
	public void Trigger(int hash, object data = null)
	{
		this.sensors.Trigger(hash, data);
	}

	// Token: 0x0600165B RID: 5723 RVA: 0x00074FD5 File Offset: 0x000731D5
	public virtual void Update()
	{
	}

	// Token: 0x0600165C RID: 5724 RVA: 0x00074FD7 File Offset: 0x000731D7
	public virtual void ShowEditor()
	{
	}

	// Token: 0x04000C79 RID: 3193
	protected Sensors sensors;
}
