using System;
using UnityEngine;

namespace Characters.AI.Chimera
{
	// Token: 0x0200125E RID: 4702
	public class WeightedPattern : MonoBehaviour
	{
		// Token: 0x06005D37 RID: 23863 RVA: 0x00112598 File Offset: 0x00110798
		private void Awake()
		{
			this._weightedRandomizer = WeightedRandomizer.From<Pattern>(new ValueTuple<Pattern, float>[]
			{
				new ValueTuple<Pattern, float>(Pattern.Idle, this._idleWeight),
				new ValueTuple<Pattern, float>(Pattern.Bite, this._biteWeight),
				new ValueTuple<Pattern, float>(Pattern.Stomp, this._slamWeight),
				new ValueTuple<Pattern, float>(Pattern.VenomFall, this._venomFallWeight),
				new ValueTuple<Pattern, float>(Pattern.VenomBall, this._venomBallWeight),
				new ValueTuple<Pattern, float>(Pattern.VenomCannon, this._venomCannonWeight),
				new ValueTuple<Pattern, float>(Pattern.SubjectDrop, this._subjectDropWeight),
				new ValueTuple<Pattern, float>(Pattern.WreckDrop, this._wreckDropWeight),
				new ValueTuple<Pattern, float>(Pattern.WreckDestroy, this._wreckDestroyWeight),
				new ValueTuple<Pattern, float>(Pattern.VenomBreath, this._venomBreathWeight)
			});
		}

		// Token: 0x06005D38 RID: 23864 RVA: 0x00112678 File Offset: 0x00110878
		public Pattern TakeOne()
		{
			return this._weightedRandomizer.TakeOne();
		}

		// Token: 0x04004ACD RID: 19149
		private WeightedRandomizer<Pattern> _weightedRandomizer;

		// Token: 0x04004ACE RID: 19150
		[Range(0f, 100f)]
		[SerializeField]
		private float _idleWeight;

		// Token: 0x04004ACF RID: 19151
		[Range(0f, 100f)]
		[SerializeField]
		private float _biteWeight;

		// Token: 0x04004AD0 RID: 19152
		[SerializeField]
		[Range(0f, 100f)]
		private float _slamWeight;

		// Token: 0x04004AD1 RID: 19153
		[Range(0f, 100f)]
		[SerializeField]
		private float _venomFallWeight;

		// Token: 0x04004AD2 RID: 19154
		[SerializeField]
		[Range(0f, 100f)]
		private float _venomBallWeight;

		// Token: 0x04004AD3 RID: 19155
		[Range(0f, 100f)]
		[SerializeField]
		private float _venomCannonWeight;

		// Token: 0x04004AD4 RID: 19156
		[SerializeField]
		[Range(0f, 100f)]
		private float _subjectDropWeight;

		// Token: 0x04004AD5 RID: 19157
		[SerializeField]
		[Range(0f, 100f)]
		private float _wreckDropWeight;

		// Token: 0x04004AD6 RID: 19158
		[Range(0f, 100f)]
		[SerializeField]
		private float _wreckDestroyWeight;

		// Token: 0x04004AD7 RID: 19159
		[SerializeField]
		[Range(0f, 100f)]
		private float _venomBreathWeight;
	}
}
