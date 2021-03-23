


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static FM_VirtualMultiTrackMML.OPNA;
using static FM_VirtualMultiTrackMML.Util;



namespace FM_VirtualMultiTrackMML
{
	namespace ForMucom88
	{
		public class Player
		{
			class Track {
				public Dictionary<int, Timbre> maTimbre;
				Sequence mSequence;
				
				public bool mb1st = true;
				public bool mbSlur = false;
				public int mClock = 1;
				public int mScale = -1;
				public int mRelativeVolume = 0;
				public Volume mAbsoluteVolume = new Volume();
				public int mDetune = 0;
				public int mStaccato = 0;
				public Timbre mTimbre = new Timbre();
				
				public int mReverv = 0;				// todo
				public bool mbReverv = false;		// todo
				public bool mbRevervMode = false;	// todo
				
				public bool mbLoop = false;
				
				public Register mRegister = new Register();
				
				
				
				public Track(Dictionary<int, Timbre> aTimbre, List<Delivery.Part> aPart)
				{
					maTimbre = aTimbre;
					mSequence = new Sequence(aPart);
				}
				
				
				
				public bool IsTerm
				{
					get { return mSequence.IsTerm; }
				}
				
				
				
				public bool Peek(out Packet @Packet)
				{
					return mSequence.Peek(out Packet);
				}
				
				
				
				public CallstackLogger.Logger Logger(int oLogger)
				{
					return mSequence.Part.CallstackLogger[oLogger];
				}
				
				
				
