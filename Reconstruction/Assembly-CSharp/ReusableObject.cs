using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000130 RID: 304
public abstract class ReusableObject : MonoBehaviour, IResuable
{
	// Token: 0x060007F5 RID: 2037 RVA: 0x00014BB3 File Offset: 0x00012DB3
	public virtual void OnSpawn()
	{
	}

	// Token: 0x060007F6 RID: 2038 RVA: 0x00014BB5 File Offset: 0x00012DB5
	public virtual void OnUnSpawn()
	{
		this.SetBackToParent();
	}

	// Token: 0x060007F7 RID: 2039 RVA: 0x00014BBD File Offset: 0x00012DBD
	public void SetBackToParent()
	{
		if (this.ParentObj != null)
		{
			base.transform.SetParent(this.ParentObj);
		}
	}

	// Token: 0x060007F8 RID: 2040 RVA: 0x00014BDE File Offset: 0x00012DDE
	public void UnspawnAfterTime(float time)
	{
		base.StartCoroutine(this.UnspawnCor(time));
	}

	// Token: 0x060007F9 RID: 2041 RVA: 0x00014BEE File Offset: 0x00012DEE
	private IEnumerator UnspawnCor(float time)
	{
		yield return new WaitForSeconds(time);
		Singleton<ObjectPool>.Instance.UnSpawn(this);
		yield break;
	}

	// Token: 0x040003C9 RID: 969
	[HideInInspector]
	public Transform ParentObj;

	// Token: 0x040003CA RID: 970
	[HideInInspector]
	public bool isActive;
}
