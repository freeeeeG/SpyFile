using System;
using Characters.Player;
using InControl;
using UnityEngine;
using UserInput;

namespace Characters.Controllers
{
	// Token: 0x02000919 RID: 2329
	public sealed class PlayerInput : MonoBehaviour
	{
		// Token: 0x17000AAF RID: 2735
		// (get) Token: 0x060031DD RID: 12765 RVA: 0x00094341 File Offset: 0x00092541
		// (set) Token: 0x060031DE RID: 12766 RVA: 0x00094354 File Offset: 0x00092554
		public PlayerAction attack
		{
			get
			{
				return this._map[Button.Attack.index];
			}
			private set
			{
				this._map[Button.Attack.index] = value;
			}
		}

		// Token: 0x17000AB0 RID: 2736
		// (get) Token: 0x060031DF RID: 12767 RVA: 0x00094368 File Offset: 0x00092568
		// (set) Token: 0x060031E0 RID: 12768 RVA: 0x0009437B File Offset: 0x0009257B
		public PlayerAction dash
		{
			get
			{
				return this._map[Button.Dash.index];
			}
			private set
			{
				this._map[Button.Dash.index] = value;
			}
		}

		// Token: 0x17000AB1 RID: 2737
		// (get) Token: 0x060031E1 RID: 12769 RVA: 0x0009438F File Offset: 0x0009258F
		// (set) Token: 0x060031E2 RID: 12770 RVA: 0x000943A2 File Offset: 0x000925A2
		public PlayerAction jump
		{
			get
			{
				return this._map[Button.Jump.index];
			}
			private set
			{
				this._map[Button.Jump.index] = value;
			}
		}

		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x060031E3 RID: 12771 RVA: 0x000943B6 File Offset: 0x000925B6
		// (set) Token: 0x060031E4 RID: 12772 RVA: 0x000943C9 File Offset: 0x000925C9
		public PlayerAction skill
		{
			get
			{
				return this._map[Button.Skill.index];
			}
			private set
			{
				this._map[Button.Skill.index] = value;
			}
		}

		// Token: 0x17000AB3 RID: 2739
		// (get) Token: 0x060031E5 RID: 12773 RVA: 0x000943DD File Offset: 0x000925DD
		// (set) Token: 0x060031E6 RID: 12774 RVA: 0x000943F0 File Offset: 0x000925F0
		public PlayerAction skill2
		{
			get
			{
				return this._map[Button.Skill2.index];
			}
			private set
			{
				this._map[Button.Skill2.index] = value;
			}
		}

		// Token: 0x17000AB4 RID: 2740
		// (get) Token: 0x060031E7 RID: 12775 RVA: 0x00094404 File Offset: 0x00092604
		// (set) Token: 0x060031E8 RID: 12776 RVA: 0x00094417 File Offset: 0x00092617
		public PlayerAction useItem
		{
			get
			{
				return this._map[Button.UseItem.index];
			}
			private set
			{
				this._map[Button.UseItem.index] = value;
			}
		}

		// Token: 0x17000AB5 RID: 2741
		// (get) Token: 0x060031E9 RID: 12777 RVA: 0x0009442B File Offset: 0x0009262B
		// (set) Token: 0x060031EA RID: 12778 RVA: 0x0009443E File Offset: 0x0009263E
		public PlayerAction notUsed
		{
			get
			{
				return this._map[Button.None.index];
			}
			private set
			{
				this._map[Button.None.index] = value;
			}
		}

		// Token: 0x17000AB6 RID: 2742
		// (get) Token: 0x060031EB RID: 12779 RVA: 0x00094452 File Offset: 0x00092652
		// (set) Token: 0x060031EC RID: 12780 RVA: 0x0009445A File Offset: 0x0009265A
		public PlayerAction interaction { get; private set; }

		// Token: 0x17000AB7 RID: 2743
		// (get) Token: 0x060031ED RID: 12781 RVA: 0x00094463 File Offset: 0x00092663
		// (set) Token: 0x060031EE RID: 12782 RVA: 0x0009446B File Offset: 0x0009266B
		public PlayerAction swap { get; private set; }

		// Token: 0x17000AB8 RID: 2744
		// (get) Token: 0x060031EF RID: 12783 RVA: 0x00094474 File Offset: 0x00092674
		// (set) Token: 0x060031F0 RID: 12784 RVA: 0x0009447C File Offset: 0x0009267C
		public PlayerAction left { get; private set; }

		// Token: 0x17000AB9 RID: 2745
		// (get) Token: 0x060031F1 RID: 12785 RVA: 0x00094485 File Offset: 0x00092685
		// (set) Token: 0x060031F2 RID: 12786 RVA: 0x0009448D File Offset: 0x0009268D
		public PlayerAction right { get; private set; }

