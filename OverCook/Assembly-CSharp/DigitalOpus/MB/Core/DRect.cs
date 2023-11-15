using System;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x0200004A RID: 74
	public struct DRect
	{
		// Token: 0x0600020C RID: 524 RVA: 0x0001891B File Offset: 0x00016D1B
		public DRect(Rect r)
		{
			this.x = (double)r.x;
			this.y = (double)r.y;
			this.width = (double)r.width;
			this.height = (double)r.height;
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00018955 File Offset: 0x00016D55
		public DRect(Vector2 o, Vector2 s)
		{
			this.x = (double)o.x;
			this.y = (double)o.y;
			this.width = (double)s.x;
			this.height = (double)s.y;
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0001898F File Offset: 0x00016D8F
		public DRect(float xx, float yy, float w, float h)
		{
			this.x = (double)xx;
			this.y = (double)yy;
			this.width = (double)w;
			this.height = (double)h;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x000189B2 File Offset: 0x00016DB2
		public DRect(double xx, double yy, double w, double h)
		{
			this.x = xx;
			this.y = yy;
			this.width = w;
			this.height = h;
		}

		// Token: 0x06000210 RID: 528 RVA: 0x000189D1 File Offset: 0x00016DD1
		public Rect GetRect()
		{
			return new Rect((float)this.x, (float)this.y, (float)this.width, (float)this.height);
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000211 RID: 529 RVA: 0x000189F4 File Offset: 0x00016DF4
		public Vector2 min
		{
			get
			{
				return new Vector2((float)this.x, (float)this.y);
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000212 RID: 530 RVA: 0x00018A09 File Offset: 0x00016E09
		public Vector2 max
		{
			get
			{
				return new Vector2((float)(this.x + this.width), (float)(this.y + this.width));
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000213 RID: 531 RVA: 0x00018A2C File Offset: 0x00016E2C
		public Vector2 size
		{
			get
			{
				return new Vector2((float)this.width, (float)this.width);
			}
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00018A44 File Offset: 0x00016E44
		public override bool Equals(object obj)
		{
			DRect drect = (DRect)obj;
			return drect.x == this.x && drect.y == this.y && drect.width == this.width && drect.height == this.height;
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00018AA3 File Offset: 0x00016EA3
		public static bool operator ==(DRect a, DRect b)
		{
			return a.Equals(b);
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00018AB8 File Offset: 0x00016EB8
		public static bool operator !=(DRect a, DRect b)
		{
			return !a.Equals(b);
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00018AD0 File Offset: 0x00016ED0
		public override string ToString()
		{
			return string.Format("(x={0},y={1},w={2},h={3})", new object[]
			{
				this.x.ToString("F5"),
				this.y.ToString("F5"),
				this.width.ToString("F5"),
				this.height.ToString("F5")
			});
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00018B3C File Offset: 0x00016F3C
		public bool Encloses(DRect smallToTestIfFits)
		{
			double num = smallToTestIfFits.x;
			double num2 = smallToTestIfFits.y;
			double num3 = smallToTestIfFits.x + smallToTestIfFits.width;
			double num4 = smallToTestIfFits.y + smallToTestIfFits.height;
			double num5 = this.x;
			double num6 = this.y;
			double num7 = this.x + this.width;
			double num8 = this.y + this.height;
			return num5 <= num && num <= num7 && num5 <= num3 && num3 <= num7 && num6 <= num2 && num2 <= num8 && num6 <= num4 && num4 <= num8;
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00018BEC File Offset: 0x00016FEC
		public override int GetHashCode()
		{
			return this.x.GetHashCode() ^ this.y.GetHashCode() ^ this.width.GetHashCode() ^ this.height.GetHashCode();
		}

		// Token: 0x0400015D RID: 349
		public double x;

		// Token: 0x0400015E RID: 350
		public double y;

		// Token: 0x0400015F RID: 351
		public double width;

		// Token: 0x04000160 RID: 352
		public double height;
	}
}