				public bool Tick(ref StringBuilder Queue, ref int Clock, int Channel, int oTrack, bool[] aKeyOn, out bool bScale, ref Register.Param KeyOn)
				{
					var oTrackSelf = oTrack;
					var oTrackPair = oTrack ^ 1;
					
					bScale = false;
					
					bool Result = true;
					if (!IsTerm){
						--mClock;
						if ((aKeyOn[oTrackSelf] || mb1st) && mClock <= mStaccato && !mbSlur){
							Duration(ref Queue, ref Clock, false);
							
							aKeyOn[oTrackSelf] = false;
							AppendKeyOn(ref Queue, Channel, aKeyOn, oTrackSelf, oTrackPair, ref KeyOn);
							
							mb1st = false;
						}
						if (mClock == 0){
							bool bBreak = false;
							while (!IsTerm && !bBreak){
								Packet Packet;
								if (mSequence.Pop(out Packet)){
									if (Packet.IsScale){
										Duration(ref Queue, ref Clock, false);
										
										mScale = Packet.Scale;
										mbSlur = Packet.IsSlur;
										mClock = (Packet.Value > 0)? Packet.Value: 0x100;
										if (!mbSlur){
											aKeyOn[oTrackSelf] = true;
											AppendKeyOn(ref Queue, Channel, aKeyOn, oTrackSelf, oTrackPair, ref KeyOn);
										}
										bScale = true;
										bBreak = true;
									} else {
										switch ((int)Packet.CommandEnum){
											case (int)Packet.eCommand.r:{
												mbSlur = false;
												mClock = (Packet.Value > 0)? Packet.Value: 0x100;
												
												aKeyOn[oTrackSelf] = false;
												AppendKeyOn(ref Queue, Channel, aKeyOn, oTrackSelf, oTrackPair, ref KeyOn);
												bBreak = true;
												break;
											}
											case (int)Packet.eCommand.q:{
												mStaccato = Packet.Value;
												break;
											}
											case (int)Packet.eCommand.t:{
												Queue.Append($"t{Packet.Value} ");
												break;
											}
											case (int)Packet.eCommand.T:{
												Queue.Append($"T{Packet.Value} ");
												break;
											}
											case (int)Packet.eCommand.DL:{
												mDetune &= ~0xff;
												mDetune |= Packet.Value;
												break;
											}
											case (int)Packet.eCommand.DH:{
												mDetune &= 0xff;
												mDetune |= Packet.ValueInt<<8;
												break;
											}
											case (int)Packet.eCommand.Timbre:{
												var oTimbre = Packet.Value;
												var Params = maTimbre[oTimbre].Params;
												mRegister.FB.Value = Params.FB;
												mRegister.aOP[0].AR.Value = Params.OP1.AR;
												mRegister.aOP[0].DR.Value = Params.OP1.DR;
												mRegister.aOP[0].SR.Value = Params.OP1.SR;
												mRegister.aOP[0].RR.Value = Params.OP1.RR;
												mRegister.aOP[0].SL.Value = Params.OP1.SL;
												mRegister.aOP[0].TL.Value = Params.OP1.TL;
												mRegister.aOP[0].KS.Value = Params.OP1.KS;
												mRegister.aOP[0].SE.Value = Params.OP1.SE;
												mRegister.aOP[1].AR.Value = Params.OP2.AR;
												mRegister.aOP[1].DR.Value = Params.OP2.DR;
												mRegister.aOP[1].SR.Value = Params.OP2.SR;
												mRegister.aOP[1].RR.Value = Params.OP2.RR;
												mRegister.aOP[1].SL.Value = Params.OP2.SL;
												mRegister.aOP[1].TL.Value = s_aValue_TL[mAbsoluteVolume.Clamp];
												mRegister.aOP[1].KS.Value = Params.OP2.KS;
												mRegister.aOP[1].SE.Value = Params.OP2.SE;
												break;
											}
											case (int)Packet.eCommand.R:{
												mReverv = Packet.Value;
												break;
											}
											case (int)Packet.eCommand.RF:{
												mbReverv = (Packet.Value > 0);
												break;
											}
											case (int)Packet.eCommand.Rm:{
												mbRevervMode = (Packet.Value > 0);
												break;
											}
											case (int)Packet.eCommand.pM:{
												mRegister.LR.Value = 0;
												Queue.Append($"p0 ");
												break;
											}
											case (int)Packet.eCommand.pR:{
												mRegister.LR.Value = 1;
												Queue.Append($"p1 ");
												break;
											}
											case (int)Packet.eCommand.pL:{
												mRegister.LR.Value = 2;
												Queue.Append($"p2 ");
												break;
											}
											case (int)Packet.eCommand.pC:{
												mRegister.LR.Value = 3;
												Queue.Append($"p3 ");
												break;
											}
											case (int)Packet.eCommand.J:{
												Queue.Append($"J ");
												break;
											}
											case (int)Packet.eCommand.L:{
												if (oTrack == 0){
													Duration(ref Queue, ref Clock, false);
													Queue.Append($"L ");
													mbLoop = true;
												}
												break;
											}
											case (int)Packet.eCommand.yFB:{
												mRegister.FB.Value = Packet.Value;
												break;
											}
											
											case (int)Packet.eCommand.yAR1:{
												mRegister.aOP[0].AR.Value = Packet.Value;
												break;
											}
											case (int)Packet.eCommand.yDR1:{
												mRegister.aOP[0].DR.Value = Packet.Value;
												break;
											}
											case (int)Packet.eCommand.ySR1:{
												mRegister.aOP[0].SR.Value = Packet.Value;
												break;
											}
											case (int)Packet.eCommand.yRR1:{
												mRegister.aOP[0].RR.Value = Packet.Value;
												break;
											}
											case (int)Packet.eCommand.ySL1:{
												mRegister.aOP[0].SL.Value = Packet.Value;
												break;
											}
											case (int)Packet.eCommand.yTL1:{
												mRegister.aOP[0].TL.Value = Packet.Value;
												break;
											}
											case (int)Packet.eCommand.yKS1:{
												mRegister.aOP[0].KS.Value = Packet.Value;
												break;
											}
											case (int)Packet.eCommand.ySE1:{
												mRegister.aOP[0].SE.Value = Packet.Value;
												break;
											}
											case (int)Packet.eCommand.yAR2:{
												mRegister.aOP[1].AR.Value = Packet.Value;
												break;
											}
											case (int)Packet.eCommand.yDR2:{
												mRegister.aOP[1].DR.Value = Packet.Value;
												break;
											}
											case (int)Packet.eCommand.ySR2:{
												mRegister.aOP[1].SR.Value = Packet.Value;
												break;
											}
											case (int)Packet.eCommand.yRR2:{
												mRegister.aOP[1].RR.Value = Packet.Value;
												break;
											}
											case (int)Packet.eCommand.ySL2:{
												mRegister.aOP[1].SL.Value = Packet.Value;
												break;
											}
											case (int)Packet.eCommand.yTL2:{
												mRegister.aOP[1].TL.Value = Packet.Value;
												break;
											}
											case (int)Packet.eCommand.yKS2:{
												mRegister.aOP[1].KS.Value = Packet.Value;
												break;
											}
											case (int)Packet.eCommand.ySE2:{
												mRegister.aOP[1].SE.Value = Packet.Value;
												break;
											}
											
											case (int)Packet.eCommand.v:{
												mAbsoluteVolume.Value = Packet.Value + mRelativeVolume;
												mRegister.aOP[1].TL.Value = s_aValue_TL[mAbsoluteVolume.Clamp];
												break;
											}
											case (int)Packet.eCommand.V:{
												mRelativeVolume = Packet.ValueInt;
												break;
											}
											case (int)Packet.eCommand.vUp:{
												mAbsoluteVolume.Value += Packet.Value;
												mRegister.aOP[1].TL.Value = s_aValue_TL[mAbsoluteVolume.Clamp];
												break;
											}
											case (int)Packet.eCommand.vDown:{
												mAbsoluteVolume.Value -= Packet.Value;
												mRegister.aOP[1].TL.Value = s_aValue_TL[mAbsoluteVolume.Clamp];
												break;
											}
										}
									}
								}
							}
						}
					}
					
					if (Result){
						if (mRegister.FB.IsModified && oTrack == 0){
							var Register = OPNA.s_Reg_FB_AL + (Channel%3);
							var Value = OPNA.s_Value_AL | (mRegister.FB.Value << 3);
							Append(ref Queue, Register, Value);
						}
						
						int oOP = 0;
						foreach (var OP in mRegister.aOP){
							if (OP.TL.IsModified){
								var Register = OPNA.s_aaReg_TL[oTrackSelf][oOP] + (Channel%3);
								var Value = OP.TL.Value;
								Append(ref Queue, Register, Value);
							}
							if (OP.AR.IsModified || OP.KS.IsModified){
								var Register = OPNA.s_aaReg_KS_AR[oTrackSelf][oOP] + (Channel%3);
								var Value = OP.AR.Value | (OP.KS.Value << 6);
								Append(ref Queue, Register, Value);
							}
							if (OP.DR.IsModified){
								var Register = OPNA.s_aaReg_AM_DR[oTrackSelf][oOP] + (Channel%3);
								var Value = OP.DR.Value;
								Append(ref Queue, Register, Value);
							}
							if (OP.SR.IsModified){
								var Register = OPNA.s_aaReg_SR[oTrackSelf][oOP] + (Channel%3);
								var Value = OP.SR.Value;
								Append(ref Queue, Register, Value);
							}
							if (OP.RR.IsModified || OP.SL.IsModified){
								var Register = OPNA.s_aaReg_SL_RR[oTrackSelf][oOP] + (Channel%3);
								var Value = OP.RR.Value | (OP.SL.Value << 4);
								Append(ref Queue, Register, Value);
							}
							if (OP.SE.IsModified){
								var Register = OPNA.s_aaReg_SSGEG[oTrackSelf][oOP] + (Channel%3);
								var Value = OP.SE.Value;
								Append(ref Queue, Register, Value);
							}
							++oOP;
						}
						
						#if UNSUPPORTED//[
						// MUCOM88 cannot access $b4 register
						if (mRegister.PMS.IsModified || mRegister.AMS.IsModified || mRegister.LR.IsModified){
							var Register = OPNA.s_Reg_LR_AMS_PMS + (Channel%3);
							var Value = mRegister.PMS.Value | (mRegister.AMS.Value << 4) | (mRegister.LR.Value << 6);
							Append(ref Queue, Register, Value);
						}
						#endif//]
					}
					return Result;
				}
				
				
				
