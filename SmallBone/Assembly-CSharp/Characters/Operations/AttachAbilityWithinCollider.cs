using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Abilities;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DAF RID: 3503
	public class AttachAbilityWithinCollider : CharacterOperation
	{
		// Token: 0x06004687 RID: 18055 RVA: 0x000CBDDC File Offset: 0x000C9FDC
		private void Awake()
		{
			if (this._optimizedCollider)
			{
				this._collider.enabled = false;
			}
			this._charactersWithinCollider = new DoubleBuffered<List<Character>>(new List<Character>(AttachAbilityWithinCollider._sharedOverlapper.capacity), new List<Character>(AttachAbilityWithinCollider._sharedOverlapper.capacity));
			this._abilityComponents.Initialize();
			if (this._duration <= 0f)
			{
				this._duration = float.PositiveInfinity;
			}
		}

		// Token: 0x06004688 RID: 18056 RVA: 0x000CBE49 File Offset: 0x000CA049
		public override void Run(Character owner)
		{
			this._charactersWithinCollider.Current.Clear();
			base.StartCoroutine(this.CRun(owner));
		}

		// Token: 0x06004689 RID: 18057 RVA: 0x000CBE6C File Offset: 0x000CA06C
		public override void Stop()
		{
			this._cCheckReference.Stop();
			foreach (Character character in this._charactersWithinCollider.Current)
			{
				foreach (AbilityComponent abilityComponent in this._abilityComponents.components)
				{
					character.ability.Remove(abilityComponent.ability);
				}
			}
			this._charactersWithinCollider.Current.Clear();
		}

		// Token: 0x0600468A RID: 18058 RVA: 0x000CBF0C File Offset: 0x000CA10C
		private IEnumerator CRun(Character owner)
		{
			this._cCheckReference.Stop();
			this._cCheckReference = this.StartCoroutineWithReference(this.CCheck(owner));
			yield return new WaitForSeconds(this._duration);
			this._cCheckReference.Stop();
			yield break;
		}

		// Token: 0x0600468B RID: 18059 RVA: 0x000CBF22 File Offset: 0x000CA122
		private IEnumerator CCheck(Character owner)
		{
			for (;;)
			{
				this._collider.enabled = true;
				AttachAbilityWithinCollider._sharedOverlapper.contactFilter.SetLayerMask(this._layer.Evaluate(owner.gameObject));
				AttachAbilityWithinCollider._sharedOverlapper.OverlapCollider(this._collider);
				if (this._optimizedCollider)
				{
					this._collider.enabled = false;
				}
				int i = 0;
				while (i < AttachAbilityWithinCollider._sharedOverlapper.results.Count)
				{
					Target component = AttachAbilityWithinCollider._sharedOverlapper.results[i].GetComponent<Target>();
					Character character;
					if (component == null)
					{
						Minion component2 = AttachAbilityWithinCollider._sharedOverlapper.results[i].GetComponent<Minion>();
						if (!(component2 == null) && this._characterTypeFilter[Character.Type.PlayerMinion])
						{
							character = component2.character;
							goto IL_111;
						}
					}
					else if (!(component.character == null) && this._characterTypeFilter[component.character.type])
					{
						character = component.character;
						goto IL_111;
					}
					IL_1A3:
					i++;
					continue;
					IL_111:
					if (this._excludeSelf && character == owner)
					{
						goto IL_1A3;
					}
					this._charactersWithinCollider.Next.Add(character);
					int num = this._charactersWithinCollider.Current.IndexOf(character);
					if (num >= 0)
					{
						this._charactersWithinCollider.Current.RemoveAt(num);
						goto IL_1A3;
					}
					for (int j = 0; j < this._abilityComponents.components.Length; j++)
					{
						character.ability.Add(this._abilityComponents.components[j].ability);
					}
					goto IL_1A3;
				}
				for (int k = 0; k < this._charactersWithinCollider.Current.Count; k++)
				{
					Character character2 = this._charactersWithinCollider.Current[k];
					for (int l = 0; l < this._abilityComponents.components.Length; l++)
					{
						character2.ability.Remove(this._abilityComponents.components[l].ability);
					}
				}
				this._charactersWithinCollider.Current.Clear();
				this._charactersWithinCollider.Swap();
				yield return new WaitForSecondsRealtime(this._checkInterval);
			}
			yield break;
		}

		// Token: 0x04003566 RID: 13670
		private static readonly NonAllocOverlapper _sharedOverlapper = new NonAllocOverlapper(99);

		// Token: 0x04003567 RID: 13671
		[SerializeField]
		private float _duration;

		// Token: 0x04003568 RID: 13672
		[SerializeField]
		[Range(0.1f, 1f)]
		[Tooltip("이 주기(초)마다 콜라이더 내에 있는 캐릭터들을 검사합니다. 낮을수록 정밀도가 올라가지만 연산량이 많아집니다.")]
		private float _checkInterval = 0.33f;

		// Token: 0x04003569 RID: 13673
		[Header("Filter")]
		[SerializeField]
		private TargetLayer _layer = new TargetLayer(0, false, true, false, false);

		// Token: 0x0400356A RID: 13674
		[SerializeField]
		private CharacterTypeBoolArray _characterTypeFilter = new CharacterTypeBoolArray(new bool[]
		{
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true
		});

		// Token: 0x0400356B RID: 13675
		[SerializeField]
		private bool _excludeSelf;

		// Token: 0x0400356C RID: 13676
		[Header("Collider")]
		[SerializeField]
		private Collider2D _collider;

		// Token: 0x0400356D RID: 13677
		[SerializeField]
		[Tooltip("콜라이더 최적화 여부, Composite Collider등 특별한 경우가 아니면 true로 유지")]
		private bool _optimizedCollider = true;

		// Token: 0x0400356E RID: 13678
		[SerializeField]
		[Header("Abilities")]
		[Space]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent.Subcomponents _abilityComponents;

		// Token: 0x0400356F RID: 13679
		private DoubleBuffered<List<Character>> _charactersWithinCollider;

		// Token: 0x04003570 RID: 13680
		private CoroutineReference _cCheckReference;
	}
}
