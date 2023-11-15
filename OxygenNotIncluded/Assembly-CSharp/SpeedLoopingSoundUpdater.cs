using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000C6C RID: 3180
public class SpeedLoopingSoundUpdater : LoopingSoundParameterUpdater
{
	// Token: 0x0600654D RID: 25933 RVA: 0x0025A365 File Offset: 0x00258565
	public SpeedLoopingSoundUpdater() : base("Speed")
	{
	}

	// Token: 0x0600654E RID: 25934 RVA: 0x0025A384 File Offset: 0x00258584
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		SpeedLoopingSoundUpdater.Entry item = new SpeedLoopingSoundUpdater.Entry
		{
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

	// Token: 0x0600654F RID: 25935 RVA: 0x0025A3D0 File Offset: 0x002585D0
	public override void Update(float dt)
	{
		float speedParameterValue = SpeedLoopingSoundUpdater.GetSpeedParameterValue();
		foreach (SpeedLoopingSoundUpdater.Entry entry in this.entries)
		{
			EventInstance ev = entry.ev;
			ev.setParameterByID(entry.parameterId, speedParameterValue, false);
		}
	}

	// Token: 0x06006550 RID: 25936 RVA: 0x0025A43C File Offset: 0x0025863C
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

	// Token: 0x06006551 RID: 25937 RVA: 0x0025A494 File Offset: 0x00258694
	public static float GetSpeedParameterValue()
	{
		return Time.timeScale * 1f;
	}

	// Token: 0x04004585 RID: 17797
	private List<SpeedLoopingSoundUpdater.Entry> entries = new List<SpeedLoopingSoundUpdater.Entry>();

	// Token: 0x02001BA1 RID: 7073
	private struct Entry
	{
		// Token: 0x04007D52 RID: 32082
		public EventInstance ev;

		// Token: 0x04007D53 RID: 32083
		public PARAMETER_ID parameterId;
	}
}
