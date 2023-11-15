using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SkulStories
{
	// Token: 0x020000FE RID: 254
	public class FadeInOut : Sequence
	{
		// Token: 0x060004DB RID: 1243 RVA: 0x0000F6D2 File Offset: 0x0000D8D2
		private void Start()
		{
			this._image = this._narration.blackScreen;
			this._originColor = this._image.color;
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x0000F6F6 File Offset: 0x0000D8F6
		public override IEnumerator CRun()
		{
			Color startColor = this._image.color;
			Color different = this._target - this._image.color;
			float elapsed = 0f;
			while (elapsed < this._duration)
			{
				elapsed += Chronometer.global.deltaTime;
				this._image.color = startColor + different * (elapsed / this._duration);
				yield return null;
			}
			this._image.color = this._target;
			yield break;
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0000F705 File Offset: 0x0000D905
		public void OnDisable()
		{
			this._image.color = this._originColor;
		}

		// Token: 0x040003B9 RID: 953
		[SerializeField]
		private Color _target;

		// Token: 0x040003BA RID: 954
		[SerializeField]
		private float _duration;

		// Token: 0x040003BB RID: 955
		private Color _originColor;

		// Token: 0x040003BC RID: 956
		private Image _image;
	}
}
