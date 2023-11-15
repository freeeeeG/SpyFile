using System;
using TMPro;
using UnityEngine;

namespace EndingCredit
{
	// Token: 0x02000199 RID: 409
	public class CreditText : MonoBehaviour
	{
		// Token: 0x060008DB RID: 2267 RVA: 0x0001936C File Offset: 0x0001756C
		public void Initialize()
		{
			string[] array = this._textAsset.text.Split(new char[]
			{
				'\n'
			});
			TextMeshProUGUI[] componentsInChildren = this._background.GetComponentsInChildren<TextMeshProUGUI>(true);
			if (componentsInChildren.Length != array.Length)
			{
				Debug.LogError(string.Format("텍스트가 들어갈 자리가 많거나 부족합니다. 확인해주세요. {0} : {1}", componentsInChildren.Length, array.Length));
				return;
			}
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].text = array[i];
			}
		}

		// Token: 0x04000710 RID: 1808
		[SerializeField]
		private TextAsset _textAsset;

		// Token: 0x04000711 RID: 1809
		[SerializeField]
		private GameObject _background;
	}
}
