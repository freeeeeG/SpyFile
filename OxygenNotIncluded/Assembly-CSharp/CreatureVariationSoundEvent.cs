using System;

// Token: 0x02000443 RID: 1091
public class CreatureVariationSoundEvent : SoundEvent
{
	// Token: 0x06001714 RID: 5908 RVA: 0x000779A7 File Offset: 0x00075BA7
	public CreatureVariationSoundEvent(string file_name, string sound_name, int frame, bool do_load, bool is_looping, float min_interval, bool is_dynamic) : base(file_name, sound_name, frame, do_load, is_looping, min_interval, is_dynamic)
	{
	}

	// Token: 0x06001715 RID: 5909 RVA: 0x000779BC File Offset: 0x00075BBC
	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		string sound = base.sound;
		CreatureBrain component = behaviour.GetComponent<CreatureBrain>();
		if (component != null && !string.IsNullOrEmpty(component.symbolPrefix))
		{
			string sound2 = GlobalAssets.GetSound(StringFormatter.Combine(component.symbolPrefix, base.name), false);
			if (!string.IsNullOrEmpty(sound2))
			{
				sound = sound2;
			}
		}
		base.PlaySound(behaviour, sound);
	}
}
