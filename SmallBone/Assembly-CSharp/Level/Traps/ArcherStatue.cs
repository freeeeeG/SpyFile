using System;
using System.Collections;
using Characters;
using Characters.Operations.Attack;
using Characters.Operations.Fx;
using PhysicsUtils;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x0200064E RID: 1614
	public class ArcherStatue : MonoBehaviour
	{
		// Token: 0x06002070 RID: 8304 RVA: 0x00062363 File Offset: 0x00060563
		private void Awake()
		{
			this._fireProjectile.Initialize();
			this._signSound.Initialize();
			this._fireSound.Initialize();
			base.StartCoroutine(this.CAttack());
		}

		// Token: 0x06002071 RID: 8305 RVA: 0x00062393 File Offset: 0x00060593
		private IEnumerator CAttack()
		{
			for (;;)
			{
				yield return Chronometer.global.WaitForSeconds(0.1f);
				this.FindPlayer();
				if (this._overlapper.results.Count != 0)
				{
					this._signSound.Run(this._character);
					this._sign.SetActive(true);
					yield return Chronometer.global.WaitForSeconds(this._signLength);
					this._sign.SetActive(false);
					this._fireProjectile.Run(this._character);
					this._fireSound.Run(this._character);
					yield return Chronometer.global.WaitForSeconds(this._interval);
				}
			}
			yield break;
		}

		// Token: 0x06002072 RID: 8306 RVA: 0x000623A4 File Offset: 0x000605A4
		private void FindPlayer()
		{
			this._range.enabled = true;
			this._overlapper.contactFilter.SetLayerMask(512);
			this._overlapper.OverlapCollider(this._range);
			this._range.enabled = false;
		}

		// Token: 0x04001B80 RID: 7040
		[SerializeField]
		private Character _character;

		// Token: 0x04001B81 RID: 7041
		[SerializeField]
		private float _signLength = 1f;

		// Token: 0x04001B82 RID: 7042
		[SerializeField]
		private float _interval = 3f;

		// Token: 0x04001B83 RID: 7043
		[SerializeField]
		private Collider2D _range;

		// Token: 0x04001B84 RID: 7044
		[SerializeField]
		private readonly NonAllocOverlapper _overlapper = new NonAllocOverlapper(1);

		// Token: 0x04001B85 RID: 7045
		[SerializeField]
		private FireProjectile _fireProjectile;

		// Token: 0x04001B86 RID: 7046
		[SerializeField]
		private PlaySound _signSound;

		// Token: 0x04001B87 RID: 7047
		[SerializeField]
		private PlaySound _fireSound;

		// Token: 0x04001B88 RID: 7048
		[SerializeField]
		private GameObject _sign;
	}
}
