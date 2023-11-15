using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Actions;
using Characters.AI.Behaviours;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x0200108C RID: 4236
	public sealed class AlchemistSummonerAI : AIController
	{
		// Token: 0x060051FD RID: 20989 RVA: 0x000F61DB File Offset: 0x000F43DB
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._checkWithinSight
			};
			this.character.status.unstoppable.Attach(this);
		}

		// Token: 0x060051FE RID: 20990 RVA: 0x000F620A File Offset: 0x000F440A
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x060051FF RID: 20991 RVA: 0x000F6232 File Offset: 0x000F4432
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			while (!base.dead)
			{
				bool flag = base.FindClosestPlayerBody(this._sightRange) || base.FindClosestPlayerBody(this._anotherSummonerSightRange);
				bool flag2 = base.lastAttacker != null;
				if (flag || flag2)
				{
					this.StartSummon();
					this._anotherSummonerAI.StartSummon();
					break;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x06005200 RID: 20992 RVA: 0x000F6241 File Offset: 0x000F4441
		public void StartSummon()
		{
			this._takeNotesAudioSource.Stop();
			if (!base.gameObject.activeSelf)
			{
				return;
			}
			this._prepareSummon.TryStart();
		}

		// Token: 0x040041D2 RID: 16850
		[SerializeField]
		private AlchemistSummonerAI _anotherSummonerAI;

		// Token: 0x040041D3 RID: 16851
		[SerializeField]
		private Collider2D _sightRange;

		// Token: 0x040041D4 RID: 16852
		[SerializeField]
		private Collider2D _anotherSummonerSightRange;

		// Token: 0x040041D5 RID: 16853
		[Subcomponent(typeof(CheckWithinSight))]
		[SerializeField]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x040041D6 RID: 16854
		[SerializeField]
		[Header("PrepareSummon")]
		private ChainAction _prepareSummon;

		// Token: 0x040041D7 RID: 16855
		[SerializeField]
		private RepeatPlaySound _takeNotesAudioSource;
	}
}
