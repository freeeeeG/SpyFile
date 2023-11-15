using System;

// Token: 0x0200045F RID: 1119
public class UIAnimationSoundEvent : SoundEvent
{
	// Token: 0x0600188A RID: 6282 RVA: 0x0007F63C File Offset: 0x0007D83C
	public UIAnimationSoundEvent(string file_name, string sound_name, int frame, bool looping) : base(file_name, sound_name, frame, true, looping, (float)SoundEvent.IGNORE_INTERVAL, false)
	{
	}

	// Token: 0x0600188B RID: 6283 RVA: 0x0007F651 File Offset: 0x0007D851
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		this.PlaySound(behaviour);
	}

	// Token: 0x0600188C RID: 6284 RVA: 0x0007F65C File Offset: 0x0007D85C
	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		if (base.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component == null)
			{
				Debug.Log(behaviour.name + " (UI Object) is missing LoopingSounds component.");
				return;
			}
			if (!component.StartSound(base.sound, false, false, false))
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					string.Format("SoundEvent has invalid sound [{0}] on behaviour [{1}]", base.sound, behaviour.name)
				});
				return;
			}
		}
		else
		{
			try
			{
				if (SoundListenerController.Instance == null)
				{
					KFMOD.PlayUISound(base.sound);
				}
				else
				{
					KFMOD.PlayOneShot(base.sound, SoundListenerController.Instance.transform.GetPosition(), 1f);
				}
			}
			catch
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"AUDIOERROR: Missing [" + base.sound + "]"
				});
			}
		}
	}

	// Token: 0x0600188D RID: 6285 RVA: 0x0007F748 File Offset: 0x0007D948
	public override void Stop(AnimEventManager.EventPlayerData behaviour)
	{
		if (base.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component != null)
			{
				component.StopSound(base.sound);
			}
		}
	}
}
