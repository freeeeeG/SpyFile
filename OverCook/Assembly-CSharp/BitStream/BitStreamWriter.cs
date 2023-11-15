using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BitStream
{
	// Token: 0x02000838 RID: 2104
	public class BitStreamWriter
	{
		// Token: 0x0600288A RID: 10378 RVA: 0x000BE68B File Offset: 0x000BCA8B
		public BitStreamWriter(FastList<byte> bufferToWriteTo)
		{
			if (bufferToWriteTo == null)
			{
				throw new ArgumentNullException("bufferToWriteTo");
			}
			this._targetBuffer = bufferToWriteTo;
		}

		// Token: 0x0600288B RID: 10379 RVA: 0x000BE6AB File Offset: 0x000BCAAB
		public void Reset(FastList<byte> bufferToWriteTo)
		{
			this._targetBuffer = bufferToWriteTo;
			this._remaining = 0;
		}

		// Token: 0x0600288C RID: 10380 RVA: 0x000BE6BC File Offset: 0x000BCABC
		public void Write(string _text, Encoding _encoding)
		{
			byte[] bytes = _encoding.GetBytes(_text);
			if (bytes.Length * 8 + 10 > this.GetRemainingBitCount())
			{
				this.Write(0U, 10);
				return;
			}
			this.Write((uint)bytes.Length, 10);
			this.Write(bytes, 8 * bytes.Length);
		}

		// Token: 0x0600288D RID: 10381 RVA: 0x000BE708 File Offset: 0x000BCB08
		public void Write(ref Quaternion value)
		{
			float num = value.x;
			uint bits = (uint)num;
			this.Write(bits, 32);
			num = value.y;
			bits = (uint)num;
			this.Write(bits, 32);
			num = value.z;
			bits = (uint)num;
			this.Write(bits, 32);
			num = value.w;
			bits = (uint)num;
			this.Write(bits, 32);
		}

		// Token: 0x0600288E RID: 10382 RVA: 0x000BE770 File Offset: 0x000BCB70
		public void Write(ref Vector3 value)
		{
			float num = value.x;
			uint bits = (uint)num;
			this.Write(bits, 32);
			num = value.y;
			bits = (uint)num;
			this.Write(bits, 32);
			num = value.z;
			bits = (uint)num;
			this.Write(bits, 32);
		}

		// Token: 0x0600288F RID: 10383 RVA: 0x000BE7C4 File Offset: 0x000BCBC4
		public void Write(ref Vector2 value)
		{
			float num = value.x;
			uint bits = (uint)num;
			this.Write(bits, 32);
			num = value.y;
			bits = (uint)num;
			this.Write(bits, 32);
		}

		// Token: 0x06002890 RID: 10384 RVA: 0x000BE800 File Offset: 0x000BCC00
		public void Write(double value)
		{
			ulong bits = (ulong)value;
			this.Write(bits, 64);
		}

		// Token: 0x06002891 RID: 10385 RVA: 0x000BE820 File Offset: 0x000BCC20
		public void Write(float value)
		{
			uint bits = (uint)value;
			this.Write(bits, 32);
		}

		// Token: 0x06002892 RID: 10386 RVA: 0x000BE83E File Offset: 0x000BCC3E
		public void Write(bool value)
		{
			this.Write((!value) ? 0 : byte.MaxValue, 1);
		}

		// Token: 0x06002893 RID: 10387 RVA: 0x000BE858 File Offset: 0x000BCC58
		public void Write(byte[] bytes, int countOfBits)
		{
			if (bytes == null || countOfBits <= 0 || countOfBits > bytes.Length << 3)
			{
				return;
			}
			int num = countOfBits / 8;
			int num2 = countOfBits % 8;
			int i;
			for (i = 0; i < num; i++)
			{
				this.Write(bytes[i], 8);
			}
			if (num2 > 0)
			{
				this.Write(bytes[i], num2);
			}
		}

		// Token: 0x06002894 RID: 10388 RVA: 0x000BE8B4 File Offset: 0x000BCCB4
		public void Write(ulong bits, int countOfBits)
		{
			if (countOfBits <= 0 || countOfBits > 64)
			{
				return;
			}
			int i = countOfBits / 8;
			int num = countOfBits % 8;
			while (i >= 0)
			{
				byte bits2 = (byte)(bits >> i * 8);
				if (num > 0)
				{
					this.Write(bits2, num);
				}
				if (i > 0)
				{
					num = 8;
				}
				i--;
			}
		}

		// Token: 0x06002895 RID: 10389 RVA: 0x000BE90C File Offset: 0x000BCD0C
		public void Write(uint bits, int countOfBits)
		{
			if (countOfBits <= 0 || countOfBits > 32)
			{
				return;
			}
			int i = countOfBits / 8;
			int num = countOfBits % 8;
			while (i >= 0)
			{
				byte bits2 = (byte)(bits >> i * 8);
				if (num > 0)
				{
					this.Write(bits2, num);
				}
				if (i > 0)
				{
					num = 8;
				}
				i--;
			}
		}

		// Token: 0x06002896 RID: 10390 RVA: 0x000BE964 File Offset: 0x000BCD64
		public void WriteReverse(uint bits, int countOfBits)
		{
			if (countOfBits <= 0 || countOfBits > 32)
			{
				return;
			}
			int num = countOfBits / 8;
			int num2 = countOfBits % 8;
			if (num2 > 0)
			{
				num++;
			}
			for (int i = 0; i < num; i++)
			{
				byte bits2 = (byte)(bits >> i * 8);
				this.Write(bits2, 8);
			}
		}

		// Token: 0x06002897 RID: 10391 RVA: 0x000BE9B8 File Offset: 0x000BCDB8
		public void Write(byte bits, int countOfBits)
		{
			if (countOfBits <= 0 || countOfBits > 8)
			{
				return;
			}
			if (this._remaining > 0)
			{
				byte b = this._targetBuffer._items[this._targetBuffer.Count - 1];
				if (countOfBits > this._remaining)
				{
					b |= (byte)(((int)bits & 255 >> 8 - countOfBits) >> countOfBits - this._remaining);
				}
				else
				{
					b |= (byte)(((int)bits & 255 >> 8 - countOfBits) << this._remaining - countOfBits);
				}
				this._targetBuffer._items[this._targetBuffer.Count - 1] = b;
			}
			if (countOfBits > this._remaining)
			{
				this._remaining = 8 - (countOfBits - this._remaining);
				byte b = (byte)(bits << this._remaining);
				this._targetBuffer.Add(b);
			}
			else
			{
				this._remaining -= countOfBits;
			}
		}

		// Token: 0x06002898 RID: 10392 RVA: 0x000BEAAB File Offset: 0x000BCEAB
		public int GetUsedBitCount()
		{
			return this._targetBuffer.Count * 8 - this._remaining;
		}

		// Token: 0x06002899 RID: 10393 RVA: 0x000BEAC1 File Offset: 0x000BCEC1
		public int GetRemainingBitCount()
		{
			return (this._targetBuffer.Capacity - this._targetBuffer.Count) * 8 + this._remaining;
		}

		// Token: 0x04002001 RID: 8193
		private FastList<byte> _targetBuffer;

		// Token: 0x04002002 RID: 8194
		private int _remaining;
	}
}
