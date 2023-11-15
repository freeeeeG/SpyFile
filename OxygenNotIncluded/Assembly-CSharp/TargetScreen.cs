using System;
using UnityEngine;

// Token: 0x02000C76 RID: 3190
public abstract class TargetScreen : KScreen
{
	// Token: 0x060065BD RID: 26045
	public abstract bool IsValidForTarget(GameObject target);

	// Token: 0x060065BE RID: 26046 RVA: 0x0025F140 File Offset: 0x0025D340
	public void SetTarget(GameObject target)
	{
		if (this.selectedTarget != target)
		{
			if (this.selectedTarget != null)
			{
				this.OnDeselectTarget(this.selectedTarget);
			}
			this.selectedTarget = target;
			if (this.selectedTarget != null)
			{
				this.OnSelectTarget(this.selectedTarget);
			}
		}
	}

	// Token: 0x060065BF RID: 26047 RVA: 0x0025F196 File Offset: 0x0025D396
	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		this.SetTarget(null);
	}

	// Token: 0x060065C0 RID: 26048 RVA: 0x0025F1A5 File Offset: 0x0025D3A5
	public virtual void OnSelectTarget(GameObject target)
	{
		target.Subscribe(1502190696, new Action<object>(this.OnTargetDestroyed));
	}

	// Token: 0x060065C1 RID: 26049 RVA: 0x0025F1BF File Offset: 0x0025D3BF
	public virtual void OnDeselectTarget(GameObject target)
	{
		target.Unsubscribe(1502190696, new Action<object>(this.OnTargetDestroyed));
	}

	// Token: 0x060065C2 RID: 26050 RVA: 0x0025F1D8 File Offset: 0x0025D3D8
	private void OnTargetDestroyed(object data)
	{
		DetailsScreen.Instance.Show(false);
		this.SetTarget(null);
	}

	// Token: 0x0400460C RID: 17932
	protected GameObject selectedTarget;
}
