using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200000A RID: 10
public class LangDropDown : MonoBehaviour
{
	// Token: 0x06000064 RID: 100 RVA: 0x00003DC8 File Offset: 0x00001FC8
	private void Start()
	{
		this.drp = base.GetComponent<Dropdown>();
		int value = 0;
		string @string = PlayerPrefs.GetString("_language");
		for (int i = 0; i < this.myLangs.Length; i++)
		{
			if (@string == this.myLangs[i])
			{
				value = i;
			}
		}
		this.drp.value = value;
		this.drp.onValueChanged.AddListener(delegate(int <p0>)
		{
			this.index = this.drp.value;
			PlayerPrefs.SetString("_language", this.myLangs[this.index]);
			Debug.Log("language changed to " + this.myLangs[this.index]);
			this.ApplyLanguageChanges();
		});
	}

	// Token: 0x06000065 RID: 101 RVA: 0x00003E3B File Offset: 0x0000203B
	private void ApplyLanguageChanges()
	{
		Singleton<Game>.Instance.ReloadScene();
		GameMultiLang.Instance.LoadLanguage();
		Singleton<TipsManager>.Instance.UpdateTranslators();
	}

	// Token: 0x04000036 RID: 54
	[SerializeField]
	private string[] myLangs;

	// Token: 0x04000037 RID: 55
	private Dropdown drp;

	// Token: 0x04000038 RID: 56
	private int index;
}
