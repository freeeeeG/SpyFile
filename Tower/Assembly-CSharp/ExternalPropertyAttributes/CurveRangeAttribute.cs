using System;
using UnityEngine;

namespace ExternalPropertyAttributes
{
	// Token: 0x02000020 RID: 32
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class CurveRangeAttribute : DrawerAttribute
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00003390 File Offset: 0x00001590
		// (set) Token: 0x06000056 RID: 86 RVA: 0x00003398 File Offset: 0x00001598
		public Vector2 Min { get; private set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000057 RID: 87 RVA: 0x000033A1 File Offset: 0x000015A1
		// (set) Token: 0x06000058 RID: 88 RVA: 0x000033A9 File Offset: 0x000015A9
		public Vector2 Max { get; private set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000059 RID: 89 RVA: 0x000033B2 File Offset: 0x000015B2
		// (set) Token: 0x0600005A RID: 90 RVA: 0x000033BA File Offset: 0x000015BA
		public EColor Color { get; private set; }

		// Token: 0x0600005B RID: 91 RVA: 0x000033C3 File Offset: 0x000015C3
		public CurveRangeAttribute(Vector2 min, Vector2 max, EColor color = EColor.Clear)
		{
			this.Min = min;
			this.Max = max;
			this.Color = color;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000033E0 File Offset: 0x000015E0
		public CurveRangeAttribute(EColor color) : this(Vector2.zero, Vector2.one, color)
		{
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000033F3 File Offset: 0x000015F3
		public CurveRangeAttribute(float minX, float minY, float maxX, float maxY, EColor color = EColor.Clear) : this(new Vector2(minX, minY), new Vector2(maxX, maxY), color)
		{
		}
	}
}
