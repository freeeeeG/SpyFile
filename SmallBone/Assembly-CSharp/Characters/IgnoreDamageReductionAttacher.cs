using System;
using Runnables.Triggers;
using UnityEngine;

namespace Characters
{
	// Token: 0x020006F2 RID: 1778
	[RequireComponent(typeof(Character))]
	public sealed class IgnoreDamageReductionAttacher : MonoBehaviour
	{
		// Token: 0x060023EA RID: 9194 RVA: 0x0006BE38 File Offset: 0x0006A038
		private bool OnOwnerGiveDamage(ITarget target, ref Damage damage)
		{
			if (target.character == null)
			{
				return false;
			}
			damage.ignoreDamageReduction += (double)this._baseExtraIgnoreDamageReduction;
			foreach (IgnoreDamageReductionAttacher.DamageInfo damageInfo in this._infos)
			{
				string key = damageInfo.key;
				if (!string.IsNullOrWhiteSpace(key) && damage.key.Equals(key, StringComparison.OrdinalIgnoreCase))
				{
					damage.ignoreDamageReduction += (double)damageInfo.extraIgnoreDamageReduction;
					return false;
				}
			}
			return false;
		}

		// Token: 0x060023EB RID: 9195 RVA: 0x0006BEB1 File Offset: 0x0006A0B1
		private void Awake()
		{
			if (this._owner == null)
			{
				return;
			}
			if (!this._trigger.IsSatisfied())
			{
				return;
			}
			this._owner.onGiveDamage.Add(int.MaxValue, new GiveDamageDelegate(this.OnOwnerGiveDamage));
		}

		// Token: 0x04001E9E RID: 7838
		[GetComponent]
		[SerializeField]
		private Character _owner;

		// Token: 0x04001E9F RID: 7839
		[Trigger.SubcomponentAttribute]
		[SerializeField]
		private Trigger _trigger;

		// Token: 0x04001EA0 RID: 7840
		[Tooltip("모든 공격에 포함되는 받는 데미지 감소 무시 값")]
		[Range(0f, 1f)]
		[SerializeField]
		private float _baseExtraIgnoreDamageReduction;

		// Token: 0x04001EA1 RID: 7841
		[SerializeField]
		private IgnoreDamageReductionAttacher.DamageInfo[] _infos;

		// Token: 0x020006F3 RID: 1779
		[Serializable]
		public class DamageInfo
		{
			// Token: 0x04001EA2 RID: 7842
			public string key;

			// Token: 0x04001EA3 RID: 7843
			[SerializeField]
			[Range(0f, 1f)]
			public float extraIgnoreDamageReduction;
		}
	}
}
