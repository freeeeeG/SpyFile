using System;
using System.Collections.Generic;
using Pathfinding.Util;

namespace Pathfinding.Voxels
{
	// Token: 0x020000B2 RID: 178
	public class Utility
	{
		// Token: 0x060007D1 RID: 2001 RVA: 0x0003383F File Offset: 0x00031A3F
		public static float Min(float a, float b, float c)
		{
			a = ((a < b) ? a : b);
			if (a >= c)
			{
				return c;
			}
			return a;
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x00033852 File Offset: 0x00031A52
		public static float Max(float a, float b, float c)
		{
			a = ((a > b) ? a : b);
			if (a <= c)
			{
				return c;
			}
			return a;
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x00033868 File Offset: 0x00031A68
		public static Int3[] RemoveDuplicateVertices(Int3[] vertices, int[] triangles)
		{
			Dictionary<Int3, int> dictionary = ObjectPoolSimple<Dictionary<Int3, int>>.Claim();
			dictionary.Clear();
			int[] array = new int[vertices.Length];
			int num = 0;
			for (int i = 0; i < vertices.Length; i++)
			{
				if (!dictionary.ContainsKey(vertices[i]))
				{
					dictionary.Add(vertices[i], num);
					array[i] = num;
					vertices[num] = vertices[i];
					num++;
				}
				else
				{
					array[i] = dictionary[vertices[i]];
				}
			}
			dictionary.Clear();
			ObjectPoolSimple<Dictionary<Int3, int>>.Release(ref dictionary);
			for (int j = 0; j < triangles.Length; j++)
			{
				triangles[j] = array[triangles[j]];
			}
			Int3[] array2 = new Int3[num];
			for (int k = 0; k < num; k++)
			{
				array2[k] = vertices[k];
			}
			return array2;
		}
	}
}
