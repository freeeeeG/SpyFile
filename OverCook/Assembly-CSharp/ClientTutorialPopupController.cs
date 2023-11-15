using System;
using System.Collections;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200078E RID: 1934
public class ClientTutorialPopupController : ClientSynchroniserBase
{
	// Token: 0x06002562 RID: 9570 RVA: 0x000B0F6A File Offset: 0x000AF36A
	public override EntityType GetEntityType()
	{
		return EntityType.TutorialPopup;
	}

	// Token: 0x06002563 RID: 9571 RVA: 0x000B0F6E File Offset: 0x000AF36E
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_controller = (TutorialPopupController)synchronisedObject;
	}

	// Token: 0x06002564 RID: 9572 RVA: 0x000B0F84 File Offset: 0x000AF384
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		TutorialDismissMessage tutorialDismissMessage = (TutorialDismissMessage)serialisable;
		if (tutorialDismissMessage != null)
		{
			this.m_dismissed = true;
		}
	}

	// Token: 0x06002565 RID: 9573 RVA: 0x000B0FA8 File Offset: 0x000AF3A8
	public IEnumerator ShowTutorial(TutorialPopup _data, Generic<IEnumerator, GameObject> _dismisser)
	{
		this.m_popup = _data.Spawn();
		this.m_popup.SetActive(false);
		if (this.m_hudCanvas == null)
		{
			this.m_hudCanvas = GameUtils.GetNamedCanvas("ScalingHUDCanvas").RequireComponent<Canvas>();
		}
		if (this.m_hoverIconCanvas == null)
		{
			this.m_hoverIconCanvas = GameUtils.GetNamedCanvas("HoverIconCanvas").RequireComponent<Canvas>();
		}
		return this.RunTutorial(this.m_popup, _dismisser);
	}

	// Token: 0x06002566 RID: 9574 RVA: 0x000B1026 File Offset: 0x000AF426
	public void Shutdown()
	{
		if (this.m_popup != null)
		{
			UnityEngine.Object.Destroy(this.m_popup);
			this.m_popup = null;
		}
	}

	// Token: 0x06002567 RID: 9575 RVA: 0x000B104C File Offset: 0x000AF44C
	private IEnumerator RunTutorial(GameObject _popup, Generic<IEnumerator, GameObject> _dismisser)
	{
		if (_popup != null)
		{
			TimeManager timeManager = GameUtils.RequireManager<TimeManager>();
			timeManager.SetPaused(TimeManager.PauseLayer.Main, true, this);
			timeManager.SetPaused(TimeManager.PauseLayer.Camera, true, this);
			this.m_hudCanvas.enabled = false;
			this.m_hoverIconCanvas.enabled = false;
			_popup.SetActive(true);
			GameUtils.TriggerAudio(GameOneShotAudioTag.TutorialPopIn, base.gameObject.layer);
			this.m_dismissed = false;
			IEnumerator dismissRoutine = _dismisser(_popup);
			while (!this.m_dismissed)
			{
				if (ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
				{
					if (!dismissRoutine.MoveNext())
					{
						this.m_dismissed = true;
						this.m_controller.OnTutorialDismissed();
					}
					yield return null;
				}
				else
				{
					yield return null;
				}
			}
			_popup.SetActive(false);
			GameUtils.TriggerAudio(GameOneShotAudioTag.TutorialPopOut, base.gameObject.layer);
			this.m_hudCanvas.enabled = true;
			this.m_hoverIconCanvas.enabled = true;
			timeManager.SetPaused(TimeManager.PauseLayer.Camera, false, this);
			timeManager.SetPaused(TimeManager.PauseLayer.Main, false, this);
		}
		yield break;
	}

	// Token: 0x04001CF3 RID: 7411
	private TutorialPopupController m_controller;

	// Token: 0x04001CF4 RID: 7412
	private GameObject m_popup;

	// Token: 0x04001CF5 RID: 7413
	private Canvas m_hudCanvas;

	// Token: 0x04001CF6 RID: 7414
	private Canvas m_hoverIconCanvas;

	// Token: 0x04001CF7 RID: 7415
	private bool m_dismissed;
}
