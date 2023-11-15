using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Token: 0x02000273 RID: 627
public class GirlWords : MonoBehaviour
{
	// Token: 0x06000F93 RID: 3987 RVA: 0x00029964 File Offset: 0x00027B64
	private void Start()
	{
		this.OnGirlClick();
	}

	// Token: 0x06000F94 RID: 3988 RVA: 0x0002996C File Offset: 0x00027B6C
	public void OnGirlClick()
	{
		int num;
		for (num = this.currentID; num == this.currentID; num = Random.Range(0, this.words.Length))
		{
		}
		this.currentID = num;
		base.StartCoroutine(this.TypeSentence(GameMultiLang.GetTraduction(this.words[this.currentID])));
	}

	// Token: 0x06000F95 RID: 3989 RVA: 0x000299C0 File Offset: 0x00027BC0
	private IEnumerator TypeSentence(string word)
	{
		this.word_Txt.text = word;
		this.word_Txt.maxVisibleCharacters = 0;
		this.word_Txt.ForceMeshUpdate(false, false);
		TMP_TextInfo textInfo = this.word_Txt.textInfo;
		for (int i = 0; i < textInfo.characterCount; i++)
		{
			this.SetCharacterAlpha(i, 0);
		}
		float timer = 0f;
		float interval = 0.03f;
		while (this.word_Txt.maxVisibleCharacters < textInfo.characterCount)
		{
			timer += Time.deltaTime;
			if (timer >= interval)
			{
				timer = 0f;
				TextMeshProUGUI textMeshProUGUI = this.word_Txt;
				int maxVisibleCharacters = textMeshProUGUI.maxVisibleCharacters;
				textMeshProUGUI.maxVisibleCharacters = maxVisibleCharacters + 1;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000F96 RID: 3990 RVA: 0x000299D8 File Offset: 0x00027BD8
	private void SetCharacterAlpha(int index, byte alpha)
	{
		int materialReferenceIndex = this.word_Txt.textInfo.characterInfo[index].materialReferenceIndex;
		Color32[] colors = this.word_Txt.textInfo.meshInfo[materialReferenceIndex].colors32;
		int vertexIndex = this.word_Txt.textInfo.characterInfo[index].vertexIndex;
		colors[vertexIndex].a = alpha;
		colors[vertexIndex + 1].a = alpha;
		colors[vertexIndex + 2].a = alpha;
		colors[vertexIndex + 3].a = alpha;
	}

	// Token: 0x040007F5 RID: 2037
	[SerializeField]
	private TextMeshProUGUI word_Txt;

	// Token: 0x040007F6 RID: 2038
	[SerializeField]
	private string[] words;

	// Token: 0x040007F7 RID: 2039
	private int currentID;
}
