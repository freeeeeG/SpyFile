using System;
using Characters.Gear.Weapons;
using FX;
using GameResources;
using Level;
using Runnables.Triggers;
using Scenes;
using Singletons;
using UI.GearPopup;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters.Abilities.Weapons
{
	// Token: 0x02000BF5 RID: 3061
	public class PrisonerSkillScroll : InteractiveObject
	{
		// Token: 0x06003EDF RID: 16095 RVA: 0x000B6AC9 File Offset: 0x000B4CC9
		protected override void Awake()
		{
			base.Awake();
			this._sortingGroup.sortingOrder = (int)PrisonerSkillScroll._sortingOrder;
			PrisonerSkillScroll._sortingOrder += 1;
			this._dropMovement.onGround += this.OnGround;
		}

		// Token: 0x06003EE0 RID: 16096 RVA: 0x000B6B05 File Offset: 0x000B4D05
		private void OnGround()
		{
			base.gameObject.layer = 12;
			base.Activate();
		}

		// Token: 0x06003EE1 RID: 16097 RVA: 0x000B6B1C File Offset: 0x000B4D1C
		private void UpdateGearPopup(GearPopup gearPopup)
		{
			gearPopup.Set(this._skillInfo.displayName, this._skillInfo.description);
			string localizedString = Localization.GetLocalizedString("label/interaction/replace");
			string displayName = this._weapon.currentSkills[0].displayName;
			string displayName2 = this._weapon.currentSkills[1].displayName;
			PrisonerSkill component = this._weapon.currentSkills[0].GetComponent<PrisonerSkill>();
			PrisonerSkill component2 = this._weapon.currentSkills[1].GetComponent<PrisonerSkill>();
			if (component.parent == this._skills)
			{
				this._interactionType = CharacterInteraction.InteractionType.Normal;
				gearPopup.SetInteractionLabel(localizedString + " (" + displayName + ")");
				return;
			}
			if (component2.parent == this._skills)
			{
				this._interactionType = CharacterInteraction.InteractionType.Normal;
				gearPopup.SetInteractionLabel(localizedString + " (" + displayName2 + ")");
				return;
			}
			this._interactionType = CharacterInteraction.InteractionType.Pressing;
			gearPopup.SetInteractionLabel(this, localizedString + " (" + displayName + ")", localizedString + " (" + displayName2 + ")");
		}

		// Token: 0x06003EE2 RID: 16098 RVA: 0x000B6C3E File Offset: 0x000B4E3E
		public void SetSkillInfo(Weapon weapon, PrisonerSkillInfosByGrade skills, SkillInfo skillInfo)
		{
			this._weapon = weapon;
			this._skillInfo = skillInfo;
			this._skills = skills;
			this._iconHolder.sprite = this._skillInfo.cachedIcon;
		}

		// Token: 0x06003EE3 RID: 16099 RVA: 0x000B6C6C File Offset: 0x000B4E6C
		public override void OpenPopupBy(Character character)
		{
			if (!this._activationTrigger.IsSatisfied())
			{
				return;
			}
			this.pressingPercent = 0f;
			Vector3 position = base.transform.position;
			Vector3 position2 = character.transform.position;
			position.x = position2.x + ((position.x > position2.x) ? InteractiveObject._popupUIOffset.x : (-InteractiveObject._popupUIOffset.x));
			position.y += InteractiveObject._popupUIOffset.y;
			GearPopupCanvas gearPopupCanvas = Scene<GameBase>.instance.uiManager.gearPopupCanvas;
			this.UpdateGearPopup(gearPopupCanvas.gearPopup);
			gearPopupCanvas.Open(position);
		}

		// Token: 0x06003EE4 RID: 16100 RVA: 0x00047EF0 File Offset: 0x000460F0
		public override void ClosePopup()
		{
			base.ClosePopup();
			Scene<GameBase>.instance.uiManager.gearPopupCanvas.Close();
		}

		// Token: 0x06003EE5 RID: 16101 RVA: 0x000B6D18 File Offset: 0x000B4F18
		public override void InteractWith(Character character)
		{
			if (!this._activationTrigger.IsSatisfied())
			{
				return;
			}
			PrisonerSkill component = this._weapon.currentSkills[0].GetComponent<PrisonerSkill>();
			PrisonerSkill component2 = this._weapon.currentSkills[1].GetComponent<PrisonerSkill>();
			if (component.parent == this._skills)
			{
				this.ApplyScroll(0);
				return;
			}
			if (component2.parent == this._skills)
			{
				this.ApplyScroll(1);
				return;
			}
			this.ApplyScroll(0);
		}

		// Token: 0x06003EE6 RID: 16102 RVA: 0x000B6D9C File Offset: 0x000B4F9C
		public override void InteractWithByPressing(Character character)
		{
			if (!this._activationTrigger.IsSatisfied())
			{
				return;
			}
			this.ApplyScroll(1);
		}

		// Token: 0x06003EE7 RID: 16103 RVA: 0x000B6DB4 File Offset: 0x000B4FB4
		private void ApplyScroll(int targetSkillIndex)
		{
			this.ReplaceSkill(targetSkillIndex);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._lootSound, base.transform.position);
			base.gameObject.layer = 0;
			base.Deactivate();
			this._dropMovement.Move();
		}

		// Token: 0x06003EE8 RID: 16104 RVA: 0x000B6E04 File Offset: 0x000B5004
		private void ReplaceSkill(int targetSkillIndex)
		{
			PrisonerSkill component = this._weapon.currentSkills[targetSkillIndex].GetComponent<PrisonerSkill>();
			SkillInfo skillInfo = this._weapon.currentSkills[targetSkillIndex];
			this._weapon.currentSkills[targetSkillIndex] = this._skillInfo;
			this.SetSkillInfo(this._weapon, component.parent, skillInfo);
			this._weapon.SetSkillButtons();
			GearPopupCanvas gearPopupCanvas = Scene<GameBase>.instance.uiManager.gearPopupCanvas;
			this.UpdateGearPopup(gearPopupCanvas.gearPopup);
		}

		// Token: 0x04003085 RID: 12421
		private static short _sortingOrder;

		// Token: 0x04003086 RID: 12422
		[SerializeField]
		private SoundInfo _lootSound;

		// Token: 0x04003087 RID: 12423
		[Space]
		[SerializeField]
		private SpriteRenderer _iconHolder;

		// Token: 0x04003088 RID: 12424
		[SerializeField]
		private DropMovement _dropMovement;

		// Token: 0x04003089 RID: 12425
		[SerializeField]
		private SortingGroup _sortingGroup;

		// Token: 0x0400308A RID: 12426
		[Trigger.SubcomponentAttribute]
		[SerializeField]
		private Trigger _activationTrigger;

		// Token: 0x0400308B RID: 12427
		private Weapon _weapon;

		// Token: 0x0400308C RID: 12428
		private PrisonerSkillInfosByGrade _skills;

		// Token: 0x0400308D RID: 12429
		private SkillInfo _skillInfo;
	}
}
