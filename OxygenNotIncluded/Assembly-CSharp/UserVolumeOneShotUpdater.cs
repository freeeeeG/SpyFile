using System;

// Token: 0x0200046B RID: 1131
internal abstract class UserVolumeOneShotUpdater : OneShotSoundParameterUpdater
{
	// Token: 0x060018D3 RID: 6355 RVA: 0x0008114C File Offset: 0x0007F34C
	public UserVolumeOneShotUpdater(string parameter, string player_pref) : base(parameter)
	{
		this.playerPref = player_pref;
	}

	// Token: 0x060018D4 RID: 6356 RVA: 0x00081164 File Offset: 0x0007F364
	public override void Play(OneShotSoundParameterUpdater.Sound sound)
	{
		if (!string.IsNullOrEmpty(this.playerPref))
		{
			float @float = KPlayerPrefs.GetFloat(this.playerPref);
			sound.ev.setParameterByID(sound.description.GetParameterId(base.parameter), @float, false);
		}
	}

	// Token: 0x04000DB8 RID: 3512
	private string playerPref;
}
