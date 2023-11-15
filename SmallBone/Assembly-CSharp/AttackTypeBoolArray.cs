using System;
using Characters;

// Token: 0x020000B1 RID: 177
[Serializable]
public class AttackTypeBoolArray : EnumArray<Damage.AttackType, bool>
{
	// Token: 0x06000382 RID: 898 RVA: 0x0000CE17 File Offset: 0x0000B017
	public AttackTypeBoolArray()
	{
	}

	// Token: 0x06000383 RID: 899 RVA: 0x0000CE20 File Offset: 0x0000B020
	public AttackTypeBoolArray(params bool[] values)
	{
		int num = Math.Min(base.Array.Length, values.Length);
		for (int i = 0; i < num; i++)
		{
			base.Array[i] = values[i];
		}
	}
}
