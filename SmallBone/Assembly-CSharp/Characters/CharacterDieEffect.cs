using System;
using FX;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters
{
	// Token: 0x020006BC RID: 1724
	public class CharacterDieEffect : MonoBehaviour
	{
		// Token: 0x1700072F RID: 1839
		// (set) Token: 0x060022A5 RID: 8869 RVA: 0x000680C1 File Offset: 0x000662C1
		public EffectInfo effect
		{
			set
			{
				this._effect = value;
			}
		}

		// Token: 0x17000730 RID: 1840
		// (set) Token: 0x060022A6 RID: 8870 RVA: 0x000680CA File Offset: 0x000662CA
		public ParticleEffectInfo particleInfo
		{
			set
			{
				this._particleInfo = value;
			}
		}

		// Token: 0x060022A7 RID: 8871 RVA: 0x000680D3 File Offset: 0x000662D3
		protected virtual void Awake()
		{
			this._character.health.onDiedTryCatch += this.Spawn;
		}

		// Token: 0x060022A8 RID: 8872 RVA: 0x000680F4 File Offset: 0x000662F4
		protected void Spawn()
		{
			Vector3 position = (this._effectSpawnPoint == null) ? this._character.transform.position : this._effectSpawnPoint.position;
			if (this._effect != null)
			{
				this._effect.Spawn(position, 0f, 1f);
			}
			ParticleEffectInfo particleInfo = this._particleInfo;
			if (particleInfo != null)
			{
				particleInfo.Emit(this._character.transform.position, this._character.collider.bounds, this._character.movement.push);
			}
			if (this._vibrationDuration > 0f)
			{
				Singleton<Service>.Instance.controllerVibation.vibration.Attach(this, this._vibrationAmount, this._vibrationDuration);
			}
			if (this._sound != null)
			{
				PersistentSingleton<SoundManager>.Instance.PlaySound(this._sound, base.transform.position);
			}
			if (this._deactivateCharacter)
			{
				base.gameObject.SetActive(false);
				this._character.collider.enabled = false;
			}
			UnityEngine.Object.Destroy(this);
		}

		// Token: 0x060022A9 RID: 8873 RVA: 0x0006820F File Offset: 0x0006640F
		public void Detach()
		{
			this._character.health.onDiedTryCatch -= this.Spawn;
		}

		// Token: 0x060022AA RID: 8874 RVA: 0x0006820F File Offset: 0x0006640F
		private void OnDestroy()
		{
			this._character.health.onDiedTryCatch -= this.Spawn;
		}

		// Token: 0x04001D6F RID: 7535
		[SerializeField]
		[GetComponent]
		protected Character _character;

		// Token: 0x04001D70 RID: 7536
		[SerializeField]
		private Transform _effectSpawnPoint;

		// Token: 0x04001D71 RID: 7537
		[SerializeField]
		private EffectInfo _effect;

		// Token: 0x04001D72 RID: 7538
		[SerializeField]
		private ParticleEffectInfo _particleInfo;

		// Token: 0x04001D73 RID: 7539
		[SerializeField]
		private float _vibrationAmount;

		// Token: 0x04001D74 RID: 7540
		[SerializeField]
		private float _vibrationDuration;

		// Token: 0x04001D75 RID: 7541
		[SerializeField]
		private SoundInfo _sound;

		// Token: 0x04001D76 RID: 7542
		[FormerlySerializedAs("_destroyCharacter")]
		[SerializeField]
		private bool _deactivateCharacter = true;
	}
}
