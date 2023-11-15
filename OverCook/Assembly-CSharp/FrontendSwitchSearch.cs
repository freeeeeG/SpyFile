using System;
using System.Collections.Generic;
using Team17.Online;
using UnityEngine;

// Token: 0x02000AC6 RID: 2758
public class FrontendSwitchSearch : FrontendMenuBehaviour
{
	// Token: 0x06003774 RID: 14196 RVA: 0x0010560F File Offset: 0x00103A0F
	public void SetResults(SearchTask.SearchResultData results)
	{
		this.m_results = results;
	}

	// Token: 0x06003775 RID: 14197 RVA: 0x00105618 File Offset: 0x00103A18
	public override bool Show(GamepadUser currentGamer, BaseMenuBehaviour parent, GameObject invoker, bool hideInvoker = true)
	{
		if (!base.Show(currentGamer, parent, invoker, hideInvoker))
		{
			return false;
		}
		if (T17FrontendFlow.Instance != null)
		{
			T17FrontendFlow.Instance.BlockFocusKitchen = true;
		}
		if (this.m_results != null && this.m_results.m_AvailableSessions != null && this.m_results.m_AvailableSessions.Count > 0)
		{
			if (this.m_noEntriesMessage != null)
			{
				this.m_noEntriesMessage.SetActive(false);
			}
			this.Open(currentGamer);
			this.m_scrollView.Show(currentGamer, parent, invoker, hideInvoker);
		}
		else
		{
			if (this.m_noEntriesMessage != null)
			{
				this.m_noEntriesMessage.SetActive(true);
			}
			if (this.m_CachedEventSystem != null && this.m_BorderSelectables.selectOnRight != null)
			{
				this.m_CachedEventSystem.SetSelectedGameObject(this.m_BorderSelectables.selectOnRight.gameObject);
			}
		}
		return true;
	}

	// Token: 0x06003776 RID: 14198 RVA: 0x00105720 File Offset: 0x00103B20
	public override bool Hide(bool restoreInvokerState = true, bool isTabSwitch = false)
	{
		this.Clear();
		if (T17FrontendFlow.Instance != null)
		{
			T17FrontendFlow.Instance.BlockFocusKitchen = false;
		}
		if (this.m_noEntriesMessage != null)
		{
			this.m_noEntriesMessage.SetActive(false);
		}
		this.m_scrollView.Hide(true, false);
		return base.Hide(restoreInvokerState, isTabSwitch);
	}

	// Token: 0x06003777 RID: 14199 RVA: 0x00105781 File Offset: 0x00103B81
	public void Clear()
	{
		this.m_scrollView.ClearContents();
		this.m_entrys.Clear();
	}

	// Token: 0x06003778 RID: 14200 RVA: 0x0010579C File Offset: 0x00103B9C
	public void Open(GamepadUser currentGamer)
	{
		if (this.m_results != null && this.m_results.m_AvailableSessions != null)
		{
			List<OnlineMultiplayerSessionEnumeratedRoom> availableSessions = this.m_results.m_AvailableSessions;
			for (int i = 0; i < availableSessions.Count; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_entryPrefab, this.m_containerObject.transform);
				this.m_scrollView.AddNewObject(gameObject);
				if (gameObject != null && gameObject.transform != null)
				{
					this.m_entrys.Add(gameObject);
					FrontendSearchJoin component = gameObject.GetComponent<FrontendSearchJoin>();
					if (component != null)
					{
						component.SetLocalUser(currentGamer);
						component.SetSelectedSession(availableSessions[i]);
						component.SetSessionName(availableSessions[i].GetHostName());
						FrontendSearchJoin frontendSearchJoin = component;
						frontendSearchJoin.onComplete = (FrontendSearchJoin.onCompleteDelegate)Delegate.Combine(frontendSearchJoin.onComplete, new FrontendSearchJoin.onCompleteDelegate(this.Close));
					}
				}
			}
		}
	}

	// Token: 0x06003779 RID: 14201 RVA: 0x00105890 File Offset: 0x00103C90
	public void OnCancel()
	{
		this.Close();
		T17FrontendFlow instance = T17FrontendFlow.Instance;
		if (instance != null && instance.m_PlayerLobby != null)
		{
			instance.m_PlayerLobby.OnSearchCancelled();
		}
	}

	// Token: 0x0600377A RID: 14202 RVA: 0x001058D1 File Offset: 0x00103CD1
	public override void Close()
	{
		base.Close();
		if (T17FrontendFlow.Instance != null)
		{
			T17FrontendFlow.Instance.FocusOnMultiplayerKitchen(true);
		}
	}

	// Token: 0x04002C83 RID: 11395
	[SerializeField]
	private GameObject m_containerObject;

	// Token: 0x04002C84 RID: 11396
	[SerializeField]
	private GameObject m_entryPrefab;

	// Token: 0x04002C85 RID: 11397
	[SerializeField]
	private GameObject m_noEntriesMessage;

	// Token: 0x04002C86 RID: 11398
	[SerializeField]
	private T17ScrollView m_scrollView;

	// Token: 0x04002C87 RID: 11399
	private List<GameObject> m_entrys = new List<GameObject>();

	// Token: 0x04002C88 RID: 11400
	private SearchTask.SearchResultData m_results;
}
