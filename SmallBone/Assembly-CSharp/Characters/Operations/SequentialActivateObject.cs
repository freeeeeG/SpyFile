using System;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DBC RID: 3516
	public class SequentialActivateObject : CharacterOperation
	{
		// Token: 0x060046B8 RID: 18104 RVA: 0x000CD449 File Offset: 0x000CB649
		private void Awake()
		{
			this.SetParent();
			this._indics = new int[this._parent.childCount];
			this.SetOrder();
		}

		// Token: 0x060046B9 RID: 18105 RVA: 0x000CD46D File Offset: 0x000CB66D
		public override void Run(Character owner)
		{
			this.ActivateNextObject();
		}

		// Token: 0x060046BA RID: 18106 RVA: 0x000CD478 File Offset: 0x000CB678
		private void ActivateNextObject()
		{
			if (this._currentIndex >= this._indics.Length)
			{
				this.SetParent();
				this._indics = new int[this._parent.childCount];
				this.SetOrder();
				this._currentIndex = 0;
			}
			DarkRushEffect component = this._parent.GetChild(this._indics[this._currentIndex]).gameObject.GetComponent<DarkRushEffect>();
			component.SetSignEffectOrder(this._currentIndex);
			component.SetImpactEffectOrder(this._currentIndex);
			component.ShowSign();
			this._currentIndex++;
		}

		// Token: 0x060046BB RID: 18107 RVA: 0x000CD50B File Offset: 0x000CB70B
		private void SetParent()
		{
			if (this._parentType == SequentialActivateObject.ParentType.Static)
			{
				this._parent = this._parentPool.GetFirstParent();
				return;
			}
			if (this._parentType == SequentialActivateObject.ParentType.Random)
			{
				this._parent = this._parentPool.GetRandomParent();
			}
		}

		// Token: 0x060046BC RID: 18108 RVA: 0x000CD544 File Offset: 0x000CB744
		private void SetOrder()
		{
			for (int i = 0; i < this._indics.Length; i++)
			{
				this._indics[i] = i;
			}
			if (this._order == SequentialActivateObject.Order.Random)
			{
				this._indics.Shuffle<int>();
				return;
			}
			if (this._order == SequentialActivateObject.Order.Decrease)
			{
				this._indics.Reverse<int>();
			}
		}

		// Token: 0x0400358A RID: 13706
		[SerializeField]
		private SequentialActivateObject.Order _order;

		// Token: 0x0400358B RID: 13707
		[SerializeField]
		private SequentialActivateObject.ParentType _parentType;

		// Token: 0x0400358C RID: 13708
		[SerializeField]
		private ParentPool _parentPool;

		// Token: 0x0400358D RID: 13709
		private int[] _indics;

		// Token: 0x0400358E RID: 13710
		private int _currentIndex;

		// Token: 0x0400358F RID: 13711
		private Transform _parent;

		// Token: 0x02000DBD RID: 3517
		private enum Order
		{
			// Token: 0x04003591 RID: 13713
			Random,
			// Token: 0x04003592 RID: 13714
			Increase,
			// Token: 0x04003593 RID: 13715
			Decrease
		}

		// Token: 0x02000DBE RID: 3518
		private enum ParentType
		{
			// Token: 0x04003595 RID: 13717
			Random,
			// Token: 0x04003596 RID: 13718
			Static
		}
	}
}
