using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000286 RID: 646
public class GameSetting : MonoBehaviour
{
	// Token: 0x06001002 RID: 4098 RVA: 0x0002AE9E File Offset: 0x0002909E
	public void ShowSetting()
	{
		this.showDmgTog.isOn = StaticData.ShowDamage;
		this.showIntensifyTog.isOn = StaticData.ShowIntensify;
	}

	// Token: 0x06001003 RID: 4099 RVA: 0x0002AEC0 File Offset: 0x000290C0
	public void ShowJumpDamage(bool value)
	{
		StaticData.ShowDamage = value;
	}

	// Token: 0x06001004 RID: 4100 RVA: 0x0002AEC8 File Offset: 0x000290C8
	public void ShowIntensify(bool value)
	{
		StaticData.ShowIntensify = value;
	}

	// Token: 0x0400084C RID: 2124
	[SerializeField]
	private Toggle showDmgTog;

	// Token: 0x0400084D RID: 2125
	[SerializeField]
	private Toggle showIntensifyTog;
}
