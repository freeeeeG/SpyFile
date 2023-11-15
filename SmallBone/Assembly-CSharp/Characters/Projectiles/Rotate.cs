using System;
using UnityEngine;

namespace Characters.Projectiles
{
	// Token: 0x02000769 RID: 1897
	public class Rotate : MonoBehaviour
	{
		// Token: 0x06002744 RID: 10052 RVA: 0x00075DA0 File Offset: 0x00073FA0
		private void Awake()
		{
			IProjectile iprojectile;
			if (!(this._projectile == null))
			{
				IProjectile projectile = this._projectile;
				iprojectile = projectile;
			}
			else
			{
				IProjectile projectile = this._bouncyProjectile;
				iprojectile = projectile;
			}
			this._iprojectile = iprojectile;
		}

		// Token: 0x06002745 RID: 10053 RVA: 0x00075DD4 File Offset: 0x00073FD4
		private void Update()
		{
			float num = this._amount * this._iprojectile.owner.chronometer.projectile.deltaTime;
			num *= Mathf.Sign(this._iprojectile.transform.localScale.x);
			base.transform.Rotate(Vector3.forward, num, Space.Self);
		}

		// Token: 0x04002164 RID: 8548
		[SerializeField]
		private Projectile _projectile;

		// Token: 0x04002165 RID: 8549
		[SerializeField]
		private BouncyProjectile _bouncyProjectile;

		// Token: 0x04002166 RID: 8550
		[SerializeField]
		private float _amount;

		// Token: 0x04002167 RID: 8551
		private IProjectile _iprojectile;
	}
}
