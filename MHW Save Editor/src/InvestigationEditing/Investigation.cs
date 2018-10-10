using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Data;
using MHW_Save_Editor.Data;


namespace MHW_Save_Editor.InvestigationEditing
{
    public class Investigation : NotifyUIBase
    {
        //public event PropertyChangedEventHandler PropertyChanged;

        public Investigation(byte[] newdata=null)
        {
            if (newdata == null) newdata = nullinvestigation;
            byte[] newish = new byte[inv_size];
            Array.Copy(newdata, 0, newish, 0, inv_size);
            _underlyingData = newish;
        }

        private byte[] _underlyingData;

    #region Thinlayer
        #region Members
        private static readonly byte[] questIsFilled = {0x30, 0x75, 0x00, 0x00};
        private bool _Filled
        {
            get => _underlyingData.Slice(0, 4).SequenceEqual(questIsFilled);
        }
        private bool _Selected
        {
            get => _underlyingData[4] != 0x00;
            set => _underlyingData[4] = (byte)(value ? 0x01 : 0x00);
        }
        private Int32 _Attempts
        {
            get => BitConverter.ToInt32(_underlyingData.Slice(5, 9),0);
            set => Array.Copy(BitConverter.GetBytes(value), 0, _underlyingData, 5, 4);
        }
        private bool _Seen
        {
            get => BitConverter.ToInt32(_underlyingData.Slice(9, 13),0)==3;
            set => _underlyingData[9] = (byte) (value?0x03:0x00);
        }
        private Int32 _LocaleIndex
        {
            get => _underlyingData[13];
            set => _underlyingData[13] = (byte) value;
        }
        private Int32 _Rank
        {
            get => _underlyingData[14];
            set => _underlyingData[14] = (byte) value;
        }
        private UInt32 _Mon1
        {
            get => GetMonster(0);
            set => SetMonster(value,0);
        }
        private UInt32 _Mon2
        {
            get => GetMonster(1);
            set => SetMonster(value,1);
        }
        private UInt32 _Mon3
        {
            get => GetMonster(2);
            set => SetMonster(value,2);
        }
        private UInt32 GetMonster(int index)
        {
            return BitConverter.ToUInt32(_underlyingData.Slice(15+index*4, 19+index*4),0);
        }
        private void SetMonster(UInt32 value, int index)
        {
            Array.Copy(BitConverter.GetBytes(value), 0, _underlyingData, 15 + 4 * index, 4);
        }
        private bool _M1Temper
        {
            get => _underlyingData[27]!=0x00;
            set => _underlyingData[27] = (byte) (value ? 0x01 : 0x00);
        }
        private bool _M2Temper
        {
            get => _underlyingData[28]!=0x00;
            set => _underlyingData[28] = (byte) (value ? 0x01 : 0x00);
        }
        private bool _M3Temper
        {
            get => _underlyingData[29]!=0x00;
            set => _underlyingData[29] = (byte) (value ? 0x01 : 0x00);
        }
        private int _HP
        {
            get => _underlyingData[30];
            set => _underlyingData[30] = (byte) value;
        }
        private int _Attack
        {
            get => _underlyingData[31];
            set => _underlyingData[31] = (byte) value;
        }
        private int _Defense
        {
            get => _underlyingData[32];
            set => _underlyingData[32] = (byte) value;
        }
        private int _X3
        {
            get => _underlyingData[33];
            set => _underlyingData[33] = (byte) value;
        }
        private int _Y0
        {
            get => _underlyingData[34];
            set => _underlyingData[34] = (byte) value;
        }
        private int _FlourishIndex
        {
            get => _underlyingData[35];
            set => _underlyingData[35] = (byte) value;
        }
        private int _TimeAmountIndex
        {
            get => _underlyingData[36];
            set => _underlyingData[36] = (byte) value;
        }
        private bool _Y3
        {
            get => _underlyingData[37]!=0x00;
            set => _underlyingData[37] = (byte) (value?0x01:0x00);
        }
        private int _FaintIndex
        {
            get => _underlyingData[38];
            set => _underlyingData[38] = (byte) value;
        }
        private int _PlayerCountIndex
        {
            get => _underlyingData[39];
            set => _underlyingData[39] = (byte) value;
        }

        private int _MonsterRewards
        {
            get => _underlyingData[40];
            set => _underlyingData[40] = (byte) value;
        }

