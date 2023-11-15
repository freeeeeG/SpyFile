using System;
using Characters;
using Data;
using Services;
using Singletons;
using UnityEngine;

namespace Level
{
	// Token: 0x0200048F RID: 1167
	public sealed class BonusCurrencyWithDroppedGear : MonoBehaviour
	{
		// Token: 0x06001636 RID: 5686 RVA: 0x0004595C File Offset: 0x00043B5C
		public void Attach(DroppedGear gear, GameData.Currency.Type type, int amount, int count)
		{
			this._gear = gear;
			this._type = type;
			this._amount = amount;
			this._count = count;
			switch (this._type)
			{
			case GameData.Currency.Type.Gold:
				this._renderer.sprite = this._goldIcon;
				break;
			case GameData.Currency.Type.DarkQuartz:
				this._renderer.sprite = this._darkQuartzIcon;
				break;
			case GameData.Currency.Type.Bone:
				this._renderer.sprite = this._boneIcon;
				break;
			}
			gear.onLoot += this.Earn;
			gear.onDestroy += this.Earn;
		}

		// Token: 0x06001637 RID: 5687 RVA: 0x000459FC File Offset: 0x00043BFC
		private void Earn(Character character)
		{
			if (!this._gear.gear.destructible)
			{
				return;
			}
			this._gear.onLoot -= this.Earn;
			this._gear.onDestroy -= this.Earn;
			Singleton<Service>.Instance.levelManager.DropCurrency(this._type, this._amount, this._count, base.transform.position);
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06001638 RID: 5688 RVA: 0x00045A81 File Offset: 0x00043C81
		private void OnDestroy()
		{
			if (this._gear == null)
			{
				return;
			}
			this._gear.onLoot -= this.Earn;
			this._gear.onDestroy -= this.Earn;
		}

		// Token: 0x04001374 RID: 4980
		[SerializeField]
		private SpriteRenderer _renderer;

		// Token: 0x04001375 RID: 4981
		[SerializeField]
		private Sprite _goldIcon;

		// Token: 0x04001376 RID: 4982
		[SerializeField]
		private Sprite _boneIcon;

		// Token: 0x04001377 RID: 4983
		[SerializeField]
		private Sprite _darkQuartzIcon;

		// Token: 0x04001378 RID: 4984
		private GameData.Currency.Type _type;

		// Token: 0x04001379 RID: 4985
		private int _amount;

		// Token: 0x0400137A RID: 4986
		private int _count;

		// Token: 0x0400137B RID: 4987
		private DroppedGear _gear;
	}
}
