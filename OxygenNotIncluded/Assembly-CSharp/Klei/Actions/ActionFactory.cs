﻿using System;
using System.Collections.Generic;

namespace Klei.Actions
{
	// Token: 0x02000E21 RID: 3617
	public class ActionFactory<ActionFactoryType, ActionType, EnumType> where ActionFactoryType : ActionFactory<ActionFactoryType, ActionType, EnumType>
	{
		// Token: 0x06006EBD RID: 28349 RVA: 0x002B86D8 File Offset: 0x002B68D8
		public static ActionType GetOrCreateAction(EnumType actionType)
		{
			ActionType result;
			if (!ActionFactory<ActionFactoryType, ActionType, EnumType>.actionInstances.TryGetValue(actionType, out result))
			{
				ActionFactory<ActionFactoryType, ActionType, EnumType>.EnsureFactoryInstance();
				result = (ActionFactory<ActionFactoryType, ActionType, EnumType>.actionInstances[actionType] = ActionFactory<ActionFactoryType, ActionType, EnumType>.actionFactory.CreateAction(actionType));
			}
			return result;
		}

		// Token: 0x06006EBE RID: 28350 RVA: 0x002B8717 File Offset: 0x002B6917
		private static void EnsureFactoryInstance()
		{
			if (ActionFactory<ActionFactoryType, ActionType, EnumType>.actionFactory != null)
			{
				return;
			}
			ActionFactory<ActionFactoryType, ActionType, EnumType>.actionFactory = (Activator.CreateInstance(typeof(ActionFactoryType)) as ActionFactoryType);
		}

		// Token: 0x06006EBF RID: 28351 RVA: 0x002B8744 File Offset: 0x002B6944
		protected virtual ActionType CreateAction(EnumType actionType)
		{
			throw new InvalidOperationException("Can not call InterfaceToolActionFactory<T1, T2>.CreateAction()! This function must be called from a deriving class!");
		}

		// Token: 0x040052E6 RID: 21222
		private static Dictionary<EnumType, ActionType> actionInstances = new Dictionary<EnumType, ActionType>();

		// Token: 0x040052E7 RID: 21223
		private static ActionFactoryType actionFactory = default(ActionFactoryType);
	}
}
