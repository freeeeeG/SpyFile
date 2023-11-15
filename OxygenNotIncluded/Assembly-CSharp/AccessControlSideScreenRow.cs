using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000BF3 RID: 3059
public class AccessControlSideScreenRow : AccessControlSideScreenDoor
{
	// Token: 0x060060B7 RID: 24759 RVA: 0x0023BDCD File Offset: 0x00239FCD
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.defaultButton.onValueChanged += this.OnDefaultButtonChanged;
	}

	// Token: 0x060060B8 RID: 24760 RVA: 0x0023BDEC File Offset: 0x00239FEC
	private void OnDefaultButtonChanged(bool state)
	{
		this.UpdateButtonStates(!state);
		if (this.defaultClickedCallback != null)
		{
			this.defaultClickedCallback(this.targetIdentity, !state);
		}
	}

	// Token: 0x060060B9 RID: 24761 RVA: 0x0023BE18 File Offset: 0x0023A018
	protected override void UpdateButtonStates(bool isDefault)
	{
		base.UpdateButtonStates(isDefault);
		this.defaultButton.GetComponent<ToolTip>().SetSimpleTooltip(isDefault ? UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.SET_TO_CUSTOM : UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.SET_TO_DEFAULT);
		this.defaultControls.SetActive(isDefault);
		this.customControls.SetActive(!isDefault);
	}

	// Token: 0x060060BA RID: 24762 RVA: 0x0023BE6C File Offset: 0x0023A06C
	public void SetMinionContent(MinionAssignablesProxy identity, AccessControl.Permission permission, bool isDefault, Action<MinionAssignablesProxy, AccessControl.Permission> onPermissionChange, Action<MinionAssignablesProxy, bool> onDefaultClick)
	{
		base.SetContent(permission, onPermissionChange);
		if (identity == null)
		{
			global::Debug.LogError("Invalid data received.");
			return;
		}
		if (this.portraitInstance == null)
		{
			this.portraitInstance = Util.KInstantiateUI<CrewPortrait>(this.crewPortraitPrefab.gameObject, this.defaultButton.gameObject, false);
			this.portraitInstance.SetAlpha(1f);
		}
		this.targetIdentity = identity;
		this.portraitInstance.SetIdentityObject(identity, false);
		this.portraitInstance.SetSubTitle(isDefault ? UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.USING_DEFAULT : UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.USING_CUSTOM);
		this.defaultClickedCallback = null;
		this.defaultButton.isOn = !isDefault;
		this.defaultClickedCallback = onDefaultClick;
	}

	// Token: 0x040041E7 RID: 16871
	[SerializeField]
	private CrewPortrait crewPortraitPrefab;

	// Token: 0x040041E8 RID: 16872
	private CrewPortrait portraitInstance;

	// Token: 0x040041E9 RID: 16873
	public KToggle defaultButton;

	// Token: 0x040041EA RID: 16874
	public GameObject defaultControls;

	// Token: 0x040041EB RID: 16875
	public GameObject customControls;

	// Token: 0x040041EC RID: 16876
	private Action<MinionAssignablesProxy, bool> defaultClickedCallback;
}
