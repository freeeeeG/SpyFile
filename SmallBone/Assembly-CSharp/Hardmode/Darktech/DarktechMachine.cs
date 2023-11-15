using System;
using System.Collections;
using Characters;
using Characters.Operations.Fx;
using GameResources;
using Scenes;
using Services;
using Singletons;
using UI;
using UnityEditor;
using UnityEngine;

namespace Hardmode.Darktech
{
	// Token: 0x02000158 RID: 344
	public sealed class DarktechMachine : InteractiveObject
	{
		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060006CA RID: 1738 RVA: 0x00013A4A File Offset: 0x00011C4A
		private new string name
		{
			get
			{
				return Localization.GetLocalizedString(string.Format("darktech/equipment/{0}/{1}", this._darktech.type, "name"));
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060006CB RID: 1739 RVA: 0x00013A70 File Offset: 0x00011C70
		private string body
		{
			get
			{
				return Localization.GetLocalizedString(string.Format("darktech/equipment/{0}/body", this._darktech.type));
			}
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x00013A94 File Offset: 0x00011C94
		private void Start()
		{
			if (!Singleton<DarktechManager>.Instance.IsUnlocked(this._darktech.type))
			{
				base.gameObject.SetActive(false);
			}
			if (Singleton<DarktechManager>.Instance.IsActivated(this._darktech.type))
			{
				this.ActivateMachine();
			}
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x00013AE4 File Offset: 0x00011CE4
		private void Update()
		{
			if (this._introWaiting && this._animator.GetCurrentAnimatorStateInfo(0).IsName(this._introName) && this._animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f)
			{
				this._loopSound.Run(Singleton<Service>.Instance.levelManager.player);
				this._introWaiting = false;
			}
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x00013B51 File Offset: 0x00011D51
		public void ActivateMachine()
		{
			this._animator.Play(this._introHash, 0, 0f);
			Singleton<DarktechManager>.Instance.ActivateDarktech(this._darktech.type);
			this._introWaiting = true;
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x00013B86 File Offset: 0x00011D86
		public override void InteractWith(Character character)
		{
			if (!Singleton<DarktechManager>.Instance.IsActivated(this._darktech.type))
			{
				this.ActivateMachine();
				return;
			}
			base.StartCoroutine(this.CTalk());
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x00013BB3 File Offset: 0x00011DB3
		private IEnumerator CTalk()
		{
			SystemDialogue ui = Scene<GameBase>.instance.uiManager.systemDialogue;
			yield return LetterBox.instance.CAppear(0.4f);
			yield return ui.CShow(this.name, this.body, true);
			LetterBox.instance.Disappear(0.4f);
			yield break;
		}

		// Token: 0x04000502 RID: 1282
		private readonly string _introName = "Intro";

		// Token: 0x04000503 RID: 1283
		private readonly int _introHash = Animator.StringToHash("Intro");

		// Token: 0x04000504 RID: 1284
		[SerializeField]
		private DarktechData _darktech;

		// Token: 0x04000505 RID: 1285
		[SerializeField]
		private Animator _animator;

		// Token: 0x04000506 RID: 1286
		[SerializeField]
		[Subcomponent(typeof(PlaySound))]
		private PlaySound _loopSound;

		// Token: 0x04000507 RID: 1287
		private bool _introWaiting;
	}
}
