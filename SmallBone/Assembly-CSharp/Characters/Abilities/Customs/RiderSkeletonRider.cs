using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Operations;
using Characters.Operations.Customs;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D86 RID: 3462
	[Serializable]
	public class RiderSkeletonRider : Ability
	{
		// Token: 0x060045A7 RID: 17831 RVA: 0x000C9E85 File Offset: 0x000C8085
		private void OnEnable()
		{
			Singleton<Service>.Instance.levelManager.onMapLoaded += this.InvokeOnMapLoaded;
		}

		// Token: 0x060045A8 RID: 17832 RVA: 0x000C9EA2 File Offset: 0x000C80A2
		private void OnDisable()
		{
			Singleton<Service>.Instance.levelManager.onMapLoaded -= this.InvokeOnMapLoaded;
		}

		// Token: 0x060045A9 RID: 17833 RVA: 0x000C9EBF File Offset: 0x000C80BF
		private void InvokeOnMapLoaded()
		{
			this.onMapLoaded();
		}

		// Token: 0x060045AA RID: 17834 RVA: 0x000C9ECC File Offset: 0x000C80CC
		public override void Initialize()
		{
			base.Initialize();
			this._riderAnimators = new Animator[this._riders.Length];
			this._riderEndingOperations = new RiderEndingOperations[this._riders.Length];
			for (int i = 0; i < this._riders.Length; i++)
			{
				OperationInfos operationInfos = this._riders[i];
				this._riderAnimators[i] = operationInfos.GetComponent<Animator>();
				this._riderEndingOperations[i] = operationInfos.GetComponent<RiderEndingOperations>();
			}
		}

		// Token: 0x060045AB RID: 17835 RVA: 0x000C9F3D File Offset: 0x000C813D
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new RiderSkeletonRider.Instance(owner, this);
		}

		// Token: 0x040034EB RID: 13547
		private static readonly Vector3 lookingRight = new Vector3(1f, 1f, 1f);

		// Token: 0x040034EC RID: 13548
		private static readonly Vector3 lookingLeft = new Vector3(-1f, 1f, 1f);

		// Token: 0x040034ED RID: 13549
		[SerializeField]
		private OperationInfos[] _riders;

		// Token: 0x040034EE RID: 13550
		[SerializeField]
		private float[] _riderDelays;

		// Token: 0x040034EF RID: 13551
		private Animator[] _riderAnimators;

		// Token: 0x040034F0 RID: 13552
		private RiderEndingOperations[] _riderEndingOperations;

		// Token: 0x040034F1 RID: 13553
		private Action onMapLoaded;

		// Token: 0x02000D87 RID: 3463
		public class Instance : AbilityInstance<RiderSkeletonRider>
		{
			// Token: 0x060045AE RID: 17838 RVA: 0x000C9F7C File Offset: 0x000C817C
			public Instance(Character owner, RiderSkeletonRider ability) : base(owner, ability)
			{
				int capacity = Mathf.CeilToInt((float)this.targetFrame * (ability._riderDelays.Last<float>() + 1f));
				this._positions = new List<Vector2>(capacity);
				this._times = new List<float>(capacity);
				this._speeds = new List<float>(capacity);
				this._flips = new List<Character.LookingDirection>(capacity);
			}

			// Token: 0x060045AF RID: 17839 RVA: 0x000C9FE8 File Offset: 0x000C81E8
			protected override void OnAttach()
			{
				foreach (OperationInfos operationInfos in this.ability._riders)
				{
					operationInfos.gameObject.SetActive(false);
					operationInfos.transform.parent = null;
				}
				Singleton<Service>.Instance.levelManager.onMapLoaded += this.OnMapLoaded;
			}

			// Token: 0x060045B0 RID: 17840 RVA: 0x000CA044 File Offset: 0x000C8244
			protected override void OnDetach()
			{
				if (Service.quitting)
				{
					return;
				}
				for (int i = 0; i < this.ability._riders.Length; i++)
				{
					OperationInfos operationInfos = this.ability._riders[i];
					RiderEndingOperations riderEndingOperations = this.ability._riderEndingOperations[i];
					riderEndingOperations.Initialize();
					riderEndingOperations.Run(this.owner);
					operationInfos.gameObject.SetActive(false);
					operationInfos.transform.parent = this.owner.transform;
				}
				Singleton<Service>.Instance.levelManager.onMapLoaded -= this.OnMapLoaded;
			}

			// Token: 0x060045B1 RID: 17841 RVA: 0x000CA0D8 File Offset: 0x000C82D8
			private void OnMapLoaded()
			{
				OperationInfos[] riders = this.ability._riders;
				for (int i = 0; i < riders.Length; i++)
				{
					riders[i].gameObject.SetActive(false);
				}
				this._positions.Clear();
				this._times.Clear();
				this._speeds.Clear();
				this._flips.Clear();
			}

			// Token: 0x060045B2 RID: 17842 RVA: 0x000CA13C File Offset: 0x000C833C
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._positions.Add(this.owner.transform.position);
				this._times.Add(deltaTime);
				this._speeds.Add(this.owner.animationController.parameter.movementSpeed);
				this._flips.Add(this.owner.lookingDirection);
				this.SortRiders();
			}

			// Token: 0x060045B3 RID: 17843 RVA: 0x000CA1B8 File Offset: 0x000C83B8
			private void SortRiders()
			{
				float num = 0f;
				int num2 = 0;
				float num3 = this.ability._riderDelays[0];
				for (int i = this._times.Count - 1; i >= 0; i--)
				{
					float num4 = num + this._times[i];
					if (num3 >= num && num3 < num4)
					{
						OperationInfos operationInfos = this.ability._riders[num2];
						if (!operationInfos.gameObject.activeSelf)
						{
							operationInfos.gameObject.SetActive(true);
							operationInfos.Initialize();
							operationInfos.Run(this.owner);
						}
						if (i == this._positions.Count - 1)
						{
							operationInfos.transform.position = this._positions[i];
						}
						else
						{
							operationInfos.transform.position = Vector2.LerpUnclamped(this._positions[i + 1], this._positions[i], (num3 - num) / (num4 - num));
						}
						this.ability._riderAnimators[num2].speed = this._speeds[i];
						operationInfos.transform.localScale = ((this._flips[i] == Character.LookingDirection.Right) ? RiderSkeletonRider.lookingRight : RiderSkeletonRider.lookingLeft);
						num2++;
						if (num2 == this.ability._riders.Length)
						{
							if (i > Application.targetFrameRate)
							{
								this._times.RemoveRange(0, i);
								this._positions.RemoveRange(0, i);
								this._speeds.RemoveRange(0, i);
								this._flips.RemoveRange(0, i);
								return;
							}
							break;
						}
						else
						{
							num3 = this.ability._riderDelays[num2];
						}
					}
					num = num4;
				}
			}

			// Token: 0x040034F2 RID: 13554
			private List<Vector2> _positions;

			// Token: 0x040034F3 RID: 13555
			private List<float> _times;

			// Token: 0x040034F4 RID: 13556
			private List<float> _speeds;

			// Token: 0x040034F5 RID: 13557
			private List<Character.LookingDirection> _flips;

			// Token: 0x040034F6 RID: 13558
			private readonly int targetFrame = 60;
		}
	}
}