				void AppendKeyOn(ref StringBuilder Queue, int Channel, bool[] aKeyOn, int oTrackSelf, int oTrackPair, ref Register.Param KeyOn)
				{
					var Register = OPNA.s_Reg_KeyOn;
					int Value = OPNA.s_aValue_Ch[Channel];
					Value |= (aKeyOn[oTrackSelf])? OPNA.s_aValue_KeyOn[oTrackSelf]: 0;
					Value |= (aKeyOn[oTrackPair])? OPNA.s_aValue_KeyOn[oTrackPair]: 0;
					KeyOn.Value = Value;
					if (KeyOn.IsModified) Append(ref Queue, Register, KeyOn.Value);
				}
			}
			
			
			
			static readonly byte[] s_aValue_TL = new byte[]{
				42, 40, 37, 34, 32, 29, 26, 24, 21, 18, 16, 13, 10, 8, 5, 2,
			};
			
			char mChannel;
			Track[] maTrack = new Track[2];
			
			
			
			~Player()
			{
			}
			
			
			
			public Player(Delivery @Delivery, char Channel)
			{
				mChannel = Char.ToUpper(Channel);
				maTrack[0] = new Track(Delivery.TimbreLibrary, Delivery.PartLibrary.Bake(Char.ToUpper(Channel)));
				maTrack[1] = new Track(Delivery.TimbreLibrary, Delivery.PartLibrary.Bake(Char.ToLower(Channel)));
			}
			
			
			
