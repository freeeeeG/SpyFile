using System;
using Characters.AI;
using Services;
using Singletons;
using UnityEngine;

namespace Characters
{
	// Token: 0x020006F7 RID: 1783
	public class EnemyCharacterSpecificator : MonoBehaviour
	{
		// Token: 0x060023FD RID: 9213 RVA: 0x0006C320 File Offset: 0x0006A520
		private void Awake()
		{
			this._aiController = this._character.GetComponentInChildren<AIController>();
			this._statValue = new Stat.Values(new Stat.Value[]
			{
				new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.MovementSpeed, 0.0),
				new Stat.Value(Stat.Category.Constant, Stat.Kind.MovementSpeed, (double)UnityEngine.Random.Range(this._movementSpeedRange.x, this._movementSpeedRange.y)),
				new Stat.Value(Stat.Category.Percent, Stat.Kind.Health, (double)Singleton<Service>.Instance.levelManager.currentChapter.currentStage.healthMultiplier)
			});
			this._character.stat.AttachValues(this._statValue);
		}

		// Token: 0x060023FE RID: 9214 RVA: 0x0006C3DC File Offset: 0x0006A5DC
		private void Update()
		{
			if (this._aiController == null)
			{
				return;
			}
			if (this._aiController.target == null)
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

		// Token: 0x04001EBC RID: 7868
		[SerializeField]
		private Character _character;

		// Token: 0x04001EBD RID: 7869
		[SerializeField]
		private Vector2 _movementSpeedRange = new Vector2(-0.2f, 0.2f);

		// Token: 0x04001EBE RID: 7870
		[SerializeField]
		private float _speedBonusAtChaseTarget = 0.3f;

		// Token: 0x04001EBF RID: 7871
		private AIController _aiController;

		// Token: 0x04001EC0 RID: 7872
		private Stat.Values _statValue;

		// Token: 0x04001EC1 RID: 7873
		private bool _movementSpeedAttached;
	}
}
