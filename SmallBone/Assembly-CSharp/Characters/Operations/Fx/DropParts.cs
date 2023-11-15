using System;
using UnityEngine;

namespace Characters.Operations.Fx
{
	// Token: 0x02000F0A RID: 3850
	public class DropParts : CharacterOperation
	{
		// Token: 0x06004B3B RID: 19259 RVA: 0x000DD784 File Offset: 0x000DB984
		protected override void OnDestroy()
		{
			base.OnDestroy();
			this._particleEffectInfo = null;
		}

		// Token: 0x06004B3C RID: 19260 RVA: 0x000DD794 File Offset: 0x000DB994
		public override void Run(Character owner)
		{
			if (this._pickRandomOne)
			{
				this._particleEffectInfo.EmitRandom(this._spawnPoint.position, this._range.bounds, this._direction * this._power, this._interpolation);
				return;
			}
			this._particleEffectInfo.Emit(this._spawnPoint.position, this._range.bounds, this._direction * this._power, this._interpolation);
		}

		// Token: 0x04003A63 RID: 14947
		[SerializeField]
		private Transform _spawnPoint;

		// Token: 0x04003A64 RID: 14948
		[SerializeField]
		private Collider2D _range;

		// Token: 0x04003A65 RID: 14949
		[SerializeField]
		private ParticleEffectInfo _particleEffectInfo;

		// Token: 0x04003A66 RID: 14950
		[SerializeField]
		private Vector2 _direction;

		// Token: 0x04003A67 RID: 14951
		[SerializeField]
		private float _power = 3f;

		// Token: 0x04003A68 RID: 14952
		[SerializeField]
		private bool _interpolation;

		// Token: 0x04003A69 RID: 14953
		[SerializeField]
		private bool _pickRandomOne;
	}
}
