using System;
using System.Collections.Generic;
using FMOD.Studio;

// Token: 0x020009A9 RID: 2473
internal class UpdateRocketSpeedParameter : LoopingSoundParameterUpdater
{
	// Token: 0x06004996 RID: 18838 RVA: 0x0019E938 File Offset: 0x0019CB38
	public UpdateRocketSpeedParameter() : base("rocketSpeed")
	{
	}

	// Token: 0x06004997 RID: 18839 RVA: 0x0019E958 File Offset: 0x0019CB58
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		UpdateRocketSpeedParameter.Entry item = new UpdateRocketSpeedParameter.Entry
		{
			rocketModule = sound.transform.GetComponent<RocketModule>(),
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

	// Token: 0x06004998 RID: 18840 RVA: 0x0019E9B4 File Offset: 0x0019CBB4
	public override void Update(float dt)
	{
		foreach (UpdateRocketSpeedParameter.Entry entry in this.entries)
		{
			if (!(entry.rocketModule == null))
			{
				LaunchConditionManager conditionManager = entry.rocketModule.conditionManager;
				if (!(conditionManager == null))
				{
					ILaunchableRocket component = conditionManager.GetComponent<ILaunchableRocket>();
					if (component != null)
					{
						EventInstance ev = entry.ev;
						ev.setParameterByID(entry.parameterId, component.rocketSpeed, false);
					}
				}
			}
		}
	}

	// Token: 0x06004999 RID: 18841 RVA: 0x0019EA4C File Offset: 0x0019CC4C
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

	// Token: 0x0400305D RID: 12381
	private List<UpdateRocketSpeedParameter.Entry> entries = new List<UpdateRocketSpeedParameter.Entry>();

	// Token: 0x02001827 RID: 6183
	private struct Entry
	{
		// Token: 0x04007131 RID: 28977
		public RocketModule rocketModule;

		// Token: 0x04007132 RID: 28978
		public EventInstance ev;

		// Token: 0x04007133 RID: 28979
		public PARAMETER_ID parameterId;
	}
}
