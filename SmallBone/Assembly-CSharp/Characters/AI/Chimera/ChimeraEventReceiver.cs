using System;
using UnityEngine;

namespace Characters.AI.Chimera
{
	// Token: 0x0200124E RID: 4686
	public class ChimeraEventReceiver : MonoBehaviour
	{
		// Token: 0x140000CC RID: 204
		// (add) Token: 0x06005C50 RID: 23632 RVA: 0x0010FDC8 File Offset: 0x0010DFC8
		// (remove) Token: 0x06005C51 RID: 23633 RVA: 0x0010FE00 File Offset: 0x0010E000
		public event Action onIntro_Ready;

		// Token: 0x06005C52 RID: 23634 RVA: 0x0010FE35 File Offset: 0x0010E035
		public void Intro_Ready()
		{
			Action action = this.onIntro_Ready;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000CD RID: 205
		// (add) Token: 0x06005C53 RID: 23635 RVA: 0x0010FE48 File Offset: 0x0010E048
		// (remove) Token: 0x06005C54 RID: 23636 RVA: 0x0010FE80 File Offset: 0x0010E080
		public event Action onIntro_Landing;

		// Token: 0x06005C55 RID: 23637 RVA: 0x0010FEB5 File Offset: 0x0010E0B5
		public void Intro_Landing()
		{
			Action action = this.onIntro_Landing;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000CE RID: 206
		// (add) Token: 0x06005C56 RID: 23638 RVA: 0x0010FEC8 File Offset: 0x0010E0C8
		// (remove) Token: 0x06005C57 RID: 23639 RVA: 0x0010FF00 File Offset: 0x0010E100
		public event Action onIntro_FallingRocks;

		// Token: 0x06005C58 RID: 23640 RVA: 0x0010FF35 File Offset: 0x0010E135
		public void Intro_FallingRocks()
		{
			Action action = this.onIntro_FallingRocks;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000CF RID: 207
		// (add) Token: 0x06005C59 RID: 23641 RVA: 0x0010FF48 File Offset: 0x0010E148
		// (remove) Token: 0x06005C5A RID: 23642 RVA: 0x0010FF80 File Offset: 0x0010E180
		public event Action onIntro_Explosion;

		// Token: 0x06005C5B RID: 23643 RVA: 0x0010FFB5 File Offset: 0x0010E1B5
		public void Intro_Explosion()
		{
			Action action = this.onIntro_Explosion;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000D0 RID: 208
		// (add) Token: 0x06005C5C RID: 23644 RVA: 0x0010FFC8 File Offset: 0x0010E1C8
		// (remove) Token: 0x06005C5D RID: 23645 RVA: 0x00110000 File Offset: 0x0010E200
		public event Action onIntro_CameraZoomOut;

		// Token: 0x06005C5E RID: 23646 RVA: 0x00110035 File Offset: 0x0010E235
		public void Intro_CameraZoomOut()
		{
			Action action = this.onIntro_CameraZoomOut;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000D1 RID: 209
		// (add) Token: 0x06005C5F RID: 23647 RVA: 0x00110048 File Offset: 0x0010E248
		// (remove) Token: 0x06005C60 RID: 23648 RVA: 0x00110080 File Offset: 0x0010E280
		public event Action onIntro_Roar_Ready;

		// Token: 0x06005C61 RID: 23649 RVA: 0x001100B5 File Offset: 0x0010E2B5
		public void Intro_Roar_Ready()
		{
			Action action = this.onIntro_Roar_Ready;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000D2 RID: 210
		// (add) Token: 0x06005C62 RID: 23650 RVA: 0x001100C8 File Offset: 0x0010E2C8
		// (remove) Token: 0x06005C63 RID: 23651 RVA: 0x00110100 File Offset: 0x0010E300
		public event Action onIntro_Roar;

		// Token: 0x06005C64 RID: 23652 RVA: 0x00110135 File Offset: 0x0010E335
		public void Intro_Roar()
		{
			Action action = this.onIntro_Roar;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000D3 RID: 211
		// (add) Token: 0x06005C65 RID: 23653 RVA: 0x00110148 File Offset: 0x0010E348
		// (remove) Token: 0x06005C66 RID: 23654 RVA: 0x00110180 File Offset: 0x0010E380
		public event Action onIntro_LetterBoxOff;

		// Token: 0x06005C67 RID: 23655 RVA: 0x001101B5 File Offset: 0x0010E3B5
		public void Intro_LetterBoxOff()
		{
			Action action = this.onIntro_LetterBoxOff;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000D4 RID: 212
		// (add) Token: 0x06005C68 RID: 23656 RVA: 0x001101C8 File Offset: 0x0010E3C8
		// (remove) Token: 0x06005C69 RID: 23657 RVA: 0x00110200 File Offset: 0x0010E400
		public event Action onIntro_HealthBarOn;

		// Token: 0x06005C6A RID: 23658 RVA: 0x00110235 File Offset: 0x0010E435
		public void Intro_HealthBarOn()
		{
			Action action = this.onIntro_HealthBarOn;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000D5 RID: 213
		// (add) Token: 0x06005C6B RID: 23659 RVA: 0x00110248 File Offset: 0x0010E448
		// (remove) Token: 0x06005C6C RID: 23660 RVA: 0x00110280 File Offset: 0x0010E480
		public event Action onStomp_Ready;

		// Token: 0x06005C6D RID: 23661 RVA: 0x001102B5 File Offset: 0x0010E4B5
		public void Stomp_Ready()
		{
			Action action = this.onStomp_Ready;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000D6 RID: 214
		// (add) Token: 0x06005C6E RID: 23662 RVA: 0x001102C8 File Offset: 0x0010E4C8
		// (remove) Token: 0x06005C6F RID: 23663 RVA: 0x00110300 File Offset: 0x0010E500
		public event Action onStomp_Attack;

		// Token: 0x06005C70 RID: 23664 RVA: 0x00110335 File Offset: 0x0010E535
		public void Stomp_Attack()
		{
			Action action = this.onStomp_Attack;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000D7 RID: 215
		// (add) Token: 0x06005C71 RID: 23665 RVA: 0x00110348 File Offset: 0x0010E548
		// (remove) Token: 0x06005C72 RID: 23666 RVA: 0x00110380 File Offset: 0x0010E580
		public event Action onStomp_Hit;

		// Token: 0x06005C73 RID: 23667 RVA: 0x001103B5 File Offset: 0x0010E5B5
		public void Stomp_Hit()
		{
			Action action = this.onStomp_Hit;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000D8 RID: 216
		// (add) Token: 0x06005C74 RID: 23668 RVA: 0x001103C8 File Offset: 0x0010E5C8
		// (remove) Token: 0x06005C75 RID: 23669 RVA: 0x00110400 File Offset: 0x0010E600
		public event Action onStomp_End;

		// Token: 0x06005C76 RID: 23670 RVA: 0x00110435 File Offset: 0x0010E635
		public void Stomp_End()
		{
			Action action = this.onStomp_End;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000D9 RID: 217
		// (add) Token: 0x06005C77 RID: 23671 RVA: 0x00110448 File Offset: 0x0010E648
		// (remove) Token: 0x06005C78 RID: 23672 RVA: 0x00110480 File Offset: 0x0010E680
		public event Action onBite_Ready;

		// Token: 0x06005C79 RID: 23673 RVA: 0x001104B5 File Offset: 0x0010E6B5
		public void Bite_Ready()
		{
			Action action = this.onBite_Ready;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000DA RID: 218
		// (add) Token: 0x06005C7A RID: 23674 RVA: 0x001104C8 File Offset: 0x0010E6C8
		// (remove) Token: 0x06005C7B RID: 23675 RVA: 0x00110500 File Offset: 0x0010E700
		public event Action onBite_Attack;

		// Token: 0x06005C7C RID: 23676 RVA: 0x00110535 File Offset: 0x0010E735
		public void Bite_Attack()
		{
			Action action = this.onBite_Attack;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000DB RID: 219
		// (add) Token: 0x06005C7D RID: 23677 RVA: 0x00110548 File Offset: 0x0010E748
		// (remove) Token: 0x06005C7E RID: 23678 RVA: 0x00110580 File Offset: 0x0010E780
		public event Action onBite_Hit;

		// Token: 0x06005C7F RID: 23679 RVA: 0x001105B5 File Offset: 0x0010E7B5
		public void Bite_Hit()
		{
			Action action = this.onBite_Hit;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000DC RID: 220
		// (add) Token: 0x06005C80 RID: 23680 RVA: 0x001105C8 File Offset: 0x0010E7C8
		// (remove) Token: 0x06005C81 RID: 23681 RVA: 0x00110600 File Offset: 0x0010E800
		public event Action onBite_End;

		// Token: 0x06005C82 RID: 23682 RVA: 0x00110635 File Offset: 0x0010E835
		public void Bite_End()
		{
			Action action = this.onBite_End;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000DD RID: 221
		// (add) Token: 0x06005C83 RID: 23683 RVA: 0x00110648 File Offset: 0x0010E848
		// (remove) Token: 0x06005C84 RID: 23684 RVA: 0x00110680 File Offset: 0x0010E880
		public event Action onVenomBall_Ready;

		// Token: 0x06005C85 RID: 23685 RVA: 0x001106B5 File Offset: 0x0010E8B5
		public void VenomBall_Ready()
		{
			Action action = this.onVenomBall_Ready;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000DE RID: 222
		// (add) Token: 0x06005C86 RID: 23686 RVA: 0x001106C8 File Offset: 0x0010E8C8
		// (remove) Token: 0x06005C87 RID: 23687 RVA: 0x00110700 File Offset: 0x0010E900
		public event Action onVenomBall_Fire;

		// Token: 0x06005C88 RID: 23688 RVA: 0x00110735 File Offset: 0x0010E935
		public void VenomBall_Fire()
		{
			Action action = this.onVenomBall_Fire;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000DF RID: 223
		// (add) Token: 0x06005C89 RID: 23689 RVA: 0x00110748 File Offset: 0x0010E948
		// (remove) Token: 0x06005C8A RID: 23690 RVA: 0x00110780 File Offset: 0x0010E980
		public event Action onVenomCannon_Ready;

		// Token: 0x06005C8B RID: 23691 RVA: 0x001107B5 File Offset: 0x0010E9B5
		public void VenomCannon_Ready()
		{
			Action action = this.onVenomCannon_Ready;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000E0 RID: 224
		// (add) Token: 0x06005C8C RID: 23692 RVA: 0x001107C8 File Offset: 0x0010E9C8
		// (remove) Token: 0x06005C8D RID: 23693 RVA: 0x00110800 File Offset: 0x0010EA00
		public event Action onVenomCannon_Fire;

		// Token: 0x06005C8E RID: 23694 RVA: 0x00110835 File Offset: 0x0010EA35
		public void VenomCannon_Fire()
		{
			Action action = this.onVenomCannon_Fire;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000E1 RID: 225
		// (add) Token: 0x06005C8F RID: 23695 RVA: 0x00110848 File Offset: 0x0010EA48
		// (remove) Token: 0x06005C90 RID: 23696 RVA: 0x00110880 File Offset: 0x0010EA80
		public event Action onVenomBreath_Ready;

		// Token: 0x06005C91 RID: 23697 RVA: 0x001108B5 File Offset: 0x0010EAB5
		public void VenomBreath_Ready()
		{
			Action action = this.onVenomBreath_Ready;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000E2 RID: 226
		// (add) Token: 0x06005C92 RID: 23698 RVA: 0x001108C8 File Offset: 0x0010EAC8
		// (remove) Token: 0x06005C93 RID: 23699 RVA: 0x00110900 File Offset: 0x0010EB00
		public event Action onVenomBreath_Fire;

		// Token: 0x06005C94 RID: 23700 RVA: 0x00110935 File Offset: 0x0010EB35
		public void VenomBreath_Fire()
		{
			Action action = this.onVenomBreath_Fire;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000E3 RID: 227
		// (add) Token: 0x06005C95 RID: 23701 RVA: 0x00110948 File Offset: 0x0010EB48
		// (remove) Token: 0x06005C96 RID: 23702 RVA: 0x00110980 File Offset: 0x0010EB80
		public event Action onVenomBreath_End;

		// Token: 0x06005C97 RID: 23703 RVA: 0x001109B5 File Offset: 0x0010EBB5
		public void VenomBreath_End()
		{
			Action action = this.onVenomBreath_End;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000E4 RID: 228
		// (add) Token: 0x06005C98 RID: 23704 RVA: 0x001109C8 File Offset: 0x0010EBC8
		// (remove) Token: 0x06005C99 RID: 23705 RVA: 0x00110A00 File Offset: 0x0010EC00
		public event Action onVenomFall_Roar_Ready;

		// Token: 0x06005C9A RID: 23706 RVA: 0x00110A35 File Offset: 0x0010EC35
		public void VenomFall_Roar_Ready()
		{
			Action action = this.onVenomFall_Roar_Ready;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000E5 RID: 229
		// (add) Token: 0x06005C9B RID: 23707 RVA: 0x00110A48 File Offset: 0x0010EC48
		// (remove) Token: 0x06005C9C RID: 23708 RVA: 0x00110A80 File Offset: 0x0010EC80
		public event Action onVenomFall_Roar;

		// Token: 0x06005C9D RID: 23709 RVA: 0x00110AB5 File Offset: 0x0010ECB5
		public void VenomFall_Roar()
		{
			Action action = this.onVenomFall_Roar;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000E6 RID: 230
		// (add) Token: 0x06005C9E RID: 23710 RVA: 0x00110AC8 File Offset: 0x0010ECC8
		// (remove) Token: 0x06005C9F RID: 23711 RVA: 0x00110B00 File Offset: 0x0010ED00
		public event Action onVenomFall_Fire;

		// Token: 0x06005CA0 RID: 23712 RVA: 0x00110B35 File Offset: 0x0010ED35
		public void VenomFall_Fire()
		{
			Action action = this.onVenomFall_Fire;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000E7 RID: 231
		// (add) Token: 0x06005CA1 RID: 23713 RVA: 0x00110B48 File Offset: 0x0010ED48
		// (remove) Token: 0x06005CA2 RID: 23714 RVA: 0x00110B80 File Offset: 0x0010ED80
		public event Action onVenomFall_Roar_End;

		// Token: 0x06005CA3 RID: 23715 RVA: 0x00110BB5 File Offset: 0x0010EDB5
		public void VenomFall_Roar_End()
		{
			Action action = this.onVenomFall_Roar_End;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000E8 RID: 232
		// (add) Token: 0x06005CA4 RID: 23716 RVA: 0x00110BC8 File Offset: 0x0010EDC8
		// (remove) Token: 0x06005CA5 RID: 23717 RVA: 0x00110C00 File Offset: 0x0010EE00
		public event Action onSubjectDrop_Roar_Ready;

		// Token: 0x06005CA6 RID: 23718 RVA: 0x00110C35 File Offset: 0x0010EE35
		public void SubjectDrop_Roar_Ready()
		{
			Action action = this.onSubjectDrop_Roar_Ready;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000E9 RID: 233
		// (add) Token: 0x06005CA7 RID: 23719 RVA: 0x00110C48 File Offset: 0x0010EE48
		// (remove) Token: 0x06005CA8 RID: 23720 RVA: 0x00110C80 File Offset: 0x0010EE80
		public event Action onSubjectDrop_Roar;

		// Token: 0x06005CA9 RID: 23721 RVA: 0x00110CB5 File Offset: 0x0010EEB5
		public void SubjectDrop_Roar()
		{
			Action action = this.onSubjectDrop_Roar;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000EA RID: 234
		// (add) Token: 0x06005CAA RID: 23722 RVA: 0x00110CC8 File Offset: 0x0010EEC8
		// (remove) Token: 0x06005CAB RID: 23723 RVA: 0x00110D00 File Offset: 0x0010EF00
		public event Action onSubjectDrop_Roar_End;

		// Token: 0x06005CAC RID: 23724 RVA: 0x00110D35 File Offset: 0x0010EF35
		public void SubjectDrop_Roar_End()
		{
			Action action = this.onSubjectDrop_Roar_End;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000EB RID: 235
		// (add) Token: 0x06005CAD RID: 23725 RVA: 0x00110D48 File Offset: 0x0010EF48
		// (remove) Token: 0x06005CAE RID: 23726 RVA: 0x00110D80 File Offset: 0x0010EF80
		public event Action onSubjectDrop_Fire;

		// Token: 0x06005CAF RID: 23727 RVA: 0x00110DB5 File Offset: 0x0010EFB5
		public void SubjectDrop_Fire()
		{
			Action action = this.onSubjectDrop_Fire;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000EC RID: 236
		// (add) Token: 0x06005CB0 RID: 23728 RVA: 0x00110DC8 File Offset: 0x0010EFC8
		// (remove) Token: 0x06005CB1 RID: 23729 RVA: 0x00110E00 File Offset: 0x0010F000
		public event Action onWreckDrop_Out_Ready;

		// Token: 0x06005CB2 RID: 23730 RVA: 0x00110E35 File Offset: 0x0010F035
		public void WreckDrop_Out_Ready()
		{
			Action action = this.onWreckDrop_Out_Ready;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000ED RID: 237
		// (add) Token: 0x06005CB3 RID: 23731 RVA: 0x00110E48 File Offset: 0x0010F048
		// (remove) Token: 0x06005CB4 RID: 23732 RVA: 0x00110E80 File Offset: 0x0010F080
		public event Action onWreckDrop_Out;

		// Token: 0x06005CB5 RID: 23733 RVA: 0x00110EB5 File Offset: 0x0010F0B5
		public void WreckDrop_Out()
		{
			Action action = this.onWreckDrop_Out;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000EE RID: 238
		// (add) Token: 0x06005CB6 RID: 23734 RVA: 0x00110EC8 File Offset: 0x0010F0C8
		// (remove) Token: 0x06005CB7 RID: 23735 RVA: 0x00110F00 File Offset: 0x0010F100
		public event Action onWreckDrop_In_Sign;

		// Token: 0x06005CB8 RID: 23736 RVA: 0x00110F35 File Offset: 0x0010F135
		public void WreckDrop_In_Sign()
		{
			Action action = this.onWreckDrop_In_Sign;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000EF RID: 239
		// (add) Token: 0x06005CB9 RID: 23737 RVA: 0x00110F48 File Offset: 0x0010F148
		// (remove) Token: 0x06005CBA RID: 23738 RVA: 0x00110F80 File Offset: 0x0010F180
		public event Action onWreckDrop_In_Ready;

		// Token: 0x06005CBB RID: 23739 RVA: 0x00110FB5 File Offset: 0x0010F1B5
		public void WreckDrop_In_Ready()
		{
			Action action = this.onWreckDrop_In_Ready;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000F0 RID: 240
		// (add) Token: 0x06005CBC RID: 23740 RVA: 0x00110FC8 File Offset: 0x0010F1C8
		// (remove) Token: 0x06005CBD RID: 23741 RVA: 0x00111000 File Offset: 0x0010F200
		public event Action onWreckDrop_Fire;

		// Token: 0x06005CBE RID: 23742 RVA: 0x00111035 File Offset: 0x0010F235
		public void WreckDrop_Fire()
		{
			Action action = this.onWreckDrop_Fire;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000F1 RID: 241
		// (add) Token: 0x06005CBF RID: 23743 RVA: 0x00111048 File Offset: 0x0010F248
		// (remove) Token: 0x06005CC0 RID: 23744 RVA: 0x00111080 File Offset: 0x0010F280
		public event Action oWreckDrop_In;

		// Token: 0x06005CC1 RID: 23745 RVA: 0x001110B5 File Offset: 0x0010F2B5
		public void WreckDrop_In()
		{
			Action action = this.oWreckDrop_In;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000F2 RID: 242
		// (add) Token: 0x06005CC2 RID: 23746 RVA: 0x001110C8 File Offset: 0x0010F2C8
		// (remove) Token: 0x06005CC3 RID: 23747 RVA: 0x00111100 File Offset: 0x0010F300
		public event Action onBigStomp_Ready;

		// Token: 0x06005CC4 RID: 23748 RVA: 0x00111135 File Offset: 0x0010F335
		public void BigStomp_Ready()
		{
			Action action = this.onBigStomp_Ready;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000F3 RID: 243
		// (add) Token: 0x06005CC5 RID: 23749 RVA: 0x00111148 File Offset: 0x0010F348
		// (remove) Token: 0x06005CC6 RID: 23750 RVA: 0x00111180 File Offset: 0x0010F380
		public event Action onBigStomp_Attack;

		// Token: 0x06005CC7 RID: 23751 RVA: 0x001111B5 File Offset: 0x0010F3B5
		public void BigStomp_Attack()
		{
			Action action = this.onBigStomp_Attack;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000F4 RID: 244
		// (add) Token: 0x06005CC8 RID: 23752 RVA: 0x001111C8 File Offset: 0x0010F3C8
		// (remove) Token: 0x06005CC9 RID: 23753 RVA: 0x00111200 File Offset: 0x0010F400
		public event Action onBigStomp_Hit;

		// Token: 0x06005CCA RID: 23754 RVA: 0x00111235 File Offset: 0x0010F435
		public void BigStomp_Hit()
		{
			Action action = this.onBigStomp_Hit;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000F5 RID: 245
		// (add) Token: 0x06005CCB RID: 23755 RVA: 0x00111248 File Offset: 0x0010F448
		// (remove) Token: 0x06005CCC RID: 23756 RVA: 0x00111280 File Offset: 0x0010F480
		public event Action onBigStomp_End;

		// Token: 0x06005CCD RID: 23757 RVA: 0x001112B5 File Offset: 0x0010F4B5
		public void BigStomp_End()
		{
			Action action = this.onBigStomp_End;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000F6 RID: 246
		// (add) Token: 0x06005CCE RID: 23758 RVA: 0x001112C8 File Offset: 0x0010F4C8
		// (remove) Token: 0x06005CCF RID: 23759 RVA: 0x00111300 File Offset: 0x0010F500
		public event Action onDead_Pause;

		// Token: 0x06005CD0 RID: 23760 RVA: 0x00111335 File Offset: 0x0010F535
		public void Dead_Pause()
		{
			Action action = this.onDead_Pause;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000F7 RID: 247
		// (add) Token: 0x06005CD1 RID: 23761 RVA: 0x00111348 File Offset: 0x0010F548
		// (remove) Token: 0x06005CD2 RID: 23762 RVA: 0x00111380 File Offset: 0x0010F580
		public event Action onDead_Ready;

		// Token: 0x06005CD3 RID: 23763 RVA: 0x001113B5 File Offset: 0x0010F5B5
		public void Dead_Ready()
		{
			Action action = this.onDead_Ready;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000F8 RID: 248
		// (add) Token: 0x06005CD4 RID: 23764 RVA: 0x001113C8 File Offset: 0x0010F5C8
		// (remove) Token: 0x06005CD5 RID: 23765 RVA: 0x00111400 File Offset: 0x0010F600
		public event Action onDead_Start;

		// Token: 0x06005CD6 RID: 23766 RVA: 0x00111435 File Offset: 0x0010F635
		public void Dead_Start()
		{
			Action action = this.onDead_Start;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000F9 RID: 249
		// (add) Token: 0x06005CD7 RID: 23767 RVA: 0x00111448 File Offset: 0x0010F648
		// (remove) Token: 0x06005CD8 RID: 23768 RVA: 0x00111480 File Offset: 0x0010F680
		public event Action onDead_BreakTerrain;

		// Token: 0x06005CD9 RID: 23769 RVA: 0x001114B5 File Offset: 0x0010F6B5
		public void Dead_BreakTerrain()
		{
			Action action = this.onDead_BreakTerrain;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000FA RID: 250
		// (add) Token: 0x06005CDA RID: 23770 RVA: 0x001114C8 File Offset: 0x0010F6C8
		// (remove) Token: 0x06005CDB RID: 23771 RVA: 0x00111500 File Offset: 0x0010F700
		public event Action onDead_Struggle1;

		// Token: 0x06005CDC RID: 23772 RVA: 0x00111535 File Offset: 0x0010F735
		public void Dead_Struggle1()
		{
			Action action = this.onDead_Struggle1;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000FB RID: 251
		// (add) Token: 0x06005CDD RID: 23773 RVA: 0x00111548 File Offset: 0x0010F748
		// (remove) Token: 0x06005CDE RID: 23774 RVA: 0x00111580 File Offset: 0x0010F780
		public event Action onDead_Struggle2;

		// Token: 0x06005CDF RID: 23775 RVA: 0x001115B5 File Offset: 0x0010F7B5
		public void Dead_Struggle2()
		{
			Action action = this.onDead_Struggle2;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000FC RID: 252
		// (add) Token: 0x06005CE0 RID: 23776 RVA: 0x001115C8 File Offset: 0x0010F7C8
		// (remove) Token: 0x06005CE1 RID: 23777 RVA: 0x00111600 File Offset: 0x0010F800
		public event Action onDead_Fall;

		// Token: 0x06005CE2 RID: 23778 RVA: 0x00111635 File Offset: 0x0010F835
		public void Dead_Fall()
		{
			Action action = this.onDead_Fall;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x140000FD RID: 253
		// (add) Token: 0x06005CE3 RID: 23779 RVA: 0x00111648 File Offset: 0x0010F848
		// (remove) Token: 0x06005CE4 RID: 23780 RVA: 0x00111680 File Offset: 0x0010F880
		public event Action onDead_Water;

		// Token: 0x06005CE5 RID: 23781 RVA: 0x001116B5 File Offset: 0x0010F8B5
		public void Dead_Water()
		{
			Action action = this.onDead_Water;
			if (action == null)
			{
				return;
			}
			action();
		}
	}
}
