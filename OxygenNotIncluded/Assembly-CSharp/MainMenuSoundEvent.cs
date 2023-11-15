using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000452 RID: 1106
public class MainMenuSoundEvent : SoundEvent
{
	// Token: 0x06001831 RID: 6193 RVA: 0x0007DABA File Offset: 0x0007BCBA
	public MainMenuSoundEvent(string file_name, string sound_name, int frame) : base(file_name, sound_name, frame, true, false, (float)SoundEvent.IGNORE_INTERVAL, false)
	{
	}

	// Token: 0x06001832 RID: 6194 RVA: 0x0007DAD0 File Offset: 0x0007BCD0
	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		EventInstance instance = KFMOD.BeginOneShot(base.sound, Vector3.zero, 1f);
		if (instance.isValid())
		{
			instance.setParameterByName("frame", (float)base.frame, false);
			KFMOD.EndOneShot(instance);
		}
	}
}
