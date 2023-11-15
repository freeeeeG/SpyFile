using System;
using System.Runtime.CompilerServices;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x0200057A RID: 1402
public class ClientConveyorStation : ClientSynchroniserBase
{
	// Token: 0x06001A92 RID: 6802 RVA: 0x00085214 File Offset: 0x00083614
	public static IClientSidePredicted CreateConveyorPrediction()
	{
		return new ConveyorPrediction();
	}

	// Token: 0x06001A93 RID: 6803 RVA: 0x0008521B File Offset: 0x0008361B
	public override EntityType GetEntityType()
	{
		return EntityType.ConveyorStation;
	}

	// Token: 0x06001A94 RID: 6804 RVA: 0x00085220 File Offset: 0x00083620
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		ConveyorStationMessage conveyorStationMessage = (ConveyorStationMessage)serialisable;
		EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(conveyorStationMessage.m_receiverEntityID);
		EntitySerialisationEntry entry2 = EntitySerialisationRegistry.GetEntry(conveyorStationMessage.m_itemEntityID);
		if (entry2 != null)
		{
			IClientConveyenceReceiver clientConveyenceReceiver = entry.m_GameObject.RequireInterface<IClientConveyenceReceiver>();
			if (clientConveyenceReceiver != this.m_receiver)
			{
				this.m_receiver = clientConveyenceReceiver;
				this.m_receiverEntityID = conveyorStationMessage.m_receiverEntityID;
			}
			this.m_item = entry2.m_GameObject.RequireInterface<IClientAttachment>();
			if (this.m_item != null)
			{
				IParentable parentable = entry.m_GameObject.RequestInterface<IParentable>();
				if (parentable != null)
				{
					IClientSidePredicted clientSidePrediction = this.m_item.GetClientSidePrediction();
					if (clientSidePrediction == null || !(clientSidePrediction is ConveyorPrediction))
					{
						this.m_item.SetClientSidePrediction(ClientConveyorStation.m_CreateConveyorPredictionCallback);
						clientSidePrediction = this.m_item.GetClientSidePrediction();
						ConveyorPrediction conveyorPrediction = clientSidePrediction as ConveyorPrediction;
						if (conveyorPrediction != null)
						{
							conveyorPrediction.m_Transform = this.m_item.AccessGameObject().transform;
						}
					}
					ConveyorPrediction conveyorPrediction2 = clientSidePrediction as ConveyorPrediction;
					if (conveyorPrediction2 != null)
					{
						ConveyorPrediction.Destination dest = new ConveyorPrediction.Destination
						{
							targetEntityID = conveyorStationMessage.m_receiverEntityID,
							targetTransform = parentable.GetAttachPoint(this.m_item.AccessGameObject()),
							arriveTime = conveyorStationMessage.m_arriveTime
						};
						conveyorPrediction2.EnqueueDestination(dest);
					}
				}
			}
		}
	}

	// Token: 0x06001A95 RID: 6805 RVA: 0x0008536C File Offset: 0x0008376C
	private void OnStartedMovingToDestination(ConveyorPrediction.Destination destination, ConveyorPrediction prediction)
	{
		if (!this.m_isConveying)
		{
			this.m_isConveying = true;
			this.m_conveyStateChanged(this.m_isConveying);
		}
		if (this.m_receiverEntityID == destination.targetEntityID)
		{
			this.m_receiver.InformStartingConveyToMe();
		}
	}

	// Token: 0x06001A96 RID: 6806 RVA: 0x000853BC File Offset: 0x000837BC
	private void OnDestinationReached(ConveyorPrediction.Destination destination, ConveyorPrediction prediction)
	{
		if (this.m_receiverEntityID == destination.targetEntityID)
		{
			this.m_item = null;
			if (this.m_isConveying)
			{
				this.m_isConveying = false;
				this.m_conveyStateChanged(this.m_isConveying);
			}
			if (this.m_receiver != null)
			{
				this.m_receiver.InformEndingConveyToMe();
			}
		}
	}

	// Token: 0x06001A97 RID: 6807 RVA: 0x0008541C File Offset: 0x0008381C
	private void Awake()
	{
		this.m_conveyorStation = base.gameObject.RequireComponent<ConveyorStation>();
		ConveyorPrediction.OnStartedMovingToDestination = (GenericVoid<ConveyorPrediction.Destination, ConveyorPrediction>)Delegate.Combine(ConveyorPrediction.OnStartedMovingToDestination, new GenericVoid<ConveyorPrediction.Destination, ConveyorPrediction>(this.OnStartedMovingToDestination));
		ConveyorPrediction.OnDestinationReached = (GenericVoid<ConveyorPrediction.Destination, ConveyorPrediction>)Delegate.Combine(ConveyorPrediction.OnDestinationReached, new GenericVoid<ConveyorPrediction.Destination, ConveyorPrediction>(this.OnDestinationReached));
	}

	// Token: 0x06001A98 RID: 6808 RVA: 0x0008547C File Offset: 0x0008387C
	protected override void OnDestroy()
	{
		ConveyorPrediction.OnStartedMovingToDestination = (GenericVoid<ConveyorPrediction.Destination, ConveyorPrediction>)Delegate.Remove(ConveyorPrediction.OnStartedMovingToDestination, new GenericVoid<ConveyorPrediction.Destination, ConveyorPrediction>(this.OnStartedMovingToDestination));
		ConveyorPrediction.OnDestinationReached = (GenericVoid<ConveyorPrediction.Destination, ConveyorPrediction>)Delegate.Remove(ConveyorPrediction.OnDestinationReached, new GenericVoid<ConveyorPrediction.Destination, ConveyorPrediction>(this.OnDestinationReached));
		base.OnDestroy();
	}

	// Token: 0x06001A99 RID: 6809 RVA: 0x000854CF File Offset: 0x000838CF
	public bool IsConveying()
	{
		return this.m_isConveying;
	}

	// Token: 0x06001A9A RID: 6810 RVA: 0x000854D7 File Offset: 0x000838D7
	public void RegisterConveyStateChangedCallback(CallbackBool _callback)
	{
		this.m_conveyStateChanged = (CallbackBool)Delegate.Combine(this.m_conveyStateChanged, _callback);
	}

	// Token: 0x06001A9B RID: 6811 RVA: 0x000854F0 File Offset: 0x000838F0
	public void UnregisterConveyStateChangedCallback(CallbackBool _callback)
	{
		this.m_conveyStateChanged = (CallbackBool)Delegate.Remove(this.m_conveyStateChanged, _callback);
	}

	// Token: 0x06001A9C RID: 6812 RVA: 0x00085509 File Offset: 0x00083909
	// Note: this type is marked as 'beforefieldinit'.
	static ClientConveyorStation()
	{
		if (ClientConveyorStation.<>f__mg$cache0 == null)
		{
			ClientConveyorStation.<>f__mg$cache0 = new CreateClientSidePredictionCallback(ClientConveyorStation.CreateConveyorPrediction);
		}
		ClientConveyorStation.m_CreateConveyorPredictionCallback = ClientConveyorStation.<>f__mg$cache0;
	}

	// Token: 0x04001502 RID: 5378
	public ConveyorStation m_conveyorStation;

	// Token: 0x04001503 RID: 5379
	public static CreateClientSidePredictionCallback m_CreateConveyorPredictionCallback;

	// Token: 0x04001504 RID: 5380
	private IClientAttachment m_item;

	// Token: 0x04001505 RID: 5381
	private IClientConveyenceReceiver m_receiver;

	// Token: 0x04001506 RID: 5382
	private uint m_receiverEntityID;

	// Token: 0x04001507 RID: 5383
	private bool m_isConveying;

	// Token: 0x04001508 RID: 5384
	private CallbackBool m_conveyStateChanged = delegate(bool _conveying)
	{
	};

	// Token: 0x0400150A RID: 5386
	[CompilerGenerated]
	private static CreateClientSidePredictionCallback <>f__mg$cache0;
}
