using System;
using UnityEngine;

namespace Characters.Operations.Decorator
{
	// Token: 0x02000EB5 RID: 3765
	public sealed class ByCharacterSize : CharacterOperation
	{
		// Token: 0x06004A0F RID: 18959 RVA: 0x000D84A4 File Offset: 0x000D66A4
		public override void Initialize()
		{
			if (this._small != null)
			{
				this._small.Initialize();
			}
			if (this._medium != null)
			{
				this._medium.Initialize();
			}
			if (this._large != null)
			{
				this._large.Initialize();
			}
			if (this._extraLarge != null)
			{
				this._extraLarge.Initialize();
			}
			if (this._none != null)
			{
				this._none.Initialize();
			}
		}

		// Token: 0x06004A10 RID: 18960 RVA: 0x000D8530 File Offset: 0x000D6730
		public override void Run(Character owner)
		{
			if (owner.sizeForEffect == Character.SizeForEffect.Small && this._small != null)
			{
				this._small.Run(owner);
				return;
			}
			if (owner.sizeForEffect == Character.SizeForEffect.Medium && this._medium != null)
			{
				this._medium.Run(owner);
				return;
			}
			if (owner.sizeForEffect == Character.SizeForEffect.Large && this._large != null)
			{
				this._large.Run(owner);
				return;
			}
			if (owner.sizeForEffect == Character.SizeForEffect.ExtraLarge && this._extraLarge != null)
			{
				this._extraLarge.Run(owner);
				return;
			}
			if (owner.sizeForEffect == Character.SizeForEffect.None && this._none != null)
			{
				this._none.Run(owner);
			}
		}

		// Token: 0x06004A11 RID: 18961 RVA: 0x000D85F0 File Offset: 0x000D67F0
		public override void Stop()
		{
			if (this._small != null)
			{
				this._small.Stop();
			}
			if (this._medium != null)
			{
				this._medium.Stop();
			}
			if (this._large != null)
			{
				this._large.Stop();
			}
			if (this._extraLarge != null)
			{
				this._extraLarge.Stop();
			}
			if (this._none != null)
			{
				this._none.Stop();
			}
		}

		// Token: 0x04003946 RID: 14662
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation _small;

		// Token: 0x04003947 RID: 14663
		[CharacterOperation.SubcomponentAttribute]
		[SerializeField]
		private CharacterOperation _medium;

		// Token: 0x04003948 RID: 14664
		[CharacterOperation.SubcomponentAttribute]
		[SerializeField]
		private CharacterOperation _large;

		// Token: 0x04003949 RID: 14665
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation _extraLarge;

		// Token: 0x0400394A RID: 14666
		[CharacterOperation.SubcomponentAttribute]
		[SerializeField]
		private CharacterOperation _none;
	}
}
