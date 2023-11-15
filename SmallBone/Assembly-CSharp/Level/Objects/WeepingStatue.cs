using System;
using Services;
using Singletons;
using UnityEngine;

namespace Level.Objects
{
	// Token: 0x02000576 RID: 1398
	public class WeepingStatue : MonoBehaviour
	{
		// Token: 0x06001B73 RID: 7027 RVA: 0x00055480 File Offset: 0x00053680
		private void OnEnable()
		{
			this._forEditor.gameObject.SetActive(false);
			if (Singleton<Service>.Instance.levelManager.currentChapter.stageIndex == 0)
			{
				this._stage1.gameObject.SetActive(true);
				this._stage2.gameObject.SetActive(false);
				return;
			}
			this._stage2.gameObject.SetActive(true);
			this._stage1.gameObject.SetActive(false);
		}

		// Token: 0x0400179C RID: 6044
		[SerializeField]
		private GameObject _forEditor;

		// Token: 0x0400179D RID: 6045
		[SerializeField]
		private GameObject _stage1;

		// Token: 0x0400179E RID: 6046
		[SerializeField]
		private GameObject _stage2;
	}
}
