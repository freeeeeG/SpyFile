using System;
using UnityEngine;

// Token: 0x020004CF RID: 1231
public class MainCamera : MonoBehaviour
{
	// Token: 0x06001C33 RID: 7219 RVA: 0x00096E63 File Offset: 0x00095063
	private void Awake()
	{
		if (Camera.main != null)
		{
			UnityEngine.Object.Destroy(Camera.main.gameObject);
		}
		base.gameObject.tag = "MainCamera";
	}
}
