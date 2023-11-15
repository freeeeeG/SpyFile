using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200000E RID: 14
public class TMProLangDropDown : MonoBehaviour
{
	// Token: 0x0600006F RID: 111 RVA: 0x00003F24 File Offset: 0x00002124
	private void Awake()
	{
		this.drp = base.GetComponent<TMP_Dropdown>();
		int @int = PlayerPrefs.GetInt("_language_index", 0);
		this.drp.value = @int;
		this.drp.onValueChanged.AddListener(delegate(int <p0>)
		{
			this.index = this.drp.value;
			PlayerPrefs.SetInt("_language_index", this.index);
			PlayerPrefs.SetString("_language", this.myLangs[this.index]);
			Debug.Log("language changed to " + this.myLangs[this.index]);
			this.ApplyLanguageChanges();
		});
	}

	// Token: 0x06000070 RID: 112 RVA: 0x00003F71 File Offset: 0x00002171
	private void ApplyLanguageChanges()
	{
		SceneManager.LoadScene(0);
	}

	// Token: 0x06000071 RID: 113 RVA: 0x00003F79 File Offset: 0x00002179
	private void OnDestroy()
	{
		this.drp.onValueChanged.RemoveAllListeners();
	}

	// Token: 0x0400003C RID: 60
	[SerializeField]
	private string[] myLangs;

	// Token: 0x0400003D RID: 61
	private TMP_Dropdown drp;

	// Token: 0x0400003E RID: 62
	private int index;
}
