using System;
using Characters.Operations;
using Level;
using UnityEngine;

namespace Characters.Projectiles.Operations
{
	// Token: 0x02000796 RID: 1942
	public class SpawnBDRandomCharacter : Operation
	{
		// Token: 0x060027BB RID: 10171 RVA: 0x00077500 File Offset: 0x00075700
		public override void Run(IProjectile projectile)
		{
			if (this._characterPrefabs.Length == 0)
			{
				return;
			}
			Vector3 position = projectile.transform.position;
			for (int i = 0; i < this._repeatCount; i++)
			{
				float num = UnityEngine.Random.Range(-this._distribution, this._distribution);
				UnityEngine.Random.Range(-this._distribution, this._distribution);
				Vector3 position2 = position;
				position2.x += UnityEngine.Random.Range(-this._distribution, this._distribution);
				position2.y += UnityEngine.Random.Range(-this._distribution, this._distribution);
				Character character = UnityEngine.Object.Instantiate<Character>(this._characterPrefabs.Random<Character>(), position2, Quaternion.identity);
				character.ForceToLookAt((num < 0f) ? Character.LookingDirection.Left : Character.LookingDirection.Right);
				if (this._containInWave)
				{
					Map.Instance.waveContainer.Attach(character);
				}
				IBDCharacterSetting[] settings = this._settings;
				for (int j = 0; j < settings.Length; j++)
				{
					settings[j].ApplyTo(character);
				}
			}
		}

		// Token: 0x040021D4 RID: 8660
		[SerializeField]
		private Character[] _characterPrefabs;

		// Token: 0x040021D5 RID: 8661
		[SerializeField]
		[Range(0f, 10f)]
		private float _distribution;

		// Token: 0x040021D6 RID: 8662
		[SerializeField]
		[Range(1f, 10f)]
		private int _repeatCount;

		// Token: 0x040021D7 RID: 8663
		[SerializeField]
		private bool _containInWave = true;

		// Token: 0x040021D8 RID: 8664
		[SerializeReference]
		[SubclassSelector]
		private IBDCharacterSetting[] _settings;
	}
}
