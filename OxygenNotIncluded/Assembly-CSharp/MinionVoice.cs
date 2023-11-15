using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200000E RID: 14
public readonly struct MinionVoice
{
	// Token: 0x06000038 RID: 56 RVA: 0x0000395C File Offset: 0x00001B5C
	public MinionVoice(int voiceIndex)
	{
		this.voiceIndex = voiceIndex;
		this.voiceId = (voiceIndex + 1).ToString("D2");
		this.isValid = true;
	}

	// Token: 0x06000039 RID: 57 RVA: 0x0000398D File Offset: 0x00001B8D
	public static MinionVoice ByPersonality(Personality personality)
	{
		return MinionVoice.ByPersonality(personality.Id);
	}

	// Token: 0x0600003A RID: 58 RVA: 0x0000399C File Offset: 0x00001B9C
	public static MinionVoice ByPersonality(string personalityId)
	{
		if (personalityId == "JORGE")
		{
			return new MinionVoice(-2);
		}
		if (personalityId == "MEEP")
		{
			return new MinionVoice(2);
		}
		MinionVoice minionVoice;
		if (!MinionVoice.personalityVoiceMap.TryGetValue(personalityId, out minionVoice))
		{
			minionVoice = MinionVoice.Random();
			MinionVoice.personalityVoiceMap.Add(personalityId, minionVoice);
		}
		return minionVoice;
	}

	// Token: 0x0600003B RID: 59 RVA: 0x000039F4 File Offset: 0x00001BF4
	public static MinionVoice Random()
	{
		return new MinionVoice(UnityEngine.Random.Range(0, 4));
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00003A04 File Offset: 0x00001C04
	public static Option<MinionVoice> ByObject(UnityEngine.Object unityObject)
	{
		GameObject gameObject = unityObject as GameObject;
		GameObject gameObject2;
		if (gameObject != null)
		{
			gameObject2 = gameObject;
		}
		else
		{
			Component component = unityObject as Component;
			if (component != null)
			{
				gameObject2 = component.gameObject;
			}
			else
			{
				gameObject2 = null;
			}
		}
		if (gameObject2.IsNullOrDestroyed())
		{
			return Option.None;
		}
		MinionVoiceProviderMB componentInParent = gameObject2.GetComponentInParent<MinionVoiceProviderMB>();
		if (componentInParent.IsNullOrDestroyed())
		{
			return Option.None;
		}
		return componentInParent.voice;
	}

	// Token: 0x0600003D RID: 61 RVA: 0x00003A68 File Offset: 0x00001C68
	public string GetSoundAssetName(string localName)
	{
		global::Debug.Assert(this.isValid);
		string d = localName;
		if (localName.Contains(":"))
		{
			d = localName.Split(new char[]
			{
				':'
			})[0];
		}
		return StringFormatter.Combine("DupVoc_", this.voiceId, "_", d);
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00003AB9 File Offset: 0x00001CB9
	public string GetSoundPath(string localName)
	{
		return GlobalAssets.GetSound(this.GetSoundAssetName(localName), true);
	}

	// Token: 0x0600003F RID: 63 RVA: 0x00003AC8 File Offset: 0x00001CC8
	public void PlaySoundUI(string localName)
	{
		global::Debug.Assert(this.isValid);
		string soundPath = this.GetSoundPath(localName);
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

	// Token: 0x0400002F RID: 47
	public readonly int voiceIndex;

	// Token: 0x04000030 RID: 48
	public readonly string voiceId;

	// Token: 0x04000031 RID: 49
	public readonly bool isValid;

	// Token: 0x04000032 RID: 50
	private static Dictionary<string, MinionVoice> personalityVoiceMap = new Dictionary<string, MinionVoice>();
}
