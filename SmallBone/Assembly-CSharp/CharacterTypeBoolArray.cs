using System;
using Characters;

// Token: 0x020000B3 RID: 179
[Serializable]
public class CharacterTypeBoolArray : EnumArray<Character.Type, bool>
{
	// Token: 0x06000385 RID: 901 RVA: 0x0000CE63 File Offset: 0x0000B063
	public CharacterTypeBoolArray()
	{
	}

	// Token: 0x06000386 RID: 902 RVA: 0x0000CE6C File Offset: 0x0000B06C
	public CharacterTypeBoolArray(params bool[] values)
	{
		int num = Math.Min(base.Array.Length, values.Length);
		for (int i = 0; i < num; i++)
		{
			base.Array[i] = values[i];
		}
	}
}
