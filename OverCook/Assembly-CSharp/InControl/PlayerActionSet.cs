using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using UnityEngine;

namespace InControl
{
	// Token: 0x020002A4 RID: 676
	public abstract class PlayerActionSet
	{
		// Token: 0x06000CB0 RID: 3248 RVA: 0x00033010 File Offset: 0x00031410
		protected PlayerActionSet()
		{
			this.Actions = new ReadOnlyCollection<PlayerAction>(this.actions);
			this.Enabled = true;
			InputManager.AttachPlayerActionSet(this);
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000CB1 RID: 3249 RVA: 0x00033078 File Offset: 0x00031478
		// (set) Token: 0x06000CB2 RID: 3250 RVA: 0x00033080 File Offset: 0x00031480
		public InputDevice Device { get; set; }

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000CB3 RID: 3251 RVA: 0x00033089 File Offset: 0x00031489
		// (set) Token: 0x06000CB4 RID: 3252 RVA: 0x00033091 File Offset: 0x00031491
		public ReadOnlyCollection<PlayerAction> Actions { get; private set; }

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000CB5 RID: 3253 RVA: 0x0003309A File Offset: 0x0003149A
		// (set) Token: 0x06000CB6 RID: 3254 RVA: 0x000330A2 File Offset: 0x000314A2
		public ulong UpdateTick { get; protected set; }

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000CB7 RID: 3255 RVA: 0x000330AB File Offset: 0x000314AB
		// (set) Token: 0x06000CB8 RID: 3256 RVA: 0x000330B3 File Offset: 0x000314B3
		public bool Enabled { get; set; }

		// Token: 0x06000CB9 RID: 3257 RVA: 0x000330BC File Offset: 0x000314BC
		public void Destroy()
		{
			InputManager.DetachPlayerActionSet(this);
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x000330C4 File Offset: 0x000314C4
		protected PlayerAction CreatePlayerAction(string name)
		{
			PlayerAction playerAction = new PlayerAction(name, this);
			playerAction.Device = (this.Device ?? InputManager.ActiveDevice);
			if (this.actionsByName.ContainsKey(name))
			{
				throw new InControlException("Action '" + name + "' already exists in this set.");
			}
			this.actions.Add(playerAction);
			this.actionsByName.Add(name, playerAction);
			return playerAction;
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x00033132 File Offset: 0x00031532
		protected void ClearActions()
		{
			this.actions.Clear();
			this.actionsByName.Clear();
			this.oneAxisActions.Clear();
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x00033158 File Offset: 0x00031558
		protected PlayerOneAxisAction CreateOneAxisPlayerAction(PlayerAction negativeAction, PlayerAction positiveAction)
		{
			PlayerOneAxisAction playerOneAxisAction = new PlayerOneAxisAction(negativeAction, positiveAction);
			this.oneAxisActions.Add(playerOneAxisAction);
			return playerOneAxisAction;
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x0003317C File Offset: 0x0003157C
		protected PlayerTwoAxisAction CreateTwoAxisPlayerAction(PlayerAction negativeXAction, PlayerAction positiveXAction, PlayerAction negativeYAction, PlayerAction positiveYAction)
		{
			PlayerTwoAxisAction playerTwoAxisAction = new PlayerTwoAxisAction(negativeXAction, positiveXAction, negativeYAction, positiveYAction);
			this.twoAxisActions.Add(playerTwoAxisAction);
			return playerTwoAxisAction;
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x000331A4 File Offset: 0x000315A4
		internal void Update(ulong updateTick, float deltaTime)
		{
			InputDevice device = this.Device ?? InputManager.ActiveDevice;
			int count = this.actions.Count;
			for (int i = 0; i < count; i++)
			{
				PlayerAction playerAction = this.actions[i];
				playerAction.Update(updateTick, deltaTime, device);
				if (playerAction.UpdateTick > this.UpdateTick)
				{
					this.UpdateTick = playerAction.UpdateTick;
					this.LastInputType = playerAction.LastInputType;
				}
			}
			int count2 = this.oneAxisActions.Count;
			for (int j = 0; j < count2; j++)
			{
				this.oneAxisActions[j].Update(updateTick, deltaTime);
			}
			int count3 = this.twoAxisActions.Count;
			for (int k = 0; k < count3; k++)
			{
				this.twoAxisActions[k].Update(updateTick, deltaTime);
			}
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x00033294 File Offset: 0x00031694
		public void Reset()
		{
			int count = this.actions.Count;
			for (int i = 0; i < count; i++)
			{
				this.actions[i].ResetBindings();
			}
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x000332D0 File Offset: 0x000316D0
		public void ClearInputState()
		{
			int count = this.actions.Count;
			for (int i = 0; i < count; i++)
			{
				this.actions[i].ClearInputState();
			}
			int count2 = this.oneAxisActions.Count;
			for (int j = 0; j < count2; j++)
			{
				this.oneAxisActions[j].ClearInputState();
			}
			int count3 = this.twoAxisActions.Count;
			for (int k = 0; k < count3; k++)
			{
				this.twoAxisActions[k].ClearInputState();
			}
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x00033374 File Offset: 0x00031774
		internal bool HasBinding(BindingSource binding)
		{
			if (binding == null)
			{
				return false;
			}
			int count = this.actions.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.actions[i].HasBinding(binding))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x000333C8 File Offset: 0x000317C8
		internal void RemoveBinding(BindingSource binding)
		{
			if (binding == null)
			{
				return;
			}
			int count = this.actions.Count;
			for (int i = 0; i < count; i++)
			{
				this.actions[i].FindAndRemoveBinding(binding);
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000CC3 RID: 3267 RVA: 0x00033412 File Offset: 0x00031812
		// (set) Token: 0x06000CC4 RID: 3268 RVA: 0x0003341A File Offset: 0x0003181A
		public BindingListenOptions ListenOptions
		{
			get
			{
				return this.listenOptions;
			}
			set
			{
				this.listenOptions = (value ?? new BindingListenOptions());
			}
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x00033430 File Offset: 0x00031830
		public string Save()
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream, Encoding.UTF8))
				{
					binaryWriter.Write(66);
					binaryWriter.Write(73);
					binaryWriter.Write(78);
					binaryWriter.Write(68);
					binaryWriter.Write(1);
					int count = this.actions.Count;
					binaryWriter.Write(count);
					for (int i = 0; i < count; i++)
					{
						this.actions[i].Save(binaryWriter);
					}
				}
				result = Convert.ToBase64String(memoryStream.ToArray());
			}
			return result;
		}

		// Token: 0x06000CC6 RID: 3270 RVA: 0x000334FC File Offset: 0x000318FC
		public void Load(string data)
		{
			if (data == null)
			{
				return;
			}
			try
			{
				using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(data)))
				{
					using (BinaryReader binaryReader = new BinaryReader(memoryStream))
					{
						if (binaryReader.ReadUInt32() != 1145981250U)
						{
							throw new Exception("Unknown data format.");
						}
						if (binaryReader.ReadUInt16() != 1)
						{
							throw new Exception("Unknown data version.");
						}
						int num = binaryReader.ReadInt32();
						for (int i = 0; i < num; i++)
						{
							PlayerAction playerAction;
							if (this.actionsByName.TryGetValue(binaryReader.ReadString(), out playerAction))
							{
								playerAction.Load(binaryReader);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Provided state could not be loaded:\n" + ex.Message);
				this.Reset();
			}
		}

		// Token: 0x040009EF RID: 2543
		public BindingSourceType LastInputType;

		// Token: 0x040009F1 RID: 2545
		private List<PlayerAction> actions = new List<PlayerAction>();

		// Token: 0x040009F2 RID: 2546
		private List<PlayerOneAxisAction> oneAxisActions = new List<PlayerOneAxisAction>();

		// Token: 0x040009F3 RID: 2547
		private List<PlayerTwoAxisAction> twoAxisActions = new List<PlayerTwoAxisAction>();

		// Token: 0x040009F4 RID: 2548
		private Dictionary<string, PlayerAction> actionsByName = new Dictionary<string, PlayerAction>();

		// Token: 0x040009F5 RID: 2549
		private BindingListenOptions listenOptions = new BindingListenOptions();

		// Token: 0x040009F6 RID: 2550
		internal PlayerAction listenWithAction;
	}
}
