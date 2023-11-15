using System;
using UnityEngine;

namespace Characters
{
	// Token: 0x020006F4 RID: 1780
	[RequireComponent(typeof(Character))]
	public sealed class UnstoppableResistanceStacker : MonoBehaviour
	{
		// Token: 0x060023EE RID: 9198 RVA: 0x0006BEF1 File Offset: 0x0006A0F1
		private void Awake()
		{
			this._freezeController.Initialize(this._owner, CharacterStatus.Kind.Freeze);
			this._stunController.Initialize(this._owner, CharacterStatus.Kind.Stun);
		}

		// Token: 0x060023EF RID: 9199 RVA: 0x0006BF18 File Offset: 0x0006A118
		private void Update()
		{
			this._freezeController.UpdateTime(this._owner.chronometer.master.deltaTime);
			this._stunController.UpdateTime(this._owner.chronometer.master.deltaTime);
		}

		// Token: 0x04001EA4 RID: 7844
		[GetComponent]
		[SerializeField]
		private Character _owner;

		// Token: 0x04001EA5 RID: 7845
		[SerializeField]
		private UnstoppableResistanceStacker.StackController _freezeController;

		// Token: 0x04001EA6 RID: 7846
		[SerializeField]
		private UnstoppableResistanceStacker.StackController _stunController;

		// Token: 0x020006F5 RID: 1781
		[Serializable]
		private class StackController
		{
			// Token: 0x060023F1 RID: 9201 RVA: 0x0006BF68 File Offset: 0x0006A168
			internal void Initialize(Character character, CharacterStatus.Kind kind)
			{
				this._increaseValue *= 0.01f;
				this._reduceValue *= 0.01f;
				this._statValueMinMax.x = this._statValueMinMax.x * 0.01f;
				this._statValueMinMax.y = this._statValueMinMax.y * 0.01f;
				this._cacheValue = float.MinValue;
				this._owner = character;
				this._stackableStat = this._targetStat.Clone();
				character.stat.AttachValues(this._stackableStat);
				if (kind != CharacterStatus.Kind.Stun)
				{
					if (kind == CharacterStatus.Kind.Freeze)
					{
						character.status.freeze.onAttachEvents += this.IncreaseValue;
						character.status.freeze.onRefreshEvents += this.TryIncreaseValue;
						return;
					}
				}
				else
				{
					character.status.stun.onAttachEvents += this.IncreaseValue;
					character.status.stun.onRefreshEvents += this.TryIncreaseValue;
				}
			}

			// Token: 0x060023F2 RID: 9202 RVA: 0x0006C070 File Offset: 0x0006A270
			internal void UpdateTime(float deltaTime)
			{
				this._remainMaintainTime -= deltaTime;
				this._remainRefreshTime -= deltaTime;
				if (this._remainMaintainTime <= 0f)
				{
					this._remainReduceTime -= deltaTime;
					if (this._remainReduceTime < 0f)
					{
						this.ReduceValue();
						this._remainReduceTime += this._reduceInterval;
					}
				}
			}

			// Token: 0x060023F3 RID: 9203 RVA: 0x0006C0DC File Offset: 0x0006A2DC
			private void ReduceValue()
			{
				for (int i = 0; i < this._stackableStat.values.Length; i++)
				{
					float num = Mathf.Clamp((float)this._stackableStat.values[i].value + this._reduceValue, this._statValueMinMax.x, this._statValueMinMax.y);
					if (this._cacheValue == num)
					{
						return;
					}
					this._cacheValue = num;
					this._stackableStat.values[i].value = (double)num;
				}
				this._owner.stat.SetNeedUpdate();
			}

			// Token: 0x060023F4 RID: 9204 RVA: 0x0006C16C File Offset: 0x0006A36C
			private void TryIncreaseValue(Character giver, Character taker)
			{
				if (this._remainRefreshTime > 0f)
				{
					return;
				}
				this.IncreaseValue(giver, taker);
			}

			// Token: 0x060023F5 RID: 9205 RVA: 0x0006C184 File Offset: 0x0006A384
			private void IncreaseValue(Character giver, Character taker)
			{
				this._remainMaintainTime = this._maintainTime;
				this._remainRefreshTime = this.refreshTime;
				for (int i = 0; i < this._stackableStat.values.Length; i++)
				{
					float num = Mathf.Clamp((float)this._stackableStat.values[i].value - this._increaseValue, this._statValueMinMax.x, this._statValueMinMax.y);
					if (this._cacheValue == num)
					{
						return;
					}
					this._cacheValue = num;
					this._stackableStat.values[i].value = (double)num;
				}
				this._owner.stat.SetNeedUpdate();
			}

			// Token: 0x04001EA7 RID: 7847
			[SerializeField]
			private float refreshTime = 1f;

			// Token: 0x04001EA8 RID: 7848
			[SerializeField]
			[Header("증가 설정, 상태이상 발생시 increaseValue값 만큼 스탯 증가, maintainTime만큼 유지")]
			private float _increaseValue;

			// Token: 0x04001EA9 RID: 7849
			[SerializeField]
			private float _maintainTime;

			// Token: 0x04001EAA RID: 7850
			[Header("감소 설정, reduceInterval마다 reduceValue만큼 스탯 감소시킴")]
			[SerializeField]
			private float _reduceInterval;

			// Token: 0x04001EAB RID: 7851
			[SerializeField]
			private float _reduceValue;

			// Token: 0x04001EAC RID: 7852
			[SerializeField]
			[Header("범위 및 스탯 설정")]
			[MinMaxSlider(0f, 100f)]
			private Vector2 _statValueMinMax;

			// Token: 0x04001EAD RID: 7853
			[SerializeField]
			private Stat.Values _targetStat;

			// Token: 0x04001EAE RID: 7854
			private Character _owner;

			// Token: 0x04001EAF RID: 7855
			private Stat.Values _stackableStat;

			// Token: 0x04001EB0 RID: 7856
			private float _remainMaintainTime;

			// Token: 0x04001EB1 RID: 7857
			private float _remainReduceTime;

			// Token: 0x04001EB2 RID: 7858
			private float _cacheValue;

			// Token: 0x04001EB3 RID: 7859
			private float _remainRefreshTime;
		}
	}
}
