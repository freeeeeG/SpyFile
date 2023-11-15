using System;

// Token: 0x02000222 RID: 546
public class Singleton<T> where T : Singleton<T>, new()
{
	// Token: 0x0600092E RID: 2350 RVA: 0x000319AC File Offset: 0x0002FDAC
	public static T Get()
	{
		return Singleton<T>.s_this;
	}

	// Token: 0x040007EC RID: 2028
	private static T s_this = Activator.CreateInstance<T>();
}
