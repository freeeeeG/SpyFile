using System;
using UnityEngine;

// Token: 0x02000B01 RID: 2817
public class FoldOutPanel : KMonoBehaviour
{
	// Token: 0x060056E2 RID: 22242 RVA: 0x001FC1F8 File Offset: 0x001FA3F8
	protected override void OnSpawn()
	{
		MultiToggle componentInChildren = base.GetComponentInChildren<MultiToggle>();
		componentInChildren.onClick = (System.Action)Delegate.Combine(componentInChildren.onClick, new System.Action(this.OnClick));
		this.ToggleOpen(this.startOpen);
	}

	// Token: 0x060056E3 RID: 22243 RVA: 0x001FC22D File Offset: 0x001FA42D
	private void OnClick()
	{
		this.ToggleOpen(!this.panelOpen);
	}

	// Token: 0x060056E4 RID: 22244 RVA: 0x001FC23E File Offset: 0x001FA43E
	private void ToggleOpen(bool open)
	{
		this.panelOpen = open;
		this.container.SetActive(this.panelOpen);
		base.GetComponentInChildren<MultiToggle>().ChangeState(this.panelOpen ? 1 : 0);
	}

	// Token: 0x04003A94 RID: 14996
	private bool panelOpen = true;

	// Token: 0x04003A95 RID: 14997
	public GameObject container;

	// Token: 0x04003A96 RID: 14998
	public bool startOpen = true;
}
