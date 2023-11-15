using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200027F RID: 639
public class UIEndlessMode : MonoBehaviour
{
	// Token: 0x06000FDE RID: 4062 RVA: 0x0002A7F0 File Offset: 0x000289F0
	public void Initialize()
	{
		this.endlessUnlockText.gameObject.SetActive(Singleton<LevelManager>.Instance.PassDiifcutly <= 4);
		this.mainArea.SetActive(Singleton<LevelManager>.Instance.PassDiifcutly > 4);
		this.endlessCustom.Initialize();
		this.endlessWeekly.Initialize();
		this.endlessCustom.gameObject.SetActive(false);
		this.endlessWeekly.gameObject.SetActive(true);
	}

	// Token: 0x04000833 RID: 2099
	[SerializeField]
	private Text endlessUnlockText;

	// Token: 0x04000834 RID: 2100
	[SerializeField]
	private GameObject mainArea;

	// Token: 0x04000835 RID: 2101
	[SerializeField]
	private EndlessCustom endlessCustom;

	// Token: 0x04000836 RID: 2102
	[SerializeField]
	private EndlessWeekly endlessWeekly;
}
