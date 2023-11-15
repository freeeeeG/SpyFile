using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200003E RID: 62
	public struct Int3 : IEquatable<Int3>
	{
		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x00010198 File Offset: 0x0000E398
		public static Int3 zero
		{
			get
			{
				return default(Int3);
			}
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x000101B0 File Offset: 0x0000E3B0
		public Int3(Vector3 position)
		{
			this.x = (int)Math.Round((double)(position.x * 1000f));
			this.y = (int)Math.Round((double)(position.y * 1000f));
			this.z = (int)Math.Round((double)(position.z * 1000f));
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x00010208 File Offset: 0x0000E408
		public Int3(int _x, int _y, int _z)
		{
			this.x = _x;
			this.y = _y;
			this.z = _z;
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0001021F File Offset: 0x0000E41F
		public static bool operator ==(Int3 lhs, Int3 rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0001024D File Offset: 0x0000E44D
		public static bool operator !=(Int3 lhs, Int3 rhs)
		{
			return lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z;
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0001027E File Offset: 0x0000E47E
		public static explicit operator Int3(Vector3 ob)
		{
			return new Int3((int)Math.Round((double)(ob.x * 1000f)), (int)Math.Round((double)(ob.y * 1000f)), (int)Math.Round((double)(ob.z * 1000f)));
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x000102BE File Offset: 0x0000E4BE
		public static explicit operator Vector3(Int3 ob)
		{
			return new Vector3((float)ob.x * 0.001f, (float)ob.y * 0.001f, (float)ob.z * 0.001f);
		}

		// Token: 0x060002DA RID: 730 RVA: 0x000102EC File Offset: 0x0000E4EC
		public static Int3 operator -(Int3 lhs, Int3 rhs)
		{
			lhs.x -= rhs.x;
			lhs.y -= rhs.y;
			lhs.z -= rhs.z;
			return lhs;
		}

		// Token: 0x060002DB RID: 731 RVA: 0x00010322 File Offset: 0x0000E522
		public static Int3 operator -(Int3 lhs)
		{
			lhs.x = -lhs.x;
			lhs.y = -lhs.y;
			lhs.z = -lhs.z;
			return lhs;
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0001034F File Offset: 0x0000E54F
		public static Int3 operator +(Int3 lhs, Int3 rhs)
		{
			lhs.x += rhs.x;
			lhs.y += rhs.y;
			lhs.z += rhs.z;
			return lhs;
		}

		// Token: 0x060002DD RID: 733 RVA: 0x00010385 File Offset: 0x0000E585
		public static Int3 operator *(Int3 lhs, int rhs)
		{
			lhs.x *= rhs;
			lhs.y *= rhs;
			lhs.z *= rhs;
			return lhs;
		}

		// Token: 0x060002DE RID: 734 RVA: 0x000103AC File Offset: 0x0000E5AC
		public static Int3 operator *(Int3 lhs, float rhs)
		{
			lhs.x = (int)Math.Round((double)((float)lhs.x * rhs));
			lhs.y = (int)Math.Round((double)((float)lhs.y * rhs));
			lhs.z = (int)Math.Round((double)((float)lhs.z * rhs));
			return lhs;
		}

		// Token: 0x060002DF RID: 735 RVA: 0x00010400 File Offset: 0x0000E600
		public static Int3 operator *(Int3 lhs, double rhs)
		{
			lhs.x = (int)Math.Round((double)lhs.x * rhs);
			lhs.y = (int)Math.Round((double)lhs.y * rhs);
			lhs.z = (int)Math.Round((double)lhs.z * rhs);
			return lhs;
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00010450 File Offset: 0x0000E650
		public static Int3 operator /(Int3 lhs, float rhs)
		{
			lhs.x = (int)Math.Round((double)((float)lhs.x / rhs));
			lhs.y = (int)Math.Round((double)((float)lhs.y / rhs));
			lhs.z = (int)Math.Round((double)((float)lhs.z / rhs));
			return lhs;
		}

		// Token: 0x17000093 RID: 147
		public int this[int i]
		{
			get
			{
				if (i == 0)
				{
					return this.x;
				}
				if (i != 1)
				{
					return this.z;
				}
				return this.y;
			}
			set
			{
				if (i == 0)
				{
					this.x = value;
					return;
				}
				if (i == 1)
				{
					this.y = value;
					return;
				}
				this.z = value;
			}
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x000104E0 File Offset: 0x0000E6E0
		public static float Angle(Int3 lhs, Int3 rhs)
		{
			double num = (double)Int3.Dot(lhs, rhs) / ((double)lhs.magnitude * (double)rhs.magnitude);
			num = ((num < -1.0) ? -1.0 : ((num > 1.0) ? 1.0 : num));
			return (float)Math.Acos(num);
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0001053F File Offset: 0x0000E73F
		public static int Dot(Int3 lhs, Int3 rhs)
		{
			return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0001056A File Offset: 0x0000E76A
		public static long DotLong(Int3 lhs, Int3 rhs)
		{
			return (long)lhs.x * (long)rhs.x + (long)lhs.y * (long)rhs.y + (long)lhs.z * (long)rhs.z;
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0001059B File Offset: 0x0000E79B
		public Int3 Normal2D()
		{
			return new Int3(this.z, this.y, -this.x);
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060002E7 RID: 743 RVA: 0x000105B8 File Offset: 0x0000E7B8
		public float magnitude
		{
			get
			{
				double num = (double)this.x;
				double num2 = (double)this.y;
				double num3 = (double)this.z;
				return (float)Math.Sqrt(num * num + num2 * num2 + num3 * num3);
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x000105EC File Offset: 0x0000E7EC
		public int costMagnitude
		{
			get
			{
				return (int)Math.Round((double)this.magnitude);
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x000105FC File Offset: 0x0000E7FC
		public float sqrMagnitude
		{
			get
			{
				double num = (double)this.x;
				double num2 = (double)this.y;
				double num3 = (double)this.z;
				return (float)(num * num + num2 * num2 + num3 * num3);
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060002EA RID: 746 RVA: 0x0001062C File Offset: 0x0000E82C
		public long sqrMagnitudeLong
		{
			get
			{
				long num = (long)this.x;
				long num2 = (long)this.y;
				long num3 = (long)this.z;
				return num * num + num2 * num2 + num3 * num3;
			}
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0001065A File Offset: 0x0000E85A
		public static implicit operator string(Int3 obj)
		{
			return obj.ToString();
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0001066C File Offset: 0x0000E86C
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"( ",
				this.x.ToString(),
				", ",
				this.y.ToString(),
				", ",
				this.z.ToString(),
				")"
			});
		}

		// Token: 0x060002ED RID: 749 RVA: 0x000106D0 File Offset: 0x0000E8D0
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			Int3 @int = (Int3)obj;
			return this.x == @int.x && this.y == @int.y && this.z == @int.z;
		}

		// Token: 0x060002EE RID: 750 RVA: 0x00010715 File Offset: 0x0000E915
		public bool Equals(Int3 other)
		{
			return this.x == other.x && this.y == other.y && this.z == other.z;
		}

		// Token: 0x060002EF RID: 751 RVA: 0x00010743 File Offset: 0x0000E943
		public override int GetHashCode()
		{
			return this.x * 73856093 ^ this.y * 19349669 ^ this.z * 83492791;
		}

		// Token: 0x040001E0 RID: 480
		public int x;

		// Token: 0x040001E1 RID: 481
		public int y;

		// Token: 0x040001E2 RID: 482
		public int z;

		// Token: 0x040001E3 RID: 483
		public const int Precision = 1000;

		// Token: 0x040001E4 RID: 484
		public const float FloatPrecision = 1000f;

		// Token: 0x040001E5 RID: 485
		public const float PrecisionFactor = 0.001f;
	}
}
