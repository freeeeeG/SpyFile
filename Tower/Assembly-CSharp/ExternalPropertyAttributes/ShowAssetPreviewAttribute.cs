using System;

namespace ExternalPropertyAttributes
{
	// Token: 0x02000030 RID: 48
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class ShowAssetPreviewAttribute : DrawerAttribute
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000088 RID: 136 RVA: 0x0000364D File Offset: 0x0000184D
		// (set) Token: 0x06000089 RID: 137 RVA: 0x00003655 File Offset: 0x00001855
		public int Width { get; private set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600008A RID: 138 RVA: 0x0000365E File Offset: 0x0000185E
		// (set) Token: 0x0600008B RID: 139 RVA: 0x00003666 File Offset: 0x00001866
		public int Height { get; private set; }

		// Token: 0x0600008C RID: 140 RVA: 0x0000366F File Offset: 0x0000186F
		public ShowAssetPreviewAttribute(int width = 64, int height = 64)
		{
			this.Width = width;
			this.Height = height;
		}

		// Token: 0x0400005D RID: 93
		public const int DefaultWidth = 64;

		// Token: 0x0400005E RID: 94
		public const int DefaultHeight = 64;
	}
}
