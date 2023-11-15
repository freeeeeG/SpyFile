using System;
using Level;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E1D RID: 3613
	public sealed class Instantiate : CharacterOperation
	{
		// Token: 0x06004821 RID: 18465 RVA: 0x000D1D99 File Offset: 0x000CFF99
		public override void Run(Character owner)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this._prefab, this._spawnPosition.position, Quaternion.identity);
			gameObject.transform.SetParent(Map.Instance.transform);
			UnityEngine.Object.Destroy(gameObject, this._lifeTime);
		}

		// Token: 0x04003736 RID: 14134
		[SerializeField]
		private GameObject _prefab;

		// Token: 0x04003737 RID: 14135
		[SerializeField]
		private Transform _spawnPosition;

		// Token: 0x04003738 RID: 14136
		[SerializeField]
		private float _lifeTime;
	}
}