        public int _ZennyMultiplier
        {
            get => _underlyingData[41];
            set => _underlyingData[41] = (byte) value;
        }
        #endregion
        
        public static readonly byte[] nullinvestigation= {0xFF,0xFF,0xFF,0xFF,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xFF,0xFF,0xFF,0xFF,
            0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00};
        
        public static readonly byte[] defaultinvestigation= {0x30,0x75,0x00,0x00,0x00,0x08,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x07,0x00,0x00,0x00,0x18,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00};

        private byte[] _Serialize()
        {
            byte[] result = new byte[42];
            _underlyingData.CopyTo(result, 0);
            return result;
        }

        private void _Overwrite(IList<byte> newestdata)
        {
            for (int i = 0; i < Investigation.inv_size; i++) _underlyingData[i] = newestdata[i];
        }
    #endregion
        
        public string InvestigationTitle
        {
            get
            {
                if (!_Filled) return "Empty Slot";
                string objective = _TimeAmountGoal[Goal];
                int count = _TimeAmountCount[Goal];
                string mainmon = count != 0 ? (MonsterNames[Mon1] + (count > 1 ? ", ..." : "")) : "Wildlife";
                return objective + " " + mainmon;
            }
        }

        public string LocaleTitle
        {
            get => LocalesNames[LocaleIndex];
        }
        
        public string Legality
        {
            get
            {
                StringBuilder Bob = new StringBuilder();
                UntemperableCondition(Bob);
                RankTemperCondition(Bob);
                ElderCondition(Bob);
                MonsterLocaleCondition(Bob);
                PickleBagelCondition(Bob);
                return Bob.ToString();
            }
        }

        public static readonly int inv_size = 42;
        public static readonly int inv_number = 250;
        public static readonly int[] inv_offsets = {0x003DADB1, 0x004D0EC1, 0x005C6FD1};
        

    #region Members
        
        public bool Filled
        {
            get => _Filled;
        }
        
        public string ToggleState
        {
            get => _Filled?"Clear":"Initialize";
        }
        
        public int Goal
        {
            get => _TimeAmountIndex;
            set{
                _TimeAmountIndex = value;
                RaisePropertyChanged();
                RaisePropertyChanged("InvestigationTitle");
                RaisePropertyChanged("Legality");
                RaiseBoxVals();
            }
        }
        public bool Seen
        {
            get => _Seen;
            set{
                _Seen = value;
                RaisePropertyChanged();
            }
        }
        public bool Selected
        {
            get => _Selected;
            set{
                _Selected = value;
                RaisePropertyChanged();
            }
        }
        public int Rank
        {
            get => _Rank;
            set
            {
                _Rank = value;
                RaisePropertyChanged();
                RaisePropertyChanged("Legality");
                RaiseBoxVals();
            }
        }
        public int Mon1
        {
            get => _CodeToMonsterIndex[_Mon1];
            set{
                _Mon1 = _MonstersCodeList[value];
                RaisePropertyChanged();
                RaisePropertyChanged("InvestigationTitle");
                RaisePropertyChanged("Legality");
                RaiseBoxVals();
            }
        }
        public int Mon2
        {
            get => _CodeToMonsterIndex[_Mon2];
            set{
                _Mon2 = _MonstersCodeList[value];
                RaisePropertyChanged();
                RaisePropertyChanged("InvestigationTitle");
                RaisePropertyChanged("Legality");
                RaiseBoxVals();
            }
        }
        public int Mon3
        {
            get => _CodeToMonsterIndex[_Mon3];
            set{
                _Mon3 = _MonstersCodeList[value];
                RaisePropertyChanged();
                RaisePropertyChanged("InvestigationTitle");
                RaisePropertyChanged("Legality");
                RaiseBoxVals();
            }
        }
        public bool M1Temper
        {
            get => _M1Temper;
            set
            {
                _M1Temper = value;
                RaisePropertyChanged();
                RaisePropertyChanged("Legality");
                RaiseBoxVals();
            }
        }
        public bool M2Temper
        {
            get => _M2Temper;
            set
            {
                _M2Temper = value;
                RaisePropertyChanged();
                RaisePropertyChanged("Legality");
                RaiseBoxVals();
            }
        }
        public bool M3Temper
        {
            get => _M3Temper;
            set
            {
                _M3Temper = value;
                RaisePropertyChanged();
                RaisePropertyChanged("Legality");
                RaiseBoxVals();
            }
        }

