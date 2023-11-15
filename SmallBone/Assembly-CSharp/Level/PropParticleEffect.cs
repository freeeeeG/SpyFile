using System;
using UnityEngine;

namespace Level
{
	// Token: 0x02000510 RID: 1296
	public class PropParticleEffect : MonoBehaviour
	{
		// Token: 0x06001990 RID: 6544 RVA: 0x00050274 File Offset: 0x0004E474
		public void Spawn(Vector2 spawnPoint, Vector2 force)
		{
			float d = 1f;
			if (this._relativeScaleToTargetSize)
			{
				Vector3 size = this._prop.collider.bounds.size;
				d = Mathf.Min(size.x, size.y);
			}
			if (this._effect != null)
			{
				this._effect.Spawn(spawnPoint, true).transform.localScale = Vector3.one * d;
			}
			ParticleEffectInfo particleInfo = this._particleInfo;
			if (particleInfo == null)
			{
				return;
			}
			particleInfo.Emit(this._prop.transform.position, this._prop.collider.bounds, force, true);
		}

		// Token: 0x04001657 RID: 5719
		[SerializeField]
		[GetComponent]
		private Prop _prop;

		// Token: 0x04001658 RID: 5720
		[SerializeField]
		private PoolObject _effect;

		// Token: 0x04001659 RID: 5721
		[SerializeField]
		private bool _relativeScaleToTargetSize = true;

		// Token: 0x0400165A RID: 5722
		[SerializeField]
		private ParticleEffectInfo _particleInfo;
	}
}
