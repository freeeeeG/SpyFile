using System;
using System.Collections;
using Characters;
using Characters.Gear.Weapons;
using Characters.Operations;
using Characters.Operations.Fx;
using Data;
using GameResources;
using Runnables;
using Scenes;
using Services;
using Singletons;
using UI;
using UnityEditor;
using UnityEngine;

namespace Hardmode.Darktech
{
	// Token: 0x02000164 RID: 356
	public sealed class DimensionMirrorObservationInstrument : InteractiveObject
	{
		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000712 RID: 1810 RVA: 0x00014347 File Offset: 0x00012547
		private new string name
		{
			get
			{
				return Localization.GetLocalizedString(string.Format("darktech/equipment/{0}/{1}", this._type, "name"));
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000713 RID: 1811 RVA: 0x00014368 File Offset: 0x00012568
		private string body
		{
			get
			{
				return Localization.GetLocalizedString(string.Format("darktech/equipment/{0}/body", this._type));
			}
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x00014384 File Offset: 0x00012584
		private void Start()
		{
			this._onChange.Initialize();
			this._onHeroToSkul.Initialize();
			if (!Singleton<DarktechManager>.Instance.IsUnlocked(this._type))
			{
				base.gameObject.SetActive(false);
			}
			if (Singleton<DarktechManager>.Instance.IsActivated(this._type))
			{
				this.ActivateMachine();
			}
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x000143E0 File Offset: 0x000125E0
		private void Update()
		{
			if (this._character == null)
			{
				return;
			}
			Character player = Singleton<Service>.Instance.levelManager.player;
			if (player.playerComponents.inventory.weapon.current.name.Equals(this._skulName) || player.playerComponents.inventory.weapon.current.name.Equals(this._heroSkulName))
			{
				this._uiObject.SetActive(true);
			}
			else
			{
				this._uiObject.SetActive(false);
			}
			if (this._introWaiting && this._animator.GetCurrentAnimatorStateInfo(0).IsName(this._introName) && this._animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f)
			{
				this._loopSound.Run(Singleton<Service>.Instance.levelManager.player);
				this._introWaiting = false;
			}
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x000144D4 File Offset: 0x000126D4
		public void ActivateMachine()
		{
			this._animator.Play(this._introHash, 0, 0f);
			Singleton<DarktechManager>.Instance.ActivateDarktech(this._type);
			this._introWaiting = true;
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x00014504 File Offset: 0x00012704
		public override void InteractWith(Character character)
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			if (!player.playerComponents.inventory.weapon.current.name.Equals(this._skulName) && !player.playerComponents.inventory.weapon.current.name.Equals(this._heroSkulName))
			{
				return;
			}
			if (!Singleton<DarktechManager>.Instance.IsActivated(DarktechData.Type.ObservationInstrument))
			{
				this._animator.Play(this._introHash, 0, 0f);
				Singleton<DarktechManager>.Instance.ActivateDarktech(this._type);
				base.StartCoroutine(this.CTalk());
				return;
			}
			base.StartCoroutine(this.CTalk());
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x000145C0 File Offset: 0x000127C0
		private IEnumerator CTalk()
		{
			SystemDialogue ui = Scene<GameBase>.instance.uiManager.systemDialogue;
			yield return LetterBox.instance.CAppear(0.4f);
			yield return ui.CShow(this.name, this.body, true);
			Character player = Singleton<Service>.Instance.levelManager.player;
			base.StartCoroutine(this._onChange.CRun(player));
			if (GameData.Generic.skinIndex == 1)
			{
				LetterBox.instance.Disappear(0.4f);
				GameData.Generic.skinIndex = 0;
				Singleton<Service>.Instance.levelManager.skulSpawnAnimaionEnable = false;
				base.StartCoroutine(this._onHeroToSkul.CRun(player));
				player.playerComponents.inventory.weapon.UpdateSkin();
			}
			else
			{
				GameData.Generic.skinIndex = 1;
				this._onChangetoHero.Run();
			}
			GameData.Generic.SaveSkin();
			yield break;
		}

		// Token: 0x04000541 RID: 1345
		private readonly string _introName = "Intro";

		// Token: 0x04000542 RID: 1346
		private readonly int _introHash = Animator.StringToHash("Intro");

		// Token: 0x04000543 RID: 1347
		private readonly string _skulName = "Skul";

		// Token: 0x04000544 RID: 1348
		private readonly string _heroSkulName = "HeroSkul";

		// Token: 0x04000545 RID: 1349
		[SerializeField]
		private Animator _animator;

		// Token: 0x04000546 RID: 1350
		[SerializeField]
		private Weapon _weapon;

		// Token: 0x04000547 RID: 1351
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _onChange;

		// Token: 0x04000548 RID: 1352
		[SerializeField]
		private Runnable _onChangetoHero;

		// Token: 0x04000549 RID: 1353
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _onHeroToSkul;

		// Token: 0x0400054A RID: 1354
		private DarktechData.Type _type = DarktechData.Type.ObservationInstrument;

		// Token: 0x0400054B RID: 1355
		[Subcomponent(typeof(PlaySound))]
		[SerializeField]
		private PlaySound _loopSound;

		// Token: 0x0400054C RID: 1356
		private bool _introWaiting;
	}
}
