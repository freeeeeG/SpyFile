using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace LitJson
{
	// Token: 0x0200024F RID: 591
	internal class Lexer
	{
		// Token: 0x06000AD3 RID: 2771 RVA: 0x0003A019 File Offset: 0x00038419
		static Lexer()
		{
			Lexer.PopulateFsmTables();
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x0003A020 File Offset: 0x00038420
		public Lexer(TextReader reader)
		{
			this.allow_comments = true;
			this.allow_single_quoted_strings = true;
			this.input_buffer = 0;
			this.string_buffer = new StringBuilder(128);
			this.state = 1;
			this.end_of_input = false;
			this.reader = reader;
			this.fsm_context = new FsmContext();
			this.fsm_context.L = this;
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000AD5 RID: 2773 RVA: 0x0003A084 File Offset: 0x00038484
		// (set) Token: 0x06000AD6 RID: 2774 RVA: 0x0003A08C File Offset: 0x0003848C
		public bool AllowComments
		{
			get
			{
				return this.allow_comments;
			}
			set
			{
				this.allow_comments = value;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000AD7 RID: 2775 RVA: 0x0003A095 File Offset: 0x00038495
		// (set) Token: 0x06000AD8 RID: 2776 RVA: 0x0003A09D File Offset: 0x0003849D
		public bool AllowSingleQuotedStrings
		{
			get
			{
				return this.allow_single_quoted_strings;
			}
			set
			{
				this.allow_single_quoted_strings = value;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000AD9 RID: 2777 RVA: 0x0003A0A6 File Offset: 0x000384A6
		public bool EndOfInput
		{
			get
			{
				return this.end_of_input;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000ADA RID: 2778 RVA: 0x0003A0AE File Offset: 0x000384AE
		public int Token
		{
			get
			{
				return this.token;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000ADB RID: 2779 RVA: 0x0003A0B6 File Offset: 0x000384B6
		public string StringValue
		{
			get
			{
				return this.string_value;
			}
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x0003A0C0 File Offset: 0x000384C0
		private static int HexValue(int digit)
		{
			switch (digit)
			{
			case 65:
				break;
			case 66:
				return 11;
			case 67:
				return 12;
			case 68:
				return 13;
			case 69:
				return 14;
			case 70:
				return 15;
			default:
				switch (digit)
				{
				case 97:
					break;
				case 98:
					return 11;
				case 99:
					return 12;
				case 100:
					return 13;
				case 101:
					return 14;
				case 102:
					return 15;
				default:
					return digit - 48;
				}
				break;
			}
			return 10;
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x0003A12C File Offset: 0x0003852C
		private static void PopulateFsmTables()
		{
			Lexer.StateHandler[] array = new Lexer.StateHandler[28];
			int num = 0;
			if (Lexer.<>f__mg$cache0 == null)
			{
				Lexer.<>f__mg$cache0 = new Lexer.StateHandler(Lexer.State1);
			}
			array[num] = Lexer.<>f__mg$cache0;
			int num2 = 1;
			if (Lexer.<>f__mg$cache1 == null)
			{
				Lexer.<>f__mg$cache1 = new Lexer.StateHandler(Lexer.State2);
			}
			array[num2] = Lexer.<>f__mg$cache1;
			int num3 = 2;
			if (Lexer.<>f__mg$cache2 == null)
			{
				Lexer.<>f__mg$cache2 = new Lexer.StateHandler(Lexer.State3);
			}
			array[num3] = Lexer.<>f__mg$cache2;
			int num4 = 3;
			if (Lexer.<>f__mg$cache3 == null)
			{
				Lexer.<>f__mg$cache3 = new Lexer.StateHandler(Lexer.State4);
			}
			array[num4] = Lexer.<>f__mg$cache3;
			int num5 = 4;
			if (Lexer.<>f__mg$cache4 == null)
			{
				Lexer.<>f__mg$cache4 = new Lexer.StateHandler(Lexer.State5);
			}
			array[num5] = Lexer.<>f__mg$cache4;
			int num6 = 5;
			if (Lexer.<>f__mg$cache5 == null)
			{
				Lexer.<>f__mg$cache5 = new Lexer.StateHandler(Lexer.State6);
			}
			array[num6] = Lexer.<>f__mg$cache5;
			int num7 = 6;
			if (Lexer.<>f__mg$cache6 == null)
			{
				Lexer.<>f__mg$cache6 = new Lexer.StateHandler(Lexer.State7);
			}
			array[num7] = Lexer.<>f__mg$cache6;
			int num8 = 7;
			if (Lexer.<>f__mg$cache7 == null)
			{
				Lexer.<>f__mg$cache7 = new Lexer.StateHandler(Lexer.State8);
			}
			array[num8] = Lexer.<>f__mg$cache7;
			int num9 = 8;
			if (Lexer.<>f__mg$cache8 == null)
			{
				Lexer.<>f__mg$cache8 = new Lexer.StateHandler(Lexer.State9);
			}
			array[num9] = Lexer.<>f__mg$cache8;
			int num10 = 9;
			if (Lexer.<>f__mg$cache9 == null)
			{
				Lexer.<>f__mg$cache9 = new Lexer.StateHandler(Lexer.State10);
			}
			array[num10] = Lexer.<>f__mg$cache9;
			int num11 = 10;
			if (Lexer.<>f__mg$cacheA == null)
			{
				Lexer.<>f__mg$cacheA = new Lexer.StateHandler(Lexer.State11);
			}
			array[num11] = Lexer.<>f__mg$cacheA;
			int num12 = 11;
			if (Lexer.<>f__mg$cacheB == null)
			{
				Lexer.<>f__mg$cacheB = new Lexer.StateHandler(Lexer.State12);
			}
			array[num12] = Lexer.<>f__mg$cacheB;
			int num13 = 12;
			if (Lexer.<>f__mg$cacheC == null)
			{
				Lexer.<>f__mg$cacheC = new Lexer.StateHandler(Lexer.State13);
			}
			array[num13] = Lexer.<>f__mg$cacheC;
			int num14 = 13;
			if (Lexer.<>f__mg$cacheD == null)
			{
				Lexer.<>f__mg$cacheD = new Lexer.StateHandler(Lexer.State14);
			}
			array[num14] = Lexer.<>f__mg$cacheD;
			int num15 = 14;
			if (Lexer.<>f__mg$cacheE == null)
			{
				Lexer.<>f__mg$cacheE = new Lexer.StateHandler(Lexer.State15);
			}
			array[num15] = Lexer.<>f__mg$cacheE;
			int num16 = 15;
			if (Lexer.<>f__mg$cacheF == null)
			{
				Lexer.<>f__mg$cacheF = new Lexer.StateHandler(Lexer.State16);
			}
			array[num16] = Lexer.<>f__mg$cacheF;
			int num17 = 16;
			if (Lexer.<>f__mg$cache10 == null)
			{
				Lexer.<>f__mg$cache10 = new Lexer.StateHandler(Lexer.State17);
			}
			array[num17] = Lexer.<>f__mg$cache10;
			int num18 = 17;
			if (Lexer.<>f__mg$cache11 == null)
			{
				Lexer.<>f__mg$cache11 = new Lexer.StateHandler(Lexer.State18);
			}
			array[num18] = Lexer.<>f__mg$cache11;
			int num19 = 18;
			if (Lexer.<>f__mg$cache12 == null)
			{
				Lexer.<>f__mg$cache12 = new Lexer.StateHandler(Lexer.State19);
			}
			array[num19] = Lexer.<>f__mg$cache12;
			int num20 = 19;
			if (Lexer.<>f__mg$cache13 == null)
			{
				Lexer.<>f__mg$cache13 = new Lexer.StateHandler(Lexer.State20);
			}
			array[num20] = Lexer.<>f__mg$cache13;
			int num21 = 20;
			if (Lexer.<>f__mg$cache14 == null)
			{
				Lexer.<>f__mg$cache14 = new Lexer.StateHandler(Lexer.State21);
			}
			array[num21] = Lexer.<>f__mg$cache14;
			int num22 = 21;
			if (Lexer.<>f__mg$cache15 == null)
			{
				Lexer.<>f__mg$cache15 = new Lexer.StateHandler(Lexer.State22);
			}
			array[num22] = Lexer.<>f__mg$cache15;
			int num23 = 22;
			if (Lexer.<>f__mg$cache16 == null)
			{
				Lexer.<>f__mg$cache16 = new Lexer.StateHandler(Lexer.State23);
			}
			array[num23] = Lexer.<>f__mg$cache16;
			int num24 = 23;
			if (Lexer.<>f__mg$cache17 == null)
			{
				Lexer.<>f__mg$cache17 = new Lexer.StateHandler(Lexer.State24);
			}
			array[num24] = Lexer.<>f__mg$cache17;
			int num25 = 24;
			if (Lexer.<>f__mg$cache18 == null)
			{
				Lexer.<>f__mg$cache18 = new Lexer.StateHandler(Lexer.State25);
			}
			array[num25] = Lexer.<>f__mg$cache18;
			int num26 = 25;
			if (Lexer.<>f__mg$cache19 == null)
			{
				Lexer.<>f__mg$cache19 = new Lexer.StateHandler(Lexer.State26);
			}
			array[num26] = Lexer.<>f__mg$cache19;
			int num27 = 26;
			if (Lexer.<>f__mg$cache1A == null)
			{
				Lexer.<>f__mg$cache1A = new Lexer.StateHandler(Lexer.State27);
			}
			array[num27] = Lexer.<>f__mg$cache1A;
			int num28 = 27;
			if (Lexer.<>f__mg$cache1B == null)
			{
				Lexer.<>f__mg$cache1B = new Lexer.StateHandler(Lexer.State28);
			}
			array[num28] = Lexer.<>f__mg$cache1B;
			Lexer.fsm_handler_table = array;
			Lexer.fsm_return_table = new int[]
			{
				65542,
				0,
				65537,
				65537,
				0,
				65537,
				0,
				65537,
				0,
				0,
				65538,
				0,
				0,
				0,
				65539,
				0,
				0,
				65540,
				65541,
				65542,
				0,
				0,
				65541,
				65542,
				0,
				0,
				0,
				0
			};
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x0003A4F0 File Offset: 0x000388F0
		private static char ProcessEscChar(int esc_char)
		{
			switch (esc_char)
			{
			case 114:
				return '\r';
			default:
				if (esc_char == 34 || esc_char == 39 || esc_char == 47 || esc_char == 92)
				{
					return Convert.ToChar(esc_char);
				}
				if (esc_char == 98)
				{
					return '\b';
				}
				if (esc_char == 102)
				{
					return '\f';
				}
				if (esc_char != 110)
				{
					return '?';
				}
				return '\n';
			case 116:
				return '\t';
			}
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x0003A568 File Offset: 0x00038968
		private static bool State1(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				if (ctx.L.input_char != 32 && (ctx.L.input_char < 9 || ctx.L.input_char > 13))
				{
					if (ctx.L.input_char >= 49 && ctx.L.input_char <= 57)
					{
						ctx.L.string_buffer.Append((char)ctx.L.input_char);
						ctx.NextState = 3;
						return true;
					}
					int num = ctx.L.input_char;
					switch (num)
					{
					case 44:
						break;
					case 45:
						ctx.L.string_buffer.Append((char)ctx.L.input_char);
						ctx.NextState = 2;
						return true;
					default:
						switch (num)
						{
						case 91:
						case 93:
							break;
						default:
							switch (num)
							{
							case 123:
							case 125:
								break;
							default:
								if (num == 34)
								{
									ctx.NextState = 19;
									ctx.Return = true;
									return true;
								}
								if (num != 39)
								{
									if (num != 58)
									{
										if (num == 102)
										{
											ctx.NextState = 12;
											return true;
										}
										if (num == 110)
										{
											ctx.NextState = 16;
											return true;
										}
										if (num != 116)
										{
											return false;
										}
										ctx.NextState = 9;
										return true;
									}
								}
								else
								{
									if (!ctx.L.allow_single_quoted_strings)
									{
										return false;
									}
									ctx.L.input_char = 34;
									ctx.NextState = 23;
									ctx.Return = true;
									return true;
								}
								break;
							}
							break;
						}
						break;
					case 47:
						if (!ctx.L.allow_comments)
						{
							return false;
						}
						ctx.NextState = 25;
						return true;
					case 48:
						ctx.L.string_buffer.Append((char)ctx.L.input_char);
						ctx.NextState = 4;
						return true;
					}
					ctx.NextState = 1;
					ctx.Return = true;
					return true;
				}
			}
			return true;
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x0003A774 File Offset: 0x00038B74
		private static bool State2(FsmContext ctx)
		{
			ctx.L.GetChar();
			if (ctx.L.input_char >= 49 && ctx.L.input_char <= 57)
			{
				ctx.L.string_buffer.Append((char)ctx.L.input_char);
				ctx.NextState = 3;
				return true;
			}
			int num = ctx.L.input_char;
			if (num != 48)
			{
				return false;
			}
			ctx.L.string_buffer.Append((char)ctx.L.input_char);
			ctx.NextState = 4;
			return true;
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x0003A818 File Offset: 0x00038C18
		private static bool State3(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				if (ctx.L.input_char >= 48 && ctx.L.input_char <= 57)
				{
					ctx.L.string_buffer.Append((char)ctx.L.input_char);
				}
				else
				{
					if (ctx.L.input_char == 32 || (ctx.L.input_char >= 9 && ctx.L.input_char <= 13))
					{
						ctx.Return = true;
						ctx.NextState = 1;
						return true;
					}
					int num = ctx.L.input_char;
					switch (num)
					{
					case 44:
						break;
					default:
						if (num != 69)
						{
							if (num == 93)
							{
								break;
							}
							if (num != 101)
							{
								if (num != 125)
								{
									return false;
								}
								break;
							}
						}
						ctx.L.string_buffer.Append((char)ctx.L.input_char);
						ctx.NextState = 7;
						return true;
					case 46:
						ctx.L.string_buffer.Append((char)ctx.L.input_char);
						ctx.NextState = 5;
						return true;
					}
					ctx.L.UngetChar();
					ctx.Return = true;
					ctx.NextState = 1;
					return true;
				}
			}
			return true;
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x0003A978 File Offset: 0x00038D78
		private static bool State4(FsmContext ctx)
		{
			ctx.L.GetChar();
			if (ctx.L.input_char == 32 || (ctx.L.input_char >= 9 && ctx.L.input_char <= 13))
			{
				ctx.Return = true;
				ctx.NextState = 1;
				return true;
			}
			int num = ctx.L.input_char;
			switch (num)
			{
			case 44:
				break;
			default:
				if (num != 69)
				{
					if (num == 93)
					{
						break;
					}
					if (num != 101)
					{
						if (num != 125)
						{
							return false;
						}
						break;
					}
				}
				ctx.L.string_buffer.Append((char)ctx.L.input_char);
				ctx.NextState = 7;
				return true;
			case 46:
				ctx.L.string_buffer.Append((char)ctx.L.input_char);
				ctx.NextState = 5;
				return true;
			}
			ctx.L.UngetChar();
			ctx.Return = true;
			ctx.NextState = 1;
			return true;
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x0003AA88 File Offset: 0x00038E88
		private static bool State5(FsmContext ctx)
		{
			ctx.L.GetChar();
			if (ctx.L.input_char >= 48 && ctx.L.input_char <= 57)
			{
				ctx.L.string_buffer.Append((char)ctx.L.input_char);
				ctx.NextState = 6;
				return true;
			}
			return false;
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x0003AAEC File Offset: 0x00038EEC
		private static bool State6(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				if (ctx.L.input_char >= 48 && ctx.L.input_char <= 57)
				{
					ctx.L.string_buffer.Append((char)ctx.L.input_char);
				}
				else
				{
					if (ctx.L.input_char == 32 || (ctx.L.input_char >= 9 && ctx.L.input_char <= 13))
					{
						ctx.Return = true;
						ctx.NextState = 1;
						return true;
					}
					int num = ctx.L.input_char;
					if (num != 44)
					{
						if (num != 69)
						{
							if (num == 93)
							{
								goto IL_CA;
							}
							if (num != 101)
							{
								if (num != 125)
								{
									return false;
								}
								goto IL_CA;
							}
						}
						ctx.L.string_buffer.Append((char)ctx.L.input_char);
						ctx.NextState = 7;
						return true;
					}
					IL_CA:
					ctx.L.UngetChar();
					ctx.Return = true;
					ctx.NextState = 1;
					return true;
				}
			}
			return true;
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x0003AC18 File Offset: 0x00039018
		private static bool State7(FsmContext ctx)
		{
			ctx.L.GetChar();
			if (ctx.L.input_char >= 48 && ctx.L.input_char <= 57)
			{
				ctx.L.string_buffer.Append((char)ctx.L.input_char);
				ctx.NextState = 8;
				return true;
			}
			int num = ctx.L.input_char;
			if (num != 43 && num != 45)
			{
				return false;
			}
			ctx.L.string_buffer.Append((char)ctx.L.input_char);
			ctx.NextState = 8;
			return true;
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x0003ACC4 File Offset: 0x000390C4
		private static bool State8(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				if (ctx.L.input_char >= 48 && ctx.L.input_char <= 57)
				{
					ctx.L.string_buffer.Append((char)ctx.L.input_char);
				}
				else
				{
					if (ctx.L.input_char == 32 || (ctx.L.input_char >= 9 && ctx.L.input_char <= 13))
					{
						ctx.Return = true;
						ctx.NextState = 1;
						return true;
					}
					int num = ctx.L.input_char;
					if (num != 44 && num != 93 && num != 125)
					{
						return false;
					}
					ctx.L.UngetChar();
					ctx.Return = true;
					ctx.NextState = 1;
					return true;
				}
			}
			return true;
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x0003ADBC File Offset: 0x000391BC
		private static bool State9(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 114)
			{
				return false;
			}
			ctx.NextState = 10;
			return true;
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x0003ADFC File Offset: 0x000391FC
		private static bool State10(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 117)
			{
				return false;
			}
			ctx.NextState = 11;
			return true;
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x0003AE3C File Offset: 0x0003923C
		private static bool State11(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 101)
			{
				return false;
			}
			ctx.Return = true;
			ctx.NextState = 1;
			return true;
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x0003AE80 File Offset: 0x00039280
		private static bool State12(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 97)
			{
				return false;
			}
			ctx.NextState = 13;
			return true;
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x0003AEC0 File Offset: 0x000392C0
		private static bool State13(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 108)
			{
				return false;
			}
			ctx.NextState = 14;
			return true;
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x0003AF00 File Offset: 0x00039300
		private static bool State14(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 115)
			{
				return false;
			}
			ctx.NextState = 15;
			return true;
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x0003AF40 File Offset: 0x00039340
		private static bool State15(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 101)
			{
				return false;
			}
			ctx.Return = true;
			ctx.NextState = 1;
			return true;
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x0003AF84 File Offset: 0x00039384
		private static bool State16(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 117)
			{
				return false;
			}
			ctx.NextState = 17;
			return true;
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x0003AFC4 File Offset: 0x000393C4
		private static bool State17(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 108)
			{
				return false;
			}
			ctx.NextState = 18;
			return true;
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x0003B004 File Offset: 0x00039404
		private static bool State18(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 108)
			{
				return false;
			}
			ctx.Return = true;
			ctx.NextState = 1;
			return true;
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x0003B048 File Offset: 0x00039448
		private static bool State19(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				int num = ctx.L.input_char;
				if (num == 34)
				{
					ctx.L.UngetChar();
					ctx.Return = true;
					ctx.NextState = 20;
					return true;
				}
				if (num == 92)
				{
					ctx.StateStack = 19;
					ctx.NextState = 21;
					return true;
				}
				ctx.L.string_buffer.Append((char)ctx.L.input_char);
			}
			return true;
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x0003B0DC File Offset: 0x000394DC
		private static bool State20(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 34)
			{
				return false;
			}
			ctx.Return = true;
			ctx.NextState = 1;
			return true;
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x0003B120 File Offset: 0x00039520
		private static bool State21(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			switch (num)
			{
			case 114:
			case 116:
				break;
			default:
				if (num != 34 && num != 39 && num != 47 && num != 92 && num != 98 && num != 102 && num != 110)
				{
					return false;
				}
				break;
			case 117:
				ctx.NextState = 22;
				return true;
			}
			ctx.L.string_buffer.Append(Lexer.ProcessEscChar(ctx.L.input_char));
			ctx.NextState = ctx.StateStack;
			return true;
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x0003B1D8 File Offset: 0x000395D8
		private static bool State22(FsmContext ctx)
		{
			int num = 0;
			int num2 = 4096;
			ctx.L.unichar = 0;
			while (ctx.L.GetChar())
			{
				if ((ctx.L.input_char < 48 || ctx.L.input_char > 57) && (ctx.L.input_char < 65 || ctx.L.input_char > 70) && (ctx.L.input_char < 97 || ctx.L.input_char > 102))
				{
					return false;
				}
				ctx.L.unichar += Lexer.HexValue(ctx.L.input_char) * num2;
				num++;
				num2 /= 16;
				if (num == 4)
				{
					ctx.L.string_buffer.Append(Convert.ToChar(ctx.L.unichar));
					ctx.NextState = ctx.StateStack;
					return true;
				}
			}
			return true;
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x0003B2E8 File Offset: 0x000396E8
		private static bool State23(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				int num = ctx.L.input_char;
				if (num == 39)
				{
					ctx.L.UngetChar();
					ctx.Return = true;
					ctx.NextState = 24;
					return true;
				}
				if (num == 92)
				{
					ctx.StateStack = 23;
					ctx.NextState = 21;
					return true;
				}
				ctx.L.string_buffer.Append((char)ctx.L.input_char);
			}
			return true;
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x0003B37C File Offset: 0x0003977C
		private static bool State24(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 39)
			{
				return false;
			}
			ctx.L.input_char = 34;
			ctx.Return = true;
			ctx.NextState = 1;
			return true;
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x0003B3CC File Offset: 0x000397CC
		private static bool State25(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num == 42)
			{
				ctx.NextState = 27;
				return true;
			}
			if (num != 47)
			{
				return false;
			}
			ctx.NextState = 26;
			return true;
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x0003B41B File Offset: 0x0003981B
		private static bool State26(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				if (ctx.L.input_char == 10)
				{
					ctx.NextState = 1;
					return true;
				}
			}
			return true;
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x0003B44E File Offset: 0x0003984E
		private static bool State27(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				if (ctx.L.input_char == 42)
				{
					ctx.NextState = 28;
					return true;
				}
			}
			return true;
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x0003B484 File Offset: 0x00039884
		private static bool State28(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				if (ctx.L.input_char != 42)
				{
					if (ctx.L.input_char == 47)
					{
						ctx.NextState = 1;
						return true;
					}
					ctx.NextState = 27;
					return true;
				}
			}
			return true;
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x0003B4E4 File Offset: 0x000398E4
		private bool GetChar()
		{
			if ((this.input_char = this.NextChar()) != -1)
			{
				return true;
			}
			this.end_of_input = true;
			return false;
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x0003B510 File Offset: 0x00039910
		private int NextChar()
		{
			if (this.input_buffer != 0)
			{
				int result = this.input_buffer;
				this.input_buffer = 0;
				return result;
			}
			return this.reader.Read();
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x0003B544 File Offset: 0x00039944
		public bool NextToken()
		{
			this.fsm_context.Return = false;
			for (;;)
			{
				Lexer.StateHandler stateHandler = Lexer.fsm_handler_table[this.state - 1];
				if (!stateHandler(this.fsm_context))
				{
					break;
				}
				if (this.end_of_input)
				{
					return false;
				}
				if (this.fsm_context.Return)
				{
					goto Block_3;
				}
				this.state = this.fsm_context.NextState;
			}
			throw new JsonException(this.input_char);
			Block_3:
			this.string_value = this.string_buffer.ToString();
			this.string_buffer.Remove(0, this.string_buffer.Length);
			this.token = Lexer.fsm_return_table[this.state - 1];
			if (this.token == 65542)
			{
				this.token = this.input_char;
			}
			this.state = this.fsm_context.NextState;
			return true;
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x0003B627 File Offset: 0x00039A27
		private void UngetChar()
		{
			this.input_buffer = this.input_char;
		}

		// Token: 0x04000869 RID: 2153
		private static int[] fsm_return_table;

		// Token: 0x0400086A RID: 2154
		private static Lexer.StateHandler[] fsm_handler_table;

		// Token: 0x0400086B RID: 2155
		private bool allow_comments;

		// Token: 0x0400086C RID: 2156
		private bool allow_single_quoted_strings;

		// Token: 0x0400086D RID: 2157
		private bool end_of_input;

		// Token: 0x0400086E RID: 2158
		private FsmContext fsm_context;

		// Token: 0x0400086F RID: 2159
		private int input_buffer;

		// Token: 0x04000870 RID: 2160
		private int input_char;

		// Token: 0x04000871 RID: 2161
		private TextReader reader;

		// Token: 0x04000872 RID: 2162
		private int state;

		// Token: 0x04000873 RID: 2163
		private StringBuilder string_buffer;

		// Token: 0x04000874 RID: 2164
		private string string_value;

		// Token: 0x04000875 RID: 2165
		private int token;

		// Token: 0x04000876 RID: 2166
		private int unichar;

		// Token: 0x04000877 RID: 2167
		[CompilerGenerated]
		private static Lexer.StateHandler <>f__mg$cache0;

		// Token: 0x04000878 RID: 2168
		[CompilerGenerated]
		private static Lexer.StateHandler <>f__mg$cache1;

		// Token: 0x04000879 RID: 2169
		[CompilerGenerated]
		private static Lexer.StateHandler <>f__mg$cache2;

		// Token: 0x0400087A RID: 2170
		[CompilerGenerated]
		private static Lexer.StateHandler <>f__mg$cache3;

		// Token: 0x0400087B RID: 2171
		[CompilerGenerated]
		private static Lexer.StateHandler <>f__mg$cache4;

		// Token: 0x0400087C RID: 2172
		[CompilerGenerated]
		private static Lexer.StateHandler <>f__mg$cache5;

		// Token: 0x0400087D RID: 2173
		[CompilerGenerated]
		private static Lexer.StateHandler <>f__mg$cache6;

		// Token: 0x0400087E RID: 2174
		[CompilerGenerated]
		private static Lexer.StateHandler <>f__mg$cache7;

		// Token: 0x0400087F RID: 2175
		[CompilerGenerated]
		private static Lexer.StateHandler <>f__mg$cache8;

		// Token: 0x04000880 RID: 2176
		[CompilerGenerated]
		private static Lexer.StateHandler <>f__mg$cache9;

		// Token: 0x04000881 RID: 2177
		[CompilerGenerated]
		private static Lexer.StateHandler <>f__mg$cacheA;

		// Token: 0x04000882 RID: 2178
		[CompilerGenerated]
		private static Lexer.StateHandler <>f__mg$cacheB;

		// Token: 0x04000883 RID: 2179
		[CompilerGenerated]
		private static Lexer.StateHandler <>f__mg$cacheC;

		// Token: 0x04000884 RID: 2180
		[CompilerGenerated]
		private static Lexer.StateHandler <>f__mg$cacheD;

		// Token: 0x04000885 RID: 2181
		[CompilerGenerated]
		private static Lexer.StateHandler <>f__mg$cacheE;

		// Token: 0x04000886 RID: 2182
		[CompilerGenerated]
		private static Lexer.StateHandler <>f__mg$cacheF;

		// Token: 0x04000887 RID: 2183
		[CompilerGenerated]
		private static Lexer.StateHandler <>f__mg$cache10;

		// Token: 0x04000888 RID: 2184
		[CompilerGenerated]
		private static Lexer.StateHandler <>f__mg$cache11;

		// Token: 0x04000889 RID: 2185
		[CompilerGenerated]
		private static Lexer.StateHandler <>f__mg$cache12;

		// Token: 0x0400088A RID: 2186
		[CompilerGenerated]
		private static Lexer.StateHandler <>f__mg$cache13;

		// Token: 0x0400088B RID: 2187
		[CompilerGenerated]
		private static Lexer.StateHandler <>f__mg$cache14;

		// Token: 0x0400088C RID: 2188
		[CompilerGenerated]
		private static Lexer.StateHandler <>f__mg$cache15;

		// Token: 0x0400088D RID: 2189
		[CompilerGenerated]
		private static Lexer.StateHandler <>f__mg$cache16;

		// Token: 0x0400088E RID: 2190
		[CompilerGenerated]
		private static Lexer.StateHandler <>f__mg$cache17;

		// Token: 0x0400088F RID: 2191
		[CompilerGenerated]
		private static Lexer.StateHandler <>f__mg$cache18;

		// Token: 0x04000890 RID: 2192
		[CompilerGenerated]
		private static Lexer.StateHandler <>f__mg$cache19;

		// Token: 0x04000891 RID: 2193
		[CompilerGenerated]
		private static Lexer.StateHandler <>f__mg$cache1A;

		// Token: 0x04000892 RID: 2194
		[CompilerGenerated]
		private static Lexer.StateHandler <>f__mg$cache1B;

		// Token: 0x02000250 RID: 592
		// (Invoke) Token: 0x06000B00 RID: 2816
		private delegate bool StateHandler(FsmContext ctx);
	}
}
