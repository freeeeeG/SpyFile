using System;
using Characters;

// Token: 0x020000B0 RID: 176
[Serializable]
public class MotionTypeBoolArray : EnumArray<Damage.MotionType, bool>
{
	// Token: 0x06000380 RID: 896 RVA: 0x0000CDD1 File Offset: 0x0000AFD1
	public MotionTypeBoolArray()
	{
	}

	// Token: 0x06000381 RID: 897 RVA: 0x0000CDDC File Offset: 0x0000AFDC
	public MotionTypeBoolArray(params bool[] values)
	{
		int num = Math.Min(base.Array.Length, values.Length);
		for (int i = 0; i < num; i++)
		{
			base.Array[i] = values[i];
		}
	}
}
