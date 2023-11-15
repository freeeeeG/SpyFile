using System;
using Characters.AI;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Projectiles.Operations
{
	// Token: 0x02000799 RID: 1945
	public class SpawnRandomEnemy : Operation
	{
		// Token: 0x060027C3 RID: 10179 RVA: 0x0007782C File Offset: 0x00075A2C
		public override void Run(IProjectile projectile)
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			Vector3 position = projectile.transform.position;
			for (int i = 0; i < this._repeatCount; i++)
			{
				float num = UnityEngine.Random.Range(-this._distribution, this._distribution);
				UnityEngine.Random.Range(-this._distribution, this._distribution);
				Vector3 position2 = position;
				position2.x += UnityEngine.Random.Range(-this._distribution, this._distribution);
				position2.y += UnityEngine.Random.Range(-this._distribution, this._distribution);
				Character character = UnityEngine.Object.Instantiate<Character>(this._characters.Random<Character>(), position2, Quaternion.identity);
				character.ForceToLookAt((num < 0f) ? Character.LookingDirection.Left : Character.LookingDirection.Right);
				if (this._setPlayerAsTarget)
				{
					character.GetComponentInChildren<AIController>().target = player;
				}
				if (this._containInWave)
				{
					Map.Instance.waveContainer.Attach(character);
				}
			}
		}

		// Token: 0x040021E2 RID: 8674
		[SerializeField]
		private Character[] _characters;

		// Token: 0x040021E3 RID: 8675
		[SerializeField]
		private bool _setPlayerAsTarget;

		// Token: 0x040021E4 RID: 8676
		[Range(0f, 10f)]
		[SerializeField]
		private float _distribution;

		// Token: 0x040021E5 RID: 8677
		[Range(1f, 10f)]
		[SerializeField]
		private int _repeatCount;

		// Token: 0x040021E6 RID: 8678
		[SerializeField]
		private bool _containInWave = true;
	}
}
