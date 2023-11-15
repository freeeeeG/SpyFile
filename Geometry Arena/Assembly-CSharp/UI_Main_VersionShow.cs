using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000BD RID: 189
public class UI_Main_VersionShow : MonoBehaviour
{
	// Token: 0x06000689 RID: 1673 RVA: 0x000051D0 File Offset: 0x000033D0
	private void OnEnable()
	{
	}

	// Token: 0x0600068A RID: 1674 RVA: 0x000253A0 File Offset: 0x000235A0
	private void Start()
	{
		if (this.text == null)
		{
			this.text = base.gameObject.GetComponent<Text>();
		}
		if (!GameParameters.Inst.ifDemo)
		{
			string versionName = LanguageText.Inst.asset_UpdateLog.VersionName;
			this.text.text = "v" + versionName;
			Debug.Log(string.Format("存档版本{0},当前版本{1}", GameData.SaveFile.version, GameParameters.Inst.version));
			Debug.Log("当前版本名为 v" + versionName);
			return;
		}
		this.text.text = MyTool.GetVersion(GameParameters.Inst.version) + " (DEMO)";
	}

	// Token: 0x0600068B RID: 1675 RVA: 0x000051D0 File Offset: 0x000033D0
	private void Update()
	{
	}

	// Token: 0x0400056B RID: 1387
	[SerializeField]
	private Text text;
}
