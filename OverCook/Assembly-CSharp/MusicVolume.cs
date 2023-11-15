using System;

// Token: 0x02000A40 RID: 2624
internal class MusicVolume : AudioVolume
{
	// Token: 0x1700039F RID: 927
	// (get) Token: 0x060033E2 RID: 13282 RVA: 0x000F38D3 File Offset: 0x000F1CD3
	public override string Label
	{
		get
		{
			return "OptionType.MusicVolume";
		}
	}

	// Token: 0x170003A0 RID: 928
	// (get) Token: 0x060033E3 RID: 13283 RVA: 0x000F38DA File Offset: 0x000F1CDA
	protected override string MixerControlName
	{
		get
		{
			return "MusicVolume";
		}
	}

	// Token: 0x170003A1 RID: 929
	// (get) Token: 0x060033E4 RID: 13284 RVA: 0x000F38E1 File Offset: 0x000F1CE1
	protected override float MaxAudioVolume
	{
		get
		{
			return 0f;
		}
	}
}
