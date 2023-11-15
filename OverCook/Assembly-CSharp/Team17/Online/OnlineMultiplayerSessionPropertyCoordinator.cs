using System;
using System.Collections;
using System.Collections.Generic;

namespace Team17.Online
{
	// Token: 0x02000961 RID: 2401
	public class OnlineMultiplayerSessionPropertyCoordinator : SteamOnlineMultiplayerSessionPropertyCoordinator, IOnlineMultiplayerSessionPropertyCoordinator
	{
		// Token: 0x06002EF3 RID: 12019 RVA: 0x000DBCB0 File Offset: 0x000DA0B0
		public bool IsInitialized()
		{
			return this.m_isInitialized;
		}

		// Token: 0x06002EF4 RID: 12020 RVA: 0x000DBCB8 File Offset: 0x000DA0B8
		public IOnlineMultiplayerSessionProperty FindProperty(OnlineMultiplayerSessionPropertyId id)
		{
			return this.FindPropertyInternal(id);
		}

		// Token: 0x06002EF5 RID: 12021 RVA: 0x000DBCC4 File Offset: 0x000DA0C4
		public void Initialize()
		{
			if (!this.m_isInitialized && this.m_sessionProperties != null)
			{
				try
				{
					uint num = 0U;
					IEnumerator enumerator = Enum.GetValues(typeof(OnlineMultiplayerSessionPropertyId)).GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							OnlineMultiplayerSessionPropertyId id = (OnlineMultiplayerSessionPropertyId)obj;
							this.m_sessionProperties.Add(new OnlineMultiplayerSessionProperty
							{
								m_name = id.ToString(),
								m_id = (uint)id,
								m_index = num++
							});
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
					if (this.m_sessionProperties.Count > 0)
					{
						if (base.Open(this.m_sessionProperties))
						{
							this.m_isInitialized = true;
						}
						else
						{
							this.m_sessionProperties.Clear();
						}
					}
				}
				catch (Exception ex)
				{
					this.m_isInitialized = false;
					this.m_sessionProperties.Clear();
				}
			}
		}

		// Token: 0x06002EF6 RID: 12022 RVA: 0x000DBDE0 File Offset: 0x000DA1E0
		private IOnlineMultiplayerSessionProperty FindPropertyInternal(OnlineMultiplayerSessionPropertyId id)
		{
			if (this.m_isInitialized)
			{
				for (int i = 0; i < this.m_sessionProperties.Count; i++)
				{
					if (id == (OnlineMultiplayerSessionPropertyId)this.m_sessionProperties[i].Id)
					{
						return this.m_sessionProperties[i];
					}
				}
			}
			return null;
		}

		// Token: 0x0400258E RID: 9614
		private bool m_isInitialized;

		// Token: 0x0400258F RID: 9615
		private List<OnlineMultiplayerSessionProperty> m_sessionProperties = new List<OnlineMultiplayerSessionProperty>(Enum.GetNames(typeof(OnlineMultiplayerSessionPropertyId)).Length);
	}
}
