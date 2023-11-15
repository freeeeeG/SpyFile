using System;

// Token: 0x0200003C RID: 60
public class Info<T0, T1, T2> : Info<T0, T1>
{
	// Token: 0x060003D4 RID: 980 RVA: 0x00014D68 File Offset: 0x00012F68
	public Info(T0 arg0, T1 arg1, T2 arg2) : base(arg0, arg1)
	{
		this.arg2 = arg2;
	}

	// Token: 0x040001D7 RID: 471
	public T2 arg2;
}
