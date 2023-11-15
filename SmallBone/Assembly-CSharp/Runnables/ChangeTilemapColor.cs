using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Runnables
{
	// Token: 0x020002EE RID: 750
	public class ChangeTilemapColor : CRunnable
	{
		// Token: 0x06000ECA RID: 3786 RVA: 0x0002DDD9 File Offset: 0x0002BFD9
		public override IEnumerator CRun()
		{
			Color startColor = this._tilemap.color;
			float elapsed = 0f;
			while (elapsed < this._curve.duration)
			{
				elapsed += Chronometer.global.deltaTime;
				this._tilemap.color = Color.Lerp(startColor, this._color, this._curve.Evaluate(elapsed));
				yield return null;
			}
			this._tilemap.color = this._color;
			yield break;
		}

		// Token: 0x04000C3B RID: 3131
		[SerializeField]
		private Tilemap _tilemap;

		// Token: 0x04000C3C RID: 3132
		[SerializeField]
		private Color _color;

		// Token: 0x04000C3D RID: 3133
		[SerializeField]
		private Curve _curve;
	}
}
