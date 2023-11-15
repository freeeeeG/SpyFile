using System;
using UnityEngine;

// Token: 0x0200026F RID: 623
[Serializable]
public class OptionalVector3 : Optional<Vector3>
{
	// Token: 0x06000BB6 RID: 2998 RVA: 0x0003DA69 File Offset: 0x0003BE69
	public OptionalVector3(Vector3 _value) : base(_value)
	{
	}

	// Token: 0x06000BB7 RID: 2999 RVA: 0x0003DA72 File Offset: 0x0003BE72
	public OptionalVector3()
	{
	}

	// Token: 0x06000BB8 RID: 3000 RVA: 0x0003DA7A File Offset: 0x0003BE7A
	public static implicit operator OptionalVector3(Vector3 _value)
	{
		return new OptionalVector3(_value);
	}
}
