using System;
using System.Collections.Generic;
using FMOD.Studio;

// Token: 0x020006EC RID: 1772
internal class UpdateDistanceToImpactParameter : LoopingSoundParameterUpdater
{
	// Token: 0x0600309F RID: 12447 RVA: 0x00101D91 File Offset: 0x000FFF91
	public UpdateDistanceToImpactParameter() : base("distanceToImpact")
	{
	}

	// Token: 0x060030A0 RID: 12448 RVA: 0x00101DB0 File Offset: 0x000FFFB0
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		UpdateDistanceToImpactParameter.Entry item = new UpdateDistanceToImpactParameter.Entry
		{
			comet = sound.transform.GetComponent<Comet>(),
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

	// Token: 0x060030A1 RID: 12449 RVA: 0x00101E0C File Offset: 0x0010000C
	public override void Update(float dt)
	{
		foreach (UpdateDistanceToImpactParameter.Entry entry in this.entries)
		{
			if (!(entry.comet == null))
			{
				float soundDistance = entry.comet.GetSoundDistance();
				EventInstance ev = entry.ev;
				ev.setParameterByID(entry.parameterId, soundDistance, false);
			}
		}
	}

	// Token: 0x060030A2 RID: 12450 RVA: 0x00101E8C File Offset: 0x0010008C
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

	// Token: 0x04001CD7 RID: 7383
	private List<UpdateDistanceToImpactParameter.Entry> entries = new List<UpdateDistanceToImpactParameter.Entry>();

	// Token: 0x02001434 RID: 5172
	private struct Entry
	{
		// Token: 0x040064A5 RID: 25765
		public Comet comet;

		// Token: 0x040064A6 RID: 25766
		public EventInstance ev;

		// Token: 0x040064A7 RID: 25767
		public PARAMETER_ID parameterId;
	}
}
