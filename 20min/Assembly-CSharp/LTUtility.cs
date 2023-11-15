using System;
using UnityEngine;

// Token: 0x02000026 RID: 38
public class LTUtility
{
	// Token: 0x060002FC RID: 764 RVA: 0x000126A8 File Offset: 0x000108A8
	public static Vector3[] reverse(Vector3[] arr)
	{
		int num = arr.Length;
		int i = 0;
		int num2 = num - 1;
		while (i < num2)
		{
			Vector3 vector = arr[i];
			arr[i] = arr[num2];
			arr[num2] = vector;
			i++;
			num2--;
		}
		return arr;
	}
}
