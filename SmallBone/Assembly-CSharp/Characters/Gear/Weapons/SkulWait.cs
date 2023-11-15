using System;
using System.Collections;
using Characters.Abilities.Constraints;
using Characters.Actions;
using Characters.Controllers;
using Characters.Movements;
using Unity.Mathematics;
using UnityEngine;

namespace Characters.Gear.Weapons
{
	// Token: 0x0200082C RID: 2092
	public class SkulWait : MonoBehaviour
	{
		// Token: 0x06002B43 RID: 11075 RVA: 0x00085159 File Offset: 0x00083359
		private void OnEnable()
		{
			base.StartCoroutine(this.CCheckWait());
		}

		// Token: 0x06002B44 RID: 11076 RVA: 0x00085168 File Offset: 0x00083368
		private IEnumerator CCheckWait()
		{
			yield return null;
			this._input = this._weapon.owner.GetComponent<PlayerInput>();
			float waitedTime = 0f;
			for (;;)
			{
				Movement movement = this._weapon.owner.movement;
				waitedTime += Chronometer.global.deltaTime;
				if (math.abs(movement.moved.x) > 0.0001f || math.abs(movement.moved.y) > 0.0001f)
				{
					waitedTime = 0f;
					yield return null;
				}
				else if (!this._constraints.Pass())
				{
					waitedTime = 0f;
					yield return null;
				}
				else
				{
					if (this.PressedAnyKey())
					{
						waitedTime = 0f;
					}
					if (waitedTime > 20f)
					{
						waitedTime = 0f;
						this._action.TryStart();
						yield return this.CWaitForWaitAction();
					}
					yield return null;
				}
			}
			yield break;
		}

		// Token: 0x06002B45 RID: 11077 RVA: 0x00085178 File Offset: 0x00083378
		private bool PressedAnyKey()
		{
			for (int i = 0; i < Button.count; i++)
			{
				if (this._input[i] != null && this._input[i].WasPressed)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002B46 RID: 11078 RVA: 0x000851B9 File Offset: 0x000833B9
		private IEnumerator CWaitForWaitAction()
		{
			Movement movement = this._weapon.owner.movement;
			while (this._action.running && this._constraints.Pass())
			{
				yield return null;
				if (this.PressedAnyKey() || math.abs(movement.moved.x) > 0.0001f || math.abs(movement.moved.y) > 0.0001f)
				{
					break;
				}
			}
			this._weapon.owner.CancelAction();
			yield break;
		}

		// Token: 0x040024CC RID: 9420
		private const float _waitingTime = 20f;

		// Token: 0x040024CD RID: 9421
		[SerializeField]
		private Weapon _weapon;

		// Token: 0x040024CE RID: 9422
		[SerializeField]
		private Characters.Actions.Action _action;

		// Token: 0x040024CF RID: 9423
		[SerializeField]
		[Constraint.SubcomponentAttribute]
		private Constraint.Subcomponents _constraints;

		// Token: 0x040024D0 RID: 9424
		private PlayerInput _input;
	}
}
