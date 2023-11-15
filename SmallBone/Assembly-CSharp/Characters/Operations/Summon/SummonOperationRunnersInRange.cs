using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FX;
using PhysicsUtils;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters.Operations.Summon
{
	// Token: 0x02000F4E RID: 3918
	public sealed class SummonOperationRunnersInRange : CharacterOperation
	{
		// Token: 0x06004C1F RID: 19487 RVA: 0x000E0DAC File Offset: 0x000DEFAC
		private void Awake()
		{
			this._overlapper = new NonAllocOverlapper(16);
			int num = 262144;
			if (this._includePlatform)
			{
				num |= 131072;
			}
			this._overlapper.contactFilter.SetLayerMask(num);
			this._terrainFindingRange.enabled = false;
		}

		// Token: 0x06004C20 RID: 19488 RVA: 0x000E0DFE File Offset: 0x000DEFFE
		protected override void OnDestroy()
		{
			base.OnDestroy();
			this._summonOption.Dispose();
		}

		// Token: 0x06004C21 RID: 19489 RVA: 0x000E0E11 File Offset: 0x000DF011
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<AttackDamage>();
		}

		// Token: 0x06004C22 RID: 19490 RVA: 0x000E0E1F File Offset: 0x000DF01F
		public override void Run(Character owner)
		{
			this.FindSurfaces();
			if (this._surfaces.Count > 0)
			{
				base.StartCoroutine(this.CSummonAll(owner));
			}
		}

		// Token: 0x06004C23 RID: 19491 RVA: 0x000E0E44 File Offset: 0x000DF044
		private void FindSurfaces()
		{
			this._terrainFindingRange.enabled = true;
			this._overlapper.OverlapCollider(this._terrainFindingRange);
			float x = this._terrainFindingRange.bounds.min.x;
			float x2 = this._terrainFindingRange.bounds.max.x;
			this._terrainFindingRange.enabled = false;
			this._surfaces.Clear();
			if (this._overlapper.results.Count == 0)
			{
				return;
			}
			for (int i = 0; i < this._overlapper.results.Count; i++)
			{
				Bounds bounds = this._overlapper.results[i].bounds;
				float2 @float = bounds.GetMostLeftTop();
				float2 float2 = bounds.GetMostRightTop();
				@float.x = Mathf.Max(@float.x, x);
				float2.x = Mathf.Min(float2.x, x2);
				this._surfaces.Add(new ValueTuple<float2, float2>(@float, float2));
			}
		}

		// Token: 0x06004C24 RID: 19492 RVA: 0x000E0F54 File Offset: 0x000DF154
		private IEnumerator CSummonAll(Character owner)
		{
			int remain = this._count;
			this._surfaces.PseudoShuffle<ValueTuple<float2, float2>>();
			float num = 0f;
			foreach (ValueTuple<float2, float2> valueTuple in this._surfaces)
			{
				num += Mathf.Abs(valueTuple.Item2.x - valueTuple.Item1.x);
			}
			for (int k = 0; k < this._surfaces.Count; k++)
			{
				float num2 = Mathf.Abs(this._surfaces[k].Item2.x - this._surfaces[k].Item1.x);
				this._weights[k] = Mathf.RoundToInt(num2 / num * (float)this._count);
			}
			int i = 0;
			int num3;
			while (i < this._surfaces.Count - 1 && remain > 0)
			{
				ValueTuple<float2, float2> surface = this._surfaces[i];
				for (int j = 0; j < this._weights[i]; j = num3 + 1)
				{
					this.Summon(owner, surface);
					yield return Chronometer.global.WaitForSeconds(this._delay);
					num3 = j;
				}
				remain -= this._weights[i];
				surface = default(ValueTuple<float2, float2>);
				num3 = i;
				i = num3 + 1;
			}
			ValueTuple<float2, float2> lastSurface = this._surfaces[this._surfaces.Count - 1];
			for (i = 0; i < remain; i = num3 + 1)
			{
				this.Summon(owner, lastSurface);
				yield return Chronometer.global.WaitForSeconds(this._delay);
				num3 = i;
			}
			yield break;
		}

		// Token: 0x06004C25 RID: 19493 RVA: 0x000E0F6C File Offset: 0x000DF16C
		private void Summon(Character owner, [TupleElementNames(new string[]
		{
			"left",
			"right"
		})] ValueTuple<float2, float2> spawnRange)
		{
			float x = UnityEngine.Random.Range(spawnRange.Item1.x, spawnRange.Item2.x);
			float y = (spawnRange.Item1.y + spawnRange.Item2.y) / 2f;
			this.Summon(owner, new Vector2(x, y));
		}

		// Token: 0x06004C26 RID: 19494 RVA: 0x000E0FC8 File Offset: 0x000DF1C8
		private void Summon(Character owner, float2 position)
		{
			Vector3 vector = new Vector3(0f, 0f, this._summonOption.angle.value);
			bool flag = this._summonOption.flipXByLookingDirection && owner.lookingDirection == Character.LookingDirection.Left;
			if (flag)
			{
				vector.z = (180f - vector.z) % 360f;
			}
			if (this._summonOption.flipX)
			{
				vector.z = (180f - vector.z) % 360f;
			}
			OperationRunner operationRunner = this._summonOption.operationRunner.Spawn();
			OperationInfos operationInfos = operationRunner.operationInfos;
			operationInfos.transform.SetPositionAndRotation(new Vector3(position.x, position.y), Quaternion.Euler(vector));
			if (this._summonOption.copyAttackDamage && this._attackDamage != null)
			{
				operationRunner.attackDamage.minAttackDamage = this._attackDamage.minAttackDamage;
				operationRunner.attackDamage.maxAttackDamage = this._attackDamage.maxAttackDamage;
			}
			SortingGroup component = operationRunner.GetComponent<SortingGroup>();
			if (component != null)
			{
				SortingGroup sortingGroup = component;
				short num = SummonOperationRunnersInRange.spriteLayer;
				SummonOperationRunnersInRange.spriteLayer = num + 1;
				sortingGroup.sortingOrder = (int)num;
			}
			if (flag)
			{
				operationInfos.transform.localScale = new Vector3(1f, -1f, 1f) * this._summonOption.scale.value;
			}
			else
			{
				operationInfos.transform.localScale = new Vector3(1f, 1f, 1f) * this._summonOption.scale.value;
			}
			if (this._summonOption.flipX)
			{
				operationInfos.transform.localScale = new Vector3(1f, -1f, 1f) * this._summonOption.scale.value;
			}
			operationInfos.Run(owner);
		}

		// Token: 0x06004C27 RID: 19495 RVA: 0x00048973 File Offset: 0x00046B73
		public override void Stop()
		{
			base.StopAllCoroutines();
		}

		// Token: 0x04003B9B RID: 15259
		[SerializeField]
		private BoxCollider2D _terrainFindingRange;

		// Token: 0x04003B9C RID: 15260
		[SerializeField]
		[Tooltip("플랫폼도 포함할 것인지")]
		private bool _includePlatform = true;

		// Token: 0x04003B9D RID: 15261
		[SerializeField]
		private int _count = 1;

		// Token: 0x04003B9E RID: 15262
		[SerializeField]
		private float _delay;

		// Token: 0x04003B9F RID: 15263
		[SerializeField]
		private SummonOperationRunnersInRange.SummonOption _summonOption;

		// Token: 0x04003BA0 RID: 15264
		private const int _maxTerrainCount = 16;

		// Token: 0x04003BA1 RID: 15265
		private static short spriteLayer = short.MinValue;

		// Token: 0x04003BA2 RID: 15266
		private NonAllocOverlapper _overlapper;

		// Token: 0x04003BA3 RID: 15267
		[TupleElementNames(new string[]
		{
			"a",
			"b"
		})]
		private List<ValueTuple<float2, float2>> _surfaces = new List<ValueTuple<float2, float2>>(16);

		// Token: 0x04003BA4 RID: 15268
		private AttackDamage _attackDamage;

		// Token: 0x04003BA5 RID: 15269
		private int[] _weights = new int[16];

		// Token: 0x02000F4F RID: 3919
		[Serializable]
		private class SummonOption
		{
			// Token: 0x06004C2A RID: 19498 RVA: 0x000E11E6 File Offset: 0x000DF3E6
			internal void Dispose()
			{
				this.operationRunner = null;
			}

			// Token: 0x04003BA6 RID: 15270
			[SerializeField]
			[Tooltip("오퍼레이션 프리팹")]
			[Space]
			internal OperationRunner operationRunner;

			// Token: 0x04003BA7 RID: 15271
			[SerializeField]
			[Space]
			internal CustomFloat scale = new CustomFloat(1f);

			// Token: 0x04003BA8 RID: 15272
			[SerializeField]
			internal CustomAngle angle;

			// Token: 0x04003BA9 RID: 15273
			[SerializeField]
			internal PositionNoise noise;

			// Token: 0x04003BAA RID: 15274
			[Space]
			[Tooltip("주인이 바라보고 있는 방향에 따라 X축으로 플립")]
			[SerializeField]
			internal bool flipXByLookingDirection;

			// Token: 0x04003BAB RID: 15275
			[Tooltip("X축 플립")]
			[SerializeField]
			internal bool flipX;

			// Token: 0x04003BAC RID: 15276
			[SerializeField]
			internal bool copyAttackDamage;
		}
	}
}
