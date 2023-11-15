using System;

// Token: 0x02000075 RID: 117
public static class eTowerTargetPriorityExtensions
{
	// Token: 0x06000285 RID: 645 RVA: 0x0000AB72 File Offset: 0x00008D72
	public static string GetLocKey(this eTowerTargetPriority type)
	{
		switch (type)
		{
		case eTowerTargetPriority.PROGRESS:
			return "TOWER_TARGET_PRIORITY_PROGRESS";
		case eTowerTargetPriority.HIGHEST_HP:
			return "TOWER_TARGET_PRIORITY_HIGHEST_HP";
		case eTowerTargetPriority.LOWEST_HP:
			return "TOWER_TARGET_PRIORITY_LOWEST_HP";
		case eTowerTargetPriority.NEAREST:
			return "TOWER_TARGET_PRIORITY_NEAREST";
		case eTowerTargetPriority.FARTHEST:
			return "TOWER_TARGET_PRIORITY_FARTHEST";
		default:
			return "";
		}
	}

	// Token: 0x06000286 RID: 646 RVA: 0x0000ABB1 File Offset: 0x00008DB1
	public static eTowerTargetPriority GetNext(this eTowerTargetPriority type)
	{
		return (type + 1) % (eTowerTargetPriority)Enum.GetValues(typeof(eTowerTargetPriority)).Length;
	}
}
