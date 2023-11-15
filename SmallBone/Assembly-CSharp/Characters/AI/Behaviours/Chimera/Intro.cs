using System;
using Characters.Operations;
using Scenes;
using UI;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Chimera
{
	// Token: 0x02001373 RID: 4979
	public class Intro : MonoBehaviour
	{
		// Token: 0x06006217 RID: 25111 RVA: 0x0011E574 File Offset: 0x0011C774
		private void Awake()
		{
			this._readyOperations.Initialize();
			this._landingOperations.Initialize();
			this._explosionOperations.Initialize();
			this._fallRocksOperations.Initialize();
			this._roarReadyOperations.Initialize();
			this._roarOperations.Initialize();
		}

		// Token: 0x06006218 RID: 25112 RVA: 0x0011E5C3 File Offset: 0x0011C7C3
		private void OnDestroy()
		{
			Scene<GameBase>.instance.cameraController.Zoom(1f, float.MaxValue);
		}

		// Token: 0x06006219 RID: 25113 RVA: 0x0011E5DE File Offset: 0x0011C7DE
		public void Ready(Character character)
		{
			this._readyOperations.gameObject.SetActive(true);
			this._readyOperations.Run(character);
		}

		// Token: 0x0600621A RID: 25114 RVA: 0x0011E5FD File Offset: 0x0011C7FD
		public void Landing(Character character, Chapter3Script script)
		{
			script.DisplayBossName();
			this._landingOperations.gameObject.SetActive(true);
			this._landingOperations.Run(character);
		}

		// Token: 0x0600621B RID: 25115 RVA: 0x0011E622 File Offset: 0x0011C822
		public void FallingRocks(Character character)
		{
			this._fallRocksOperations.gameObject.SetActive(true);
			this._fallRocksOperations.Run(character);
		}

		// Token: 0x0600621C RID: 25116 RVA: 0x0011E641 File Offset: 0x0011C841
		public void Explosion(Character character)
		{
			this._explosionOperations.gameObject.SetActive(true);
			this._explosionOperations.Run(character);
		}

		// Token: 0x0600621D RID: 25117 RVA: 0x0006160F File Offset: 0x0005F80F
		public void CameraZoomOut()
		{
			Scene<GameBase>.instance.cameraController.Zoom(1.3f, 1f);
		}

		// Token: 0x0600621E RID: 25118 RVA: 0x0011E660 File Offset: 0x0011C860
		public void RoarReady(Character character)
		{
			this._roarReadyOperations.gameObject.SetActive(true);
			this._roarReadyOperations.Run(character);
		}

		// Token: 0x0600621F RID: 25119 RVA: 0x0011E67F File Offset: 0x0011C87F
		public void Roar(Character character)
		{
			this._roarOperations.gameObject.SetActive(true);
			this._roarOperations.Run(character);
		}

		// Token: 0x06006220 RID: 25120 RVA: 0x000FBDED File Offset: 0x000F9FED
		public void LetterBoxOff()
		{
			Scene<GameBase>.instance.uiManager.letterBox.Disappear(0.4f);
		}

		// Token: 0x06006221 RID: 25121 RVA: 0x0011E69E File Offset: 0x0011C89E
		public void HealthBarOn(Character character)
		{
			Scene<GameBase>.instance.uiManager.headupDisplay.bossHealthBar.Open(BossHealthbarController.Type.Chapter3_Phase2, character);
		}

		// Token: 0x04004F1F RID: 20255
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		[Header("Ready")]
		private OperationInfos _readyOperations;

		// Token: 0x04004F20 RID: 20256
		[Header("Landing")]
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _landingOperations;

		// Token: 0x04004F21 RID: 20257
		[Header("FallRocks")]
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _fallRocksOperations;

		// Token: 0x04004F22 RID: 20258
		[Header("Explosion")]
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _explosionOperations;

		// Token: 0x04004F23 RID: 20259
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		[Header("RoarReady")]
		private OperationInfos _roarReadyOperations;

		// Token: 0x04004F24 RID: 20260
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		[Header("Roar")]
		private OperationInfos _roarOperations;
	}
}
