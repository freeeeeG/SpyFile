using System;
using System.Collections;
using Characters.Actions;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x02001033 RID: 4147
	public class AdventurerThiefBunshin : AIController
	{
		// Token: 0x06004FF5 RID: 20469 RVA: 0x000F158A File Offset: 0x000EF78A
		protected override void OnEnable()
		{
			this.Run();
		}

		// Token: 0x06004FF6 RID: 20470 RVA: 0x000F1592 File Offset: 0x000EF792
		protected override void OnDisable()
		{
			this.Hide();
		}

		// Token: 0x06004FF7 RID: 20471 RVA: 0x000F159A File Offset: 0x000EF79A
		public void Run()
		{
			this.Show();
			this.character.animationController.ForceUpdate();
			base.StartCoroutine(this.CProcess());
		}

		// Token: 0x06004FF8 RID: 20472 RVA: 0x000F15BF File Offset: 0x000EF7BF
		private void Show()
		{
			this.character.gameObject.SetActive(true);
		}

		// Token: 0x06004FF9 RID: 20473 RVA: 0x000F15D2 File Offset: 0x000EF7D2
		private void Hide()
		{
			this.character.gameObject.SetActive(false);
		}

		// Token: 0x06004FFA RID: 20474 RVA: 0x000F15E5 File Offset: 0x000EF7E5
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			this.character.ForceToLookAt(base.target.transform.position.x);
			yield return Chronometer.global.WaitForSeconds(1f);
			if (MMMaths.RandomBool())
			{
				yield return this.CastFlashCut();
			}
			else
			{
				yield return this.CastSuriken();
			}
			this.character.animationController.ForceUpdate();
			this._despawnAction.TryStart();
			while (this._despawnAction.running)
			{
				yield return null;
			}
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x06004FFB RID: 20475 RVA: 0x000F15F4 File Offset: 0x000EF7F4
		private IEnumerator CastFlashCut()
		{
			yield return this._teleportBehind.CRun(this);
			if (this.character.transform.position.x > base.target.transform.position.x)
			{
				this.character.lookingDirection = Character.LookingDirection.Left;
			}
			else
			{
				this.character.lookingDirection = Character.LookingDirection.Right;
			}
			yield return this._flashCut.CRun(this);
			yield break;
		}

		// Token: 0x06004FFC RID: 20476 RVA: 0x000F1603 File Offset: 0x000EF803
		private IEnumerator CastSuriken()
		{
			if (this.character.transform.position.x > base.target.transform.position.x)
			{
				this.character.lookingDirection = Character.LookingDirection.Left;
			}
			else
			{
				this.character.lookingDirection = Character.LookingDirection.Right;
			}
			yield return this._surikenJump.CRun(this);
			yield break;
		}

		// Token: 0x0400405B RID: 16475
		[SerializeField]
		[Subcomponent(typeof(ActionAttack))]
		[Space]
		[Header("Flashcut")]
		private ActionAttack _flashCut;

		// Token: 0x0400405C RID: 16476
		[SerializeField]
		[Subcomponent(typeof(TeleportBehind))]
		private TeleportBehind _teleportBehind;

		// Token: 0x0400405D RID: 16477
		[SerializeField]
		[Space]
		[Subcomponent(typeof(Jump))]
		[Header("Shuriken")]
		private Jump _surikenJump;

		// Token: 0x0400405E RID: 16478
		[Header("Despawn Action")]
		[SerializeField]
		private Characters.Actions.Action _despawnAction;
	}
}
