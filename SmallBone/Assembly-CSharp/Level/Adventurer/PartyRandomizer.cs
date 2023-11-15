using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using Characters;
using CutScenes;
using Data;
using Hardmode;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Level.Adventurer
{
	// Token: 0x02000693 RID: 1683
	[Serializable]
	public class PartyRandomizer
	{
		// Token: 0x060021A2 RID: 8610 RVA: 0x00065104 File Offset: 0x00063304
		public void Initialize()
		{
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			this._random = new System.Random((int)(GameData.Save.instance.randomSeed + 1020464638 + currentChapter.type * (Chapter.Type)256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex));
			this._adventurerReferences.PseudoShuffle(this._random);
		}

		// Token: 0x060021A3 RID: 8611 RVA: 0x00065170 File Offset: 0x00063370
		private Character SpawnCharacter(AssetReference reference, Transform transform, Vector3 position)
		{
			AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(reference);
			Character result = UnityEngine.Object.Instantiate<Character>(handle.WaitForCompletion().GetComponent<Character>(), position, Quaternion.identity, transform);
			Addressables.Release<GameObject>(handle);
			return result;
		}

		// Token: 0x060021A4 RID: 8612 RVA: 0x000651A4 File Offset: 0x000633A4
		public List<Character> SpawnCharacters()
		{
			List<Character> list = new List<Character>();
			if (this._adventurerReferences.Length < this._spawnPoints.Length)
			{
				Debug.LogError("캐릭터 배열의 크기가 작습니다");
				return list;
			}
			if (this.CanPlayHardmodeCutScene())
			{
				if (this._spawnPoints.Length == 0)
				{
					Debug.LogError("캐릭터 배열의 크기가 작습니다");
					return list;
				}
				Character item = this.SpawnCharacter(this._tutorialWarriorReference, this._enemyWave.transform, this._spawnPoints[0].position);
				list.Add(item);
			}
			else if (this.CanPlayNormalmodeCutScene())
			{
				if (this._spawnPoints.Length == 0)
				{
					Debug.LogError("캐릭터 배열의 크기가 작습니다");
					return list;
				}
				Character item2 = this.SpawnCharacter(this._firstMeetingCharacterReference, this._enemyWave.transform, this._spawnPoints[0].position);
				list.Add(item2);
			}
			else
			{
				for (int i = 0; i < this._spawnPoints.Length; i++)
				{
					Character item3 = this.SpawnCharacter(this._adventurerReferences[i]._character, this._enemyWave.transform, this._spawnPoints[i].position);
					list.Add(item3);
				}
			}
			this._enemyWave.Initialize();
			return list;
		}

		// Token: 0x060021A5 RID: 8613 RVA: 0x000652C4 File Offset: 0x000634C4
		public Character SpawnSupportingCharacter()
		{
			if (this._adventurerReferences.Length <= this._spawnPoints.Length)
			{
				Debug.LogError("캐릭터 배열의 크기가 작습니다");
				return null;
			}
			if (this.CanPlayHardmodeCutScene())
			{
				Character character = this.SpawnCharacter(this._tutorialSupportingHeroReference, this._enemyWave.transform, this._supportingCharacterSpawnPoint.position);
				BehaviorDesignerCommunicator tutorialHeroCommunicator = character.GetComponent<BehaviorDesignerCommunicator>();
				this._enemyWave.onClear += delegate()
				{
					tutorialHeroCommunicator.SetVariable<SharedBool>(this._waveClearKey, true);
				};
				return character;
			}
			AssetReference supportingCharacter = this._adventurerReferences[this._spawnPoints.Length]._supportingCharacter;
			if (supportingCharacter == null || this._supportingCharacterSpawnPoint == null)
			{
				return null;
			}
			Character character2 = this.SpawnCharacter(supportingCharacter, this._enemyWave.transform, this._supportingCharacterSpawnPoint.position);
			BehaviorDesignerCommunicator communicator = character2.GetComponent<BehaviorDesignerCommunicator>();
			this._enemyWave.onClear += delegate()
			{
				communicator.SetVariable<SharedBool>(this._waveClearKey, true);
			};
			int[] enumerable = new int[]
			{
				0,
				1
			};
			communicator.SetVariable<SharedInt>(this._patternKey, enumerable.Random(this._random));
			return character2;
		}

		// Token: 0x060021A6 RID: 8614 RVA: 0x000653F7 File Offset: 0x000635F7
		public bool CanPlayNormalmodeCutScene()
		{
			return !Singleton<HardmodeManager>.Instance.hardmode && this._firstMeetingCharacterReference != null && this._firstMeetingCharacterReference.RuntimeKeyIsValid() && !GameData.Progress.cutscene.GetData(CutScenes.Key.rookieHero);
		}

		// Token: 0x060021A7 RID: 8615 RVA: 0x0006542C File Offset: 0x0006362C
		public bool CanPlayHardmodeCutScene()
		{
			return Singleton<HardmodeManager>.Instance.hardmode && !GameData.Progress.cutscene.GetData(CutScenes.Key.supportingAdventurer_First) && this._tutorialSupportingHeroReference != null && this._tutorialWarriorReference != null && this._tutorialSupportingHeroReference.RuntimeKeyIsValid() && this._tutorialWarriorReference.RuntimeKeyIsValid();
		}

		// Token: 0x04001CB2 RID: 7346
		private const int _randomSeed = 1020464638;

		// Token: 0x04001CB3 RID: 7347
		[SerializeField]
		private PartyRandomizer.AdventurerReference[] _adventurerReferences;

		// Token: 0x04001CB4 RID: 7348
		[SerializeField]
		private AssetReference _firstMeetingCharacterReference;

		// Token: 0x04001CB5 RID: 7349
		[SerializeField]
		private AssetReference _tutorialSupportingHeroReference;

		// Token: 0x04001CB6 RID: 7350
		[SerializeField]
		private AssetReference _tutorialWarriorReference;

		// Token: 0x04001CB7 RID: 7351
		[SerializeField]
		private Transform[] _spawnPoints;

		// Token: 0x04001CB8 RID: 7352
		[SerializeField]
		private Transform _supportingCharacterSpawnPoint;

		// Token: 0x04001CB9 RID: 7353
		[SerializeField]
		private EnemyWave _enemyWave;

		// Token: 0x04001CBA RID: 7354
		private System.Random _random;

		// Token: 0x04001CBB RID: 7355
		private string _waveClearKey = "WaveClear";

		// Token: 0x04001CBC RID: 7356
		private string _patternKey = "Pattern";

		// Token: 0x02000694 RID: 1684
		[Serializable]
		private class AdventurerReference
		{
			// Token: 0x04001CBD RID: 7357
			[SerializeField]
			internal AssetReference _character;

			// Token: 0x04001CBE RID: 7358
			[SerializeField]
			internal AssetReference _supportingCharacter;
		}
	}
}
