using System;
namespace AisParser {
    public static class MessageBuilder {

        public static Messages Build (this in VdmVdo vdm, ref SixbitSpan sixbit) {
            switch (vdm.MsgId) {
                case 1:
                    return BuildMsg1 (ref sixbit);
                case 2:
                    return BuildMsg2 (ref sixbit);
                case 3:
                    return BuildMsg3 (ref sixbit);
                case 4:
                    return BuildMsg4 (ref sixbit);
                case 5:
                    return BuildMsg5 (ref sixbit);
                case 6:
                    return BuildMsg6 (ref sixbit);
                case 7:
                    return BuildMsg7 (ref sixbit);
                case 8:
                    return BuildMsg8 (ref sixbit);
                case 9:
                    return BuildMsg9 (ref sixbit);
                case 10:
                    return BuildMsg10 (ref sixbit);
                case 11:
                    return BuildMsg11 (ref sixbit);
                case 12:
                    return BuildMsg12 (ref sixbit);
                case 13:
                    return BuildMsg13 (ref sixbit);
                case 14:
                    return BuildMsg14 (ref sixbit);
                case 15:
                    return BuildMsg15 (ref sixbit);
                case 16:
                    return BuildMsg16 (ref sixbit);
                case 17:
                    return BuildMsg17 (ref sixbit);
                case 18:
                    return BuildMsg18 (ref sixbit);
                case 19:
                    return BuildMsg19 (ref sixbit);
                case 20:
                    return BuildMsg20 (ref sixbit);
                case 21:
                    return BuildMsg21 (ref sixbit);
                case 22:
                    return BuildMsg22 (ref sixbit);
                case 23:
                    return BuildMsg23 (ref sixbit);
                case 24:
                    return BuildMsg24 (ref sixbit);

                default:
                    throw new NotSupportedException ("不支持的消息类型");
            }
        }

        private static void FillMessages (this Messages messages, ref SixbitSpan sixState) {
            messages.Repeat = (int) sixState.Get (2);
            messages.UserId = sixState.Get (30);
        }

        private static void FillMsg123 (this Message123 message, ref SixbitSpan sixState) {
            if (sixState.BitLength != 168) throw new AisMessageException ($"Message {message.MsgId} wrong length");

            message.FillMessages (ref sixState);

            /* Parse the Message 1 */
            message.NavStatus = (int) sixState.Get (4);
            message.Rot = (int) sixState.Get (8);
            message.Sog = (int) sixState.Get (10);
            message.PosAcc = (int) sixState.Get (1);
            message.Pos = Position.FromAis (longitude: sixState.Get (28), latitude: sixState.Get (27));
            message.Cog = (int) sixState.Get (12);
            message.TrueHeading = (int) sixState.Get (9);
            message.UtcSec = (int) sixState.Get (6);
            message.Regional = (int) sixState.Get (2);
            message.Spare = (int) sixState.Get (3);
            message.Raim = (int) sixState.Get (1);
            message.SyncState = (int) sixState.Get (2);
        }

        private static Message1 BuildMsg1 (ref SixbitSpan sixState) {
            var msg = new Message1 ();
            msg.FillMsg123 (ref sixState);
            /* Parse the Message 1 */
            msg.SlotTimeout = (int) sixState.Get (3);
            msg.SubMessage = (int) sixState.Get (14);
            return msg;
        }

        private static Message2 BuildMsg2 (ref SixbitSpan sixState) {
            var msg = new Message2 ();
            FillMsg123 (msg, ref sixState);
            /* Parse the Message 1 */
            msg.SlotTimeout = (int) sixState.Get (3);
            msg.SubMessage = (int) sixState.Get (14);
            return msg;
        }

        private static Message3 BuildMsg3 (ref SixbitSpan sixState) {
            var msg = new Message3 ();
            FillMsg123 (msg, ref sixState);
            // Parse the Message 3 
            msg.SlotIncrement = (int) sixState.Get (13);
            msg.NumSlots = (int) sixState.Get (3);
            msg.Keep = (int) sixState.Get (1);
            return msg;
        }

        private static Message10 BuildMsg10 (ref SixbitSpan sixState) {
            var msg = new Message10 ();
            if (sixState.BitLength != 72) throw new AisMessageException ("Message 10 wrong length");

            FillMessages (msg, ref sixState);

            msg.Spare1 = (int) sixState.Get (2);
            msg.Destination = sixState.Get (30);
            msg.Spare2 = (int) sixState.Get (2);

            return msg;
        }

