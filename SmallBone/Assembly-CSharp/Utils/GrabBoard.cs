using System;
using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace Utils
{
	// Token: 0x02000468 RID: 1128
	public sealed class GrabBoard : MonoBehaviour
	{
		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06001580 RID: 5504 RVA: 0x00043A6B File Offset: 0x00041C6B
		public List<Target> targets
		{
			get
			{
				return this._targets;
			}
		}

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06001581 RID: 5505 RVA: 0x00043A73 File Offset: 0x00041C73
		public List<Target> failTargets
		{
			get
			{
				return this._failTargets;
			}
		}

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x06001582 RID: 5506 RVA: 0x00043A7B File Offset: 0x00041C7B
		public int maxTargetCount
		{
			get
			{
				return this._maxTargetCount;
			}
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x00043A83 File Offset: 0x00041C83
		public void Clear()
		{
			this._targets.Clear();
			this._failTargets.Clear();
		}

		// Token: 0x06001584 RID: 5508 RVA: 0x00043A9B File Offset: 0x00041C9B
		public void Add(Target target)
		{
			if (this._targets.Contains(target))
			{
				return;
			}
			if (this.targets.Count > this._maxTargetCount)
			{
				return;
			}
			this._targets.Add(target);
		}

		// Token: 0x06001585 RID: 5509 RVA: 0x00043ACC File Offset: 0x00041CCC
		public void AddFailed(Target target)
		{
			if (this._failTargets.Contains(target))
			{
				return;
			}
			if (this.failTargets.Count > this._maxFailedTargetCount)
			{
				return;
			}
			this._failTargets.Add(target);
		}

		// Token: 0x06001586 RID: 5510 RVA: 0x00043B00 File Offset: 0x00041D00
		public bool HasInTargets(Character character)
		{
			using (List<Target>.Enumerator enumerator = this._targets.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.character == character)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001587 RID: 5511 RVA: 0x00043B60 File Offset: 0x00041D60
		public int TargetCount()
		{
			return this._targets.Count;
		}

		// Token: 0x06001588 RID: 5512 RVA: 0x00043B6D File Offset: 0x00041D6D
		private void Awake()
		{
			this._targets = new List<Target>(this._maxTargetCount);
			this._failTargets = new List<Target>(this._maxFailedTargetCount);
		}

		// Token: 0x06001589 RID: 5513 RVA: 0x00043B91 File Offset: 0x00041D91
		public void Remove(Target target)
		{
			if (!this._targets.Contains(target))
			{
				return;
			}
			this._targets.Remove(target);
		}

		// Token: 0x040012CC RID: 4812
		[SerializeField]
		private int _maxTargetCount = 32;

		// Token: 0x040012CD RID: 4813
		[SerializeField]
		private int _maxFailedTargetCount = 32;

		// Token: 0x040012CE RID: 4814
		private List<Target> _targets;

		// Token: 0x040012CF RID: 4815
		private List<Target> _failTargets;
	}
}
