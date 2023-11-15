using System;

// Token: 0x0200003B RID: 59
public class Info<T0, T1> : Info<T0>
{
	// Token: 0x060003D3 RID: 979 RVA: 0x00014D58 File Offset: 0x00012F58
	public Info(T0 arg0, T1 arg1) : base(arg0)
	{
		this.arg1 = arg1;
	}

	// Token: 0x040001D6 RID: 470
	public T1 arg1;
}
