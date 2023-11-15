using System;
using System.Collections;
using System.Linq;
using Characters.Movements;
using UnityEngine;
using Utils;

namespace Characters.Operations.Movement
{
	// Token: 0x02000E6E RID: 3694
	public sealed class StartToAddGrabTarget : CharacterOperation
	{
		// Token: 0x06004949 RID: 18761 RVA: 0x000D5F9C File Offset: 0x000D419C
		public override void Run(Character owner)
		{
			this._owner = owner;
			owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.OnOwnerGaveDamage));
			if (this._duration > 0f)
			{
				base.StartCoroutine(this.WaitForDuration());
			}
		}

		// Token: 0x0600494A RID: 18762 RVA: 0x000D5FEC File Offset: 0x000D41EC
		private IEnumerator WaitForDuration()
		{
			yield return Chronometer.global.WaitForSeconds(this._duration);
			Character owner = this._owner;
			owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.OnOwnerGaveDamage));
			yield break;
		}

		// Token: 0x0600494B RID: 18763 RVA: 0x000D5FFC File Offset: 0x000D41FC
		private void OnOwnerGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			if (target == null)
			{
				return;
			}
			if (target.character == null)
			{
				return;
			}
			if (target.character.health.dead)
			{
				return;
			}
			Damage gaveDamageTemp = gaveDamage;
			if (!this._attackKeys.Any((string key) => gaveDamageTemp.key.Equals(key, StringComparison.OrdinalIgnoreCase)))
			{
				return;
			}
			Target target2 = target as Target;
			if (target2 == null)
			{
				return;
			}
			if (target.character.movement.config.type == Movement.Config.Type.Static)
			{
				this._grabBoard.AddFailed(target2);
				return;
			}
			if (target.character.stat.GetFinal(Stat.Kind.KnockbackResistance) == 0.0)
			{
				this._grabBoard.AddFailed(target2);
				return;
			}
			this._chronoToGlobe.ApplyGlobe();
			this._chronoToOwner.ApplyTo(this._owner);
			this._chronoToTarget.ApplyTo(target.character);
			this._grabBoard.Add(target2);
		}

		// Token: 0x0600494C RID: 18764 RVA: 0x000D60F6 File Offset: 0x000D42F6
		public override void Stop()
		{
			base.Stop();
			if (this._owner == null)
			{
				return;
			}
			Character owner = this._owner;
			owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.OnOwnerGaveDamage));
		}

		// Token: 0x0400387E RID: 14462
		[SerializeField]
		private GrabBoard _grabBoard;

		// Token: 0x0400387F RID: 14463
		[SerializeField]
		private float _duration;

		// Token: 0x04003880 RID: 14464
		[SerializeField]
		private string[] _attackKeys = new string[]
		{
			"grab"
		};

		// Token: 0x04003881 RID: 14465
		[SerializeField]
		private ChronoInfo _chronoToGlobe;

		// Token: 0x04003882 RID: 14466
		[SerializeField]
		private ChronoInfo _chronoToOwner;

		// Token: 0x04003883 RID: 14467
		[SerializeField]
		private ChronoInfo _chronoToTarget;

		// Token: 0x04003884 RID: 14468
		private Character _owner;
	}
}
