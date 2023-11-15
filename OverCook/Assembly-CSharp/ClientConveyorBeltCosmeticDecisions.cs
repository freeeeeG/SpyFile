using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020003A8 RID: 936
public class ClientConveyorBeltCosmeticDecisions : ClientSynchroniserBase
{
	// Token: 0x06001186 RID: 4486 RVA: 0x000646B4 File Offset: 0x00062AB4
	protected virtual void Awake()
	{
		this.m_speedPropID = Shader.PropertyToID("_speed");
		this.m_materialPropertyBlock = new MaterialPropertyBlock();
		this.m_conveyorStation = base.GetComponent<ConveyorStation>();
		this.m_ScrollSpeed = this.ConveySpeedToUVSpeed * this.m_conveyorStation.m_conveySpeed;
	}

	// Token: 0x06001187 RID: 4487 RVA: 0x00064700 File Offset: 0x00062B00
	protected virtual void Start()
	{
		this.m_materialPropertyBlock.SetFloat(this.m_speedPropID, (!this.LastScrollSet) ? 0f : this.m_ScrollSpeed);
		this.m_targetRenderer = this.m_conveyorStation.m_targetRenderer;
		this.m_targetRenderer.SetPropertyBlock(this.m_materialPropertyBlock);
	}

	// Token: 0x06001188 RID: 4488 RVA: 0x0006475C File Offset: 0x00062B5C
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_station = base.gameObject.RequireComponent<ClientConveyorStation>();
		this.m_attacher = base.gameObject.RequireComponent<ClientAttachStation>();
		this.m_receiver = base.gameObject.RequireInterface<IClientConveyenceReceiver>();
		this.m_station.RegisterConveyStateChangedCallback(delegate(bool _param1)
		{
			this.OnStateChanged();
		});
		this.m_attacher.RegisterOnItemAdded(delegate(IClientAttachment _iHoldable)
		{
			this.OnStateChanged();
		});
		this.m_attacher.RegisterOnItemRemoved(delegate(IClientAttachment _iHoldable)
		{
			this.OnStateChanged();
		});
		this.m_receiver.RegisterRefreshedConveyToCallback(new CallbackVoid(this.OnStateChanged));
		this.OnStateChanged();
	}

	// Token: 0x06001189 RID: 4489 RVA: 0x00064808 File Offset: 0x00062C08
	private void OnStateChanged()
	{
		bool flag = this.m_station.IsConveying() || this.m_receiver.IsReceiving() || !this.m_attacher.HasItem();
		if (flag != this.LastScrollSet)
		{
			this.LastScrollSet = flag;
			this.m_materialPropertyBlock.SetFloat(this.m_speedPropID, (!flag) ? 0f : this.m_ScrollSpeed);
			this.m_targetRenderer.SetPropertyBlock(this.m_materialPropertyBlock);
		}
	}

	// Token: 0x04000DA1 RID: 3489
	public bool LastScrollSet = true;

	// Token: 0x04000DA2 RID: 3490
	public float m_ScrollSpeed = 0.28f;

	// Token: 0x04000DA3 RID: 3491
	public float ConveySpeedToUVSpeed = 0.56f;

	// Token: 0x04000DA4 RID: 3492
	private MeshRenderer m_targetRenderer;

	// Token: 0x04000DA5 RID: 3493
	private IClientConveyenceReceiver m_receiver;

	// Token: 0x04000DA6 RID: 3494
	private ClientConveyorStation m_station;

	// Token: 0x04000DA7 RID: 3495
	private ClientAttachStation m_attacher;

	// Token: 0x04000DA8 RID: 3496
	private MaterialPropertyBlock m_materialPropertyBlock;

	// Token: 0x04000DA9 RID: 3497
	private ConveyorStation m_conveyorStation;

	// Token: 0x04000DAA RID: 3498
	private int m_speedPropID;
}
