using System;
using Characters;
using Characters.Abilities;
using Characters.Operations.Fx;
using FX.SpriteEffects;
using GameResources;
using Scenes;
using Singletons;
using UI.GearPopup;
using UnityEditor;
using UnityEngine;

namespace Level
{
	// Token: 0x020004AD RID: 1197
	public sealed class DeathKnightReward : DroppedPurchasableReward
	{
		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x06001705 RID: 5893 RVA: 0x000485D1 File Offset: 0x000467D1
		private new string _keyBase
		{
			get
			{
				return string.Format("{0}/{1}/{2}", "DroppedPurchasableReward", "DeathKnightReward", this._abilityName);
			}
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06001706 RID: 5894 RVA: 0x000485F2 File Offset: 0x000467F2
		public new string displayName
		{
			get
			{
				return Localization.GetLocalizedString(this._keyBase + "/name");
			}
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x06001707 RID: 5895 RVA: 0x00048609 File Offset: 0x00046809
		public new string description
		{
			get
			{
				return Localization.GetLocalizedString(this._keyBase + "/desc");
			}
		}

		// Token: 0x06001708 RID: 5896 RVA: 0x00048620 File Offset: 0x00046820
		public override void InteractWith(Character character)
		{
			base.InteractWith(character);
			character.playerComponents.savableAbilityManager.Apply(this._abilityName, this._stack, (float)((this._stack < 30f) ? 0 : 2));
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._interactSound, base.transform.position);
			this._getEffect.Run(character);
			character.spriteEffectStack.Add(new EasedColorBlend(this._priority, this._startColor, this._endColor, this._curve));
			this.ClosePopup();
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06001709 RID: 5897 RVA: 0x000486C4 File Offset: 0x000468C4
		public override void OpenPopupBy(Character character)
		{
			base.OpenPopupBy(character);
			Vector3 position = base.transform.position;
			Vector3 position2 = character.transform.position;
			position.x = position2.x + ((position.x > position2.x) ? InteractiveObject._popupUIOffset.x : (-InteractiveObject._popupUIOffset.x));
			position.y += InteractiveObject._popupUIOffset.y;
			GearPopupCanvas gearPopupCanvas = Scene<GameBase>.instance.uiManager.gearPopupCanvas;
			gearPopupCanvas.gearPopup.Set(this.displayName, this.description);
			gearPopupCanvas.gearPopup.SetInteractionLabelAsLoot();
			Scene<GameBase>.instance.uiManager.gearPopupCanvas.Open(position);
		}

		// Token: 0x0600170A RID: 5898 RVA: 0x00047EF0 File Offset: 0x000460F0
		public override void ClosePopup()
		{
			base.ClosePopup();
			Scene<GameBase>.instance.uiManager.gearPopupCanvas.Close();
		}

		// Token: 0x0600170B RID: 5899 RVA: 0x00002191 File Offset: 0x00000391
		public void Initialize()
		{
		}

		// Token: 0x04001425 RID: 5157
		[SerializeField]
		private string _nameKey;

		// Token: 0x04001426 RID: 5158
		[SerializeField]
		private string _descriptionKey;

		// Token: 0x04001427 RID: 5159
		[SerializeField]
		private SavableAbilityManager.Name _abilityName;

		// Token: 0x04001428 RID: 5160
		[SerializeField]
		private float _stack;

		// Token: 0x04001429 RID: 5161
		[SerializeField]
		private int _priority;

		// Token: 0x0400142A RID: 5162
		[SerializeField]
		private Color _startColor;

		// Token: 0x0400142B RID: 5163
		[SerializeField]
		private Color _endColor;

		// Token: 0x0400142C RID: 5164
		[SerializeField]
		private Curve _curve;

		// Token: 0x0400142D RID: 5165
		[Subcomponent(typeof(SpawnEffect))]
		[SerializeField]
		private SpawnEffect _getEffect;

		// Token: 0x0400142E RID: 5166
		private new const string _prefix = "DroppedPurchasableReward";

		// Token: 0x0400142F RID: 5167
		private const string _prefix2 = "DeathKnightReward";
	}
}
