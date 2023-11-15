using System;
using UnityEngine;

// Token: 0x0200041E RID: 1054
public class IdleCellSensor : Sensor
{
	// Token: 0x06001643 RID: 5699 RVA: 0x00074B95 File Offset: 0x00072D95
	public IdleCellSensor(Sensors sensors) : base(sensors)
	{
		this.navigator = base.GetComponent<Navigator>();
		this.brain = base.GetComponent<MinionBrain>();
		this.prefabid = base.GetComponent<KPrefabID>();
	}

	// Token: 0x06001644 RID: 5700 RVA: 0x00074BC4 File Offset: 0x00072DC4
	public override void Update()
	{
		if (!this.prefabid.HasTag(GameTags.Idle))
		{
			this.cell = Grid.InvalidCell;
			return;
		}
		MinionPathFinderAbilities minionPathFinderAbilities = (MinionPathFinderAbilities)this.navigator.GetCurrentAbilities();
		minionPathFinderAbilities.SetIdleNavMaskEnabled(true);
		IdleCellQuery idleCellQuery = PathFinderQueries.idleCellQuery.Reset(this.brain, UnityEngine.Random.Range(30, 60));
		this.navigator.RunQuery(idleCellQuery);
		minionPathFinderAbilities.SetIdleNavMaskEnabled(false);
		this.cell = idleCellQuery.GetResultCell();
	}

	// Token: 0x06001645 RID: 5701 RVA: 0x00074C3E File Offset: 0x00072E3E
	public int GetCell()
	{
		return this.cell;
	}

	// Token: 0x04000C6A RID: 3178
	private MinionBrain brain;

	// Token: 0x04000C6B RID: 3179
	private Navigator navigator;

	// Token: 0x04000C6C RID: 3180
	private KPrefabID prefabid;

	// Token: 0x04000C6D RID: 3181
	private int cell;
}
