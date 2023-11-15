using System;
using System.Collections;
using Characters;
using Characters.Abilities.Weapons.Wizard;
using Characters.Actions;
using FX;
using Services;
using Singletons;
using UnityEngine;

namespace Level
{
	// Token: 0x020004BE RID: 1214
	public class DroppedManatechPart : MonoBehaviour, IPickupable
	{
		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x0600177F RID: 6015 RVA: 0x00049D7B File Offset: 0x00047F7B
		public PoolObject poolObject
		{
			get
			{
				return this._poolObject;
			}
		}

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06001780 RID: 6016 RVA: 0x00049D83 File Offset: 0x00047F83
		// (set) Token: 0x06001781 RID: 6017 RVA: 0x00049D8B File Offset: 0x00047F8B
		public float cooldownReducingAmount { get; set; }

		// Token: 0x06001782 RID: 6018 RVA: 0x00049D94 File Offset: 0x00047F94
		private void Awake()
		{
			this._pickupTrigger.enabled = false;
			this._pickupTrigger.gameObject.AddComponent<DroppedManatechPart.PickupProxy>().part = this;
		}

		// Token: 0x06001783 RID: 6019 RVA: 0x00049DB8 File Offset: 0x00047FB8
		private void OnEnable()
		{
			this._rigidbody.gravityScale = 3f;
			float num = UnityEngine.Random.Range(this._xPowerRange.x, this._xPowerRange.y);
			if (MMMaths.RandomBool())
			{
				num *= -1f;
			}
			this._rigidbody.velocity = new Vector2(num, UnityEngine.Random.Range(this._yPowerRange.x, this._yPowerRange.y));
			this._rigidbody.AddTorque((float)(UnityEngine.Random.Range(0, 20) * (MMMaths.RandomBool() ? 1 : -1)));
			this._player = Singleton<Service>.Instance.levelManager.player;
			base.StartCoroutine(this.CUpdatePickupable());
		}

		// Token: 0x06001784 RID: 6020 RVA: 0x00049E6E File Offset: 0x0004806E
		private IEnumerator CUpdatePickupable()
		{
			this._pickupTrigger.enabled = false;
			yield return Chronometer.global.WaitForSeconds(0.5f);
			this._pickupTrigger.enabled = true;
			yield break;
		}

		// Token: 0x06001785 RID: 6021 RVA: 0x00049E80 File Offset: 0x00048080
		public void PickedUpBy(Character character)
		{
			foreach (Characters.Actions.Action action in this._player.actions)
			{
				if (action.type == Characters.Actions.Action.Type.Skill && action.cooldown.time != null)
				{
					action.cooldown.time.ReduceCooldown(this.cooldownReducingAmount);
				}
			}
			if (character != null)
			{
				WizardPassive.Instance instanceByInstanceType = character.ability.GetInstanceByInstanceType<WizardPassive.Instance>();
				if (instanceByInstanceType != null)
				{
					instanceByInstanceType.AddGauge(this._wizardPassiveGauge);
				}
			}
			this._effect.Spawn(base.transform.position, 0f, 1f);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._sound, base.transform.position);
			this._poolObject.Despawn();
		}

		// Token: 0x0400148A RID: 5258
		[SerializeField]
		[MinMaxSlider(0f, 100f)]
		private Vector2 _xPowerRange;

		// Token: 0x0400148B RID: 5259
		[MinMaxSlider(0f, 100f)]
		[SerializeField]
		private Vector2 _yPowerRange;

		// Token: 0x0400148C RID: 5260
		[SerializeField]
		private Collider2D _pickupTrigger;

		// Token: 0x0400148D RID: 5261
		[SerializeField]
		private EffectInfo _effect;

		// Token: 0x0400148E RID: 5262
		[SerializeField]
		private SoundInfo _sound;

		// Token: 0x0400148F RID: 5263
		[SerializeField]
		[GetComponent]
		private PoolObject _poolObject;

		// Token: 0x04001490 RID: 5264
		[SerializeField]
		[GetComponent]
		private Rigidbody2D _rigidbody;

		// Token: 0x04001491 RID: 5265
		private Character _player;

		// Token: 0x04001492 RID: 5266
		[Header("Custom")]
		[SerializeField]
		private float _wizardPassiveGauge;

		// Token: 0x020004BF RID: 1215
		private class PickupProxy : MonoBehaviour, IPickupable
		{
			// Token: 0x06001787 RID: 6023 RVA: 0x00049F6C File Offset: 0x0004816C
			public void PickedUpBy(Character character)
			{
				this.part.PickedUpBy(character);
			}

			// Token: 0x04001494 RID: 5268
			public DroppedManatechPart part;
		}
	}
}
