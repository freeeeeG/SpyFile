using System;
using System.Collections;
using Characters;
using Data;
using FX;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace Level.Npc.FieldNpcs
{
	// Token: 0x020005CB RID: 1483
	public class DarkPriest : FieldNpc
	{
		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x06001D8D RID: 7565 RVA: 0x0005A32F File Offset: 0x0005852F
		protected override NpcType _type
		{
			get
			{
				return this._npcType;
			}
		}

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x06001D8E RID: 7566 RVA: 0x0005A337 File Offset: 0x00058537
		private int[] _goldCosts
		{
			get
			{
				return Singleton<Service>.Instance.levelManager.currentChapter.currentStage.fieldNpcSettings.darkPriestGoldCosts;
			}
		}

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x06001D8F RID: 7567 RVA: 0x0005A357 File Offset: 0x00058557
		private int _goldCost
		{
			get
			{
				return this._goldCosts[this._goldCostIndex];
			}
		}

		// Token: 0x06001D90 RID: 7568 RVA: 0x0005A366 File Offset: 0x00058566
		protected override void Interact(Character character)
		{
			base.Interact(character);
			base.StartCoroutine(this.CGreetingAndConfirm(character, this._goldCost));
		}

		// Token: 0x06001D91 RID: 7569 RVA: 0x0005A388 File Offset: 0x00058588
		private IEnumerator CGreetingAndConfirm(Character character, object confirmArg = null)
		{
			yield return LetterBox.instance.CAppear(0.4f);
			string[] scripts = (this._phase == FieldNpc.Phase.Initial) ? base._greeting : base._regreeting;
			this._phase = FieldNpc.Phase.Greeted;
			this._npcConversation.skippable = true;
			int lastIndex = scripts.Length - 1;
			int num;
			for (int i = 0; i < lastIndex; i = num + 1)
			{
				yield return this._npcConversation.CConversation(new string[]
				{
					scripts[i]
				});
				num = i;
			}
			this._npcConversation.skippable = true;
			this._npcConversation.body = ((confirmArg == null) ? scripts[lastIndex] : string.Format(scripts[lastIndex], confirmArg));
			this._npcConversation.OpenCurrencyBalancePanel(GameData.Currency.Type.Gold);
			yield return this._npcConversation.CType();
			yield return new WaitForSecondsRealtime(0.3f);
			this._npcConversation.OpenConfirmSelector(delegate
			{
				this.OnConfirmed(character);
			}, new Action(base.Close));
			yield break;
		}

		// Token: 0x06001D92 RID: 7570 RVA: 0x0005A3A8 File Offset: 0x000585A8
		private void OnConfirmed(Character character)
		{
			DarkPriest.<>c__DisplayClass15_0 CS$<>8__locals1 = new DarkPriest.<>c__DisplayClass15_0();
			CS$<>8__locals1.character = character;
			CS$<>8__locals1.<>4__this = this;
			this._npcConversation.CloseCurrencyBalancePanel();
			if (GameData.Currency.gold.Consume(this._goldCost))
			{
				if (this._goldCostIndex < this._goldCosts.Length - 1)
				{
					this._goldCostIndex++;
				}
				base.StartCoroutine(CS$<>8__locals1.<OnConfirmed>g__CRerollSkills|0());
				return;
			}
			base.StartCoroutine(CS$<>8__locals1.<OnConfirmed>g__CNoMoneyAndClose|1());
		}

		// Token: 0x0400190A RID: 6410
		[SerializeField]
		private NpcType _npcType;

		// Token: 0x0400190B RID: 6411
		[Space]
		[SerializeField]
		private EffectInfo _frontEffect;

		// Token: 0x0400190C RID: 6412
		[SerializeField]
		private EffectInfo _behindEffect;

		// Token: 0x0400190D RID: 6413
		[SerializeField]
		private SoundInfo _sound;

		// Token: 0x0400190E RID: 6414
		[SerializeField]
		private float _effectShowingDuration;

		// Token: 0x0400190F RID: 6415
		[SerializeField]
		private SkillChangingEffect _skillChangingEffect;

		// Token: 0x04001910 RID: 6416
		private int _goldCostIndex;
	}
}
