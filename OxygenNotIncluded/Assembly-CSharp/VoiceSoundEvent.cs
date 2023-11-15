using System;
using FMOD.Studio;
using Klei.AI;
using UnityEngine;

// Token: 0x02000461 RID: 1121
public class VoiceSoundEvent : SoundEvent
{
	// Token: 0x06001892 RID: 6290 RVA: 0x0007F92D File Offset: 0x0007DB2D
	public VoiceSoundEvent(string file_name, string sound_name, int frame, bool is_looping) : base(file_name, sound_name, frame, false, is_looping, (float)SoundEvent.IGNORE_INTERVAL, true)
	{
		base.noiseValues = SoundEventVolumeCache.instance.GetVolume("VoiceSoundEvent", sound_name);
	}

	// Token: 0x06001893 RID: 6291 RVA: 0x0007F963 File Offset: 0x0007DB63
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		VoiceSoundEvent.PlayVoice(base.name, behaviour.controller, this.intervalBetweenSpeaking, base.looping, false);
	}

	// Token: 0x06001894 RID: 6292 RVA: 0x0007F984 File Offset: 0x0007DB84
	public static EventInstance PlayVoice(string name, KBatchedAnimController controller, float interval_between_speaking, bool looping, bool objectIsSelectedAndVisible = false)
	{
		EventInstance eventInstance = default(EventInstance);
		MinionIdentity component = controller.GetComponent<MinionIdentity>();
		if (component == null || (name.Contains("state") && Time.time - component.timeLastSpoke < interval_between_speaking))
		{
			return eventInstance;
		}
		if (name.Contains(":"))
		{
			float num = float.Parse(name.Split(new char[]
			{
				':'
			})[1]);
			if ((float)UnityEngine.Random.Range(0, 100) > num)
			{
				return eventInstance;
			}
		}
		Worker component2 = controller.GetComponent<Worker>();
		string assetName = VoiceSoundEvent.GetAssetName(name, component2);
		StaminaMonitor.Instance smi = component2.GetSMI<StaminaMonitor.Instance>();
		if (!name.Contains("sleep_") && smi != null && smi.IsSleeping())
		{
			return eventInstance;
		}
		Vector3 vector = component2.transform.GetPosition();
		vector.z = 0f;
		if (SoundEvent.ObjectIsSelectedAndVisible(controller.gameObject))
		{
			vector = SoundEvent.AudioHighlightListenerPosition(vector);
		}
		string sound = GlobalAssets.GetSound(assetName, true);
		if (!SoundEvent.ShouldPlaySound(controller, sound, looping, false))
		{
			return eventInstance;
		}
		if (sound != null)
		{
			if (looping)
			{
				LoopingSounds component3 = controller.GetComponent<LoopingSounds>();
				if (component3 == null)
				{
					global::Debug.Log(controller.name + " is missing LoopingSounds component. ");
				}
				else if (!component3.StartSound(sound))
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						string.Format("SoundEvent has invalid sound [{0}] on behaviour [{1}]", sound, controller.name)
					});
				}
			}
			else
			{
				eventInstance = SoundEvent.BeginOneShot(sound, vector, 1f, false);
				if (sound.Contains("sleep_") && controller.GetComponent<Traits>().HasTrait("Snorer"))
				{
					eventInstance.setParameterByName("snoring", 1f, false);
				}
				SoundEvent.EndOneShot(eventInstance);
				component.timeLastSpoke = Time.time;
			}
		}
		else if (AudioDebug.Get().debugVoiceSounds)
		{
			global::Debug.LogWarning("Missing voice sound: " + assetName);
		}
		return eventInstance;
	}

	// Token: 0x06001895 RID: 6293 RVA: 0x0007FB50 File Offset: 0x0007DD50
	private static string GetAssetName(string name, Component cmp)
	{
		string b = "F01";
		if (cmp != null)
		{
			MinionIdentity component = cmp.GetComponent<MinionIdentity>();
			if (component != null)
			{
				b = component.GetVoiceId();
			}
		}
		string d = name;
		if (name.Contains(":"))
		{
			d = name.Split(new char[]
			{
				':'
			})[0];
		}
		return StringFormatter.Combine("DupVoc_", b, "_", d);
	}

	// Token: 0x06001896 RID: 6294 RVA: 0x0007FBB8 File Offset: 0x0007DDB8
	public override void Stop(AnimEventManager.EventPlayerData behaviour)
	{
		if (base.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component != null)
			{
				string sound = GlobalAssets.GetSound(VoiceSoundEvent.GetAssetName(base.name, component), true);
				component.StopSound(sound);
			}
		}
	}

	// Token: 0x04000D89 RID: 3465
	public static float locomotionSoundProb = 50f;

	// Token: 0x04000D8A RID: 3466
	public float timeLastSpoke;

	// Token: 0x04000D8B RID: 3467
	public float intervalBetweenSpeaking = 10f;
}
