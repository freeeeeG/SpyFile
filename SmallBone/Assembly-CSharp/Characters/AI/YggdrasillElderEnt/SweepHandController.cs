using System;
using UnityEngine;

namespace Characters.AI.YggdrasillElderEnt
{
	// Token: 0x0200113D RID: 4413
	public class SweepHandController : MonoBehaviour
	{
		// Token: 0x1700110B RID: 4363
		// (get) Token: 0x060055DF RID: 21983 RVA: 0x000FFF14 File Offset: 0x000FE114
		// (set) Token: 0x060055E0 RID: 21984 RVA: 0x000FFF1C File Offset: 0x000FE11C
		public bool left { get; private set; }

		// Token: 0x060055E1 RID: 21985 RVA: 0x000FFF25 File Offset: 0x000FE125
		private void Awake()
		{
			this._ownerHealth.onDie += this.Stop;
		}

		// Token: 0x060055E2 RID: 21986 RVA: 0x000FFF3E File Offset: 0x000FE13E
		public void Select()
		{
			this.left = MMMaths.RandomBool();
			this._current = (this.left ? this._left : this._right);
		}

		// Token: 0x060055E3 RID: 21987 RVA: 0x000FFF67 File Offset: 0x000FE167
		public void Attack()
		{
			if (this._current != null)
			{
				this._current.Attack();
			}
		}

		// Token: 0x060055E4 RID: 21988 RVA: 0x000FFF82 File Offset: 0x000FE182
		public void Stop()
		{
			if (this._current != null)
			{
				this._current.Stop();
			}
		}

		// Token: 0x060055E5 RID: 21989 RVA: 0x000FFF9D File Offset: 0x000FE19D
		public void ReplaceHands()
		{
			if (this._current == null)
			{
				return;
			}
			this.Stop();
			this._current = ((this._current == this._left) ? this._right : this._left);
		}

		// Token: 0x040044D4 RID: 17620
		[SerializeField]
		private Health _ownerHealth;

		// Token: 0x040044D5 RID: 17621
		[SerializeField]
		private SweepHand _left;

		// Token: 0x040044D6 RID: 17622
		[SerializeField]
		private SweepHand _right;

		// Token: 0x040044D7 RID: 17623
		private SweepHand _current;
	}
}
