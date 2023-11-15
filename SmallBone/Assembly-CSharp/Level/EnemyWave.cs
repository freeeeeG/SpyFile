using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Characters;
using Data;
using FX;
using GameResources;
using Level.Waves;
using PhysicsUtils;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Level
{
	// Token: 0x02000533 RID: 1331
	public class EnemyWave : Wave
	{
		// Token: 0x06001A1E RID: 6686 RVA: 0x00051D2D File Offset: 0x0004FF2D
		static EnemyWave()
		{
			EnemyWave._overlapper.contactFilter.SetLayerMask(512);
		}

		// Token: 0x14000029 RID: 41
		// (add) Token: 0x06001A1F RID: 6687 RVA: 0x00051D54 File Offset: 0x0004FF54
		// (remove) Token: 0x06001A20 RID: 6688 RVA: 0x00051D8C File Offset: 0x0004FF8C
		public event Action<int> onChildrenChanged;

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x06001A21 RID: 6689 RVA: 0x00051DC1 File Offset: 0x0004FFC1
		public int remains
		{
			get
			{
				return this._remains;
			}
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06001A22 RID: 6690 RVA: 0x00051DC9 File Offset: 0x0004FFC9
		public string[] keys
		{
			get
			{
				return this._keys;
			}
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06001A23 RID: 6691 RVA: 0x00051DD1 File Offset: 0x0004FFD1
		// (set) Token: 0x06001A24 RID: 6692 RVA: 0x00051DD9 File Offset: 0x0004FFD9
		public List<Character> characters { get; private set; }

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x06001A25 RID: 6693 RVA: 0x00051DE2 File Offset: 0x0004FFE2
		// (set) Token: 0x06001A26 RID: 6694 RVA: 0x00051DEA File Offset: 0x0004FFEA
		public List<DestructibleObject> destructibleObjects { get; private set; }

		// Token: 0x06001A27 RID: 6695 RVA: 0x00051DF4 File Offset: 0x0004FFF4
		public void Spawn(bool effect = true)
		{
			try
			{
				if (base.state == Wave.State.Waiting)
				{
					base.state = Wave.State.Spawned;
					Action onSpawn = this._onSpawn;
					if (onSpawn != null)
					{
						onSpawn();
					}
					if (effect)
					{
						base.StartCoroutine(this.<Spawn>g__CRun|25_0());
					}
					else
					{
						foreach (Character character in this.characters)
						{
							character.gameObject.SetActive(true);
						}
						foreach (DestructibleObject destructibleObject in this.destructibleObjects)
						{
							destructibleObject.gameObject.SetActive(true);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.Log("Error while spawn enemy wave : " + ex.Message);
				this.Clear();
			}
		}

		// Token: 0x06001A28 RID: 6696 RVA: 0x00051EF4 File Offset: 0x000500F4
		private void Clear()
		{
			if (base.state == Wave.State.Cleared)
			{
				return;
			}
			base.state = Wave.State.Cleared;
			Action onClear = this._onClear;
			if (onClear == null)
			{
				return;
			}
			onClear();
		}

		// Token: 0x06001A29 RID: 6697 RVA: 0x00051F17 File Offset: 0x00050117
		private void DecreaseRemains()
		{
			this._remains--;
			Action<int> action = this.onChildrenChanged;
			if (action != null)
			{
				action(this._remains);
			}
			if (this._remains == 0)
			{
				this.Clear();
			}
		}

		// Token: 0x06001A2A RID: 6698 RVA: 0x00051F4C File Offset: 0x0005014C
		public override void Initialize()
		{
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			new System.Random((int)(GameData.Save.instance.randomSeed + 1787508074 + currentChapter.type * (Chapter.Type)256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex));
			this.destructibleObjects = new List<DestructibleObject>();
			this.characters = new List<Character>();
			this.<Initialize>g__AddCharacterOrDestructibleObject|28_0(base.transform);
			this._remains = this.characters.Count + this.destructibleObjects.Count;
			base.gameObject.SetActive(true);
			base.StartCoroutine("CCheckSpawnConditions");
		}

		// Token: 0x06001A2B RID: 6699 RVA: 0x00051FF9 File Offset: 0x000501F9
		private IEnumerator CCheckSpawnConditions()
		{
			if (this._conditions == null)
			{
				yield break;
			}
			yield return null;
			while (!this._conditions.IsSatisfied(this))
			{
				yield return null;
			}
			if (base.state != Wave.State.Waiting)
			{
				yield break;
			}
			this.Spawn(true);
			yield break;
		}

		// Token: 0x06001A2D RID: 6701 RVA: 0x00052017 File Offset: 0x00050217
		[CompilerGenerated]
		private IEnumerator <Spawn>g__CRun|25_0()
		{
			foreach (Character character in this.characters)
			{
				if (!character.gameObject.activeSelf)
				{
					EnemyWave.Assets.enemyAppearance.Spawn(character.transform.position, 0f, 1f);
				}
			}
			foreach (DestructibleObject destructibleObject in this.destructibleObjects)
			{
				if (!destructibleObject.gameObject.activeSelf)
				{
					EnemyWave.Assets.enemyAppearance.Spawn(destructibleObject.transform.position, 0f, 1f);
				}
			}
			yield return Chronometer.global.WaitForSeconds(0.4f);
			foreach (Character character2 in this.characters)
			{
				character2.gameObject.SetActive(true);
			}
			using (List<DestructibleObject>.Enumerator enumerator2 = this.destructibleObjects.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					DestructibleObject destructibleObject2 = enumerator2.Current;
					destructibleObject2.gameObject.SetActive(true);
				}
				yield break;
			}
			yield break;
		}

		// Token: 0x06001A2E RID: 6702 RVA: 0x00052028 File Offset: 0x00050228
		[CompilerGenerated]
		private void <Initialize>g__AddCharacterOrDestructibleObject|28_0(Transform transform)
		{
			foreach (object obj in transform)
			{
				Transform transform2 = (Transform)obj;
				GroupSelector component = transform2.GetComponent<GroupSelector>();
				Character character = transform2.GetComponent<Character>();
				DestructibleObject destructibleObject = transform2.GetComponent<DestructibleObject>();
				if (component != null)
				{
					ICollection<Character> collection = component.Load();
					if (collection.Count == 0)
					{
						Debug.Log("Wave (" + base.gameObject.name + ")가 비어있습니다");
						this.Clear();
					}
					using (IEnumerator<Character> enumerator2 = collection.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							Character enemy = enumerator2.Current;
							this.characters.Add(enemy);
							enemy.health.onDiedTryCatch += delegate()
							{
								this.DecreaseRemains();
								this.characters.Remove(enemy);
							};
							if (this._deactiveOnAwake)
							{
								enemy.gameObject.SetActive(false);
							}
						}
						continue;
					}
				}
				if (character != null)
				{
					this.characters.Add(character);
					character.health.onDiedTryCatch += delegate()
					{
						this.DecreaseRemains();
						this.characters.Remove(character);
					};
					if (this._deactiveOnAwake)
					{
						character.gameObject.SetActive(false);
					}
				}
				else if (destructibleObject != null)
				{
					this.destructibleObjects.Add(destructibleObject);
					destructibleObject.onDestroy += delegate()
					{
						this.DecreaseRemains();
						this.destructibleObjects.Remove(destructibleObject);
					};
					if (this._deactiveOnAwake)
					{
						destructibleObject.gameObject.SetActive(false);
					}
				}
				else
				{
					this.<Initialize>g__AddCharacterOrDestructibleObject|28_0(transform2);
				}
			}
		}

		// Token: 0x040016D9 RID: 5849
		private const int _randomSeed = 1787508074;

		// Token: 0x040016DA RID: 5850
		private static readonly NonAllocOverlapper _overlapper = new NonAllocOverlapper(1);

		// Token: 0x040016DB RID: 5851
		private const float _enemySpawnDelay = 0.4f;

		// Token: 0x040016DD RID: 5853
		[SerializeField]
		private bool _deactiveOnAwake = true;

		// Token: 0x040016DE RID: 5854
		[SerializeField]
		private string[] _keys;

		// Token: 0x040016DF RID: 5855
		[SerializeField]
		[Subcomponent(typeof(SpawnConditionInfo))]
		private SpawnConditionInfo _conditions;

		// Token: 0x040016E0 RID: 5856
		private int _remains;

		// Token: 0x02000534 RID: 1332
		[Serializable]
		private class GameObjectRandomizer
		{
			// Token: 0x040016E3 RID: 5859
			[SerializeField]
			private GameObject _gameObject;

			// Token: 0x040016E4 RID: 5860
			[SerializeField]
			private int _weight = 1;

			// Token: 0x02000535 RID: 1333
			[Serializable]
			public class Reorderable : ReorderableArray<EnemyWave.GameObjectRandomizer>
			{
				// Token: 0x06001A30 RID: 6704 RVA: 0x00052258 File Offset: 0x00050458
				public void Randomize(System.Random random)
				{
					if (this.values.Length == 0)
					{
						return;
					}
					int maxValue = this.values.Sum((EnemyWave.GameObjectRandomizer v) => v._weight);
					int num = random.Next(0, maxValue) + 1;
					int num2 = 0;
					for (int i = 0; i < this.values.Length; i++)
					{
						num -= this.values[i]._weight;
						if (num <= 0)
						{
							num2 = i;
							break;
						}
					}
					for (int j = 0; j < this.values.Length; j++)
					{
						if (j != num2)
						{
							GameObject gameObject = this.values[j]._gameObject;
							gameObject.transform.parent = null;
							UnityEngine.Object.Destroy(gameObject);
						}
					}
				}
			}
		}

		// Token: 0x02000537 RID: 1335
		private class Assets
		{
			// Token: 0x040016E7 RID: 5863
			internal static EffectInfo enemyAppearance = new EffectInfo(CommonResource.instance.enemyAppearanceEffect)
			{
				sortingLayerId = SortingLayer.NameToID("Summon")
			};
		}
	}
}
