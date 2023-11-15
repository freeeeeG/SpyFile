using System;
using System.Collections.Generic;
using Team17.Online;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000AAB RID: 2731
public class ControlSchemeToggle : MonoBehaviour
{
	// Token: 0x0600361E RID: 13854 RVA: 0x000FD91B File Offset: 0x000FBD1B
	private void Awake()
	{
		this.SetClickListener(this.m_LeftButton, new UnityAction(this.OnPreviousControlScheme));
		this.SetClickListener(this.m_RightButton, new UnityAction(this.OnNextControlScheme));
		this.ShowAppropriateControlScheme();
	}

	// Token: 0x0600361F RID: 13855 RVA: 0x000FD954 File Offset: 0x000FBD54
	private void OnEnable()
	{
		if (this.m_ParentMenu != null)
		{
			KeyboardRebindElement[] componentsInChildren = this.m_ControlSchemes[this.m_ControlSchemeIndex].m_Scheme.GetComponentsInChildren<KeyboardRebindElement>();
			Selectable selectable = (componentsInChildren == null || componentsInChildren.Length <= 0) ? null : componentsInChildren[0].GetComponent<Selectable>();
			if (selectable != null && selectable.gameObject.activeInHierarchy)
			{
				this.m_ParentMenu.m_BorderSelectables.selectOnUp = selectable;
			}
			else if (this.m_ControlSchemes[this.m_ControlSchemeIndex].m_ControlType == GamepadUser.ControlTypeEnum.Keyboard)
			{
				this.m_ParentMenu.m_BorderSelectables.selectOnUp = this.m_AcceptButton;
			}
			else
			{
				this.m_ParentMenu.m_BorderSelectables.selectOnUp = this.m_RightButton;
			}
		}
	}

	// Token: 0x06003620 RID: 13856 RVA: 0x000FDA23 File Offset: 0x000FBE23
	public bool IsCurrentSchemeSplit()
	{
		return this.m_ControlSchemeIndex >= 0 && this.m_ControlSchemeIndex < this.m_ControlSchemes.Length && this.m_ControlSchemes[this.m_ControlSchemeIndex].m_Split;
	}

	// Token: 0x06003621 RID: 13857 RVA: 0x000FDA58 File Offset: 0x000FBE58
	private void ShowAppropriateControlScheme()
	{
		int num = -1;
		FastList<User> users = ClientUserSystem.m_Users;
		if (users != null && users.Count > 0)
		{
			GamepadUser gamepadUser = users._items[0].GamepadUser;
			if (gamepadUser != null)
			{
				GamepadUser.ControlTypeEnum controlType = gamepadUser.ControlType;
				bool flag = users._items[0].PadSide != PadSide.Both;
				for (int i = 0; i < this.m_ControlSchemes.Length; i++)
				{
					if (this.m_ControlSchemes[i].m_ControlType == controlType && (num == -1 || this.m_ControlSchemes[i].m_Split == flag))
					{
						num = i;
					}
				}
			}
		}
		this.ShowControlScheme((num == -1) ? 0 : num);
	}

	// Token: 0x06003622 RID: 13858 RVA: 0x000FDB19 File Offset: 0x000FBF19
	private void OnPreviousControlScheme()
	{
		this.ShowControlScheme((this.m_ControlSchemeIndex + (this.m_ControlSchemes.Length - 1)) % this.m_ControlSchemes.Length);
	}

	// Token: 0x06003623 RID: 13859 RVA: 0x000FDB3B File Offset: 0x000FBF3B
	private void OnNextControlScheme()
	{
		this.ShowControlScheme((this.m_ControlSchemeIndex + 1) % this.m_ControlSchemes.Length);
	}

