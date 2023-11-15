using System;

// Token: 0x020008D6 RID: 2262
public class PassiveElementConsumer : ElementConsumer, IGameObjectEffectDescriptor
{
	// Token: 0x06004173 RID: 16755 RVA: 0x0016E6A2 File Offset: 0x0016C8A2
	protected override bool IsActive()
	{
		return true;
	}
}
