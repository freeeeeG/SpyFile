using System;
using UnityEngine;

// Token: 0x02000B82 RID: 2946
public class CRC32
{
	// Token: 0x06003C26 RID: 15398 RVA: 0x0011FD9B File Offset: 0x0011E19B
	private CRC32()
	{
		if (CRC32.s_table == null)
		{
			this.MakeTable();
		}
	}

	// Token: 0x06003C27 RID: 15399 RVA: 0x0011FDB4 File Offset: 0x0011E1B4
	protected void MakeTable()
	{
		CRC32.s_table = new uint[256];
		for (uint num = 0U; num < 256U; num += 1U)
		{
			uint num2 = num;
			for (uint num3 = 0U; num3 < 8U; num3 += 1U)
			{
				num2 = (((num2 & 1U) != 1U) ? (num2 >> 1) : (num2 ^ 1491524015U));
			}
			CRC32.s_table[(int)((UIntPtr)num)] = num2;
		}
	}

	// Token: 0x06003C28 RID: 15400 RVA: 0x0011FE1D File Offset: 0x0011E21D
	public static uint Calculate(byte[] _data)
	{
		return new CRC32().CalculateHash(_data);
	}

	// Token: 0x06003C29 RID: 15401 RVA: 0x0011FE2A File Offset: 0x0011E22A
	public uint CalculateHash(byte[] _data)
	{
		return this.CalculateHash(_data, 0U, (uint)_data.Length);
	}

	// Token: 0x06003C2A RID: 15402 RVA: 0x0011FE37 File Offset: 0x0011E237
	public static uint Calculate(byte[] _data, uint _size)
	{
		return new CRC32().CalculateHash(_data, _size);
	}

	// Token: 0x06003C2B RID: 15403 RVA: 0x0011FE45 File Offset: 0x0011E245
	public uint CalculateHash(byte[] _data, uint _size)
	{
		return this.CalculateHash(_data, 0U, _size);
	}

	// Token: 0x06003C2C RID: 15404 RVA: 0x0011FE50 File Offset: 0x0011E250
	public static uint Calculate(byte[] _data, uint _start, uint _size)
	{
		return new CRC32().CalculateHash(_data, _start, _size);
	}

	// Token: 0x06003C2D RID: 15405 RVA: 0x0011FE60 File Offset: 0x0011E260
	public uint CalculateHash(byte[] _data, uint _start, uint _size)
	{
		uint num = 3605721660U;
		for (uint num2 = _start; num2 < _start + _size; num2 += 1U)
		{
			num = (num >> 8 ^ CRC32.s_table[(int)((UIntPtr)((uint)_data[(int)((UIntPtr)num2)] ^ (num & 255U)))]);
		}
		return num;
	}

	// Token: 0x06003C2E RID: 15406 RVA: 0x0011FEA0 File Offset: 0x0011E2A0
	public static void Append(ref byte[] _buffer)
	{
		new CRC32().AppendHash(ref _buffer);
	}

	// Token: 0x06003C2F RID: 15407 RVA: 0x0011FEAD File Offset: 0x0011E2AD
	public void AppendHash(ref byte[] _buffer)
	{
		this.AppendHash(ref _buffer, 0U, (uint)_buffer.Length);
	}

	// Token: 0x06003C30 RID: 15408 RVA: 0x0011FEBB File Offset: 0x0011E2BB
	public static void Append(ref byte[] _buffer, uint _start, uint _size)
	{
		new CRC32().AppendHash(ref _buffer, _start, _size);
	}

	// Token: 0x06003C31 RID: 15409 RVA: 0x0011FECA File Offset: 0x0011E2CA
	public void AppendHash(ref byte[] _buffer, uint _start, uint _size)
	{
		this.AppendHash(ref _buffer, 0U, _size, this.CalculateHash(_buffer, _start, _size));
	}

	// Token: 0x06003C32 RID: 15410 RVA: 0x0011FEDF File Offset: 0x0011E2DF
	public static void Append(ref byte[] _buffer, uint _start, uint _size, uint _hash)
	{
		new CRC32().AppendHash(ref _buffer, _start, _size, _hash);
	}

	// Token: 0x06003C33 RID: 15411 RVA: 0x0011FEF0 File Offset: 0x0011E2F0
	public void AppendHash(ref byte[] _buffer, uint _start, uint _size, uint _hash)
	{
		byte[] bytes = BitConverter.GetBytes(_hash);
		int num = _buffer.Length - (int)(_start + _size + 4U);
		bool flag = num < 0;
		if (flag)
		{
			num = Mathf.Abs(num);
			if ((long)num < 4L)
			{
				_buffer = _buffer.AllRemoved_Generic((int idx, byte val) => (long)idx > (long)((ulong)(_start + _size)));
			}
			_buffer = _buffer.Union(bytes);
		}
		else
		{
			int size = (int)_size;
			for (int i = 0; i < bytes.Length; i++)
			{
				_buffer[i + size] = bytes[i];
			}
		}
	}

	// Token: 0x06003C34 RID: 15412 RVA: 0x0011FF99 File Offset: 0x0011E399
	public static bool Validate(byte[] _buffer, uint _size)
	{
		return new CRC32().HasValidHash(_buffer, _size);
	}

	// Token: 0x06003C35 RID: 15413 RVA: 0x0011FFA7 File Offset: 0x0011E3A7
	public bool HasValidHash(byte[] _buffer, uint _size)
	{
		return this.HasValidHash(_buffer, 0U, _size);
	}

	// Token: 0x06003C36 RID: 15414 RVA: 0x0011FFB2 File Offset: 0x0011E3B2
	public static bool Validate(byte[] _buffer, uint _start, uint _size)
	{
		return new CRC32().HasValidHash(_buffer, _start, _size);
	}

	// Token: 0x06003C37 RID: 15415 RVA: 0x0011FFC1 File Offset: 0x0011E3C1
	public bool HasValidHash(byte[] _buffer, uint _start, uint _size)
	{
		return this.HasValidHash(_buffer, _start, _size, _start + _size);
	}

	// Token: 0x06003C38 RID: 15416 RVA: 0x0011FFCF File Offset: 0x0011E3CF
	public static bool Validate(byte[] _buffer, uint _start, uint _size, uint _hashStar)
	{
		return new CRC32().HasValidHash(_buffer, _start, _size, _hashStar);
	}

	// Token: 0x06003C39 RID: 15417 RVA: 0x0011FFE0 File Offset: 0x0011E3E0
	public bool HasValidHash(byte[] _buffer, uint _start, uint _size, uint _hashStart)
	{
		if ((ulong)(_hashStart + 4U) > (ulong)((long)_buffer.Length))
		{
			return false;
		}
		uint num = this.CalculateHash(_buffer, _start, _size);
		uint num2 = BitConverter.ToUInt32(_buffer, (int)_hashStart);
		return num == num2;
	}

	// Token: 0x040030DB RID: 12507
	public const uint c_HashSize = 4U;

	// Token: 0x040030DC RID: 12508
	private const uint poly = 1491524015U;

	// Token: 0x040030DD RID: 12509
	private const uint seed = 3605721660U;

	// Token: 0x040030DE RID: 12510
	private static uint[] s_table;
}
