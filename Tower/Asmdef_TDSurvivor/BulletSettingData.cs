using System;
using UnityEngine;

// Token: 0x0200003A RID: 58
[CreateAssetMenu(fileName = "Data", menuName = "設定檔/BulletAssetData", order = 1)]
public class BulletSettingData : ScriptableObject
{
	// Token: 0x0600011B RID: 283 RVA: 0x000055D2 File Offset: 0x000037D2
	public int GetDamage(float multiplier = 1f)
	{
		if (this.baseDamageMin == this.baseDamageMax)
		{
			return (int)((float)this.baseDamageMin * multiplier);
		}
		return (int)((float)Random.Range(this.baseDamageMin, this.baseDamageMax + 1) * multiplier);
	}

	// Token: 0x040000C5 RID: 197
	[SerializeField]
	private int baseDamageMin = 1;

	// Token: 0x040000C6 RID: 198
	[SerializeField]
	private int baseDamageMax = 1;
}
