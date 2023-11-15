using System;
using UnityEngine;

// Token: 0x02000BC2 RID: 3010
public class PlanSubCategoryToggle : KMonoBehaviour
{
	// Token: 0x06005E7C RID: 24188 RVA: 0x0022B0CE File Offset: 0x002292CE
	protected override void OnSpawn()
	{
		base.OnSpawn();
		MultiToggle multiToggle = this.toggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.open = !this.open;
			this.gridContainer.SetActive(this.open);
			this.toggle.ChangeState(this.open ? 0 : 1);
		}));
	}

	// Token: 0x04003FB6 RID: 16310
	[SerializeField]
	private MultiToggle toggle;

	// Token: 0x04003FB7 RID: 16311
	[SerializeField]
	private GameObject gridContainer;

	// Token: 0x04003FB8 RID: 16312
	private bool open = true;
}
