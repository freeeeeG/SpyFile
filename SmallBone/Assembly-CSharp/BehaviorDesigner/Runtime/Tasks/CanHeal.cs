using System;
using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x0200149A RID: 5274
	[TaskDescription("타겟 리스트에 있는 적들 중 힐이 필요한 적을 추려냅니다.리스트가 비게 되면 Fail을 반환합니다.조건에 맞지 않는 캐릭터는 리스트에서 제외됩니다.")]
	public sealed class CanHeal : Conditional
	{
		// Token: 0x060066F2 RID: 26354 RVA: 0x001299B0 File Offset: 0x00127BB0
		public override TaskStatus OnUpdate()
		{
			List<Character> value = this._targetsList.Value;
			float num = this._healHealth.Value;
			for (int i = value.Count - 1; i >= 0; i--)
			{
				if (!value[i].gameObject.activeSelf)
				{
					value.Remove(value[i]);
				}
				else if (value[i].health.dead)
				{
					value.Remove(value[i]);
				}
				else
				{
					float num2 = (this._healthType == CanHeal.HealthType.Constant) ? ((float)value[i].health.currentHealth) : ((float)value[i].health.percent * 100f);
					switch (this.operation)
					{
					case CanHeal.Operation.LessThan:
						if (num2 >= num)
						{
							value.Remove(value[i]);
						}
						break;
					case CanHeal.Operation.LessThanOrEqualTo:
						if (num2 > num)
						{
							value.Remove(value[i]);
						}
						break;
					case CanHeal.Operation.EqualTo:
						if (!Mathf.Approximately(num2, num))
						{
							value.Remove(value[i]);
						}
						break;
					case CanHeal.Operation.NotEqualTo:
						if (Mathf.Approximately(num2, num))
						{
							value.Remove(value[i]);
						}
						break;
					case CanHeal.Operation.GreaterThanOrEqualTo:
						if (num2 < num)
						{
							value.Remove(value[i]);
						}
						break;
					case CanHeal.Operation.GreaterThan:
						if (num2 <= num)
						{
							value.Remove(value[i]);
						}
						break;
					}
				}
			}
			if (value.Count == 0)
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x040052C0 RID: 21184
		[SerializeField]
		private SharedCharacterList _targetsList;

		// Token: 0x040052C1 RID: 21185
		[SerializeField]
		public CanHeal.Operation operation;

		// Token: 0x040052C2 RID: 21186
		[SerializeField]
		private CanHeal.HealthType _healthType;

		// Token: 0x040052C3 RID: 21187
		[SerializeField]
		[Tooltip("조건에 맞지 않는 캐릭터는 리스트에서 제외됩니다.")]
		private SharedFloat _healHealth;

		// Token: 0x0200149B RID: 5275
		public enum Operation
		{
			// Token: 0x040052C5 RID: 21189
			LessThan,
			// Token: 0x040052C6 RID: 21190
			LessThanOrEqualTo,
			// Token: 0x040052C7 RID: 21191
			EqualTo,
			// Token: 0x040052C8 RID: 21192
			NotEqualTo,
			// Token: 0x040052C9 RID: 21193
			GreaterThanOrEqualTo,
			// Token: 0x040052CA RID: 21194
			GreaterThan
		}

		// Token: 0x0200149C RID: 5276
		public enum HealthType
		{
			// Token: 0x040052CC RID: 21196
			Percent,
			// Token: 0x040052CD RID: 21197
			Constant
		}
	}
}
