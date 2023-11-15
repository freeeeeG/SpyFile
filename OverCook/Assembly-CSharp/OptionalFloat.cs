using System;

// Token: 0x0200026D RID: 621
[Serializable]
public class OptionalFloat : Optional<float>
{
	// Token: 0x06000BB0 RID: 2992 RVA: 0x0003DA37 File Offset: 0x0003BE37
	public OptionalFloat(float _value) : base(_value)
	{
	}

	// Token: 0x06000BB1 RID: 2993 RVA: 0x0003DA40 File Offset: 0x0003BE40
	public OptionalFloat()
	{
	}

	// Token: 0x06000BB2 RID: 2994 RVA: 0x0003DA48 File Offset: 0x0003BE48
	public static implicit operator OptionalFloat(float _value)
	{
		return new OptionalFloat(_value);
	}
}
