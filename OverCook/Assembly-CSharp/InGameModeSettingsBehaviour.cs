using System;
using GameModes;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A99 RID: 2713
public class InGameModeSettingsBehaviour : InGameMenuBehaviour
{
	// Token: 0x060035A4 RID: 13732 RVA: 0x000FAB18 File Offset: 0x000F8F18
	protected override void SingleTimeInitialize()
	{
		base.SingleTimeInitialize();
		for (int i = 0; i < this.m_gameModeUIData.m_gameModeSettings.Length; i++)
		{
			this.m_settingListElements[i] = GameUtils.InstantiateUIController(this.m_elementPrefab, this.m_elementParent);
			this.m_settingListElementSelectables[i] = this.m_settingListElements[i].RequireComponent<Selectable>();
			if (i == 0)
			{
				Navigation borderSelectables = this.m_BorderSelectables;
				borderSelectables.selectOnUp = this.m_settingListElementSelectables[0];
				borderSelectables.selectOnLeft = this.m_settingListElementSelectables[0];
				borderSelectables.selectOnRight = this.m_settingListElementSelectables[0];
				this.m_BorderSelectables = borderSelectables;
			}
		}
	}

	// Token: 0x060035A5 RID: 13733 RVA: 0x000FABBC File Offset: 0x000F8FBC
	public override bool Show(GamepadUser currentGamer, BaseMenuBehaviour parent, GameObject invoker, bool hideInvoker = true)
	{
		GameSession gameSession = GameUtils.GetGameSession();
		ModeUIData modeUIData = this.m_gameModeUIData.m_gameModes[(int)gameSession.GameModeKind];
		if (modeUIData.m_supportedSettings == null || modeUIData.m_supportedSettings.Length < 1)
		{
			return false;
		}
		if (!base.Show(currentGamer, parent, invoker, hideInvoker))
		{
			return false;
		}
		this.m_settingListBufferCount = 0;
		for (int i = 0; i < this.m_settingListElements.Length; i++)
		{
			SettingKind settingKind = (SettingKind)i;
			bool flag = Array.IndexOf<SettingKind>(modeUIData.m_supportedSettings, settingKind) != -1;
			if (flag)
			{
				this.m_settingListElements[i].SetActive(true);
				this.m_settingListBuffer[this.m_settingListBufferCount++] = this.m_settingListElementSelectables[i];
				GameModeSettingElementUIController gameModeSettingElementUIController = this.m_settingListElements[i].RequestComponent<GameModeSettingElementUIController>();
				gameModeSettingElementUIController.SetData(settingKind, this.m_gameModeUIData.m_gameModeSettings[i], this.m_CachedEventSystem);
			}
		}
		for (int j = 0; j < this.m_settingListBufferCount; j++)
		{
			Navigation navigation = this.m_settingListBuffer[j].navigation;
			checked
			{
				navigation.selectOnUp = this.m_settingListBuffer[(int)((IntPtr)(unchecked((ulong)(j - 1) % (ulong)((long)this.m_settingListBuffer.Length))))];
				navigation.selectOnDown = this.m_settingListBuffer[(int)((IntPtr)(unchecked((ulong)(j + 1) % (ulong)((long)this.m_settingListBuffer.Length))))];
				this.m_settingListBuffer[j].navigation = navigation;
			}
		}
		if (this.m_settingListBufferCount > 0)
		{
			Navigation navigation2 = this.m_settingListBuffer[this.m_settingListBufferCount - 1].navigation;
			navigation2.selectOnDown = this.m_confirmButton;
			this.m_settingListBuffer[this.m_settingListBufferCount - 1].navigation = navigation2;
			Navigation navigation3 = this.m_confirmButton.navigation;
			navigation3.selectOnUp = this.m_settingListBuffer[this.m_settingListBufferCount - 1];
			this.m_confirmButton.navigation = navigation3;
			Navigation navigation4 = this.m_cancelButton.navigation;
			navigation4.selectOnUp = this.m_settingListBuffer[this.m_settingListBufferCount - 1];
			this.m_cancelButton.navigation = navigation4;
		}
		return true;
	}

