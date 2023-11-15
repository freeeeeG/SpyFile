using System;
using Characters;
using Data;
using GameResources;
using Scenes;
using Services;
using Singletons;
using UI.GearPopup;
using UnityEngine;

namespace Level
{
	// Token: 0x02000511 RID: 1297
	public sealed class PurchasableCurrency : DroppedPurchasableReward
	{
		// Token: 0x06001992 RID: 6546 RVA: 0x00050338 File Offset: 0x0004E538
		public override void OpenPopupBy(Character character)
		{
			Vector3 position = base.transform.position;
			Vector3 position2 = character.transform.position;
			position.x = position2.x + ((position.x > position2.x) ? InteractiveObject._popupUIOffset.x : (-InteractiveObject._popupUIOffset.x));
			position.y += InteractiveObject._popupUIOffset.y;
			GearPopupCanvas gearPopupCanvas = Scene<GameBase>.instance.uiManager.gearPopupCanvas;
			gearPopupCanvas.gearPopup.Set(this.displayName, this.description);
			Localization.GetLocalizedString("label/interaction/loot");
			string colorCode = GameData.Currency.currencies[this._type].colorCode;
			gearPopupCanvas.gearPopup.SetInteractionLabelAsPurchase(base.priceCurrency, base.price);
			gearPopupCanvas.Open(position);
		}

		// Token: 0x06001993 RID: 6547 RVA: 0x00047EF0 File Offset: 0x000460F0
		public override void ClosePopup()
		{
			base.ClosePopup();
			Scene<GameBase>.instance.uiManager.gearPopupCanvas.Close();
		}

		// Token: 0x06001994 RID: 6548 RVA: 0x0005040C File Offset: 0x0004E60C
		public override void InteractWith(Character character)
		{
			base.InteractWith(character);
			if (base.price != 0)
			{
				return;
			}
			Singleton<Service>.Instance.levelManager.DropCurrency(this._type, this._amount, this._count, (this._dropPoint != null) ? this._dropPoint.position : base.transform.position);
			this._released = true;
			this.ClosePopup();
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06001995 RID: 6549 RVA: 0x00050488 File Offset: 0x0004E688
		private void OnDestroy()
		{
			if (this._released)
			{
				return;
			}
			if (Map.Instance.type != Map.Type.Normal)
			{
				return;
			}
			Singleton<Service>.Instance.levelManager.DropCurrency(this._type, this._amount, this._count, (this._dropPoint != null) ? this._dropPoint.position : base.transform.position);
		}

		// Token: 0x0400165B RID: 5723
		[SerializeField]
		private GameData.Currency.Type _type;

		// Token: 0x0400165C RID: 5724
		[SerializeField]
		private int _amount;

		// Token: 0x0400165D RID: 5725
		[SerializeField]
		private int _count;

		// Token: 0x0400165E RID: 5726
		[SerializeField]
		private Transform _dropPoint;

		// Token: 0x0400165F RID: 5727
		[SerializeField]
		private string _overrideNameKey;

		// Token: 0x04001660 RID: 5728
		private bool _released;
	}
}
