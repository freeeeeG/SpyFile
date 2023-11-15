using System;
using UnityEngine;

// Token: 0x02000162 RID: 354
public class UI_EditMode : MonoBehaviour
{
	// Token: 0x06000959 RID: 2393 RVA: 0x00023706 File Offset: 0x00021906
	private void OnEnable()
	{
		EventMgr.Register<eGameState>(eGameEvents.GameStateChanged, new Action<eGameState>(this.OnGameStateChanged));
	}

	// Token: 0x0600095A RID: 2394 RVA: 0x0002371F File Offset: 0x0002191F
	private void OnDisable()
	{
		EventMgr.Remove<eGameState>(eGameEvents.GameStateChanged, new Action<eGameState>(this.OnGameStateChanged));
	}

	// Token: 0x0600095B RID: 2395 RVA: 0x00023738 File Offset: 0x00021938
	private void OnGameStateChanged(eGameState state)
	{
		if (state == eGameState.EDIT_MODE)
		{
			this.canvasGroup.alpha = 1f;
			return;
		}
		this.canvasGroup.alpha = 0f;
	}

	// Token: 0x0400076A RID: 1898
	[SerializeField]
	private CanvasGroup canvasGroup;
}
