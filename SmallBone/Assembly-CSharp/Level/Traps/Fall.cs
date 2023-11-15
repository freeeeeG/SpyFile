using System;
using Characters;
using Characters.Operations;
using Services;
using Singletons;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x0200065D RID: 1629
	[RequireComponent(typeof(Character))]
	public class Fall : Trap
	{
		// Token: 0x060020B6 RID: 8374 RVA: 0x00062C41 File Offset: 0x00060E41
		private void Awake()
		{
			this._onEscape.Initialize();
			this._attackDamage = base.GetComponent<IAttackDamage>();
			this._stun = new CharacterStatus.ApplyInfo(CharacterStatus.Kind.Stun);
		}

		// Token: 0x060020B7 RID: 8375 RVA: 0x00062C68 File Offset: 0x00060E68
		private void OnTriggerEnter2D(Collider2D collision)
		{
			Character component = collision.GetComponent<Character>();
			if (component == null)
			{
				return;
			}
			switch (component.type)
			{
			case Character.Type.TrashMob:
			case Character.Type.Summoned:
				component.health.Kill();
				return;
			case Character.Type.Named:
			{
				double @base = component.health.maximumHealth * (double)this._darkenemyDamagePercent * 0.009999999776482582;
				Damage damage = new Damage(this._character, @base, component.transform.position, Damage.Attribute.Fixed, Damage.AttackType.Additional, Damage.MotionType.Basic, 1.0, 0f, 0.0, 1.0, 1.0, true, false, 0.0, 0.0, 0, null, 1.0);
				this._character.GiveStatus(component, this._stun);
				component.health.TakeDamage(ref damage);
				component.transform.position = this._escapePoint.position;
				break;
			}
			case Character.Type.Adventurer:
			case Character.Type.Boss:
			case Character.Type.Trap:
			case Character.Type.Dummy:
				break;
			case Character.Type.Player:
			{
				float num = (this._damage == 0.0) ? 0f : this._attackDamage.amount;
				Damage damage2 = new Damage(this._character, (double)num, component.transform.position, Damage.Attribute.Fixed, Damage.AttackType.Additional, Damage.MotionType.Basic, 1.0, 0f, 0.0, 1.0, 1.0, true, false, 0.0, 0.0, 0, null, 1.0);
				if (Math.Floor(component.health.currentHealth) <= this._damage)
				{
					damage2.@base = component.health.currentHealth - 1.0;
				}
				component.health.TakeDamage(ref damage2);
				component.transform.position = this._escapePoint.position;
				Singleton<Service>.Instance.floatingTextSpawner.SpawnPlayerTakingDamage(damage2.amount, component.transform.position);
				this._onEscape.Run(component);
				return;
			}
			case Character.Type.PlayerMinion:
				component.transform.position = this._escapePoint.position;
				return;
			default:
				return;
			}
		}

		// Token: 0x04001BC1 RID: 7105
		[GetComponent]
		[SerializeField]
		private Character _character;

		// Token: 0x04001BC2 RID: 7106
		[Tooltip("이미 설정된 값으로, 참조용으로 사용중")]
		[SerializeField]
		private double _damage;

		// Token: 0x04001BC3 RID: 7107
		[Range(0f, 100f)]
		[SerializeField]
		private int _darkenemyDamagePercent;

		// Token: 0x04001BC4 RID: 7108
		[SerializeField]
		private Transform _escapePoint;

		// Token: 0x04001BC5 RID: 7109
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation.Subcomponents _onEscape;

		// Token: 0x04001BC6 RID: 7110
		private IAttackDamage _attackDamage;

		// Token: 0x04001BC7 RID: 7111
		private CharacterStatus.ApplyInfo _stun;
	}
}
