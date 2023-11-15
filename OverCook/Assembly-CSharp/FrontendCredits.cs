using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000AB3 RID: 2739
public class FrontendCredits : FrontendMenuBehaviour
{
	// Token: 0x06003674 RID: 13940 RVA: 0x000FF480 File Offset: 0x000FD880
	protected override void SingleTimeInitialize()
	{
		base.SingleTimeInitialize();
		this.m_animator = this.m_rootObject.RequestComponentRecursive<Animator>();
		if (FrontendCredits.m_startId == -1)
		{
			FrontendCredits.m_startId = Animator.StringToHash(this.m_startTrigger);
		}
		if (FrontendCredits.m_resetId == -1)
		{
			FrontendCredits.m_resetId = Animator.StringToHash(this.m_resetTrigger);
		}
		if (FrontendCredits.m_finishedId == -1)
		{
			FrontendCredits.m_finishedId = Animator.StringToHash(this.m_finishedTrigger);
		}
		this.m_gamepadEngagementManager = GameUtils.RequireManager<GamepadEngagementManager>();
	}

	// Token: 0x06003675 RID: 13941 RVA: 0x000FF500 File Offset: 0x000FD900
	public override bool Show(GamepadUser currentGamer, BaseMenuBehaviour parent, GameObject invoker, bool hideInvoker = true)
	{
		if (!base.Show(currentGamer, parent, invoker, hideInvoker))
		{
			return false;
		}
		if (this.m_rootObject != null)
		{
			this.m_rootObject.SetActive(false);
		}
		this.m_creditsRoutine = this.RunCredits();
		this.m_pendingClose = false;
		if (T17FrontendFlow.Instance != null)
		{
			T17FrontendFlow.Instance.BlockFocusKitchen = true;
		}
		if (this.m_gamepadEngagementManager != null)
		{
			this.m_engagementSuppressor = this.m_gamepadEngagementManager.Suppressor.AddSuppressor(this);
		}
		return true;
	}

	// Token: 0x06003676 RID: 13942 RVA: 0x000FF594 File Offset: 0x000FD994
	public override bool Hide(bool restoreInvokerState = true, bool isTabSwitch = false)
	{
		if (!base.Hide(restoreInvokerState, isTabSwitch))
		{
			return false;
		}
		this.Shutdown();
		if (T17FrontendFlow.Instance != null)
		{
			T17FrontendFlow.Instance.BlockFocusKitchen = false;
		}
		if (this.m_engagementSuppressor != null)
		{
			this.m_engagementSuppressor.Release();
			this.m_engagementSuppressor = null;
		}
		return true;
	}

	// Token: 0x06003677 RID: 13943 RVA: 0x000FF5EF File Offset: 0x000FD9EF
	protected override void Update()
	{
		base.Update();
		if (this.m_creditsRoutine != null && !this.m_creditsRoutine.MoveNext())
		{
			this.m_creditsRoutine = null;
		}
	}

	// Token: 0x06003678 RID: 13944 RVA: 0x000FF619 File Offset: 0x000FDA19
	public override void Close()
	{
		this.m_pendingClose = true;
	}

	// Token: 0x06003679 RID: 13945 RVA: 0x000FF624 File Offset: 0x000FDA24
	private IEnumerator RunCredits()
	{
		ScreenTransitionManager transitionManager = GameUtils.RequireManager<ScreenTransitionManager>();
		bool transitionFinished = false;
		transitionManager.StartTransitionUp(delegate
		{
			transitionManager.StartTransitionDown(delegate
			{
				transitionFinished = true;
			});
			if (this.m_rootObject != null)
			{
				this.m_rootObject.SetActive(true);
			}
		});
		while (!transitionFinished)
		{
			yield return null;
		}
		if (!this.m_pendingClose)
		{
			if (this.m_animator != null)
			{
				this.m_animator.SetTrigger(FrontendCredits.m_startId);
				this.m_animator.Update(0f);
			}
			while (!this.m_pendingClose)
			{
				if (this.m_animator != null && this.m_animator.GetBool(FrontendCredits.m_finishedId))
				{
					break;
				}
				yield return null;
			}
		}
		this.m_pendingClose = false;
		transitionManager.StartTransitionUp(delegate
		{
			this.Shutdown();
			this.<Close>__BaseCallProxy0();
			transitionManager.StartTransitionDown(null);
		});
		yield break;
	}

	// Token: 0x0600367A RID: 13946 RVA: 0x000FF640 File Offset: 0x000FDA40
	private void Shutdown()
	{
		if (this.m_animator != null && this.m_animator.IsActive())
		{
			this.m_animator.SetTrigger(FrontendCredits.m_resetId);
			this.m_animator.Update(0f);
		}
		if (this.m_rootObject != null)
		{
			this.m_rootObject.SetActive(false);
		}
		this.m_creditsRoutine = null;
	}

	// Token: 0x04002BC3 RID: 11203
	[Header("Credits")]
	[SerializeField]
	[AssignChild("Credits", Editorbility.NonEditable)]
	private GameObject m_rootObject;

	// Token: 0x04002BC4 RID: 11204
	[SerializeField]
	private string m_startTrigger = "Start";

	// Token: 0x04002BC5 RID: 11205
	[SerializeField]
	private string m_resetTrigger = "Reset";

	// Token: 0x04002BC6 RID: 11206
	[SerializeField]
	private string m_finishedTrigger = "Finished";

	// Token: 0x04002BC7 RID: 11207
	private static int m_startId = -1;

	// Token: 0x04002BC8 RID: 11208
	private static int m_resetId = -1;

	// Token: 0x04002BC9 RID: 11209
	private static int m_finishedId = -1;

	// Token: 0x04002BCA RID: 11210
	private Animator m_animator;

	// Token: 0x04002BCB RID: 11211
	private IEnumerator m_creditsRoutine;

	// Token: 0x04002BCC RID: 11212
	private bool m_pendingClose;

	// Token: 0x04002BCD RID: 11213
	private GamepadEngagementManager m_gamepadEngagementManager;

	// Token: 0x04002BCE RID: 11214
	private Suppressor m_engagementSuppressor;
}
