using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Characters.Abilities.Constraints;
using Characters.Actions;
using Characters.Gear.Weapons;
using Characters.Movements;
using Characters.Player;
using PhysicsUtils;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x02000685 RID: 1669
	[ExecuteAlways]
	public class TransportPlane : ControlableTrap
	{
		// Token: 0x06002162 RID: 8546 RVA: 0x000646B0 File Offset: 0x000628B0
		private void SetSize()
		{
			Vector2 size = this._spriteRenderer.size;
			size.x = (float)(this._size + 1);
			this._spriteRenderer.size = size;
			Vector2 size2 = this._collider.size;
			size2.x = (float)(this._size + 1) - 1.5f;
			this._collider.size = size2;
		}

		// Token: 0x06002163 RID: 8547 RVA: 0x00002191 File Offset: 0x00000391
		private void SetSpeed()
		{
		}

		// Token: 0x06002164 RID: 8548 RVA: 0x00064714 File Offset: 0x00062914
		private void Awake()
		{
			this._weaponNamesToExclude = (from weapon in this._weaponsToExclude
			select weapon.name).ToArray<string>();
			this._coroutine = this.CRun();
			this.SetSize();
			this.SetSpeed();
		}

		// Token: 0x06002165 RID: 8549 RVA: 0x00002191 File Offset: 0x00000391
		private void Update()
		{
		}

		// Token: 0x06002166 RID: 8550 RVA: 0x0006476E File Offset: 0x0006296E
		public override void Activate()
		{
			this._action.TryStart();
			base.StartCoroutine(this._coroutine);
		}

		// Token: 0x06002167 RID: 8551 RVA: 0x00064789 File Offset: 0x00062989
		public override void Deactivate()
		{
			if (this._action.running)
			{
				this._character.CancelAction();
			}
			base.StopCoroutine(this._coroutine);
		}

		// Token: 0x06002168 RID: 8552 RVA: 0x000647AF File Offset: 0x000629AF
		private IEnumerator CRun()
		{
			for (;;)
			{
				yield return null;
				if (this._constraints.Pass())
				{
					using (List<Character>.Enumerator enumerator = this.targets.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Character character = enumerator.Current;
							if (character == null || !character.liveAndActive)
							{
								this.targets.Remove(character);
							}
							else
							{
								Movement movement = character.movement;
								movement.force.x = movement.force.x + (float)((this._character.lookingDirection == Character.LookingDirection.Right) ? 1 : -1) * this._speed * this._character.chronometer.master.deltaTime;
							}
						}
						continue;
					}
					yield break;
				}
			}
		}

		// Token: 0x06002169 RID: 8553 RVA: 0x000647BE File Offset: 0x000629BE
		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!this._layer.Contains(other.gameObject.layer))
			{
				return;
			}
			this.AddTarget(other.gameObject);
		}

		// Token: 0x0600216A RID: 8554 RVA: 0x000647E5 File Offset: 0x000629E5
		private void OnTriggerExit2D(Collider2D other)
		{
			if (!this._layer.Contains(other.gameObject.layer))
			{
				return;
			}
			this.RemoveTarget(other.gameObject);
		}

		// Token: 0x0600216B RID: 8555 RVA: 0x0006480C File Offset: 0x00062A0C
		private void AddTarget(GameObject target)
		{
			Character character;
			if (!target.TryFindCharacterComponent(out character))
			{
				return;
			}
			PlayerComponents playerComponents = character.playerComponents;
			if (playerComponents != null && this._weaponNamesToExclude.Any((string name) => name.Equals(playerComponents.inventory.weapon.polymorphOrCurrent.name, StringComparison.OrdinalIgnoreCase)))
			{
				return;
			}
			this.targets.Add(character);
		}

		// Token: 0x0600216C RID: 8556 RVA: 0x00064864 File Offset: 0x00062A64
		private void RemoveTarget(GameObject target)
		{
			Character item;
			if (!target.TryFindCharacterComponent(out item))
			{
				return;
			}
			if (!this.targets.Contains(item))
			{
				return;
			}
			this.targets.Remove(item);
		}

		// Token: 0x04001C72 RID: 7282
		[SerializeField]
		[GetComponent]
		private SpriteRenderer _spriteRenderer;

		// Token: 0x04001C73 RID: 7283
		[SerializeField]
		private BoxCollider2D _collider;

		// Token: 0x04001C74 RID: 7284
		[SerializeField]
		private Character _character;

		// Token: 0x04001C75 RID: 7285
		[SerializeField]
		private Characters.Actions.Action _action;

		// Token: 0x04001C76 RID: 7286
		[SerializeField]
		private int _size = 2;

		// Token: 0x04001C77 RID: 7287
		[SerializeField]
		private float _speed;

		// Token: 0x04001C78 RID: 7288
		[SerializeField]
		private float _castDistance;

		// Token: 0x04001C79 RID: 7289
		[SerializeField]
		private LayerMask _layer;

		// Token: 0x04001C7A RID: 7290
		private List<Character> targets = new List<Character>();

		// Token: 0x04001C7B RID: 7291
		private IEnumerator _coroutine;

		// Token: 0x04001C7C RID: 7292
		private static readonly NonAllocCaster _caster = new NonAllocCaster(15);

		// Token: 0x04001C7D RID: 7293
		[Constraint.SubcomponentAttribute]
		[SerializeField]
		private Constraint[] _constraints;

		// Token: 0x04001C7E RID: 7294
		[SerializeField]
		[Space]
		private Weapon[] _weaponsToExclude;

		// Token: 0x04001C7F RID: 7295
		private string[] _weaponNamesToExclude;
	}
}
