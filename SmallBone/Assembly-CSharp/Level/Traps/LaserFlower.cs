using System;
using System.Collections;
using Characters;
using Characters.Actions;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x02000664 RID: 1636
	public class LaserFlower : MonoBehaviour
	{
		// Token: 0x060020D5 RID: 8405 RVA: 0x0006325C File Offset: 0x0006145C
		private void OnEnable()
		{
			this._attackRangeTransform.localScale = new Vector3(1f, (float)(this._laserSize + 1), 1f);
			this._effectBody.localScale = new Vector3(1f, (float)this._laserSize, 1f);
			Vector3 position = this._effectBody.position;
			Vector3 zero = Vector3.zero;
			float z = base.transform.localRotation.eulerAngles.z;
			if (z != 0f)
			{
				if (z != 180f)
				{
					if (z != 90f)
					{
						if (z == 270f)
						{
							zero = new Vector3(position.x + (float)this._laserSize, position.y, position.z);
						}
					}
					else
					{
						zero = new Vector3(position.x - (float)this._laserSize, position.y, position.z);
					}
				}
				else
				{
					zero = new Vector3(position.x, position.y - (float)this._laserSize, position.z);
				}
			}
			else
			{
				zero = new Vector3(position.x, position.y + (float)this._laserSize, position.z);
			}
			this._effectHead.position = zero;
		}

		// Token: 0x060020D6 RID: 8406 RVA: 0x00063393 File Offset: 0x00061593
		private void Awake()
		{
			base.StartCoroutine(this.CAttack());
		}

		// Token: 0x060020D7 RID: 8407 RVA: 0x000633A2 File Offset: 0x000615A2
		private IEnumerator CAttack()
		{
			yield return Chronometer.global.WaitForSeconds(this._startTiming * this._interval);
			for (;;)
			{
				this._attackAction.TryStart();
				yield return Chronometer.global.WaitForSeconds(this._interval);
				this._idleAction.TryStart();
			}
			yield break;
		}

		// Token: 0x04001BE2 RID: 7138
		[SerializeField]
		private Character _character;

		// Token: 0x04001BE3 RID: 7139
		[SerializeField]
		private GameObject _horizontalBody;

		// Token: 0x04001BE4 RID: 7140
		[SerializeField]
		private GameObject _verticalBody;

		// Token: 0x04001BE5 RID: 7141
		[SerializeField]
		[Range(0f, 1f)]
		private float _startTiming;

		// Token: 0x04001BE6 RID: 7142
		[SerializeField]
		private float _interval = 4f;

		// Token: 0x04001BE7 RID: 7143
		[SerializeField]
		private int _laserSize = 3;

		// Token: 0x04001BE8 RID: 7144
		[SerializeField]
		private Characters.Actions.Action _attackAction;

		// Token: 0x04001BE9 RID: 7145
		[SerializeField]
		private Characters.Actions.Action _idleAction;

		// Token: 0x04001BEA RID: 7146
		[SerializeField]
		private Transform _attackRangeTransform;

		// Token: 0x04001BEB RID: 7147
		[SerializeField]
		private Transform _effectHead;

		// Token: 0x04001BEC RID: 7148
		[SerializeField]
		private Transform _effectBody;

		// Token: 0x04001BED RID: 7149
		private LaserFlower.FireDirection _fireDirection;

		// Token: 0x02000665 RID: 1637
		private enum FireDirection
		{
			// Token: 0x04001BEF RID: 7151
			Up,
			// Token: 0x04001BF0 RID: 7152
			Down,
			// Token: 0x04001BF1 RID: 7153
			Right,
			// Token: 0x04001BF2 RID: 7154
			Left
		}
	}
}
