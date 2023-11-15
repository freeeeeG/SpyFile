using System;
using Characters;
using Characters.Operations.Attack;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x0200065A RID: 1626
	public class DarkOrb : MonoBehaviour
	{
		// Token: 0x060020AE RID: 8366 RVA: 0x000629B6 File Offset: 0x00060BB6
		private void Start()
		{
			this._sweepAttack.Initialize();
			this._sweepAttack.Run(this._character);
		}

		// Token: 0x060020AF RID: 8367 RVA: 0x000629D4 File Offset: 0x00060BD4
		private void Update()
		{
			Vector3 v = this._pivot.transform.position - this._character.transform.position;
			this._rotationTime += this._rotateSpeed * this._character.chronometer.master.deltaTime;
			this._character.movement.MoveHorizontal(v + new Vector2(Mathf.Cos(this._rotationTime), Mathf.Sin(this._rotationTime)) * this._radius);
		}

		// Token: 0x04001BAE RID: 7086
		[SerializeField]
		private Character _character;

		// Token: 0x04001BAF RID: 7087
		[SerializeField]
		private SweepAttack _sweepAttack;

		// Token: 0x04001BB0 RID: 7088
		[SerializeField]
		private Transform _pivot;

		// Token: 0x04001BB1 RID: 7089
		[SerializeField]
		private float _rotateSpeed = 1f;

		// Token: 0x04001BB2 RID: 7090
		[SerializeField]
		private float _radius;

		// Token: 0x04001BB3 RID: 7091
		private float _rotationTime;
	}
}
