using System;
using UnityEngine;

// Token: 0x0200095A RID: 2394
[AddComponentMenu("KMonoBehaviour/scripts/Schedulable")]
public class Schedulable : KMonoBehaviour
{
	// Token: 0x06004651 RID: 18001 RVA: 0x0018DD98 File Offset: 0x0018BF98
	public Schedule GetSchedule()
	{
		return ScheduleManager.Instance.GetSchedule(this);
	}

	// Token: 0x06004652 RID: 18002 RVA: 0x0018DDA8 File Offset: 0x0018BFA8
	public bool IsAllowed(ScheduleBlockType schedule_block_type)
	{
		WorldContainer myWorld = base.gameObject.GetMyWorld();
		if (myWorld == null)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				string.Format("Trying to schedule {0} but {1} is not on a valid world. Grid cell: {2}", schedule_block_type.Id, base.gameObject.name, Grid.PosToCell(base.gameObject.GetComponent<KPrefabID>()))
			});
			return false;
		}
		return myWorld.AlertManager.IsRedAlert() || ScheduleManager.Instance.IsAllowed(this, schedule_block_type);
	}

	// Token: 0x06004653 RID: 18003 RVA: 0x0018DE25 File Offset: 0x0018C025
	public void OnScheduleChanged(Schedule schedule)
	{
		base.Trigger(467134493, schedule);
	}

	// Token: 0x06004654 RID: 18004 RVA: 0x0018DE33 File Offset: 0x0018C033
	public void OnScheduleBlocksTick(Schedule schedule)
	{
		base.Trigger(1714332666, schedule);
	}

	// Token: 0x06004655 RID: 18005 RVA: 0x0018DE41 File Offset: 0x0018C041
	public void OnScheduleBlocksChanged(Schedule schedule)
	{
		base.Trigger(-894023145, schedule);
	}
}
