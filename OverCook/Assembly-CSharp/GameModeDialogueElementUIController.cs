using System;
using GameModes;
using UnityEngine;

// Token: 0x02000A96 RID: 2710
public class GameModeDialogueElementUIController : UIControllerBase
{
	// Token: 0x06003597 RID: 13719 RVA: 0x000FA670 File Offset: 0x000F8A70
	private void Start()
	{
	}

	// Token: 0x06003598 RID: 13720 RVA: 0x000FA674 File Offset: 0x000F8A74
	public void SetData(InGameMenuBehaviour parent, Kind kind, ModeUIData uiData)
	{
		this.m_parent = parent;
		this.m_kind = kind;
		this.m_uiData = uiData;
		this.m_name.SetLocalisedTextCatchAll(this.m_uiData.m_nameLocalisationKey);
		this.m_description.SetLocalisedTextCatchAll(this.m_uiData.m_descriptionLocalisationKey);
		this.m_image.sprite = this.m_uiData.m_previewImage;
	}

	// Token: 0x06003599 RID: 13721 RVA: 0x000FA6D8 File Offset: 0x000F8AD8
	public void OnModeSelect()
	{
		GameSession gameSession = GameUtils.GetGameSession();
		gameSession.GameModeKind = this.m_kind;
		gameSession.CommitGameModeSessionConfig();
		this.m_parent.Close();
	}

	// Token: 0x04002B12 RID: 11026
	[HideInInspector]
	private ModeUIData m_uiData;

	// Token: 0x04002B13 RID: 11027
	private InGameMenuBehaviour m_parent;

	// Token: 0x04002B14 RID: 11028
	private Kind m_kind;

	// Token: 0x04002B15 RID: 11029
	[SerializeField]
	private T17Text m_name;

	// Token: 0x04002B16 RID: 11030
	[SerializeField]
	private T17Text m_description;

	// Token: 0x04002B17 RID: 11031
	[SerializeField]
	private T17Image m_image;

	// Token: 0x04002B18 RID: 11032
	[SerializeField]
	private T17Button m_selectButton;
}
