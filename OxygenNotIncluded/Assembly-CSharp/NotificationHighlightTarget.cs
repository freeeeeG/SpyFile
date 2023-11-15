using System;

// Token: 0x02000BAC RID: 2988
public class NotificationHighlightTarget : KMonoBehaviour
{
	// Token: 0x06005D39 RID: 23865 RVA: 0x00221DBD File Offset: 0x0021FFBD
	protected void OnEnable()
	{
		this.controller = base.GetComponentInParent<NotificationHighlightController>();
		if (this.controller != null)
		{
			this.controller.AddTarget(this);
		}
	}

	// Token: 0x06005D3A RID: 23866 RVA: 0x00221DE5 File Offset: 0x0021FFE5
	protected override void OnDisable()
	{
		if (this.controller != null)
		{
			this.controller.RemoveTarget(this);
		}
	}

	// Token: 0x06005D3B RID: 23867 RVA: 0x00221E01 File Offset: 0x00220001
	public void View()
	{
		base.GetComponentInParent<NotificationHighlightController>().TargetViewed(this);
	}

	// Token: 0x04003EBE RID: 16062
	public string targetKey;

	// Token: 0x04003EBF RID: 16063
	private NotificationHighlightController controller;
}
