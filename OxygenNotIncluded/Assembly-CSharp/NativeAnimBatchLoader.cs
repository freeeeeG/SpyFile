using System;
using UnityEngine;

// Token: 0x02000454 RID: 1108
public class NativeAnimBatchLoader : MonoBehaviour
{
	// Token: 0x06001835 RID: 6197 RVA: 0x0007DB48 File Offset: 0x0007BD48
	private void Start()
	{
		if (this.generateObjects)
		{
			for (int i = 0; i < this.enableObjects.Length; i++)
			{
				if (this.enableObjects[i] != null)
				{
					this.enableObjects[i].GetComponent<KBatchedAnimController>().visibilityType = KAnimControllerBase.VisibilityType.Always;
					this.enableObjects[i].SetActive(true);
				}
			}
		}
		if (this.setTimeScale)
		{
			Time.timeScale = 1f;
		}
		if (this.destroySelf)
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x06001836 RID: 6198 RVA: 0x0007DBC4 File Offset: 0x0007BDC4
	private void LateUpdate()
	{
		if (this.destroySelf)
		{
			return;
		}
		if (this.performUpdate)
		{
			KAnimBatchManager.Instance().UpdateActiveArea(new Vector2I(0, 0), new Vector2I(9999, 9999));
			KAnimBatchManager.Instance().UpdateDirty(Time.frameCount);
		}
		if (this.performRender)
		{
			KAnimBatchManager.Instance().Render();
		}
	}

	// Token: 0x04000D62 RID: 3426
	public bool performTimeUpdate;

	// Token: 0x04000D63 RID: 3427
	public bool performUpdate;

	// Token: 0x04000D64 RID: 3428
	public bool performRender;

	// Token: 0x04000D65 RID: 3429
	public bool setTimeScale;

	// Token: 0x04000D66 RID: 3430
	public bool destroySelf;

	// Token: 0x04000D67 RID: 3431
	public bool generateObjects;

	// Token: 0x04000D68 RID: 3432
	public GameObject[] enableObjects;
}
