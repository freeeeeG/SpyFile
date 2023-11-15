using System;

// Token: 0x0200002E RID: 46
public class BaseException
{
	// Token: 0x1700002B RID: 43
	// (get) Token: 0x060003B6 RID: 950 RVA: 0x00014985 File Offset: 0x00012B85
	// (set) Token: 0x060003B7 RID: 951 RVA: 0x0001498D File Offset: 0x00012B8D
	public bool toggle { get; private set; }

	// Token: 0x060003B8 RID: 952 RVA: 0x00014996 File Offset: 0x00012B96
	public BaseException(bool defaultToggle)
	{
		this.defaultToggle = defaultToggle;
		this.toggle = defaultToggle;
	}

	// Token: 0x060003B9 RID: 953 RVA: 0x000149AC File Offset: 0x00012BAC
	public void FlipToggle()
	{
		this.toggle = !this.defaultToggle;
	}

	// Token: 0x040001CB RID: 459
	public readonly bool defaultToggle;
}
