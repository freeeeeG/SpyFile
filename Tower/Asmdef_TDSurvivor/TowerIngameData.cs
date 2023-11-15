using System;
using System.Collections.Generic;

// Token: 0x02000124 RID: 292
[Serializable]
public class TowerIngameData
{
	// Token: 0x0600077D RID: 1917 RVA: 0x0001C62F File Offset: 0x0001A82F
	public TowerIngameData(eItemType type, int level)
	{
		this.ItemType = type;
		this.Level = level;
	}

	// Token: 0x04000611 RID: 1553
	public eItemType ItemType;

	// Token: 0x04000612 RID: 1554
	public int Level;

	// Token: 0x04000613 RID: 1555
	public List<TowerStats> List_AdditionalBuff;
}
