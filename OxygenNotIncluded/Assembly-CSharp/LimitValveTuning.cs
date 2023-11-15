using System;

// Token: 0x02000207 RID: 519
public class LimitValveTuning
{
	// Token: 0x06000A69 RID: 2665 RVA: 0x0003C0C6 File Offset: 0x0003A2C6
	public static NonLinearSlider.Range[] GetDefaultSlider()
	{
		return new NonLinearSlider.Range[]
		{
			new NonLinearSlider.Range(70f, 100f),
			new NonLinearSlider.Range(30f, 500f)
		};
	}

	// Token: 0x04000656 RID: 1622
	public const float MAX_LIMIT = 500f;

	// Token: 0x04000657 RID: 1623
	public const float DEFAULT_LIMIT = 100f;
}
