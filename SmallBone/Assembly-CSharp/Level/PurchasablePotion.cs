using System;
using Characters;
using Characters.Operations.Fx;
using FX.SpriteEffects;
using Scenes;
using Singletons;
using UI.GearPopup;
using UnityEditor;
using UnityEngine;

namespace Level
{
	// Token: 0x02000512 RID: 1298
	public sealed class PurchasablePotion : DroppedPurchasableReward
	{
		// Token: 0x06001997 RID: 6551 RVA: 0x000504F4 File Offset: 0x0004E6F4
		public override void InteractWith(Character character)
		{
			base.InteractWith(character);
			if (base.price != 0)
			{
				return;
			}
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._interactSound, base.transform.position);
			this._spawn.Run(character);
			character.spriteEffectStack.Add(new EasedColorBlend(this.priority, this._startColor, this._endColor, this._curve));
			character.health.Heal(new Health.HealInfo(this._giverType, (double)this._healAmount, true));
			this.ClosePopup();
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06001998 RID: 6552 RVA: 0x00050594 File Offset: 0x0004E794
		public override void OpenPopupBy(Character character)
		{
			base.OpenPopupBy(character);
			Vector3 position = base.transform.position;
			Vector3 position2 = character.transform.position;
			position.x = position2.x + ((position.x > position2.x) ? InteractiveObject._popupUIOffset.x : (-InteractiveObject._popupUIOffset.x));
			position.y += InteractiveObject._popupUIOffset.y;
			GearPopupCanvas gearPopupCanvas = Scene<GameBase>.instance.uiManager.gearPopupCanvas;
			gearPopupCanvas.gearPopup.Set(this.displayName, this.description);
			gearPopupCanvas.gearPopup.SetInteractionLabelAsPurchase(base.priceCurrency, base.price);
			Scene<GameBase>.instance.uiManager.gearPopupCanvas.Open(position);
		}

		// Token: 0x06001999 RID: 6553 RVA: 0x00047EF0 File Offset: 0x000460F0
		public override void ClosePopup()
		{
			base.ClosePopup();
			Scene<GameBase>.instance.uiManager.gearPopupCanvas.Close();
		}

		// Token: 0x0600199A RID: 6554 RVA: 0x00002191 File Offset: 0x00000391
		public void Initialize()
		{
		}

		// Token: 0x04001661 RID: 5729
		[SerializeField]
		private int _healAmount;

		// Token: 0x04001662 RID: 5730
		[SerializeField]
		private int priority;

		// Token: 0x04001663 RID: 5731
		[SerializeField]
		private Color _startColor;

		// Token: 0x04001664 RID: 5732
		[SerializeField]
		private Color _endColor;

		// Token: 0x04001665 RID: 5733
		[SerializeField]
		private Curve _curve;

		// Token: 0x04001666 RID: 5734
		[SerializeField]
		private Health.HealthGiverType _giverType = Health.HealthGiverType.Potion;

		// Token: 0x04001667 RID: 5735
		[SerializeField]
		[Subcomponent(typeof(SpawnEffect))]
		private SpawnEffect _spawn;
	}
}
