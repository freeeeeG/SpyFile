using System;
using System.Linq;
using Characters.Abilities.Constraints;
using Level;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D6C RID: 3436
	[Serializable]
	public class MummyGunDropPassive : Ability, IAbilityInstance
	{
		// Token: 0x17000E61 RID: 3681
		// (get) Token: 0x0600453B RID: 17723 RVA: 0x000C8F2F File Offset: 0x000C712F
		// (set) Token: 0x0600453C RID: 17724 RVA: 0x000C8F37 File Offset: 0x000C7137
		public Character owner { get; set; }

		// Token: 0x17000E62 RID: 3682
		// (get) Token: 0x0600453D RID: 17725 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000E63 RID: 3683
		// (get) Token: 0x0600453E RID: 17726 RVA: 0x000C8F40 File Offset: 0x000C7140
		// (set) Token: 0x0600453F RID: 17727 RVA: 0x000C8F48 File Offset: 0x000C7148
		public float remainTime { get; set; }

		// Token: 0x17000E64 RID: 3684
		// (get) Token: 0x06004540 RID: 17728 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000E65 RID: 3685
		// (get) Token: 0x06004541 RID: 17729 RVA: 0x0009ADBE File Offset: 0x00098FBE
		public Sprite icon
		{
			get
			{
				return this._defaultIcon;
			}
		}

		// Token: 0x17000E66 RID: 3686
		// (get) Token: 0x06004542 RID: 17730 RVA: 0x000C8F51 File Offset: 0x000C7151
		public float iconFillAmount
		{
			get
			{
				if (this._supplyInterval != 0f)
				{
					return 1f - this._remainSupplyTime / this._supplyInterval;
				}
				return 0f;
			}
		}

		// Token: 0x17000E67 RID: 3687
		// (get) Token: 0x06004543 RID: 17731 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconStacks
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000E68 RID: 3688
		// (get) Token: 0x06004544 RID: 17732 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool expired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004545 RID: 17733 RVA: 0x000C8F79 File Offset: 0x000C7179
		public override IAbilityInstance CreateInstance(Character owner)
		{
			this.owner = owner;
			return this;
		}

		// Token: 0x06004546 RID: 17734 RVA: 0x000C8F83 File Offset: 0x000C7183
		public void SupplyGunBySwap()
		{
			this.SupplyGun(this._gunsBySwapSupply);
		}

		// Token: 0x06004547 RID: 17735 RVA: 0x000C8F94 File Offset: 0x000C7194
		public void UpdateTime(float deltaTime)
		{
			if (this._supplyInterval == 0f)
			{
				return;
			}
			if (!this._constraints.Pass())
			{
				return;
			}
			this._remainSupplyTime -= deltaTime;
			if (this._remainSupplyTime > 0f)
			{
				return;
			}
			this._remainSupplyTime = this._supplyInterval;
			this.SupplyGun(this._gunsByPeriodicSupply);
		}

		// Token: 0x06004548 RID: 17736 RVA: 0x000C8FF1 File Offset: 0x000C71F1
		private bool IsInTerrain(Vector3 position)
		{
			position.y += 0.5f;
			return !Physics2D.OverlapPoint(position, Layers.terrainMask);
		}

		// Token: 0x06004549 RID: 17737 RVA: 0x000C9020 File Offset: 0x000C7220
		private void SupplyGun(MummyGunDropPassive.DroppedGuns guns)
		{
			Vector3 vector = this.owner.transform.position;
			Collider2D lastStandingCollider = this.owner.movement.controller.collisionState.lastStandingCollider;
			if (lastStandingCollider != null)
			{
				vector.y = lastStandingCollider.bounds.max.y;
			}
			Vector3 one = Vector3.one;
			for (int i = 0; i < 10; i++)
			{
				one.x = this._supplyWidth.value;
				RaycastHit2D hit = Physics2D.Raycast(vector + one, Vector2.down, 5f, Layers.groundMask);
				if (hit && this.IsInTerrain(hit.point))
				{
					vector = hit.point;
					break;
				}
			}
			float y = vector.y;
			vector.y += this._supplyHeight.value;
			DroppedMummyGun droppedMummyGun = this.DropGun(guns, Vector3.one);
			if (droppedMummyGun == null)
			{
				return;
			}
			this._supplyPrefab.Spawn(droppedMummyGun, vector, y);
		}

		// Token: 0x0600454A RID: 17738 RVA: 0x000C9140 File Offset: 0x000C7340
		private void OnOwnerKilled(ITarget target, ref Damage damage)
		{
			if (target.character == null || target.character.type == Character.Type.Dummy || target.character.type == Character.Type.Trap)
			{
				return;
			}
			if (!MMMaths.PercentChance(this._gunDropPossibilityByKill))
			{
				return;
			}
			this.DropGun(this._gunsByKill, damage.hitPoint);
		}

		// Token: 0x0600454B RID: 17739 RVA: 0x000C91A0 File Offset: 0x000C73A0
		private DroppedMummyGun GetRandomGun(MummyGunDropPassive.DroppedGuns guns)
		{
			MummyGunDropPassive.DroppedGuns.Property[] values = guns.values;
			float num = UnityEngine.Random.Range(0f, values.Sum((MummyGunDropPassive.DroppedGuns.Property a) => a.weight));
			for (int i = 0; i < values.Length; i++)
			{
				num -= values[i].weight;
				if (num <= 0f)
				{
					return values[i].droppedGun;
				}
			}
			return null;
		}

		// Token: 0x0600454C RID: 17740 RVA: 0x000C9210 File Offset: 0x000C7410
		private DroppedMummyGun DropGun(MummyGunDropPassive.DroppedGuns guns, Vector3 position)
		{
			DroppedMummyGun randomGun = this.GetRandomGun(guns);
			if (randomGun == null)
			{
				return null;
			}
			return randomGun.Spawn(position, this._mummyPassive.baseAbility);
		}

		// Token: 0x0600454D RID: 17741 RVA: 0x000C9242 File Offset: 0x000C7442
		public void Attach()
		{
			this.remainTime = 0f;
			Character owner = this.owner;
			owner.onKilled = (Character.OnKilledDelegate)Delegate.Combine(owner.onKilled, new Character.OnKilledDelegate(this.OnOwnerKilled));
		}

		// Token: 0x0600454E RID: 17742 RVA: 0x000C9276 File Offset: 0x000C7476
		public void Detach()
		{
			Character owner = this.owner;
			owner.onKilled = (Character.OnKilledDelegate)Delegate.Remove(owner.onKilled, new Character.OnKilledDelegate(this.OnOwnerKilled));
		}

		// Token: 0x0600454F RID: 17743 RVA: 0x00002191 File Offset: 0x00000391
		public void Refresh()
		{
		}

		// Token: 0x040034AE RID: 13486
		[Constraint.SubcomponentAttribute]
		[SerializeField]
		private Constraint.Subcomponents _constraints;

		// Token: 0x040034AF RID: 13487
		[SerializeField]
		private MummyPassiveComponent _mummyPassive;

		// Token: 0x040034B0 RID: 13488
		[FormerlySerializedAs("_possibility")]
		[SerializeField]
		[Range(1f, 100f)]
		private int _gunDropPossibilityByKill;

		// Token: 0x040034B1 RID: 13489
		[Header("Supply")]
		[Space]
		[SerializeField]
		[FormerlySerializedAs("_supply")]
		private DroppedMummyGunSupply _supplyPrefab;

		// Token: 0x040034B2 RID: 13490
		[SerializeField]
		[Information("보급 주기, 0이면 보급되지 않습니다.", InformationAttribute.InformationType.Info, false)]
		private float _supplyInterval;

		// Token: 0x040034B3 RID: 13491
		private float _remainSupplyTime;

		// Token: 0x040034B4 RID: 13492
		[SerializeField]
		private CustomFloat _supplyWidth = new CustomFloat(-3f, 5f);

		// Token: 0x040034B5 RID: 13493
		[SerializeField]
		private CustomFloat _supplyHeight = new CustomFloat(6.5f, 7.5f);

		// Token: 0x040034B6 RID: 13494
		[Header("Weights")]
		[SerializeField]
		[FormerlySerializedAs("_guns")]
		private MummyGunDropPassive.DroppedGuns _gunsByKill;

		// Token: 0x040034B7 RID: 13495
		[SerializeField]
		private MummyGunDropPassive.DroppedGuns _gunsByPeriodicSupply;

		// Token: 0x040034B8 RID: 13496
		[SerializeField]
		private MummyGunDropPassive.DroppedGuns _gunsBySwapSupply;

		// Token: 0x02000D6D RID: 3437
		[Serializable]
		private class DroppedGuns : ReorderableArray<MummyGunDropPassive.DroppedGuns.Property>
		{
			// Token: 0x02000D6E RID: 3438
			[Serializable]
			internal class Property
			{
				// Token: 0x17000E69 RID: 3689
				// (get) Token: 0x06004552 RID: 17746 RVA: 0x000C92D9 File Offset: 0x000C74D9
				public float weight
				{
					get
					{
						return this._weight;
					}
				}

				// Token: 0x17000E6A RID: 3690
				// (get) Token: 0x06004553 RID: 17747 RVA: 0x000C92E1 File Offset: 0x000C74E1
				public DroppedMummyGun droppedGun
				{
					get
					{
						return this._droppedGun;
					}
				}

				// Token: 0x040034BB RID: 13499
				[SerializeField]
				private float _weight;

				// Token: 0x040034BC RID: 13500
				[SerializeField]
				private DroppedMummyGun _droppedGun;
			}
		}
	}
}
