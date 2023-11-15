using System;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000632 RID: 1586
[RequireComponent(typeof(BoxCollider))]
public class WindVolume : MonoBehaviour, IWindSource
{
	// Token: 0x06001E1F RID: 7711 RVA: 0x00091E70 File Offset: 0x00090270
	public Vector3 GetVelocity()
	{
		return (!base.enabled) ? Vector3.zero : (this.m_windSpeed * base.transform.right);
	}

	// Token: 0x06001E20 RID: 7712 RVA: 0x00091EA0 File Offset: 0x000902A0
	public void ObjectAdded(GameObject _gameObject)
	{
		if ((this.m_windFilter.value & 1 << _gameObject.layer) != 0)
		{
			IWindReceiver windReceiver = _gameObject.RequestInterface<IWindReceiver>();
			if (windReceiver != null)
			{
				windReceiver.AddWindSource(this);
			}
		}
	}

	// Token: 0x06001E21 RID: 7713 RVA: 0x00091EE0 File Offset: 0x000902E0
	public void ObjectRemoved(GameObject _gameObject)
	{
		if ((this.m_windFilter.value & 1 << _gameObject.layer) != 0)
		{
			IWindReceiver windReceiver = _gameObject.RequestInterface<IWindReceiver>();
			if (windReceiver != null)
			{
				windReceiver.RemoveWindSource(this);
			}
		}
	}

	// Token: 0x06001E22 RID: 7714 RVA: 0x00091F1D File Offset: 0x0009031D
	private void OnCollisionEnter(Collision collision)
	{
		this.ObjectAdded(collision.gameObject);
	}

	// Token: 0x06001E23 RID: 7715 RVA: 0x00091F2B File Offset: 0x0009032B
	private void OnCollisionStay(Collision collision)
	{
		this.ObjectAdded(collision.collider.gameObject);
	}

	// Token: 0x06001E24 RID: 7716 RVA: 0x00091F3E File Offset: 0x0009033E
	private void OnTriggerEnter(Collider collider)
	{
		this.ObjectAdded(collider.gameObject);
	}

	// Token: 0x06001E25 RID: 7717 RVA: 0x00091F4C File Offset: 0x0009034C
	private void OnTriggerStay(Collider collider)
	{
		this.ObjectAdded(collider.gameObject);
	}

	// Token: 0x06001E26 RID: 7718 RVA: 0x00091F5A File Offset: 0x0009035A
	private void OnCollisionExit(Collision collision)
	{
		this.ObjectRemoved(collision.gameObject);
	}

	// Token: 0x06001E27 RID: 7719 RVA: 0x00091F68 File Offset: 0x00090368
	private void OnTriggerExit(Collider collider)
	{
		this.ObjectRemoved(collider.gameObject);
	}

	// Token: 0x06001E28 RID: 7720 RVA: 0x00091F78 File Offset: 0x00090378
	private void Awake()
	{
		Mailbox.Client.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		BoxCollider boxCollider = base.gameObject.RequireComponentRecursive<BoxCollider>();
		this.m_volumeBounds = boxCollider.bounds;
	}

	// Token: 0x06001E29 RID: 7721 RVA: 0x00091FB5 File Offset: 0x000903B5
	private void OnEnable()
	{
		this.AddInitialObjects();
	}

	// Token: 0x06001E2A RID: 7722 RVA: 0x00091FC0 File Offset: 0x000903C0
	private void Update()
	{
		if (!Mathf.Approximately(this.m_windSpeed, 0f) && !this.m_isWindy)
		{
			GameUtils.TriggerAudio(GameOneShotAudioTag.WindGust, base.gameObject.layer);
			this.m_isWindy = true;
		}
		else if (Mathf.Approximately(this.m_windSpeed, 0f))
		{
			this.m_isWindy = false;
		}
	}

	// Token: 0x06001E2B RID: 7723 RVA: 0x00092028 File Offset: 0x00090428
	private void OnDisable()
	{
		Collider[] array = Physics.OverlapBox(this.m_volumeBounds.center, this.m_volumeBounds.extents, base.transform.rotation);
		for (int i = 0; i < array.Length; i++)
		{
			this.OnTriggerExit(array[i]);
		}
		this.m_isWindy = false;
	}

	// Token: 0x06001E2C RID: 7724 RVA: 0x00092080 File Offset: 0x00090480
	private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		GameStateMessage gameStateMessage = (GameStateMessage)message;
		if (gameStateMessage.m_State == GameState.StartEntities)
		{
			this.AddInitialObjects();
		}
	}

	// Token: 0x06001E2D RID: 7725 RVA: 0x000920A7 File Offset: 0x000904A7
	protected virtual void OnDestroy()
	{
		Mailbox.Client.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x06001E2E RID: 7726 RVA: 0x000920C4 File Offset: 0x000904C4
	private void AddInitialObjects()
	{
		BoxCollider boxCollider = base.gameObject.RequireComponent<BoxCollider>();
		Collider[] array = Physics.OverlapBox(boxCollider.bounds.center, boxCollider.bounds.extents, base.transform.rotation);
		for (int i = 0; i < array.Length; i++)
		{
			this.OnTriggerEnter(array[i]);
		}
	}

	// Token: 0x0400173D RID: 5949
	[SerializeField]
	private LayerMask m_windFilter = -1;

	// Token: 0x0400173E RID: 5950
	[SerializeField]
	private float m_windSpeed;

	// Token: 0x0400173F RID: 5951
	private bool m_isWindy;

	// Token: 0x04001740 RID: 5952
	private Bounds m_volumeBounds;
}
