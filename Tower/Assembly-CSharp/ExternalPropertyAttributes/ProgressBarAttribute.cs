using System;

namespace ExternalPropertyAttributes
{
	// Token: 0x0200002C RID: 44
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class ProgressBarAttribute : DrawerAttribute
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00003598 File Offset: 0x00001798
		// (set) Token: 0x0600007A RID: 122 RVA: 0x000035A0 File Offset: 0x000017A0
		public string Name { get; private set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600007B RID: 123 RVA: 0x000035A9 File Offset: 0x000017A9
		// (set) Token: 0x0600007C RID: 124 RVA: 0x000035B1 File Offset: 0x000017B1
		public float MaxValue { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600007D RID: 125 RVA: 0x000035BA File Offset: 0x000017BA
		// (set) Token: 0x0600007E RID: 126 RVA: 0x000035C2 File Offset: 0x000017C2
		public string MaxValueName { get; private set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600007F RID: 127 RVA: 0x000035CB File Offset: 0x000017CB
		// (set) Token: 0x06000080 RID: 128 RVA: 0x000035D3 File Offset: 0x000017D3
		public EColor Color { get; private set; }

		// Token: 0x06000081 RID: 129 RVA: 0x000035DC File Offset: 0x000017DC
		public ProgressBarAttribute(string name, int maxValue, EColor color = EColor.Blue)
		{
			this.Name = name;
			this.MaxValue = (float)maxValue;
			this.Color = color;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000035FA File Offset: 0x000017FA
		public ProgressBarAttribute(string name, string maxValueName, EColor color = EColor.Blue)
		{
			this.Name = name;
			this.MaxValueName = maxValueName;
			this.Color = color;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003617 File Offset: 0x00001817
		public ProgressBarAttribute(int maxValue, EColor color = EColor.Blue) : this("", maxValue, color)
		{
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003626 File Offset: 0x00001826
		public ProgressBarAttribute(string maxValueName, EColor color = EColor.Blue) : this("", maxValueName, color)
		{
		}
	}
}
