using System;
using Characters;
using Characters.Abilities;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x02000675 RID: 1653
	public class PowderKeg : MonoBehaviour
	{
		// Token: 0x06002115 RID: 8469 RVA: 0x00063BA0 File Offset: 0x00061DA0
		private void Awake()
		{
			this._operationsOnDie.Initialize();
			this._abilityAttacher.Initialize(this._character);
			this._abilityAttacher.StartAttach();
			this._character.health.onDie += this.Run;
		}

		// Token: 0x06002116 RID: 8470 RVA: 0x00063BF0 File Offset: 0x00061DF0
		private void OnDestroy()
		{
			this._abilityAttacher.StopAttach();
			this._character.health.onDie -= this.Run;
		}

		// Token: 0x06002117 RID: 8471 RVA: 0x00063C1C File Offset: 0x00061E1C
		private void Run()
		{
			this._character.health.onDie -= this.Run;
			this._particleEffectInfo.Emit(this._character.transform.position, this._character.collider.bounds, Vector2.up * 3f, true);
			if (MMMaths.RandomBool())
			{
				this._remain1.gameObject.SetActive(true);
			}
			else
			{
				this._remain2.gameObject.SetActive(true);
			}
			this._character.@base.gameObject.SetActive(false);
			base.StartCoroutine(this._operationsOnDie.CRun(this._character));
		}

		// Token: 0x04001C2E RID: 7214
		[SerializeField]
		private Character _character;

		// Token: 0x04001C2F RID: 7215
		[SerializeField]
		private GameObject _remain1;

		// Token: 0x04001C30 RID: 7216
		[SerializeField]
		private GameObject _remain2;

		// Token: 0x04001C31 RID: 7217
		[SerializeField]
		private ParticleEffectInfo _particleEffectInfo;

		// Token: 0x04001C32 RID: 7218
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _operationsOnDie;

		// Token: 0x04001C33 RID: 7219
		[AbilityAttacher.SubcomponentAttribute]
		[SerializeField]
		private AbilityAttacher _abilityAttacher;
	}
}
