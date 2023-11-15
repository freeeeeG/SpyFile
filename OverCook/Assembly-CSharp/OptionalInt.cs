using System;

// Token: 0x0200026C RID: 620
[Serializable]
public class OptionalInt : Optional<int>
{
	// Token: 0x06000BAD RID: 2989 RVA: 0x0003DA1E File Offset: 0x0003BE1E
	public OptionalInt(int _value) : base(_value)
	{
	}

	// Token: 0x06000BAE RID: 2990 RVA: 0x0003DA27 File Offset: 0x0003BE27
	public OptionalInt()
	{
	}

	// Token: 0x06000BAF RID: 2991 RVA: 0x0003DA2F File Offset: 0x0003BE2F
	public static implicit operator OptionalInt(int _value)
	{
		return new OptionalInt(_value);
	}
}
