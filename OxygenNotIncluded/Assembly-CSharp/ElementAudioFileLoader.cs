using System;

// Token: 0x02000592 RID: 1426
internal class ElementAudioFileLoader : AsyncCsvLoader<ElementAudioFileLoader, ElementsAudio.ElementAudioConfig>
{
	// Token: 0x060022A1 RID: 8865 RVA: 0x000BE299 File Offset: 0x000BC499
	public ElementAudioFileLoader() : base(Assets.instance.elementAudio)
	{
	}

	// Token: 0x060022A2 RID: 8866 RVA: 0x000BE2AB File Offset: 0x000BC4AB
	public override void Run()
	{
		base.Run();
	}
}
