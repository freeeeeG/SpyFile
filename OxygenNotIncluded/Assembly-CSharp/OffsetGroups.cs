using System;
using System.Collections.Generic;

// Token: 0x020008C2 RID: 2242
public static class OffsetGroups
{
	// Token: 0x06004100 RID: 16640 RVA: 0x0016B000 File Offset: 0x00169200
	public static CellOffset[] InitGrid(int x0, int x1, int y0, int y1)
	{
		List<CellOffset> list = new List<CellOffset>();
		for (int i = y0; i <= y1; i++)
		{
			for (int j = x0; j <= x1; j++)
			{
				list.Add(new CellOffset(j, i));
			}
		}
		CellOffset[] array = list.ToArray();
		Array.Sort<CellOffset>(array, 0, array.Length, new OffsetGroups.CellOffsetComparer());
		return array;
	}

	// Token: 0x06004101 RID: 16641 RVA: 0x0016B050 File Offset: 0x00169250
	public static CellOffset[][] BuildReachabilityTable(CellOffset[] area_offsets, CellOffset[][] table, CellOffset[] filter)
	{
		Dictionary<CellOffset[][], Dictionary<CellOffset[], CellOffset[][]>> dictionary = null;
		Dictionary<CellOffset[], CellOffset[][]> dictionary2 = null;
		CellOffset[][] array = null;
		if (OffsetGroups.reachabilityTableCache.TryGetValue(area_offsets, out dictionary) && dictionary.TryGetValue(table, out dictionary2) && dictionary2.TryGetValue((filter == null) ? OffsetGroups.nullFilter : filter, out array))
		{
			return array;
		}
		HashSet<CellOffset> hashSet = new HashSet<CellOffset>();
		foreach (CellOffset a in area_offsets)
		{
			foreach (CellOffset[] array2 in table)
			{
				if (filter == null || Array.IndexOf<CellOffset>(filter, array2[0]) == -1)
				{
					CellOffset item = a + array2[0];
					hashSet.Add(item);
				}
			}
		}
		List<CellOffset[]> list = new List<CellOffset[]>();
		foreach (CellOffset cellOffset in hashSet)
		{
			CellOffset b = area_offsets[0];
			foreach (CellOffset cellOffset2 in area_offsets)
			{
				if ((cellOffset - b).GetOffsetDistance() > (cellOffset - cellOffset2).GetOffsetDistance())
				{
					b = cellOffset2;
				}
			}
			foreach (CellOffset[] array3 in table)
			{
				if ((filter == null || Array.IndexOf<CellOffset>(filter, array3[0]) == -1) && array3[0] + b == cellOffset)
				{
					CellOffset[] array4 = new CellOffset[array3.Length];
					for (int k = 0; k < array3.Length; k++)
					{
						array4[k] = array3[k] + b;
					}
					list.Add(array4);
				}
			}
		}
		array = list.ToArray();
		Array.Sort<CellOffset[]>(array, (CellOffset[] x, CellOffset[] y) => x[0].GetOffsetDistance().CompareTo(y[0].GetOffsetDistance()));
		if (dictionary == null)
		{
			dictionary = new Dictionary<CellOffset[][], Dictionary<CellOffset[], CellOffset[][]>>();
			OffsetGroups.reachabilityTableCache.Add(area_offsets, dictionary);
		}
		if (dictionary2 == null)
		{
			dictionary2 = new Dictionary<CellOffset[], CellOffset[][]>();
			dictionary.Add(table, dictionary2);
		}
		dictionary2.Add((filter == null) ? OffsetGroups.nullFilter : filter, array);
		return array;
	}

	// Token: 0x04002A4B RID: 10827
	public static CellOffset[] Use = new CellOffset[1];

	// Token: 0x04002A4C RID: 10828
	public static CellOffset[] Chat = new CellOffset[]
	{
		new CellOffset(1, 0),
		new CellOffset(-1, 0),
		new CellOffset(1, 1),
		new CellOffset(1, -1),
		new CellOffset(-1, 1),
		new CellOffset(-1, -1)
	};

	// Token: 0x04002A4D RID: 10829
	public static CellOffset[] LeftOnly = new CellOffset[]
	{
		new CellOffset(-1, 0)
	};

	// Token: 0x04002A4E RID: 10830
	public static CellOffset[] RightOnly = new CellOffset[]
	{
		new CellOffset(1, 0)
	};

