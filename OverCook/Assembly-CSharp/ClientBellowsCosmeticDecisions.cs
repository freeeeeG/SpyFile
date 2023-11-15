using System;
using System.Collections;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000383 RID: 899
public class ClientBellowsCosmeticDecisions : ClientSynchroniserBase
{
	// Token: 0x06001103 RID: 4355 RVA: 0x00061794 File Offset: 0x0005FB94
	public override void StartSynchronising(Component _synchronisedObject)
	{
		base.StartSynchronising(_synchronisedObject);
		this.m_cosmetics = (BellowsCosmeticDecisions)_synchronisedObject;
		this.m_bellowsUsedParam = Animator.StringToHash(this.m_cosmetics.m_animParams.Used);
		this.m_InUseParam = Animator.StringToHash(this.m_cosmetics.m_animParams.InUse);
		this.m_stopParam = Animator.StringToHash(this.m_cosmetics.m_animParams.Stop);
		this.AddAnimator();
		this.m_attachment = base.gameObject.RequireInterface<IClientAttachment>();
		this.m_attachment.RegisterAttachChangedCallback(new AttachChangedCallback(this.OnAttachmentChanged));
		this.m_bellows = base.gameObject.RequireComponent<ClientBellowsSpray>();
		this.m_bellows.RegisterOnSpray(new CallbackVoid(this.OnSpray));
	}

	// Token: 0x06001104 RID: 4356 RVA: 0x0006185B File Offset: 0x0005FC5B
	private void AddAnimator()
	{
		this.m_animator = this.m_cosmetics.m_animTransform.gameObject.AddComponent<Animator>();
		this.m_animator.runtimeAnimatorController = this.m_cosmetics.m_animController;
	}

	// Token: 0x06001105 RID: 4357 RVA: 0x00061890 File Offset: 0x0005FC90
	private IEnumerator ToggleAnimatorRoutine(bool _isActive)
	{
		if (_isActive)
		{
			if (this.m_animator != null)
			{
				this.m_animator.ResetTrigger(this.m_stopParam);
				this.m_animator.enabled = true;
			}
		}
		else if (this.m_animator != null)
		{
			this.m_animator.SetTrigger(this.m_stopParam);
			while (this.m_animator.GetBool(this.m_InUseParam))
			{
				yield return null;
			}
			yield return null;
			this.m_animator.enabled = false;
		}
		yield break;
	}

	// Token: 0x06001106 RID: 4358 RVA: 0x000618B2 File Offset: 0x0005FCB2
	private void OnSpray()
	{
		if (this.m_animator != null)
		{
			this.m_animator.SetTrigger(this.m_bellowsUsedParam);
			GameUtils.TriggerAudio(GameOneShotAudioTag.DLC_02_Bellow, base.gameObject.layer);
		}
	}

	// Token: 0x06001107 RID: 4359 RVA: 0x000618EC File Offset: 0x0005FCEC
	private void OnAttachmentChanged(IParentable _parentable)
	{
		bool isActive = false;
		if (_parentable != null && _parentable is PlayerAttachmentCarrier)
		{
			isActive = true;
		}
		this.m_animToggleRoutine = this.ToggleAnimatorRoutine(isActive);
	}

	// Token: 0x06001108 RID: 4360 RVA: 0x0006191B File Offset: 0x0005FD1B
	public override void UpdateSynchronising()
	{
		base.UpdateSynchronising();
		if (this.m_animToggleRoutine != null && !this.m_animToggleRoutine.MoveNext())
		{
			this.m_animToggleRoutine = null;
		}
	}

	// Token: 0x06001109 RID: 4361 RVA: 0x00061948 File Offset: 0x0005FD48
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_attachment != null)
		{
			this.m_attachment.UnregisterAttachChangedCallback(new AttachChangedCallback(this.OnAttachmentChanged));
		}
		if (this.m_bellows != null)
		{
			this.m_bellows.UnregisterOnSpray(new CallbackVoid(this.OnSpray));
		}
	}

	// Token: 0x04000D2A RID: 3370
	private int m_bellowsUsedParam = -1;

	// Token: 0x04000D2B RID: 3371
	private int m_InUseParam = -1;

	// Token: 0x04000D2C RID: 3372
	private int m_stopParam = -1;

	// Token: 0x04000D2D RID: 3373
	private BellowsCosmeticDecisions m_cosmetics;

	// Token: 0x04000D2E RID: 3374
	private Animator m_animator;

	// Token: 0x04000D2F RID: 3375
	private IClientAttachment m_attachment;

	// Token: 0x04000D30 RID: 3376
	private ClientBellowsSpray m_bellows;

	// Token: 0x04000D31 RID: 3377
	private IEnumerator m_animToggleRoutine;
}
