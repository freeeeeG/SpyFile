using System;
using System.Collections;
using System.Collections.Generic;
using flanne.UI;
using UnityEngine;

namespace flanne.Core
{
	// Token: 0x020001F1 RID: 497
	public class GunEvoMenuState : GameState
	{
		// Token: 0x06000B35 RID: 2869 RVA: 0x0002A368 File Offset: 0x00028568
		private void OnEvoClicked(object sender, InfoEventArgs<int> e)
		{
			GunEvolution data = base.gunEvoMenu.GetEntry(e.info).GetComponent<GunEvoUI>().data;
			base.StartCoroutine(this.EndGunEvoAnimationCR(data));
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x0002A39F File Offset: 0x0002859F
		public override void Enter()
		{
			base.pauseController.Pause();
			AudioManager.Instance.SetLowPassFilter(true);
			base.StartCoroutine(this.PlayGunEvoAnimationCR());
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x0002A3C4 File Offset: 0x000285C4
		public override void Exit()
		{
			base.gunEvoMenu.ClickEvent -= this.OnEvoClicked;
			AudioManager.Instance.SetLowPassFilter(false);
			base.pauseController.UnPause();
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x0002A3F4 File Offset: 0x000285F4
		private void GenerateEvolutions()
		{
			int num = 3;
			List<GunEvolution> list = new List<GunEvolution>();
			List<GunEvolution> gunEvolutions;
			if (Loadout.GunSelection != null)
			{
				gunEvolutions = Loadout.GunSelection.gunEvolutions;
			}
			else
			{
				gunEvolutions = PlayerController.Instance.gun.gunData.gunEvolutions;
			}
			for (int i = 0; i < num; i++)
			{
				GunEvolution gunEvolution = null;
				while (gunEvolution == null)
				{
					GunEvolution gunEvolution2 = gunEvolutions[Random.Range(0, gunEvolutions.Count)];
					if (!list.Contains(gunEvolution2))
					{
						gunEvolution = gunEvolution2;
					}
				}
				base.gunEvoMenu.GetEntry(i).GetComponent<GunEvoUI>().Set(gunEvolution);
				list.Add(gunEvolution);
			}
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x0002A495 File Offset: 0x00028695
		private IEnumerator PlayGunEvoAnimationCR()
		{
			base.screenFlash.Flash(1);
			PlayerController.Instance.gun.PlayGunEvoAnimation();
			base.gunEvoStartSFX.Play(null);
			yield return new WaitForSecondsRealtime(0.5f);
			base.screenFlash.Flash(1);
			this.GenerateEvolutions();
			base.gunEvoPanel.Show();
			base.gunEvoMenuSFX.Play(null);
			yield return new WaitForSecondsRealtime(0.2f);
			base.gunEvoMenu.ClickEvent += this.OnEvoClicked;
			yield break;
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x0002A4A4 File Offset: 0x000286A4
		private IEnumerator EndGunEvoAnimationCR(GunEvolution evo)
		{
			base.gunEvoPanel.Hide();
			yield return new WaitForSecondsRealtime(0.2f);
			PlayerController.Instance.gun.EndGunEvoAnimation();
			base.gunEvoEndSFX.Play(null);
			yield return null;
			PlayerController.Instance.playerPerks.Equip(evo);
			yield return new WaitForSecondsRealtime(1f);
			this.owner.ChangeState<CombatState>();
			yield break;
		}

		// Token: 0x040007E4 RID: 2020
		private List<Powerup> powerupChoices;
	}
}
