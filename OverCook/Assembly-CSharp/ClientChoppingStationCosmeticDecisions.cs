using System;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x02000397 RID: 919
public class ClientChoppingStationCosmeticDecisions : ClientSynchroniserBase
{
	// Token: 0x06001156 RID: 4438 RVA: 0x0006393C File Offset: 0x00061D3C
	private void Awake()
	{
		this.m_choppingStationCosmeticDecisions = base.gameObject.RequireComponent<ChoppingStationCosmeticDecisions>();
		this.m_attachStation = base.gameObject.RequireComponent<ClientAttachStation>();
		this.m_attachStation.RegisterOnItemAdded(new ClientAttachStation.OnItemAdded(this.OnItemAdded));
		this.m_attachStation.RegisterOnItemRemoved(new ClientAttachStation.OnItemRemoved(this.OnItemRemoved));
	}

	// Token: 0x06001157 RID: 4439 RVA: 0x00063999 File Offset: 0x00061D99
	private void OnItemAdded(IClientAttachment _iHoldable)
	{
		this.m_choppingStationCosmeticDecisions.m_knifeModel.SetActive(false);
	}

	// Token: 0x06001158 RID: 4440 RVA: 0x000639AC File Offset: 0x00061DAC
	private void OnItemRemoved(IClientAttachment _iHoldable)
	{
		this.m_choppingStationCosmeticDecisions.m_knifeModel.SetActive(true);
	}

	// Token: 0x04000D7E RID: 3454
	private ChoppingStationCosmeticDecisions m_choppingStationCosmeticDecisions;

	// Token: 0x04000D7F RID: 3455
	private ClientAttachStation m_attachStation;
}
