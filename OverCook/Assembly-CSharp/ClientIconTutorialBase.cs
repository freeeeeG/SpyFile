using System;
using System.Collections;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200077D RID: 1917
public abstract class ClientIconTutorialBase : ClientSynchroniserBase
{
	// Token: 0x06002508 RID: 9480 RVA: 0x000AEEC8 File Offset: 0x000AD2C8
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_iconTutorial = (IconTutorialBase)synchronisedObject;
		FlowControllerBase flowControllerBase = GameUtils.RequireManager<FlowControllerBase>();
		this.m_iClientFlowController = flowControllerBase.gameObject.RequireComponent<ClientFlowControllerBase>();
		this.m_iClientFlowController.RoundActivatedCallback += this.EnterRound;
		this.m_iClientFlowController.RoundDeactivatedCallback += this.ExitRound;
	}

	// Token: 0x06002509 RID: 9481 RVA: 0x000AEF30 File Offset: 0x000AD330
	public override void UpdateSynchronising()
	{
		if (this.m_tutorialActive)
		{
			this.OnTutorialUpdate();
			for (int i = 0; i < this.m_icons.Length; i++)
			{
				this.m_icons[i].UpdateSynchronising();
			}
		}
	}

	// Token: 0x0600250A RID: 9482 RVA: 0x000AEF74 File Offset: 0x000AD374
	protected virtual void OnStartTutorial()
	{
	}

	// Token: 0x0600250B RID: 9483 RVA: 0x000AEF76 File Offset: 0x000AD376
	protected virtual void OnStopTutorial()
	{
	}

	// Token: 0x0600250C RID: 9484 RVA: 0x000AEF78 File Offset: 0x000AD378
	protected void DisableIcons()
	{
		for (int i = 0; i < this.m_icons.Length; i++)
		{
			UnityEngine.Object.Destroy(this.m_icons[i].Icon.gameObject);
		}
		this.m_icons = new ClientIconTutorialBase.IconData[0];
	}

	// Token: 0x0600250D RID: 9485 RVA: 0x000AEFC1 File Offset: 0x000AD3C1
	private void EnterRound()
	{
		if (!this.m_completed)
		{
			this.m_tutorialActive = true;
			this.OnStartTutorial();
		}
	}

	// Token: 0x0600250E RID: 9486 RVA: 0x000AEFDB File Offset: 0x000AD3DB
	private void ExitRound()
	{
		if (this.m_tutorialActive)
		{
			this.DisableIcons();
			if (this.m_activeTutorialPanel != null)
			{
				UnityEngine.Object.Destroy(this.m_activeTutorialPanel);
			}
			this.m_tutorialActive = false;
			this.OnStopTutorial();
		}
	}

	// Token: 0x0600250F RID: 9487 RVA: 0x000AF017 File Offset: 0x000AD417
	protected void CompleteTutorial()
	{
		this.ExitRound();
		this.m_completed = true;
	}

	// Token: 0x06002510 RID: 9488 RVA: 0x000AF026 File Offset: 0x000AD426
	protected virtual void OnTutorialUpdate()
	{
	}

	// Token: 0x06002511 RID: 9489 RVA: 0x000AF028 File Offset: 0x000AD428
	protected IEnumerator RunTutorialPanel(GameObject _uiPrefab)
	{
		ILogicalButton iLogicalButton = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UISelect, PlayerInputLookup.Player.One);
		float t = 0f;
		Generic<bool> testSelect = delegate()
		{
			t += Time.unscaledDeltaTime;
			if (t > 1f)
			{
				return iLogicalButton.JustPressed();
			}
			iLogicalButton.ClaimPressEvent();
			return false;
		};
		IEnumerator panelRoutine = this.RunPanel(_uiPrefab, testSelect);
		while (panelRoutine.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002512 RID: 9490 RVA: 0x000AF04C File Offset: 0x000AD44C
	protected IEnumerator RunPanel(GameObject _uiPrefab, Generic<bool> _waitForEnd)
	{
		while (this.m_activeTutorialPanel != null)
		{
			yield return null;
		}
		TimeManager timeManager = GameUtils.RequestManager<TimeManager>();
		timeManager.SetPaused(TimeManager.PauseLayer.Main, true, this);
		this.m_activeTutorialPanel = GameUtils.InstantiateUIController(_uiPrefab, "UICanvas");
		while (!_waitForEnd())
		{
			yield return null;
		}
		UnityEngine.Object.Destroy(this.m_activeTutorialPanel);
		timeManager.SetPaused(TimeManager.PauseLayer.Main, false, this);
		yield break;
	}

	// Token: 0x04001C96 RID: 7318
	private IconTutorialBase m_iconTutorial;

	// Token: 0x04001C97 RID: 7319
	protected IFlowController m_iClientFlowController;

	// Token: 0x04001C98 RID: 7320
	protected ClientIconTutorialBase.IconData[] m_icons = new ClientIconTutorialBase.IconData[0];

	// Token: 0x04001C99 RID: 7321
	private bool m_tutorialActive;

	// Token: 0x04001C9A RID: 7322
	private bool m_completed;

	// Token: 0x04001C9B RID: 7323
	private GameObject m_activeTutorialPanel;

	// Token: 0x0200077E RID: 1918
	public class IconData
	{
		// Token: 0x06002513 RID: 9491 RVA: 0x000AF078 File Offset: 0x000AD478
		public IconData(IconTutorialBase _tutorial, Sprite _sprite, Transform _parent, ClientIconTutorialBase.ActiveQuery _callback)
		{
			this.m_tutorialBase = _tutorial;
			this.ActiveCallback = _callback;
			GameObject obj = GameUtils.InstantiateHoverIconUIController(_tutorial.m_iconPrefab, _parent, "HoverIconCanvas", default(Vector3));
			this.Icon = obj.RequireComponent<HoverIconUIController>();
			GameObject gameObject = this.Icon.transform.Find("Icon").gameObject;
			Image image = gameObject.RequireComponent<Image>();
			image.sprite = _sprite;
			this.m_active = false;
			this.Icon.gameObject.SetActive(this.m_active);
		}

		// Token: 0x06002514 RID: 9492 RVA: 0x000AF108 File Offset: 0x000AD508
		public void UpdateSynchronising()
		{
			Transform followTransform = this.Icon.GetFollowTransform();
			Transform y = followTransform;
			bool flag = this.ActiveCallback(ref followTransform, this.Icon);
			if (followTransform != y)
			{
				this.Icon.SetFollowTransform(followTransform);
			}
			if (this.m_active != flag)
			{
				this.Icon.gameObject.SetActive(flag);
				this.m_active = flag;
				if (flag)
				{
					GameUtils.InstantiateHoverIconUIController(this.m_tutorialBase.m_attentionDrawer, this.Icon.GetFollowTransform(), "HoverIconCanvas", default(Vector3));
				}
			}
		}

		// Token: 0x04001C9C RID: 7324
		public ClientIconTutorialBase.ActiveQuery ActiveCallback;

		// Token: 0x04001C9D RID: 7325
		public HoverIconUIController Icon;

		// Token: 0x04001C9E RID: 7326
		private bool m_active;

		// Token: 0x04001C9F RID: 7327
		private IconTutorialBase m_tutorialBase;
	}

	// Token: 0x0200077F RID: 1919
	// (Invoke) Token: 0x06002516 RID: 9494
	public delegate bool ActiveQuery(ref Transform _followParent, HoverIconUIController _controller);
}
