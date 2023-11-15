using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000BF2 RID: 3058
[AddComponentMenu("KMonoBehaviour/scripts/AccessControlSideScreenDoor")]
public class AccessControlSideScreenDoor : KMonoBehaviour
{
	// Token: 0x060060B1 RID: 24753 RVA: 0x0023BC2F File Offset: 0x00239E2F
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.leftButton.onClick += this.OnPermissionButtonClicked;
		this.rightButton.onClick += this.OnPermissionButtonClicked;
	}

	// Token: 0x060060B2 RID: 24754 RVA: 0x0023BC68 File Offset: 0x00239E68
	private void OnPermissionButtonClicked()
	{
		AccessControl.Permission arg;
		if (this.leftButton.isOn)
		{
			if (this.rightButton.isOn)
			{
				arg = AccessControl.Permission.Both;
			}
			else
			{
				arg = AccessControl.Permission.GoLeft;
			}
		}
		else if (this.rightButton.isOn)
		{
			arg = AccessControl.Permission.GoRight;
		}
		else
		{
			arg = AccessControl.Permission.Neither;
		}
		this.UpdateButtonStates(false);
		this.permissionChangedCallback(this.targetIdentity, arg);
	}

	// Token: 0x060060B3 RID: 24755 RVA: 0x0023BCC4 File Offset: 0x00239EC4
	protected virtual void UpdateButtonStates(bool isDefault)
	{
		ToolTip component = this.leftButton.GetComponent<ToolTip>();
		ToolTip component2 = this.rightButton.GetComponent<ToolTip>();
		if (this.isUpDown)
		{
			component.SetSimpleTooltip(this.leftButton.isOn ? UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_UP_ENABLED : UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_UP_DISABLED);
			component2.SetSimpleTooltip(this.rightButton.isOn ? UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_DOWN_ENABLED : UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_DOWN_DISABLED);
			return;
		}
		component.SetSimpleTooltip(this.leftButton.isOn ? UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_LEFT_ENABLED : UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_LEFT_DISABLED);
		component2.SetSimpleTooltip(this.rightButton.isOn ? UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_RIGHT_ENABLED : UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_RIGHT_DISABLED);
	}

	// Token: 0x060060B4 RID: 24756 RVA: 0x0023BD82 File Offset: 0x00239F82
	public void SetRotated(bool rotated)
	{
		this.isUpDown = rotated;
	}

	// Token: 0x060060B5 RID: 24757 RVA: 0x0023BD8B File Offset: 0x00239F8B
	public void SetContent(AccessControl.Permission permission, Action<MinionAssignablesProxy, AccessControl.Permission> onPermissionChange)
	{
		this.permissionChangedCallback = onPermissionChange;
		this.leftButton.isOn = (permission == AccessControl.Permission.Both || permission == AccessControl.Permission.GoLeft);
		this.rightButton.isOn = (permission == AccessControl.Permission.Both || permission == AccessControl.Permission.GoRight);
		this.UpdateButtonStates(false);
	}

	// Token: 0x040041E2 RID: 16866
	public KToggle leftButton;

	// Token: 0x040041E3 RID: 16867
	public KToggle rightButton;

	// Token: 0x040041E4 RID: 16868
	private Action<MinionAssignablesProxy, AccessControl.Permission> permissionChangedCallback;

	// Token: 0x040041E5 RID: 16869
	private bool isUpDown;

	// Token: 0x040041E6 RID: 16870
	protected MinionAssignablesProxy targetIdentity;
}