	// Token: 0x060035A6 RID: 13734 RVA: 0x000FADD8 File Offset: 0x000F91D8
	public override void Close()
	{
		GameSession gameSession = GameUtils.GetGameSession();
		if (gameSession.PendingGameModeSessionConfigChanges)
		{
			T17DialogBox dialog = T17DialogBoxManager.GetDialog(false);
			dialog.Initialize("Text.Warning", "Text.Menu.UnsavedChanges.Body", "Text.Button.Discard", "Text.Button.Save", null, T17DialogBox.Symbols.Warning, true, true, false);
			T17DialogBox t17DialogBox = dialog;
			t17DialogBox.OnConfirm = (T17DialogBox.DialogEvent)Delegate.Combine(t17DialogBox.OnConfirm, new T17DialogBox.DialogEvent(this.OnDialogConfirm));
			T17DialogBox t17DialogBox2 = dialog;
			t17DialogBox2.OnCancel = (T17DialogBox.DialogEvent)Delegate.Combine(t17DialogBox2.OnCancel, new T17DialogBox.DialogEvent(this.OnDialogCancel));
			T17DialogBox t17DialogBox3 = dialog;
			t17DialogBox3.OnDecline = (T17DialogBox.DialogEvent)Delegate.Combine(t17DialogBox3.OnDecline, new T17DialogBox.DialogEvent(this.OnDialogCancel));
			dialog.Show();
		}
		else
		{
			gameSession.RevertGameModeSessionConfig();
			base.Close();
		}
	}

	// Token: 0x060035A7 RID: 13735 RVA: 0x000FAE9C File Offset: 0x000F929C
	private void OnDialogConfirm()
	{
		GameSession gameSession = GameUtils.GetGameSession();
		gameSession.RevertGameModeSessionConfig();
		base.Close();
	}

	// Token: 0x060035A8 RID: 13736 RVA: 0x000FAEBC File Offset: 0x000F92BC
	private void OnDialogCancel()
	{
		GameSession gameSession = GameUtils.GetGameSession();
		gameSession.CommitGameModeSessionConfig();
		base.Close();
	}

	// Token: 0x060035A9 RID: 13737 RVA: 0x000FAEDC File Offset: 0x000F92DC
	public void Confirm()
	{
		GameSession gameSession = GameUtils.GetGameSession();
		gameSession.CommitGameModeSessionConfig();
		base.Close();
	}

	// Token: 0x04002B23 RID: 11043
	[SerializeField]
	[AssignResource("GameModeUIData", Editorbility.Editable)]
	private GameModeUIData m_gameModeUIData;

	// Token: 0x04002B24 RID: 11044
	[SerializeField]
	private T17ScrollView m_scrollView;

	// Token: 0x04002B25 RID: 11045
	[SerializeField]
	private RectTransform m_elementParent;

	// Token: 0x04002B26 RID: 11046
	[SerializeField]
	[AssignResource("InGameModeSettingElement", Editorbility.Editable)]
	private GameObject m_elementPrefab;

	// Token: 0x04002B27 RID: 11047
	[SerializeField]
	private T17Button m_confirmButton;

	// Token: 0x04002B28 RID: 11048
	[SerializeField]
	private T17Button m_cancelButton;

	// Token: 0x04002B29 RID: 11049
	private GameObject[] m_settingListElements = new GameObject[3];

	// Token: 0x04002B2A RID: 11050
	private Selectable[] m_settingListElementSelectables = new Selectable[3];

	// Token: 0x04002B2B RID: 11051
	private int m_settingListBufferCount;

	// Token: 0x04002B2C RID: 11052
	private Selectable[] m_settingListBuffer = new Selectable[3];

	// Token: 0x04002B2D RID: 11053
	private SessionConfig m_sessionConfig = new SessionConfig();
}
