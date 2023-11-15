using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000596 RID: 1430
[AddComponentMenu("KMonoBehaviour/scripts/Assignables")]
public class Assignables : KMonoBehaviour
{
	// Token: 0x17000197 RID: 407
	// (get) Token: 0x060022C3 RID: 8899 RVA: 0x000BEA6C File Offset: 0x000BCC6C
	public List<AssignableSlotInstance> Slots
	{
		get
		{
			return this.slots;
		}
	}

	// Token: 0x060022C4 RID: 8900 RVA: 0x000BEA74 File Offset: 0x000BCC74
	protected IAssignableIdentity GetAssignableIdentity()
	{
		MinionIdentity component = base.GetComponent<MinionIdentity>();
		if (component != null)
		{
			return component.assignableProxy.Get();
		}
		return base.GetComponent<MinionAssignablesProxy>();
	}

	// Token: 0x060022C5 RID: 8901 RVA: 0x000BEAA3 File Offset: 0x000BCCA3
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameUtil.SubscribeToTags<Assignables>(this, Assignables.OnDeadTagAddedDelegate, true);
	}

	// Token: 0x060022C6 RID: 8902 RVA: 0x000BEAB8 File Offset: 0x000BCCB8
	private void OnDeath(object data)
	{
		foreach (AssignableSlotInstance assignableSlotInstance in this.slots)
		{
			assignableSlotInstance.Unassign(true);
		}
	}

	// Token: 0x060022C7 RID: 8903 RVA: 0x000BEB0C File Offset: 0x000BCD0C
	public void Add(AssignableSlotInstance slot_instance)
	{
		this.slots.Add(slot_instance);
	}

	// Token: 0x060022C8 RID: 8904 RVA: 0x000BEB1C File Offset: 0x000BCD1C
	public Assignable GetAssignable(AssignableSlot slot)
	{
		AssignableSlotInstance slot2 = this.GetSlot(slot);
		if (slot2 == null)
		{
			return null;
		}
		return slot2.assignable;
	}

	// Token: 0x060022C9 RID: 8905 RVA: 0x000BEB3C File Offset: 0x000BCD3C
	public AssignableSlotInstance GetSlot(AssignableSlot slot)
	{
		global::Debug.Assert(this.slots.Count > 0, "GetSlot called with no slots configured");
		if (slot == null)
		{
			return null;
		}
		foreach (AssignableSlotInstance assignableSlotInstance in this.slots)
		{
			if (assignableSlotInstance.slot == slot)
			{
				return assignableSlotInstance;
			}
		}
		return null;
	}

	// Token: 0x060022CA RID: 8906 RVA: 0x000BEBB8 File Offset: 0x000BCDB8
	public Assignable AutoAssignSlot(AssignableSlot slot)
	{
		Assignable assignable = this.GetAssignable(slot);
		if (assignable != null)
		{
			return assignable;
		}
		GameObject targetGameObject = base.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
		if (targetGameObject == null)
		{
			global::Debug.LogWarning("AutoAssignSlot failed, proxy game object was null.");
			return null;
		}
		Navigator component = targetGameObject.GetComponent<Navigator>();
		IAssignableIdentity assignableIdentity = this.GetAssignableIdentity();
		int num = int.MaxValue;
		foreach (Assignable assignable2 in Game.Instance.assignmentManager)
		{
			if (!(assignable2 == null) && !assignable2.IsAssigned() && assignable2.slot == slot && assignable2.CanAutoAssignTo(assignableIdentity))
			{
				int navigationCost = assignable2.GetNavigationCost(component);
				if (navigationCost != -1 && navigationCost < num)
				{
					num = navigationCost;
					assignable = assignable2;
				}
			}
		}
		if (assignable != null)
		{
			assignable.Assign(assignableIdentity);
		}
		return assignable;
	}

	// Token: 0x060022CB RID: 8907 RVA: 0x000BECA8 File Offset: 0x000BCEA8
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		foreach (AssignableSlotInstance assignableSlotInstance in this.slots)
		{
			assignableSlotInstance.Unassign(true);
		}
	}

	// Token: 0x040013F0 RID: 5104
	protected List<AssignableSlotInstance> slots = new List<AssignableSlotInstance>();

	// Token: 0x040013F1 RID: 5105
	private static readonly EventSystem.IntraObjectHandler<Assignables> OnDeadTagAddedDelegate = GameUtil.CreateHasTagHandler<Assignables>(GameTags.Dead, delegate(Assignables component, object data)
	{
		component.OnDeath(data);
	});
}
