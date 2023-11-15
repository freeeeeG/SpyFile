using System;
using System.Diagnostics;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000DF1 RID: 3569
	[DebuggerDisplay("{effect.Id}")]
	public class EffectInstance : ModifierInstance<Effect>
	{
		// Token: 0x06006DA3 RID: 28067 RVA: 0x002B3D18 File Offset: 0x002B1F18
		public EffectInstance(GameObject game_object, Effect effect, bool should_save) : base(game_object, effect)
		{
			this.effect = effect;
			this.shouldSave = should_save;
			this.ConfigureStatusItem();
			if (effect.showInUI)
			{
				KSelectable component = base.gameObject.GetComponent<KSelectable>();
				if (!component.GetStatusItemGroup().HasStatusItem(this.statusItem))
				{
					component.AddStatusItem(this.statusItem, this);
				}
			}
			if (effect.triggerFloatingText && PopFXManager.Instance != null)
			{
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, effect.Name, game_object.transform, 1.5f, false);
			}
			if (effect.emote != null)
			{
				this.RegisterEmote(effect.emote, effect.emoteCooldown);
			}
			if (!string.IsNullOrEmpty(effect.emoteAnim))
			{
				this.RegisterEmote(effect.emoteAnim, effect.emoteCooldown);
			}
		}

		// Token: 0x06006DA4 RID: 28068 RVA: 0x002B3DEC File Offset: 0x002B1FEC
		public void RegisterEmote(string emoteAnim, float cooldown = -1f)
		{
			ReactionMonitor.Instance smi = base.gameObject.GetSMI<ReactionMonitor.Instance>();
			if (smi == null)
			{
				return;
			}
			bool flag = cooldown < 0f;
			float globalCooldown = flag ? 100000f : cooldown;
			EmoteReactable emoteReactable = smi.AddSelfEmoteReactable(base.gameObject, this.effect.Name + "_Emote", emoteAnim, flag, Db.Get().ChoreTypes.Emote, globalCooldown, 20f, float.NegativeInfinity, this.effect.maxInitialDelay, this.effect.emotePreconditions);
			if (emoteReactable == null)
			{
				return;
			}
			emoteReactable.InsertPrecondition(0, new Reactable.ReactablePrecondition(this.NotInATube));
			if (!flag)
			{
				this.reactable = emoteReactable;
			}
		}

		// Token: 0x06006DA5 RID: 28069 RVA: 0x002B3E94 File Offset: 0x002B2094
		public void RegisterEmote(Emote emote, float cooldown = -1f)
		{
			ReactionMonitor.Instance smi = base.gameObject.GetSMI<ReactionMonitor.Instance>();
			if (smi == null)
			{
				return;
			}
			bool flag = cooldown < 0f;
			float globalCooldown = flag ? 100000f : cooldown;
			EmoteReactable emoteReactable = smi.AddSelfEmoteReactable(base.gameObject, this.effect.Name + "_Emote", emote, flag, Db.Get().ChoreTypes.Emote, globalCooldown, 20f, float.NegativeInfinity, this.effect.maxInitialDelay, this.effect.emotePreconditions);
			if (emoteReactable == null)
			{
				return;
			}
			emoteReactable.InsertPrecondition(0, new Reactable.ReactablePrecondition(this.NotInATube));
			if (!flag)
			{
				this.reactable = emoteReactable;
			}
		}

		// Token: 0x06006DA6 RID: 28070 RVA: 0x002B3F40 File Offset: 0x002B2140
		private bool NotInATube(GameObject go, Navigator.ActiveTransition transition)
		{
			return transition.navGridTransition.start != NavType.Tube && transition.navGridTransition.end != NavType.Tube;
		}

		// Token: 0x06006DA7 RID: 28071 RVA: 0x002B3F64 File Offset: 0x002B2164
		public override void OnCleanUp()
		{
			if (this.statusItem != null)
			{
				base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(this.statusItem, false);
				this.statusItem = null;
			}
			if (this.reactable != null)
			{
				this.reactable.Cleanup();
				this.reactable = null;
			}
		}

		// Token: 0x06006DA8 RID: 28072 RVA: 0x002B3FB2 File Offset: 0x002B21B2
		public float GetTimeRemaining()
		{
			return this.timeRemaining;
		}

		// Token: 0x06006DA9 RID: 28073 RVA: 0x002B3FBA File Offset: 0x002B21BA
		public bool IsExpired()
		{
			return this.effect.duration > 0f && this.timeRemaining <= 0f;
		}

		// Token: 0x06006DAA RID: 28074 RVA: 0x002B3FE0 File Offset: 0x002B21E0
		private void ConfigureStatusItem()
		{
			StatusItem.IconType icon_type = this.effect.isBad ? StatusItem.IconType.Exclamation : StatusItem.IconType.Info;
			if (!this.effect.customIcon.IsNullOrWhiteSpace())
			{
				icon_type = StatusItem.IconType.Custom;
			}
			this.statusItem = new StatusItem(this.effect.Id, this.effect.Name, this.effect.description, this.effect.customIcon, icon_type, this.effect.isBad ? NotificationType.Bad : NotificationType.Neutral, false, OverlayModes.None.ID, 2, false, null);
			this.statusItem.resolveStringCallback = new Func<string, object, string>(this.ResolveString);
			this.statusItem.resolveTooltipCallback = new Func<string, object, string>(this.ResolveTooltip);
		}

		// Token: 0x06006DAB RID: 28075 RVA: 0x002B4093 File Offset: 0x002B2293
		private string ResolveString(string str, object data)
		{
			return str;
		}

		// Token: 0x06006DAC RID: 28076 RVA: 0x002B4098 File Offset: 0x002B2298
		private string ResolveTooltip(string str, object data)
		{
			string text = str;
			EffectInstance effectInstance = (EffectInstance)data;
			string text2 = Effect.CreateTooltip(effectInstance.effect, false, "\n    • ", true);
			if (!string.IsNullOrEmpty(text2))
			{
				text = text + "\n\n" + text2;
			}
			if (effectInstance.effect.duration > 0f)
			{
				text = text + "\n\n" + string.Format(DUPLICANTS.MODIFIERS.TIME_REMAINING, GameUtil.GetFormattedCycles(this.GetTimeRemaining(), "F1", false));
			}
			return text;
		}

		// Token: 0x04005236 RID: 21046
		public Effect effect;

		// Token: 0x04005237 RID: 21047
		public bool shouldSave;

		// Token: 0x04005238 RID: 21048
		public StatusItem statusItem;

		// Token: 0x04005239 RID: 21049
		public float timeRemaining;

		// Token: 0x0400523A RID: 21050
		public EmoteReactable reactable;
	}
}