	// Token: 0x06003624 RID: 13860 RVA: 0x000FDB54 File Offset: 0x000FBF54
	private void ShowControlScheme(int index)
	{
		this.m_ControlSchemeIndex = index;
		for (int i = 0; i < this.m_ControlSchemes.Length; i++)
		{
			this.m_ControlSchemes[i].m_Scheme.SetActive(i == this.m_ControlSchemeIndex);
		}
		KeyboardRebindElement[] componentsInChildren = this.m_ControlSchemes[this.m_ControlSchemeIndex].m_Scheme.GetComponentsInChildren<KeyboardRebindElement>();
		if (componentsInChildren.Length > 0)
		{
			Selectable component = componentsInChildren[0].GetComponent<Selectable>();
			this.SetNavigationOnDown(this.m_LeftButton, component);
			this.SetNavigationOnDown(this.m_RightButton, component);
			Selectable component2 = componentsInChildren[componentsInChildren.Length - 1].GetComponent<Selectable>();
			this.SetNavigationOnUp(this.m_AcceptButton, component2);
			if (this.m_ControlSchemes[this.m_ControlSchemeIndex].m_Split)
			{
				Selectable component3 = componentsInChildren[(componentsInChildren.Length - 1) / 2].GetComponent<Selectable>();
				this.SetNavigationOnUp(this.m_DefaultsButton, component3);
				this.SetNavigationOnDown(component3, this.m_DefaultsButton);
				this.SetNavigationOnDown(component2, this.m_AcceptButton);
			}
			else
			{
				this.SetNavigationOnUp(this.m_DefaultsButton, component2);
				this.SetNavigationOnDown(component2, this.m_AcceptButton);
			}
		}
		if (this.m_ToggleText != null)
		{
			this.m_ToggleText.SetLocalisedTextCatchAll(this.m_ControlSchemes[this.m_ControlSchemeIndex].m_Name);
		}
		bool active = this.m_ControlSchemes[this.m_ControlSchemeIndex].m_ControlType == GamepadUser.ControlTypeEnum.Keyboard;
		if (this.m_DefaultsButton != null)
		{
			this.m_DefaultsButton.gameObject.SetActive(active);
		}
		if (this.m_AcceptButton != null)
		{
			this.m_AcceptButton.gameObject.SetActive(active);
		}
	}

	// Token: 0x06003625 RID: 13861 RVA: 0x000FDCF4 File Offset: 0x000FC0F4
	private void SetClickListener(T17Button button, UnityAction listener)
	{
		if (button != null && listener != null)
		{
			button.onClick.AddListener(listener);
		}
	}

	// Token: 0x06003626 RID: 13862 RVA: 0x000FDD14 File Offset: 0x000FC114
	private void SetNavigationOnDown(Selectable obj, Selectable target)
	{
		if (obj != null && target != null)
		{
			Navigation navigation = obj.navigation;
			navigation.selectOnDown = target;
			obj.navigation = navigation;
		}
	}

	// Token: 0x06003627 RID: 13863 RVA: 0x000FDD50 File Offset: 0x000FC150
	private void SetNavigationOnUp(Selectable obj, Selectable target)
	{
		if (obj != null && target != null)
		{
			Navigation navigation = obj.navigation;
			navigation.selectOnUp = target;
			obj.navigation = navigation;
		}
	}

	// Token: 0x04002B85 RID: 11141
	[Header("PC Input Toggle")]
	[SerializeField]
	private BaseMenuBehaviour m_ParentMenu;

	// Token: 0x04002B86 RID: 11142
	[SerializeField]
	private T17Button m_LeftButton;

	// Token: 0x04002B87 RID: 11143
	[SerializeField]
	private T17Button m_RightButton;

	// Token: 0x04002B88 RID: 11144
	[SerializeField]
	private T17Button m_DefaultsButton;

	// Token: 0x04002B89 RID: 11145
	[SerializeField]
	private T17Button m_AcceptButton;

	// Token: 0x04002B8A RID: 11146
	[SerializeField]
	private T17Text m_ToggleText;

	// Token: 0x04002B8B RID: 11147
	[SerializeField]
	private ControlSchemeToggle.ControlScheme[] m_ControlSchemes;

	// Token: 0x04002B8C RID: 11148
	private int m_ControlSchemeIndex;

	// Token: 0x02000AAC RID: 2732
	[Serializable]
	private class ControlScheme
	{
		// Token: 0x04002B8D RID: 11149
		public string m_Name = string.Empty;

		// Token: 0x04002B8E RID: 11150
		public GamepadUser.ControlTypeEnum m_ControlType;

		// Token: 0x04002B8F RID: 11151
		public bool m_Split;

		// Token: 0x04002B90 RID: 11152
		public GameObject m_Scheme;
	}
}
