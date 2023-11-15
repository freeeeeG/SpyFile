using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000139 RID: 313
[Serializable]
public class ContentStruct
{
	// Token: 0x04000400 RID: 1024
	public int ContentType;

	// Token: 0x04000401 RID: 1025
	public Vector2Int Pos;

	// Token: 0x04000402 RID: 1026
	public int Direction;

	// Token: 0x04000403 RID: 1027
	public string ContentName;

	// Token: 0x04000404 RID: 1028
	public int Element;

	// Token: 0x04000405 RID: 1029
	public int Quality;

	// Token: 0x04000406 RID: 1030
	public string TotalDamage;

	// Token: 0x04000407 RID: 1031
	public int ExtraSlot;

	// Token: 0x04000408 RID: 1032
	public bool TrapRevealed;

	// Token: 0x04000409 RID: 1033
	public bool IsAbnormalBuilding;

	// Token: 0x0400040A RID: 1034
	public Dictionary<string, List<int>> SkillList;

	// Token: 0x0400040B RID: 1035
	public Dictionary<string, List<int>> ElementsList;

	// Token: 0x0400040C RID: 1036
	public Dictionary<string, bool> IsException;
}
