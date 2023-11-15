using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000442 RID: 1090
public class CreatureChewSoundEvent : SoundEvent
{
	// Token: 0x06001710 RID: 5904 RVA: 0x0007787C File Offset: 0x00075A7C
	public CreatureChewSoundEvent(string file_name, string sound_name, int frame, float min_interval) : base(file_name, sound_name, frame, false, false, min_interval, true)
	{
	}

	// Token: 0x06001711 RID: 5905 RVA: 0x0007788C File Offset: 0x00075A8C
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		string sound = GlobalAssets.GetSound(StringFormatter.Combine(base.name, "_", CreatureChewSoundEvent.GetChewSound(behaviour)), false);
		GameObject gameObject = behaviour.controller.gameObject;
		base.objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(gameObject);
		if (base.objectIsSelectedAndVisible || SoundEvent.ShouldPlaySound(behaviour.controller, sound, base.looping, this.isDynamic))
		{
			Vector3 vector = behaviour.GetComponent<Transform>().GetPosition();
			vector.z = 0f;
			if (base.objectIsSelectedAndVisible)
			{
				vector = SoundEvent.AudioHighlightListenerPosition(vector);
			}
			EventInstance instance = SoundEvent.BeginOneShot(sound, vector, SoundEvent.GetVolume(base.objectIsSelectedAndVisible), false);
			if (behaviour.controller.gameObject.GetDef<BabyMonitor.Def>() != null)
			{
				instance.setParameterByName("isBaby", 1f, false);
			}
			SoundEvent.EndOneShot(instance);
		}
	}

	// Token: 0x06001712 RID: 5906 RVA: 0x00077958 File Offset: 0x00075B58
	private static string GetChewSound(AnimEventManager.EventPlayerData behaviour)
	{
		string result = CreatureChewSoundEvent.DEFAULT_CHEW_SOUND;
		EatStates.Instance smi = behaviour.controller.GetSMI<EatStates.Instance>();
		if (smi != null)
		{
			Element latestMealElement = smi.GetLatestMealElement();
			if (latestMealElement != null)
			{
				string creatureChewSound = latestMealElement.substance.GetCreatureChewSound();
				if (!string.IsNullOrEmpty(creatureChewSound))
				{
					result = creatureChewSound;
				}
			}
		}
		return result;
	}

	// Token: 0x04000CD0 RID: 3280
	private static string DEFAULT_CHEW_SOUND = "Rock";

	// Token: 0x04000CD1 RID: 3281
	private const string FMOD_PARAM_IS_BABY_ID = "isBaby";
}
