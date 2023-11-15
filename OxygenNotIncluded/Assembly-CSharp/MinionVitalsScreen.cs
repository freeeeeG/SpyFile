using System;
using UnityEngine;

// Token: 0x02000B97 RID: 2967
public class MinionVitalsScreen : TargetScreen
{
	// Token: 0x06005C6F RID: 23663 RVA: 0x0021E167 File Offset: 0x0021C367
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<MinionIdentity>();
	}

	// Token: 0x06005C70 RID: 23664 RVA: 0x0021E174 File Offset: 0x0021C374
	public override void ScreenUpdate(bool topLevel)
	{
		base.ScreenUpdate(topLevel);
	}

	// Token: 0x06005C71 RID: 23665 RVA: 0x0021E17D File Offset: 0x0021C37D
	public override void OnSelectTarget(GameObject target)
	{
		this.panel.selectedEntity = target;
		this.panel.Refresh();
	}

	// Token: 0x06005C72 RID: 23666 RVA: 0x0021E196 File Offset: 0x0021C396
	public override void OnDeselectTarget(GameObject target)
	{
	}

	// Token: 0x06005C73 RID: 23667 RVA: 0x0021E198 File Offset: 0x0021C398
	protected override void OnActivate()
	{
		base.OnActivate();
		if (this.panel == null)
		{
			this.panel = base.GetComponent<MinionVitalsPanel>();
		}
		this.panel.Init();
	}

	// Token: 0x04003E31 RID: 15921
	public MinionVitalsPanel panel;
}
