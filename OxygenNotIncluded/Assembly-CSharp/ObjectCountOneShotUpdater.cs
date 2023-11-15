using System;
using System.Collections.Generic;

// Token: 0x0200045C RID: 1116
internal class ObjectCountOneShotUpdater : OneShotSoundParameterUpdater
{
	// Token: 0x06001873 RID: 6259 RVA: 0x0007EDD7 File Offset: 0x0007CFD7
	public ObjectCountOneShotUpdater() : base("objectCount")
	{
	}

	// Token: 0x06001874 RID: 6260 RVA: 0x0007EDF4 File Offset: 0x0007CFF4
	public override void Update(float dt)
	{
		this.soundCounts.Clear();
	}

	// Token: 0x06001875 RID: 6261 RVA: 0x0007EE04 File Offset: 0x0007D004
	public override void Play(OneShotSoundParameterUpdater.Sound sound)
	{
		UpdateObjectCountParameter.Settings settings = UpdateObjectCountParameter.GetSettings(sound.path, sound.description);
		int num = 0;
		this.soundCounts.TryGetValue(sound.path, out num);
		num = (this.soundCounts[sound.path] = num + 1);
		UpdateObjectCountParameter.ApplySettings(sound.ev, num, settings);
	}

	// Token: 0x04000D7F RID: 3455
	private Dictionary<HashedString, int> soundCounts = new Dictionary<HashedString, int>();
}
