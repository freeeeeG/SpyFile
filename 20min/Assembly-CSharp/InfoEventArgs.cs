using System;

// Token: 0x02000039 RID: 57
public class InfoEventArgs<T> : EventArgs
{
	// Token: 0x060003D0 RID: 976 RVA: 0x00014D26 File Offset: 0x00012F26
	public InfoEventArgs()
	{
		this.info = default(T);
	}

	// Token: 0x060003D1 RID: 977 RVA: 0x00014D3A File Offset: 0x00012F3A
	public InfoEventArgs(T info)
	{
		this.info = info;
	}

	// Token: 0x040001D4 RID: 468
	public T info;
}
