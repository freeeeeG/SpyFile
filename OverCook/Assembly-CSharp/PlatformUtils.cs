using System;
using UnityEngine;

// Token: 0x02000A46 RID: 2630
public class PlatformUtils : MonoBehaviour
{
	// Token: 0x060033FF RID: 13311 RVA: 0x000F3A02 File Offset: 0x000F1E02
	public static bool HasPlatformFlag(int _mask)
	{
		return MaskUtils.HasFlag<PlatformUtils.Platforms>(_mask, PlatformUtils.Platforms.PC);
	}

	// Token: 0x06003400 RID: 13312 RVA: 0x000F3A0B File Offset: 0x000F1E0B
	public static PlatformUtils.Platforms GetCurrentPlatform()
	{
		return PlatformUtils.Platforms.PC;
	}

	// Token: 0x06003401 RID: 13313 RVA: 0x000F3A0E File Offset: 0x000F1E0E
	public static bool HasOperatingSystemFlag(int _mask)
	{
		return MaskUtils.HasFlag<PlatformUtils.OperatingSystem>(_mask, PlatformUtils.OperatingSystem.Windows);
	}

	// Token: 0x06003402 RID: 13314 RVA: 0x000F3A17 File Offset: 0x000F1E17
	public static PlatformUtils.OperatingSystem GetCurrentOperatingSystem()
	{
		return PlatformUtils.OperatingSystem.Windows;
	}

	// Token: 0x040029B0 RID: 10672
	public static readonly int s_PlatformCount = 4;

	// Token: 0x02000A47 RID: 2631
	public enum Platforms
	{
		// Token: 0x040029B2 RID: 10674
		PC,
		// Token: 0x040029B3 RID: 10675
		XboxOne,
		// Token: 0x040029B4 RID: 10676
		PS4,
		// Token: 0x040029B5 RID: 10677
		NX
	}

	// Token: 0x02000A48 RID: 2632
	public enum OperatingSystem
	{
		// Token: 0x040029B7 RID: 10679
		Unknown,
		// Token: 0x040029B8 RID: 10680
		Windows,
		// Token: 0x040029B9 RID: 10681
		OSX,
		// Token: 0x040029BA RID: 10682
		Linux
	}
}
