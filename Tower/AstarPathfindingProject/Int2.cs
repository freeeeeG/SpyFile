using System;

namespace Pathfinding
{
	// Token: 0x0200003F RID: 63
	public struct Int2 : IEquatable<Int2>
	{
		// Token: 0x060002F0 RID: 752 RVA: 0x0001076B File Offset: 0x0000E96B
		public Int2(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x0001077B File Offset: 0x0000E97B
		public long sqrMagnitudeLong
		{
			get
			{
				return (long)this.x * (long)this.x + (long)this.y * (long)this.y;
			}
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0001079C File Offset: 0x0000E99C
		public static Int2 operator -(Int2 lhs)
		{
			lhs.x = -lhs.x;
			lhs.y = -lhs.y;
			return lhs;
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x000107BB File Offset: 0x0000E9BB
		public static Int2 operator +(Int2 a, Int2 b)
		{
			return new Int2(a.x + b.x, a.y + b.y);
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x000107DC File Offset: 0x0000E9DC
		public static Int2 operator -(Int2 a, Int2 b)
		{
			return new Int2(a.x - b.x, a.y - b.y);
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x000107FD File Offset: 0x0000E9FD
		public static bool operator ==(Int2 a, Int2 b)
		{
			return a.x == b.x && a.y == b.y;
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0001081D File Offset: 0x0000EA1D
		public static bool operator !=(Int2 a, Int2 b)
		{
			return a.x != b.x || a.y != b.y;
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x00010840 File Offset: 0x0000EA40
		public static long DotLong(Int2 a, Int2 b)
		{
			return (long)a.x * (long)b.x + (long)a.y * (long)b.y;
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00010864 File Offset: 0x0000EA64
		public override bool Equals(object o)
		{
			if (o == null)
			{
				return false;
			}
			Int2 @int = (Int2)o;
			return this.x == @int.x && this.y == @int.y;
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0001089B File Offset: 0x0000EA9B
		public bool Equals(Int2 other)
		{
			return this.x == other.x && this.y == other.y;
		}

		// Token: 0x060002FA RID: 762 RVA: 0x000108BB File Offset: 0x0000EABB
		public override int GetHashCode()
		{
			return this.x * 49157 + this.y * 98317;
		}

		// Token: 0x060002FB RID: 763 RVA: 0x000108D6 File Offset: 0x0000EAD6
		public static Int2 Min(Int2 a, Int2 b)
		{
			return new Int2(Math.Min(a.x, b.x), Math.Min(a.y, b.y));
		}

		// Token: 0x060002FC RID: 764 RVA: 0x000108FF File Offset: 0x0000EAFF
		public static Int2 Max(Int2 a, Int2 b)
		{
			return new Int2(Math.Max(a.x, b.x), Math.Max(a.y, b.y));
		}

		// Token: 0x060002FD RID: 765 RVA: 0x00010928 File Offset: 0x0000EB28
		public static Int2 FromInt3XZ(Int3 o)
		{
			return new Int2(o.x, o.z);
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0001093B File Offset: 0x0000EB3B
		public static Int3 ToInt3XZ(Int2 o)
		{
			return new Int3(o.x, 0, o.y);
		}

		// Token: 0x060002FF RID: 767 RVA: 0x00010950 File Offset: 0x0000EB50
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				this.x.ToString(),
				", ",
				this.y.ToString(),
				")"
			});
		}

		// Token: 0x040001E6 RID: 486
		public int x;

		// Token: 0x040001E7 RID: 487
		public int y;
	}
}
