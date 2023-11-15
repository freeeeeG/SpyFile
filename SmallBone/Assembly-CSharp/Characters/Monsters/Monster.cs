using System;
using UnityEngine;

namespace Characters.Monsters
{
	// Token: 0x02000812 RID: 2066
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Character))]
	public class Monster : MonoBehaviour
	{
		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x06002A80 RID: 10880 RVA: 0x00083204 File Offset: 0x00081404
		public PoolObject poolObject
		{
			get
			{
				return this._poolObject;
			}
		}

		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x06002A81 RID: 10881 RVA: 0x0008320C File Offset: 0x0008140C
		public Character character
		{
			get
			{
				return this._character;
			}
		}

		// Token: 0x14000073 RID: 115
		// (add) Token: 0x06002A82 RID: 10882 RVA: 0x00083214 File Offset: 0x00081414
		// (remove) Token: 0x06002A83 RID: 10883 RVA: 0x0008324C File Offset: 0x0008144C
		public event Monster.OnDespawnDelegate OnDespawn;

		// Token: 0x06002A84 RID: 10884 RVA: 0x00083281 File Offset: 0x00081481
		public Monster Summon(Vector3 position)
		{
			Monster component = this._poolObject.Spawn(true).GetComponent<Monster>();
			component.transform.position = position;
			component.character.health.Revive();
			return component;
		}

		// Token: 0x06002A85 RID: 10885 RVA: 0x000832B0 File Offset: 0x000814B0
		public void Despawn()
		{
			Monster.OnDespawnDelegate onDespawn = this.OnDespawn;
			if (onDespawn != null)
			{
				onDespawn();
			}
			this._poolObject.Despawn();
		}

		// Token: 0x0400243D RID: 9277
		[SerializeField]
		[GetComponent]
		private PoolObject _poolObject;

		// Token: 0x0400243E RID: 9278
		[SerializeField]
		[GetComponent]
		private Character _character;

		// Token: 0x02000813 RID: 2067
		// (Invoke) Token: 0x06002A88 RID: 10888
		public delegate void OnDespawnDelegate();
	}
}