        public int HP
        {
            get => _HP;
            set
            {
                _HP = value;
                RaisePropertyChanged();
                RaiseBoxVals();
            }
        }
        public int Attack
        {
            get => _Attack;
            set
            {
                _Attack = value;
                RaisePropertyChanged();
                RaiseBoxVals();
            }
        }
        public int Defense
        {
            get => _Defense;
            set
            {
                _Defense = value;
                RaisePropertyChanged();
                RaiseBoxVals();
            }
        }
        public int X3
        {
            get => _X3;
            set
            {
                _X3 = value;
                RaisePropertyChanged();
                
            }
        }
        public int FaintIndex
        {
            get => _FaintIndex;
            set
            {
                _FaintIndex = value;
                RaisePropertyChanged();
                RaiseBoxVals();
            }
        }
        public int PlayerCountIndex
        {
            get => _PlayerCountIndex;
            set
            {
                _PlayerCountIndex = value;
                RaisePropertyChanged();
                RaiseBoxVals();
            }
        }   
        public int ZennyBonus
        {
            get => _ZennyMultiplier;
            set
            {
                _ZennyMultiplier = value;
                RaisePropertyChanged();
                RaiseBoxVals();
            }
        }  
        public int LocaleIndex
        {
            get => _LocaleIndex;
            set
            {
                _LocaleIndex = value;
                RaisePropertyChanged();
                RaisePropertyChanged("CurrentFlourishes");
                RaisePropertyChanged("FlourishIndex");
                RaisePropertyChanged("Legality");
                RaisePropertyChanged("LocaleTitle");
            }
        }

        public int FlourishIndex
        {
            get => _FlourishIndex;
            set
            {
                _FlourishIndex = value;
                RaisePropertyChanged();
            }
        }
        public int Y0
        {
            get => _Y0;
            set
            {
                _Y0 = value;
                RaisePropertyChanged();
            }
        }
        public bool Y3
        {
            get => _Y3;
            set
            {
                _Y3 = value;
                RaisePropertyChanged();
            }
        }
        public int MonsterRewards
        {
            get => _MonsterRewards;
            set
            {
                _MonsterRewards = value;
                RaisePropertyChanged();
                RaiseBoxVals();
            }
        }

        public int Attempts
        {
            get => _Attempts;
            set
            {
                _Attempts = value;
                RaisePropertyChanged();
                RaiseBoxVals();
            }
        }
    #endregion

    #region Methods

        public byte[] Serialize()
        {
            return _Serialize();
        }

        public static readonly string CSVHeader =
            "Seen,Filled,Attempts,Rank,Goal,Mon#,Time,Locale,Flourish,Monster1,Temper1,Monster2,Temper2,Monster3,Temper3,HP,Attack,Defense,X3,Y0,Y3,Faints,PlayerCount,MonRewards,ZennyBonus";
        
        public string LogCSV()
        {
            return (Seen ? 1 : 0) + "," + (Filled ? 1 : 0) + "," + Attempts + "," + _RankChoices[Rank] + "," +
                   _TimeAmountGoal[Goal] + "," + _TimeAmountCount[Goal] + "," + _TimeAmountObjective[Goal] + "," +
                   _LocalesNames[LocaleIndex] + "," + CurrentFlourishes[FlourishIndex] + "," + MonsterNames[Mon1] +
                   "," + (M1Temper ? 1 : 0) + "," + MonsterNames[Mon2] + "," + (M2Temper ? 1 : 0) + "," +
                   MonsterNames[Mon3] + "," + (M3Temper ? 1 : 0) + "," + HP + "," + Attack + "," + Defense + "," + X3 +
                   "," + Y0 + "," + Y3 + "," + _FaintValues[FaintIndex] + "," + _PlayerCountValues[PlayerCountIndex] +
                   "," + MonsterRewards + "," +
                   ZennyBonus;
        }        

