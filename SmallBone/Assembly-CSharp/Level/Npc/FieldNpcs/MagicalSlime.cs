using System;
using System.Collections;
using Characters;
using Characters.Gear.Items;
using Data;
using FX;
using GameResources;
using Services;
using Singletons;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Level.Npc.FieldNpcs
{
	// Token: 0x020005EE RID: 1518
	public sealed class MagicalSlime : FieldNpc
	{
		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x06001E5C RID: 7772 RVA: 0x0005C5D7 File Offset: 0x0005A7D7
		protected override NpcType _type
		{
			get
			{
				return NpcType.MagicalSlime;
			}
		}

		// Token: 0x06001E5D RID: 7773 RVA: 0x0005C5DA File Offset: 0x0005A7DA
		protected override void OnDestroy()
		{
			base.OnDestroy();
			this._polymorphAnimationClip = null;
			this._specialItem = null;
		}

		// Token: 0x06001E5E RID: 7774 RVA: 0x0005C5F0 File Offset: 0x0005A7F0
		protected override void OnStopConversation()
		{
			base.OnStopConversation();
			this._animator.Play(this._idle);
			if (this._spawnedPolymorphStartSound != null)
			{
				this._spawnedPolymorphStartSound.Stop();
			}
		}

		// Token: 0x06001E5F RID: 7775 RVA: 0x0005C622 File Offset: 0x0005A822
		protected override void Interact(Character character)
		{
			base.Interact(character);
			if (this._phase != FieldNpc.Phase.Gave)
			{
				base.StartCoroutine(this.CGreetingAndPolymorph());
			}
		}

		// Token: 0x06001E60 RID: 7776 RVA: 0x0005C641 File Offset: 0x0005A841
		private IEnumerator CGreetingAndPolymorph()
		{
			yield return LetterBox.instance.CAppear(0.4f);
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			System.Random random = new System.Random((int)(GameData.Save.instance.randomSeed + -699075432 + currentChapter.type * (Chapter.Type)256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex));
			Item item = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.item.GetRandomItem(random);
			if (item == null)
			{
				yield return this.CNoItem();
			}
			else
			{
				yield return base.CGreeting();
				yield return this.CPolymorphToRandomItem(item);
			}
			base.Close();
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x06001E61 RID: 7777 RVA: 0x0005C650 File Offset: 0x0005A850
		private IEnumerator CNoItem()
		{
			this._npcConversation.skippable = true;
			yield return this._npcConversation.CConversation(base._noMoney);
			this._animator.Play(this._polymorphCastingHash);
			this._spawnedPolymorphStartSound = PersistentSingleton<SoundManager>.Instance.PlaySound(this._polymorphStartSound, base.transform.position);
			yield return Chronometer.global.WaitForSeconds(this._polymorphAnimationClip.length);
			UnityEvent onPolymorphEnd = this._onPolymorphEnd;
			if (onPolymorphEnd != null)
			{
				onPolymorphEnd.Invoke();
			}
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._polymorphEndSound, base.transform.position);
			Singleton<Service>.Instance.levelManager.DropItem(this._specialItem, (this._itemDropPoint == null) ? base.transform.position : this._itemDropPoint.position);
			this._phase = FieldNpc.Phase.Gave;
			yield break;
		}

		// Token: 0x06001E62 RID: 7778 RVA: 0x0005C65F File Offset: 0x0005A85F
		private IEnumerator CPolymorphToRandomItem(Item targetItem)
		{
			ItemReference itemByKey = Singleton<Service>.Instance.gearManager.GetItemByKey(targetItem.name);
			ItemRequest request = itemByKey.LoadAsync();
			while (!request.isDone)
			{
				yield return null;
			}
			this._animator.Play(this._polymorphCastingHash);
			this._spawnedPolymorphStartSound = PersistentSingleton<SoundManager>.Instance.PlaySound(this._polymorphStartSound, base.transform.position);
			yield return Chronometer.global.WaitForSeconds(this._polymorphAnimationClip.length);
			UnityEvent onPolymorphEnd = this._onPolymorphEnd;
			if (onPolymorphEnd != null)
			{
				onPolymorphEnd.Invoke();
			}
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._polymorphEndSound, base.transform.position);
			Item item = Singleton<Service>.Instance.levelManager.DropItem(request, (this._itemDropPoint == null) ? base.transform.position : this._itemDropPoint.position);
			item.keyword1 = targetItem.keyword1;
			item.keyword2 = targetItem.keyword2;
			IStackable componentInChildren = targetItem.GetComponentInChildren<IStackable>();
			if (componentInChildren != null)
			{
				item.GetComponentInChildren<IStackable>().stack = componentInChildren.stack;
			}
			this._phase = FieldNpc.Phase.Gave;
			yield break;
		}

		// Token: 0x040019A0 RID: 6560
		private const int _randomSeed = -699075432;

		// Token: 0x040019A1 RID: 6561
		[SerializeField]
		private Transform _itemDropPoint;

		// Token: 0x040019A2 RID: 6562
		[SerializeField]
		private AnimationClip _polymorphAnimationClip;

		// Token: 0x040019A3 RID: 6563
		[SerializeField]
		private SoundInfo _polymorphStartSound;

		// Token: 0x040019A4 RID: 6564
		[SerializeField]
		private SoundInfo _polymorphEndSound;

		// Token: 0x040019A5 RID: 6565
		[SerializeField]
		private UnityEvent _onPolymorphEnd;

		// Token: 0x040019A6 RID: 6566
		[SerializeField]
		[Header("아이템이 없을 때")]
		private Item _specialItem;

		// Token: 0x040019A7 RID: 6567
		private ReusableAudioSource _spawnedPolymorphStartSound;

		// Token: 0x040019A8 RID: 6568
		private readonly int _polymorphCastingHash = Animator.StringToHash("Polymorph_Casting");

		// Token: 0x040019A9 RID: 6569
		private readonly int _idle = Animator.StringToHash("Idle");
	}
}
