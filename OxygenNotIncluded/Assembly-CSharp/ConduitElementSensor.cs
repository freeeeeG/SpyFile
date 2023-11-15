using System;
using KSerialization;

// Token: 0x020005E1 RID: 1505
[SerializationConfig(MemberSerialization.OptIn)]
public class ConduitElementSensor : ConduitSensor
{
	// Token: 0x0600257A RID: 9594 RVA: 0x000CBB3E File Offset: 0x000C9D3E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.filterable.onFilterChanged += this.OnFilterChanged;
		this.OnFilterChanged(this.filterable.SelectedTag);
	}

	// Token: 0x0600257B RID: 9595 RVA: 0x000CBB70 File Offset: 0x000C9D70
	private void OnFilterChanged(Tag tag)
	{
		if (!tag.IsValid)
		{
			return;
		}
		bool on = tag == GameTags.Void;
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NoFilterElementSelected, on, null);
	}

	// Token: 0x0600257C RID: 9596 RVA: 0x000CBBB0 File Offset: 0x000C9DB0
	protected override void ConduitUpdate(float dt)
	{
		Tag a;
		bool flag;
		this.GetContentsElement(out a, out flag);
		if (!base.IsSwitchedOn)
		{
			if (a == this.filterable.SelectedTag && flag)
			{
				this.Toggle();
				return;
			}
		}
		else if (a != this.filterable.SelectedTag || !flag)
		{
			this.Toggle();
		}
	}

	// Token: 0x0600257D RID: 9597 RVA: 0x000CBC08 File Offset: 0x000C9E08
	private void GetContentsElement(out Tag element, out bool hasMass)
	{
		int cell = Grid.PosToCell(this);
		if (this.conduitType == ConduitType.Liquid || this.conduitType == ConduitType.Gas)
		{
			ConduitFlow.ConduitContents contents = Conduit.GetFlowManager(this.conduitType).GetContents(cell);
			element = contents.element.CreateTag();
			hasMass = (contents.mass > 0f);
			return;
		}
		SolidConduitFlow flowManager = SolidConduit.GetFlowManager();
		SolidConduitFlow.ConduitContents contents2 = flowManager.GetContents(cell);
		Pickupable pickupable = flowManager.GetPickupable(contents2.pickupableHandle);
		KPrefabID kprefabID = (pickupable != null) ? pickupable.GetComponent<KPrefabID>() : null;
		if (kprefabID != null && pickupable.PrimaryElement.Mass > 0f)
		{
			element = kprefabID.PrefabTag;
			hasMass = true;
			return;
		}
		element = GameTags.Void;
		hasMass = false;
	}

	// Token: 0x0400156B RID: 5483
	[MyCmpGet]
	private Filterable filterable;
}
