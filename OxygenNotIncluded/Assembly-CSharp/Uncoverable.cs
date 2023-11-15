using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000538 RID: 1336
[AddComponentMenu("KMonoBehaviour/scripts/Uncoverable")]
public class Uncoverable : KMonoBehaviour
{
	// Token: 0x1700015C RID: 348
	// (get) Token: 0x06001FF1 RID: 8177 RVA: 0x000AB549 File Offset: 0x000A9749
	public bool IsUncovered
	{
		get
		{
			return this.hasBeenUncovered;
		}
	}

	// Token: 0x06001FF2 RID: 8178 RVA: 0x000AB554 File Offset: 0x000A9754
	private bool IsAnyCellShowing()
	{
		int rootCell = Grid.PosToCell(this);
		return !this.occupyArea.TestArea(rootCell, null, Uncoverable.IsCellBlockedDelegate);
	}

	// Token: 0x06001FF3 RID: 8179 RVA: 0x000AB57D File Offset: 0x000A977D
	private static bool IsCellBlocked(int cell, object data)
	{
		return Grid.Element[cell].IsSolid && !Grid.Foundation[cell];
	}

	// Token: 0x06001FF4 RID: 8180 RVA: 0x000AB59D File Offset: 0x000A979D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06001FF5 RID: 8181 RVA: 0x000AB5A8 File Offset: 0x000A97A8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.IsAnyCellShowing())
		{
			this.hasBeenUncovered = true;
		}
		if (!this.hasBeenUncovered)
		{
			base.GetComponent<KSelectable>().IsSelectable = false;
			Extents extents = this.occupyArea.GetExtents();
			this.partitionerEntry = GameScenePartitioner.Instance.Add("Uncoverable.OnSpawn", base.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
		}
	}

	// Token: 0x06001FF6 RID: 8182 RVA: 0x000AB61C File Offset: 0x000A981C
	private void OnSolidChanged(object data)
	{
		if (this.IsAnyCellShowing() && !this.hasBeenUncovered && this.partitionerEntry.IsValid())
		{
			GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
			this.hasBeenUncovered = true;
			base.GetComponent<KSelectable>().IsSelectable = true;
			Notification notification = new Notification(MISC.STATUSITEMS.BURIEDITEM.NOTIFICATION, NotificationType.Good, new Func<List<Notification>, object, string>(Uncoverable.OnNotificationToolTip), this, true, 0f, null, null, null, true, false, false);
			base.gameObject.AddOrGet<Notifier>().Add(notification, "");
		}
	}

	// Token: 0x06001FF7 RID: 8183 RVA: 0x000AB6AC File Offset: 0x000A98AC
	private static string OnNotificationToolTip(List<Notification> notifications, object data)
	{
		Uncoverable cmp = (Uncoverable)data;
		return MISC.STATUSITEMS.BURIEDITEM.NOTIFICATION_TOOLTIP.Replace("{Uncoverable}", cmp.GetProperName());
	}

	// Token: 0x06001FF8 RID: 8184 RVA: 0x000AB6D5 File Offset: 0x000A98D5
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x040011D4 RID: 4564
	[MyCmpReq]
	private OccupyArea occupyArea;

	// Token: 0x040011D5 RID: 4565
	[Serialize]
	private bool hasBeenUncovered;

	// Token: 0x040011D6 RID: 4566
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x040011D7 RID: 4567
	private static readonly Func<int, object, bool> IsCellBlockedDelegate = (int cell, object data) => Uncoverable.IsCellBlocked(cell, data);
}
