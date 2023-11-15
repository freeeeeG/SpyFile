using System;
using Characters;
using Scenes;
using Services;
using Singletons;
using UnityEngine;
using UserInput;

namespace UI.Witch
{
	// Token: 0x020003E7 RID: 999
	public class Panel : Dialogue
	{
		// Token: 0x170003CC RID: 972
		// (get) Token: 0x060012A6 RID: 4774 RVA: 0x00018EC5 File Offset: 0x000170C5
		public override bool closeWithPauseKey
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x00037CCC File Offset: 0x00035ECC
		protected override void OnEnable()
		{
			base.OnEnable();
			Character player = Singleton<Service>.Instance.levelManager.player;
			this._witchBonus = WitchBonus.instance;
			this._skull.Initialize(this);
			this._body.Initialize(this);
			this._soul.Initialize(this);
			this._skull.Set(this._witchBonus.skull);
			this._body.Set(this._witchBonus.body);
			this._soul.Set(this._witchBonus.soul);
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x00037D60 File Offset: 0x00035F60
		protected override void OnDisable()
		{
			if (Service.quitting)
			{
				return;
			}
			base.OnDisable();
			LetterBox.instance.Disappear(0.4f);
		}

		// Token: 0x060012A9 RID: 4777 RVA: 0x00037D7F File Offset: 0x00035F7F
		private new void Update()
		{
			if (KeyMapper.Map.Cancel.WasPressed)
			{
				Scene<GameBase>.instance.uiManager.npcConversation.visible = false;
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x00037DB3 File Offset: 0x00035FB3
		public void Set(WitchBonus.Bonus bonus)
		{
			this._bonus = bonus;
			this._currentOption.Set(bonus);
		}

		// Token: 0x060012AB RID: 4779 RVA: 0x00037DC8 File Offset: 0x00035FC8
		public void UpdateCurrentOption()
		{
			this._currentOption.UpdateTexts();
		}

		// Token: 0x04000FA5 RID: 4005
		[SerializeField]
		private Tree _skull;

		// Token: 0x04000FA6 RID: 4006
		[SerializeField]
		private Tree _body;

		// Token: 0x04000FA7 RID: 4007
		[SerializeField]
		private Tree _soul;

		// Token: 0x04000FA8 RID: 4008
		[SerializeField]
		private Option _currentOption;

		// Token: 0x04000FA9 RID: 4009
		private WitchBonus _witchBonus;

		// Token: 0x04000FAA RID: 4010
		private WitchBonus.Bonus _bonus;
	}
}
