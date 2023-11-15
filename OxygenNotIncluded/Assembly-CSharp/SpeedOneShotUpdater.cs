using System;

// Token: 0x02000C6D RID: 3181
public class SpeedOneShotUpdater : OneShotSoundParameterUpdater
{
	// Token: 0x06006552 RID: 25938 RVA: 0x0025A4A1 File Offset: 0x002586A1
	public SpeedOneShotUpdater() : base("Speed")
	{
	}

	// Token: 0x06006553 RID: 25939 RVA: 0x0025A4B3 File Offset: 0x002586B3
	public override void Play(OneShotSoundParameterUpdater.Sound sound)
	{
		sound.ev.setParameterByID(sound.description.GetParameterId(base.parameter), SpeedLoopingSoundUpdater.GetSpeedParameterValue(), false);
	}
}
