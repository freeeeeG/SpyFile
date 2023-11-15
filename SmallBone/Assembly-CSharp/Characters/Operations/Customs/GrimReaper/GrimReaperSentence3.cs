using System;
using System.Collections;
using System.Collections.Generic;
using FX;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters.Operations.Customs.GrimReaper
{
	// Token: 0x02001007 RID: 4103
	public sealed class GrimReaperSentence3 : TargetedCharacterOperation
	{
		// Token: 0x06004F43 RID: 20291 RVA: 0x000EE5DB File Offset: 0x000EC7DB
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<AttackDamage>();
			this._targetDatas = new Dictionary<Character, GrimReaperSentence3.TargetData>(32);
		}

		// Token: 0x06004F44 RID: 20292 RVA: 0x000EE5F8 File Offset: 0x000EC7F8
		public override void Run(Character owner, Character target)
		{
			GrimReaperSentence3.<>c__DisplayClass18_0 CS$<>8__locals1 = new GrimReaperSentence3.<>c__DisplayClass18_0();
			CS$<>8__locals1.target = target;
			CS$<>8__locals1.<>4__this = this;
			if (this._targetDatas.ContainsKey(CS$<>8__locals1.target))
			{
				return;
			}
			if (CS$<>8__locals1.target == null || !CS$<>8__locals1.target.liveAndActive)
			{
				return;
			}
			float num = this._rotationSyncWithMark ? this._angle.value : 0f;
			EffectPoolInstance markEffect = (this._markEffectInfo == null) ? null : this._markEffectInfo.Spawn(CS$<>8__locals1.target.transform.position, CS$<>8__locals1.target, num, 1f);
			GrimReaperSentence3.TargetData value = new GrimReaperSentence3.TargetData(markEffect, num);
			this._targetDatas.Add(CS$<>8__locals1.target, value);
			CS$<>8__locals1.target.health.onDiedTryCatch += CS$<>8__locals1.<Run>g__SummonClosure|0;
			this._owner = owner;
			if (this._duration > 0f)
			{
				this._owner.StartCoroutine(this.CWaitForDurationAndSummon(new Action(CS$<>8__locals1.<Run>g__SummonClosure|0), CS$<>8__locals1.target));
			}
		}

		// Token: 0x06004F45 RID: 20293 RVA: 0x000EE706 File Offset: 0x000EC906
		private IEnumerator CWaitForDurationAndSummon(Action summonClosure, Character target)
		{
			yield return target.chronometer.master.WaitForSeconds(this._duration);
			if (!target.health.dead)
			{
				if (summonClosure != null)
				{
					summonClosure();
				}
			}
			yield break;
		}

		// Token: 0x06004F46 RID: 20294 RVA: 0x000EE724 File Offset: 0x000EC924
		private void Summon(Character target)
		{
			if (target == null)
			{
				return;
			}
			if (!this._targetDatas.ContainsKey(target))
			{
				return;
			}
			GrimReaperSentence3.TargetData targetData = this._targetDatas[target];
			this._targetDatas.Remove(target);
			Vector3 euler = this._rotationSyncWithMark ? new Vector3(0f, 0f, targetData.angle) : new Vector3(0f, 0f, this._angle.value);
			if (targetData.markEffect != null)
			{
				targetData.markEffect.Stop();
				targetData.markEffect = null;
			}
			Vector3 position = target.transform.position;
			position.x += target.collider.offset.x;
			position.y += target.collider.offset.y;
			Vector3 size = target.collider.bounds.size;
			size.x *= this._positionInfo.pivotValue.x;
			size.y *= this._positionInfo.pivotValue.y;
			Vector3 position2 = position + size + this._noise.Evaluate();
			OperationRunner operationRunner = this._operationRunner.Spawn();
			OperationInfos operationInfos = operationRunner.operationInfos;
			operationInfos.transform.SetPositionAndRotation(position2, Quaternion.Euler(euler));
			if (this._copyAttackDamage && this._attackDamage != null)
			{
				operationRunner.attackDamage.minAttackDamage = this._attackDamage.minAttackDamage;
				operationRunner.attackDamage.maxAttackDamage = this._attackDamage.maxAttackDamage;
			}
			SortingGroup component = operationRunner.GetComponent<SortingGroup>();
			if (component != null)
			{
				SortingGroup sortingGroup = component;
				short num = GrimReaperSentence3.spriteLayer;
				GrimReaperSentence3.spriteLayer = num + 1;
				sortingGroup.sortingOrder = (int)num;
			}
			operationInfos.transform.localScale = Vector3.one * this._scale.value;
			operationInfos.Run(this._owner);
		}

		// Token: 0x04003F65 RID: 16229
		private static short spriteLayer = short.MinValue;

		// Token: 0x04003F66 RID: 16230
		[SerializeField]
		[Header("마크")]
		private EffectInfo _markEffectInfo;

		// Token: 0x04003F67 RID: 16231
		[SerializeField]
		[Tooltip("오퍼레이션 프리팹")]
		private OperationRunner _operationRunner;

		// Token: 0x04003F68 RID: 16232
		[SerializeField]
		[Space]
		private GrimReaperSentence3.PositionInfo _positionInfo;

		// Token: 0x04003F69 RID: 16233
		[SerializeField]
		private CustomFloat _scale = new CustomFloat(1f);

		// Token: 0x04003F6A RID: 16234
		[SerializeField]
		private CustomAngle _angle;

		// Token: 0x04003F6B RID: 16235
		[SerializeField]
		private PositionNoise _noise;

		// Token: 0x04003F6C RID: 16236
		[SerializeField]
		[Space]
		private bool _snapToGround;

		// Token: 0x04003F6D RID: 16237
		[Tooltip("땅을 찾기 위해 소환지점으로부터 아래로 탐색할 거리. 실패 시 그냥 소환 지점에 소환됨")]
		[SerializeField]
		private float _groundFindingDistance = 7f;

		// Token: 0x04003F6E RID: 16238
		[SerializeField]
		[Space]
		private bool _copyAttackDamage;

		// Token: 0x04003F6F RID: 16239
		[SerializeField]
		private float _duration;

		// Token: 0x04003F70 RID: 16240
		[SerializeField]
		private bool _rotationSyncWithMark;

		// Token: 0x04003F71 RID: 16241
		private AttackDamage _attackDamage;

		// Token: 0x04003F72 RID: 16242
		private Character _owner;

		// Token: 0x04003F73 RID: 16243
		private Dictionary<Character, GrimReaperSentence3.TargetData> _targetDatas;

		// Token: 0x02001008 RID: 4104
		[Serializable]
		public class PositionInfo
		{
			// Token: 0x17000FA1 RID: 4001
			// (get) Token: 0x06004F49 RID: 20297 RVA: 0x000EE952 File Offset: 0x000ECB52
			public GrimReaperSentence3.PositionInfo.Pivot pivot
			{
				get
				{
					return this._pivot;
				}
			}

			// Token: 0x17000FA2 RID: 4002
			// (get) Token: 0x06004F4A RID: 20298 RVA: 0x000EE95A File Offset: 0x000ECB5A
			public Vector2 pivotValue
			{
				get
				{
					return this._pivotValue;
				}
			}

			// Token: 0x06004F4B RID: 20299 RVA: 0x000EE962 File Offset: 0x000ECB62
			public PositionInfo()
			{
				this._pivot = GrimReaperSentence3.PositionInfo.Pivot.Center;
				this._pivotValue = Vector2.zero;
			}

			// Token: 0x06004F4C RID: 20300 RVA: 0x000EE97C File Offset: 0x000ECB7C
			public PositionInfo(bool attach, bool layerOnly, int layerOrderOffset, GrimReaperSentence3.PositionInfo.Pivot pivot)
			{
				this._pivot = pivot;
				this._pivotValue = GrimReaperSentence3.PositionInfo._pivotValues[pivot];
			}

			// Token: 0x04003F74 RID: 16244
			private static readonly EnumArray<GrimReaperSentence3.PositionInfo.Pivot, Vector2> _pivotValues = new EnumArray<GrimReaperSentence3.PositionInfo.Pivot, Vector2>(new Vector2[]
			{
				new Vector2(0f, 0f),
				new Vector2(-0.5f, 0.5f),
				new Vector2(0f, 0.5f),
				new Vector2(0.5f, 0.5f),
				new Vector2(-0.5f, 0f),
				new Vector2(0f, 0.5f),
				new Vector2(-0.5f, -0.5f),
				new Vector2(0f, -0.5f),
				new Vector2(0.5f, -0.5f),
				new Vector2(0f, 0f)
			});

			// Token: 0x04003F75 RID: 16245
			[SerializeField]
			private GrimReaperSentence3.PositionInfo.Pivot _pivot;

			// Token: 0x04003F76 RID: 16246
			[SerializeField]
			[HideInInspector]
			private Vector2 _pivotValue;

			// Token: 0x02001009 RID: 4105
			public enum Pivot
			{
				// Token: 0x04003F78 RID: 16248
				Center,
				// Token: 0x04003F79 RID: 16249
				TopLeft,
				// Token: 0x04003F7A RID: 16250
				Top,
				// Token: 0x04003F7B RID: 16251
				TopRight,
				// Token: 0x04003F7C RID: 16252
				Left,
				// Token: 0x04003F7D RID: 16253
				Right,
				// Token: 0x04003F7E RID: 16254
				BottomLeft,
				// Token: 0x04003F7F RID: 16255
				Bottom,
				// Token: 0x04003F80 RID: 16256
				BottomRight,
				// Token: 0x04003F81 RID: 16257
				Custom
			}
		}

		// Token: 0x0200100A RID: 4106
		private struct TargetData
		{
			// Token: 0x06004F4E RID: 20302 RVA: 0x000EEA9B File Offset: 0x000ECC9B
			internal TargetData(EffectPoolInstance markEffect, float angle)
			{
				this.markEffect = markEffect;
				this.angle = angle;
			}

			// Token: 0x04003F82 RID: 16258
			internal EffectPoolInstance markEffect;

			// Token: 0x04003F83 RID: 16259
			internal float angle;
		}
	}
}