		// Token: 0x17000ABA RID: 2746
		// (get) Token: 0x060031F3 RID: 12787 RVA: 0x00094496 File Offset: 0x00092696
		// (set) Token: 0x060031F4 RID: 12788 RVA: 0x0009449E File Offset: 0x0009269E
		public PlayerAction up { get; private set; }

		// Token: 0x17000ABB RID: 2747
		// (get) Token: 0x060031F5 RID: 12789 RVA: 0x000944A7 File Offset: 0x000926A7
		// (set) Token: 0x060031F6 RID: 12790 RVA: 0x000944AF File Offset: 0x000926AF
		public PlayerAction down { get; private set; }

		// Token: 0x17000ABC RID: 2748
		public PlayerAction this[int index]
		{
			get
			{
				return this._map[index];
			}
		}

		// Token: 0x17000ABD RID: 2749
		public PlayerAction this[Button button]
		{
			get
			{
				return this._map[button.index];
			}
		}

		// Token: 0x060031F9 RID: 12793 RVA: 0x000944D4 File Offset: 0x000926D4
		private void Awake()
		{
			this.attack = KeyMapper.Map.Attack;
			this.dash = KeyMapper.Map.Dash;
			this.jump = KeyMapper.Map.Jump;
			this.skill = KeyMapper.Map.Skill1;
			this.skill2 = KeyMapper.Map.Skill2;
			this.interaction = KeyMapper.Map.Interaction;
			this.swap = KeyMapper.Map.Swap;
			this.useItem = KeyMapper.Map.Quintessence;
			this.left = KeyMapper.Map.Left;
			this.right = KeyMapper.Map.Right;
			this.up = KeyMapper.Map.Up;
			this.down = KeyMapper.Map.Down;
			this._weaponInventory = base.GetComponent<WeaponInventory>();
			this._quintessenceInventory = base.GetComponent<QuintessenceInventory>();
			this._characterInteraction = base.GetComponent<CharacterInteraction>();
		}

		// Token: 0x060031FA RID: 12794 RVA: 0x000945C8 File Offset: 0x000927C8
		private void Update()
		{
			if (PlayerInput.blocked.value)
			{
				return;
			}
			this.direction = KeyMapper.Map.Move.Vector;
			if (this.direction.x > 0.33f)
			{
				if (PlayerInput.reverseHorizontal.value)
				{
					this._character.movement.MoveHorizontal(Vector2.left);
				}
				else
				{
					this._character.movement.MoveHorizontal(Vector2.right);
				}
			}
			if (this.direction.x < -0.33f)
			{
				if (PlayerInput.reverseHorizontal.value)
				{
					this._character.movement.MoveHorizontal(Vector2.right);
				}
				else
				{
					this._character.movement.MoveHorizontal(Vector2.left);
				}
			}
			if (this.direction.y > 0.33f)
			{
				this._character.movement.MoveVertical(Vector2.up);
			}
			if (this.direction.y < -0.33f)
			{
				this._character.movement.MoveVertical(Vector2.down);
			}
			for (int i = 0; i < this._character.actions.Count; i++)
			{
				if (this._character.actions[i].Process())
				{
					return;
				}
			}
			if (this.swap.WasPressed && !this._character.silence.value)
			{
				this._weaponInventory.NextWeapon(false);
				return;
			}
			if (this.useItem.WasPressed)
			{
				this._quintessenceInventory.UseAt(0);
				return;
			}
			if (this.interaction.WasPressed)
			{
				this._characterInteraction.InteractionKeyWasPressed();
			}
			if (this.interaction.WasReleased)
			{
				this._characterInteraction.InteractionKeyWasReleased();
			}
		}

		// Token: 0x040028D5 RID: 10453
		public static readonly TrueOnlyLogicalSumList blocked = new TrueOnlyLogicalSumList(false);

		// Token: 0x040028D6 RID: 10454
		public static readonly TrueOnlyLogicalSumList reverseHorizontal = new TrueOnlyLogicalSumList(false);

		// Token: 0x040028D7 RID: 10455
		public readonly PlayerAction[] _map = new PlayerAction[Button.count];

		// Token: 0x040028D8 RID: 10456
		[GetComponent]
		[SerializeField]
		private Character _character;

		// Token: 0x040028D9 RID: 10457
		private WeaponInventory _weaponInventory;

		// Token: 0x040028DA RID: 10458
		private QuintessenceInventory _quintessenceInventory;

		// Token: 0x040028DB RID: 10459
		private CharacterInteraction _characterInteraction;

		// Token: 0x040028DC RID: 10460
		public Vector2 direction;
	}
}
