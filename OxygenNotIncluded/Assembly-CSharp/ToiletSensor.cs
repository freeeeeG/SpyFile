using System;

// Token: 0x02000425 RID: 1061
public class ToiletSensor : Sensor
{
	// Token: 0x06001664 RID: 5732 RVA: 0x0007515F File Offset: 0x0007335F
	public ToiletSensor(Sensors sensors) : base(sensors)
	{
		this.navigator = base.GetComponent<Navigator>();
	}

	// Token: 0x06001665 RID: 5733 RVA: 0x00075174 File Offset: 0x00073374
	public override void Update()
	{
		IUsable usable = null;
		int num = int.MaxValue;
		bool flag = false;
		foreach (IUsable usable2 in Components.Toilets.Items)
		{
			if (usable2.IsUsable())
			{
				flag = true;
				int navigationCost = this.navigator.GetNavigationCost(Grid.PosToCell(usable2.transform.GetPosition()));
				if (navigationCost != -1 && navigationCost < num)
				{
					usable = usable2;
					num = navigationCost;
				}
			}
		}
		bool flag2 = Components.Toilets.Count > 0;
		if (usable != this.toilet || flag2 != this.areThereAnyToilets || this.areThereAnyUsableToilets != flag)
		{
			this.toilet = usable;
			this.areThereAnyToilets = flag2;
			this.areThereAnyUsableToilets = flag;
			base.Trigger(-752545459, null);
		}
	}

	// Token: 0x06001666 RID: 5734 RVA: 0x00075254 File Offset: 0x00073454
	public bool AreThereAnyToilets()
	{
		return this.areThereAnyToilets;
	}

	// Token: 0x06001667 RID: 5735 RVA: 0x0007525C File Offset: 0x0007345C
	public bool AreThereAnyUsableToilets()
	{
		return this.areThereAnyUsableToilets;
	}

	// Token: 0x06001668 RID: 5736 RVA: 0x00075264 File Offset: 0x00073464
	public IUsable GetNearestUsableToilet()
	{
		return this.toilet;
	}

	// Token: 0x04000C7C RID: 3196
	private Navigator navigator;

	// Token: 0x04000C7D RID: 3197
	private IUsable toilet;

	// Token: 0x04000C7E RID: 3198
	private bool areThereAnyToilets;

	// Token: 0x04000C7F RID: 3199
	private bool areThereAnyUsableToilets;
}
