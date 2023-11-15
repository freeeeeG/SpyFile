using System;

// Token: 0x020000E0 RID: 224
public class SoundCooldown
{
	// Token: 0x06000545 RID: 1349 RVA: 0x0001524D File Offset: 0x0001344D
	public SoundCooldown(string _sndName, float _timer)
	{
		this.sndName = _sndName;
		this.timer = _timer;
	}

	// Token: 0x040004F2 RID: 1266
	public string sndName;

	// Token: 0x040004F3 RID: 1267
	public float timer;
}
