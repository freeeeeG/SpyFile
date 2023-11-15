using System;
using Scenes;
using UI.Pause;
using UnityEngine;

// Token: 0x0200002F RID: 47
public class DarkMirrorTransition : MonoBehaviour
{
	// Token: 0x060000AF RID: 175 RVA: 0x00004533 File Offset: 0x00002733
	public void PushEmptyPauseEvent()
	{
		if (this._pauseEventSystem == null)
		{
			this._pauseEventSystem = Scene<GameBase>.instance.uiManager.pauseEventSystem;
		}
		this._pauseEventSystem.PushEvent(this._pauseEvent);
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x00004569 File Offset: 0x00002769
	public void PopPauseEvent()
	{
		if (this._pauseEventSystem == null)
		{
			this._pauseEventSystem = Scene<GameBase>.instance.uiManager.pauseEventSystem;
		}
		this._pauseEventSystem.PopEvent();
	}

	// Token: 0x0400009C RID: 156
	[PauseEvent.SubcomponentAttribute]
	[SerializeField]
	private PauseEvent _pauseEvent;

	// Token: 0x0400009D RID: 157
	[SerializeField]
	private PauseEventSystem _pauseEventSystem;
}
