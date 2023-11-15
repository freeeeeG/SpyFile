using System;
using UnityEngine;

// Token: 0x0200026E RID: 622
[Serializable]
public class OptionalVector2 : Optional<Vector2>
{
	// Token: 0x06000BB3 RID: 2995 RVA: 0x0003DA50 File Offset: 0x0003BE50
	public OptionalVector2(Vector2 _value) : base(_value)
	{
	}

	// Token: 0x06000BB4 RID: 2996 RVA: 0x0003DA59 File Offset: 0x0003BE59
	public OptionalVector2()
	{
	}

	// Token: 0x06000BB5 RID: 2997 RVA: 0x0003DA61 File Offset: 0x0003BE61
	public static implicit operator OptionalVector2(Vector2 _value)
	{
		return new OptionalVector2(_value);
	}
}
