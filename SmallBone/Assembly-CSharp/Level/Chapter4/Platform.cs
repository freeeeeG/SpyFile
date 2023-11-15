using System;
using System.Collections;
using Characters;
using Characters.Monsters;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Level.Chapter4
{
	// Token: 0x02000639 RID: 1593
	public class Platform : MonoBehaviour, IPurification, IDivineCrossHelp
	{
		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06001FFB RID: 8187 RVA: 0x00060E76 File Offset: 0x0005F076
		public Collider2D collider
		{
			get
			{
				return this._collider;
			}
		}

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x06001FFC RID: 8188 RVA: 0x00060E7E File Offset: 0x0005F07E
		// (set) Token: 0x06001FFD RID: 8189 RVA: 0x00060E86 File Offset: 0x0005F086
		public bool tentacleAlives { get; set; }

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06001FFE RID: 8190 RVA: 0x00060E8F File Offset: 0x0005F08F
		public Transform firePosition
		{
			get
			{
				return this._divineCrossFirePosition;
			}
		}

		// Token: 0x06001FFF RID: 8191 RVA: 0x00060E97 File Offset: 0x0005F097
		private void Awake()
		{
			this._readyOperations.Initialize();
			this._operations.Initialize();
		}

		// Token: 0x06002000 RID: 8192 RVA: 0x00060EAF File Offset: 0x0005F0AF
		public void ShowSign(Character owner)
		{
			this._readyOperations.gameObject.SetActive(true);
			this._readyOperations.Run(owner);
		}

		// Token: 0x06002001 RID: 8193 RVA: 0x00060ECE File Offset: 0x0005F0CE
		public void Purifiy(Character owner)
		{
			base.StartCoroutine(this.CRunPurifiy(owner));
		}

		// Token: 0x06002002 RID: 8194 RVA: 0x00060EDE File Offset: 0x0005F0DE
		private IEnumerator CRunPurifiy(Character owner)
		{
			Platform.<>c__DisplayClass18_0 CS$<>8__locals1 = new Platform.<>c__DisplayClass18_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.summoned = this._tentaclePrefab.Summon(this._spawnPoint.position);
			Map.Instance.waveContainer.summonWave.Attach(CS$<>8__locals1.summoned.character);
			CS$<>8__locals1.summoned.character.health.onDied += CS$<>8__locals1.<CRunPurifiy>g__OnDied|0;
			this.tentacleAlives = true;
			this._operations.gameObject.SetActive(true);
			this._operations.Run(owner);
			yield return owner.chronometer.master.WaitForSeconds(this._duration);
			yield break;
		}

		// Token: 0x06002003 RID: 8195 RVA: 0x00060EF4 File Offset: 0x0005F0F4
		public Vector3 GetFirePosition()
		{
			return this._divineCrossFirePosition.position;
		}

		// Token: 0x04001B1F RID: 6943
		[Header("Purification")]
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _readyOperations;

		// Token: 0x04001B20 RID: 6944
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _operations;

		// Token: 0x04001B21 RID: 6945
		[Space]
		[SerializeField]
		private float _duration;

		// Token: 0x04001B22 RID: 6946
		[SerializeField]
		private Monster _tentaclePrefab;

		// Token: 0x04001B23 RID: 6947
		[SerializeField]
		private Transform _spawnPoint;

		// Token: 0x04001B24 RID: 6948
		[Header("Divine Cross")]
		[SerializeField]
		private Transform _divineCrossFirePosition;

		// Token: 0x04001B25 RID: 6949
		[SerializeField]
		private Collider2D _collider;
	}
}
