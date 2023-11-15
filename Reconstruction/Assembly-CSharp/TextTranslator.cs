using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200000C RID: 12
[RequireComponent(typeof(Text))]
public class TextTranslator : MonoBehaviour
{
	// Token: 0x0600006A RID: 106 RVA: 0x00003EDB File Offset: 0x000020DB
	private void Start()
	{
		this.UpdateTrans();
	}

	// Token: 0x0600006B RID: 107 RVA: 0x00003EE3 File Offset: 0x000020E3
	public void UpdateTrans()
	{
		base.GetComponent<Text>().text = GameMultiLang.GetTraduction(this.key);
	}

	// Token: 0x0400003A RID: 58
	[Tooltip("enter one of the keys that you specify in your (txt) file for all languages.\n\n# for example: [HOME=home]\n# the key here is [HOME]")]
	[Header("Enter your word key here.")]
	[SerializeField]
	private string key;
}
