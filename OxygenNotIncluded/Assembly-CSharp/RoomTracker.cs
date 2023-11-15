using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020004F6 RID: 1270
[AddComponentMenu("KMonoBehaviour/scripts/RoomTracker")]
public class RoomTracker : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x1700013A RID: 314
	// (get) Token: 0x06001DD6 RID: 7638 RVA: 0x0009EDDC File Offset: 0x0009CFDC
	// (set) Token: 0x06001DD7 RID: 7639 RVA: 0x0009EDE4 File Offset: 0x0009CFE4
	public Room room { get; private set; }

	// Token: 0x06001DD8 RID: 7640 RVA: 0x0009EDF0 File Offset: 0x0009CFF0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		global::Debug.Assert(!string.IsNullOrEmpty(this.requiredRoomType) && this.requiredRoomType != Db.Get().RoomTypes.Neutral.Id, "RoomTracker must have a requiredRoomType!");
		base.Subscribe<RoomTracker>(144050788, RoomTracker.OnUpdateRoomDelegate);
		this.FindAndSetRoom();
	}

	// Token: 0x06001DD9 RID: 7641 RVA: 0x0009EE54 File Offset: 0x0009D054
	public void FindAndSetRoom()
	{
		CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(base.gameObject));
		if (cavityForCell != null && cavityForCell.room != null)
		{
			this.OnUpdateRoom(cavityForCell.room);
			return;
		}
		this.OnUpdateRoom(null);
	}

	// Token: 0x06001DDA RID: 7642 RVA: 0x0009EE9B File Offset: 0x0009D09B
	public bool IsInCorrectRoom()
	{
		return this.room != null && this.room.roomType.Id == this.requiredRoomType;
	}

	// Token: 0x06001DDB RID: 7643 RVA: 0x0009EEC4 File Offset: 0x0009D0C4
	public bool SufficientBuildLocation(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		if (this.requirement == RoomTracker.Requirement.Required || this.requirement == RoomTracker.Requirement.CustomRequired)
		{
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
			if (((cavityForCell != null) ? cavityForCell.room : null) == null)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06001DDC RID: 7644 RVA: 0x0009EF10 File Offset: 0x0009D110
	private void OnUpdateRoom(object data)
	{
		this.room = (Room)data;
		if (this.room != null && !(this.room.roomType.Id != this.requiredRoomType))
		{
			this.statusItemGuid = base.GetComponent<KSelectable>().RemoveStatusItem(this.statusItemGuid, false);
			return;
		}
		switch (this.requirement)
		{
		case RoomTracker.Requirement.TrackingOnly:
			this.statusItemGuid = base.GetComponent<KSelectable>().RemoveStatusItem(this.statusItemGuid, false);
			return;
		case RoomTracker.Requirement.Recommended:
			this.statusItemGuid = base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.RequiredRoom, Db.Get().BuildingStatusItems.NotInRecommendedRoom, this.requiredRoomType);
			return;
		case RoomTracker.Requirement.Required:
			this.statusItemGuid = base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.RequiredRoom, Db.Get().BuildingStatusItems.NotInRequiredRoom, this.requiredRoomType);
			return;
		case RoomTracker.Requirement.CustomRecommended:
		case RoomTracker.Requirement.CustomRequired:
			this.statusItemGuid = base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.RequiredRoom, Db.Get().BuildingStatusItems.Get(this.customStatusItemID), this.requiredRoomType);
			return;
		default:
			return;
		}
	}

	// Token: 0x06001DDD RID: 7645 RVA: 0x0009F04C File Offset: 0x0009D24C
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (!string.IsNullOrEmpty(this.requiredRoomType))
		{
			string name = Db.Get().RoomTypes.Get(this.requiredRoomType).Name;
			switch (this.requirement)
			{
			case RoomTracker.Requirement.Recommended:
			case RoomTracker.Requirement.CustomRecommended:
				list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.PREFERS_ROOM, name), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.PREFERS_ROOM, name), Descriptor.DescriptorType.Requirement, false));
				break;
			case RoomTracker.Requirement.Required:
			case RoomTracker.Requirement.CustomRequired:
				list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.REQUIRESROOM, name), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESROOM, name), Descriptor.DescriptorType.Requirement, false));
				break;
			}
		}
		return list;
	}

	// Token: 0x04001096 RID: 4246
	public RoomTracker.Requirement requirement;

	// Token: 0x04001097 RID: 4247
	public string requiredRoomType;

	// Token: 0x04001098 RID: 4248
	public string customStatusItemID;

	// Token: 0x04001099 RID: 4249
	private Guid statusItemGuid;

	// Token: 0x0400109B RID: 4251
	private static readonly EventSystem.IntraObjectHandler<RoomTracker> OnUpdateRoomDelegate = new EventSystem.IntraObjectHandler<RoomTracker>(delegate(RoomTracker component, object data)
	{
		component.OnUpdateRoom(data);
	});

	// Token: 0x020011A2 RID: 4514
	public enum Requirement
	{
		// Token: 0x04005D03 RID: 23811
		TrackingOnly,
		// Token: 0x04005D04 RID: 23812
		Recommended,
		// Token: 0x04005D05 RID: 23813
		Required,
		// Token: 0x04005D06 RID: 23814
		CustomRecommended,
		// Token: 0x04005D07 RID: 23815
		CustomRequired
	}
}
