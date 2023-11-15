using System;

// Token: 0x0200041A RID: 1050
public class AssignableReachabilitySensor : Sensor
{
	// Token: 0x06001635 RID: 5685 RVA: 0x000744A0 File Offset: 0x000726A0
	public AssignableReachabilitySensor(Sensors sensors) : base(sensors)
	{
		MinionAssignablesProxy minionAssignablesProxy = base.gameObject.GetComponent<MinionIdentity>().assignableProxy.Get();
		minionAssignablesProxy.ConfigureAssignableSlots();
		Assignables[] components = minionAssignablesProxy.GetComponents<Assignables>();
		if (components.Length == 0)
		{
			Debug.LogError(base.gameObject.GetProperName() + ": No 'Assignables' components found for AssignableReachabilitySensor");
		}
		int num = 0;
		foreach (Assignables assignables in components)
		{
			num += assignables.Slots.Count;
		}
		this.slots = new AssignableReachabilitySensor.SlotEntry[num];
		int num2 = 0;
		foreach (Assignables assignables2 in components)
		{
			for (int k = 0; k < assignables2.Slots.Count; k++)
			{
				this.slots[num2++].slot = assignables2.Slots[k];
			}
		}
		this.navigator = base.GetComponent<Navigator>();
	}

	// Token: 0x06001636 RID: 5686 RVA: 0x00074588 File Offset: 0x00072788
	public bool IsReachable(AssignableSlot slot)
	{
		for (int i = 0; i < this.slots.Length; i++)
		{
			if (this.slots[i].slot.slot == slot)
			{
				return this.slots[i].isReachable;
			}
		}
		Debug.LogError("Could not find slot: " + ((slot != null) ? slot.ToString() : null));
		return false;
	}

	// Token: 0x06001637 RID: 5687 RVA: 0x000745F0 File Offset: 0x000727F0
	public override void Update()
	{
		for (int i = 0; i < this.slots.Length; i++)
		{
			AssignableReachabilitySensor.SlotEntry slotEntry = this.slots[i];
			AssignableSlotInstance slot = slotEntry.slot;
			if (slot.IsAssigned())
			{
				bool flag = slot.assignable.GetNavigationCost(this.navigator) != -1;
				Operational component = slot.assignable.GetComponent<Operational>();
				if (component != null)
				{
					flag = (flag && component.IsOperational);
				}
				if (flag != slotEntry.isReachable)
				{
					slotEntry.isReachable = flag;
					this.slots[i] = slotEntry;
					base.Trigger(334784980, slotEntry);
				}
			}
			else if (slotEntry.isReachable)
			{
				slotEntry.isReachable = false;
				this.slots[i] = slotEntry;
				base.Trigger(334784980, slotEntry);
			}
		}
	}

	// Token: 0x04000C5F RID: 3167
	private AssignableReachabilitySensor.SlotEntry[] slots;

	// Token: 0x04000C60 RID: 3168
	private Navigator navigator;

	// Token: 0x02001096 RID: 4246
	private struct SlotEntry
	{
		// Token: 0x0400599F RID: 22943
		public AssignableSlotInstance slot;

		// Token: 0x040059A0 RID: 22944
		public bool isReachable;
	}
}