			public bool Convert(out string Muc)
			{
				bool Result = true;
				
				var Queue = new StringBuilder(0x4000);
				int Channel = "ABCDEF".IndexOf(mChannel);
				if (Channel >= 0){
					var Alias = "ABCHIJ"[Channel];
					
					{	// 
						var Register = OPNA.s_Reg_KeyOn;
						int Value = OPNA.s_aValue_Ch[Channel];
						Queue.Append($"{Alias} o1q0 ");
						Queue.Append($"yTL,1,$7f yTL,2,$7f yTL,3,$7f yTL,4,$7f ");
						Queue.Append($"yKA,1,$00 yKA,2,$00 yKA,3,$00 yKA,4,$00 ");
						Queue.Append($"ySL,1,$ff ySL,2,$ff ySL,3,$ff ySL,4,$ff ");
						Queue.Append($"c%1& ");
						Append(ref Queue, Register, Value);
						Queue.Append($"c%1& \r\n");
						Queue.Append($"{Alias} ");
					}
					
					bool[] abTerm = new bool[2]{false, false};
					bool[] aKeyOn = new bool[2]{false, false};
					bool[] abScale = new bool[2]{false, false};
					int[] aScale = new int[2]{-1, -1};
					int[] aDetune = new int[2]{0, 0};
					int Detune = 0;
					int Clock = -1;
					
					var Block_FNumber = new Register.Param();
					var DT_MT_0 = new Register.Param();
					var DT_MT_1 = new Register.Param();
					var KeyOn = new Register.Param();
					
					while (Result && !(abTerm[0] && abTerm[1])){
						++Clock;
						
						{	// 
							int oTrack = 0;
							foreach (var t in maTrack){
								Result = t.Tick(ref Queue, ref Clock, Channel, oTrack, aKeyOn, out abScale[oTrack], ref KeyOn);
								if (Result){
									abTerm[oTrack] = t.IsTerm;
									++oTrack;
								} else {
									break;
								}
							}
						}
						
						if (Result){
							if (abScale[0] || abScale[1]){
								Queue.Append($"\r\n{Alias} ");
							}
							
							if (aScale[0] != maTrack[0].mScale
							||	aScale[1] != maTrack[1].mScale
							||	aDetune[0] != maTrack[0].mDetune
							||	aDetune[1] != maTrack[1].mDetune
								){
								aScale[0] = maTrack[0].mScale;
								aScale[1] = maTrack[1].mScale;
								aDetune[0] = maTrack[0].mDetune;
								aDetune[1] = maTrack[1].mDetune;
								Detune = (Math.Abs(aDetune[0]) >= Math.Abs(aDetune[1]))? aDetune[0]: aDetune[1];
								
								var Scale0 = (aScale[0] >= 0)? aScale[0]: aScale[1];
								var Scale1 = (aScale[1] >= 0)? aScale[1]: aScale[0];
								if (Scale0 >= 0 || Scale1 >= 0){
									var bChord = (Scale0 < Scale1);
									var oChord = (bChord)? Scale1 - Scale0: Scale0 - Scale1;
									if (oChord < Util.s_aChord.Length){
										var oScale = (bChord)? Scale0: Scale1;
										var Chord = Util.s_aChord[oChord];
										
										oScale += Chord.Transpose;
										if (oScale >= 0 && oScale < OPNA.s_aValue_Block_FNumber.Length){
											{	// 
												var Register0 = OPNA.s_aaReg_Block_FNumber[0] + (Channel%3);
												var Register1 = OPNA.s_aaReg_Block_FNumber[1] + (Channel%3);
												var Value = OPNA.s_aValue_Block_FNumber[oScale] + Chord.Detune + Detune;
												Block_FNumber.Value = Value;
												if (Block_FNumber.IsModified){
													// The order cannot be changed
													Append(ref Queue, Register1, Block_FNumber.Value>>8);
													Append(ref Queue, Register0, Block_FNumber.Value&0xff);
												}
											}
											
											{	// 
												int DT_MT_L = Chord.MT_Low | (Chord.DT_Low << 4);
												int DT_MT_H = Chord.MT_High | (Chord.DT_High << 4);
												
												var Register00 = OPNA.s_aaReg_DT_MT[0][0] + (Channel%3);
												var Register01 = OPNA.s_aaReg_DT_MT[0][1] + (Channel%3);
												var Register10 = OPNA.s_aaReg_DT_MT[1][0] + (Channel%3);
												var Register11 = OPNA.s_aaReg_DT_MT[1][1] + (Channel%3);
												var Value0 = (bChord)? DT_MT_L: DT_MT_H;
												var Value1 = (bChord)? DT_MT_H: DT_MT_L;
												DT_MT_0.Value = Value0;
												DT_MT_1.Value = Value1;
												if (DT_MT_0.IsModified || DT_MT_1.IsModified){
													Append(ref Queue, Register00, DT_MT_0.Value);
													Append(ref Queue, Register01, DT_MT_0.Value);
													Append(ref Queue, Register10, DT_MT_1.Value);
													Append(ref Queue, Register11, DT_MT_1.Value);
												}
											}
										} else {
											Console.WriteLine("!! Error !! : Out of scale");
											Result = false;
										}
									} else {
										Console.WriteLine("!! Error !! : Chords are too far apart");
										Result = false;
									}
								}
								if (!Result){
									foreach (var t in maTrack){
										Packet Packet;
										if (t.Peek(out Packet)){
											if (Packet.Logger >= 0){
												var Logger = t.Logger(Packet.Logger);
												Logger.Callstack.Log(Logger.In_oLine, Packet.Column, Logger.In_nMacro);
											}
										} else {
											Console.WriteLine("!! Error !! : Fatal error");
										}
									}
								}
							}
						}
						
						if (maTrack[0].mbLoop && abTerm[0]){
							break;
						}
					}
					
					if (Result){
						Duration(ref Queue, ref Clock, !maTrack[0].mbLoop);
					}
				}
				Muc = Queue.ToString();
				return Result;
			}
			
			
			
			static void Append(ref StringBuilder Queue, int Register, int Value)
			{
				Queue.Append($"y${Register:x02},${Value:x02} ");
			}
			
			
			
			static void Duration(ref StringBuilder Queue, ref int Clock, bool bTerm)
			{
				if (Clock > 0){
					var Surplus = Clock & 0x7f;
					if (Surplus > 0){
						Clock -= Surplus;
						Queue.Append($"c%{Surplus}");
						if (!(Clock == 0 && bTerm)) Queue.Append("&");
					}
					while (Clock > 0){
						Clock -= 0x80;
						Queue.Append($"c%128");
						if (!(Clock == 0 && bTerm)) Queue.Append("&");
					}
					Queue.Append($" ");
				}
			}
		}
	}
}
