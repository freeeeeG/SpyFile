using System;
using Characters.Gear.Weapons;

// Token: 0x020000B4 RID: 180
[Serializable]
public class WeaponTypeBoolArray : EnumArray<Weapon.Category, bool>
{
	// Token: 0x06000387 RID: 903 RVA: 0x0000CEA7 File Offset: 0x0000B0A7
	public WeaponTypeBoolArray()
	{
	}

	// Token: 0x06000388 RID: 904 RVA: 0x0000CEB0 File Offset: 0x0000B0B0
	public WeaponTypeBoolArray(params bool[] values)
	{
		int num = Math.Min(base.Array.Length, values.Length);
		for (int i = 0; i < num; i++)
		{
			base.Array[i] = values[i];
		}
	}
}
