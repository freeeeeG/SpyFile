using System;
using Characters;
using Services;
using Singletons;
using UnityEngine;

namespace Level
{
	// Token: 0x020004E5 RID: 1253
	public class GoldReward : InteractiveObject, ILootable
	{
		// Token: 0x14000023 RID: 35
		// (add) Token: 0x0600187B RID: 6267 RVA: 0x0004C91C File Offset: 0x0004AB1C
		// (remove) Token: 0x0600187C RID: 6268 RVA: 0x0004C954 File Offset: 0x0004AB54
		public event Action onLoot;

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x0600187D RID: 6269 RVA: 0x0004C989 File Offset: 0x0004AB89
		// (set) Token: 0x0600187E RID: 6270 RVA: 0x0004C991 File Offset: 0x0004AB91
		public bool looted { get; private set; }

		// Token: 0x0600187F RID: 6271 RVA: 0x0004C99A File Offset: 0x0004AB9A
		public override void OnActivate()
		{
			base.OnActivate();
			this._animator.Play(InteractiveObject._activateHash);
		}

		// Token: 0x06001880 RID: 6272 RVA: 0x0004C9B2 File Offset: 0x0004ABB2
		public override void OnDeactivate()
		{
			base.OnDeactivate();
			this._animator.Play(InteractiveObject._deactivateHash);
		}

		// Token: 0x06001881 RID: 6273 RVA: 0x0004C9CC File Offset: 0x0004ABCC
		public override void InteractWith(Character character)
		{
			Action action = this.onLoot;
			if (action != null)
			{
				action();
			}
			this.looted = true;
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._interactSound, base.transform.position);
			Vector2Int goldrewardAmount = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.goldrewardAmount;
			int amount = UnityEngine.Random.Range(goldrewardAmount.x, goldrewardAmount.y);
			Singleton<Service>.Instance.levelManager.DropGold(amount, 40, base.transform.position);
			base.Deactivate();
		}

		// Token: 0x0400154D RID: 5453
		[GetComponent]
		[SerializeField]
		private Animator _animator;
	}
}
