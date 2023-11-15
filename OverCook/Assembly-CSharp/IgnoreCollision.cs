using System;
using UnityEngine;

// Token: 0x020005A1 RID: 1441
public class IgnoreCollision : MonoBehaviour
{
	// Token: 0x06001B68 RID: 7016 RVA: 0x00087C04 File Offset: 0x00086004
	private void Awake()
	{
		Collider[] array = this.m_target.RequestComponentsRecursive<Collider>();
		Collider[] array2 = base.gameObject.RequestComponentsRecursive<Collider>();
		for (int i = 0; i < array.Length; i++)
		{
			for (int j = 0; j < array2.Length; j++)
			{
				Physics.IgnoreCollision(array[i], array2[j]);
			}
		}
	}

	// Token: 0x0400157E RID: 5502
	[SerializeField]
	private GameObject m_target;
}
