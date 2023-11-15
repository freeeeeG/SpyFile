using System;
using Klei.AI;

// Token: 0x02000422 RID: 1058
public class SafeCellSensor : Sensor
{
	// Token: 0x0600164D RID: 5709 RVA: 0x00074DC4 File Offset: 0x00072FC4
	public SafeCellSensor(Sensors sensors) : base(sensors)
	{
		this.navigator = base.GetComponent<Navigator>();
		this.brain = base.GetComponent<MinionBrain>();
		this.prefabid = base.GetComponent<KPrefabID>();
		this.traits = base.GetComponent<Traits>();
	}

	// Token: 0x0600164E RID: 5710 RVA: 0x00074E14 File Offset: 0x00073014
	public override void Update()
	{
		if (!this.prefabid.HasTag(GameTags.Idle))
		{
			this.cell = Grid.InvalidCell;
			return;
		}
		bool flag = this.HasSafeCell();
		this.RunSafeCellQuery(false);
		bool flag2 = this.HasSafeCell();
		if (flag2 != flag)
		{
			if (flag2)
			{
				this.sensors.Trigger(982561777, null);
				return;
			}
			this.sensors.Trigger(506919987, null);
		}
	}

	// Token: 0x0600164F RID: 5711 RVA: 0x00074E80 File Offset: 0x00073080
	public void RunSafeCellQuery(bool avoid_light)
	{
		MinionPathFinderAbilities minionPathFinderAbilities = (MinionPathFinderAbilities)this.navigator.GetCurrentAbilities();
		minionPathFinderAbilities.SetIdleNavMaskEnabled(true);
		SafeCellQuery safeCellQuery = PathFinderQueries.safeCellQuery.Reset(this.brain, avoid_light);
		this.navigator.RunQuery(safeCellQuery);
		minionPathFinderAbilities.SetIdleNavMaskEnabled(false);
		this.cell = safeCellQuery.GetResultCell();
		if (this.cell == Grid.PosToCell(this.navigator))
		{
			this.cell = Grid.InvalidCell;
		}
	}

	// Token: 0x06001650 RID: 5712 RVA: 0x00074EF2 File Offset: 0x000730F2
	public int GetSensorCell()
	{
		return this.cell;
	}

	// Token: 0x06001651 RID: 5713 RVA: 0x00074EFA File Offset: 0x000730FA
	public int GetCellQuery()
	{
		if (this.cell == Grid.InvalidCell)
		{
			this.RunSafeCellQuery(false);
		}
		return this.cell;
	}

	// Token: 0x06001652 RID: 5714 RVA: 0x00074F16 File Offset: 0x00073116
	public int GetSleepCellQuery()
	{
		if (this.cell == Grid.InvalidCell)
		{
			this.RunSafeCellQuery(!this.traits.HasTrait("NightLight"));
		}
		return this.cell;
	}

	// Token: 0x06001653 RID: 5715 RVA: 0x00074F47 File Offset: 0x00073147
	public bool HasSafeCell()
	{
		return this.cell != Grid.InvalidCell && this.cell != Grid.PosToCell(this.sensors);
	}

	// Token: 0x04000C74 RID: 3188
	private MinionBrain brain;

	// Token: 0x04000C75 RID: 3189
	private Navigator navigator;

	// Token: 0x04000C76 RID: 3190
	private KPrefabID prefabid;

	// Token: 0x04000C77 RID: 3191
	private Traits traits;

	// Token: 0x04000C78 RID: 3192
	private int cell = Grid.InvalidCell;
}
