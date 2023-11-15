using System;
using UnityEngine;

// Token: 0x02000460 RID: 1120
public class UIAnimationVoiceSoundEvent : SoundEvent
{
	// Token: 0x0600188E RID: 6286 RVA: 0x0007F77A File Offset: 0x0007D97A
	public UIAnimationVoiceSoundEvent(string file_name, string sound_name, int frame, bool looping) : base(file_name, sound_name, frame, false, looping, (float)SoundEvent.IGNORE_INTERVAL, false)
	{
		this.actualSoundName = sound_name;
	}

	// Token: 0x0600188F RID: 6287 RVA: 0x0007F796 File Offset: 0x0007D996
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		this.PlaySound(behaviour);
	}

	// Token: 0x06001890 RID: 6288 RVA: 0x0007F7A0 File Offset: 0x0007D9A0
	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		string soundPath = MinionVoice.ByObject(behaviour.controller).UnwrapOr(MinionVoice.Random(), string.Format("Couldn't find MinionVoice on UI {0}, falling back to random voice", behaviour.controller)).GetSoundPath(this.actualSoundName);
		if (this.actualSoundName.Contains(":"))
		{
			float num = float.Parse(this.actualSoundName.Split(new char[]
			{
				':'
			})[1]);
			if ((float)UnityEngine.Random.Range(0, 100) > num)
			{
				return;
			}
		}
		if (base.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component == null)
			{
				global::Debug.Log(behaviour.name + " (UI Object) is missing LoopingSounds component.");
			}
			else if (!component.StartSound(soundPath, false, false, false))
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					string.Format("SoundEvent has invalid sound [{0}] on behaviour [{1}]", soundPath, behaviour.name)
				});
			}
			this.lastPlayedLoopingSoundPath = soundPath;
			return;
		}
		try
		{
			if (SoundListenerController.Instance == null)
			{
				KFMOD.PlayUISound(soundPath);
			}
			else
			{
				KFMOD.PlayOneShot(soundPath, SoundListenerController.Instance.transform.GetPosition(), 1f);
			}
		}
		catch
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"AUDIOERROR: Missing [" + soundPath + "]"
			});
		}
	}

	// Token: 0x06001891 RID: 6289 RVA: 0x0007F8EC File Offset: 0x0007DAEC
	public override void Stop(AnimEventManager.EventPlayerData behaviour)
	{
		if (base.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component != null && this.lastPlayedLoopingSoundPath != null)
			{
				component.StopSound(this.lastPlayedLoopingSoundPath);
			}
		}
		this.lastPlayedLoopingSoundPath = null;
	}

	// Token: 0x04000D87 RID: 3463
	private string actualSoundName;

	// Token: 0x04000D88 RID: 3464
	private string lastPlayedLoopingSoundPath;
}
