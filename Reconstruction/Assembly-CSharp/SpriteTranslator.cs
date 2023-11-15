using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200000B RID: 11
[RequireComponent(typeof(Image))]
public class SpriteTranslator : MonoBehaviour
{
	// Token: 0x06000068 RID: 104 RVA: 0x00003EBB File Offset: 0x000020BB
	private void Start()
	{
		base.GetComponent<Image>().sprite = GameMultiLang.GetSprite(this.key);
	}

	// Token: 0x04000039 RID: 57
	[SerializeField]
	private string key;
}
