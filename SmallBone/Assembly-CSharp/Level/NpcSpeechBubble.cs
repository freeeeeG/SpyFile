using System;
using System.Collections.Generic;
using GameResources;
using UnityEngine;
using UnityEngine.UI;

namespace Level
{
	// Token: 0x0200054C RID: 1356
	public class NpcSpeechBubble : MonoBehaviour
	{
		// Token: 0x06001AF0 RID: 6896 RVA: 0x00054124 File Offset: 0x00052324
		public void Initialize(string key)
		{
			int num = 1;
			string item;
			while (Localization.TryGetLocalizedString(string.Format("{0}_bubble{1}", key, num), out item))
			{
				num++;
				this._scripts.Add(item);
			}
		}

		// Token: 0x06001AF1 RID: 6897 RVA: 0x0005415F File Offset: 0x0005235F
		public void Show()
		{
			this._text.text = this._scripts.Random<string>();
			base.gameObject.SetActive(true);
		}

		// Token: 0x06001AF2 RID: 6898 RVA: 0x000075E7 File Offset: 0x000057E7
		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x04001732 RID: 5938
		[SerializeField]
		private Text _text;

		// Token: 0x04001733 RID: 5939
		private readonly List<string> _scripts = new List<string>();
	}
}
