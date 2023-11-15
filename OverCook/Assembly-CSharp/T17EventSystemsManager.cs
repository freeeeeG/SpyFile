using System;
using System.Collections.Generic;
using System.Linq;
using Team17.Online;
using UnityEngine;

// Token: 0x02000B19 RID: 2841
public class T17EventSystemsManager
{
	// Token: 0x0600397D RID: 14717 RVA: 0x00110C69 File Offset: 0x0010F069
	private T17EventSystemsManager()
	{
		this.m_EventSystems = new Dictionary<string, T17EventSystem>();
		this.m_SelectedBeforeDisable = new Dictionary<string, GameObject>();
		this.m_FreeEventSystems = new List<T17EventSystem>();
		this.m_DisabledUsersRefCounts = new Dictionary<string, int>();
	}

	// Token: 0x170003ED RID: 1005
	// (get) Token: 0x0600397E RID: 14718 RVA: 0x00110C9D File Offset: 0x0010F09D
	public static T17EventSystemsManager Instance
	{
		get
		{
			if (T17EventSystemsManager.m_Instance == null)
			{
				T17EventSystemsManager.m_Instance = new T17EventSystemsManager();
			}
			return T17EventSystemsManager.m_Instance;
		}
	}

	// Token: 0x0600397F RID: 14719 RVA: 0x00110CB8 File Offset: 0x0010F0B8
	~T17EventSystemsManager()
	{
	}

	// Token: 0x06003980 RID: 14720 RVA: 0x00110CE4 File Offset: 0x0010F0E4
	public void ResetEventSystem(T17EventSystem eventSystem)
	{
		if (this.m_EventSystems.ContainsKey(eventSystem.AssignedGamepadUser.UID))
		{
			this.m_EventSystems.Remove(eventSystem.AssignedGamepadUser.UID);
		}
		eventSystem.ResetSystem();
		eventSystem.enabled = true;
		this.m_FreeEventSystems.Add(eventSystem);
	}

	// Token: 0x06003981 RID: 14721 RVA: 0x00110D3C File Offset: 0x0010F13C
	public void ResetAll()
	{
		List<T17EventSystem> list = this.m_EventSystems.Values.ToList<T17EventSystem>();
		for (int i = 0; i < list.Count; i++)
		{
			this.ResetEventSystem(list[i]);
		}
		this.m_EventSystems.Clear();
		this.m_SelectedBeforeDisable.Clear();
		this.m_DisabledUsersRefCounts.Clear();
	}

	// Token: 0x06003982 RID: 14722 RVA: 0x00110D9F File Offset: 0x0010F19F
	public void RegisterEventSystem(T17EventSystem eventSystem)
	{
		eventSystem.SetAssignedGamepadUser(null);
		this.m_FreeEventSystems.Add(eventSystem);
	}

	// Token: 0x06003983 RID: 14723 RVA: 0x00110DB4 File Offset: 0x0010F1B4
	public T17EventSystem AssignFreeEventSystemToGamepadUser(GamepadUser user)
	{
		if (user == null)
		{
			return null;
		}
		if (this.m_EventSystems.ContainsKey(user.UID))
		{
			return null;
		}
		for (int i = this.m_FreeEventSystems.Count - 1; i >= 0; i--)
		{
			if (this.m_FreeEventSystems[i].AssignedGamepadUser == null)
			{
				T17EventSystem t17EventSystem = this.m_FreeEventSystems[i];
				t17EventSystem.SetAssignedGamepadUser(user);
				this.m_EventSystems.Add(user.UID, t17EventSystem);
				this.m_FreeEventSystems.RemoveAt(i);
				return t17EventSystem;
			}
		}
		return null;
	}

	// Token: 0x06003984 RID: 14724 RVA: 0x00110E56 File Offset: 0x0010F256
	public T17EventSystem GetEventSystemForGamepadUser(GamepadUser gamepadUser)
	{
		if (gamepadUser == null)
		{
			return null;
		}
		if (this.m_EventSystems.ContainsKey(gamepadUser.UID))
		{
			return this.m_EventSystems[gamepadUser.UID];
		}
		return null;
	}

