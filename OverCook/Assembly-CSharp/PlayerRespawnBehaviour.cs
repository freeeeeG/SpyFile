using System;
using System.Collections.Generic;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000A19 RID: 2585
[RequireComponent(typeof(PlayerControls))]
public class PlayerRespawnBehaviour : MonoBehaviour
{
	// Token: 0x0600333F RID: 13119 RVA: 0x000F0610 File Offset: 0x000EEA10
	private void Awake()
	{
		this.m_playerControls = base.gameObject.RequireComponent<PlayerControls>();
		this.m_windAccumulator = base.gameObject.RequestComponent<WindAccumulator>();
		Mailbox.Client.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		if (!GameUtils.GetLevelConfig().m_disableDynamicParenting)
		{
			this.m_groundCast = base.gameObject.RequireComponent<GroundCast>();
			if (this.m_groundCast != null)
			{
				this.m_groundCast.RegisterGroundChangedCallback(new VoidGeneric<Collider>(this.OnGroundChange));
			}
		}
	}

	// Token: 0x06003340 RID: 13120 RVA: 0x000F069F File Offset: 0x000EEA9F
	protected void OnDestroy()
	{
		Mailbox.Client.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x06003341 RID: 13121 RVA: 0x000F06BC File Offset: 0x000EEABC
	private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		GameStateMessage gameStateMessage = (GameStateMessage)message;
		if (gameStateMessage.m_State == GameState.StartEntities)
		{
			this.Initialise();
		}
	}

	// Token: 0x06003342 RID: 13122 RVA: 0x000F06E3 File Offset: 0x000EEAE3
	private void Initialise()
	{
		if (this.m_initialGroundFound)
		{
			return;
		}
		this.m_startParent = base.transform.parent;
		this.m_startLocation = base.transform.localPosition;
	}

	// Token: 0x06003343 RID: 13123 RVA: 0x000F0714 File Offset: 0x000EEB14
	private void OnGroundChange(Collider collider)
	{
		if (!this.m_initialGroundFound)
		{
			if (!this.m_groundCast.HasGroundContact())
			{
				return;
			}
			Collider groundCollider = this.m_groundCast.GetGroundCollider();
			if (groundCollider != null)
			{
				IParentable parentable = groundCollider.gameObject.RequestInterfaceUpwardsRecursive<IParentable>();
				if (parentable != null)
				{
					Transform attachPoint = parentable.GetAttachPoint(base.gameObject);
					if (attachPoint != null)
					{
						this.m_startParent = attachPoint;
						this.m_startLocation = this.m_startParent.InverseTransformPoint(base.transform.position);
						this.m_initialGroundFound = true;
					}
				}
			}
		}
	}

	// Token: 0x04002928 RID: 10536
	[SerializeField]
	public float m_respawnTime = 5f;

	// Token: 0x04002929 RID: 10537
	[SerializeField]
	public float m_switchDelay = 0.5f;

	// Token: 0x0400292A RID: 10538
	[SerializeField]
	public float m_particleTime = 1f;

	// Token: 0x0400292B RID: 10539
	[SerializeField]
	public float m_maxDrowingVerticalVelocity = -0.1f;

	// Token: 0x0400292C RID: 10540
	[SerializeField]
	public GameObject m_spawnEffect;

	// Token: 0x0400292D RID: 10541
	[SerializeField]
	public GameObject m_fallingEffect;

	// Token: 0x0400292E RID: 10542
	[SerializeField]
	public GameObject m_respawnCounterPrefab;

	// Token: 0x0400292F RID: 10543
	[SerializeField]
	public Shader m_fadeShader;

	// Token: 0x04002930 RID: 10544
	[SerializeField]
	public GameOneShotAudioTag m_fallAudioTag = GameOneShotAudioTag.PlayerFall;

	// Token: 0x04002931 RID: 10545
	[SerializeField]
	public GameOneShotAudioTag m_drownAudioTag = GameOneShotAudioTag.PlayerDive;

	// Token: 0x04002932 RID: 10546
	[SerializeField]
	public GameOneShotAudioTag m_carHitAudioTag = GameOneShotAudioTag.PlayerSlip;

	// Token: 0x04002933 RID: 10547
	[SerializeField]
	public List<PlayerRespawnBehaviour.RespawnParams> m_respawnParams = new List<PlayerRespawnBehaviour.RespawnParams>
	{
		new PlayerRespawnBehaviour.RespawnParams(RespawnCollider.RespawnType.Hit, PlayerRespawnBehaviour.RespawnParams.Type.Fade, 1f, 2f),
		new PlayerRespawnBehaviour.RespawnParams(RespawnCollider.RespawnType.Car, PlayerRespawnBehaviour.RespawnParams.Type.Fade, 1f, 2f)
	};

	// Token: 0x04002934 RID: 10548
	public PlayerControls m_playerControls;

	// Token: 0x04002935 RID: 10549
	public WindAccumulator m_windAccumulator;

	// Token: 0x04002936 RID: 10550
	public Vector3 m_startLocation;

	// Token: 0x04002937 RID: 10551
	public Transform m_startParent;

	// Token: 0x04002938 RID: 10552
	private GroundCast m_groundCast;

	// Token: 0x04002939 RID: 10553
	private bool m_initialGroundFound;

	// Token: 0x02000A1A RID: 2586
	[Serializable]
	public struct RespawnParams
	{
		// Token: 0x06003344 RID: 13124 RVA: 0x000F07AA File Offset: 0x000EEBAA
		public RespawnParams(RespawnCollider.RespawnType _deathBy, PlayerRespawnBehaviour.RespawnParams.Type _type, float _delay, float _duration)
		{
			this.m_deathBy = _deathBy;
			this.m_type = _type;
			this.m_delay = _delay;
			this.m_duration = _duration;
		}

		// Token: 0x0400293A RID: 10554
		[SerializeField]
		public RespawnCollider.RespawnType m_deathBy;

		// Token: 0x0400293B RID: 10555
		[SerializeField]
		public PlayerRespawnBehaviour.RespawnParams.Type m_type;

		// Token: 0x0400293C RID: 10556
		[SerializeField]
		public float m_delay;

		// Token: 0x0400293D RID: 10557
		[SerializeField]
		public float m_duration;

		// Token: 0x02000A1B RID: 2587
		public enum Type
		{
			// Token: 0x0400293F RID: 10559
			Fade,
			// Token: 0x04002940 RID: 10560
			Scale,
			// Token: 0x04002941 RID: 10561
			None
		}
	}
}
