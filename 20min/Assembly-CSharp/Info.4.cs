using System;

// Token: 0x0200003D RID: 61
public class Info<T0, T1, T2, T3> : Info<T0, T1, T2>
{
	// Token: 0x060003D5 RID: 981 RVA: 0x00014D79 File Offset: 0x00012F79
	public Info(T0 arg0, T1 arg1, T2 arg2, T3 arg3) : base(arg0, arg1, arg2)
	{
		this.arg3 = arg3;
	}

	// Token: 0x040001D8 RID: 472
	public T3 arg3;
}
