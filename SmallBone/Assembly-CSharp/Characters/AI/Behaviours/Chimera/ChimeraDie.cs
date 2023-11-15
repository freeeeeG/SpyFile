using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Chimera
{
	// Token: 0x02001370 RID: 4976
	public class ChimeraDie : MonoBehaviour
	{
		// Token: 0x06006201 RID: 25089 RVA: 0x0011E240 File Offset: 0x0011C440
		private void Awake()
		{
			this._pauesOperations.Initialize();
			this._readyOperations.Initialize();
			this._startOperations.Initialize();
			this._breakTerrainOperations.Initialize();
			this._struggle1Operations.Initialize();
			this._struggle2Operations.Initialize();
			this._fallOperations.Initialize();
			this._waterOperations.Initialize();
		}

		// Token: 0x06006202 RID: 25090 RVA: 0x0011E2A5 File Offset: 0x0011C4A5
		public void KillAllEnemyInBounds(AIController controller)
		{
			base.StartCoroutine(this.KillLoop(controller));
		}

		// Token: 0x06006203 RID: 25091 RVA: 0x0011E2B5 File Offset: 0x0011C4B5
		private IEnumerator KillLoop(AIController controller)
		{
			float duration = 10f;
			float elapsed = 0f;
			while (elapsed < duration)
			{
				List<Character> list = controller.FindEnemiesInRange(this._killEnemyRange);
				for (int i = 0; i < list.Count; i++)
				{
					if (!(controller.character == list[i]) && !(list[i].health == null))
					{
						list[i].health.Kill();
					}
				}
				elapsed += controller.character.chronometer.animation.deltaTime;
				yield return null;
			}
			yield break;
		}

		// Token: 0x06006204 RID: 25092 RVA: 0x0011E2CB File Offset: 0x0011C4CB
		public void Pause(Character character)
		{
			this._pauesOperations.gameObject.SetActive(true);
			this._pauesOperations.Run(character);
		}

		// Token: 0x06006205 RID: 25093 RVA: 0x0011E2EA File Offset: 0x0011C4EA
		public void Ready(Character character)
		{
			this._readyOperations.gameObject.SetActive(true);
			this._readyOperations.Run(character);
		}

		// Token: 0x06006206 RID: 25094 RVA: 0x0011E309 File Offset: 0x0011C509
		public void Down(Character character)
		{
			this._startOperations.gameObject.SetActive(true);
			this._startOperations.Run(character);
		}

		// Token: 0x06006207 RID: 25095 RVA: 0x0011E328 File Offset: 0x0011C528
		public void BreakTerrain(Character character)
		{
			this._breakTerrainOperations.gameObject.SetActive(true);
			this._breakTerrainOperations.Run(character);
		}

		// Token: 0x06006208 RID: 25096 RVA: 0x0011E347 File Offset: 0x0011C547
		public void Struggle1(Character character)
		{
			this._struggle1Operations.gameObject.SetActive(true);
			this._struggle1Operations.Run(character);
		}

		// Token: 0x06006209 RID: 25097 RVA: 0x0011E366 File Offset: 0x0011C566
		public void Struggle2(Character character)
		{
			this._struggle2Operations.gameObject.SetActive(true);
			this._struggle2Operations.Run(character);
		}

		// Token: 0x0600620A RID: 25098 RVA: 0x0011E385 File Offset: 0x0011C585
		public void Fall(Character character)
		{
			this._fallOperations.gameObject.SetActive(true);
			this._fallOperations.Run(character);
		}

		// Token: 0x0600620B RID: 25099 RVA: 0x0011E3A4 File Offset: 0x0011C5A4
		public void Water(Character character, Chapter3Script script)
		{
			this._waterOperations.gameObject.SetActive(true);
			this._waterOperations.Run(character);
			script.EndOutro();
		}

		// Token: 0x04004F0C RID: 20236
		[SerializeField]
		[Header("Pause")]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _pauesOperations;

		// Token: 0x04004F0D RID: 20237
		[Subcomponent(typeof(OperationInfos))]
		[Header("Ready")]
		[SerializeField]
		private OperationInfos _readyOperations;

		// Token: 0x04004F0E RID: 20238
		[SerializeField]
		[Header("Start")]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _startOperations;

		// Token: 0x04004F0F RID: 20239
		[Header("BreakTerrain")]
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _breakTerrainOperations;

		// Token: 0x04004F10 RID: 20240
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		[Header("Struggle1")]
		private OperationInfos _struggle1Operations;

		// Token: 0x04004F11 RID: 20241
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		[Header("Struggle2")]
		private OperationInfos _struggle2Operations;

		// Token: 0x04004F12 RID: 20242
		[Header("Fall")]
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _fallOperations;

		// Token: 0x04004F13 RID: 20243
		[Header("Water")]
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _waterOperations;

		// Token: 0x04004F14 RID: 20244
		[Header("KillEnemy")]
		[SerializeField]
		private Collider2D _killEnemyRange;
	}
}
