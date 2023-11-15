using System;
using BehaviorDesigner.Runtime;
using Services;
using Singletons;
using UnityEngine;

namespace Characters
{
	// Token: 0x020006A2 RID: 1698
	public class BDEnemyCharacterSpecificator : MonoBehaviour
	{
		// Token: 0x060021E8 RID: 8680 RVA: 0x00065B9C File Offset: 0x00063D9C
		private void Awake()
		{
			this._bdCommunicator = this._character.GetComponentInChildren<BehaviorDesignerCommunicator>();
			this._statValue = new Stat.Values(new Stat.Value[]
			{
				new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.MovementSpeed, 0.0),
				new Stat.Value(Stat.Category.Constant, Stat.Kind.MovementSpeed, (double)UnityEngine.Random.Range(this._movementSpeedRange.x, this._movementSpeedRange.y)),
				new Stat.Value(Stat.Category.Percent, Stat.Kind.Health, (double)Singleton<Service>.Instance.levelManager.currentChapter.currentStage.healthMultiplier)
			});
			this._character.stat.AttachValues(this._statValue);
		}

		// Token: 0x060021E9 RID: 8681 RVA: 0x00065C58 File Offset: 0x00063E58
		private void Update()
		{
			if (this._bdCommunicator == null)
			{
				return;
			}
			if (this._bdCommunicator.GetVariable<SharedCharacter>(this._targetName).Value == null)
			{
				if (this._movementSpeedAttached)
				{
					this._statValue.values[0].value = 0.0;
					this._movementSpeedAttached = false;
					this._character.stat.SetNeedUpdate();
					return;
				}
			}
			else if (!this._movementSpeedAttached)
			{
				this._statValue.values[0].value = (double)this._speedBonusAtChaseTarget;
				this._movementSpeedAttached = true;
				this._character.stat.SetNeedUpdate();
			}
		}

		// Token: 0x04001CDC RID: 7388
		[SerializeField]
		private Character _character;

		// Token: 0x04001CDD RID: 7389
		[SerializeField]
		private Vector2 _movementSpeedRange = new Vector2(-0.2f, 0.2f);

		// Token: 0x04001CDE RID: 7390
		[SerializeField]
		private float _speedBonusAtChaseTarget = 0.3f;

		// Token: 0x04001CDF RID: 7391
		[SerializeField]
		private string _targetName = "Target";

		// Token: 0x04001CE0 RID: 7392
		private BehaviorDesignerCommunicator _bdCommunicator;

		// Token: 0x04001CE1 RID: 7393
		private Stat.Values _statValue;

		// Token: 0x04001CE2 RID: 7394
		private bool _movementSpeedAttached;
	}
}
