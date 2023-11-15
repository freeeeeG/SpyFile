using System;
using Characters;
using Characters.Projectiles;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x02000670 RID: 1648
	public class PlateTable : MonoBehaviour
	{
		// Token: 0x06002100 RID: 8448 RVA: 0x000637CE File Offset: 0x000619CE
		private void Awake()
		{
			this._prop.onDidHit += new Prop.DidHitDelegate(this.OnPropDidHit);
		}

		// Token: 0x06002101 RID: 8449 RVA: 0x000637E8 File Offset: 0x000619E8
		private void OnPropDidHit(Character from, in Damage damage, Vector2 force)
		{
			if (this._prop.phase == 0)
			{
				return;
			}
			this._prop.onDidHit -= new Prop.DidHitDelegate(this.OnPropDidHit);
			float num = Mathf.Atan2(force.y, force.x) * 57.29578f;
			for (int i = 0; i < this._quantity; i++)
			{
				float speedMultiplier = Mathf.Clamp(force.magnitude, 2f, 6f);
				Vector2 v = MMMaths.RandomPointWithinBounds(this._fireRange.bounds);
				this._projectile.reusable.Spawn(v, true).GetComponent<Projectile>().Fire(from, this._damage, num + UnityEngine.Random.Range(-10f, 10f), false, false, speedMultiplier, null, 0f);
			}
		}

		// Token: 0x04001C15 RID: 7189
		[SerializeField]
		private Prop _prop;

		// Token: 0x04001C16 RID: 7190
		[SerializeField]
		private Projectile _projectile;

		// Token: 0x04001C17 RID: 7191
		[SerializeField]
		private float _damage;

		// Token: 0x04001C18 RID: 7192
		[SerializeField]
		private int _quantity;

		// Token: 0x04001C19 RID: 7193
		[SerializeField]
		private Collider2D _fireRange;
	}
}
