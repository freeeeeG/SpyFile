using System;
using UnityEngine;

// Token: 0x02000595 RID: 1429
public abstract class AssignableSlotInstance
{
	// Token: 0x17000195 RID: 405
	// (get) Token: 0x060022BB RID: 8891 RVA: 0x000BE99E File Offset: 0x000BCB9E
	// (set) Token: 0x060022BC RID: 8892 RVA: 0x000BE9A6 File Offset: 0x000BCBA6
	public Assignables assignables { get; private set; }

	// Token: 0x17000196 RID: 406
	// (get) Token: 0x060022BD RID: 8893 RVA: 0x000BE9AF File Offset: 0x000BCBAF
	public GameObject gameObject
	{
		get
		{
			return this.assignables.gameObject;
		}
	}

	// Token: 0x060022BE RID: 8894 RVA: 0x000BE9BC File Offset: 0x000BCBBC
	public AssignableSlotInstance(Assignables assignables, AssignableSlot slot)
	{
		this.slot = slot;
		this.assignables = assignables;
	}

	// Token: 0x060022BF RID: 8895 RVA: 0x000BE9D2 File Offset: 0x000BCBD2
	public void Assign(Assignable assignable)
	{
		if (this.assignable == assignable)
		{
			return;
		}
		this.Unassign(false);
		this.assignable = assignable;
		this.assignables.Trigger(-1585839766, this);
	}

	// Token: 0x060022C0 RID: 8896 RVA: 0x000BEA04 File Offset: 0x000BCC04
	public virtual void Unassign(bool trigger_event = true)
	{
		if (this.unassigning)
		{
			return;
		}
		if (this.IsAssigned())
		{
			this.unassigning = true;
			this.assignable.Unassign();
			if (trigger_event)
			{
				this.assignables.Trigger(-1585839766, this);
			}
			this.assignable = null;
			this.unassigning = false;
		}
	}

	// Token: 0x060022C1 RID: 8897 RVA: 0x000BEA56 File Offset: 0x000BCC56
	public bool IsAssigned()
	{
		return this.assignable != null;
	}

	// Token: 0x060022C2 RID: 8898 RVA: 0x000BEA64 File Offset: 0x000BCC64
	public bool IsUnassigning()
	{
		return this.unassigning;
	}

	// Token: 0x040013EC RID: 5100
	public AssignableSlot slot;

	// Token: 0x040013ED RID: 5101
	public Assignable assignable;

	// Token: 0x040013EF RID: 5103
	private bool unassigning;
}
