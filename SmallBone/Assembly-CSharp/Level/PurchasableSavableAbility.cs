using System;
using Characters;
using Characters.Abilities;
using Characters.Operations.Fx;
using FX.SpriteEffects;
using Scenes;
using Singletons;
using UI.GearPopup;
using UnityEditor;
using UnityEngine;

namespace Level
{
	// Token: 0x02000513 RID: 1299
	public sealed class PurchasableSavableAbility : DroppedPurchasableReward
	{
		// Token: 0x0600199C RID: 6556 RVA: 0x00050668 File Offset: 0x0004E868
		public override void InteractWith(Character character)
		{
			base.InteractWith(character);
			if (base.price != 0)
			{
				return;
			}
			character.playerComponents.savableAbilityManager.IncreaseStack(this._abilityName, this._stack);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._interactSound, base.transform.position);
			this._getEffect.Run(character);
			character.spriteEffectStack.Add(new EasedColorBlend(this._priority, this._startColor, this._endColor, this._curve));
			this.ClosePopup();
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x0600199D RID: 6557 RVA: 0x00050704 File Offset: 0x0004E904
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

		// Token: 0x0600199E RID: 6558 RVA: 0x00047EF0 File Offset: 0x000460F0
		public override void ClosePopup()
		{
			base.ClosePopup();
			Scene<GameBase>.instance.uiManager.gearPopupCanvas.Close();
		}

		// Token: 0x0600199F RID: 6559 RVA: 0x00002191 File Offset: 0x00000391
		public void Initialize()
		{
		}

		// Token: 0x04001668 RID: 5736
		[SerializeField]
		private SavableAbilityManager.Name _abilityName;

		// Token: 0x04001669 RID: 5737
		[SerializeField]
		private float _stack;

		// Token: 0x0400166A RID: 5738
		[SerializeField]
		private int _priority;

		// Token: 0x0400166B RID: 5739
		[SerializeField]
		private Color _startColor;

		// Token: 0x0400166C RID: 5740
		[SerializeField]
		private Color _endColor;

		// Token: 0x0400166D RID: 5741
		[SerializeField]
		private Curve _curve;

		// Token: 0x0400166E RID: 5742
		[Subcomponent(typeof(SpawnEffect))]
		[SerializeField]
		private SpawnEffect _getEffect;
	}
}