        public string Log()
        {
            var builder = new StringBuilder()
                .AppendLine($"_____________________________________")
                .AppendLine($"Attempts: {Attempts}")
                .AppendLine($"Locale: {_LocalesNames[LocaleIndex]} - {CurrentFlourishes[FlourishIndex]}")
                .AppendLine($"Rank: {_RankChoices[Rank]}")
                .AppendLine($"{(M1Temper?"Tempered ":"")}{MonsterNames[Mon1]}")
                .AppendLine($"{(M2Temper?"Tempered ":"")}{MonsterNames[Mon2]}")
                .AppendLine($"{(M3Temper?"Tempered ":"")}{MonsterNames[Mon3]}")
                .AppendLine($"HP: {HP} - Att: {Attack} - Def: {Defense} - X3: {X3}")
                .AppendLine("Goal: "+_TimeAmountGoal[Goal]+" "+(_TimeAmountCount[Goal]!=0?(_TimeAmountCount[Goal]+
                           " Monster"+(_TimeAmountCount[Goal]>1?"s":"")):"")+ " in "+_TimeAmountObjective[Goal]+" min")
                .AppendLine($"Y0: {Y0} - Y3:{Y3}")
                .AppendLine($"Faints: {_FaintValues[FaintIndex]} - Players: {_PlayerCountValues[PlayerCountIndex]} - Box Multiplier: {MonsterRewards} - Zenny Multiplier: {ZennyBonus}");
            return builder.ToString();
        }

    #endregion
        
    #region LegalityConditions
       
        private void UntemperableCondition(StringBuilder Bob)
            //Checks if an untemperable monster has been tempered
        {
            string mon = MonsterNames[Mon1];
            if (_untemperable.Contains(mon) && M1Temper)
                Bob.Append($"{mon} cannot be tempered.");
            mon = MonsterNames[Mon2];
            if (_untemperable.Contains(mon) && M2Temper)
                Bob.Append($"{mon} cannot be tempered.");
            mon = MonsterNames[Mon3];
            if (_untemperable.Contains(mon) && M3Temper)
                Bob.Append($"{mon} cannot be tempered.");
        }

        private void RankTemperCondition(StringBuilder Bob)
            //Checks if there's tempering on a Low or High Rank investigation
        {
            if ((M1Temper || M2Temper || M3Temper)&& Rank!=2)Bob.Append($"Cannot have a Tempered Monster on a Non-Tempered Hunt.{Environment.NewLine}");
        }
        
        private static bool IsElder(int moncode)
        {
            return _Elders.Contains(_MonsterNames[moncode]);
        }

        private static bool IsEmpty(int moncode)
        {
            return _MonsterNames[moncode] == "Empty";
        }
        
        private void ElderCondition(StringBuilder Bob)
            //Checks elder dragons have no other monsters on their hunt
        {
            if (IsElder(Mon2) || IsElder(Mon3)) Bob.Append($"Elder Dragons should be in slot 1.{Environment.NewLine}");
            if (IsElder(Mon1) && !(IsEmpty(Mon2) && IsEmpty(Mon3))) Bob.Append($"Elder Dragons should be alone, set slot 2 and 3 to Empty.{Environment.NewLine}");
            if (IsElder(Mon1) && Goal>=3) Bob.Append($"Elder Dragons only allow Hunt 1 Monster in 50/30/15 min as a goal.{Environment.NewLine}");
        }

        private void MonsterLocaleCondition(StringBuilder Bob)
            //Checks if Monsters can be in the Locale and Rank of the investigation
        {
            if (!MonsterInLocale(Mon1, LocaleIndex, Rank))Bob.Append($"{MonsterNames[Mon1]} cannot be found in {LocalesNames[LocaleIndex]} at rank {RankChoices[Rank]}.{Environment.NewLine}");
            if (!MonsterInLocale(Mon2, LocaleIndex, Rank))Bob.Append($"{MonsterNames[Mon2]} cannot be found in {LocalesNames[LocaleIndex]} at rank {RankChoices[Rank]}.{Environment.NewLine}");
            if (!MonsterInLocale(Mon3, LocaleIndex, Rank))Bob.Append($"{MonsterNames[Mon3]} cannot be found in {LocalesNames[LocaleIndex]} at rank {RankChoices[Rank]}.{Environment.NewLine}");
        }
        
        private static bool MonsterInLocale(int monster, int locale, int rank)
        {
            return true;
        }

        private void PickleBagelCondition(StringBuilder Bob)
            //Checks any of the bizarre Bagel and Pickle conditions
        {
        }
        

    #endregion
        
