using System;

// Token: 0x02000A41 RID: 2625
internal class SFXVolume : AudioVolume
{
	// Token: 0x170003A2 RID: 930
	// (get) Token: 0x060033E6 RID: 13286 RVA: 0x000F38F0 File Offset: 0x000F1CF0
	public override string Label
	{
		get
		{
			return "OptionType.SFXVolume";
		}
	}

	// Token: 0x170003A3 RID: 931
	// (get) Token: 0x060033E7 RID: 13287 RVA: 0x000F38F7 File Offset: 0x000F1CF7
	protected override string MixerControlName
	{
		get
		{
			return "SFXVolume";
		}
	}
}
