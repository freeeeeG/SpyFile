using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C94 RID: 3220
public class ClippyPanel : KScreen
{
	// Token: 0x06006697 RID: 26263 RVA: 0x00264084 File Offset: 0x00262284
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06006698 RID: 26264 RVA: 0x0026408C File Offset: 0x0026228C
	protected override void OnActivate()
	{
		base.OnActivate();
		SpeedControlScreen.Instance.Pause(true, false);
		Game.Instance.Trigger(1634669191, null);
	}

	// Token: 0x06006699 RID: 26265 RVA: 0x002640B0 File Offset: 0x002622B0
	public void OnOk()
	{
		SpeedControlScreen.Instance.Unpause(true);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x040046C0 RID: 18112
	public Text title;

	// Token: 0x040046C1 RID: 18113
	public Text detailText;

	// Token: 0x040046C2 RID: 18114
	public Text flavorText;

	// Token: 0x040046C3 RID: 18115
	public Image topicIcon;

	// Token: 0x040046C4 RID: 18116
	private KButton okButton;
}
