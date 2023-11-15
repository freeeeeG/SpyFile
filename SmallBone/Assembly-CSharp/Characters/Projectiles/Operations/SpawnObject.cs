using System;
using Hardmode;
using Level;
using Singletons;
using UnityEngine;

namespace Characters.Projectiles.Operations
{
	// Token: 0x02000798 RID: 1944
	public class SpawnObject : HitOperation
	{
		// Token: 0x060027C1 RID: 10177 RVA: 0x0007776C File Offset: 0x0007596C
		public override void Run(IProjectile projectile, RaycastHit2D raycastHit)
		{
			if (Singleton<HardmodeManager>.Instance.hardmode)
			{
				if (this._objectsInHardmode != null && this._objectsInHardmode.Length != 0)
				{
					this._object = this._objectsInHardmode.Random<GameObject>();
				}
			}
			else if (this._objects != null && this._objects.Length != 0)
			{
				this._object = this._objects.Random<GameObject>();
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this._object, raycastHit.point, Quaternion.identity, Map.Instance.transform);
			Character component = gameObject.GetComponent<Character>();
			if (component != null)
			{
				Map.Instance.waveContainer.Attach(component);
			}
			if (this._lifeTime != 0f)
			{
				UnityEngine.Object.Destroy(gameObject, this._lifeTime);
			}
		}

		// Token: 0x040021DE RID: 8670
		[SerializeField]
		private GameObject[] _objectsInHardmode;

		// Token: 0x040021DF RID: 8671
		[SerializeField]
		private GameObject[] _objects;

		// Token: 0x040021E0 RID: 8672
		[SerializeField]
		private GameObject _object;

		// Token: 0x040021E1 RID: 8673
		[SerializeField]
		private float _lifeTime;
	}
}