    #region DataCollections

        public static readonly int[] _TimeAmountObjective =
            {50, 30, 15, 50, 30, 50, 50, 50, 50, 30, 15};

        public static readonly string[] _TimeAmountGoal =
        {
            "Hunt", "Hunt", "Hunt", "Hunt", "Hunt", "Hunt", "Slay Wildlife 1", "Slay Wildlife 2", "Capture",
            "Capture", "Capture"
        };
        public static readonly int[] _TimeAmountCount = {1,1,1,2,2,3,0,0,1,1,1};

        public static  readonly UInt32[] _MonstersCodeList =
        {
            0x00, 0x01, 0x07, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x10, 0x11,
            0x12, 0x13, 0x14, 0x15, 0x16, 0x18, 0x19, 0x1B, 0x1C, 0x1D, 0x1E,
            0x1F, 0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x27, 0xFFFFFFFF
        };

        private static List<String> _untemperable = new List<string>()
        {
            "Dodogama", "Great Jagras", "Great Girros", "Kulu-Ya-Ku", "Tzitzi-Ya-Ku"
        };
        
        private static string[] _Elders = new[]
        {
            "Kirin", "Kushala Daora", "Teostra", "Lunastra", "Nergigante", "Vaal Hazak"
        };
        
        public ObservableCollection<int> CommonValues
        {
            get => new ObservableCollection<int>(new[] {0, 1, 2, 3, 4, 5});
        }

        public static readonly ObservableCollection<int> _ZennyValues = new ObservableCollection<int>(new[]{0,1,2,3,4});
        public static  ObservableCollection<int> ZennyValues => _ZennyValues;
        public static readonly ObservableCollection<int> _PlayerCountValues = new ObservableCollection<int>(new[]{4,2});
        public static ObservableCollection<int> PlayerCountValues => _PlayerCountValues;
        public static readonly ObservableCollection<int> _FaintValues = new ObservableCollection<int>(new[]{5,3,2,1});
        public static  ObservableCollection<int> FaintValues => _FaintValues;
        public static readonly ObservableCollection<int> _MonsterRewardsValues= new ObservableCollection<int>(new[]{0,1,2,3,4});
        public static  ObservableCollection<int> MonsterRewardsValues => _MonsterRewardsValues;
        public static readonly ObservableCollection<string> _LocalesNames = new ObservableCollection<string>(
            new []{"Ancient Forest", "Wildspire Wastes", "Coral Highlands","Rotten Vale", "Elder Recess"});
        public static  ObservableCollection<string> LocalesNames => _LocalesNames;
        public static readonly ObservableCollection<string> _RankChoices = new ObservableCollection<string>(
            new []{"Low Rank", "High Rank", "Tempered"});

        public static  ObservableCollection<string> RankChoices => _RankChoices;
        public ObservableCollection<string> CurrentFlourishes
        {
            get => _FlourishMatrix[LocaleIndex];
        }

        public static readonly ObservableCollection<string> _GoalChoices = new ObservableCollection<string>(new []
        {
            "Hunt 1 Monster in 50 min", "Hunt 1 Monster in 30 min", "Hunt 1 Monster in 15 min",
            
            "Hunt 2 Monsters in 50 min", "Hunt 2 Monsters in 30 min", "Hunt 3 Monsters in 50 min",
            "Slay Wildlife 1 in 50 min", "Slay Wildlife 2 in 50 min",
            "Capture 1 Monster in 50 min", "Capture 1 Monster in 30 min", "Capture 1 Monster in 15 min"
        });

        public static ObservableCollection<string> GoalChoices => _GoalChoices;
        public static readonly Dictionary<UInt32, int> _CodeToMonsterIndex = new Dictionary<UInt32, int>()
        {
            {0x00,0},{0x01,1},{0x07,2},{0x09,3},{0x0a,4},{0x0b,5},{0x0c,6},{0x0d,7},{0x0e,8},{0x10,9},{0x11,10},
            {0x12,11},{0x13,12},{0x14,13},{0x15,14},{0x16,15},{0x18,16},{0x19,17},{0x1b,18},{0x1c,19},{0x1d,20},{0x1e,21},
            {0x1f,22},{0x20,23},{0x21,24},{0x22,25},{0x23,26},{0x24,27},{0x25,28},{0x27,29},{0xffffffff,30}
        };