	// Token: 0x06003985 RID: 14725 RVA: 0x00110E8F File Offset: 0x0010F28F
	public T17EventSystem GetFirstFreeEventSystem()
	{
		if (this.m_FreeEventSystems.Count > 0)
		{
			return this.m_FreeEventSystems[0];
		}
		return null;
	}

	// Token: 0x06003986 RID: 14726 RVA: 0x00110EB0 File Offset: 0x0010F2B0
	public void DisableAllEventSystemsExceptFor(GamepadUser gamer)
	{
		foreach (KeyValuePair<string, T17EventSystem> keyValuePair in this.m_EventSystems)
		{
			if (gamer == null || keyValuePair.Key != gamer.UID)
			{
				int num = 0;
				this.m_DisabledUsersRefCounts.TryGetValue(keyValuePair.Key, out num);
				if (num == 0)
				{
					if (!this.m_SelectedBeforeDisable.ContainsKey(keyValuePair.Key))
					{
						this.m_SelectedBeforeDisable.Add(keyValuePair.Key, keyValuePair.Value.currentSelectedGameObject);
					}
					keyValuePair.Value.enabled = false;
				}
				num++;
				this.m_DisabledUsersRefCounts[keyValuePair.Key] = num;
			}
		}
	}

	// Token: 0x06003987 RID: 14727 RVA: 0x00110FA0 File Offset: 0x0010F3A0
	public void EnableAllEventSystems()
	{
		foreach (KeyValuePair<string, T17EventSystem> keyValuePair in this.m_EventSystems)
		{
			int num = 0;
			if (this.m_DisabledUsersRefCounts.TryGetValue(keyValuePair.Key, out num))
			{
				num--;
				if (num == 0)
				{
					keyValuePair.Value.enabled = true;
					if (this.m_SelectedBeforeDisable.ContainsKey(keyValuePair.Key))
					{
						GameObject gameObject = this.m_SelectedBeforeDisable[keyValuePair.Key];
						if (gameObject != null && gameObject.activeInHierarchy)
						{
							keyValuePair.Value.SetSelectedGameObject(null);
							keyValuePair.Value.SetSelectedGameObject(gameObject);
						}
						this.m_SelectedBeforeDisable.Remove(keyValuePair.Key);
					}
					this.m_DisabledUsersRefCounts.Remove(keyValuePair.Key);
				}
				else
				{
					this.m_DisabledUsersRefCounts[keyValuePair.Key] = num;
				}
			}
		}
	}

	// Token: 0x06003988 RID: 14728 RVA: 0x001110C0 File Offset: 0x0010F4C0
	public T17EventSystem GetEventSystemForUser(User user)
	{
		if (user == null || user.GamepadUser == null)
		{
			return null;
		}
		return this.GetEventSystemForGamepadUser(user.GamepadUser);
	}

	// Token: 0x06003989 RID: 14729 RVA: 0x001110E8 File Offset: 0x0010F4E8
	public T17EventSystem GetEventSystemForEngagementSlot(EngagementSlot slot)
	{
		FastList<User> users = ClientUserSystem.m_Users;
		int i = 0;
		while (i < users.Count)
		{
			User user = users._items[i];
			if (user.Engagement == slot)
			{
				if (user.GamepadUser != null)
				{
					return T17EventSystemsManager.Instance.GetEventSystemForGamepadUser(user.GamepadUser);
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

	// Token: 0x04002E36 RID: 11830
	private static T17EventSystemsManager m_Instance;

	// Token: 0x04002E37 RID: 11831
	private Dictionary<string, T17EventSystem> m_EventSystems;

	// Token: 0x04002E38 RID: 11832
	private Dictionary<string, GameObject> m_SelectedBeforeDisable;

	// Token: 0x04002E39 RID: 11833
	private List<T17EventSystem> m_FreeEventSystems;

	// Token: 0x04002E3A RID: 11834
	private Dictionary<string, int> m_DisabledUsersRefCounts;
}
