using System;
using System.Collections;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000AF8 RID: 2808
public class EditableTitleBar : TitleBar
{
	// Token: 0x14000023 RID: 35
	// (add) Token: 0x060056A6 RID: 22182 RVA: 0x001FA028 File Offset: 0x001F8228
	// (remove) Token: 0x060056A7 RID: 22183 RVA: 0x001FA060 File Offset: 0x001F8260
	public event Action<string> OnNameChanged;

	// Token: 0x14000024 RID: 36
	// (add) Token: 0x060056A8 RID: 22184 RVA: 0x001FA098 File Offset: 0x001F8298
	// (remove) Token: 0x060056A9 RID: 22185 RVA: 0x001FA0D0 File Offset: 0x001F82D0
	public event System.Action OnStartedEditing;

	// Token: 0x060056AA RID: 22186 RVA: 0x001FA108 File Offset: 0x001F8308
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.randomNameButton != null)
		{
			this.randomNameButton.onClick += this.GenerateRandomName;
		}
		if (this.editNameButton != null)
		{
			this.EnableEditButtonClick();
		}
		if (this.inputField != null)
		{
			this.inputField.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
		}
	}

	// Token: 0x060056AB RID: 22187 RVA: 0x001FA180 File Offset: 0x001F8380
	public void UpdateRenameTooltip(GameObject target)
	{
		if (this.editNameButton != null && target != null)
		{
			if (target.GetComponent<MinionBrain>() != null)
			{
				this.editNameButton.GetComponent<ToolTip>().toolTip = UI.TOOLTIPS.EDITNAME;
			}
			if (target.GetComponent<ClustercraftExteriorDoor>() != null || target.GetComponent<CommandModule>() != null)
			{
				this.editNameButton.GetComponent<ToolTip>().toolTip = UI.TOOLTIPS.EDITNAMEROCKET;
				return;
			}
			this.editNameButton.GetComponent<ToolTip>().toolTip = string.Format(UI.TOOLTIPS.EDITNAMEGENERIC, target.GetProperName());
		}
	}

	// Token: 0x060056AC RID: 22188 RVA: 0x001FA230 File Offset: 0x001F8430
	private void OnEndEdit(string finalStr)
	{
		finalStr = Localization.FilterDirtyWords(finalStr);
		this.SetEditingState(false);
		if (string.IsNullOrEmpty(finalStr))
		{
			return;
		}
		if (this.OnNameChanged != null)
		{
			this.OnNameChanged(finalStr);
		}
		this.titleText.text = finalStr;
		if (this.postEndEdit != null)
		{
			base.StopCoroutine(this.postEndEdit);
		}
		if (base.gameObject.activeInHierarchy && base.enabled)
		{
			this.postEndEdit = base.StartCoroutine(this.PostOnEndEditRoutine());
		}
	}

	// Token: 0x060056AD RID: 22189 RVA: 0x001FA2B0 File Offset: 0x001F84B0
	private IEnumerator PostOnEndEditRoutine()
	{
		int i = 0;
		while (i < 10)
		{
			int num = i;
			i = num + 1;
			yield return SequenceUtil.WaitForEndOfFrame;
		}
		this.EnableEditButtonClick();
		if (this.randomNameButton != null)
		{
			this.randomNameButton.gameObject.SetActive(false);
		}
		yield break;
	}

	// Token: 0x060056AE RID: 22190 RVA: 0x001FA2BF File Offset: 0x001F84BF
	private IEnumerator PreToggleNameEditingRoutine()
	{
		yield return SequenceUtil.WaitForEndOfFrame;
		this.ToggleNameEditing();
		this.preToggleNameEditing = null;
		yield break;
	}

	// Token: 0x060056AF RID: 22191 RVA: 0x001FA2CE File Offset: 0x001F84CE
	private void EnableEditButtonClick()
	{
		this.editNameButton.onClick += delegate()
		{
			if (this.preToggleNameEditing != null)
			{
				return;
			}
			this.preToggleNameEditing = base.StartCoroutine(this.PreToggleNameEditingRoutine());
		};
	}

	// Token: 0x060056B0 RID: 22192 RVA: 0x001FA2E8 File Offset: 0x001F84E8
	private void GenerateRandomName()
	{
		if (this.postEndEdit != null)
		{
			base.StopCoroutine(this.postEndEdit);
		}
		string text = GameUtil.GenerateRandomDuplicantName();
		if (this.OnNameChanged != null)
		{
			this.OnNameChanged(text);
		}
		this.titleText.text = text;
		this.SetEditingState(true);
	}

	// Token: 0x060056B1 RID: 22193 RVA: 0x001FA338 File Offset: 0x001F8538
	private void ToggleNameEditing()
	{
		this.editNameButton.ClearOnClick();
		bool flag = !this.inputField.gameObject.activeInHierarchy;
		if (this.randomNameButton != null)
		{
			this.randomNameButton.gameObject.SetActive(flag);
		}
		this.SetEditingState(flag);
	}

	// Token: 0x060056B2 RID: 22194 RVA: 0x001FA38C File Offset: 0x001F858C
	private void SetEditingState(bool state)
	{
		this.titleText.gameObject.SetActive(!state);
		if (this.setCameraControllerState)
		{
			CameraController.Instance.DisableUserCameraControl = state;
		}
		if (this.inputField == null)
		{
			return;
		}
		this.inputField.gameObject.SetActive(state);
		if (state)
		{
			this.inputField.text = this.titleText.text;
			this.inputField.Select();
			this.inputField.ActivateInputField();
			if (this.OnStartedEditing != null)
			{
				this.OnStartedEditing();
				return;
			}
		}
		else
		{
			this.inputField.DeactivateInputField();
		}
	}

	// Token: 0x060056B3 RID: 22195 RVA: 0x001FA42E File Offset: 0x001F862E
	public void ForceStopEditing()
	{
		if (this.postEndEdit != null)
		{
			base.StopCoroutine(this.postEndEdit);
		}
		this.editNameButton.ClearOnClick();
		this.SetEditingState(false);
		this.EnableEditButtonClick();
	}

	// Token: 0x060056B4 RID: 22196 RVA: 0x001FA45C File Offset: 0x001F865C
	public void SetUserEditable(bool editable)
	{
		this.userEditable = editable;
		this.editNameButton.gameObject.SetActive(editable);
		this.editNameButton.ClearOnClick();
		this.EnableEditButtonClick();
	}

	// Token: 0x04003A5D RID: 14941
	public KButton editNameButton;

	// Token: 0x04003A5E RID: 14942
	public KButton randomNameButton;

	// Token: 0x04003A5F RID: 14943
	public KInputTextField inputField;

	// Token: 0x04003A62 RID: 14946
	private Coroutine postEndEdit;

	// Token: 0x04003A63 RID: 14947
	private Coroutine preToggleNameEditing;
}
