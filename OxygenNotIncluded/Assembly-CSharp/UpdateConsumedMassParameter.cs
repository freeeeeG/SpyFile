using System;
using System.Collections.Generic;
using FMOD.Studio;

// Token: 0x02000499 RID: 1177
internal class UpdateConsumedMassParameter : LoopingSoundParameterUpdater
{
	// Token: 0x06001A8D RID: 6797 RVA: 0x0008DD4B File Offset: 0x0008BF4B
	public UpdateConsumedMassParameter() : base("consumedMass")
	{
	}

	// Token: 0x06001A8E RID: 6798 RVA: 0x0008DD68 File Offset: 0x0008BF68
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		UpdateConsumedMassParameter.Entry item = new UpdateConsumedMassParameter.Entry
		{
			creatureCalorieMonitor = sound.transform.GetSMI<CreatureCalorieMonitor.Instance>(),
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

	// Token: 0x06001A8F RID: 6799 RVA: 0x0008DDC4 File Offset: 0x0008BFC4
	public override void Update(float dt)
	{
		foreach (UpdateConsumedMassParameter.Entry entry in this.entries)
		{
			if (!entry.creatureCalorieMonitor.IsNullOrStopped())
			{
				float fullness = entry.creatureCalorieMonitor.stomach.GetFullness();
				EventInstance ev = entry.ev;
				ev.setParameterByID(entry.parameterId, fullness, false);
			}
		}
	}

	// Token: 0x06001A90 RID: 6800 RVA: 0x0008DE48 File Offset: 0x0008C048
	public override void Remove(LoopingSoundParameterUpdater.Sound sound)
	{
		for (int i = 0; i < this.entries.Count; i++)
		{
			if (this.entries[i].ev.handle == sound.ev.handle)
			{
				this.entries.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x04000EC4 RID: 3780
	private List<UpdateConsumedMassParameter.Entry> entries = new List<UpdateConsumedMassParameter.Entry>();

	// Token: 0x0200112E RID: 4398
	private struct Entry
	{
		// Token: 0x04005B97 RID: 23447
		public CreatureCalorieMonitor.Instance creatureCalorieMonitor;

		// Token: 0x04005B98 RID: 23448
		public EventInstance ev;

		// Token: 0x04005B99 RID: 23449
		public PARAMETER_ID parameterId;
	}
}
