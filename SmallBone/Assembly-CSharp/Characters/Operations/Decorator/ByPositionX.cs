using System;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Decorator
{
	// Token: 0x02000EB7 RID: 3767
	public sealed class ByPositionX : CharacterOperation
	{
		// Token: 0x06004A17 RID: 18967 RVA: 0x000D8704 File Offset: 0x000D6904
		public override void Initialize()
		{
			if (this._onRight != null)
			{
				this._onRight.Initialize();
			}
			if (this._onLeft != null)
			{
				this._onLeft.Initialize();
			}
		}

		// Token: 0x06004A18 RID: 18968 RVA: 0x000D8738 File Offset: 0x000D6938
		public override void Run(Character owner)
		{
			if (this._target.GetPositionX(owner) > this._based.GetPositionX(owner))
			{
				if (this._onRight == null)
				{
					return;
				}
				this._onRight.Stop();
				this._onRight.Run(owner);
				return;
			}
			else
			{
				if (this._onLeft == null)
				{
					return;
				}
				this._onLeft.Stop();
				this._onLeft.Run(owner);
				return;
			}
		}

		// Token: 0x06004A19 RID: 18969 RVA: 0x000D87AC File Offset: 0x000D69AC
		public override void Stop()
		{
			if (this._onRight != null)
			{
				this._onRight.Stop();
			}
			if (this._onLeft != null)
			{
				this._onLeft.Stop();
			}
		}

		// Token: 0x0400394D RID: 14669
		[SerializeField]
		private ByPositionX.TargetInfo _target;

		// Token: 0x0400394E RID: 14670
		[SerializeField]
		private ByPositionX.TargetInfo _based;

		// Token: 0x0400394F RID: 14671
		[CharacterOperation.SubcomponentAttribute]
		[SerializeField]
		private CharacterOperation _onRight;

		// Token: 0x04003950 RID: 14672
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation _onLeft;

		// Token: 0x02000EB8 RID: 3768
		[Serializable]
		private class TargetInfo
		{
			// Token: 0x06004A1B RID: 18971 RVA: 0x000D87E0 File Offset: 0x000D69E0
			public float GetPositionX(Character owner)
			{
				switch (this._type)
				{
				case ByPositionX.Type.Object:
					return this._object.transform.position.x;
				case ByPositionX.Type.Owner:
					return owner.transform.position.x;
				case ByPositionX.Type.Player:
					return Singleton<Service>.Instance.levelManager.player.transform.position.x;
				default:
					return 0f;
				}
			}

			// Token: 0x04003951 RID: 14673
			[SerializeField]
			private ByPositionX.Type _type;

			// Token: 0x04003952 RID: 14674
			[SerializeField]
			private Transform _object;
		}

		// Token: 0x02000EB9 RID: 3769
		private enum Type
		{
			// Token: 0x04003954 RID: 14676
			Object,
			// Token: 0x04003955 RID: 14677
			Owner,
			// Token: 0x04003956 RID: 14678
			Player
		}
	}
}
