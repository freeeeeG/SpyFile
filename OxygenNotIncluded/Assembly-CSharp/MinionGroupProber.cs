using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000864 RID: 2148
[AddComponentMenu("KMonoBehaviour/scripts/MinionGroupProber")]
public class MinionGroupProber : KMonoBehaviour, IGroupProber
{
	// Token: 0x06003ED0 RID: 16080 RVA: 0x0015D598 File Offset: 0x0015B798
	public static void DestroyInstance()
	{
		MinionGroupProber.Instance = null;
	}

	// Token: 0x06003ED1 RID: 16081 RVA: 0x0015D5A0 File Offset: 0x0015B7A0
	public static MinionGroupProber Get()
	{
		return MinionGroupProber.Instance;
	}

	// Token: 0x06003ED2 RID: 16082 RVA: 0x0015D5A7 File Offset: 0x0015B7A7
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MinionGroupProber.Instance = this;
		this.cells = new Dictionary<object, short>[Grid.CellCount];
	}

	// Token: 0x06003ED3 RID: 16083 RVA: 0x0015D5C8 File Offset: 0x0015B7C8
	private bool IsReachable_AssumeLock(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		Dictionary<object, short> dictionary = this.cells[cell];
		if (dictionary == null)
		{
			return false;
		}
		bool result = false;
		foreach (KeyValuePair<object, short> keyValuePair in dictionary)
		{
			object key = keyValuePair.Key;
			short value = keyValuePair.Value;
			KeyValuePair<short, short> keyValuePair2;
			if (this.valid_serial_nos.TryGetValue(key, out keyValuePair2) && (value == keyValuePair2.Key || value == keyValuePair2.Value))
			{
				result = true;
				break;
			}
			this.pending_removals.Add(key);
		}
		foreach (object key2 in this.pending_removals)
		{
			dictionary.Remove(key2);
			if (dictionary.Count == 0)
			{
				this.cells[cell] = null;
			}
		}
		this.pending_removals.Clear();
		return result;
	}

	// Token: 0x06003ED4 RID: 16084 RVA: 0x0015D6D8 File Offset: 0x0015B8D8
	public bool IsReachable(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		bool result = false;
		object obj = this.access;
		lock (obj)
		{
			result = this.IsReachable_AssumeLock(cell);
		}
		return result;
	}

	// Token: 0x06003ED5 RID: 16085 RVA: 0x0015D728 File Offset: 0x0015B928
	public bool IsReachable(int cell, CellOffset[] offsets)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		bool result = false;
		object obj = this.access;
		lock (obj)
		{
			foreach (CellOffset offset in offsets)
			{
				if (this.IsReachable_AssumeLock(Grid.OffsetCell(cell, offset)))
				{
					result = true;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06003ED6 RID: 16086 RVA: 0x0015D7A4 File Offset: 0x0015B9A4
	public bool IsAllReachable(int cell, CellOffset[] offsets)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		bool result = false;
		object obj = this.access;
		lock (obj)
		{
			if (this.IsReachable_AssumeLock(cell))
			{
				result = true;
			}
			else
			{
				foreach (CellOffset offset in offsets)
				{
					if (this.IsReachable_AssumeLock(Grid.OffsetCell(cell, offset)))
					{
						result = true;
						break;
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06003ED7 RID: 16087 RVA: 0x0015D82C File Offset: 0x0015BA2C
	public bool IsReachable(Workable workable)
	{
		return this.IsReachable(Grid.PosToCell(workable), workable.GetOffsets());
	}

	// Token: 0x06003ED8 RID: 16088 RVA: 0x0015D840 File Offset: 0x0015BA40
	public void Occupy(object prober, short serial_no, IEnumerable<int> cells)
	{
		object obj = this.access;
		lock (obj)
		{
			foreach (int num in cells)
			{
				if (this.cells[num] == null)
				{
					this.cells[num] = new Dictionary<object, short>();
				}
				this.cells[num][prober] = serial_no;
			}
		}
	}

	// Token: 0x06003ED9 RID: 16089 RVA: 0x0015D8D0 File Offset: 0x0015BAD0
	public void SetValidSerialNos(object prober, short previous_serial_no, short serial_no)
	{
		object obj = this.access;
		lock (obj)
		{
			this.valid_serial_nos[prober] = new KeyValuePair<short, short>(previous_serial_no, serial_no);
		}
	}

	// Token: 0x06003EDA RID: 16090 RVA: 0x0015D920 File Offset: 0x0015BB20
	public bool ReleaseProber(object prober)
	{
		object obj = this.access;
		bool result;
		lock (obj)
		{
			result = this.valid_serial_nos.Remove(prober);
		}
		return result;
	}

	// Token: 0x040028AD RID: 10413
	private static MinionGroupProber Instance;

	// Token: 0x040028AE RID: 10414
	private Dictionary<object, short>[] cells;

	// Token: 0x040028AF RID: 10415
	private Dictionary<object, KeyValuePair<short, short>> valid_serial_nos = new Dictionary<object, KeyValuePair<short, short>>();

	// Token: 0x040028B0 RID: 10416
	private List<object> pending_removals = new List<object>();

	// Token: 0x040028B1 RID: 10417
	private readonly object access = new object();
}
