using System;
using UnityEngine;

// Token: 0x020006C6 RID: 1734
public class CargoDropperStorage : GameStateMachine<CargoDropperStorage, CargoDropperStorage.StatesInstance, IStateMachineTarget, CargoDropperStorage.Def>
{
	// Token: 0x06002F30 RID: 12080 RVA: 0x000F8C69 File Offset: 0x000F6E69
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.EventHandler(GameHashes.JettisonCargo, delegate(CargoDropperStorage.StatesInstance smi, object data)
		{
			smi.JettisonCargo(data);
		});
	}

	// Token: 0x02001405 RID: 5125
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006417 RID: 25623
		public Vector3 dropOffset;
	}

	// Token: 0x02001406 RID: 5126
	public class StatesInstance : GameStateMachine<CargoDropperStorage, CargoDropperStorage.StatesInstance, IStateMachineTarget, CargoDropperStorage.Def>.GameInstance
	{
		// Token: 0x06008323 RID: 33571 RVA: 0x002FC890 File Offset: 0x002FAA90
		public StatesInstance(IStateMachineTarget master, CargoDropperStorage.Def def) : base(master, def)
		{
		}

		// Token: 0x06008324 RID: 33572 RVA: 0x002FC89C File Offset: 0x002FAA9C
		public void JettisonCargo(object data)
		{
			Vector3 position = base.master.transform.GetPosition() + base.def.dropOffset;
			Storage component = base.GetComponent<Storage>();
			if (component != null)
			{
				GameObject gameObject = component.FindFirst("ScoutRover");
				if (gameObject != null)
				{
					component.Drop(gameObject, true);
					Vector3 position2 = base.master.transform.GetPosition();
					position2.z = Grid.GetLayerZ(Grid.SceneLayer.Creatures);
					gameObject.transform.SetPosition(position2);
					ChoreProvider component2 = gameObject.GetComponent<ChoreProvider>();
					if (component2 != null)
					{
						KBatchedAnimController component3 = gameObject.GetComponent<KBatchedAnimController>();
						if (component3 != null)
						{
							component3.Play("enter", KAnim.PlayMode.Once, 1f, 0f);
						}
						new EmoteChore(component2, Db.Get().ChoreTypes.EmoteHighPriority, null, new HashedString[]
						{
							"enter"
						}, KAnim.PlayMode.Once, false);
					}
					gameObject.GetMyWorld().SetRoverLanded();
				}
				component.DropAll(position, false, false, default(Vector3), true, null);
			}
		}
	}
}
