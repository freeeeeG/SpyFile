using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000149 RID: 329
public class UI_BuildTowerButtonController : MonoBehaviour
{
	// Token: 0x060008A7 RID: 2215 RVA: 0x000214FB File Offset: 0x0001F6FB
	private void OnEnable()
	{
		EventMgr.Register<int, int>(eGameEvents.OnRoundStart, new Action<int, int>(this.OnRoundStart));
		EventMgr.Register(eGameEvents.OnPlayerVictory, new Action(this.OnPlayerVictory));
	}

	// Token: 0x060008A8 RID: 2216 RVA: 0x0002152D File Offset: 0x0001F72D
	private void OnDisable()
	{
		EventMgr.Remove<int, int>(eGameEvents.OnRoundStart, new Action<int, int>(this.OnRoundStart));
		EventMgr.Remove(eGameEvents.OnPlayerVictory, new Action(this.OnPlayerVictory));
	}

	// Token: 0x060008A9 RID: 2217 RVA: 0x0002155F File Offset: 0x0001F75F
	private void OnRoundStart(int index, int totalRound)
	{
		if (index == 1)
		{
			this.animator.SetBool("isOn", true);
			base.StartCoroutine(this.CR_ShowButtons());
		}
	}

	// Token: 0x060008AA RID: 2218 RVA: 0x00021583 File Offset: 0x0001F783
	private void OnPlayerVictory()
	{
		this.animator.SetBool("isOn", false);
	}

	// Token: 0x060008AB RID: 2219 RVA: 0x00021596 File Offset: 0x0001F796
	private IEnumerator CR_ShowButtons()
	{
		int num;
		for (int i = 0; i < this.list_Buttons.Count; i = num + 1)
		{
			this.list_Buttons[i].ToggleButton(true);
			yield return new WaitForSeconds(0.03f);
			num = i;
		}
		yield break;
	}

	// Token: 0x04000702 RID: 1794
	[SerializeField]
	private Animator animator;

	// Token: 0x04000703 RID: 1795
	[SerializeField]
	private List<UI_BuildTowerButton> list_Buttons;
}
