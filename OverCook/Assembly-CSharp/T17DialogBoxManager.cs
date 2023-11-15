using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B14 RID: 2836
public class T17DialogBoxManager : MonoBehaviour
{
	// Token: 0x06003955 RID: 14677 RVA: 0x00110144 File Offset: 0x0010E544
	private void Awake()
	{
		if (T17DialogBoxManager.Instance != null)
		{
			UnityEngine.Object.Destroy(this);
		}
		else
		{
			T17DialogBoxManager.Instance = this;
			if (this.m_DialogBoxPrefab != null)
			{
				this.m_DialogBoxPool = new List<T17DialogBox>();
				this.m_Bindings = new Dictionary<GamepadUser, List<T17DialogBox>>();
				this.m_bindingKeys = new FastList<GamepadUser>();
				this.m_GlobalDialogs = new List<T17DialogBox>();
				for (int i = 0; i < 15; i++)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_DialogBoxPrefab);
					gameObject.transform.SetParent(this.m_GlobalDialogBoxCanvas.transform, false);
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localPosition = Vector3.zero;
					T17DialogBox component = gameObject.GetComponent<T17DialogBox>();
					if (component != null)
					{
						gameObject.SetActive(false);
						this.m_DialogBoxPool.Add(component);
					}
				}
			}
			this.m_EventSystem = GameObject.Find("EventSystems");
		}
	}

	// Token: 0x06003956 RID: 14678 RVA: 0x00110240 File Offset: 0x0010E640
	private void OnDestroy()
	{
		if (T17DialogBoxManager.Instance == this)
		{
			T17DialogBoxManager.Instance = null;
		}
	}

	// Token: 0x06003957 RID: 14679 RVA: 0x00110258 File Offset: 0x0010E658
	public static T17DialogBox GetDialog(bool forSingleUser)
	{
		if (T17DialogBoxManager.Instance == null)
		{
			return null;
		}
		forSingleUser = false;
		int i = 0;
		while (i < 15)
		{
			T17DialogBox t17DialogBox = T17DialogBoxManager.Instance.m_DialogBoxPool[i];
			if (!t17DialogBox.IsActive)
			{
				GameObject gameObject = null;
				PlayerManager playerManager = GameUtils.RequireManager<PlayerManager>();
				GamepadUser gamepadUser = playerManager.GetUser(EngagementSlot.One);
				if (gamepadUser == null)
				{
					gamepadUser = playerManager.GetCachedPrimaryUser();
				}
				if (gamepadUser != null)
				{
					if (forSingleUser)
					{
						if (T17DialogBoxManager.Instance.m_Bindings.ContainsKey(gamepadUser))
						{
							T17DialogBoxManager.Instance.m_Bindings[gamepadUser].Add(t17DialogBox);
						}
						else
						{
							T17DialogBoxManager.Instance.m_Bindings.Add(gamepadUser, new List<T17DialogBox>
							{
								t17DialogBox
							});
							T17DialogBoxManager.Instance.m_bindingKeys.Add(gamepadUser);
						}
					}
					else
					{
						T17EventSystemsManager.Instance.DisableAllEventSystemsExceptFor(gamepadUser);
						gameObject = T17DialogBoxManager.Instance.m_GlobalDialogBoxCanvas.gameObject;
						T17DialogBoxManager.Instance.m_GlobalDialogs.Add(t17DialogBox);
						T17DialogBoxManager.Instance.m_bHasGlobalDiags = true;
					}
					gameObject.SetActive(true);
					t17DialogBox.transform.SetParent(gameObject.transform, false);
					t17DialogBox.transform.localScale = Vector3.one;
					t17DialogBox.transform.localPosition = Vector3.zero;
					t17DialogBox.ReparentToOnHide(T17DialogBoxManager.Instance.m_GlobalDialogBoxCanvas.transform);
					t17DialogBox.SetEventSystem(T17EventSystemsManager.Instance.GetEventSystemForGamepadUser(gamepadUser));
					return t17DialogBox;
				}
				return null;
			}
			else
			{
				i++;
			}
		}
		return null;
	}

	// Token: 0x06003958 RID: 14680 RVA: 0x001103E8 File Offset: 0x0010E7E8
	public static void RequestDialogShow(T17DialogBox dialog, T17DialogBoxManager.AllowedToShowEvent callOnAllowed)
	{
		if (T17DialogBoxManager.Instance == null)
		{
			return;
		}
		if (T17DialogBoxManager.Instance.m_EventSystem != null)
		{
			T17DialogBoxManager.Instance.m_EventSystem.SetActive(true);
		}
		T17DialogBoxManager.DialogQueueElement dialogQueueElement = new T17DialogBoxManager.DialogQueueElement();
		dialogQueueElement.Dialog = dialog;
		dialogQueueElement.ShowEvent = callOnAllowed;
		if (!T17DialogBoxManager.Instance.m_DialogBoxQueue.Exists((T17DialogBoxManager.DialogQueueElement q) => q.Dialog == dialog))
		{
			T17DialogBoxManager.Instance.m_DialogBoxQueue.Add(dialogQueueElement);
		}
	}

	// Token: 0x06003959 RID: 14681 RVA: 0x00110481 File Offset: 0x0010E881
	public static bool HasGlobalDialogs()
	{
		return !(T17DialogBoxManager.Instance == null) && T17DialogBoxManager.Instance.m_bHasGlobalDiags;
	}

	// Token: 0x0600395A RID: 14682 RVA: 0x001104A0 File Offset: 0x0010E8A0
	public static bool HasDialogsForGamer(GamepadUser user)
	{
		return !(user == null) && !(T17DialogBoxManager.Instance == null) && (T17DialogBoxManager.Instance.m_bHasGlobalDiags || (T17DialogBoxManager.Instance.m_Bindings.ContainsKey(user) && T17DialogBoxManager.Instance.m_Bindings[user].Count > 0));
	}

	// Token: 0x0600395B RID: 14683 RVA: 0x00110514 File Offset: 0x0010E914
	public static bool HasAnyOpenDialogs()
	{
		if (T17DialogBoxManager.Instance == null)
		{
			return false;
		}
		for (int i = 0; i < 15; i++)
		{
			if (T17DialogBoxManager.Instance.m_DialogBoxPool[i].IsActive)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600395C RID: 14684 RVA: 0x00110564 File Offset: 0x0010E964
	private void EnsureFocus()
	{
		if (T17DialogBoxManager.Instance.m_bHasGlobalDiags)
		{
			if (this.m_GlobalDialogs != null && this.m_GlobalDialogs.Count > 0)
			{
				T17DialogBox t17DialogBox = this.m_GlobalDialogs[this.m_GlobalDialogs.Count - 1];
				if (!t17DialogBox.HasFocus())
				{
					t17DialogBox.Focus();
					this.UpdateHoldsForFocus(t17DialogBox);
				}
			}
		}
		else
		{
			for (int i = 0; i < T17DialogBoxManager.Instance.m_bindingKeys.Count; i++)
			{
				GamepadUser key = T17DialogBoxManager.Instance.m_bindingKeys._items[i];
				List<T17DialogBox> list = T17DialogBoxManager.Instance.m_Bindings[key];
				if (list != null && list.Count > 0)
				{
					T17DialogBox t17DialogBox2 = list[list.Count - 1];
					if (!t17DialogBox2.HasFocus())
					{
						t17DialogBox2.Focus();
						this.UpdateHoldsForFocus(t17DialogBox2);
					}
				}
			}
		}
	}

	// Token: 0x0600395D RID: 14685 RVA: 0x00110654 File Offset: 0x0010EA54
	public void Update()
	{
		if (this.m_DialogBoxQueue.Count > 0)
		{
			T17DialogBoxManager.DialogQueueElement dialogQueueElement = this.m_DialogBoxQueue[0];
			this.m_DialogBoxQueue.RemoveAt(0);
			dialogQueueElement.ShowEvent();
			this.UpdateHoldsForFocus(dialogQueueElement.Dialog);
		}
		this.EnsureFocus();
	}

	// Token: 0x0600395E RID: 14686 RVA: 0x001106A8 File Offset: 0x0010EAA8
	private void UpdateHoldsForFocus(T17DialogBox _dialog)
	{
		if (!_dialog.gameObject.activeInHierarchy || !_dialog.IsActive)
		{
			return;
		}
		if (_dialog.m_EventSystemForGamer == null)
		{
			return;
		}
		T17EventSystem eventSystemForGamer = _dialog.m_EventSystemForGamer;
		if (eventSystemForGamer.AssignedGamepadUser == null)
		{
			return;
		}
		if (!_dialog.HasButtons())
		{
			GamepadUser assignedGamepadUser = eventSystemForGamer.AssignedGamepadUser;
			Suppressor suppressor;
			if (this.m_suppressors.TryGetValue(assignedGamepadUser.UID, out suppressor))
			{
				if (!suppressor.IsSuppressedBy(_dialog))
				{
					this.ReleaseHoldForUser(assignedGamepadUser);
					this.m_suppressors.Add(assignedGamepadUser.UID, eventSystemForGamer.Disable(_dialog));
				}
			}
			else
			{
				this.m_suppressors.Add(assignedGamepadUser.UID, eventSystemForGamer.Disable(_dialog));
			}
		}
		else
		{
			this.ReleaseHoldForUser(eventSystemForGamer.AssignedGamepadUser);
		}
	}

	// Token: 0x0600395F RID: 14687 RVA: 0x00110780 File Offset: 0x0010EB80
	private void ReleaseHoldForUser(GamepadUser _user)
	{
		if (this.m_suppressors.ContainsKey(_user.UID))
		{
			T17EventSystem eventSystemForGamepadUser = T17EventSystemsManager.Instance.GetEventSystemForGamepadUser(_user);
			if (eventSystemForGamepadUser != null)
			{
				eventSystemForGamepadUser.ReleaseSuppressor(this.m_suppressors[_user.UID]);
			}
			else
			{
				this.m_suppressors[_user.UID].Release();
			}
			this.m_suppressors.Remove(_user.UID);
		}
	}

	// Token: 0x06003960 RID: 14688 RVA: 0x00110800 File Offset: 0x0010EC00
	public static void ReleaseAll()
	{
		if (T17DialogBoxManager.Instance == null)
		{
			return;
		}
		for (int i = 0; i < 15; i++)
		{
			if (T17DialogBoxManager.Instance.m_DialogBoxPool[i].IsActive)
			{
				T17DialogBoxManager.Instance.m_DialogBoxPool[i].Hide();
			}
		}
		T17DialogBoxManager.Instance.m_DialogBoxQueue.Clear();
		T17DialogBoxManager.Instance.m_bHasGlobalDiags = false;
		T17DialogBoxManager.Instance.m_GlobalDialogs.Clear();
		T17DialogBoxManager.Instance.m_Bindings.Clear();
		T17DialogBoxManager.Instance.m_bindingKeys.Clear();
	}

	// Token: 0x06003961 RID: 14689 RVA: 0x001108A8 File Offset: 0x0010ECA8
	public static void ReleaseMe(T17DialogBox box)
	{
		int num = T17DialogBoxManager.Instance.m_DialogBoxQueue.Count - 1;
		for (int i = num; i >= 0; i--)
		{
			if (T17DialogBoxManager.Instance.m_DialogBoxQueue[i].Dialog == box)
			{
				T17DialogBoxManager.Instance.m_DialogBoxQueue.RemoveAt(i);
				break;
			}
		}
		GamepadUser assignedGamepadUser = box.m_EventSystemForGamer.AssignedGamepadUser;
		if (assignedGamepadUser != null)
		{
			T17DialogBoxManager.Instance.ReleaseHoldForUser(assignedGamepadUser);
		}
		if (T17DialogBoxManager.Instance.m_GlobalDialogs.Contains(box))
		{
			T17DialogBoxManager.Instance.m_GlobalDialogs.Remove(box);
			T17DialogBoxManager.Instance.m_bHasGlobalDiags = (T17DialogBoxManager.Instance.m_GlobalDialogs.Count > 0);
			T17EventSystemsManager.Instance.EnableAllEventSystems();
			return;
		}
		foreach (KeyValuePair<GamepadUser, List<T17DialogBox>> keyValuePair in T17DialogBoxManager.Instance.m_Bindings)
		{
			if (keyValuePair.Value.Contains(box))
			{
				keyValuePair.Value.Remove(box);
				break;
			}
		}
	}

	// Token: 0x04002E23 RID: 11811
	public static T17DialogBoxManager Instance;

	// Token: 0x04002E24 RID: 11812
	public GameObject m_DialogBoxPrefab;

	// Token: 0x04002E25 RID: 11813
	public Canvas m_GlobalDialogBoxCanvas;

	// Token: 0x04002E26 RID: 11814
	private List<T17DialogBox> m_DialogBoxPool;

	// Token: 0x04002E27 RID: 11815
	private const int POOL_SIZE = 15;

	// Token: 0x04002E28 RID: 11816
	private Dictionary<GamepadUser, List<T17DialogBox>> m_Bindings;

	// Token: 0x04002E29 RID: 11817
	private FastList<GamepadUser> m_bindingKeys;

	// Token: 0x04002E2A RID: 11818
	private List<T17DialogBox> m_GlobalDialogs;

	// Token: 0x04002E2B RID: 11819
	private Dictionary<string, Suppressor> m_suppressors = new Dictionary<string, Suppressor>();

	// Token: 0x04002E2C RID: 11820
	private bool m_bHasGlobalDiags;

	// Token: 0x04002E2D RID: 11821
	private List<T17DialogBoxManager.DialogQueueElement> m_DialogBoxQueue = new List<T17DialogBoxManager.DialogQueueElement>();

	// Token: 0x04002E2E RID: 11822
	private GameObject m_EventSystem;

	// Token: 0x02000B15 RID: 2837
	public class DialogQueueElement
	{
		// Token: 0x04002E2F RID: 11823
		public T17DialogBox Dialog;

		// Token: 0x04002E30 RID: 11824
		public T17DialogBoxManager.AllowedToShowEvent ShowEvent;
	}

	// Token: 0x02000B16 RID: 2838
	// (Invoke) Token: 0x06003965 RID: 14693
	public delegate void AllowedToShowEvent();
}