        public static readonly string[] _MonsterNames = new[]
        {
            "Anjanath", "Rathalos", "Great Jagras", "Rathian", "Pink Rathian", "Azure Rathalos",
            "Diablos", "Black Diablos", "Kirin", "Kushala Daora", "Lunastra", "Teostra",
            "Lavasioth", "Deviljho", "Barroth", "Uragaan", "Pukei-Pukei", "Nergigante",
            "Kulu-Ya-Ku", "Tzitzi-Ya-Ku", "Jyuratodus", "Tobi-Kadachi", "Paolumu",
            "Legiana", "Great Girros", "Odogaron", "Radobaan", "Vaal Hazak", "Dodogama",
            "Bazelgeuse", "Empty"
        };

        public ObservableCollection<string> MonsterNames
        {
            get => new ObservableCollection<string>(_MonsterNames);
        }

        public static readonly ObservableCollection<string>[] _FlourishMatrix =
        {
            new ObservableCollection<string>(new [] {"Nothing","Mushrooms", "Flower Beds", "Mining Outcrops","Bonepiles","Gathering Points"}),
            new ObservableCollection<string>(new [] {"Nothing","Cactus", "Fruit", "Mining Outcrops","Bonepiles","Gathering Points"}),
            new ObservableCollection<string>(new [] {"Nothing","Conch Shells", "Pearl Oysters", "Mining Outcrops","Bonepiles","Gathering Points"}),
            new ObservableCollection<string>(new [] {"Nothing","Ancient Fossils", "Crimson Fruit", "Mining Outcrops","Bonepiles","Gathering Points"}),
            new ObservableCollection<string>(new [] {"Nothing","Amber Deposits", "Beryl Deposits", "Mining Outcrops","Bonepiles","Gathering Points"})
        };
        
        
    #endregion
        
    #region BoxstoreCalculations

        private void RaiseBoxVals()
        {
            RaisePropertyChanged("Box1Val");
            RaisePropertyChanged("Box2Val");
            RaisePropertyChanged("Box3Val");
            RaisePropertyChanged("Box4Val");
            RaisePropertyChanged("Box5Val");
        }
        
        public int Box1Val {get=>BoxValues[0];}
        public int Box2Val {get=>BoxValues[1];}
        public int Box3Val {get=>BoxValues[2];}
        public int Box4Val {get=>BoxValues[3];}
        public int Box5Val {get=>BoxValues[4];}
        
        
        public int[] BoxValues
        {
            get => (Goal == 6 || Goal == 7)?WildlifeRewards():RegularRewards();
        }

        private int[] WildlifeRewards()
        {
            int boxnum = Math.Min(RewardsPoints()/100+1, 3)+MonsterRewards;
            int[] result = {0, 0, 0, 0, 0};
            for (int i = 0; i < boxnum; i++) result[4 - i] = 1;
            return result;
        }

        private int[] RegularRewards()
        {
            int temperParity = Rank / 2;
            int column = 2*MonsterRewards+temperParity;
            int row = Math.Min(RewardsPoints()/100,8);
            (int, int, int, int, int) tuple =  RewardsMatrix[row,column];
            return new int[] {tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5};
        }

        private int RewardsPoints()
        {
            return AttackPoints[Attack] + HPPoints[HP] + DefensePoints[Defense] +
                FaintPoints[FaintIndex] + PlayerCountPoints[PlayerCountIndex] +
                ZennyPoints[ZennyBonus] + ObjectivePoints[Goal] + rdiPoints();
        }

        private static int[] AttackPoints = {0, 0, 0, 30, 60, 100};
        private static int[] HPPoints = {0, 0, 0, 30, 60, 100};
        private static int[] DefensePoints = {0, 0, 0, 30, 60, 60};
        private static int[] FaintPoints = {0, 0, 40, 100};
        private static int[] PlayerCountPoints = {0, 100};
        private static int[] ZennyPoints = {100,60,30,0,0};
        private static int[] ObjectivePoints = {0,100,150,80,130,200,10,10,70,170,220};

