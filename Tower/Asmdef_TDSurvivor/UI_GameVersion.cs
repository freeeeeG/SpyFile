using System;
using TMPro;
using UnityEngine;

// Token: 0x02000167 RID: 359
public class UI_GameVersion : MonoBehaviour
{
	// Token: 0x06000971 RID: 2417 RVA: 0x00023A84 File Offset: 0x00021C84
	private void Start()
	{
		if (this.gameVersionText == null)
		{
			Debug.LogError("未設置TextMeshProUGUI物件的引用。請將引用拖放到UI_GameVersion腳本的Inspector上。");
			return;
		}
		string version = Application.version;
		this.gameVersionText.text = "v" + version;
	}

	// Token: 0x04000775 RID: 1909
	public TextMeshProUGUI gameVersionText;
}
