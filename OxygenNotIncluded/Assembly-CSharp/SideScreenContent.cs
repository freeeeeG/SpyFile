using System;
using UnityEngine;

// Token: 0x02000C49 RID: 3145
public abstract class SideScreenContent : KScreen
{
	// Token: 0x060063B6 RID: 25526 RVA: 0x0024E4AE File Offset: 0x0024C6AE
	public virtual void SetTarget(GameObject target)
	{
	}

	// Token: 0x060063B7 RID: 25527 RVA: 0x0024E4B0 File Offset: 0x0024C6B0
	public virtual void ClearTarget()
	{
	}

	// Token: 0x060063B8 RID: 25528
	public abstract bool IsValidForTarget(GameObject target);

	// Token: 0x060063B9 RID: 25529 RVA: 0x0024E4B2 File Offset: 0x0024C6B2
	public virtual int GetSideScreenSortOrder()
	{
		return 0;
	}

	// Token: 0x060063BA RID: 25530 RVA: 0x0024E4B5 File Offset: 0x0024C6B5
	public virtual string GetTitle()
	{
		return Strings.Get(this.titleKey);
	}

	// Token: 0x04004416 RID: 17430
	[SerializeField]
	protected string titleKey;

	// Token: 0x04004417 RID: 17431
	public GameObject ContentContainer;
}
