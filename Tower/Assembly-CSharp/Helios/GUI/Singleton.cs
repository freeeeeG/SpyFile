using System;

namespace Helios.GUI
{
	// Token: 0x020000E4 RID: 228
	public class Singleton<T> where T : new()
	{
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000350 RID: 848 RVA: 0x0000EED7 File Offset: 0x0000D0D7
		public static T Instance
		{
			get
			{
				if (Singleton<T>._instance == null)
				{
					Singleton<T>._instance = Activator.CreateInstance<T>();
				}
				return Singleton<T>._instance;
			}
		}

		// Token: 0x04000326 RID: 806
		private static T _instance;
	}
}
