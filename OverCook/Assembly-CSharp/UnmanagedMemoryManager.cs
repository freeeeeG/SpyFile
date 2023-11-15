using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

// Token: 0x02000075 RID: 117
public class UnmanagedMemoryManager
{
	// Token: 0x060002FF RID: 767 RVA: 0x0001E5EC File Offset: 0x0001C9EC
	public static IntPtr Alloc(int size)
	{
		IntPtr intPtr = Marshal.AllocHGlobal(size);
		if (intPtr == IntPtr.Zero)
		{
			PiaPluginUtil.UnityLog("UnmanagedMemoryManager.Alloc : Marshal.AllocHGlobal(" + size + ") failed.");
		}
		try
		{
			UnmanagedMemoryManager.s_NeedFreeList.Add(new UnmanagedMemoryManager.AllocInfo(intPtr, size));
		}
		catch (OutOfMemoryException arg)
		{
			PiaPluginUtil.UnityLog("s_NeedFreeList.Add is OutOfMemoryException.[" + arg + "]");
			return IntPtr.Zero;
		}
		return intPtr;
	}

	// Token: 0x06000300 RID: 768 RVA: 0x0001E674 File Offset: 0x0001CA74
	public static IntPtr Alloc<T>()
	{
		int size = Marshal.SizeOf(typeof(T));
		return UnmanagedMemoryManager.Alloc(size);
	}

	// Token: 0x06000301 RID: 769 RVA: 0x0001E698 File Offset: 0x0001CA98
	public static bool Free(IntPtr p)
	{
		if (p == IntPtr.Zero)
		{
			PiaPluginUtil.UnityLog("UnmanagedMemoryManager.Free : p == IntPtr.Zero");
			return false;
		}
		List<UnmanagedMemoryManager.AllocInfo> list = UnmanagedMemoryManager.s_NeedFreeList.FindAll(delegate(UnmanagedMemoryManager.AllocInfo info)
		{
			if (info == null)
			{
				PiaPluginUtil.UnityLog("List.FindAll is failed.");
				return false;
			}
			return info.ptr == p;
		});
		if (list.Count != 1)
		{
			PiaPluginUtil.UnityLog("UnmanagedMemoryManager.Free : allocInfoList.Count:" + list.Count);
			for (int i = 0; i < list.Count; i++)
			{
				Debug.LogErrorFormat("[{0}] 0x{1:X} size:{2}", new object[]
				{
					i,
					list[i].ptr,
					list[i].size
				});
			}
		}
		else
		{
			UnmanagedMemoryManager.s_NeedFreeList.Remove(list[0]);
		}
		Marshal.FreeHGlobal(p);
		return true;
	}

	// Token: 0x06000302 RID: 770 RVA: 0x0001E78B File Offset: 0x0001CB8B
	public static void DestroyStructure<T>(IntPtr p)
	{
		Marshal.DestroyStructure(p, typeof(T));
	}

	// Token: 0x06000303 RID: 771 RVA: 0x0001E7A0 File Offset: 0x0001CBA0
	public static IntPtr WriteObject<T>(T obj, ref int bufferSize, int allocSize = 0)
	{
		if (obj == null)
		{
			PiaPluginUtil.UnityLog("UnmanagedMemoryManager.WriteObject : obj == null");
			bufferSize = 0;
			return IntPtr.Zero;
		}
		if (allocSize == 0)
		{
			bufferSize = Marshal.SizeOf(typeof(T));
		}
		else
		{
			if (allocSize < Marshal.SizeOf(typeof(T)))
			{
				PiaPluginUtil.UnityLog(string.Format("UnmanagedMemoryManager.WriteObject : allocSize({0}) < {1}", allocSize, Marshal.SizeOf(typeof(T))));
				bufferSize = 0;
				return IntPtr.Zero;
			}
			bufferSize = allocSize;
		}
		IntPtr intPtr = UnmanagedMemoryManager.Alloc(bufferSize);
		if (intPtr == IntPtr.Zero)
		{
			bufferSize = 0;
			return intPtr;
		}
		Marshal.StructureToPtr(obj, intPtr, false);
		return intPtr;
	}

