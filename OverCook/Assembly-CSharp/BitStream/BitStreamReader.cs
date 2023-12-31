﻿using System;
using System.Text;
using UnityEngine;

namespace BitStream
{
	// Token: 0x02000837 RID: 2103
	public class BitStreamReader
	{
		// Token: 0x06002874 RID: 10356 RVA: 0x000BE140 File Offset: 0x000BC540
		public BitStreamReader(byte[] buffer)
		{
			this._byteArray = buffer;
			this._bufferLengthInBits = (uint)(buffer.Length * 8);
		}

		// Token: 0x06002875 RID: 10357 RVA: 0x000BE15A File Offset: 0x000BC55A
		public BitStreamReader(byte[] buffer, int startIndex)
		{
			if (startIndex < 0 || startIndex >= buffer.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			this._byteArray = buffer;
			this._byteArrayIndex = startIndex;
			this._bufferLengthInBits = (uint)((buffer.Length - startIndex) * 8);
		}

		// Token: 0x06002876 RID: 10358 RVA: 0x000BE198 File Offset: 0x000BC598
		public BitStreamReader(byte[] buffer, uint bufferLengthInBits) : this(buffer)
		{
			if ((ulong)bufferLengthInBits > (ulong)((long)(buffer.Length * 8)))
			{
				return;
			}
			this._bufferLengthInBits = bufferLengthInBits;
		}

		// Token: 0x06002877 RID: 10359 RVA: 0x000BE1B6 File Offset: 0x000BC5B6
		public void Reset(byte[] buffer)
		{
			this._byteArray = buffer;
			this._bufferLengthInBits = (uint)(buffer.Length * 8);
			this._byteArrayIndex = 0;
			this._partialByte = 0;
			this._cbitsInPartialByte = 0;
		}

		// Token: 0x06002878 RID: 10360 RVA: 0x000BE1DF File Offset: 0x000BC5DF
		public void Reset(byte[] buffer, int usedLength)
		{
			this.Reset(buffer);
			this._bufferLengthInBits = (uint)(usedLength * 8);
		}

		// Token: 0x06002879 RID: 10361 RVA: 0x000BE1F1 File Offset: 0x000BC5F1
		public void AdvanceToNextByteBoundary()
		{
			if (this._cbitsInPartialByte > 0)
			{
				this._cbitsInPartialByte = 0;
				this._partialByte = 0;
			}
		}

		// Token: 0x0600287A RID: 10362 RVA: 0x000BE20D File Offset: 0x000BC60D
		public void SkipToByteIndex(int index)
		{
			this._byteArrayIndex = index;
			this.AdvanceToNextByteBoundary();
		}

		// Token: 0x0600287B RID: 10363 RVA: 0x000BE21C File Offset: 0x000BC61C
		public long ReadUInt64(int countOfBits)
		{
			if (countOfBits > 64 || countOfBits <= 0)
			{
				return 0L;
			}
			long num = 0L;
			while (countOfBits > 0)
			{
				int num2 = 8;
				if (countOfBits < 8)
				{
					num2 = countOfBits;
				}
				num <<= num2;
				byte b = this.ReadByte(num2);
				num |= (long)b;
				countOfBits -= num2;
			}
			return num;
		}

		// Token: 0x0600287C RID: 10364 RVA: 0x000BE270 File Offset: 0x000BC670
		public ushort ReadUInt16(int countOfBits)
		{
			if (countOfBits > 16 || countOfBits <= 0)
			{
				return 0;
			}
			ushort num = 0;
			while (countOfBits > 0)
			{
				int num2 = 8;
				if (countOfBits < 8)
				{
					num2 = countOfBits;
				}
				num = (ushort)(num << num2);
				byte b = this.ReadByte(num2);
				num |= (ushort)b;
				countOfBits -= num2;
			}
			return num;
		}

		// Token: 0x0600287D RID: 10365 RVA: 0x000BE2C4 File Offset: 0x000BC6C4
		public uint ReadUInt16Reverse(int countOfBits)
		{
			if (countOfBits > 16 || countOfBits <= 0)
			{
				return 0U;
			}
			ushort num = 0;
			int num2 = 0;
			while (countOfBits > 0)
			{
				int num3 = 8;
				if (countOfBits < 8)
				{
					num3 = countOfBits;
				}
				ushort num4 = (ushort)this.ReadByte(num3);
				num4 = (ushort)(num4 << num2 * 8);
				num |= num4;
				num2++;
				countOfBits -= num3;
			}
			return (uint)num;
		}

		// Token: 0x0600287E RID: 10366 RVA: 0x000BE320 File Offset: 0x000BC720
		public uint ReadUInt32(int countOfBits)
		{
			if (countOfBits > 32 || countOfBits <= 0)
			{
				return 0U;
			}
			uint num = 0U;
			while (countOfBits > 0)
			{
				int num2 = 8;
				if (countOfBits < 8)
				{
					num2 = countOfBits;
				}
				num <<= num2;
				byte b = this.ReadByte(num2);
				num |= (uint)b;
				countOfBits -= num2;
			}
			return num;
		}

		// Token: 0x0600287F RID: 10367 RVA: 0x000BE370 File Offset: 0x000BC770
		public uint ReadUInt32Reverse(int countOfBits)
		{
			if (countOfBits > 32 || countOfBits <= 0)
			{
				return 0U;
			}
			uint num = 0U;
			int num2 = 0;
			while (countOfBits > 0)
			{
				int num3 = 8;
				if (countOfBits < 8)
				{
					num3 = countOfBits;
				}
				uint num4 = (uint)this.ReadByte(num3);
				num4 <<= num2 * 8;
				num |= num4;
				num2++;
				countOfBits -= num3;
			}
			return num;
		}

		// Token: 0x06002880 RID: 10368 RVA: 0x000BE3C8 File Offset: 0x000BC7C8
		public bool ReadBit()
		{
			byte b = this.ReadByte(1);
			return (b & 1) == 1;
		}

		// Token: 0x06002881 RID: 10369 RVA: 0x000BE3E4 File Offset: 0x000BC7E4
		public byte ReadByte(int countOfBits)
		{
			if (this.EndOfStream)
			{
				return 0;
			}
			if (countOfBits > 8 || countOfBits <= 0)
			{
				return 0;
			}
			if ((long)countOfBits > (long)((ulong)this._bufferLengthInBits))
			{
				return 0;
			}
			this._bufferLengthInBits -= (uint)countOfBits;
			byte b;
			if (this._cbitsInPartialByte >= countOfBits)
			{
				int num = 8 - countOfBits;
				b = (byte)(this._partialByte >> num);
				this._partialByte = (byte)(this._partialByte << countOfBits);
				this._cbitsInPartialByte -= countOfBits;
			}
			else
			{
				byte b2 = this._byteArray[this._byteArrayIndex];
				this._byteArrayIndex++;
				int num2 = 8 - countOfBits;
				b = (byte)(this._partialByte >> num2);
				int num3 = Math.Abs(countOfBits - this._cbitsInPartialByte - 8);
				b |= (byte)(b2 >> num3);
				this._partialByte = (byte)(b2 << countOfBits - this._cbitsInPartialByte);
				this._cbitsInPartialByte = 8 - (countOfBits - this._cbitsInPartialByte);
			}
			return b;
		}

		// Token: 0x06002882 RID: 10370 RVA: 0x000BE4E0 File Offset: 0x000BC8E0
		public float ReadFloat32()
		{
			return (float)this.ReadUInt32(32);
		}

		// Token: 0x06002883 RID: 10371 RVA: 0x000BE504 File Offset: 0x000BC904
		public double ReadFloat64()
		{
			return (double)this.ReadUInt64(64);
		}

		// Token: 0x06002884 RID: 10372 RVA: 0x000BE52C File Offset: 0x000BC92C
		public void ReadVector3(ref Vector3 value)
		{
			uint num = this.ReadUInt32(32);
			value.x = (float)num;
			num = this.ReadUInt32(32);
			value.y = (float)num;
			num = this.ReadUInt32(32);
			value.z = (float)num;
		}

		// Token: 0x06002885 RID: 10373 RVA: 0x000BE578 File Offset: 0x000BC978
		public void ReadVector2(ref Vector2 value)
		{
			uint num = this.ReadUInt32(32);
			value.x = (float)num;
			num = this.ReadUInt32(32);
			value.y = (float)num;
		}

		// Token: 0x06002886 RID: 10374 RVA: 0x000BE5B0 File Offset: 0x000BC9B0
		public void ReadQuaternion(ref Quaternion value)
		{
			uint num = this.ReadUInt32(32);
			value.x = (float)num;
			num = this.ReadUInt32(32);
			value.y = (float)num;
			num = this.ReadUInt32(32);
			value.z = (float)num;
			num = this.ReadUInt32(32);
			value.w = (float)num;
		}

		// Token: 0x06002887 RID: 10375 RVA: 0x000BE610 File Offset: 0x000BCA10
		public string ReadString(Encoding _encoding)
		{
			if (this.EndOfStream)
			{
				return string.Empty;
			}
			uint num = this.ReadUInt32(10);
			if (num == 0U)
			{
				return string.Empty;
			}
			byte[] array = new byte[num];
			int num2 = 0;
			while ((long)num2 < (long)((ulong)num))
			{
				byte b = this.ReadByte(8);
				array[num2] = b;
				num2++;
			}
			return _encoding.GetString(array);
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06002888 RID: 10376 RVA: 0x000BE676 File Offset: 0x000BCA76
		public bool EndOfStream
		{
			get
			{
				return 0U == this._bufferLengthInBits;
			}
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06002889 RID: 10377 RVA: 0x000BE681 File Offset: 0x000BCA81
		public int CurrentIndex
		{
			get
			{
				return this._byteArrayIndex - 1;
			}
		}

		// Token: 0x04001FFC RID: 8188
		private byte[] _byteArray;

		// Token: 0x04001FFD RID: 8189
		private uint _bufferLengthInBits;

		// Token: 0x04001FFE RID: 8190
		private int _byteArrayIndex;

		// Token: 0x04001FFF RID: 8191
		private byte _partialByte;

		// Token: 0x04002000 RID: 8192
		private int _cbitsInPartialByte;
	}
}
