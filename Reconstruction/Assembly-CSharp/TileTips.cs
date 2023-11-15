using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000290 RID: 656
public class TileTips : IUserInterface
{
	// Token: 0x0600102B RID: 4139 RVA: 0x0002B6D0 File Offset: 0x000298D0
	public override void Show()
	{
		base.Show();
		this.anim.SetBool("isOpen", true);
	}

	// Token: 0x0600102C RID: 4140 RVA: 0x0002B6E9 File Offset: 0x000298E9
	public virtual void CloseTips()
	{
		this.anim.SetBool("isOpen", false);
	}

	// Token: 0x04000871 RID: 2161
	[SerializeField]
	protected Animator anim;

	// Token: 0x04000872 RID: 2162
	[SerializeField]
	protected Image Icon;

	// Token: 0x04000873 RID: 2163
	[SerializeField]
	protected TextMeshProUGUI Name;

	// Token: 0x04000874 RID: 2164
	[SerializeField]
	protected TextMeshProUGUI Description;
}
