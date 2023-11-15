using System;
using TMPro;
using UnityEngine;

// Token: 0x0200000D RID: 13
[RequireComponent(typeof(TMP_Text))]
public class TextMeshProTranslator : MonoBehaviour
{
	// Token: 0x0600006D RID: 109 RVA: 0x00003F03 File Offset: 0x00002103
	private void Start()
	{
		base.GetComponent<TMP_Text>().text = GameMultiLang.GetTraduction(this.key);
	}

	// Token: 0x0400003B RID: 59
	[Tooltip("enter one of the keys that you specify in your (txt) file for all languages.\n\n# for example: [HOME=home]\n# the key here is [HOME]")]
	[Header("Enter your word key here.")]
	[SerializeField]
	private string key;
}
