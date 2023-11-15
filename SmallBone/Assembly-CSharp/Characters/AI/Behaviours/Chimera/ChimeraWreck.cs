using System;
using Characters.Operations;
using Level;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Chimera
{
	// Token: 0x02001372 RID: 4978
	public class ChimeraWreck : MonoBehaviour
	{
		// Token: 0x06006213 RID: 25107 RVA: 0x0011E4DA File Offset: 0x0011C6DA
		private void Awake()
		{
			this._operationInfos.Initialize();
		}

		// Token: 0x06006214 RID: 25108 RVA: 0x0011E4E7 File Offset: 0x0011C6E7
		private void Update()
		{
			if (Map.Instance.waveContainer.enemyWaves[0].remains <= 0)
			{
				this.DestroyProp(Singleton<Service>.Instance.levelManager.player);
			}
		}

		// Token: 0x06006215 RID: 25109 RVA: 0x0011E518 File Offset: 0x0011C718
		public void DestroyProp(Character chimera)
		{
			this._particle.Emit(this._emitPosition.position, this._range.bounds, Vector2.up * 2f, true);
			this._operationInfos.Run(chimera);
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x04004F1B RID: 20251
		[SerializeField]
		private ParticleEffectInfo _particle;

		// Token: 0x04004F1C RID: 20252
		[SerializeField]
		private Transform _emitPosition;

		// Token: 0x04004F1D RID: 20253
		[SerializeField]
		private Collider2D _range;

		// Token: 0x04004F1E RID: 20254
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _operationInfos;
	}
}
