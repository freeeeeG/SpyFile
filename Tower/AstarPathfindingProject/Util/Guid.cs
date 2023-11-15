using System;
using System.Text;

namespace Pathfinding.Util
{
	// Token: 0x020000D0 RID: 208
	public struct Guid
	{
		// Token: 0x060008B9 RID: 2233 RVA: 0x0003AE40 File Offset: 0x00039040
		public Guid(byte[] bytes)
		{
			ulong num = (ulong)bytes[0] | (ulong)bytes[1] << 8 | (ulong)bytes[2] << 16 | (ulong)bytes[3] << 24 | (ulong)bytes[4] << 32 | (ulong)bytes[5] << 40 | (ulong)bytes[6] << 48 | (ulong)bytes[7] << 56;
			ulong num2 = (ulong)bytes[8] | (ulong)bytes[9] << 8 | (ulong)bytes[10] << 16 | (ulong)bytes[11] << 24 | (ulong)bytes[12] << 32 | (ulong)bytes[13] << 40 | (ulong)bytes[14] << 48 | (ulong)bytes[15] << 56;
			this._a = (BitConverter.IsLittleEndian ? num : Guid.SwapEndianness(num));
			this._b = (BitConverter.IsLittleEndian ? num2 : Guid.SwapEndianness(num2));
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x0003AEF8 File Offset: 0x000390F8
		public Guid(string str)
		{
			this._a = 0UL;
			this._b = 0UL;
			if (str.Length < 32)
			{
				throw new FormatException("Invalid Guid format");
			}
			int i = 0;
			int num = 0;
			int num2 = 60;
			while (i < 16)
			{
				if (num >= str.Length)
				{
					throw new FormatException("Invalid Guid format. String too short");
				}
				char c = str[num];
				if (c != '-')
				{
					int num3 = "0123456789ABCDEF".IndexOf(char.ToUpperInvariant(c));
					if (num3 == -1)
					{
						throw new FormatException("Invalid Guid format : " + c.ToString() + " is not a hexadecimal character");
					}
					this._a |= (ulong)((ulong)((long)num3) << num2);
					num2 -= 4;
					i++;
				}
				num++;
			}
			num2 = 60;
			while (i < 32)
			{
				if (num >= str.Length)
				{
					throw new FormatException("Invalid Guid format. String too short");
				}
				char c2 = str[num];
				if (c2 != '-')
				{
					int num4 = "0123456789ABCDEF".IndexOf(char.ToUpperInvariant(c2));
					if (num4 == -1)
					{
						throw new FormatException("Invalid Guid format : " + c2.ToString() + " is not a hexadecimal character");
					}
					this._b |= (ulong)((ulong)((long)num4) << num2);
					num2 -= 4;
					i++;
				}
				num++;
			}
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x0003B02F File Offset: 0x0003922F
		public static Guid Parse(string input)
		{
			return new Guid(input);
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x0003B038 File Offset: 0x00039238
		private static ulong SwapEndianness(ulong value)
		{
			ulong num = value & 255UL;
			ulong num2 = value >> 8 & 255UL;
			ulong num3 = value >> 16 & 255UL;
			ulong num4 = value >> 24 & 255UL;
			ulong num5 = value >> 32 & 255UL;
			ulong num6 = value >> 40 & 255UL;
			ulong num7 = value >> 48 & 255UL;
			ulong num8 = value >> 56 & 255UL;
			return num << 56 | num2 << 48 | num3 << 40 | num4 << 32 | num5 << 24 | num6 << 16 | num7 << 8 | num8;
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x0003B0C8 File Offset: 0x000392C8
		public byte[] ToByteArray()
		{
			byte[] array = new byte[16];
			byte[] bytes = BitConverter.GetBytes((!BitConverter.IsLittleEndian) ? Guid.SwapEndianness(this._a) : this._a);
			byte[] bytes2 = BitConverter.GetBytes((!BitConverter.IsLittleEndian) ? Guid.SwapEndianness(this._b) : this._b);
			for (int i = 0; i < 8; i++)
			{
				array[i] = bytes[i];
				array[i + 8] = bytes2[i];
			}
			return array;
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x0003B138 File Offset: 0x00039338
		public static Guid NewGuid()
		{
			byte[] array = new byte[16];
			Guid.random.NextBytes(array);
			return new Guid(array);
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x0003B15E File Offset: 0x0003935E
		public static bool operator ==(Guid lhs, Guid rhs)
		{
			return lhs._a == rhs._a && lhs._b == rhs._b;
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x0003B17E File Offset: 0x0003937E
		public static bool operator !=(Guid lhs, Guid rhs)
		{
			return lhs._a != rhs._a || lhs._b != rhs._b;
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x0003B1A4 File Offset: 0x000393A4
		public override bool Equals(object _rhs)
		{
			if (!(_rhs is Guid))
			{
				return false;
			}
			Guid guid = (Guid)_rhs;
			return this._a == guid._a && this._b == guid._b;
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x0003B1E0 File Offset: 0x000393E0
		public override int GetHashCode()
		{
			ulong num = this._a ^ this._b;
			return (int)(num >> 32) ^ (int)num;
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x0003B204 File Offset: 0x00039404
		public override string ToString()
		{
			if (Guid.text == null)
			{
				Guid.text = new StringBuilder();
			}
			StringBuilder obj = Guid.text;
			string result;
			lock (obj)
			{
				Guid.text.Length = 0;
				Guid.text.Append(this._a.ToString("x16")).Append('-').Append(this._b.ToString("x16"));
				result = Guid.text.ToString();
			}
			return result;
		}

		// Token: 0x0400050B RID: 1291
		private const string hex = "0123456789ABCDEF";

		// Token: 0x0400050C RID: 1292
		public static readonly Guid zero = new Guid(new byte[16]);

		// Token: 0x0400050D RID: 1293
		public static readonly string zeroString = new Guid(new byte[16]).ToString();

		// Token: 0x0400050E RID: 1294
		private readonly ulong _a;

		// Token: 0x0400050F RID: 1295
		private readonly ulong _b;

		// Token: 0x04000510 RID: 1296
		private static Random random = new Random();

		// Token: 0x04000511 RID: 1297
		private static StringBuilder text;
	}
}