	// Token: 0x06000304 RID: 772 RVA: 0x0001E85C File Offset: 0x0001CC5C
	public static bool ReadObject<T>(IntPtr p, ref T obj)
	{
		try
		{
			if (p == IntPtr.Zero)
			{
				PiaPluginUtil.UnityLog("UnmanagedMemoryManager.ReadObject : p == IntPtr.Zero");
				return false;
			}
			obj = (T)((object)Marshal.PtrToStructure(p, typeof(T)));
		}
		catch (Exception arg)
		{
			PiaPluginUtil.UnityLog("UnmanagedMemoryManager.ReadObject : exception " + arg);
		}
		return true;
	}

	// Token: 0x06000305 RID: 773 RVA: 0x0001E8D4 File Offset: 0x0001CCD4
	public static IntPtr WriteArray<T>(T[] array, ref int bufferSize)
	{
		if (array.Length == 0)
		{
			PiaPluginUtil.UnityLog("UnmanagedMemoryManager.WriteArray : array.Length == 0");
			bufferSize = 0;
			return IntPtr.Zero;
		}
		int num = Marshal.SizeOf(typeof(T));
		bufferSize = num * array.Length;
		IntPtr intPtr = UnmanagedMemoryManager.Alloc(bufferSize);
		if (intPtr == IntPtr.Zero)
		{
			bufferSize = 0;
			return intPtr;
		}
		IntPtr ptr = intPtr;
		for (int i = 0; i < array.Length; i++)
		{
			Marshal.StructureToPtr(array[i], ptr, false);
			ptr = new IntPtr(ptr.ToInt64() + (long)num);
		}
		return intPtr;
	}

	// Token: 0x06000306 RID: 774 RVA: 0x0001E96C File Offset: 0x0001CD6C
	public static bool ReadArray<T>(IntPtr p, int arrayLength, ref T[] array)
	{
		try
		{
			if (p == IntPtr.Zero)
			{
				PiaPluginUtil.UnityLog("UnmanagedMemoryManager.ReadArray : p == IntPtr.Zero");
				return false;
			}
			int num = Marshal.SizeOf(typeof(T));
			IntPtr ptr = p;
			for (int i = 0; i < arrayLength; i++)
			{
				array[i] = (T)((object)Marshal.PtrToStructure(ptr, typeof(T)));
				ptr = new IntPtr(ptr.ToInt64() + (long)num);
			}
		}
		catch (Exception arg)
		{
			PiaPluginUtil.UnityLog("UnmanagedMemoryManager.ReadArray : exception " + arg);
		}
		return true;
	}

	// Token: 0x06000307 RID: 775 RVA: 0x0001EA1C File Offset: 0x0001CE1C
	public static IntPtr WriteList<T>(List<T> list, ref int bufferSize)
	{
		if (list.Count == 0)
		{
			PiaPluginUtil.UnityLog("UnmanagedMemoryManager.WriteList : list.Count == 0");
			bufferSize = 0;
			return IntPtr.Zero;
		}
		int num = Marshal.SizeOf(typeof(T));
		bufferSize = num * list.Count;
		IntPtr intPtr = UnmanagedMemoryManager.Alloc(bufferSize);
		if (intPtr == IntPtr.Zero)
		{
			bufferSize = 0;
			return intPtr;
		}
		IntPtr ptr = intPtr;
		foreach (T t in list)
		{
			Marshal.StructureToPtr(t, ptr, false);
			ptr = new IntPtr(ptr.ToInt64() + (long)num);
		}
		return intPtr;
	}

	// Token: 0x06000308 RID: 776 RVA: 0x0001EAE4 File Offset: 0x0001CEE4
	public static bool ReadList<T>(IntPtr p, int listCount, ref List<T> list)
	{
		try
		{
			if (p == IntPtr.Zero)
			{
				PiaPluginUtil.UnityLog("UnmanagedMemoryManager.ReadList : p == IntPtr.Zero");
				return false;
			}
			int num = Marshal.SizeOf(typeof(T));
			IntPtr ptr = p;
			list.Clear();
			for (int i = 0; i < listCount; i++)
			{
				list.Add((T)((object)Marshal.PtrToStructure(ptr, typeof(T))));
				ptr = new IntPtr(ptr.ToInt64() + (long)num);
			}
		}
		catch (Exception arg)
		{
			PiaPluginUtil.UnityLog("UnmanagedMemoryManager.ReadList : exception " + arg);
		}
		return true;
	}

