using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions
{
	// Token: 0x02000950 RID: 2384
	public class ParryAction : Action
	{
		// Token: 0x17000B13 RID: 2835
		// (get) Token: 0x0600334E RID: 13134 RVA: 0x000985A6 File Offset: 0x000967A6
		public Motion waitingMotion
		{
			get
			{
				return this._waitingMotion;
			}
		}

		// Token: 0x17000B14 RID: 2836
		// (get) Token: 0x0600334F RID: 13135 RVA: 0x000985AE File Offset: 0x000967AE
		public Motion parryMotion
		{
			get
			{
				return this._parryMotion;
			}
		}

		// Token: 0x17000B15 RID: 2837
		// (get) Token: 0x06003350 RID: 13136 RVA: 0x000985B6 File Offset: 0x000967B6
		public override Motion[] motions
		{
			get
			{
				return new Motion[]
				{
					this._waitingMotion,
					this._parryMotion
				};
			}
		}

		// Token: 0x17000B16 RID: 2838
		// (get) Token: 0x06003351 RID: 13137 RVA: 0x000985D0 File Offset: 0x000967D0
		public override bool canUse
		{
			get
			{
				return base.cooldown.canUse && !this._owner.stunedOrFreezed && base.PassAllConstraints(this._waitingMotion);
			}
		}

		// Token: 0x06003352 RID: 13138 RVA: 0x000985FC File Offset: 0x000967FC
		protected override void Awake()
		{
			base.Awake();
			this._waitingMotion.onStart += delegate()
			{
				this._lookingDirection = base.owner.lookingDirection;
				this._owner.health.onTakeDamage.Add(10000, new TakeDamageDelegate(this.OnTakeDamage));
			};
			this._waitingMotion.onEnd += delegate()
			{
				this._owner.health.onTakeDamage.Remove(new TakeDamageDelegate(this.OnTakeDamage));
			};
			this._waitingMotion.onCancel += delegate()
			{
				this._owner.health.onTakeDamage.Remove(new TakeDamageDelegate(this.OnTakeDamage));
			};
			this._parryMotion.onStart += delegate()
			{
				base.owner.lookingDirection = this._lookingDirection;
			};
			if (this._countableRange.y == 1f)
			{
				this._countableRange.y = float.PositiveInfinity;
			}
		}

		// Token: 0x06003353 RID: 13139 RVA: 0x0009868D File Offset: 0x0009688D
		public override void Initialize(Character owner)
		{
			base.Initialize(owner);
			this._waitingMotion.Initialize(this);
			this._parryMotion.Initialize(this);
		}

		// Token: 0x06003354 RID: 13140 RVA: 0x000986AE File Offset: 0x000968AE
		private void OnDisable()
		{
			this._owner.health.onTakeDamage.Remove(new TakeDamageDelegate(this.OnTakeDamage));
		}

		// Token: 0x06003355 RID: 13141 RVA: 0x000986D4 File Offset: 0x000968D4
		private bool OnTakeDamage(ref Damage damage)
		{
			if (damage.attackType == Damage.AttackType.Additional || damage.attackType == Damage.AttackType.None || !MMMaths.Range(this._waitingMotion.normalizedTime, this._countableRange))
			{
				return false;
			}
			Vector3 position = base.transform.position;
			Vector3 position2 = damage.attacker.transform.position;
			if (!this._frontOnly || (this._owner.lookingDirection == Character.LookingDirection.Right && position.x < position2.x) || (this._owner.lookingDirection == Character.LookingDirection.Left && position.x > position2.x))
			{
				base.DoMotion(this._parryMotion);
				damage.@null = true;
				return this._ignoreDamage;
			}
			return false;
		}

		// Token: 0x06003356 RID: 13142 RVA: 0x00098783 File Offset: 0x00096983
		public override bool TryStart()
		{
			if (!this.canUse || !base.ConsumeCooldownIfNeeded())
			{
				return false;
			}
			base.DoAction(this._waitingMotion);
			return true;
		}

		// Token: 0x040029BC RID: 10684
		[SerializeField]
		private bool _ignoreDamage;

		// Token: 0x040029BD RID: 10685
		[SerializeField]
		private bool _frontOnly = true;

		// Token: 0x040029BE RID: 10686
		[MinMaxSlider(0f, 1f)]
		[SerializeField]
		private Vector2 _countableRange = new Vector2(0f, 1f);

		// Token: 0x040029BF RID: 10687
		[Subcomponent(typeof(Motion))]
		[SerializeField]
		protected Motion _waitingMotion;

		// Token: 0x040029C0 RID: 10688
		[SerializeField]
		[Subcomponent(typeof(Motion))]
		protected Motion _parryMotion;

		// Token: 0x040029C1 RID: 10689
		private Character.LookingDirection _lookingDirection;
	}
}
