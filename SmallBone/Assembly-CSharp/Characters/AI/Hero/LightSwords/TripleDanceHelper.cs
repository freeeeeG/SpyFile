using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.AI.Hero.LightSwords
{
	// Token: 0x0200128A RID: 4746
	public class TripleDanceHelper : MonoBehaviour
	{
		// Token: 0x06005E28 RID: 24104 RVA: 0x00114DB8 File Offset: 0x00112FB8
		private void Start()
		{
			this._sword = new TripleDanceHelper.TripleDanceSword(this._pool.Get());
		}

		// Token: 0x06005E29 RID: 24105 RVA: 0x00114DD0 File Offset: 0x00112FD0
		public void Fire(Transform source)
		{
			Vector2 destination = this.CalculateDestination(source.position, this._left);
			this._sword.left.Fire(this._owner, source.position, destination);
			destination = this.CalculateDestination(source.position, this._center);
			this._sword.center.Fire(this._owner, source.position, destination);
			destination = this.CalculateDestination(source.position, this._right);
			this._sword.right.Fire(this._owner, source.position, destination);
		}

		// Token: 0x06005E2A RID: 24106 RVA: 0x00114E8B File Offset: 0x0011308B
		public ValueTuple<LightSword, LightSword, LightSword> GetStuck()
		{
			return new ValueTuple<LightSword, LightSword, LightSword>(this._sword.left, this._sword.center, this._sword.right);
		}

		// Token: 0x06005E2B RID: 24107 RVA: 0x00114EB4 File Offset: 0x001130B4
		private Vector2 CalculateDestination(Vector2 source, float degree)
		{
			Vector3 v = Quaternion.Euler(0f, 0f, degree) * Vector2.right;
			return Physics2D.Raycast(source, v, float.PositiveInfinity, Layers.groundMask).point;
		}

		// Token: 0x04004BA6 RID: 19366
		[SerializeField]
		private Character _owner;

		// Token: 0x04004BA7 RID: 19367
		[SerializeField]
		private LightSwordPool _pool;

		// Token: 0x04004BA8 RID: 19368
		[Range(180f, 360f)]
		[SerializeField]
		private float _left;

		// Token: 0x04004BA9 RID: 19369
		[Range(180f, 360f)]
		[SerializeField]
		private float _center = 270f;

		// Token: 0x04004BAA RID: 19370
		[Range(180f, 360f)]
		[SerializeField]
		private float _right;

		// Token: 0x04004BAB RID: 19371
		private TripleDanceHelper.TripleDanceSword _sword;

		// Token: 0x0200128B RID: 4747
		private class TripleDanceSword
		{
			// Token: 0x06005E2D RID: 24109 RVA: 0x00114F17 File Offset: 0x00113117
			internal TripleDanceSword(List<LightSword> swords)
			{
				if (swords == null)
				{
					return;
				}
				if (swords.Count < 3)
				{
					return;
				}
				this.left = swords[0];
				this.center = swords[1];
				this.right = swords[2];
			}

			// Token: 0x04004BAC RID: 19372
			internal LightSword left;

			// Token: 0x04004BAD RID: 19373
			internal LightSword center;

			// Token: 0x04004BAE RID: 19374
			internal LightSword right;
		}
	}
}
