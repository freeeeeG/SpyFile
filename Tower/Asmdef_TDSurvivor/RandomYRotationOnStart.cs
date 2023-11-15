using System;
using UnityEngine;

// Token: 0x020000D7 RID: 215
public class RandomYRotationOnStart : MonoBehaviour
{
	// Token: 0x0600051C RID: 1308 RVA: 0x00014A64 File Offset: 0x00012C64
	private void Awake()
	{
		int num = Random.Range(0, 4);
		base.transform.Rotate(0f, (float)(num * 90), 0f);
	}
}
