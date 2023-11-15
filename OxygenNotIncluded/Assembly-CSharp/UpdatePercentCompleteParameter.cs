using System;
using System.Collections.Generic;
using FMOD.Studio;

// Token: 0x0200053D RID: 1341
internal class UpdatePercentCompleteParameter : LoopingSoundParameterUpdater
{
	// Token: 0x06002066 RID: 8294 RVA: 0x000ADBEA File Offset: 0x000ABDEA
	public UpdatePercentCompleteParameter() : base("percentComplete")
	{
	}

	// Token: 0x06002067 RID: 8295 RVA: 0x000ADC08 File Offset: 0x000ABE08
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		UpdatePercentCompleteParameter.Entry item = new UpdatePercentCompleteParameter.Entry
		{
			worker = sound.transform.GetComponent<Worker>(),
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

	// Token: 0x06002068 RID: 8296 RVA: 0x000ADC64 File Offset: 0x000ABE64
	public override void Update(float dt)
	{
		foreach (UpdatePercentCompleteParameter.Entry entry in this.entries)
		{
			if (!(entry.worker == null))
			{
				Workable workable = entry.worker.workable;
				if (!(workable == null))
				{
					float percentComplete = workable.GetPercentComplete();
					EventInstance ev = entry.ev;
					ev.setParameterByID(entry.parameterId, percentComplete, false);
				}
			}
		}
	}

	// Token: 0x06002069 RID: 8297 RVA: 0x000ADCF4 File Offset: 0x000ABEF4
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

	// Token: 0x0400121E RID: 4638
	private List<UpdatePercentCompleteParameter.Entry> entries = new List<UpdatePercentCompleteParameter.Entry>();

	// Token: 0x020011DF RID: 4575
	private struct Entry
	{
		// Token: 0x04005DE0 RID: 24032
		public Worker worker;

		// Token: 0x04005DE1 RID: 24033
		public EventInstance ev;

		// Token: 0x04005DE2 RID: 24034
		public PARAMETER_ID parameterId;
	}
}
