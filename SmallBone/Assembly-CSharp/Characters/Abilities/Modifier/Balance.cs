using System;
using Data;
using UnityEngine;

namespace Characters.Abilities.Modifier
{
	// Token: 0x02000BD5 RID: 3029
	[Serializable]
	public sealed class Balance : IStackResolver
	{
		// Token: 0x06003E5E RID: 15966 RVA: 0x00002191 File Offset: 0x00000391
		public void Initialize()
		{
		}

		// Token: 0x06003E5F RID: 15967 RVA: 0x00002191 File Offset: 0x00000391
		public void Attach(Character owner)
		{
		}

		// Token: 0x06003E60 RID: 15968 RVA: 0x00002191 File Offset: 0x00000391
		public void Detach(Character owner)
		{
		}

		// Token: 0x06003E61 RID: 15969 RVA: 0x000B5740 File Offset: 0x000B3940
		public int GetStack(ref Damage damage)
		{
			if (this._stackPerBalance == 0)
			{
				return 0;
			}
			int num = GameData.Currency.currencies[this._type].balance / this._stackPerBalance;
			if (this._maxStack <= 0)
			{
				return num;
			}
			return Mathf.Min(num, this._maxStack);
		}

		// Token: 0x06003E62 RID: 15970 RVA: 0x00002191 File Offset: 0x00000391
		public void UpdateTime(float deltaTime)
		{
		}

		// Token: 0x0400302D RID: 12333
		[SerializeField]
		private GameData.Currency.Type _type;

		// Token: 0x0400302E RID: 12334
		[SerializeField]
		private int _stackPerBalance;

		// Token: 0x0400302F RID: 12335
		[SerializeField]
		private int _maxStack;
	}
}
