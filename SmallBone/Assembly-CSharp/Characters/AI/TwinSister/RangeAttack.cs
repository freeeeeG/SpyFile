using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Actions;
using Hardmode;
using Singletons;
using UnityEngine;

namespace Characters.AI.TwinSister
{
	// Token: 0x02001199 RID: 4505
	public class RangeAttack : MonoBehaviour
	{
		// Token: 0x06005877 RID: 22647 RVA: 0x00107C24 File Offset: 0x00105E24
		private Vector2 GetRightPosition(int minAngle, int maxAngle)
		{
			float angle = (float)UnityEngine.Random.Range(minAngle, maxAngle);
			return this.RotateVector(Vector2.right, angle) * this._distanceFromCenter;
		}

		// Token: 0x06005878 RID: 22648 RVA: 0x00107C54 File Offset: 0x00105E54
		private Vector2 GetLeftPosition(int minAngle, int maxAngle)
		{
			float num = (float)UnityEngine.Random.Range(minAngle, maxAngle);
			return this.RotateVector(Vector2.right, num + 90f) * this._distanceFromCenter;
		}

		// Token: 0x06005879 RID: 22649 RVA: 0x00107C88 File Offset: 0x00105E88
		private Vector2 RotateVector(Vector2 v, float angle)
		{
			float f = angle * 0.017453292f;
			float x = v.x * Mathf.Cos(f) - v.y * Mathf.Sin(f);
			float y = v.x * Mathf.Sin(f) + v.y * Mathf.Cos(f);
			return new Vector2(x, y);
		}

		// Token: 0x0600587A RID: 22650 RVA: 0x00107CDC File Offset: 0x00105EDC
		private void SetSpawnPosition()
		{
			bool flag = MMMaths.RandomBool();
			List<Vector2> list = new List<Vector2>(3);
			if (flag)
			{
				list.Add(this.GetLeftPosition(this._angleOfMeteorInAir.x, this._angleOfMeteorInAir.y));
				Vector2 rightPosition = this.GetRightPosition(this._angleOfMeteorInAir.x, this._angleOfMeteorInAir.y);
				float num = Mathf.Atan2(rightPosition.y, rightPosition.x) * 57.29578f;
				list.Add(rightPosition);
				if (num >= 45f)
				{
					list.Add(this.GetRightPosition(this._angleOfMeteorInAir.x, 40));
				}
				else
				{
					list.Add(this.GetRightPosition(50, this._angleOfMeteorInAir.y));
				}
			}
			else
			{
				list.Add(this.GetRightPosition(this._angleOfMeteorInAir.x, this._angleOfMeteorInAir.y));
				Vector2 leftPosition = this.GetLeftPosition(this._angleOfMeteorInAir.x, this._angleOfMeteorInAir.y);
				float num2 = Mathf.Atan2(leftPosition.y, leftPosition.x) * 57.29578f;
				list.Add(leftPosition);
				if (num2 >= 45f)
				{
					list.Add(this.GetLeftPosition(this._angleOfMeteorInAir.x, 40));
				}
				else
				{
					list.Add(this.GetLeftPosition(50, this._angleOfMeteorInAir.y));
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				this._attackPositions[i].position = list[i];
			}
		}

		// Token: 0x0600587B RID: 22651 RVA: 0x00107E5C File Offset: 0x0010605C
		private void SetHardmodeSpawnPosition()
		{
			int num = 36;
			int num2 = 0;
			int num3 = num;
			for (int i = 0; i < this._attackPositions.Length; i++)
			{
				this._attackPositions[i].position = this.GetRightPosition(num2, num3);
				num2 += num;
				num3 += num;
			}
		}

		// Token: 0x0600587C RID: 22652 RVA: 0x00107EA5 File Offset: 0x001060A5
		public IEnumerator CRun(AIController controller)
		{
			Character character = controller.character;
			if (Singleton<HardmodeManager>.Instance.hardmode)
			{
				this.SetHardmodeSpawnPosition();
			}
			else
			{
				this.SetSpawnPosition();
			}
			this._action.TryStart();
			while (this._action.running)
			{
				if (character.health.dead)
				{
					yield break;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x04004761 RID: 18273
		[SerializeField]
		private Transform[] _attackPositions;

		// Token: 0x04004762 RID: 18274
		[SerializeField]
		private Characters.Actions.Action _action;

		// Token: 0x04004763 RID: 18275
		[MinMaxSlider(15f, 90f)]
		[SerializeField]
		private Vector2Int _angleOfMeteorInAir;

		// Token: 0x04004764 RID: 18276
		[SerializeField]
		private float _distanceFromCenter = 6.5f;
	}
}
