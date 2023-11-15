using System;
using UnityEngine;

// Token: 0x02000006 RID: 6
public class MB2_TestShowHide : MonoBehaviour
{
	// Token: 0x06000005 RID: 5 RVA: 0x00002070 File Offset: 0x00000470
	private void Update()
	{
		if (Time.frameCount == 100)
		{
			this.mb.ShowHide(null, this.objs);
			this.mb.ApplyShowHide();
			Debug.Log("should have disappeared");
		}
		if (Time.frameCount == 200)
		{
			this.mb.ShowHide(this.objs, null);
			this.mb.ApplyShowHide();
			Debug.Log("should show");
		}
	}

	// Token: 0x04000016 RID: 22
	public MB3_MeshBaker mb;

	// Token: 0x04000017 RID: 23
	public GameObject[] objs;
}