        private static (int, int, int, int, int)[,] RewardsMatrix = new [,]
        {
            {(0,0,0,1,2),(0,0,0,0,4),(0,0,0,2,2),(0,0,0,0,4),(0,0,0,1,3),(0,0,0,4,4)},
            {(0,0,0,1,2),(0,0,0,4,4),(0,0,0,2,2),(0,0,0,4,4),(0,0,0,2,3),(0,0,4,4,4)},
            {(0,0,1,2,2),(0,0,0,4,4),(0,0,2,2,2),(0,0,0,4,4),(0,0,2,2,3),(0,0,4,4,4)},
            {(0,0,1,1,3),(0,0,0,4,4),(0,0,1,2,3),(0,0,4,4,4),(0,0,2,2,3),(0,4,4,4,4)},
            {(0,1,1,3,3),(0,0,4,4,4),(0,1,2,3,3),(0,0,4,4,4),(0,2,2,3,3),(0,4,4,4,4)},
            {(0,1,1,3,3),(0,4,4,4,4),(0,1,2,3,3),(0,4,4,4,4),(0,2,2,3,3),(4,4,4,4,4)},
            {(0,1,3,3,3),(0,4,4,4,4),(0,2,3,3,3),(0,4,4,4,4),(0,3,3,3,3),(4,4,4,4,4)},
            {(0,1,3,3,3),(0,4,4,4,4),(0,2,3,3,3),(0,4,4,4,4),(0,3,3,3,3),(4,4,4,4,4)},
            {(1,3,3,3,3),(4,4,4,4,4),(2,3,3,3,3),(4,4,4,4,4),(3,3,3,3,3),(4,4,4,4,4)}
        };

        private int rdiPoints()
        {
            int rdi = 0;
            rdi += (IsElder(Mon1) ? 1 : 0) * 100;
            rdi += MonsterPointsSlot1[MonsterNames[Mon1]]*(M1Temper?1:0);
            rdi += MonsterPoints[MonsterNames[Mon2]];
            rdi += MonsterPoints[MonsterNames[Mon3]];
            rdi += 50 * (M2Temper ? 1 : 0) + 50 * (M3Temper ? 1 : 0);
            return rdi;
        }

        private static Dictionary<string, int> MonsterPointsSlot1 = new Dictionary<string, int>
        {
            {"Empty", 0},
            {"Anjanath", 60},
            {"Tobi-Kadachi", 40},
            {"Rathian", 60},
            {"Rathalos", 40},
            {"Pukei-Pukei", 0},
            {"Kushala Daora", 60},
            {"Kulu-Ya-Ku", 0},
            {"Great Jagras", 0},
            {"Bazelgeuse", 60},
            {"Deviljho", 0},
            {"Azure Rathalos", 60},
            {"Barroth", 40},
            {"Black Diablos", 60},
            {"Diablos", 40},
            {"Jyuratodus", 40},
            {"Pink Rathian", 0},
            {"Tzitzi-Ya-Ku", 0},
            {"Paolumu", 60},
            {"Legiana", 40},
            {"Kirin", 60},
            {"Teostra", 60},
            {"Odogaron", 40},
            {"Great Girros", 0},
            {"Radobaan", 60},
            {"Vaal Hazak", 60},
            {"Nergigante", 60},
            {"Lavasioth", 60},
            {"Uragaan", 60},
            {"Dodogama", 0}
        };

        private static Dictionary<string, int> MonsterPoints = new Dictionary<string, int>
        {
            {"Empty", 0},
            {"Anjanath", 30},
            {"Tobi-Kadachi", 30},
            {"Rathian", 30},
            {"Rathalos", 60},
            {"Pukei-Pukei", 30},
            {"Kushala Daora", 100},
            {"Kulu-Ya-Ku", 0},
            {"Great Jagras", 0},
            {"Bazelgeuse", 60},
            {"Deviljho", 0},
            {"Azure Rathalos", 60},
            {"Barroth", 30},
            {"Black Diablos", 60},
            {"Diablos", 60},
            {"Jyuratodus", 30},
            {"Pink Rathian", 60},
            {"Tzitzi-Ya-Ku", 0},
            {"Paolumu", 30},
            {"Legiana", 60},
            {"Kirin", 100},
            {"Teostra", 100},
            {"Odogaron", 60},
            {"Great Girros", 0},
            {"Radobaan", 30},
            {"Vaal Hazak", 100},
            {"Nergigante", 100},
            {"Lavasioth", 60},
            {"Uragaan", 60},
            {"Dodogama", 30}
        };

        #endregion
    }
}