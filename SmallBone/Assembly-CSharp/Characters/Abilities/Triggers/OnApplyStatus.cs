using System;
using UnityEngine;
using Utils;

namespace Characters.Abilities.Triggers
{
	// Token: 0x02000B21 RID: 2849
	[Serializable]
	public sealed class OnApplyStatus : Trigger
	{
		// Token: 0x060039C7 RID: 14791 RVA: 0x000AAA64 File Offset: 0x000A8C64
		public override void Attach(Character character)
		{
			this._character = character;
			CharacterStatus.Timing timing = CharacterStatus.Timing.Apply;
			switch (this._timing)
			{
			case OnApplyStatus.Timing.Apply:
				timing = CharacterStatus.Timing.Apply;
				break;
			case OnApplyStatus.Timing.Refresh:
				timing = CharacterStatus.Timing.Refresh;
				break;
			case OnApplyStatus.Timing.Release:
				timing = CharacterStatus.Timing.Release;
				break;
			}
			this._character.status.Register(this._kind, timing, new CharacterStatus.OnTimeDelegate(this.Invoke));
		}

		// Token: 0x060039C8 RID: 14792 RVA: 0x000AAAC4 File Offset: 0x000A8CC4
		public override void Detach()
		{
			CharacterStatus.Timing timing = CharacterStatus.Timing.Apply;
			switch (this._timing)
			{
			case OnApplyStatus.Timing.Apply:
				timing = CharacterStatus.Timing.Apply;
				break;
			case OnApplyStatus.Timing.Refresh:
				timing = CharacterStatus.Timing.Refresh;
				break;
			case OnApplyStatus.Timing.Release:
				timing = CharacterStatus.Timing.Release;
				break;
			}
			this._character.status.Unregister(this._kind, timing, new CharacterStatus.OnTimeDelegate(this.Invoke));
		}

		// Token: 0x060039C9 RID: 14793 RVA: 0x000AAB1B File Offset: 0x000A8D1B
		public void Invoke(Character giver, Character target)
		{
			this._positionInfo.Attach(target, this._moveToTargetPoint);
			base.Invoke();
		}

		// Token: 0x04002DD6 RID: 11734
		[SerializeField]
		private PositionInfo _positionInfo = new PositionInfo();

		// Token: 0x04002DD7 RID: 11735
		[SerializeField]
		private Transform _moveToTargetPoint;

		// Token: 0x04002DD8 RID: 11736
		[SerializeField]
		private OnApplyStatus.Timing _timing;

		// Token: 0x04002DD9 RID: 11737
		[SerializeField]
		private CharacterStatus.Kind _kind;

		// Token: 0x04002DDA RID: 11738
		private Character _character;

		// Token: 0x02000B22 RID: 2850
		private enum Timing
		{
			// Token: 0x04002DDC RID: 11740
			Apply,
			// Token: 0x04002DDD RID: 11741
			Refresh,
			// Token: 0x04002DDE RID: 11742
			Release
		}
	}
}
