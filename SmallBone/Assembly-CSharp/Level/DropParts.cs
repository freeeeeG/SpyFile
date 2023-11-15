using System;
using System.Collections;
using UnityEngine;

namespace Level
{
	// Token: 0x0200046C RID: 1132
	public class DropParts : MonoBehaviour
	{
		// Token: 0x06001598 RID: 5528 RVA: 0x00043EB0 File Offset: 0x000420B0
		private void OnDestroy()
		{
			this._particleEffectInfo = null;
		}

		// Token: 0x06001599 RID: 5529 RVA: 0x00043EB9 File Offset: 0x000420B9
		public IEnumerator CSpawn(Vector2 position, Bounds bounds, Vector2 force)
		{
			yield return Chronometer.global.WaitForSeconds(this._delay);
			this._particleEffectInfo.Emit(position, bounds, force, true);
			yield break;
		}

		// Token: 0x0600159A RID: 5530 RVA: 0x00043EDD File Offset: 0x000420DD
		public IEnumerator CSpawn()
		{
			yield return Chronometer.global.WaitForSeconds(this._delay);
			this._particleEffectInfo.Emit(base.transform.position, this._range.bounds, Vector2.zero, true);
			yield break;
		}

		// Token: 0x040012DF RID: 4831
		[SerializeField]
		[FrameTime]
		private float _delay;

		// Token: 0x040012E0 RID: 4832
		[SerializeField]
		private Collider2D _range;

		// Token: 0x040012E1 RID: 4833
		[SerializeField]
		private ParticleEffectInfo _particleEffectInfo;
	}
}
