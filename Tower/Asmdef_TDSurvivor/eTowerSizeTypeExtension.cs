using System;

// Token: 0x02000073 RID: 115
public static class eTowerSizeTypeExtension
{
	// Token: 0x06000284 RID: 644 RVA: 0x0000AB1C File Offset: 0x00008D1C
	public static string GetString(this eTowerSizeType type)
	{
		switch (type)
		{
		case eTowerSizeType.NONE:
			return "未設定";
		case eTowerSizeType._1x1:
			return "1x1";
		case eTowerSizeType._1x2:
			return "1x2";
		case eTowerSizeType._1x3:
			return "1x3";
		case eTowerSizeType._2x2:
			return "2x2";
		case eTowerSizeType._3x3:
			return "3x3";
		default:
			return "未設定";
		}
	}
}
