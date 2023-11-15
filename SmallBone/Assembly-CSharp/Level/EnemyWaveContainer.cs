using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace Level
{
	// Token: 0x0200053C RID: 1340
	public class EnemyWaveContainer : MonoBehaviour, IEnumerable<Character>, IEnumerable
	{
		// Token: 0x1400002A RID: 42
		// (add) Token: 0x06001A48 RID: 6728 RVA: 0x00052644 File Offset: 0x00050844
		// (remove) Token: 0x06001A49 RID: 6729 RVA: 0x0005267C File Offset: 0x0005087C
		public event Action<EnemyWaveContainer.State> onStateChanged;

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06001A4A RID: 6730 RVA: 0x000526B1 File Offset: 0x000508B1
		// (set) Token: 0x06001A4B RID: 6731 RVA: 0x000526B9 File Offset: 0x000508B9
		public EnemyWaveContainer.State state { get; private set; }

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06001A4C RID: 6732 RVA: 0x000526C2 File Offset: 0x000508C2
		// (set) Token: 0x06001A4D RID: 6733 RVA: 0x000526CA File Offset: 0x000508CA
		public Wave[] waves { get; private set; }

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06001A4E RID: 6734 RVA: 0x000526D3 File Offset: 0x000508D3
		// (set) Token: 0x06001A4F RID: 6735 RVA: 0x000526DB File Offset: 0x000508DB
		public EnemyWave[] enemyWaves { get; private set; }

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06001A50 RID: 6736 RVA: 0x000526E4 File Offset: 0x000508E4
		// (set) Token: 0x06001A51 RID: 6737 RVA: 0x000526EC File Offset: 0x000508EC
		public SummonWave summonWave { get; private set; }

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06001A52 RID: 6738 RVA: 0x000526F5 File Offset: 0x000508F5
		// (set) Token: 0x06001A53 RID: 6739 RVA: 0x000526FD File Offset: 0x000508FD
		public SummonWave summonEnemyWave { get; private set; }

		// Token: 0x06001A54 RID: 6740 RVA: 0x00052708 File Offset: 0x00050908
		public void Initialize()
		{
			this.waves = base.GetComponentsInChildren<Wave>(true);
			this.enemyWaves = base.GetComponentsInChildren<EnemyWave>(true);
			GameObject gameObject = new GameObject("SummonWave");
			gameObject.transform.SetParent(base.transform);
			this.summonWave = gameObject.AddComponent<SummonWave>();
			GameObject gameObject2 = new GameObject("SummonEnemyWave");
			gameObject2.transform.SetParent(base.transform);
			this.summonEnemyWave = gameObject2.AddComponent<SummonWave>();
			foreach (Wave wave in this.waves)
			{
				wave.Initialize();
				wave.onClear += this.CheckWaveState;
				wave.onSpawn += this.CheckWaveState;
			}
			this.summonEnemyWave.onClear += this.CheckWaveState;
			this.state = this.GetState();
			Action<EnemyWaveContainer.State> action = this.onStateChanged;
			if (action == null)
			{
				return;
			}
			action(this.state);
		}

		// Token: 0x06001A55 RID: 6741 RVA: 0x000527FC File Offset: 0x000509FC
		public void HideAll()
		{
			foreach (EnemyWave enemyWave in this.enemyWaves)
			{
				foreach (Character character in enemyWave.characters)
				{
					character.gameObject.SetActive(false);
				}
				enemyWave.gameObject.SetActive(false);
			}
		}

		// Token: 0x06001A56 RID: 6742 RVA: 0x00052878 File Offset: 0x00050A78
		public List<Character> GetAllEnemies()
		{
			List<Character> list = new List<Character>();
			foreach (EnemyWave enemyWave in this.enemyWaves)
			{
				list.AddRange(enemyWave.characters);
			}
			if (this.summonWave != null)
			{
				list.AddRange(this.summonWave.characters);
			}
			if (this.summonEnemyWave != null)
			{
				list.AddRange(this.summonEnemyWave.characters);
			}
			return list;
		}

		// Token: 0x06001A57 RID: 6743 RVA: 0x000528F0 File Offset: 0x00050AF0
		public List<Character> GetAllSpawnedEnemies()
		{
			List<Character> list = new List<Character>();
			foreach (EnemyWave enemyWave in this.enemyWaves)
			{
				if (enemyWave.state == Wave.State.Spawned)
				{
					list.AddRange(enemyWave.characters);
				}
			}
			if (this.summonWave != null)
			{
				list.AddRange(this.summonWave.characters);
			}
			if (this.summonEnemyWave != null)
			{
				list.AddRange(this.summonEnemyWave.characters);
			}
			return list;
		}

		// Token: 0x06001A58 RID: 6744 RVA: 0x00052970 File Offset: 0x00050B70
		public int GetAllSpawnedEnemiesCount()
		{
			int num = 0;
			foreach (EnemyWave enemyWave in this.enemyWaves)
			{
				if (enemyWave.state == Wave.State.Spawned)
				{
					num += enemyWave.characters.Count;
				}
			}
			if (this.summonWave != null)
			{
				num += this.summonWave.characters.Count;
			}
			if (this.summonEnemyWave != null)
			{
				num += this.summonEnemyWave.characters.Count;
			}
			return num;
		}

		// Token: 0x06001A59 RID: 6745 RVA: 0x000529F2 File Offset: 0x00050BF2
		public IEnumerator<Character> GetEnumerator()
		{
			foreach (EnemyWave enemyWave in this.enemyWaves)
			{
				foreach (Character character in enemyWave.characters)
				{
					yield return character;
				}
				List<Character>.Enumerator enumerator = default(List<Character>.Enumerator);
			}
			EnemyWave[] array = null;
			if (this.summonWave != null)
			{
				foreach (Character character2 in this.summonWave.characters)
				{
					yield return character2;
				}
				List<Character>.Enumerator enumerator = default(List<Character>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x06001A5A RID: 6746 RVA: 0x00052A04 File Offset: 0x00050C04
		public Character GetRandomEnemy()
		{
			int num = 0;
			foreach (EnemyWave enemyWave in this.enemyWaves)
			{
				num += enemyWave.characters.Count;
			}
			int num2 = UnityEngine.Random.Range(0, num);
			foreach (EnemyWave enemyWave2 in this.enemyWaves)
			{
				if (num2 < enemyWave2.characters.Count)
				{
					return enemyWave2.characters[num2];
				}
				num2 -= enemyWave2.characters.Count;
			}
			return null;
		}

		// Token: 0x06001A5B RID: 6747 RVA: 0x00052A8B File Offset: 0x00050C8B
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06001A5C RID: 6748 RVA: 0x00052A94 File Offset: 0x00050C94
		public void Stop()
		{
			Wave[] waves = this.waves;
			for (int i = 0; i < waves.Length; i++)
			{
				waves[i].Stop();
			}
		}

		// Token: 0x06001A5D RID: 6749 RVA: 0x00052ABE File Offset: 0x00050CBE
		public void Attach(Character character)
		{
			character.transform.parent = this.summonWave.transform;
			this.summonWave.Attach(character);
		}

		// Token: 0x06001A5E RID: 6750 RVA: 0x00052AE2 File Offset: 0x00050CE2
		public void AttachToSummonEnemyWave(Character character)
		{
			character.transform.parent = this.summonEnemyWave.transform;
			this.summonEnemyWave.Attach(character);
		}

		// Token: 0x06001A5F RID: 6751 RVA: 0x00052B08 File Offset: 0x00050D08
		private EnemyWaveContainer.State GetState()
		{
			EnemyWaveContainer.State result = EnemyWaveContainer.State.Empty;
			Wave[] waves = this.waves;
			for (int i = 0; i < waves.Length; i++)
			{
				if (waves[i].state == Wave.State.Spawned)
				{
					result = EnemyWaveContainer.State.Remain;
					break;
				}
			}
			if (this.summonEnemyWave.state == Wave.State.Spawned)
			{
				result = EnemyWaveContainer.State.Remain;
			}
			return result;
		}

		// Token: 0x06001A60 RID: 6752 RVA: 0x00052B4C File Offset: 0x00050D4C
		private void CheckWaveState()
		{
			EnemyWaveContainer.State state = this.GetState();
			if (this.state != state)
			{
				this.state = state;
				Action<EnemyWaveContainer.State> action = this.onStateChanged;
				if (action == null)
				{
					return;
				}
				action(this.state);
			}
		}

		// Token: 0x0200053D RID: 1341
		public enum State
		{
			// Token: 0x040016FA RID: 5882
			Empty,
			// Token: 0x040016FB RID: 5883
			Remain
		}
	}
}
