using System;
using System.Collections.Generic;
using Klei.Actions;
using UnityEngine;

namespace Klei.Input
{
	// Token: 0x02000E1A RID: 3610
	[CreateAssetMenu(fileName = "InterfaceToolConfig", menuName = "Klei/Interface Tools/Config")]
	public class InterfaceToolConfig : ScriptableObject
	{
		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x06006EA5 RID: 28325 RVA: 0x002B848F File Offset: 0x002B668F
		public DigAction DigAction
		{
			get
			{
				return ActionFactory<DigToolActionFactory, DigAction, DigToolActionFactory.Actions>.GetOrCreateAction(this.digAction);
			}
		}

		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x06006EA6 RID: 28326 RVA: 0x002B849C File Offset: 0x002B669C
		public int Priority
		{
			get
			{
				return this.priority;
			}
		}

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x06006EA7 RID: 28327 RVA: 0x002B84A4 File Offset: 0x002B66A4
		public global::Action InputAction
		{
			get
			{
				return (global::Action)Enum.Parse(typeof(global::Action), this.inputAction);
			}
		}

		// Token: 0x040052DE RID: 21214
		[SerializeField]
		private DigToolActionFactory.Actions digAction;

		// Token: 0x040052DF RID: 21215
		public static InterfaceToolConfig.Comparer ConfigComparer = new InterfaceToolConfig.Comparer();

		// Token: 0x040052E0 RID: 21216
		[SerializeField]
		[Tooltip("Defines which config will take priority should multiple configs be activated\n0 is the lower bound for this value.")]
		private int priority;

		// Token: 0x040052E1 RID: 21217
		[SerializeField]
		[Tooltip("This will serve as a key for activating different configs. Currently, these Actionsare how we indicate that different input modes are desired.\nAssigning Action.Invalid to this field will indicate that this is the \"default\" config")]
		private string inputAction = global::Action.Invalid.ToString();

		// Token: 0x02001F99 RID: 8089
		public class Comparer : IComparer<InterfaceToolConfig>
		{
			// Token: 0x0600A30A RID: 41738 RVA: 0x00367A9B File Offset: 0x00365C9B
			public int Compare(InterfaceToolConfig lhs, InterfaceToolConfig rhs)
			{
				if (lhs.Priority == rhs.Priority)
				{
					return 0;
				}
				if (lhs.Priority <= rhs.Priority)
				{
					return -1;
				}
				return 1;
			}
		}
	}
}
