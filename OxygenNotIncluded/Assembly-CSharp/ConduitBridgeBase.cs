using System;

// Token: 0x020005DA RID: 1498
public class ConduitBridgeBase : KMonoBehaviour
{
	// Token: 0x06002533 RID: 9523 RVA: 0x000CAF36 File Offset: 0x000C9136
	protected void SendEmptyOnMassTransfer()
	{
		if (this.OnMassTransfer != null)
		{
			this.OnMassTransfer(SimHashes.Void, 0f, 0f, 0, 0, null);
		}
	}

	// Token: 0x04001550 RID: 5456
	public ConduitBridgeBase.DesiredMassTransfer desiredMassTransfer;

	// Token: 0x04001551 RID: 5457
	public ConduitBridgeBase.ConduitBridgeEvent OnMassTransfer;

	// Token: 0x02001276 RID: 4726
	// (Invoke) Token: 0x06007D7F RID: 32127
	public delegate float DesiredMassTransfer(float dt, SimHashes element, float mass, float temperature, byte disease_idx, int disease_count, Pickupable pickupable);

	// Token: 0x02001277 RID: 4727
	// (Invoke) Token: 0x06007D83 RID: 32131
	public delegate void ConduitBridgeEvent(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count, Pickupable pickupable);
}
