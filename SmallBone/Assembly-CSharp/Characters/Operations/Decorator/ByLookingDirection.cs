using System;
using UnityEngine;

namespace Characters.Operations.Decorator
{
	// Token: 0x02000EB6 RID: 3766
	public class ByLookingDirection : CharacterOperation
	{
		// Token: 0x06004A13 RID: 18963 RVA: 0x000D867A File Offset: 0x000D687A
		public override void Initialize()
		{
			if (this._left != null)
			{
				this._left.Initialize();
			}
			if (this._right != null)
			{
				this._right.Initialize();
			}
		}

		// Token: 0x06004A14 RID: 18964 RVA: 0x000D86A2 File Offset: 0x000D68A2
		public override void Run(Character owner)
		{
			if (owner.lookingDirection == Character.LookingDirection.Left)
			{
				this._left.Stop();
				this._left.Run(owner);
				return;
			}
			this._right.Stop();
			this._right.Run(owner);
		}

		// Token: 0x06004A15 RID: 18965 RVA: 0x000D86DC File Offset: 0x000D68DC
		public override void Stop()
		{
			if (this._left != null)
			{
				this._left.Stop();
			}
			if (this._right != null)
			{
				this._right.Stop();
			}
		}

		// Token: 0x0400394B RID: 14667
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation.Subcomponents _left;

		// Token: 0x0400394C RID: 14668
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation.Subcomponents _right;
	}
}