	// Token: 0x04002A4F RID: 10831
	public static CellOffset[] LeftOrRight = new CellOffset[]
	{
		new CellOffset(-1, 0),
		new CellOffset(1, 0)
	};

	// Token: 0x04002A50 RID: 10832
	public static CellOffset[] Standard = OffsetGroups.InitGrid(-2, 2, -3, 3);

	// Token: 0x04002A51 RID: 10833
	public static CellOffset[] LiquidSource = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(1, 0),
		new CellOffset(-1, 0),
		new CellOffset(0, 1),
		new CellOffset(0, -1),
		new CellOffset(1, 1),
		new CellOffset(1, -1),
		new CellOffset(-1, 1),
		new CellOffset(-1, -1),
		new CellOffset(2, 0),
		new CellOffset(-2, 0)
	};

	// Token: 0x04002A52 RID: 10834
	public static CellOffset[][] InvertedStandardTable = OffsetTable.Mirror(new CellOffset[][]
	{
		new CellOffset[]
		{
			new CellOffset(0, 0)
		},
		new CellOffset[]
		{
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(0, 2),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(0, 3),
			new CellOffset(0, 2),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(0, -1)
		},
		new CellOffset[]
		{
			new CellOffset(0, -2)
		},
		new CellOffset[]
		{
			new CellOffset(0, -3),
			new CellOffset(0, -2),
			new CellOffset(0, -1)
		},
		new CellOffset[]
		{
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(1, 1),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(1, 1),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(1, 2),
			new CellOffset(1, 1),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(1, 2),
			new CellOffset(0, 2),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(1, 3),
			new CellOffset(1, 2),
			new CellOffset(1, 1),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(1, 3),
			new CellOffset(0, 3),
			new CellOffset(0, 2),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(1, -1)
		},
		new CellOffset[]
		{
			new CellOffset(1, -2),
			new CellOffset(1, -1),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(1, -2),
			new CellOffset(1, -1),
			new CellOffset(0, -1)
		},
		new CellOffset[]
		{
			new CellOffset(1, -3),
			new CellOffset(1, -2),
			new CellOffset(1, -1),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(1, -3),
			new CellOffset(1, -2),
			new CellOffset(0, -2),
			new CellOffset(0, -1)
		},
		new CellOffset[]
		{
			new CellOffset(2, 0),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(2, 1),
			new CellOffset(1, 1),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(2, 1),
			new CellOffset(1, 1),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(2, 2),
			new CellOffset(1, 2),
			new CellOffset(1, 1),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(2, 2),
			new CellOffset(1, 2),
			new CellOffset(1, 1),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(2, 3),
			new CellOffset(1, 3),
			new CellOffset(1, 2),
			new CellOffset(1, 1),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(2, -1),
			new CellOffset(2, 0),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(2, -2),
			new CellOffset(2, -1),
			new CellOffset(1, -1),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(2, -3),
			new CellOffset(1, -2),
			new CellOffset(1, -1),
			new CellOffset(1, 0)
		}
	});

	// Token: 0x04002A53 RID: 10835
	public static CellOffset[][] InvertedStandardTableWithCorners = OffsetTable.Mirror(new CellOffset[][]
	{
		new CellOffset[]
		{
			new CellOffset(0, 0)
		},
		new CellOffset[]
		{
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(0, 2),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(0, 3),
			new CellOffset(0, 2),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(0, -1)
		},
		new CellOffset[]
		{
			new CellOffset(0, -2)
		},
		new CellOffset[]
		{
			new CellOffset(0, -3),
			new CellOffset(0, -2),
			new CellOffset(0, -1)
		},
		new CellOffset[]
		{
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(1, 1)
		},
		new CellOffset[]
		{
			new CellOffset(1, 2),
			new CellOffset(1, 1)
		},
		new CellOffset[]
		{
			new CellOffset(1, 2),
			new CellOffset(0, 2),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(1, 3),
			new CellOffset(1, 2),
			new CellOffset(1, 1)
		},
		new CellOffset[]
		{
			new CellOffset(1, 3),
			new CellOffset(0, 3),
			new CellOffset(0, 2),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(1, -1)
		},
		new CellOffset[]
		{
			new CellOffset(1, -2),
			new CellOffset(1, -1)
		},
		new CellOffset[]
		{
			new CellOffset(1, -3),
			new CellOffset(1, -2),
			new CellOffset(0, -2),
			new CellOffset(0, -1)
		},
		new CellOffset[]
		{
			new CellOffset(1, -3),
			new CellOffset(1, -2),
			new CellOffset(1, -1)
		},
		new CellOffset[]
		{
			new CellOffset(2, 0),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(2, 1),
			new CellOffset(1, 1)
		},
		new CellOffset[]
		{
			new CellOffset(2, 2),
			new CellOffset(1, 2),
			new CellOffset(1, 1)
		},
		new CellOffset[]
		{
			new CellOffset(2, 3),
			new CellOffset(1, 3),
			new CellOffset(1, 2),
			new CellOffset(1, 1)
		},
		new CellOffset[]
		{
			new CellOffset(2, -1),
			new CellOffset(2, 0),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(2, -2),
			new CellOffset(2, -1),
			new CellOffset(1, -1)
		},
		new CellOffset[]
		{
			new CellOffset(2, -3),
			new CellOffset(1, -2),
			new CellOffset(1, -1)
		}
	});

	// Token: 0x04002A54 RID: 10836
	public static CellOffset[][] InvertedWideTable = OffsetTable.Mirror(new CellOffset[][]
	{
		new CellOffset[]
		{
			new CellOffset(0, 0)
		},
		new CellOffset[]
		{
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(0, 2),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(0, 3),
			new CellOffset(0, 2),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(0, -1)
		},
		new CellOffset[]
		{
			new CellOffset(0, -2)
		},
		new CellOffset[]
		{
			new CellOffset(0, -3),
			new CellOffset(0, -2),
			new CellOffset(0, -1)
		},
		new CellOffset[]
		{
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(1, 1),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(1, 1),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(1, 2),
			new CellOffset(1, 1),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(1, 2),
			new CellOffset(0, 2),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(1, 3),
			new CellOffset(1, 2),
			new CellOffset(1, 1),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(1, 3),
			new CellOffset(0, 3),
			new CellOffset(0, 2),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(1, -1)
		},
		new CellOffset[]
		{
			new CellOffset(1, -2),
			new CellOffset(1, -1),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(1, -2),
			new CellOffset(1, -1),
			new CellOffset(0, -1)
		},
		new CellOffset[]
		{
			new CellOffset(1, -3),
			new CellOffset(1, -2),
			new CellOffset(1, -1),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(1, -3),
			new CellOffset(1, -2),
			new CellOffset(0, -2),
			new CellOffset(0, -1)
		},
		new CellOffset[]
		{
			new CellOffset(2, 0),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(2, 1),
			new CellOffset(1, 1),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(2, 1),
			new CellOffset(1, 1),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(2, 2),
			new CellOffset(1, 2),
			new CellOffset(1, 1),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(2, 2),
			new CellOffset(1, 2),
			new CellOffset(1, 1),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(2, 3),
			new CellOffset(1, 3),
			new CellOffset(1, 2),
			new CellOffset(1, 1),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(2, -1),
			new CellOffset(2, 0),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(2, -2),
			new CellOffset(2, -1),
			new CellOffset(1, -1),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(2, -3),
			new CellOffset(1, -2),
			new CellOffset(1, -1),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(3, 0),
			new CellOffset(2, 0),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(3, 1),
			new CellOffset(2, 1),
			new CellOffset(1, 1),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(3, 1),
			new CellOffset(2, 1),
			new CellOffset(1, 1),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(3, -1),
			new CellOffset(2, -1),
			new CellOffset(1, -1),
			new CellOffset(0, -1)
		},
		new CellOffset[]
		{
			new CellOffset(3, -1),
			new CellOffset(2, -1),
			new CellOffset(1, -1),
			new CellOffset(1, 0)
		}
	});

	// Token: 0x04002A55 RID: 10837
	private static Dictionary<CellOffset[], Dictionary<CellOffset[][], Dictionary<CellOffset[], CellOffset[][]>>> reachabilityTableCache = new Dictionary<CellOffset[], Dictionary<CellOffset[][], Dictionary<CellOffset[], CellOffset[][]>>>();

	// Token: 0x04002A56 RID: 10838
	private static readonly CellOffset[] nullFilter = new CellOffset[0];

	// Token: 0x02001709 RID: 5897
	private class CellOffsetComparer : IComparer<CellOffset>
	{
		// Token: 0x06008D4B RID: 36171 RVA: 0x0031B4C8 File Offset: 0x003196C8
		public int Compare(CellOffset a, CellOffset b)
		{
			int num = Math.Abs(a.x) + Math.Abs(a.y);
			int value = Math.Abs(b.x) + Math.Abs(b.y);
			return num.CompareTo(value);
		}
	}
}
