using System;
using UnityEngine;

namespace Characters.Operations.Decorator
{
	// Token: 0x02000EC5 RID: 3781
	public class Repeater : CharacterOperation
	{
		// Token: 0x06004A41 RID: 19009 RVA: 0x000D8D34 File Offset: 0x000D6F34
		public override void Initialize()
		{
			this._toRepeat.Initialize();
		}

		// Token: 0x06004A42 RID: 19010 RVA: 0x000D8D41 File Offset: 0x000D6F41
		private void Awake()
		{
			if (this._times == 0)
			{
				this._times = int.MaxValue;
			}
		}

		// Token: 0x06004A43 RID: 19011 RVA: 0x000D8D58 File Offset: 0x000D6F58
		public override void Run(Character owner)
		{
			Repeater.<>c__DisplayClass7_0 CS$<>8__locals1 = new Repeater.<>c__DisplayClass7_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.owner = owner;
			CS$<>8__locals1.interval = this._interval / this.runSpeed;
			this._repeatCoroutineReference = this.StartCoroutineWithReference(CS$<>8__locals1.<Run>g__CRepeat|0());
		}

		// Token: 0x06004A44 RID: 19012 RVA: 0x000D8DA0 File Offset: 0x000D6FA0
		public override void Run(Character owner, Character target)
		{
			Repeater.<>c__DisplayClass8_0 CS$<>8__locals1 = new Repeater.<>c__DisplayClass8_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.owner = owner;
			CS$<>8__locals1.target = target;
			CS$<>8__locals1.interval = this._interval / this.runSpeed;
			this._repeatCoroutineReference = this.StartCoroutineWithReference(CS$<>8__locals1.<Run>g__CRepeat|0());
		}

		// Token: 0x06004A45 RID: 19013 RVA: 0x000D8DED File Offset: 0x000D6FED
		public override void Stop()
		{
			this._toRepeat.Stop();
			this._repeatCoroutineReference.Stop();
		}

		// Token: 0x04003970 RID: 14704
		[SerializeField]
		private int _times;

		// Token: 0x04003971 RID: 14705
		[SerializeField]
		private float _interval;

		// Token: 0x04003972 RID: 14706
		[SerializeField]
		private bool _timeIndependant;

		// Token: 0x04003973 RID: 14707
		[CharacterOperation.SubcomponentAttribute]
		[SerializeField]
		private CharacterOperation _toRepeat;

		// Token: 0x04003974 RID: 14708
		private CoroutineReference _repeatCoroutineReference;
	}
}
