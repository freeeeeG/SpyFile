using System;
using Characters.Operations;
using Characters.Operations.Attack;
using UnityEditor;
using UnityEngine;

namespace Characters.Gear.Items.Customs
{
	// Token: 0x02000903 RID: 2307
	public sealed class ChosenMagicianBadgeOrb : MonoBehaviour
	{
		// Token: 0x06003134 RID: 12596 RVA: 0x00093574 File Offset: 0x00091774
		private void OnEnable()
		{
			this._normalAttack.Initialize();
			this._fastAttack.Initialize();
			this._onFast.Initialize();
			this._character = this._item.owner;
			this._normalAttack.Run(this._character);
		}

		// Token: 0x06003135 RID: 12597 RVA: 0x000935C4 File Offset: 0x000917C4
		public void Initialize(float startRadian)
		{
			this._elapsed = startRadian;
			this._attack = this._normalAttack;
			this._speed = this._normalSpeed;
		}

		// Token: 0x06003136 RID: 12598 RVA: 0x000935E8 File Offset: 0x000917E8
		public void Move(float radius)
		{
			Vector3 v = this._pivot.transform.position - base.transform.position;
			this._elapsed += this._speed * this._character.chronometer.master.deltaTime;
			Vector2 b = v + new Vector2(Mathf.Cos(this._elapsed), Mathf.Sin(this._elapsed)) * radius;
			base.transform.position = base.transform.position + b;
		}

		// Token: 0x06003137 RID: 12599 RVA: 0x00093690 File Offset: 0x00091890
		public void ChangeToFast()
		{
			this._speed = this._fastSpeed;
			this._attack.Stop();
			this._attack = this._fastAttack;
			this._attack.Run(this._character);
			this._onFast.StopAll();
			base.StartCoroutine(this._onFast.CRun(this._character));
		}

		// Token: 0x06003138 RID: 12600 RVA: 0x000936F4 File Offset: 0x000918F4
		public void ChangeToNormal()
		{
			this._speed = this._normalSpeed;
			this._attack.Stop();
			this._attack = this._normalAttack;
			this._attack.Run(this._character);
		}

		// Token: 0x04002882 RID: 10370
		[SerializeField]
		private Item _item;

		// Token: 0x04002883 RID: 10371
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _onFast;

		// Token: 0x04002884 RID: 10372
		[Subcomponent(typeof(SweepAttack))]
		[SerializeField]
		private SweepAttack _normalAttack;

		// Token: 0x04002885 RID: 10373
		[Subcomponent(typeof(SweepAttack))]
		[SerializeField]
		private SweepAttack _fastAttack;

		// Token: 0x04002886 RID: 10374
		[SerializeField]
		private Transform _pivot;

		// Token: 0x04002887 RID: 10375
		[SerializeField]
		private float _normalSpeed;

		// Token: 0x04002888 RID: 10376
		[SerializeField]
		private float _fastSpeed;

		// Token: 0x04002889 RID: 10377
		private Character _character;

		// Token: 0x0400288A RID: 10378
		private float _elapsed;

		// Token: 0x0400288B RID: 10379
		private float _speed;

		// Token: 0x0400288C RID: 10380
		private SweepAttack _attack;
	}
}
