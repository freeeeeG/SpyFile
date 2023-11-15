using System;

namespace GameResources
{
	// Token: 0x0200018A RID: 394
	public static class MapReferenceExtension
	{
		// Token: 0x0600088D RID: 2189 RVA: 0x00018C89 File Offset: 0x00016E89
		public static bool IsNullOrEmpty(this MapReference mapReference)
		{
			return mapReference == null || mapReference.empty;
		}
	}
}