        private static Sotdma FillSotdma (ref SixbitSpan sixState) {
            if (sixState.BitLength < 19) throw new AisMessageException ("SOTDMA wrong length");
            return new Sotdma () {
                SyncState = (char) sixState.Get (2),
                    SlotTimeout = (char) sixState.Get (3),
                    SubMessage = (int) sixState.Get (14),
            };
        }

        private static Itdma FillItdma (ref SixbitSpan sixState) {
            if (sixState.BitLength < 19) throw new AisMessageException ("ITDMA wrong length");
            return new Itdma () {
                SyncState = (char) sixState.Get (2),
                    SlotInc = (int) sixState.Get (13),
                    NumSlots = (char) sixState.Get (3),
                    KeepFlag = (char) sixState.Get (1)
            };

        }
        private static Message11 BuildMsg11 (ref SixbitSpan sixState) {
            if (sixState.BitLength != 168) throw new AisMessageException ("Message 11 wrong length");
            var msg = new Message11 ();
            FillMessages (msg, ref sixState);

            /* Parse the Message 11 */
            msg.UtcYear = (int) sixState.Get (14);
            msg.UtcMonth = (int) sixState.Get (4);
            msg.UtcDay = (int) sixState.Get (5);
            msg.UtcHour = (int) sixState.Get (5);
            msg.UtcMinute = (int) sixState.Get (6);
            msg.UtcSecond = (int) sixState.Get (6);
            msg.PosAcc = (int) sixState.Get (1);

            msg.Pos = Position.FromAis (longitude: sixState.Get (28), latitude: sixState.Get (27));

            msg.PosType = (int) sixState.Get (4);
            msg.Spare = (int) sixState.Get (10);
            msg.Raim = (int) sixState.Get (1);
            msg.SotdmaState = FillSotdma (ref sixState);

            return msg;
        }
        private static Message12 BuildMsg12 (ref SixbitSpan sixState) {
            var msg = new Message12 ();
            var length = sixState.BitLength;
            if (length < 72 || length > 1008) throw new AisMessageException ("Message 12 wrong length");

            FillMessages (msg, ref sixState);

            msg.Sequence = (int) sixState.Get (2);
            msg.Destination = sixState.Get (30);
            msg.Retransmit = (int) sixState.Get (1);
            msg.Spare = (int) sixState.Get (1);
            msg.Message = sixState.GetString ((length - 72) / 6);

            return msg;
        }
        private static Message13 BuildMsg13 (ref SixbitSpan sixState) {
            var msg = new Message13 ();

            var length = sixState.BitLength;
            if (length < 72 || length > 168) throw new AisMessageException ("Message 13 wrong length");

            FillMessages (msg, ref sixState);

            msg.Spare = (int) sixState.Get (2);
            msg.Destid1 = sixState.Get (30);
            msg.Sequence1 = (int) sixState.Get (2);
            msg.NumAcks = 1;

            if (length > 72) {
                msg.Destid2 = sixState.Get (30);
                msg.Sequence2 = (int) sixState.Get (2);
                msg.NumAcks++;
            }

            if (length > 104) {
                msg.Destid3 = sixState.Get (30);
                msg.Sequence3 = (int) sixState.Get (2);
                msg.NumAcks++;
            }

            if (length > 136) {
                msg.Destid4 = sixState.Get (30);
                msg.Sequence4 = (int) sixState.Get (2);
                msg.NumAcks++;
            }

            return msg;
        }
        private static Message14 BuildMsg14 (ref SixbitSpan sixState) {

            var length = sixState.BitLength;
            if (length < 40 || length > 1008) throw new AisMessageException ("Message 14 wrong length");
            var msg = new Message14 ();
            FillMessages (msg, ref sixState);
            msg.Spare = (int) sixState.Get (2);
            msg.Message = sixState.GetString ((length - 40) / 6);
            return msg;
        }
        private static Message15 BuildMsg15 (ref SixbitSpan sixState) {
            var msg = new Message15 ();

            var length = sixState.BitLength;
            if (length < 88 || length > 162) throw new AisMessageException ("Message 15 wrong length");
            FillMessages (msg, ref sixState);

            msg.Spare1 = (int) sixState.Get (2);
            msg.Destid1 = sixState.Get (30);
            msg.MsgId1_1 = (int) sixState.Get (6);
            msg.Offset1_1 = (int) sixState.Get (12);
            msg.NumReqs = 1;

            if (length > 88) {
                msg.Spare2 = (int) sixState.Get (2);
                msg.MsgId1_2 = (int) sixState.Get (6);
                msg.Offset1_2 = (int) sixState.Get (12);
                msg.NumReqs = 2;
            }

            if (length == 160) {
                msg.Spare3 = (int) sixState.Get (2);
                msg.Destid2 = sixState.Get (30);
                msg.MsgId2_1 = (int) sixState.Get (6);
                msg.Offset2_1 = (int) sixState.Get (12);
                msg.Spare4 = (int) sixState.Get (2);
                msg.NumReqs = 3;
            }
            return msg;
        }
        private static Message16 BuildMsg16 (ref SixbitSpan sixState) {
            var msg = new Message16 ();

            var length = sixState.BitLength;
            if (length < 96 || length > 144) throw new AisMessageException ("Message 16 wrong length");

            FillMessages (msg, ref sixState);

            msg.Spare1 = (int) sixState.Get (2);
            msg.DestIdA = sixState.Get (30);
            msg.OffsetA = (int) sixState.Get (12);
            msg.IncrementA = (int) sixState.Get (10);
            msg.NumCmds = 1;

            if (length == 144) {
                msg.DestIdB = sixState.Get (30);
                msg.OffsetB = (int) sixState.Get (12);
                msg.IncrementB = (int) sixState.Get (10);
                msg.Spare2 = (int) sixState.Get (4);
                msg.NumCmds = 2;
            }
            return msg;
        }
        private static Message17 BuildMsg17 (ref SixbitSpan sixState) {
            var msg = new Message17 ();

            var length = sixState.BitLength;
            if (length < 80 || length > 816) throw new AisMessageException ("Message 17 wrong length");

            FillMessages (msg, ref sixState);

            msg.Spare1 = (int) sixState.Get (2);

            msg.Pos = Position.FromAis (longitude: sixState.Get (18) * 10, latitude: sixState.Get (17) * 10);

            msg.Spare2 = (int) sixState.Get (5);
            msg.MsgType = (int) sixState.Get (6);
            msg.StationId = (int) sixState.Get (10);
            msg.ZCount = (int) sixState.Get (13);
            msg.SeqNum = (int) sixState.Get (3);
            msg.NumWords = (int) sixState.Get (5);
            msg.Health = (int) sixState.Get (3);

            //msg.Data = sixState;
            return msg;
        }
        private static Message18 BuildMsg18 (ref SixbitSpan sixState) {
            var msg = new Message18 ();

            if (sixState.BitLength != 168) throw new AisMessageException ("Message 18 wrong length");

            FillMessages (msg, ref sixState);

            msg.Regional1 = (int) sixState.Get (8);
            msg.Sog = (int) sixState.Get (10);
            msg.PosAcc = (int) sixState.Get (1);

            msg.Pos = Position.FromAis (
                longitude: sixState.Get (28),
                latitude: sixState.Get (27)
            );

            msg.Cog = (int) sixState.Get (12);
            msg.TrueHeading = (int) sixState.Get (9);
            msg.UtcSec = (int) sixState.Get (6);
            msg.Regional2 = (int) sixState.Get (2);
            msg.UnitFlag = (int) sixState.Get (1);
            msg.DisplayFlag = (int) sixState.Get (1);
            msg.DscFlag = (int) sixState.Get (1);
            msg.BandFlag = (int) sixState.Get (1);
            msg.Msg22Flag = (int) sixState.Get (1);
            msg.ModeFlag = (int) sixState.Get (1);
            msg.Raim = (int) sixState.Get (1);
            msg.CommState = (int) sixState.Get (1);

            if (msg.CommState == 0) {
                msg.SotdmaState = FillSotdma (ref sixState);
            } else {
                msg.ItdmaState = FillItdma (ref sixState);
            }
            return msg;
        }
        private static Message19 BuildMsg19 (ref SixbitSpan sixState) {
            var msg = new Message19 ();

            if (sixState.BitLength != 312) throw new AisMessageException ("Message 19 wrong length");

            FillMessages (msg, ref sixState);

            msg.Regional1 = (int) sixState.Get (8);
            msg.Sog = (int) sixState.Get (10);
            msg.PosAcc = (int) sixState.Get (1);

            msg.Pos = Position.FromAis (longitude: sixState.Get (28), latitude: sixState.Get (27));

            msg.Cog = (int) sixState.Get (12);
            msg.TrueHeading = (int) sixState.Get (9);
            msg.UtcSec = (int) sixState.Get (6);
            msg.Regional2 = (int) sixState.Get (4);
            msg.Name = sixState.GetString (20);
            msg.ShipType = (int) sixState.Get (8);
            msg.DimBow = (int) sixState.Get (9);
            msg.DimStern = (int) sixState.Get (9);
            msg.DimPort = (int) sixState.Get (6);
            msg.DimStarboard = (int) sixState.Get (6);
            msg.PosType = (int) sixState.Get (4);
            msg.Raim = (int) sixState.Get (1);
            msg.Dte = (int) sixState.Get (1);
            msg.Spare = (int) sixState.Get (5);
            return msg;
        }
        private static Message20 BuildMsg20 (ref SixbitSpan sixState) {
            var msg = new Message20 ();

            var length = sixState.BitLength;
            if (length < 72 || length > 162) throw new AisMessageException ("Message 20 wrong length");

            FillMessages (msg, ref sixState);

            msg.Spare1 = (int) sixState.Get (2);
            msg.Offset1 = (int) sixState.Get (12);
            msg.Slots1 = (int) sixState.Get (4);
            msg.Timeout1 = (int) sixState.Get (3);
            msg.Increment1 = (int) sixState.Get (11);
            msg.NumCmds = 1;

            if (length > 72) {
                msg.Offset2 = (int) sixState.Get (12);
                msg.Slots2 = (int) sixState.Get (4);
                msg.Timeout2 = (int) sixState.Get (3);
                msg.Increment2 = (int) sixState.Get (11);
                msg.NumCmds = 2;
            }

            if (length > 104) {
                msg.Offset3 = (int) sixState.Get (12);
                msg.Slots3 = (int) sixState.Get (4);
                msg.Timeout3 = (int) sixState.Get (3);
                msg.Increment3 = (int) sixState.Get (11);
                msg.NumCmds = 3;
            }

            if (length > 136) {
                msg.Offset4 = (int) sixState.Get (12);
                msg.Slots4 = (int) sixState.Get (4);
                msg.Timeout4 = (int) sixState.Get (3);
                msg.Increment4 = (int) sixState.Get (11);
                msg.NumCmds = 4;
            }
            return msg;
        }
        private static Message21 BuildMsg21 (ref SixbitSpan sixState) {
            var msg = new Message21 ();

            var length = sixState.BitLength;
            if (length < 272 || length > 360) throw new AisMessageException ("Message 21 wrong length");

            FillMessages (msg, ref sixState);

            msg.AtonType = (int) sixState.Get (5);
            msg.Name = sixState.GetString (20);
            msg.PosAcc = (int) sixState.Get (1);

            msg.Pos = Position.FromAis (longitude: sixState.Get (28), latitude: sixState.Get (27));

            msg.DimBow = (int) sixState.Get (9);
            msg.DimStern = (int) sixState.Get (9);
            msg.DimPort = (int) sixState.Get (6);
            msg.DimStarboard = (int) sixState.Get (6);
            msg.PosType = (int) sixState.Get (4);
            msg.UtcSec = (int) sixState.Get (6);
            msg.OffPosition = (int) sixState.Get (1);
            msg.Regional = (int) sixState.Get (8);
            msg.Raim = (int) sixState.Get (1);
            msg.VirtualRenamed = (int) sixState.Get (1);
            msg.Assigned = (int) sixState.Get (1);
            msg.Spare1 = (int) sixState.Get (1);

            if (length > 272) msg.NameExt = sixState.GetString ((length - 272) / 6);
            return msg;
        }
        private static Message22 BuildMsg22 (ref SixbitSpan sixState) {
            var msg = new Message22 ();

            if (sixState.BitLength != 168) throw new AisMessageException ("Message 22 wrong length");

            FillMessages (msg, ref sixState);

            msg.Spare1 = (int) sixState.Get (1);
            msg.ChannelA = (int) sixState.Get (12);
            msg.ChannelB = (int) sixState.Get (12);
            msg.TxrxMode = (int) sixState.Get (4);
            msg.Power = (int) sixState.Get (1);

            var neLongitude = sixState.Get (18);
            var neLatitude = sixState.Get (17);

            var swLongitude = sixState.Get (18);
            var swLatitude = sixState.Get (17);

            msg.Addressed = (int) sixState.Get (1);
            msg.BwA = (int) sixState.Get (1);
            msg.BwB = (int) sixState.Get (1);
            msg.TzSize = (int) sixState.Get (3);

            // Is the position actually an address?
            if (msg.Addressed == 1) {
                // Convert the positions to addresses 
                msg.Addressed1 = (neLongitude << 12) + (neLatitude >> 5);
                msg.Addressed2 = (swLongitude << 12) + (swLatitude >> 5);
            } else {
                msg.NePos = Position.FromAis (
                    longitude: neLongitude * 10,
                    latitude: neLatitude * 10
                );

                msg.SwPos = Position.FromAis (
                    longitude: swLongitude * 10,
                    latitude: swLatitude * 10
                );
            }
            return msg;
        }
        private static Message23 BuildMsg23 (ref SixbitSpan sixState) {
            if (sixState.BitLength == 168) throw new AisMessageException ("Message 23 wrong length");
            var msg = new Message23 ();
            FillMessages (msg, ref sixState);

            msg.Spare1 = (int) sixState.Get (2);

            msg.NePos = Position.FromAis (
                longitude: sixState.Get (18) * 10,
                latitude: sixState.Get (17) * 10
            );

            msg.SwPos = Position.FromAis (
                longitude: sixState.Get (18) * 10,
                latitude: sixState.Get (17) * 10
            );

            msg.StationType = (int) sixState.Get (4);
            msg.ShipType = (int) sixState.Get (8);
            msg.Spare2 = sixState.Get (22);
            msg.TxrxMode = (int) sixState.Get (2);
            msg.ReportInterval = (int) sixState.Get (4);
            msg.QuietTime = (int) sixState.Get (4);
            msg.Spare3 = (int) sixState.Get (6);
            return msg;
        }
        private static Message24 BuildMsg24 (ref SixbitSpan sixState) {

            var length = sixState.BitLength;
            if (length != 160 && length != 168) throw new AisMessageException ("Message 24 wrong length");

            var msg = new Message24 ();
            FillMessages (msg, ref sixState);

            msg.PartNumber = (int) sixState.Get (2);

            if (msg.PartNumber == 0) {
                /* Parse 24A */
                /* Get the Ship Name, convert to ASCII */
                msg.Name = sixState.GetString (20);

                /* Indicate reception of part A */
                msg.Flags |= 0x01;
            } else if (msg.PartNumber == 1) {
                /* Parse 24B */
                msg.ShipType = (int) sixState.Get (8);
                msg.VendorId = sixState.GetString (7);
                msg.Callsign = sixState.GetString (7);

                msg.DimBow = (int) sixState.Get (9);
                msg.DimStern = (int) sixState.Get (9);
                msg.DimPort = (int) sixState.Get (6);
                msg.DimStarboard = (int) sixState.Get (6);
                msg.Spare = (int) sixState.Get (6);

                /* Indicate reception of part A */
                msg.Flags |= 0x02;
            } else {
                throw new AisMessageException ("Unknown Message 24 Part #");
            }
            return msg;
        }
        private static Message4 BuildMsg4 (ref SixbitSpan sixState) {
            if (sixState.BitLength != 168) throw new AisMessageException ("Message 4 wrong length");

            var msg = new Message4 ();
            FillMessages (msg, ref sixState);

            msg.UtcYear = (int) sixState.Get (14);
            msg.UtcMonth = (int) sixState.Get (4);
            msg.UtcDay = (int) sixState.Get (5);
            msg.UtcHour = (int) sixState.Get (5);
            msg.UtcMinute = (int) sixState.Get (6);
            msg.UtcSecond = (int) sixState.Get (6);
            msg.PosAcc = (int) sixState.Get (1);

            msg.Pos = Position.FromAis (
                longitude: sixState.Get (28),
                latitude: sixState.Get (27)
            );

            msg.PosType = (int) sixState.Get (4);
            msg.Spare = (int) sixState.Get (10);
            msg.Raim = (int) sixState.Get (1);
            msg.SyncState = (int) sixState.Get (2);
            msg.SlotTimeout = (int) sixState.Get (3);
            msg.SubMessage = (int) sixState.Get (14);
            return msg;
        }
        private static Message5 BuildMsg5 (ref SixbitSpan sixState) {

            if (sixState.BitLength != 424) throw new AisMessageException ("Message 5 wrong length");

            var msg = new Message5 ();
            FillMessages (msg, ref sixState);

            msg.Version = (int) sixState.Get (2);
            msg.Imo = sixState.Get (30);
            msg.Callsign = sixState.GetString (7);
            msg.Name = sixState.GetString (20);
            msg.ShipType = (int) sixState.Get (8);
            msg.DimBow = (int) sixState.Get (9);
            msg.DimStern = (int) sixState.Get (9);
            msg.DimPort = (int) sixState.Get (6);
            msg.DimStarboard = (int) sixState.Get (6);
            msg.PosType = (int) sixState.Get (4);
            msg.Eta = sixState.Get (20);
            msg.Draught = (int) sixState.Get (8);
            msg.Dest = sixState.GetString (20);
            return msg;
        }
        private static Message6 BuildMsg6 (ref SixbitSpan sixState) {

            if (sixState.BitLength < 88 || sixState.BitLength > 1008)
                throw new AisMessageException ("Message 6 wrong length");

            var msg = new Message6 ();
            FillMessages (msg, ref sixState);

            msg.Sequence = (int) sixState.Get (2);
            msg.Destination = sixState.Get (30);
            msg.Retransmit = sixState.Get (1) == 1;
            msg.Spare = (int) sixState.Get (1);
            msg.AppId = (int) sixState.Get (16);

            /* Store the remaining payload of the packet for further processing */
            //msg.Data = sixState;
            return msg;
        }
        private static Message7 BuildMsg7 (ref SixbitSpan sixState) {

            var length = sixState.BitLength;
            if (length < 72 || length > 168) throw new AisMessageException ("Message 7 wrong length");

            var msg = new Message7 ();
            FillMessages (msg, ref sixState);

            msg.Spare = (int) sixState.Get (2);
            msg.Destid1 = sixState.Get (30);
            msg.Sequence1 = (int) sixState.Get (2);
            msg.NumAcks = 1;

            if (length > 72) {
                msg.Destid2 = sixState.Get (30);
                msg.Sequence2 = (int) sixState.Get (2);
                msg.NumAcks++;
            }

            if (length > 104) {
                msg.Destid3 = sixState.Get (30);
                msg.Sequence3 = (int) sixState.Get (2);
                msg.NumAcks++;
            }

            if (length > 136) {
                msg.Destid4 = sixState.Get (30);
                msg.Sequence4 = (int) sixState.Get (2);
                msg.NumAcks++;
            }
            return msg;
        }
        private static Message8 BuildMsg8 (ref SixbitSpan sixState) {

            var length = sixState.BitLength;
            if (length < 56 || length > 1008) throw new AisMessageException ("Message 8 wrong length");

            var msg = new Message8 ();
            FillMessages (msg, ref sixState);

            msg.Spare = (int) sixState.Get (2);
            msg.AppId = (int) sixState.Get (16);

            /* Store the remaining payload of the packet for further processing */
            //msg.Data = sixState;
            return msg;
        }
        private static Message9 BuildMsg9 (ref SixbitSpan sixState) {
            if (sixState.BitLength != 168) throw new AisMessageException ("Message 9 wrong length");

            var msg = new Message9 ();
            FillMessages (msg, ref sixState);

            msg.Altitude = (int) sixState.Get (12);
            msg.Sog = (int) sixState.Get (10);
            msg.PosAcc = (int) sixState.Get (1);

            msg.Pos = Position.FromAis (longitude: sixState.Get (28), latitude: sixState.Get (27));

            msg.Cog = (int) sixState.Get (12);
            msg.UtcSec = (int) sixState.Get (6);
            msg.Regional = (char) sixState.Get (8);
            msg.Dte = (char) sixState.Get (1);
            msg.Spare = (char) sixState.Get (3);
            msg.Assigned = (char) sixState.Get (1);
            msg.Raim = (char) sixState.Get (1);
            msg.CommState = (char) sixState.Get (1);

            if (msg.CommState == 0)
                msg.SotdmaState = FillSotdma (ref sixState);
            else
                msg.ItdmaState = FillItdma (ref sixState);
            return msg;
        }
    }
}