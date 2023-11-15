using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020008CF RID: 2255
[SerializationConfig(MemberSerialization.OptIn)]
public class Ownable : Assignable, ISaveLoadable, IGameObjectEffectDescriptor
{
	// Token: 0x06004141 RID: 16705 RVA: 0x0016D830 File Offset: 0x0016BA30
	public override void Assign(IAssignableIdentity new_assignee)
	{
		if (new_assignee == this.assignee)
		{
			return;
		}
		if (base.slot != null && new_assignee is MinionIdentity)
		{
			new_assignee = (new_assignee as MinionIdentity).assignableProxy.Get();
		}
		if (base.slot != null && new_assignee is StoredMinionIdentity)
		{
			new_assignee = (new_assignee as StoredMinionIdentity).assignableProxy.Get();
		}
		if (new_assignee is MinionAssignablesProxy)
		{
			AssignableSlotInstance slot = new_assignee.GetSoleOwner().GetComponent<Ownables>().GetSlot(base.slot);
			if (slot != null)
			{
				Assignable assignable = slot.assignable;
				if (assignable != null)
				{
					assignable.Unassign();
				}
			}
		}
		base.Assign(new_assignee);
	}

	// Token: 0x06004142 RID: 16706 RVA: 0x0016D8CC File Offset: 0x0016BACC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UpdateTint();
		this.UpdateStatusString();
		base.OnAssign += this.OnNewAssignment;
		if (this.assignee == null)
		{
			MinionStorage component = base.GetComponent<MinionStorage>();
			if (component)
			{
				List<MinionStorage.Info> storedMinionInfo = component.GetStoredMinionInfo();
				if (storedMinionInfo.Count > 0)
				{
					Ref<KPrefabID> serializedMinion = storedMinionInfo[0].serializedMinion;
					if (serializedMinion != null && serializedMinion.GetId() != -1)
					{
						StoredMinionIdentity component2 = serializedMinion.Get().GetComponent<StoredMinionIdentity>();
						component2.ValidateProxy();
						this.Assign(component2);
					}
				}
			}
		}
	}

	// Token: 0x06004143 RID: 16707 RVA: 0x0016D956 File Offset: 0x0016BB56
	private void OnNewAssignment(IAssignableIdentity assignables)
	{
		this.UpdateTint();
		this.UpdateStatusString();
	}

	// Token: 0x06004144 RID: 16708 RVA: 0x0016D964 File Offset: 0x0016BB64
	private void UpdateTint()
	{
		if (this.tintWhenUnassigned)
		{
			KAnimControllerBase component = base.GetComponent<KAnimControllerBase>();
			if (component != null && component.HasBatchInstanceData)
			{
				component.TintColour = ((this.assignee == null) ? this.unownedTint : this.ownedTint);
				return;
			}
			KBatchedAnimController component2 = base.GetComponent<KBatchedAnimController>();
			if (component2 != null && component2.HasBatchInstanceData)
			{
				component2.TintColour = ((this.assignee == null) ? this.unownedTint : this.ownedTint);
			}
		}
	}

	// Token: 0x06004145 RID: 16709 RVA: 0x0016D9EC File Offset: 0x0016BBEC
	private void UpdateStatusString()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (component == null)
		{
			return;
		}
		StatusItem status_item;
		if (this.assignee != null)
		{
			if (this.assignee is MinionIdentity)
			{
				status_item = Db.Get().BuildingStatusItems.AssignedTo;
			}
			else if (this.assignee is Room)
			{
				status_item = Db.Get().BuildingStatusItems.AssignedTo;
			}
			else
			{
				status_item = Db.Get().BuildingStatusItems.AssignedTo;
			}
		}
		else
		{
			status_item = Db.Get().BuildingStatusItems.Unassigned;
		}
		component.SetStatusItem(Db.Get().StatusItemCategories.Ownable, status_item, this);
	}

	// Token: 0x06004146 RID: 16710 RVA: 0x0016DA8C File Offset: 0x0016BC8C
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.ASSIGNEDDUPLICANT, UI.BUILDINGEFFECTS.TOOLTIPS.ASSIGNEDDUPLICANT, Descriptor.DescriptorType.Requirement);
		list.Add(item);
		return list;
	}

	// Token: 0x04002A7A RID: 10874
	public bool tintWhenUnassigned = true;

	// Token: 0x04002A7B RID: 10875
	private Color unownedTint = Color.gray;

	// Token: 0x04002A7C RID: 10876
	private Color ownedTint = Color.white;
}
