using System;
using GameResources;
using Scenes;
using UnityEngine;

// Token: 0x020000AC RID: 172
public class StageInfoDisplay : MonoBehaviour
{
	// Token: 0x0600036C RID: 876 RVA: 0x0000CBC0 File Offset: 0x0000ADC0
	private void Start()
	{
		Scene<GameBase>.instance.uiManager.stageName.Show(Localization.GetLocalizedString(this.nameKey), Localization.GetLocalizedString(this.stageNumber), Localization.GetLocalizedString(this.subNameKey));
	}

	// Token: 0x040002C8 RID: 712
	[SerializeField]
	private string nameKey;

	// Token: 0x040002C9 RID: 713
	[SerializeField]
	private string stageNumber;

	// Token: 0x040002CA RID: 714
	[SerializeField]
	private string subNameKey;
}
