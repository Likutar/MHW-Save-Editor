using System;
using System.IO;
using System.Reflection;
using MHW_Save_Editor.Data;

namespace MHW_Save_Editor.InventoryEditing
{

    public class Item :  NotifyUIBase
    {
        public override string ToString()
        {
            return name;
        }

        public UInt32 id  { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public Byte  subType  { get; set; }
        public UInt32 storageID { get; set; }
        public Byte rarity  { get; set; }
        public Byte carryLimit  { get; set; }
        public Byte  unk  { get; set; }
        public UInt16 sortOrder { get; set; }
        public UInt32 flags { get; set; }
        public UInt32 iconID { get; set; }
        public Byte  iconColor { get; set; }
        public Byte  carryItem { get; set; }
        public UInt32 sellPrice { get; set; }
        public UInt32 buyPrice    { get; set; }

        public bool Default { get => (flags&0x1)!=0x0;}
        public bool EZ { get => (flags&(0x1<<1))!=0x0;}
        //public bool Unknown { get => (flags&(0x1<<2))!=0x0;}
        public bool Usable { get => (flags&(0x1<<3))!=0x0;}
        public bool Appraisal { get => (flags&(0x1<<4))!=0x0;}
        //public bool Unknown { get => (flags&(0x1<<5))!=0x0;}
        public bool Mega { get => (flags&(0x1<<6))!=0x0;}
        public bool Level1 { get => (flags&(0x1<<7))!=0x0;}
        public bool Level2 { get => (flags&(0x1<<8))!=0x0;}
        public bool Level3 { get => (flags&(0x1<<9))!=0x0;}
        public bool Glitter { get => (flags&(0x1<<10))!=0x0;}
        public bool Deliverable { get => (flags&(0x1<<11))!=0x0;}
        public bool PouchVisible { get => (flags&(0x1<<12))!=0x0;}
        //public bool Unknown { get => (flags&(0x1<<13))!=0x0;}...

        private static readonly string _imageRoot = "/src/Resources/ItemIcons/";
        private static readonly string _StarIconPath = "star.png";
        private static readonly string _EmptyIcon = "255_0.png";
        public bool CanIncrease { get => !Default && id != 0;}
        public string StarPath { get => _imageRoot+(Mega?_StarIconPath:_EmptyIcon);}
        public bool Level { get => Level1 || Level2 || Level3;}
        public string LevelPath { get => _imageRoot+(Level? (Level3?"lvl3.png":(Level2?"lvl2.png":"lvl1.png")) : _EmptyIcon);}
        public string ItemImagePath {get => _imageRoot+iconID+"_"+iconColor+".png";}
        public int MinCount { get => Default?0:1;}
        public int MaxCount { get => Default?0:9999; }
        public byte DisplayRarity { get=> (byte)(rarity+1);}
        public string DisplayPrice { get=> sellPrice+"z";}

    }
}