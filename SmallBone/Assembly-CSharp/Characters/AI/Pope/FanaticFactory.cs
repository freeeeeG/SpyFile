using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Characters.AI.Pope.Summon;
using Level;
using Level.Chapter4;
using UnityEngine;

namespace Characters.AI.Pope
{
	// Token: 0x02001204 RID: 4612
	public class FanaticFactory : MonoBehaviour
	{
		// Token: 0x06005A7F RID: 23167 RVA: 0x0010C7AC File Offset: 0x0010A9AC
		private void Awake()
		{
			this._fanatics = new Dictionary<FanaticFactory.SummonType, Character>(this._config.toSummonTypes.Length);
			foreach (FanaticFactory.SummonInfo summonInfo in this._config.fanaticPrefabs)
			{
				this._fanatics.Add(summonInfo.tag, summonInfo.prefab);
			}
		}

		// Token: 0x06005A80 RID: 23168 RVA: 0x0010C806 File Offset: 0x0010AA06
		public void StartToSummon()
		{
			this._running = true;
			base.StartCoroutine(this.CProcess());
			this._scenario.OnPhase1End += this.DropFanatics;
		}

		// Token: 0x06005A81 RID: 23169 RVA: 0x0010C833 File Offset: 0x0010AA33
		public void StopToSummon()
		{
			this._running = false;
			this._scenario.OnPhase1End -= this.DropFanatics;
		}

		// Token: 0x06005A82 RID: 23170 RVA: 0x0010C854 File Offset: 0x0010AA54
		public void DropFanatics()
		{
			foreach (FanaticLadder fanaticLadder in this.ladders)
			{
				base.StartCoroutine(fanaticLadder.CFall());
			}
		}

		// Token: 0x06005A83 RID: 23171 RVA: 0x0010C8B0 File Offset: 0x0010AAB0
		public IEnumerator CProcess()
		{
			if (this._config.fanaticPrefabs.Length == 0)
			{
				Debug.LogError("Fanatics count is 0");
				yield break;
			}
			while (this._running)
			{
				this.ladders = this._ladderPolicy.GetLadders(this._fanaticPolicy.GetToSummons(this._countPolicy.GetCount()));
				foreach (FanaticLadder ladder in this.ladders)
				{
					base.StartCoroutine(this.CSummon(ladder));
				}
				yield return Chronometer.global.WaitForSeconds(this._config.interval);
			}
			yield break;
		}

		// Token: 0x06005A84 RID: 23172 RVA: 0x0010C8BF File Offset: 0x0010AABF
		private IEnumerator CSummon(FanaticLadder ladder)
		{
			yield return ladder.CClimb();
			if (this._running)
			{
				Character spawned = UnityEngine.Object.Instantiate<Character>(this._fanatics[ladder.fanatic], ladder.spawnPoint, Quaternion.identity, this._config.spawnedContainer);
				spawned.health.onDied += delegate()
				{
					UnityEngine.Object.Destroy(spawned);
				};
				Map.Instance.waveContainer.Attach(spawned);
			}
			yield break;
		}

		// Token: 0x04004910 RID: 18704
		[SerializeField]
		private Scenario _scenario;

		// Token: 0x04004911 RID: 18705
		[SerializeField]
		private FanaticFactory.Config _config;

		// Token: 0x04004912 RID: 18706
		[CountPolicy.SubcomponentAttribute(true)]
		[SerializeField]
		private CountPolicy _countPolicy;

		// Token: 0x04004913 RID: 18707
		[SerializeField]
		[FanaticPolicy.SubcomponentAttribute(true)]
		private FanaticPolicy _fanaticPolicy;

		// Token: 0x04004914 RID: 18708
		[SerializeField]
		[LadderPolicy.SubcomponentAttribute(true)]
		private LadderPolicy _ladderPolicy;

		// Token: 0x04004915 RID: 18709
		private Dictionary<FanaticFactory.SummonType, Character> _fanatics;

		// Token: 0x04004916 RID: 18710
		private List<FanaticLadder> ladders;

		// Token: 0x04004917 RID: 18711
		private bool _running;

		// Token: 0x02001205 RID: 4613
		public enum SummonType
		{
			// Token: 0x04004919 RID: 18713
			Fanatic,
			// Token: 0x0400491A RID: 18714
			AgedFanatic,
			// Token: 0x0400491B RID: 18715
			MartyrFanatic
		}

		// Token: 0x02001206 RID: 4614
		[Serializable]
		internal class SummonInfo
		{
			// Token: 0x17001207 RID: 4615
			// (get) Token: 0x06005A86 RID: 23174 RVA: 0x0010C8D5 File Offset: 0x0010AAD5
			internal FanaticFactory.SummonType tag
			{
				get
				{
					return this._tag;
				}
			}

			// Token: 0x17001208 RID: 4616
			// (get) Token: 0x06005A87 RID: 23175 RVA: 0x0010C8DD File Offset: 0x0010AADD
			internal Character prefab
			{
				get
				{
					return this._prefab;
				}
			}

			// Token: 0x0400491C RID: 18716
			[SerializeField]
			private FanaticFactory.SummonType _tag;

			// Token: 0x0400491D RID: 18717
			[SerializeField]
			private Character _prefab;
		}

		// Token: 0x02001207 RID: 4615
		[Serializable]
		public class Config
		{
			// Token: 0x17001209 RID: 4617
			// (get) Token: 0x06005A89 RID: 23177 RVA: 0x0010C8E5 File Offset: 0x0010AAE5
			internal float interval
			{
				get
				{
					return this._interval;
				}
			}

			// Token: 0x1700120A RID: 4618
			// (get) Token: 0x06005A8A RID: 23178 RVA: 0x0010C8ED File Offset: 0x0010AAED
			internal FanaticFactory.SummonInfo[] fanaticPrefabs
			{
				get
				{
					return this._fanaticPrefabs;
				}
			}

			// Token: 0x1700120B RID: 4619
			// (get) Token: 0x06005A8B RID: 23179 RVA: 0x0010C8F8 File Offset: 0x0010AAF8
			internal FanaticFactory.SummonType[] toSummonTypes
			{
				get
				{
					if (this.toSummonTypesCached == null)
					{
						this.toSummonTypesCached = (from x in this.fanaticPrefabs
						select x.tag).ToArray<FanaticFactory.SummonType>();
					}
					return this.toSummonTypesCached;
				}
			}

			// Token: 0x1700120C RID: 4620
			// (get) Token: 0x06005A8C RID: 23180 RVA: 0x0010C948 File Offset: 0x0010AB48
			internal Transform spawnedContainer
			{
				get
				{
					return this._spawnedContainer;
				}
			}

			// Token: 0x0400491E RID: 18718
			[SerializeField]
			private float _interval;

			// Token: 0x0400491F RID: 18719
			[SerializeField]
			private Transform _spawnedContainer;

			// Token: 0x04004920 RID: 18720
			[SerializeField]
			private FanaticFactory.SummonInfo[] _fanaticPrefabs;

			// Token: 0x04004921 RID: 18721
			private FanaticFactory.SummonType[] toSummonTypesCached;
		}
	}
}
