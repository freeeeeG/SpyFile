using System;
using System.Collections;
using Characters;
using Characters.Gear.Weapons;
using Services;
using Singletons;
using UnityEngine;

namespace Level
{
	// Token: 0x020004D1 RID: 1233
	public class DroppedSkulHead : MonoBehaviour
	{
		// Token: 0x06001801 RID: 6145 RVA: 0x0004B678 File Offset: 0x00049878
		private void OnEnable()
		{
			this._rigidbody.gravityScale = 3f;
			this._rigidbody.velocity = new Vector2(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(5f, 15f));
			this._rigidbody.AddTorque((float)(UnityEngine.Random.Range(0, 15) * (MMMaths.RandomBool() ? 1 : -1)));
			this._player = Singleton<Service>.Instance.levelManager.player;
			base.StartCoroutine(this.CUpdate());
			foreach (Weapon weapon in this._player.playerComponents.inventory.weapon.weapons)
			{
				if (!(weapon == null) && (weapon.name.Equals("Skul", StringComparison.OrdinalIgnoreCase) || weapon.name.Equals("HeroSkul", StringComparison.OrdinalIgnoreCase)))
				{
					this._skulHeadController = weapon.equipped.GetComponent<SkulHeadController>();
					return;
				}
			}
		}

		// Token: 0x06001802 RID: 6146 RVA: 0x0004B773 File Offset: 0x00049973
		private IEnumerator CUpdate()
		{
			float time = 0f;
			yield return Chronometer.global.WaitForSeconds(0.5f);
			while (!(this._skulHeadController == null) && this._skulHeadController.cooldown.stacks <= 0)
			{
				Vector3 center = Singleton<Service>.Instance.levelManager.player.collider.bounds.center;
				Vector2 vector = new Vector2(base.transform.position.x - center.x, base.transform.position.y - center.y);
				float sqrMagnitude = vector.sqrMagnitude;
				time += Chronometer.global.deltaTime;
				if (time >= 0.5f && sqrMagnitude < 1f)
				{
					this._skulHeadController.cooldown.time.remainTime = 0f;
					this._poolObject.Despawn();
				}
				yield return null;
			}
			this._poolObject.Despawn();
			yield break;
			yield break;
		}

		// Token: 0x040014F3 RID: 5363
		private const float _lootDistance = 1f;

		// Token: 0x040014F4 RID: 5364
		private const float _sqrLootDistance = 1f;

		// Token: 0x040014F5 RID: 5365
		[GetComponent]
		[SerializeField]
		private PoolObject _poolObject;

		// Token: 0x040014F6 RID: 5366
		[GetComponent]
		[SerializeField]
		private Rigidbody2D _rigidbody;

		// Token: 0x040014F7 RID: 5367
		private Character _player;

		// Token: 0x040014F8 RID: 5368
		private SkulHeadController _skulHeadController;
	}
}
