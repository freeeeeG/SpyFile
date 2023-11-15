using System;
using Characters.Operations;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x020008B5 RID: 2229
	public sealed class Strike : SimpleStatBonusKeyword
	{
		// Token: 0x17000A2B RID: 2603
		// (get) Token: 0x06002F78 RID: 12152 RVA: 0x0008E3CF File Offset: 0x0008C5CF
		protected override double[] statBonusByStep
		{
			get
			{
				return this._statBonusByStep;
			}
		}

		// Token: 0x17000A2C RID: 2604
		// (get) Token: 0x06002F79 RID: 12153 RVA: 0x00088DBC File Offset: 0x00086FBC
		protected override Stat.Category statCategory
		{
			get
			{
				return Stat.Category.PercentPoint;
			}
		}

		// Token: 0x17000A2D RID: 2605
		// (get) Token: 0x06002F7A RID: 12154 RVA: 0x0008E3D7 File Offset: 0x0008C5D7
		protected override Stat.Kind statKind
		{
			get
			{
				return Stat.Kind.CriticalDamage;
			}
		}

		// Token: 0x06002F7B RID: 12155 RVA: 0x0008E3DE File Offset: 0x0008C5DE
		protected override void Initialize()
		{
			base.Initialize();
			this._onTargetHit.Initialize();
		}

		// Token: 0x06002F7C RID: 12156 RVA: 0x0008E3F1 File Offset: 0x0008C5F1
		public override void Attach()
		{
			base.Attach();
			base.character.onGiveDamage.Add(int.MinValue, new GiveDamageDelegate(this.OnGiveDamage));
		}

		// Token: 0x06002F7D RID: 12157 RVA: 0x0008C7EC File Offset: 0x0008A9EC
		public override void UpdateBonus(bool wasActive, bool wasOmen)
		{
			base.UpdateStat();
		}

		// Token: 0x06002F7E RID: 12158 RVA: 0x0008E41C File Offset: 0x0008C61C
		private bool OnGiveDamage(ITarget target, ref Damage damage)
		{
			Strike.<>c__DisplayClass15_0 CS$<>8__locals1 = new Strike.<>c__DisplayClass15_0();
			CS$<>8__locals1.target = target;
			CS$<>8__locals1.<>4__this = this;
			if (this.keyword.step < this._statBonusByStep.Length - 1)
			{
				return false;
			}
			if (!MMMaths.Chance(this._maxStatBonusChance))
			{
				return false;
			}
			if (this._targetPoint != null)
			{
				Vector3 center = CS$<>8__locals1.target.collider.bounds.center;
				Vector3 size = CS$<>8__locals1.target.collider.bounds.size;
				size.x *= this._positionInfo.pivotValue.x;
				size.y *= this._positionInfo.pivotValue.y;
				Vector3 position = center + size;
				this._targetPoint.position = position;
			}
			CS$<>8__locals1.target.character.health.onTakeDamage.Add(int.MinValue, new TakeDamageDelegate(CS$<>8__locals1.<OnGiveDamage>g__MultiplyCriticalDamage|0));
			return false;
		}

		// Token: 0x06002F7F RID: 12159 RVA: 0x0008E519 File Offset: 0x0008C719
		public override void Detach()
		{
			base.Detach();
			base.character.onGiveDamage.Remove(new GiveDamageDelegate(this.OnGiveDamage));
		}

		// Token: 0x04002726 RID: 10022
		[Header("2세트 효과")]
		[SerializeField]
		private double[] _statBonusByStep = new double[]
		{
			0.0,
			0.20000000298023224,
			0.20000000298023224
		};

		// Token: 0x04002727 RID: 10023
		[Header("4세트 효과 (Percent)")]
		[SerializeField]
		[Range(0f, 1f)]
		private float _maxStatBonusChance = 0.5f;

		// Token: 0x04002728 RID: 10024
		[SerializeField]
		private float _criticalDamageMultiplier = 0.2f;

		// Token: 0x04002729 RID: 10025
		[SerializeField]
		private PositionInfo _positionInfo;

		// Token: 0x0400272A RID: 10026
		[SerializeField]
		private Transform _targetPoint;

		// Token: 0x0400272B RID: 10027
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _onTargetHit;
	}
}
