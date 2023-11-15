using System;
using System.Collections.Generic;
using flanne.RuneSystem;
using UnityEngine;
using UnityEngine.UI;

namespace flanne
{
	// Token: 0x020000C7 RID: 199
	public class LoadoutInitializer : MonoBehaviour
	{
		// Token: 0x06000650 RID: 1616 RVA: 0x0001CE44 File Offset: 0x0001B044
		private void Start()
		{
			CharacterData characterSelection = Loadout.CharacterSelection;
			if (characterSelection == null)
			{
				characterSelection = this.defaultCharacter;
			}
			this.playerAnimator.runtimeAnimatorController = characterSelection.animController;
			this.playerHealth.baseMaxHP = characterSelection.startHP;
			if (characterSelection.passivePrefab != null)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(characterSelection.passivePrefab);
				gameObject.transform.SetParent(this.player.transform.root);
				gameObject.transform.localPosition = Vector3.zero;
			}
			this.player.loadedCharacter = characterSelection;
			this.powerupGenerator.SetCharacterPowerupPool(characterSelection.exclusivePowerups);
			this.characterIcon.sprite = characterSelection.icon;
			GunData gunSelection = Loadout.GunSelection;
			this.gun.LoadGun(gunSelection);
			if (gunSelection != null)
			{
				this.gunIcon.sprite = gunSelection.icon;
			}
			MapData mapData = SelectedMap.MapData;
			if (mapData != null && !mapData.runesDisabled)
			{
				List<RuneData> runeSelection = Loadout.RuneSelection;
				if (runeSelection != null)
				{
					foreach (RuneData runeData in runeSelection)
					{
						runeData.Apply(this.player);
					}
				}
			}
		}

		// Token: 0x04000414 RID: 1044
		[SerializeField]
		private CharacterData defaultCharacter;

		// Token: 0x04000415 RID: 1045
		[SerializeField]
		private PlayerController player;

		// Token: 0x04000416 RID: 1046
		[SerializeField]
		private Animator playerAnimator;

		// Token: 0x04000417 RID: 1047
		[SerializeField]
		private PlayerHealth playerHealth;

		// Token: 0x04000418 RID: 1048
		[SerializeField]
		private PowerupGenerator powerupGenerator;

		// Token: 0x04000419 RID: 1049
		[SerializeField]
		private Gun gun;

		// Token: 0x0400041A RID: 1050
		[SerializeField]
		private Image characterIcon;

		// Token: 0x0400041B RID: 1051
		[SerializeField]
		private Image gunIcon;
	}
}
