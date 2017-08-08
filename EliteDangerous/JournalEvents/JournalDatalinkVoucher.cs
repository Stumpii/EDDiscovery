﻿/*
 * Copyright © 2016 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 *
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EliteDangerousCore.JournalEvents
{
//    When written: when scanning a datalink generates a reward
//    Parameters:
//•	Reward: value in credits
//•	VictimFaction
//•	PayeeFaction
    [JournalEntryType(JournalTypeEnum.DatalinkVoucher)]
    public class JournalDatalinkVoucher : JournalEntry, ILedgerNoCashJournalEntry
    {
        public JournalDatalinkVoucher(JObject evt) : base(evt, JournalTypeEnum.DatalinkVoucher)
        {
            VictimFaction = evt["VictimFaction"].Str();
            Reward = evt["Reward"].Long();
            PayeeFaction = evt["PayeeFaction"].Str();

        }
        public string PayeeFaction { get; set; }
        public long Reward { get; set; }
        public string VictimFaction { get; set; }

        public void LedgerNC(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEventNoCash(Id, EventTimeUTC, EventTypeID, PayeeFaction + " " + Reward.ToString("N0"));
        }

        public override System.Drawing.Bitmap Icon { get { return EliteDangerous.Properties.Resources.datalinkvoucher; } }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("Reward ; credits", Reward, "< from faction ", PayeeFaction, "against ", VictimFaction);
            detailed = "";
        }
    }
}