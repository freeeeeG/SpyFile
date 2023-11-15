using System;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E37 RID: 3639
	public class SpawnGoldAtTarget : TargetedCharacterOperation
	{
		// Token: 0x06004885 RID: 18565 RVA: 0x000D2DE0 File Offset: 0x000D0FE0
		public override void Run(Character owner, Character target)
		{
			if (!this._characterTypeFilter[target.type])
			{
				return;
			}
			if (!MMMaths.PercentChance(this._possibility))
			{
				return;
			}
			Vector3 position = this._spawnAtOwner ? owner.transform.position : target.transform.position;
			Singleton<Service>.Instance.levelManager.DropGold(this._gold, this._count, position);
		}

		// Token: 0x04003799 RID: 14233
		[SerializeField]
		[Range(0f, 100f)]
		private int _possibility;

		// Token: 0x0400379A RID: 14234
		[SerializeField]
		private int _gold;

		// Token: 0x0400379B RID: 14235
		[SerializeField]
		private int _count;

		// Token: 0x0400379C RID: 14236
		[SerializeField]
		private bool _spawnAtOwner;

		// Token: 0x0400379D RID: 14237
		[SerializeField]
		private CharacterTypeBoolArray _characterTypeFilter;
	}
}
