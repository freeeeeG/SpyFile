using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200037D RID: 893
public class ClientBackpackCosmeticDecisions : ClientSynchroniserBase
{
	// Token: 0x060010EF RID: 4335 RVA: 0x00061240 File Offset: 0x0005F640
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_backpackCosmeticDecisions = (BackpackCosmeticDecisions)synchronisedObject;
		this.m_animator = base.gameObject.RequestComponentRecursive<Animator>();
		this.m_attachment = base.gameObject.RequireInterface<IClientAttachment>();
		this.m_attachment.RegisterAttachChangedCallback(new AttachChangedCallback(this.OnAttachmentChanged));
		this.m_itemSpawner = base.gameObject.RequireComponent<ClientPickupItemSpawner>();
		this.m_interactHoverIcon = base.gameObject.RequireComponent<ButtonHoverIcon>();
		this.m_interactHoverIcon.SetVisibility(true);
		this.m_interactHoverIcon.HoverIconController.SetFollowTransform(this.m_backpackCosmeticDecisions.m_hoverIconTarget);
		this.m_contentsHoverIcon = GameUtils.InstantiateHoverIconUIController(this.m_backpackCosmeticDecisions.m_contentsHoverIconPrefab, this.m_backpackCosmeticDecisions.m_hoverIconTarget, "HoverIconCanvas", this.m_backpackCosmeticDecisions.m_offset);
		this.m_contentsHoverIcon.SetActive(false);
		Image image = this.m_contentsHoverIcon.RequireChild("Icon").RequireComponent<Image>();
		GameObject itemPrefab = this.m_itemSpawner.GetItemPrefab();
		WorkableItem workableItem = itemPrefab.RequestComponent<WorkableItem>();
		if (workableItem != null)
		{
			GameObject nextPrefab = workableItem.GetNextPrefab();
			ISpawnableItem spawnableItem = nextPrefab.RequireInterface<ISpawnableItem>();
			image.sprite = spawnableItem.GetUIIcon();
		}
		else
		{
			ISpawnableItem spawnableItem2 = itemPrefab.RequireInterface<ISpawnableItem>();
			image.sprite = spawnableItem2.GetUIIcon();
		}
	}

	// Token: 0x060010F0 RID: 4336 RVA: 0x0006138C File Offset: 0x0005F78C
	private void OnAttachmentChanged(IParentable _parentable)
	{
		if (_parentable as MonoBehaviour != null)
		{
			this.m_interactHoverIcon.SetVisibility(false);
			this.m_contentsHoverIcon.SetActive(true);
			GameUtils.TriggerAudio(GameOneShotAudioTag.DLC_05_Bag_Pickup, base.gameObject.layer);
		}
		else
		{
			this.m_interactHoverIcon.SetVisibility(true);
			this.m_contentsHoverIcon.SetActive(false);
		}
	}

	// Token: 0x060010F1 RID: 4337 RVA: 0x000613F5 File Offset: 0x0005F7F5
	public void OnPickupItem()
	{
		this.m_animator.SetTrigger(ClientBackpackCosmeticDecisions.m_iOpen);
		GameUtils.TriggerAudio(GameOneShotAudioTag.DLC_05_Item_Collect, base.gameObject.layer);
	}

	// Token: 0x060010F2 RID: 4338 RVA: 0x0006141D File Offset: 0x0005F81D
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_attachment != null)
		{
			this.m_attachment.UnregisterAttachChangedCallback(new AttachChangedCallback(this.OnAttachmentChanged));
		}
	}

	// Token: 0x060010F3 RID: 4339 RVA: 0x00061448 File Offset: 0x0005F848
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_contentsHoverIcon != null)
		{
			this.m_contentsHoverIcon.SetActive(false);
		}
		if (this.m_interactHoverIcon != null)
		{
			this.m_interactHoverIcon.enabled = false;
		}
	}

	// Token: 0x060010F4 RID: 4340 RVA: 0x00061495 File Offset: 0x0005F895
	protected override void OnEnable()
	{
		base.OnEnable();
		if (this.m_interactHoverIcon != null)
		{
			this.m_interactHoverIcon.enabled = true;
		}
	}

	// Token: 0x04000D13 RID: 3347
	private BackpackCosmeticDecisions m_backpackCosmeticDecisions;

	// Token: 0x04000D14 RID: 3348
	private static readonly int m_iOpen = Animator.StringToHash("Open");

	// Token: 0x04000D15 RID: 3349
	private Animator m_animator;

	// Token: 0x04000D16 RID: 3350
	private IClientAttachment m_attachment;

	// Token: 0x04000D17 RID: 3351
	private ClientPickupItemSpawner m_itemSpawner;

	// Token: 0x04000D18 RID: 3352
	private ButtonHoverIcon m_interactHoverIcon;

	// Token: 0x04000D19 RID: 3353
	private GameObject m_contentsHoverIcon;
}
