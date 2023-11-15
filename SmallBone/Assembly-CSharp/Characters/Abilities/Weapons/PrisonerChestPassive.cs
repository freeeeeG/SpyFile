using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Gear.Weapons;
using Characters.Movements;
using Data;
using Level;
using PhysicsUtils;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Weapons
{
	// Token: 0x02000BE8 RID: 3048
	[Serializable]
	public class PrisonerChestPassive : Ability
	{
		// Token: 0x06003E9D RID: 16029 RVA: 0x000B5F52 File Offset: 0x000B4152
		public override IAbilityInstance CreateInstance(Character owner)
		{
			this._overlapper = new NonAllocOverlapper(1);
			return new PrisonerChestPassive.Instance(owner, this);
		}

		// Token: 0x06003E9E RID: 16030 RVA: 0x000B5F68 File Offset: 0x000B4168
		private bool TryGetChestSpawnPosition(System.Random random, out Vector3 position)
		{
			List<Character> allEnemies = Map.Instance.waveContainer.GetAllEnemies();
			allEnemies.PseudoShuffle(random);
			for (int i = 0; i < allEnemies.Count; i++)
			{
				Character character = allEnemies[i];
				if (character.type == Character.Type.TrashMob && !(character.movement == null) && character.movement.baseConfig.type != Movement.Config.Type.AcceleratingFlying && character.movement.baseConfig.type != Movement.Config.Type.Flying)
				{
					RaycastHit2D hit = Physics2D.Raycast(character.transform.position, Vector2.down, 10f, Layers.groundMask);
					if (hit && !(hit.collider.GetComponent<PolygonCollider2D>() == null) && hit.collider.bounds.size.x >= this._widthForChest)
					{
						position = new Vector2(hit.collider.bounds.center.x, hit.collider.bounds.max.y);
						Cage cage = Map.Instance.cage;
						if ((!(cage != null) || !cage.isActiveAndEnabled || Vector2.SqrMagnitude(position - cage.transform.position) >= 9f) && this.CheckPlayableArea(position))
						{
							return true;
						}
					}
				}
			}
			position = Vector3.zero;
			return false;
		}

		// Token: 0x06003E9F RID: 16031 RVA: 0x000B6110 File Offset: 0x000B4310
		private bool CheckPlayableArea(Vector2 point)
		{
			this._cutSceneArea.transform.position = point;
			this._overlapper.contactFilter.SetLayerMask(256);
			Physics2D.SyncTransforms();
			return this._overlapper.OverlapCollider(this._cutSceneArea).results.Count <= 0;
		}

		// Token: 0x06003EA0 RID: 16032 RVA: 0x000B6174 File Offset: 0x000B4374
		private void SpawnChest()
		{
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			System.Random random = new System.Random((int)(GameData.Save.instance.randomSeed + -612673708 + currentChapter.type * (Chapter.Type)256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex));
			if (Map.Instance.type != Map.Type.Normal)
			{
				return;
			}
			if (!MMMaths.PercentChance(random, this._chestChance))
			{
				return;
			}
			Vector3 position;
			if (!this.TryGetChestSpawnPosition(random, out position))
			{
				Debug.Log("spawn chest failed");
				return;
			}
			PrisonerChest prisonerChest = UnityEngine.Object.Instantiate<PrisonerChest>(MMMaths.PercentChance(random, this._cursedChestChance) ? this._cursedChest : this._chest, position, Quaternion.identity, Map.Instance.transform);
			int gradeBonus = prisonerChest.GetGradeBonus();
			PrisonerSkill skill1 = this._weapon.currentSkills[0].GetComponent<PrisonerSkill>();
			PrisonerSkill skill2 = this._weapon.currentSkills[1].GetComponent<PrisonerSkill>();
			int scrollGrade = (skill1.level + skill2.level + gradeBonus) / 2;
			int num = skill1.parent.skillInfos.Count - 1;
			if (scrollGrade > num)
			{
				scrollGrade = num;
			}
			PrisonerSkillInfosByGrade prisonerSkillInfosByGrade = (from s in this._skills
			where (skill1.level < scrollGrade || !skill1.parent.key.Equals(s.key, StringComparison.OrdinalIgnoreCase)) && (skill2.level < scrollGrade || !skill2.parent.key.Equals(s.key, StringComparison.OrdinalIgnoreCase))
			select s).Random(random);
			SkillInfo skillInfo = prisonerSkillInfosByGrade.skillInfos[scrollGrade];
			prisonerChest.SetSkillInfo(this._weapon, prisonerSkillInfosByGrade, skillInfo);
		}

		// Token: 0x0400304A RID: 12362
		private const int _randomSeed = -612673708;

		// Token: 0x0400304B RID: 12363
		[SerializeField]
		private Weapon _weapon;

		// Token: 0x0400304C RID: 12364
		[SerializeField]
		[Space]
		private PrisonerChest _chest;

		// Token: 0x0400304D RID: 12365
		[SerializeField]
		private PrisonerChest _cursedChest;

		// Token: 0x0400304E RID: 12366
		[SerializeField]
		[Space]
		private PrisonerSkillInfosByGrade[] _skills;

		// Token: 0x0400304F RID: 12367
		[Range(0f, 100f)]
		[Header("상자가 나올 확률")]
		[SerializeField]
		private int _chestChance;

		// Token: 0x04003050 RID: 12368
		[Header("상자가 나왔을 때, 저주받은 상자일 확률")]
		[Range(0f, 100f)]
		[SerializeField]
		private int _cursedChestChance;

		// Token: 0x04003051 RID: 12369
		[SerializeField]
		[Header("상자 연출을 위해 필요한 좌우 너비")]
		private float _widthForChest;

		// Token: 0x04003052 RID: 12370
		[SerializeField]
		private BoxCollider2D _cutSceneArea;

		// Token: 0x04003053 RID: 12371
		private NonAllocOverlapper _overlapper;

		// Token: 0x02000BE9 RID: 3049
		public class Instance : AbilityInstance<PrisonerChestPassive>
		{
			// Token: 0x06003EA2 RID: 16034 RVA: 0x000B6302 File Offset: 0x000B4502
			public Instance(Character owner, PrisonerChestPassive ability) : base(owner, ability)
			{
			}

			// Token: 0x06003EA3 RID: 16035 RVA: 0x000B630C File Offset: 0x000B450C
			protected override void OnAttach()
			{
				Singleton<Service>.Instance.levelManager.onMapLoadedAndFadedIn += this.ability.SpawnChest;
			}

			// Token: 0x06003EA4 RID: 16036 RVA: 0x000B632E File Offset: 0x000B452E
			protected override void OnDetach()
			{
				Singleton<Service>.Instance.levelManager.onMapLoadedAndFadedIn -= this.ability.SpawnChest;
			}
		}
	}
}
