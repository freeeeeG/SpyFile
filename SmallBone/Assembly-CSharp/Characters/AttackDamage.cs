using System;
using UnityEngine;

namespace Characters
{
	// Token: 0x020006A0 RID: 1696
	public class AttackDamage : MonoBehaviour, IAttackDamage
	{
		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x060021DB RID: 8667 RVA: 0x00065AA4 File Offset: 0x00063CA4
		// (set) Token: 0x060021DC RID: 8668 RVA: 0x00065AAC File Offset: 0x00063CAC
		public int minAttackDamage
		{
			get
			{
				return this._minAttackDamage;
			}
			set
			{
				this._minAttackDamage = value;
			}
		}

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x060021DD RID: 8669 RVA: 0x00065AB5 File Offset: 0x00063CB5
		// (set) Token: 0x060021DE RID: 8670 RVA: 0x00065ABD File Offset: 0x00063CBD
		public int maxAttackDamage
		{
			get
			{
				return this._maxAttackDamage;
			}
			set
			{
				this._maxAttackDamage = value;
			}
		}

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x060021DF RID: 8671 RVA: 0x00065AC6 File Offset: 0x00063CC6
		public float amount
		{
			get
			{
				return (float)UnityEngine.Random.Range(this.minAttackDamage, this._maxAttackDamage + 1);
			}
		}

		// Token: 0x04001CD5 RID: 7381
		[SerializeField]
		private int _minAttackDamage;

		// Token: 0x04001CD6 RID: 7382
		[SerializeField]
		private int _maxAttackDamage;
	}
}
