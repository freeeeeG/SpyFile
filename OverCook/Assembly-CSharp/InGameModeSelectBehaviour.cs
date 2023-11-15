using System;
using GameModes;
using Team17.Online;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A98 RID: 2712
public class InGameModeSelectBehaviour : InGameMenuBehaviour
{
	// Token: 0x0600359F RID: 13727 RVA: 0x000FA93C File Offset: 0x000F8D3C
	protected override void SingleTimeInitialize()
	{
		base.SingleTimeInitialize();
		for (int i = 0; i < this.m_gameModeUIData.m_gameModes.Length; i++)
		{
			GameObject obj = GameUtils.InstantiateUIController(this.m_elementPrefab, this.m_elementParent);
			GameModeDialogueElementUIController gameModeDialogueElementUIController = obj.RequestComponent<GameModeDialogueElementUIController>();
			gameModeDialogueElementUIController.SetData(this, (Kind)i, this.m_gameModeUIData.m_gameModes[i]);
			if (i == 0)
			{
				Selectable selectable = obj.RequireComponent<Selectable>();
				Navigation borderSelectables = this.m_BorderSelectables;
				borderSelectables.selectOnDown = selectable;
				borderSelectables.selectOnUp = selectable;
				borderSelectables.selectOnLeft = selectable;
				borderSelectables.selectOnRight = selectable;
				this.m_BorderSelectables = borderSelectables;
			}
		}
		this.m_timeManager = GameUtils.RequestManager<TimeManager>();
	}

	// Token: 0x060035A0 RID: 13728 RVA: 0x000FA9EC File Offset: 0x000F8DEC
	public override bool Show(GamepadUser currentGamer, BaseMenuBehaviour parent, GameObject invoker, bool hideInvoker = true)
	{
		if (!base.Show(currentGamer, parent, invoker, hideInvoker))
		{
			return false;
		}
		this.m_pauseLayers = this.GetLayersToPause();
		for (int i = 0; i < this.m_pauseLayers.Length; i++)
		{
			this.m_timeManager.SetPaused(this.m_pauseLayers[i], true, this);
		}
		return true;
	}

	// Token: 0x060035A1 RID: 13729 RVA: 0x000FAA48 File Offset: 0x000F8E48
	public override bool Hide(bool restoreInvokerState = true, bool isTabSwitch = false)
	{
		if (!base.Hide(restoreInvokerState, isTabSwitch))
		{
			return false;
		}
		if (this.m_pauseLayers != null && this.m_pauseLayers.Length > 0)
		{
			for (int i = 0; i < this.m_pauseLayers.Length; i++)
			{
				this.m_timeManager.SetPaused(this.m_pauseLayers[i], false, this);
			}
			this.m_pauseLayers = null;
		}
		return true;
	}

	// Token: 0x060035A2 RID: 13730 RVA: 0x000FAAB3 File Offset: 0x000F8EB3
	private TimeManager.PauseLayer[] GetLayersToPause()
	{
		TimeManager.PauseLayer[] result;
		if (ConnectionStatus.IsInSession() && UserSystemUtils.AnyRemoteUsers())
		{
			(result = new TimeManager.PauseLayer[1])[0] = TimeManager.PauseLayer.Network;
		}
		else
		{
			result = new TimeManager.PauseLayer[1];
		}
		return result;
	}

	// Token: 0x04002B1E RID: 11038
	[SerializeField]
	[AssignResource("GameModeUIData", Editorbility.Editable)]
	private GameModeUIData m_gameModeUIData;

	// Token: 0x04002B1F RID: 11039
	[SerializeField]
	private RectTransform m_elementParent;

	// Token: 0x04002B20 RID: 11040
	[SerializeField]
	[AssignResource("InGameModeSelectElement", Editorbility.Editable)]
	private GameObject m_elementPrefab;

	// Token: 0x04002B21 RID: 11041
	private TimeManager m_timeManager;

	// Token: 0x04002B22 RID: 11042
	private TimeManager.PauseLayer[] m_pauseLayers;
}
