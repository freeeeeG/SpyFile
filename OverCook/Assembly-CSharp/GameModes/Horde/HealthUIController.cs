using System;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameModes.Horde
{
	// Token: 0x020007BB RID: 1979
	public class HealthUIController : UIControllerBase
	{
		// Token: 0x060025E4 RID: 9700 RVA: 0x000B33F4 File Offset: 0x000B17F4
		private void Awake()
		{
			this.m_healthBar.Value = 1f;
			this.m_healthAlertId = Animator.StringToHash(this.m_healthAlertString);
			this.m_healthPulseId = Animator.StringToHash(this.m_healthPulseString);
			Mailbox.Client.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		}

		// Token: 0x060025E5 RID: 9701 RVA: 0x000B344B File Offset: 0x000B184B
		private void OnDestroy()
		{
			Mailbox.Client.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		}

		// Token: 0x060025E6 RID: 9702 RVA: 0x000B3468 File Offset: 0x000B1868
		private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
		{
			GameStateMessage gameStateMessage = (GameStateMessage)message;
			if (gameStateMessage.m_State == GameState.StartEntities)
			{
				FlowControllerBase flowControllerBase = GameUtils.RequireManager<FlowControllerBase>();
				this.m_flowController = flowControllerBase.gameObject.RequireComponent<ClientHordeFlowController>();
				this.m_health = (float)this.m_flowController.Health;
			}
		}

		// Token: 0x060025E7 RID: 9703 RVA: 0x000B34B4 File Offset: 0x000B18B4
		private void Update()
		{
			if (this.m_flowController != null)
			{
				float num = (float)this.m_flowController.Health;
				float num2 = Mathf.Abs(num - this.m_health);
				if (num2 > 0f && base.isActiveAndEnabled)
				{
					GameObject obj = GameUtils.InstantiateUIControllerOnScalingHUDCanvas(this.m_removePointsFloatingTextPrefab);
					RectTransform rectTransform = (RectTransform)base.transform;
					RectTransformExtension rectTransformExtension = obj.RequireComponent<RectTransformExtension>();
					Vector2 anchorOffset = this.m_floatingTextOffset.MultipliedBy(rectTransform.anchorMin + rectTransform.anchorMax);
					rectTransformExtension.AnchorOffset = anchorOffset;
					obj.RequireComponent<DisplayIntUIController>().Value = Mathf.RoundToInt(Mathf.Abs(num2));
					this.m_animator.SetTrigger(this.m_healthAlertId);
					if (this.m_pulseStart != 0f && num > 0f && num <= this.m_pulseStart)
					{
						this.m_animator.SetTrigger(this.m_healthPulseId);
					}
					else if (num > 0f)
					{
						this.m_animator.SetTrigger(this.m_healthAlertId);
					}
				}
				this.m_healthBar.Value = num / (float)this.m_flowController.MaxHealth;
				this.m_health = num;
			}
		}

		// Token: 0x04001DC7 RID: 7623
		[SerializeField]
		[AssignResource("RemovePointsFloatingNumberUI", Editorbility.Editable)]
		private GameObject m_removePointsFloatingTextPrefab;

		// Token: 0x04001DC8 RID: 7624
		[SerializeField]
		private Vector2 m_floatingTextOffset = new Vector2(0.5f, 0.5f);

		// Token: 0x04001DC9 RID: 7625
		[SerializeField]
		private ProgressBarUI m_healthBar;

		// Token: 0x04001DCA RID: 7626
		[SerializeField]
		private Animator m_animator;

		// Token: 0x04001DCB RID: 7627
		[SerializeField]
		private float m_pulseStart = 0.5f;

		// Token: 0x04001DCC RID: 7628
		[FormerlySerializedAs("m_heartPulseTriggerString")]
		[SerializeField]
		private string m_healthAlertString = string.Empty;

		// Token: 0x04001DCD RID: 7629
		private int m_healthAlertId;

		// Token: 0x04001DCE RID: 7630
		[SerializeField]
		private string m_healthPulseString = string.Empty;

		// Token: 0x04001DCF RID: 7631
		private int m_healthPulseId;

		// Token: 0x04001DD0 RID: 7632
		private ClientHordeFlowController m_flowController;

		// Token: 0x04001DD1 RID: 7633
		private float m_health;
	}
}
