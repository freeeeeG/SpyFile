using System;
using UnityEngine;

// Token: 0x02000542 RID: 1346
public class PlacementLayerSwapper : MonoBehaviour, ICarryNotified
{
	// Token: 0x06001937 RID: 6455 RVA: 0x0007F85E File Offset: 0x0007DC5E
	private void Awake()
	{
		this.m_defaultLayerId = LayerMask.NameToLayer("Attachments");
		this.m_layerWhenPlacedId = LayerMask.NameToLayer("HeldAttachments");
		base.gameObject.layer = this.m_defaultLayerId;
	}

	// Token: 0x06001938 RID: 6456 RVA: 0x0007F891 File Offset: 0x0007DC91
	private void UpdateLayer()
	{
		if (this.m_carried)
		{
			base.gameObject.layer = this.m_layerWhenPlacedId;
		}
		else
		{
			base.gameObject.layer = this.m_defaultLayerId;
		}
	}

	// Token: 0x06001939 RID: 6457 RVA: 0x0007F8C5 File Offset: 0x0007DCC5
	public void OnCarryBegun(ICarrier _carrier)
	{
		this.m_carried = true;
		this.UpdateLayer();
	}

	// Token: 0x0600193A RID: 6458 RVA: 0x0007F8D4 File Offset: 0x0007DCD4
	public void OnCarryEnded(ICarrier _carrier)
	{
		this.m_carried = false;
		this.UpdateLayer();
	}

	// Token: 0x0400142A RID: 5162
	[SerializeField]
	public string m_layerWhenPlaced;

	// Token: 0x0400142B RID: 5163
	private int m_defaultLayerId;

	// Token: 0x0400142C RID: 5164
	private int m_layerWhenPlacedId;

	// Token: 0x0400142D RID: 5165
	private bool m_carried;

	// Token: 0x0400142E RID: 5166
	private bool m_onSurface;
}
