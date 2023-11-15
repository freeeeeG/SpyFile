using System;

// Token: 0x02000451 RID: 1105
public class LaserSoundEvent : SoundEvent
{
	// Token: 0x06001830 RID: 6192 RVA: 0x0007DA94 File Offset: 0x0007BC94
	public LaserSoundEvent(string file_name, string sound_name, int frame, float min_interval) : base(file_name, sound_name, frame, true, true, min_interval, false)
	{
		base.noiseValues = SoundEventVolumeCache.instance.GetVolume("LaserSoundEvent", sound_name);
	}
}
