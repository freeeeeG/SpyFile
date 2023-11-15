using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Decorator
{
	// Token: 0x02000EBC RID: 3772
	public sealed class HealthFilter : CharacterOperation
	{
		// Token: 0x06004A26 RID: 18982 RVA: 0x000D898B File Offset: 0x000D6B8B
		public override void Initialize()
		{
			this._operations.Initialize();
		}

		// Token: 0x06004A27 RID: 18983 RVA: 0x000D7314 File Offset: 0x000D5514
		public override void Run(Character owner)
		{
			this.Run(owner, owner);
		}

		// Token: 0x06004A28 RID: 18984 RVA: 0x000D8998 File Offset: 0x000D6B98
		public override void Run(Character owner, Character target)
		{
			bool flag = false;
			HealthFilter.Compare compare = this._compare;
			if (compare != HealthFilter.Compare.GreaterThan)
			{
				if (compare == HealthFilter.Compare.LessThan)
				{
					if (this._valueType == HealthFilter.ValueType.Constant)
					{
						flag = (target.health.currentHealth <= (double)this._value);
					}
					else if (this._valueType == HealthFilter.ValueType.Percent)
					{
						flag = (target.health.percent * 100.0 <= (double)this._value);
					}
				}
			}
			else if (this._valueType == HealthFilter.ValueType.Constant)
			{
				flag = (target.health.currentHealth >= (double)this._value);
			}
			else if (this._valueType == HealthFilter.ValueType.Percent)
			{
				flag = (target.health.percent * 100.0 >= (double)this._value);
			}
			if (flag)
			{
				base.StartCoroutine(this._operations.CRun(owner, target));
			}
		}

		// Token: 0x06004A29 RID: 18985 RVA: 0x00048973 File Offset: 0x00046B73
		public override void Stop()
		{
			base.StopAllCoroutines();
		}

		// Token: 0x0400395C RID: 14684
		[SerializeField]
		private HealthFilter.ValueType _valueType;

		// Token: 0x0400395D RID: 14685
		[SerializeField]
		private HealthFilter.Compare _compare;

		// Token: 0x0400395E RID: 14686
		[SerializeField]
		[Information("percent(0~100)", InformationAttribute.InformationType.Info, false)]
		private float _value;

		// Token: 0x0400395F RID: 14687
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(TargetedOperationInfo))]
		private TargetedOperationInfo.Subcomponents _operations;

		// Token: 0x02000EBD RID: 3773
		private enum ValueType
		{
			// Token: 0x04003961 RID: 14689
			Percent,
			// Token: 0x04003962 RID: 14690
			Constant
		}

		// Token: 0x02000EBE RID: 3774
		private enum Compare
		{
			// Token: 0x04003964 RID: 14692
			GreaterThan,
			// Token: 0x04003965 RID: 14693
			LessThan
		}
	}
}
