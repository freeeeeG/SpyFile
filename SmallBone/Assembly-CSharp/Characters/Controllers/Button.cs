using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Characters.Controllers
{
	// Token: 0x02000912 RID: 2322
	public sealed class Button
	{
		// Token: 0x060031C8 RID: 12744 RVA: 0x000940C8 File Offset: 0x000922C8
		static Button()
		{
			Button._names = (from kind in Button._values
			select kind.name).ToArray<string>();
			Button.values = Array.AsReadOnly<Button>(Button._values);
			Button.names = Array.AsReadOnly<string>(Button._names);
			Button.count = Button._count;
		}

		// Token: 0x060031C9 RID: 12745 RVA: 0x000941B1 File Offset: 0x000923B1
		private Button(string name)
		{
			this.name = name;
			this.index = Button._count++;
		}

		// Token: 0x060031CA RID: 12746 RVA: 0x000941D3 File Offset: 0x000923D3
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x040028BF RID: 10431
		public static readonly Button Attack;

		// Token: 0x040028C0 RID: 10432
		public static readonly Button Dash;

		// Token: 0x040028C1 RID: 10433
		public static readonly Button Jump;

		// Token: 0x040028C2 RID: 10434
		public static readonly Button Skill;

		// Token: 0x040028C3 RID: 10435
		public static readonly Button Skill2;

		// Token: 0x040028C4 RID: 10436
		public static readonly Button UseItem;

		// Token: 0x040028C5 RID: 10437
		public static readonly Button None;

		// Token: 0x040028C6 RID: 10438
		public static readonly int count;

		// Token: 0x040028C7 RID: 10439
		public static readonly ReadOnlyCollection<Button> values;

		// Token: 0x040028C8 RID: 10440
		public static readonly ReadOnlyCollection<string> names;

		// Token: 0x040028C9 RID: 10441
		private static readonly string[] _names;

		// Token: 0x040028CA RID: 10442
		private static Button[] _values = new Button[]
		{
			Button.Attack = new Button("Attack"),
			Button.Dash = new Button("Dash"),
			Button.Jump = new Button("Jump"),
			Button.Skill = new Button("Skill"),
			Button.Skill2 = new Button("Skill2"),
			Button.UseItem = new Button("UseItem"),
			Button.None = new Button("None")
		};

		// Token: 0x040028CB RID: 10443
		private static int _count;

		// Token: 0x040028CC RID: 10444
		public readonly string name;

		// Token: 0x040028CD RID: 10445
		public readonly int index;

		// Token: 0x02000913 RID: 2323
		public class StringPopupAttribute : PopupAttribute
		{
			// Token: 0x060031CB RID: 12747 RVA: 0x000941DC File Offset: 0x000923DC
			public StringPopupAttribute()
			{
				bool allowCustom = false;
				object[] names = Button._names;
				base..ctor(allowCustom, names);
			}
		}
	}
}
