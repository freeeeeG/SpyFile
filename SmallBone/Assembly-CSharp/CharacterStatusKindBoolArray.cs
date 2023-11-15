using System;
using Characters;

// Token: 0x020000B7 RID: 183
[Serializable]
public class CharacterStatusKindBoolArray : EnumArray<CharacterStatus.Kind, bool>
{
	// Token: 0x0600038D RID: 909 RVA: 0x0000CF04 File Offset: 0x0000B104
	public CharacterStatusKindBoolArray(params bool[] values)
	{
		int num = Math.Min(base.Array.Length, values.Length);
		for (int i = 0; i < num; i++)
		{
			base.Array[i] = values[i];
		}
	}
}
