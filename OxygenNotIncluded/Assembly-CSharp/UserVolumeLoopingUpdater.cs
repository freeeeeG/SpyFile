using System;
using System.Collections.Generic;
using FMOD.Studio;

// Token: 0x02000466 RID: 1126
internal abstract class UserVolumeLoopingUpdater : LoopingSoundParameterUpdater
{
	// Token: 0x060018CB RID: 6347 RVA: 0x00080FC0 File Offset: 0x0007F1C0
	public UserVolumeLoopingUpdater(string parameter, string player_pref) : base(parameter)
	{
		this.playerPref = player_pref;
	}

	// Token: 0x060018CC RID: 6348 RVA: 0x00080FE0 File Offset: 0x0007F1E0
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		UserVolumeLoopingUpdater.Entry item = new UserVolumeLoopingUpdater.Entry
		{
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

	// Token: 0x060018CD RID: 6349 RVA: 0x0008102C File Offset: 0x0007F22C
	public override void Update(float dt)
	{
		if (string.IsNullOrEmpty(this.playerPref))
		{
			return;
		}
		float @float = KPlayerPrefs.GetFloat(this.playerPref);
		foreach (UserVolumeLoopingUpdater.Entry entry in this.entries)
		{
			EventInstance ev = entry.ev;
			ev.setParameterByID(entry.parameterId, @float, false);
		}
	}

	// Token: 0x060018CE RID: 6350 RVA: 0x000810AC File Offset: 0x0007F2AC
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

	// Token: 0x04000DB6 RID: 3510
	private List<UserVolumeLoopingUpdater.Entry> entries = new List<UserVolumeLoopingUpdater.Entry>();

	// Token: 0x04000DB7 RID: 3511
	private string playerPref;

	// Token: 0x020010EB RID: 4331
	private struct Entry
	{
		// Token: 0x04005A97 RID: 23191
		public EventInstance ev;

		// Token: 0x04005A98 RID: 23192
		public PARAMETER_ID parameterId;
	}
}
