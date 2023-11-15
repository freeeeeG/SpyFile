using System;
using System.Collections;
using Characters;
using FX;
using UI;
using UnityEngine;

namespace Level.Npc.FieldNpcs
{
	// Token: 0x020005D2 RID: 1490
	public sealed class FieldDeathKnight : FieldNpc
	{
		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x06001DAF RID: 7599 RVA: 0x0005A999 File Offset: 0x00058B99
		protected override NpcType _type
		{
			get
			{
				return NpcType.FieldDeathKnight;
			}
		}

		// Token: 0x06001DB0 RID: 7600 RVA: 0x0005A9A0 File Offset: 0x00058BA0
		protected override void Interact(Character character)
		{
			base.Interact(character);
			FieldNpc.Phase phase = this._phase;
			if (phase <= FieldNpc.Phase.Greeted)
			{
				base.StartCoroutine(this.CGreetingAndConfirm(character, null));
				return;
			}
			if (phase != FieldNpc.Phase.Gave)
			{
				return;
			}
			base.StartCoroutine(base.CChat());
		}

		// Token: 0x06001DB1 RID: 7601 RVA: 0x0005A9E1 File Offset: 0x00058BE1
		private IEnumerator CGreetingAndConfirm(Character character, object confirmArg = null)
		{
			yield return LetterBox.instance.CAppear(0.4f);
			this._npcConversation.skippable = true;
			int lastIndex = 3;
			int num;
			for (int i = 0; i < lastIndex; i = num + 1)
			{
				yield return this._npcConversation.CConversation(new string[]
				{
					base._greeting[i]
				});
				num = i;
			}
			UnityEngine.Object.Instantiate<DeathKnightReward>(this._reward, this._dropPosition.transform.position, Quaternion.identity, Map.Instance.transform);
			this._phase = FieldNpc.Phase.Gave;
			for (int i = lastIndex; i < base._greeting.Length; i = num + 1)
			{
				yield return this._npcConversation.CConversation(new string[]
				{
					base._greeting[i]
				});
				num = i;
			}
			LetterBox.instance.Disappear(0.4f);
			yield break;
		}

		// Token: 0x04001927 RID: 6439
		[SerializeField]
		private DeathKnightReward _reward;

		// Token: 0x04001928 RID: 6440
		[SerializeField]
		private Transform _dropPosition;

		// Token: 0x04001929 RID: 6441
		[SerializeField]
		private EffectInfo _dropEffect;

		// Token: 0x0400192A RID: 6442
		[SerializeField]
		private SoundInfo _dropSound;
	}
}
