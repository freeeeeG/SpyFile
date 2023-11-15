using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B9D RID: 2973
[AddComponentMenu("KMonoBehaviour/scripts/NewGameFlow")]
public class NewGameFlow : KMonoBehaviour
{
	// Token: 0x06005CBE RID: 23742 RVA: 0x0021FC9C File Offset: 0x0021DE9C
	public void BeginFlow()
	{
		this.currentScreenIndex = -1;
		this.Next();
	}

	// Token: 0x06005CBF RID: 23743 RVA: 0x0021FCAB File Offset: 0x0021DEAB
	private void Next()
	{
		this.ClearCurrentScreen();
		this.currentScreenIndex++;
		this.ActivateCurrentScreen();
	}

	// Token: 0x06005CC0 RID: 23744 RVA: 0x0021FCC7 File Offset: 0x0021DEC7
	private void Previous()
	{
		this.ClearCurrentScreen();
		this.currentScreenIndex--;
		this.ActivateCurrentScreen();
	}

	// Token: 0x06005CC1 RID: 23745 RVA: 0x0021FCE3 File Offset: 0x0021DEE3
	private void ClearCurrentScreen()
	{
		if (this.currentScreen != null)
		{
			this.currentScreen.Deactivate();
			this.currentScreen = null;
		}
	}

	// Token: 0x06005CC2 RID: 23746 RVA: 0x0021FD08 File Offset: 0x0021DF08
	private void ActivateCurrentScreen()
	{
		if (this.currentScreenIndex >= 0 && this.currentScreenIndex < this.newGameFlowScreens.Count)
		{
			NewGameFlowScreen newGameFlowScreen = Util.KInstantiateUI<NewGameFlowScreen>(this.newGameFlowScreens[this.currentScreenIndex].gameObject, base.transform.parent.gameObject, true);
			newGameFlowScreen.OnNavigateForward += this.Next;
			newGameFlowScreen.OnNavigateBackward += this.Previous;
			if (!newGameFlowScreen.IsActive() && !newGameFlowScreen.activateOnSpawn)
			{
				newGameFlowScreen.Activate();
			}
			this.currentScreen = newGameFlowScreen;
		}
	}

	// Token: 0x04003E57 RID: 15959
	public List<NewGameFlowScreen> newGameFlowScreens;

	// Token: 0x04003E58 RID: 15960
	private int currentScreenIndex = -1;

	// Token: 0x04003E59 RID: 15961
	private NewGameFlowScreen currentScreen;
}
