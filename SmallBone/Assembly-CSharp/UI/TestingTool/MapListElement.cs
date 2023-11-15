using System;
using GameResources;
using Level;
using Services;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TestingTool
{
	// Token: 0x0200040A RID: 1034
	public class MapListElement : MonoBehaviour
	{
		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x0600138F RID: 5007 RVA: 0x0003B2DC File Offset: 0x000394DC
		// (set) Token: 0x06001390 RID: 5008 RVA: 0x0003B2E4 File Offset: 0x000394E4
		public Chapter.Type chapter { get; private set; }

		// Token: 0x06001391 RID: 5009 RVA: 0x0003B2F0 File Offset: 0x000394F0
		public void Set(Chapter.Type chapterType, string stageName, MapReference mapReference)
		{
			this.chapter = chapterType;
			this._text.text = stageName;
			PathNode pathNode = new PathNode(mapReference, MapReward.Type.None, Gate.Type.None);
			this._button.onClick.AddListener(delegate
			{
				Singleton<Service>.Instance.levelManager.currentChapter.ChangeMap(pathNode);
			});
		}

		// Token: 0x04001075 RID: 4213
		[SerializeField]
		private Button _button;

		// Token: 0x04001076 RID: 4214
		[SerializeField]
		private TMP_Text _text;
	}
}
