using System;
using UnityEngine;

// Token: 0x02000270 RID: 624
public class MenuRot : MonoBehaviour
{
	// Token: 0x06000F80 RID: 3968 RVA: 0x000296D3 File Offset: 0x000278D3
	private void Start()
	{
	}

	// Token: 0x06000F81 RID: 3969 RVA: 0x000296D5 File Offset: 0x000278D5
	private void Update()
	{
		base.transform.Rotate(Vector3.up, this.rotSpeed * Time.deltaTime, Space.Self);
	}

	// Token: 0x040007E9 RID: 2025
	[SerializeField]
	private float rotSpeed;
}
