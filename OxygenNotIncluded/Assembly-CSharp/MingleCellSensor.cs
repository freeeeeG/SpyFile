using System;
using UnityEngine;

// Token: 0x0200041F RID: 1055
public class MingleCellSensor : Sensor
{
	// Token: 0x06001646 RID: 5702 RVA: 0x00074C46 File Offset: 0x00072E46
	public MingleCellSensor(Sensors sensors) : base(sensors)
	{
		this.navigator = base.GetComponent<Navigator>();
		this.brain = base.GetComponent<MinionBrain>();
	}

	// Token: 0x06001647 RID: 5703 RVA: 0x00074C68 File Offset: 0x00072E68
	public override void Update()
	{
		this.cell = Grid.InvalidCell;
		int num = int.MaxValue;
		ListPool<int, MingleCellSensor>.PooledList pooledList = ListPool<int, MingleCellSensor>.Allocate();
		int num2 = 50;
		foreach (int num3 in Game.Instance.mingleCellTracker.mingleCells)
		{
			if (this.brain.IsCellClear(num3))
			{
				int navigationCost = this.navigator.GetNavigationCost(num3);
				if (navigationCost != -1)
				{
					if (num3 == Grid.InvalidCell || navigationCost < num)
					{
						this.cell = num3;
						num = navigationCost;
					}
					if (navigationCost < num2)
					{
						pooledList.Add(num3);
					}
				}
			}
		}
		if (pooledList.Count > 0)
		{
			this.cell = pooledList[UnityEngine.Random.Range(0, pooledList.Count)];
		}
		pooledList.Recycle();
	}

	// Token: 0x06001648 RID: 5704 RVA: 0x00074D48 File Offset: 0x00072F48
	public int GetCell()
	{
		return this.cell;
	}

	// Token: 0x04000C6E RID: 3182
	private MinionBrain brain;

	// Token: 0x04000C6F RID: 3183
	private Navigator navigator;

	// Token: 0x04000C70 RID: 3184
	private int cell;
}
