using System;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

namespace GameModes.Horde
{
	// Token: 0x020007C2 RID: 1986
	public class MoneyUIController : UIControllerAndContainer
	{
		// Token: 0x06002609 RID: 9737 RVA: 0x000B4A1A File Offset: 0x000B2E1A
		protected void Awake()
		{
			this.m_moneyText.SetNonLocalizedText("0");
			Mailbox.Client.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		}

		// Token: 0x0600260A RID: 9738 RVA: 0x000B4A44 File Offset: 0x000B2E44
		protected void OnDestroy()
		{
			Mailbox.Client.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		}

		// Token: 0x0600260B RID: 9739 RVA: 0x000B4A60 File Offset: 0x000B2E60
		private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
		{
			GameStateMessage gameStateMessage = (GameStateMessage)message;
			if (gameStateMessage.m_State == GameState.StartEntities)
			{
				FlowControllerBase flowControllerBase = GameUtils.RequireManager<FlowControllerBase>();
				this.m_flowController = flowControllerBase.gameObject.RequireComponent<ClientHordeFlowController>();
			}
		}

		// Token: 0x0600260C RID: 9740 RVA: 0x000B4A98 File Offset: 0x000B2E98
		private void Update()
		{
			if (this.m_flowController != null)
			{
				int money = this.m_flowController.Money;
				int num = money - this.m_money;
				if (num != 0 && base.isActiveAndEnabled)
				{
					this.m_animator.SetTrigger(this.m_coinSpinTriggerId);
					GameObject obj = GameUtils.InstantiateUIControllerOnScalingHUDCanvas((num <= 0) ? this.m_removeMoneyFloatingTextPrefab : this.m_addMoneyFloatingTextPrefab);
					RectTransform rectTransform = (RectTransform)base.transform;
					RectTransformExtension rectTransformExtension = obj.RequireComponent<RectTransformExtension>();
					Vector2 anchorOffset = this.m_floatingTextOffset.MultipliedBy(rectTransform.anchorMin + rectTransform.anchorMax);
					rectTransformExtension.AnchorOffset = anchorOffset;
					obj.RequireComponent<DisplayIntUIController>().Value = Mathf.Abs(num);
					this.m_moneyText.SetNonLocalizedText(money.ToString());
				}
				this.m_money = money;
			}
		}

		// Token: 0x04001E06 RID: 7686
		[SerializeField]
		private Animator m_animator;

		// Token: 0x04001E07 RID: 7687
		[Header("Floating Text")]
		[SerializeField]
		[AssignResource("AddPointsFloatingNumberUI", Editorbility.Editable)]
		private GameObject m_addMoneyFloatingTextPrefab;

		// Token: 0x04001E08 RID: 7688
		[SerializeField]
		[AssignResource("RemovePointsFloatingNumberUI", Editorbility.Editable)]
		private GameObject m_removeMoneyFloatingTextPrefab;

		// Token: 0x04001E09 RID: 7689
		[SerializeField]
		private Vector2 m_floatingTextOffset = new Vector2(0.5f, 0.5f);

		// Token: 0x04001E0A RID: 7690
		[SerializeField]
		private T17Text m_moneyText;

		// Token: 0x04001E0B RID: 7691
		[SerializeField]
		private string m_coinSpinTriggerString = string.Empty;

		// Token: 0x04001E0C RID: 7692
		private int m_coinSpinTriggerId;

		// Token: 0x04001E0D RID: 7693
		private ClientHordeFlowController m_flowController;

		// Token: 0x04001E0E RID: 7694
		private int m_money;
	}
}
