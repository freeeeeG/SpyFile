using System;

namespace ExternalPropertyAttributes
{
	// Token: 0x02000027 RID: 39
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public class HorizontalLineAttribute : DrawerAttribute
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000069 RID: 105 RVA: 0x000034E8 File Offset: 0x000016E8
		// (set) Token: 0x0600006A RID: 106 RVA: 0x000034F0 File Offset: 0x000016F0
		public float Height { get; private set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600006B RID: 107 RVA: 0x000034F9 File Offset: 0x000016F9
		// (set) Token: 0x0600006C RID: 108 RVA: 0x00003501 File Offset: 0x00001701
		public EColor Color { get; private set; }

		// Token: 0x0600006D RID: 109 RVA: 0x0000350A File Offset: 0x0000170A
		public HorizontalLineAttribute(float height = 2f, EColor color = EColor.Gray)
		{
			this.Height = height;
			this.Color = color;
		}

		// Token: 0x0400004D RID: 77
		public const float DefaultHeight = 2f;

		// Token: 0x0400004E RID: 78
		public const EColor DefaultColor = EColor.Gray;
	}
}
