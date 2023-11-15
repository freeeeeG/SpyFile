using System;
using System.Collections;
using Characters;
using FX;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace Level.Npc.FieldNpcs
{
	// Token: 0x020005EC RID: 1516
	public class HarpyWarrior : FieldNpc
	{
		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x06001E51 RID: 7761 RVA: 0x0005C44E File Offset: 0x0005A64E
		protected override NpcType _type
		{
			get
			{
				return NpcType.HarpyWarrior;
			}
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x06001E52 RID: 7762 RVA: 0x0005C451 File Offset: 0x0005A651
		private int _bones
		{
			get
			{
				return Singleton<Service>.Instance.levelManager.currentChapter.currentStage.fieldNpcSettings.harpyWarriorBones;
			}
		}

		// Token: 0x06001E53 RID: 7763 RVA: 0x0005C474 File Offset: 0x0005A674
		protected override void Interact(Character character)
		{
			base.Interact(character);
			FieldNpc.Phase phase = this._phase;
			if (phase <= FieldNpc.Phase.Greeted)
			{
				base.StartCoroutine(this.CGiveBones(character));
				return;
			}
			if (phase != FieldNpc.Phase.Gave)
			{
				return;
			}
			base.StartCoroutine(base.CChat());
		}

		// Token: 0x06001E54 RID: 7764 RVA: 0x0005C4B4 File Offset: 0x0005A6B4
		private IEnumerator CGiveBones(Character character)
		{
			yield return LetterBox.instance.CAppear(0.4f);
			yield return base.CGreeting();
			Singleton<Service>.Instance.levelManager.DropBone(this._bones, 10);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._sound, base.transform.position);
			this._phase = FieldNpc.Phase.Gave;
			this._npcConversation.skippable = true;
			yield return this._npcConversation.visible = false;
			LetterBox.instance.Disappear(0.4f);
			yield break;
		}

		// Token: 0x0400199C RID: 6556
		[SerializeField]
		private SoundInfo _sound;
	}
}
