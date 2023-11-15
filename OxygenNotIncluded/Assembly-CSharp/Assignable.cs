using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x02000593 RID: 1427
public abstract class Assignable : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x17000193 RID: 403
	// (get) Token: 0x060022A3 RID: 8867 RVA: 0x000BE2B3 File Offset: 0x000BC4B3
	public AssignableSlot slot
	{
		get
		{
			if (this._slot == null)
			{
				this._slot = Db.Get().AssignableSlots.Get(this.slotID);
			}
			return this._slot;
		}
	}

	// Token: 0x17000194 RID: 404
	// (get) Token: 0x060022A4 RID: 8868 RVA: 0x000BE2DE File Offset: 0x000BC4DE
	public bool CanBeAssigned
	{
		get
		{
			return this.canBeAssigned;
		}
	}

	// Token: 0x14000010 RID: 16
	// (add) Token: 0x060022A5 RID: 8869 RVA: 0x000BE2E8 File Offset: 0x000BC4E8
	// (remove) Token: 0x060022A6 RID: 8870 RVA: 0x000BE320 File Offset: 0x000BC520
	public event Action<IAssignableIdentity> OnAssign;

	// Token: 0x060022A7 RID: 8871 RVA: 0x000BE355 File Offset: 0x000BC555
	[OnDeserialized]
	internal void OnDeserialized()
	{
	}

	// Token: 0x060022A8 RID: 8872 RVA: 0x000BE358 File Offset: 0x000BC558
	private void RestoreAssignee()
	{
		IAssignableIdentity savedAssignee = this.GetSavedAssignee();
		if (savedAssignee != null)
		{
			this.Assign(savedAssignee);
		}
	}

	// Token: 0x060022A9 RID: 8873 RVA: 0x000BE378 File Offset: 0x000BC578
	private IAssignableIdentity GetSavedAssignee()
	{
		if (this.assignee_identityRef.Get() != null)
		{
			return this.assignee_identityRef.Get().GetComponent<IAssignableIdentity>();
		}
		if (!string.IsNullOrEmpty(this.assignee_groupID))
		{
			return Game.Instance.assignmentManager.assignment_groups[this.assignee_groupID];
		}
		return null;
	}

	// Token: 0x060022AA RID: 8874 RVA: 0x000BE3D4 File Offset: 0x000BC5D4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.RestoreAssignee();
		Game.Instance.assignmentManager.Add(this);
		if (this.assignee == null && this.canBePublic)
		{
			this.Assign(Game.Instance.assignmentManager.assignment_groups["public"]);
		}
		this.assignmentPreconditions.Add(delegate(MinionAssignablesProxy proxy)
		{
			GameObject targetGameObject = proxy.GetTargetGameObject();
			return targetGameObject.GetComponent<KMonoBehaviour>().GetMyWorldId() == this.GetMyWorldId() || targetGameObject.IsMyParentWorld(base.gameObject);
		});
		this.autoassignmentPreconditions.Add(delegate(MinionAssignablesProxy proxy)
		{
			Operational component = base.GetComponent<Operational>();
			return !(component != null) || component.IsOperational;
		});
	}

	// Token: 0x060022AB RID: 8875 RVA: 0x000BE45A File Offset: 0x000BC65A
	protected override void OnCleanUp()
	{
		this.Unassign();
		Game.Instance.assignmentManager.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x060022AC RID: 8876 RVA: 0x000BE478 File Offset: 0x000BC678
	public bool CanAutoAssignTo(IAssignableIdentity identity)
	{
		MinionAssignablesProxy minionAssignablesProxy = identity as MinionAssignablesProxy;
		if (minionAssignablesProxy == null)
		{
			return true;
		}
		if (!this.CanAssignTo(minionAssignablesProxy))
		{
			return false;
		}
		using (List<Func<MinionAssignablesProxy, bool>>.Enumerator enumerator = this.autoassignmentPreconditions.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current(minionAssignablesProxy))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x060022AD RID: 8877 RVA: 0x000BE4F0 File Offset: 0x000BC6F0
	public bool CanAssignTo(IAssignableIdentity identity)
	{
		MinionAssignablesProxy minionAssignablesProxy = identity as MinionAssignablesProxy;
		if (minionAssignablesProxy == null)
		{
			return true;
		}
		using (List<Func<MinionAssignablesProxy, bool>>.Enumerator enumerator = this.assignmentPreconditions.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current(minionAssignablesProxy))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x060022AE RID: 8878 RVA: 0x000BE55C File Offset: 0x000BC75C
	public bool IsAssigned()
	{
		return this.assignee != null;
	}

	// Token: 0x060022AF RID: 8879 RVA: 0x000BE568 File Offset: 0x000BC768
	public bool IsAssignedTo(IAssignableIdentity identity)
	{
		global::Debug.Assert(identity != null, "IsAssignedTo identity is null");
		Ownables soleOwner = identity.GetSoleOwner();
		global::Debug.Assert(soleOwner != null, "IsAssignedTo identity sole owner is null");
		if (this.assignee != null)
		{
			foreach (Ownables ownables in this.assignee.GetOwners())
			{
				global::Debug.Assert(ownables, "Assignable owners list contained null");
				if (ownables.gameObject == soleOwner.gameObject)
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x060022B0 RID: 8880 RVA: 0x000BE610 File Offset: 0x000BC810
	public virtual void Assign(IAssignableIdentity new_assignee)
	{
		if (new_assignee == this.assignee)
		{
			return;
		}
		if (new_assignee is KMonoBehaviour)
		{
			if (!this.CanAssignTo(new_assignee))
			{
				return;
			}
			this.assignee_identityRef.Set((KMonoBehaviour)new_assignee);
			this.assignee_groupID = "";
		}
		else if (new_assignee is AssignmentGroup)
		{
			this.assignee_identityRef.Set(null);
			this.assignee_groupID = ((AssignmentGroup)new_assignee).id;
		}
		base.GetComponent<KPrefabID>().AddTag(GameTags.Assigned, false);
		this.assignee = new_assignee;
		if (this.slot != null && (new_assignee is MinionIdentity || new_assignee is StoredMinionIdentity || new_assignee is MinionAssignablesProxy))
		{
			Ownables soleOwner = new_assignee.GetSoleOwner();
			if (soleOwner != null)
			{
				AssignableSlotInstance slot = soleOwner.GetSlot(this.slot);
				if (slot != null)
				{
					slot.Assign(this);
				}
			}
			Equipment component = soleOwner.GetComponent<Equipment>();
			if (component != null)
			{
				AssignableSlotInstance slot2 = component.GetSlot(this.slot);
				if (slot2 != null)
				{
					slot2.Assign(this);
				}
			}
		}
		if (this.OnAssign != null)
		{
			this.OnAssign(new_assignee);
		}
		base.Trigger(684616645, new_assignee);
	}

	// Token: 0x060022B1 RID: 8881 RVA: 0x000BE724 File Offset: 0x000BC924
	public virtual void Unassign()
	{
		if (this.assignee == null)
		{
			return;
		}
		base.GetComponent<KPrefabID>().RemoveTag(GameTags.Assigned);
		if (this.slot != null)
		{
			Ownables soleOwner = this.assignee.GetSoleOwner();
			if (soleOwner)
			{
				AssignableSlotInstance slot = soleOwner.GetSlot(this.slot);
				if (slot != null)
				{
					slot.Unassign(true);
				}
				Equipment component = soleOwner.GetComponent<Equipment>();
				if (component != null)
				{
					slot = component.GetSlot(this.slot);
					if (slot != null)
					{
						slot.Unassign(true);
					}
				}
			}
		}
		this.assignee = null;
		if (this.canBePublic)
		{
			this.Assign(Game.Instance.assignmentManager.assignment_groups["public"]);
		}
		this.assignee_identityRef.Set(null);
		this.assignee_groupID = "";
		if (this.OnAssign != null)
		{
			this.OnAssign(null);
		}
		base.Trigger(684616645, null);
	}

	// Token: 0x060022B2 RID: 8882 RVA: 0x000BE809 File Offset: 0x000BCA09
	public void SetCanBeAssigned(bool state)
	{
		this.canBeAssigned = state;
	}

	// Token: 0x060022B3 RID: 8883 RVA: 0x000BE812 File Offset: 0x000BCA12
	public void AddAssignPrecondition(Func<MinionAssignablesProxy, bool> precondition)
	{
		this.assignmentPreconditions.Add(precondition);
	}

	// Token: 0x060022B4 RID: 8884 RVA: 0x000BE820 File Offset: 0x000BCA20
	public void AddAutoassignPrecondition(Func<MinionAssignablesProxy, bool> precondition)
	{
		this.autoassignmentPreconditions.Add(precondition);
	}

	// Token: 0x060022B5 RID: 8885 RVA: 0x000BE830 File Offset: 0x000BCA30
	public int GetNavigationCost(Navigator navigator)
	{
		int num = -1;
		int cell = Grid.PosToCell(this);
		IApproachable component = base.GetComponent<IApproachable>();
		CellOffset[] array = (component != null) ? component.GetOffsets() : new CellOffset[1];
		DebugUtil.DevAssert(navigator != null, "Navigator is mysteriously null", null);
		if (navigator == null)
		{
			return -1;
		}
		foreach (CellOffset offset in array)
		{
			int cell2 = Grid.OffsetCell(cell, offset);
			int navigationCost = navigator.GetNavigationCost(cell2);
			if (navigationCost != -1 && (num == -1 || navigationCost < num))
			{
				num = navigationCost;
			}
		}
		return num;
	}

	// Token: 0x040013E0 RID: 5088
	public string slotID;

	// Token: 0x040013E1 RID: 5089
	private AssignableSlot _slot;

	// Token: 0x040013E2 RID: 5090
	public IAssignableIdentity assignee;

	// Token: 0x040013E3 RID: 5091
	[Serialize]
	protected Ref<KMonoBehaviour> assignee_identityRef = new Ref<KMonoBehaviour>();

	// Token: 0x040013E4 RID: 5092
	[Serialize]
	private string assignee_groupID = "";

	// Token: 0x040013E5 RID: 5093
	public AssignableSlot[] subSlots;

	// Token: 0x040013E6 RID: 5094
	public bool canBePublic;

	// Token: 0x040013E7 RID: 5095
	[Serialize]
	private bool canBeAssigned = true;

	// Token: 0x040013E8 RID: 5096
	private List<Func<MinionAssignablesProxy, bool>> autoassignmentPreconditions = new List<Func<MinionAssignablesProxy, bool>>();

	// Token: 0x040013E9 RID: 5097
	private List<Func<MinionAssignablesProxy, bool>> assignmentPreconditions = new List<Func<MinionAssignablesProxy, bool>>();
}
