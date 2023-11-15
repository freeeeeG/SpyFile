using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000BFA RID: 3066
[AddComponentMenu("KMonoBehaviour/scripts/AssignableSideScreenRow")]
public class AssignableSideScreenRow : KMonoBehaviour
{
	// Token: 0x0600610C RID: 24844 RVA: 0x0023D978 File Offset: 0x0023BB78
	public void Refresh(object data = null)
	{
		if (!this.sideScreen.targetAssignable.CanAssignTo(this.targetIdentity))
		{
			this.currentState = AssignableSideScreenRow.AssignableState.Disabled;
			this.assignmentText.text = UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.DISABLED;
		}
		else if (this.sideScreen.targetAssignable.assignee == this.targetIdentity)
		{
			this.currentState = AssignableSideScreenRow.AssignableState.Selected;
			this.assignmentText.text = UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.ASSIGNED;
		}
		else
		{
			bool flag = false;
			KMonoBehaviour kmonoBehaviour = this.targetIdentity as KMonoBehaviour;
			if (kmonoBehaviour != null)
			{
				Ownables component = kmonoBehaviour.GetComponent<Ownables>();
				if (component != null)
				{
					AssignableSlotInstance slot = component.GetSlot(this.sideScreen.targetAssignable.slot);
					if (slot != null && slot.IsAssigned())
					{
						this.currentState = AssignableSideScreenRow.AssignableState.AssignedToOther;
						this.assignmentText.text = slot.assignable.GetProperName();
						flag = true;
					}
				}
				Equipment component2 = kmonoBehaviour.GetComponent<Equipment>();
				if (component2 != null)
				{
					AssignableSlotInstance slot2 = component2.GetSlot(this.sideScreen.targetAssignable.slot);
					if (slot2 != null && slot2.IsAssigned())
					{
						this.currentState = AssignableSideScreenRow.AssignableState.AssignedToOther;
						this.assignmentText.text = slot2.assignable.GetProperName();
						flag = true;
					}
				}
			}
			if (!flag)
			{
				this.currentState = AssignableSideScreenRow.AssignableState.Unassigned;
				this.assignmentText.text = UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.UNASSIGNED;
			}
		}
		this.toggle.ChangeState((int)this.currentState);
	}

	// Token: 0x0600610D RID: 24845 RVA: 0x0023DAEA File Offset: 0x0023BCEA
	protected override void OnCleanUp()
	{
		if (this.refreshHandle == -1)
		{
			Game.Instance.Unsubscribe(this.refreshHandle);
		}
		base.OnCleanUp();
	}

	// Token: 0x0600610E RID: 24846 RVA: 0x0023DB0C File Offset: 0x0023BD0C
	public void SetContent(IAssignableIdentity identity_object, Action<IAssignableIdentity> selectionCallback, AssignableSideScreen assignableSideScreen)
	{
		if (this.refreshHandle == -1)
		{
			Game.Instance.Unsubscribe(this.refreshHandle);
		}
		this.refreshHandle = Game.Instance.Subscribe(-2146166042, delegate(object o)
		{
			if (this != null && this.gameObject != null && this.gameObject.activeInHierarchy)
			{
				this.Refresh(null);
			}
		});
		this.toggle = base.GetComponent<MultiToggle>();
		this.sideScreen = assignableSideScreen;
		this.targetIdentity = identity_object;
		if (this.portraitInstance == null)
		{
			this.portraitInstance = Util.KInstantiateUI<CrewPortrait>(this.crewPortraitPrefab.gameObject, base.gameObject, false);
			this.portraitInstance.transform.SetSiblingIndex(1);
			this.portraitInstance.SetAlpha(1f);
		}
		this.toggle.onClick = delegate()
		{
			selectionCallback(this.targetIdentity);
		};
		this.portraitInstance.SetIdentityObject(identity_object, false);
		base.GetComponent<ToolTip>().OnToolTip = new Func<string>(this.GetTooltip);
		this.Refresh(null);
	}

	// Token: 0x0600610F RID: 24847 RVA: 0x0023DC10 File Offset: 0x0023BE10
	private string GetTooltip()
	{
		ToolTip component = base.GetComponent<ToolTip>();
		component.ClearMultiStringTooltip();
		if (this.targetIdentity != null && !this.targetIdentity.IsNull())
		{
			AssignableSideScreenRow.AssignableState assignableState = this.currentState;
			if (assignableState != AssignableSideScreenRow.AssignableState.Selected)
			{
				if (assignableState != AssignableSideScreenRow.AssignableState.Disabled)
				{
					component.AddMultiStringTooltip(string.Format(UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.ASSIGN_TO_TOOLTIP, this.targetIdentity.GetProperName()), null);
				}
				else
				{
					component.AddMultiStringTooltip(string.Format(UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.DISABLED_TOOLTIP, this.targetIdentity.GetProperName()), null);
				}
			}
			else
			{
				component.AddMultiStringTooltip(string.Format(UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.UNASSIGN_TOOLTIP, this.targetIdentity.GetProperName()), null);
			}
		}
		return "";
	}

	// Token: 0x0400421A RID: 16922
	[SerializeField]
	private CrewPortrait crewPortraitPrefab;

	// Token: 0x0400421B RID: 16923
	[SerializeField]
	private LocText assignmentText;

	// Token: 0x0400421C RID: 16924
	public AssignableSideScreen sideScreen;

	// Token: 0x0400421D RID: 16925
	private CrewPortrait portraitInstance;

	// Token: 0x0400421E RID: 16926
	[MyCmpReq]
	private MultiToggle toggle;

	// Token: 0x0400421F RID: 16927
	public IAssignableIdentity targetIdentity;

	// Token: 0x04004220 RID: 16928
	public AssignableSideScreenRow.AssignableState currentState;

	// Token: 0x04004221 RID: 16929
	private int refreshHandle = -1;

	// Token: 0x02001B4E RID: 6990
	public enum AssignableState
	{
		// Token: 0x04007C66 RID: 31846
		Selected,
		// Token: 0x04007C67 RID: 31847
		AssignedToOther,
		// Token: 0x04007C68 RID: 31848
		Unassigned,
		// Token: 0x04007C69 RID: 31849
		Disabled
	}
}