	// Token: 0x06000309 RID: 777 RVA: 0x0001EB9C File Offset: 0x0001CF9C
	public static IntPtr WriteUtf8(string str, ref int bufferSize)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(str);
		int num = Marshal.SizeOf(typeof(byte));
		bufferSize = num * (bytes.Length + 1);
		IntPtr intPtr = UnmanagedMemoryManager.Alloc(bufferSize);
		IntPtr ptr = intPtr;
		for (int i = 0; i < bytes.Length; i++)
		{
			Marshal.WriteByte(ptr, bytes[i]);
			ptr = new IntPtr(ptr.ToInt64() + (long)num);
		}
		Marshal.WriteByte(ptr, 0);
		return intPtr;
	}

	// Token: 0x0600030A RID: 778 RVA: 0x0001EC14 File Offset: 0x0001D014
	public static string ReadUtf8(IntPtr pStr, int stringSize)
	{
		byte[] array = new byte[stringSize];
		int num = Marshal.SizeOf(typeof(byte));
		IntPtr ptr = pStr;
		for (int i = 0; i < stringSize; i++)
		{
			array[i] = Marshal.ReadByte(ptr);
			ptr = new IntPtr(ptr.ToInt64() + (long)num);
		}
		return Encoding.UTF8.GetString(array);
	}

	// Token: 0x0600030B RID: 779 RVA: 0x0001EC78 File Offset: 0x0001D078
	public static IntPtr WriteUtf16(string str, ref int bufferSize)
	{
		byte[] bytes = Encoding.Unicode.GetBytes(str);
		int num = Marshal.SizeOf(typeof(byte));
		bufferSize = num * (bytes.Length + 2);
		IntPtr intPtr = UnmanagedMemoryManager.Alloc(bufferSize);
		IntPtr ptr = intPtr;
		for (int i = 0; i < bytes.Length; i++)
		{
			Marshal.WriteByte(ptr, bytes[i]);
			ptr = new IntPtr(ptr.ToInt64() + (long)num);
		}
		Marshal.WriteByte(ptr, 0);
		ptr = new IntPtr(ptr.ToInt64() + (long)num);
		Marshal.WriteByte(ptr, 0);
		return intPtr;
	}

	// Token: 0x0600030C RID: 780 RVA: 0x0001ED08 File Offset: 0x0001D108
	public static string ReadUtf16(IntPtr pStr, int stringSize)
	{
		byte[] array = new byte[stringSize];
		int num = Marshal.SizeOf(typeof(byte));
		IntPtr ptr = pStr;
		for (int i = 0; i < stringSize; i++)
		{
			array[i] = Marshal.ReadByte(ptr);
			ptr = new IntPtr(ptr.ToInt64() + (long)num);
		}
		return Encoding.Unicode.GetString(array);
	}

	// Token: 0x0600030D RID: 781 RVA: 0x0001ED6C File Offset: 0x0001D16C
	public static void ValidateAllocInfo()
	{
		PiaPluginUtil.UnityLog("UnmanagedMemoryManager.ValidateAllocInfo s_NeedFreeList count:" + UnmanagedMemoryManager.s_NeedFreeList.Count);
		for (int i = 0; i < UnmanagedMemoryManager.s_NeedFreeList.Count; i++)
		{
			Debug.LogFormat("[{0}] 0x{1:X} size:{2}", new object[]
			{
				i,
				UnmanagedMemoryManager.s_NeedFreeList[i].ptr.ToInt64(),
				UnmanagedMemoryManager.s_NeedFreeList[i].size
			});
		}
	}

	// Token: 0x040001E8 RID: 488
	public static List<UnmanagedMemoryManager.AllocInfo> s_NeedFreeList = new List<UnmanagedMemoryManager.AllocInfo>();

	// Token: 0x02000076 RID: 118
	public class AllocInfo
	{
		// Token: 0x0600030F RID: 783 RVA: 0x0001EE0F File Offset: 0x0001D20F
		public AllocInfo(IntPtr _ptr, int _size)
		{
			this.ptr = _ptr;
			this.size = _size;
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000310 RID: 784 RVA: 0x0001EE25 File Offset: 0x0001D225
		// (set) Token: 0x06000311 RID: 785 RVA: 0x0001EE2D File Offset: 0x0001D22D
		public IntPtr ptr { get; private set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000312 RID: 786 RVA: 0x0001EE36 File Offset: 0x0001D236
		// (set) Token: 0x06000313 RID: 787 RVA: 0x0001EE3E File Offset: 0x0001D23E
		public int size { get; private set; }
	}
}
