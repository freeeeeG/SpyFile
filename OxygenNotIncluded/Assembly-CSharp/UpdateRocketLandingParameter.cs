using System;
using System.Collections.Generic;
using FMOD.Studio;

// Token: 0x020009A8 RID: 2472
internal class UpdateRocketLandingParameter : LoopingSoundParameterUpdater
{
	// Token: 0x06004992 RID: 18834 RVA: 0x0019E7AB File Offset: 0x0019C9AB
	public UpdateRocketLandingParameter() : base("rocketLanding")
	{
	}

	// Token: 0x06004993 RID: 18835 RVA: 0x0019E7C8 File Offset: 0x0019C9C8
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		UpdateRocketLandingParameter.Entry item = new UpdateRocketLandingParameter.Entry
		{
			rocketModule = sound.transform.GetComponent<RocketModule>(),
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

	// Token: 0x06004994 RID: 18836 RVA: 0x0019E824 File Offset: 0x0019CA24
	public override void Update(float dt)
	{
		foreach (UpdateRocketLandingParameter.Entry entry in this.entries)
		{
			if (!(entry.rocketModule == null))
			{
				LaunchConditionManager conditionManager = entry.rocketModule.conditionManager;
				if (!(conditionManager == null))
				{
					ILaunchableRocket component = conditionManager.GetComponent<ILaunchableRocket>();
					if (component != null)
					{
						if (component.isLanding)
						{
							EventInstance ev = entry.ev;
							ev.setParameterByID(entry.parameterId, 1f, false);
						}
						else
						{
							EventInstance ev = entry.ev;
							ev.setParameterByID(entry.parameterId, 0f, false);
						}
					}
				}
			}
		}
	}

	// Token: 0x06004995 RID: 18837 RVA: 0x0019E8E0 File Offset: 0x0019CAE0
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

	// Token: 0x0400305C RID: 12380
	private List<UpdateRocketLandingParameter.Entry> entries = new List<UpdateRocketLandingParameter.Entry>();

	// Token: 0x02001826 RID: 6182
	private struct Entry
	{
		// Token: 0x0400712E RID: 28974
		public RocketModule rocketModule;

		// Token: 0x0400712F RID: 28975
		public EventInstance ev;

		// Token: 0x04007130 RID: 28976
		public PARAMETER_ID parameterId;
	}
}
