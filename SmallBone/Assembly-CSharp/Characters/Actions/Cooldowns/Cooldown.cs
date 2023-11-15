using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions.Cooldowns
{
	// Token: 0x02000964 RID: 2404
	public abstract class Cooldown : MonoBehaviour
	{
		// Token: 0x17000B34 RID: 2868
		// (get) Token: 0x060033D4 RID: 13268 RVA: 0x00099D0E File Offset: 0x00097F0E
		// (set) Token: 0x060033D5 RID: 13269 RVA: 0x00099D16 File Offset: 0x00097F16
		public int stacks
		{
			get
			{
				return this._stacks;
			}
			protected set
			{
				if (this._stacks == 0 && value > 0 && this._onReady != null)
				{
					this._onReady();
				}
				this._stacks = value;
			}
		}

		// Token: 0x17000B35 RID: 2869
		// (get) Token: 0x060033D6 RID: 13270
		public abstract bool canUse { get; }

		// Token: 0x17000B36 RID: 2870
		// (get) Token: 0x060033D7 RID: 13271
		public abstract float remainPercent { get; }

		// Token: 0x14000091 RID: 145
		// (add) Token: 0x060033D8 RID: 13272 RVA: 0x00099D3E File Offset: 0x00097F3E
		// (remove) Token: 0x060033D9 RID: 13273 RVA: 0x00099D57 File Offset: 0x00097F57
		public event Action onReady
		{
			add
			{
				this._onReady = (Action)Delegate.Combine(this._onReady, value);
			}
			remove
			{
				this._onReady = (Action)Delegate.Remove(this._onReady, value);
			}
		}

		// Token: 0x060033DA RID: 13274
		internal abstract bool Consume();

		// Token: 0x060033DB RID: 13275 RVA: 0x00002191 File Offset: 0x00000391
		protected virtual void Awake()
		{
		}

		// Token: 0x060033DC RID: 13276 RVA: 0x00099D70 File Offset: 0x00097F70
		internal virtual void Initialize(Character character)
		{
			this._character = character;
		}

		// Token: 0x060033DD RID: 13277 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x04002A05 RID: 10757
		public static readonly Type[] types = new Type[]
		{
			typeof(Infinity),
			typeof(Time),
			typeof(Gauge),
			typeof(Damage)
		};

		// Token: 0x04002A06 RID: 10758
		protected int _stacks;

		// Token: 0x04002A07 RID: 10759
		protected Character _character;

		// Token: 0x04002A08 RID: 10760
		private Action _onReady;

		// Token: 0x02000965 RID: 2405
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x060033E0 RID: 13280 RVA: 0x00099DC8 File Offset: 0x00097FC8
			public SubcomponentAttribute() : base(typeof(Time), Cooldown.types)
			{
			}
		}
	}
}
