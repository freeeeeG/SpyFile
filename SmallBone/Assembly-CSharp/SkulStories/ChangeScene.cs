using System;
using System.Collections;
using Scenes;
using UnityEngine;

namespace SkulStories
{
	// Token: 0x020000F7 RID: 247
	public class ChangeScene : Sequence
	{
		// Token: 0x060004C3 RID: 1219 RVA: 0x0000F45A File Offset: 0x0000D65A
		private void Start()
		{
			this._narrationScene = Scene<GameBase>.instance.uiManager.narrationScene;
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0000F471 File Offset: 0x0000D671
		public override IEnumerator CRun()
		{
			if (this._narration.skipped)
			{
				yield break;
			}
			yield return this.CWaitForTime(this._delay);
			if (this._top)
			{
				this._narrationScene.scene.rectTransform.SetAsLastSibling();
			}
			this._narrationScene.SetPivot(this._pivot);
			this._narrationScene.Change(this._sequence, this._overlay);
			yield return this._narrationScene.CFadeIn(this._speed);
			yield break;
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0000F480 File Offset: 0x0000D680
		private IEnumerator CWaitForTime(float length)
		{
			float elapsed = 0f;
			while (length > elapsed)
			{
				elapsed += Chronometer.global.deltaTime;
				yield return null;
				if (this._narration.skipped || !this._narration.sceneVisible)
				{
					break;
				}
			}
			this._delay = 0f;
			yield break;
		}

		// Token: 0x040003A8 RID: 936
		[SerializeField]
		private Sprite _sequence;

		// Token: 0x040003A9 RID: 937
		[SerializeField]
		private float _speed;

		// Token: 0x040003AA RID: 938
		[SerializeField]
		private float _delay;

		// Token: 0x040003AB RID: 939
		[SerializeField]
		private Vector2 _pivot = new Vector2(0f, 1f);

		// Token: 0x040003AC RID: 940
		[SerializeField]
		private bool _overlay;

		// Token: 0x040003AD RID: 941
		[SerializeField]
		private bool _top;

		// Token: 0x040003AE RID: 942
		private NarrationScene _narrationScene;
	}
}
