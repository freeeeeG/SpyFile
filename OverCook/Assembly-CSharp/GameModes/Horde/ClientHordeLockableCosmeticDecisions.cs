using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;
using UnityEngine.UI;

namespace GameModes.Horde
{
	// Token: 0x020007B1 RID: 1969
	public class ClientHordeLockableCosmeticDecisions : ClientSynchroniserBase, IAnticipateInteractionNotifications
	{
		// Token: 0x060025C5 RID: 9669 RVA: 0x000B277C File Offset: 0x000B0B7C
		public override void StartSynchronising(Component synchronisedObject)
		{
			base.StartSynchronising(synchronisedObject);
			this.m_cosmeticDecisions = (HordeLockableCosmeticDecisions)synchronisedObject;
			this.m_lockable = base.gameObject.RequireComponent<ClientHordeLockable>();
			this.m_lockable.RegisterOnLock(this, new GenericVoid<ClientHordeLockable>(this.OnLock));
			this.m_lockable.RegisterOnUnlock(this, new GenericVoid<ClientHordeLockable>(this.OnUnlock));
			this.m_hoverIcon = GameUtils.InstantiateHoverIconUIController<HoverIconUIController>(out this.m_hoverIconController, this.m_cosmeticDecisions.m_hoverIconPrefab, this.m_cosmeticDecisions.m_hoverIconTarget, "HoverIconCanvas", this.m_cosmeticDecisions.m_offset);
			Image image = this.m_hoverIcon.RequireChild("Icon").RequireComponent<Image>();
			image.sprite = this.m_cosmeticDecisions.m_hoverIcon;
			T17Text t17Text = this.m_hoverIcon.RequireComponentRecursive<T17Text>();
			t17Text.SetNonLocalizedText(this.m_lockable.UnlockCost.ToString());
			this.m_hoverIcon.SetActive(false);
			this.m_highlight = base.gameObject.RequestComponent<ClientAnticipateInteractionHighlight>();
		}

		// Token: 0x060025C6 RID: 9670 RVA: 0x000B2888 File Offset: 0x000B0C88
		private void OnLock(ClientHordeLockable lockable)
		{
			this.m_locked = true;
			this.m_hoverIcon.SetActive(true);
			this.m_cosmeticDecisions.m_animator.SetTrigger(this.m_cosmeticDecisions.m_lockAnimationId);
			if (this.m_highlight != null)
			{
				this.m_highlight.enabled = true;
			}
		}

		// Token: 0x060025C7 RID: 9671 RVA: 0x000B28E0 File Offset: 0x000B0CE0
		private void OnUnlock(ClientHordeLockable lockable)
		{
			this.m_locked = false;
			this.m_hoverIcon.SetActive(false);
			this.m_cosmeticDecisions.m_animator.SetTrigger(this.m_cosmeticDecisions.m_unlockAnimationId);
			if (this.m_highlight != null)
			{
				this.m_highlight.enabled = false;
			}
		}

		// Token: 0x060025C8 RID: 9672 RVA: 0x000B2938 File Offset: 0x000B0D38
		public void OnInteractionAnticipationStart(InteractionType type, GameObject player)
		{
			if (this.m_locked)
			{
				this.m_hoverIcon.SetActive(true);
			}
		}

		// Token: 0x060025C9 RID: 9673 RVA: 0x000B2951 File Offset: 0x000B0D51
		public void OnInteractionAnticipationEnded(InteractionType type, GameObject player)
		{
			if (this.m_locked)
			{
				this.m_hoverIcon.SetActive(false);
			}
		}

		// Token: 0x04001D84 RID: 7556
		private HordeLockableCosmeticDecisions m_cosmeticDecisions;

		// Token: 0x04001D85 RID: 7557
		private ClientHordeLockable m_lockable;

		// Token: 0x04001D86 RID: 7558
		private GameObject m_hoverIcon;

		// Token: 0x04001D87 RID: 7559
		private HoverIconUIController m_hoverIconController;

		// Token: 0x04001D88 RID: 7560
		private bool m_locked = true;

		// Token: 0x04001D89 RID: 7561
		public ClientAnticipateInteractionHighlight m_highlight;
	}
}
