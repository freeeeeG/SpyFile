using System;

// Token: 0x02000453 RID: 1107
public class MouthFlapSoundEvent : SoundEvent
{
	// Token: 0x06001833 RID: 6195 RVA: 0x0007DB18 File Offset: 0x0007BD18
	public MouthFlapSoundEvent(string file_name, string sound_name, int frame, bool is_looping) : base(file_name, sound_name, frame, false, is_looping, (float)SoundEvent.IGNORE_INTERVAL, true)
	{
	}

	// Token: 0x06001834 RID: 6196 RVA: 0x0007DB2D File Offset: 0x0007BD2D
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		behaviour.controller.GetSMI<SpeechMonitor.Instance>().PlaySpeech(base.name, null);
	}
}
