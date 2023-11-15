using System;
using UnityEngine;

// Token: 0x02000C44 RID: 3140
public class SealedDoorSideScreen : SideScreenContent
{
	// Token: 0x06006376 RID: 25462 RVA: 0x0024C078 File Offset: 0x0024A278
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.button.onClick += delegate()
		{
			this.target.OrderUnseal();
		};
		this.Refresh();
	}

	// Token: 0x06006377 RID: 25463 RVA: 0x0024C09D File Offset: 0x0024A29D
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Door>() != null;
	}

	// Token: 0x06006378 RID: 25464 RVA: 0x0024C0AC File Offset: 0x0024A2AC
	public override void SetTarget(GameObject target)
	{
		Door component = target.GetComponent<Door>();
		if (component == null)
		{
			global::Debug.LogError("Target doesn't have a Door associated with it.");
			return;
		}
		this.target = component;
		this.Refresh();
	}

	// Token: 0x06006379 RID: 25465 RVA: 0x0024C0E1 File Offset: 0x0024A2E1
	private void Refresh()
	{
		if (!this.target.isSealed)
		{
			this.ContentContainer.SetActive(false);
			return;
		}
		this.ContentContainer.SetActive(true);
	}

	// Token: 0x040043E1 RID: 17377
	[SerializeField]
	private LocText label;

	// Token: 0x040043E2 RID: 17378
	[SerializeField]
	private KButton button;

	// Token: 0x040043E3 RID: 17379
	[SerializeField]
	private Door target;
}
