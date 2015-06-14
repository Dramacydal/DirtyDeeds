using DD.Game.Enums;

namespace DD.Game.AutoTeleport
{
    public enum TeleportTargetType
    {
        None = 0,
        Monster = 1,
        Object = 2,
        Tile = 5,
        Exit = 6,
        XY = 7
    }

    public class TeleportInfo
    {
        public TeleportTargetType Type = TeleportTargetType.None;
        public uint Id = 0;
        public uint Id2 = 0;
        public uint Area = 0;

        public TeleportInfo() { }
        public TeleportInfo(TeleportTargetType Type, uint Id) { this.Type = Type; this.Id = Id; }
        public TeleportInfo(TeleportTargetType Type, uint Id, uint Id2) { this.Type = Type; this.Id = Id; this.Id2 = Id2; }
        public TeleportInfo(TeleportTargetType Type, uint Id, uint Id2, uint Area) { this.Type = Type; this.Id = Id; this.Id2 = Id2; this.Area = Area; }

        public TeleportInfo Clone()
        {
            return new TeleportInfo(Type, Id, Id2, Area);
        }

        public static TeleportInfo[] Vectors = new TeleportInfo[]
        {
            new TeleportInfo(), new TeleportInfo(), new TeleportInfo(), new TeleportInfo(),                                        //0x0
            new TeleportInfo(), new TeleportInfo(), new TeleportInfo(), new TeleportInfo(),                                        //0x01

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_COLD_PLAINS), 
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_DEN_OF_EVIL), 
            new TeleportInfo(), 
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_ROGUE_ENCAMPMENT),                        //0x02

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_STONY_FIELD),                                //0x3
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_BURIAL_GROUNDS),
            new TeleportInfo(TeleportTargetType.Object, 119),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_BLOOD_MOOR),

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_UNDERGROUND_PASSAGE_LEVEL_1),
            new TeleportInfo(TeleportTargetType.Object, 22),                                        //0x4
            new TeleportInfo(TeleportTargetType.Object, 119),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_COLD_PLAINS),

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_BLACK_MARSH),                                //0x5
            new TeleportInfo(TeleportTargetType.Object, 30),                                        
            new TeleportInfo(TeleportTargetType.Object, 119),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_UNDERGROUND_PASSAGE_LEVEL_1),

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_TAMOE_HIGHLAND),                            //0x6
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_FORGOTTEN_TOWER),
            new TeleportInfo(TeleportTargetType.Object, 119),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_DARK_WOOD),

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_MONASTERY_GATE),                            //0x7
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_PIT_LEVEL_1),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_BLACK_MARSH),

            new TeleportInfo(),                                                    //0x08
            new TeleportInfo(), 
            new TeleportInfo(), 
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_BLOOD_MOOR),                                

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_CAVE_LEVEL_2),                            //0x9
            new TeleportInfo(TeleportTargetType.Monster, 736),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_COLD_PLAINS),

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_DARK_WOOD),                                //0xa
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_UNDERGROUND_PASSAGE_LEVEL_2),
            new TeleportInfo(TeleportTargetType.Object, 157),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_STONY_FIELD),

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_HOLE_LEVEL_2),                            //0xb
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_HOLE_LEVEL_2),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_BLACK_MARSH),

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_PIT_LEVEL_2),                                //0xc
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_PIT_LEVEL_2),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_TAMOE_HIGHLAND),

            new TeleportInfo(TeleportTargetType.Object, 397),                                        //0xd
            new TeleportInfo(TeleportTargetType.Object, 397),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_CAVE_LEVEL_1),

            new TeleportInfo(TeleportTargetType.Object, 397),                                        //0xe
            new TeleportInfo(TeleportTargetType.Object, 397),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_UNDERGROUND_PASSAGE_LEVEL_1),


            new TeleportInfo(TeleportTargetType.Object, 397),                                        //0xf
            new TeleportInfo(TeleportTargetType.Object, 397),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_HOLE_LEVEL_1),

            new TeleportInfo(TeleportTargetType.Object, 397),                                        //0x10
            new TeleportInfo(TeleportTargetType.Object, 397),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_PIT_LEVEL_1),

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_CRYPT), 
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_MAUSOLEUM), 
            new TeleportInfo(TeleportTargetType.Monster, 805), 
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_COLD_PLAINS),                                //0x11

            new TeleportInfo(TeleportTargetType.Object, 397), 
            new TeleportInfo(TeleportTargetType.Object, 397), 
            new TeleportInfo(), 
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_BURIAL_GROUNDS),                            //0x12

            new TeleportInfo(TeleportTargetType.Object, 397), 
            new TeleportInfo(TeleportTargetType.Object, 397), 
            new TeleportInfo(), 
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_BURIAL_GROUNDS),                            //0x13

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_TOWER_CELLAR_LEVEL_1),                    //0x14
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_TOWER_CELLAR_LEVEL_1),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_BLACK_MARSH),

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_TOWER_CELLAR_LEVEL_2),                    //0x15
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_TOWER_CELLAR_LEVEL_2),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_FORGOTTEN_TOWER),


            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_TOWER_CELLAR_LEVEL_3),                    //0x16
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_TOWER_CELLAR_LEVEL_3),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_TOWER_CELLAR_LEVEL_1),


            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_TOWER_CELLAR_LEVEL_4),                    //0x17
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_TOWER_CELLAR_LEVEL_4),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_TOWER_CELLAR_LEVEL_2),


            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_TOWER_CELLAR_LEVEL_5),                    //0x18
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_TOWER_CELLAR_LEVEL_5),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_TOWER_CELLAR_LEVEL_3),


            new TeleportInfo(TeleportTargetType.Monster, 740),                                    //0x19
            new TeleportInfo(TeleportTargetType.Monster, 740),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_TOWER_CELLAR_LEVEL_4),

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_OUTER_CLOISTER),                            //0x1a
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_OUTER_CLOISTER),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.XY, 15141, 5091, (uint)Map.A1_TAMOE_HIGHLAND),


            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_BARRACKS),                                //0x1b
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_BARRACKS),
            new TeleportInfo(TeleportTargetType.Object, 119),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_MONASTERY_GATE),


            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_JAIL_LEVEL_1),                            //0x1c
            new TeleportInfo(TeleportTargetType.Object, 108),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_OUTER_CLOISTER),


            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_JAIL_LEVEL_2),                            //0x1d
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_JAIL_LEVEL_2),
            new TeleportInfo(TeleportTargetType.Object, 157),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_BARRACKS),


            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_JAIL_LEVEL_3),                            //0x1e
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_JAIL_LEVEL_3),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_JAIL_LEVEL_1),


            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_INNER_CLOISTER),                            //0x1f
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_INNER_CLOISTER),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_JAIL_LEVEL_2),


            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_CATHEDRAL),                                //0x20
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_CATHEDRAL),
            new TeleportInfo(TeleportTargetType.Object, 119),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_JAIL_LEVEL_3),


            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_CATACOMBS_LEVEL_1),                        //0x21
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_CATACOMBS_LEVEL_1),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_INNER_CLOISTER),


            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_CATACOMBS_LEVEL_2),                        //0x22
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_CATACOMBS_LEVEL_2),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_CATHEDRAL),


            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_CATACOMBS_LEVEL_3),                        //0x23
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_CATACOMBS_LEVEL_3),
            new TeleportInfo(TeleportTargetType.Object, 157),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_CATACOMBS_LEVEL_1),


            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_CATACOMBS_LEVEL_4),                        //0x24
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_CATACOMBS_LEVEL_4),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_CATACOMBS_LEVEL_2),


            new TeleportInfo(TeleportTargetType.XY, 22533, 9556),                                        //0x25
            new TeleportInfo(TeleportTargetType.XY, 22563, 9556),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A1_CATACOMBS_LEVEL_3),

            new TeleportInfo(TeleportTargetType.Object, 268), 
            new TeleportInfo(TeleportTargetType.Object, 26), 
            new TeleportInfo(), 
            new TeleportInfo(TeleportTargetType.XY, 25173, 5083),                                        //0x26 -> tristram

            new TeleportInfo(), new TeleportInfo(), new TeleportInfo(), new TeleportInfo(),                                        //0x27 -> cow lvl


            ////////////////////////////////////////////////////////////////////
            // Act 2
            ////////////////////////////////////////////////////////////////////

            new TeleportInfo(), new TeleportInfo(), new TeleportInfo(), new TeleportInfo(),                                        //0x28 -> lut gholein

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_DRY_HILLS),                                //0x29
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_DRY_HILLS),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_LUT_GHOLEIN),

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_FAR_OASIS),                                //0x2a
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_HALLS_OF_THE_DEAD_LEVEL_1),
            new TeleportInfo(TeleportTargetType.Object, 156),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_ROCKY_WASTE),

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_LOST_CITY),                                //0x2b
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_MAGGOT_LAIR_LEVEL_1),
            new TeleportInfo(TeleportTargetType.Object, 156),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_DRY_HILLS),

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_VALLEY_OF_SNAKES),                        //0x2c
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_ANCIENT_TUNNELS),
            new TeleportInfo(TeleportTargetType.Object, 156),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_FAR_OASIS),

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_CLAW_VIPER_TEMPLE_LEVEL_1),                //0x2d
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_CLAW_VIPER_TEMPLE_LEVEL_1),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_LOST_CITY),

            new TeleportInfo(),                                                    //0x2e -> canyon of the magi
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Object, 402),                                        
            new TeleportInfo(TeleportTargetType.Object, 402),


            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_SEWERS_LEVEL_2),                            //0x2f
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_SEWERS_LEVEL_2),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_LUT_GHOLEIN),

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_SEWERS_LEVEL_3),                            //0x30
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_SEWERS_LEVEL_3),
            new TeleportInfo(TeleportTargetType.Object, 323),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_SEWERS_LEVEL_1),

            new TeleportInfo(TeleportTargetType.Object, 355),                                        //0x31
            new TeleportInfo(TeleportTargetType.Object, 355),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_SEWERS_LEVEL_2),

            new TeleportInfo(TeleportTargetType.Tile, 29),                                        //0x32
            new TeleportInfo(TeleportTargetType.Tile, 29),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_LUT_GHOLEIN),

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_PALACE_CELLAR_LEVEL_1),                    //0x33
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_PALACE_CELLAR_LEVEL_1),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_HAREM_LEVEL_1),

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_PALACE_CELLAR_LEVEL_2),                    //0x34
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_PALACE_CELLAR_LEVEL_2),
            new TeleportInfo(TeleportTargetType.Object, 288),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_HAREM_LEVEL_2),

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_PALACE_CELLAR_LEVEL_3),                    //0x35
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_PALACE_CELLAR_LEVEL_3),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_PALACE_CELLAR_LEVEL_1),

            new TeleportInfo(TeleportTargetType.Object, 298),                                        //0x36
            new TeleportInfo(TeleportTargetType.Object, 298),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_PALACE_CELLAR_LEVEL_2),

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_STONY_TOMB_LEVEL_2), 
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_STONY_TOMB_LEVEL_2), 
            new TeleportInfo(), 
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_ROCKY_WASTE),                                //0x37 -> stony tomb

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_HALLS_OF_THE_DEAD_LEVEL_2),                //0x38
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_HALLS_OF_THE_DEAD_LEVEL_2),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_DRY_HILLS),

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_HALLS_OF_THE_DEAD_LEVEL_3),                //0x39
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_HALLS_OF_THE_DEAD_LEVEL_3),
            new TeleportInfo(TeleportTargetType.Object, 156),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_HALLS_OF_THE_DEAD_LEVEL_1),

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_CLAW_VIPER_TEMPLE_LEVEL_2),                //0x3a
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_CLAW_VIPER_TEMPLE_LEVEL_2),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_VALLEY_OF_SNAKES),

            new TeleportInfo(TeleportTargetType.Object, 397), 
            new TeleportInfo(TeleportTargetType.Object, 397), 
            new TeleportInfo(), 
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_STONY_TOMB_LEVEL_1),                        //0x3b -> stony tomb lvl 2

            new TeleportInfo(TeleportTargetType.Object, 354),                                        //0x3c
            new TeleportInfo(TeleportTargetType.Object, 354),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_HALLS_OF_THE_DEAD_LEVEL_2),

            new TeleportInfo(TeleportTargetType.Object, 149),                                        //0x3d
            new TeleportInfo(TeleportTargetType.Object, 149),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_CLAW_VIPER_TEMPLE_LEVEL_1),

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_MAGGOT_LAIR_LEVEL_2),                        //0x3e
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_MAGGOT_LAIR_LEVEL_2),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_FAR_OASIS),

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_MAGGOT_LAIR_LEVEL_3),                        //0x3f
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_MAGGOT_LAIR_LEVEL_3),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_MAGGOT_LAIR_LEVEL_1),

            new TeleportInfo(TeleportTargetType.Object, 356),                                        //0x40
            new TeleportInfo(TeleportTargetType.Object, 356),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_MAGGOT_LAIR_LEVEL_2),

            new TeleportInfo(), new TeleportInfo(), new TeleportInfo(), new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_LOST_CITY),                                        //0x41 ancient tunnels

            new TeleportInfo(TeleportTargetType.Object, 152),                                        //0x42
            new TeleportInfo(TeleportTargetType.Object, 152),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_CANYON_OF_THE_MAGI),


            new TeleportInfo(TeleportTargetType.Object, 152),                                        //0x43
            new TeleportInfo(TeleportTargetType.Object, 152),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_CANYON_OF_THE_MAGI),


            new TeleportInfo(TeleportTargetType.Object, 152),                                        //0x44
            new TeleportInfo(TeleportTargetType.Object, 152),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_CANYON_OF_THE_MAGI),


            new TeleportInfo(TeleportTargetType.Object, 152),                                        //0x45
            new TeleportInfo(TeleportTargetType.Object, 152),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_CANYON_OF_THE_MAGI),


            new TeleportInfo(TeleportTargetType.Object, 152),                                        //0x46
            new TeleportInfo(TeleportTargetType.Object, 152),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_CANYON_OF_THE_MAGI),


            new TeleportInfo(TeleportTargetType.Object, 152),                                        //0x47
            new TeleportInfo(TeleportTargetType.Object, 152),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_CANYON_OF_THE_MAGI),


            new TeleportInfo(TeleportTargetType.Object, 152),                                        //0x48                                        
            new TeleportInfo(TeleportTargetType.Object, 152),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A2_CANYON_OF_THE_MAGI),

            new TeleportInfo(), new TeleportInfo(), new TeleportInfo(), new TeleportInfo(),                                        //0x49 -> tal chambre

            new TeleportInfo(TeleportTargetType.Object, 357),                                        //0x4a
            new TeleportInfo(TeleportTargetType.Object, 357),
            new TeleportInfo(TeleportTargetType.Object, 402),
            new TeleportInfo(TeleportTargetType.Object, 298),

            ////////////////////////////////////////////////////////////////////
            // Act 3
            ////////////////////////////////////////////////////////////////////

            new TeleportInfo(), new TeleportInfo(), new TeleportInfo(), new TeleportInfo(),                                        //0x4b -> kurast docks


            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A3_GREAT_MARSH),                                //0x4c
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A3_SPIDER_CAVERN),
            new TeleportInfo(TeleportTargetType.Object, 237),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A3_KURAST_DOCKS),


            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A3_FLAYER_JUNGLE),                            //0x4d
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A3_FLAYER_JUNGLE),
            new TeleportInfo(TeleportTargetType.Object, 237),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A3_SPIDER_FOREST),


            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A3_LOWER_KURAST),                            //0x4e
            new TeleportInfo(TeleportTargetType.Object, 252),
            new TeleportInfo(TeleportTargetType.Object, 237),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A3_GREAT_MARSH),


            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A3_KURAST_BAZAAR),                            //0x4f
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A3_KURAST_BAZAAR),
            new TeleportInfo(TeleportTargetType.Object, 237),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A3_FLAYER_JUNGLE),


            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A3_UPPER_KURAST),                            //0x50
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A3_RUINED_TEMPLE),
            new TeleportInfo(TeleportTargetType.Object, 237),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A3_LOWER_KURAST),


            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A3_KURAST_CAUSEWAY),                            //0x51
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A3_KURAST_CAUSEWAY),
            new TeleportInfo(TeleportTargetType.Object, 237),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A3_KURAST_BAZAAR),


            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A3_TRAVINCAL),                                //0x52
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A3_TRAVINCAL),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A3_UPPER_KURAST),


            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A3_DURANCE_OF_HATE_LEVEL_1),                    //0x53
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A3_DURANCE_OF_HATE_LEVEL_1),
            new TeleportInfo(TeleportTargetType.Object, 237),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A3_KURAST_CAUSEWAY),

            new TeleportInfo(), new TeleportInfo(), new TeleportInfo(), new TeleportInfo(),                                        //0x54 -> arachnid lair
            new TeleportInfo(), new TeleportInfo(), new TeleportInfo(), new TeleportInfo(),                                        //0x55 -> spider cavern
            new TeleportInfo(), new TeleportInfo(), new TeleportInfo(), new TeleportInfo(),                                        //0x56 -> swampy pit lvl 1
            new TeleportInfo(), new TeleportInfo(), new TeleportInfo(), new TeleportInfo(),                                        //0x57 -> swampy pit lvl 2
            new TeleportInfo(), new TeleportInfo(), new TeleportInfo(), new TeleportInfo(),                                        //0x58 -> flayer dungeon lvl 1
            new TeleportInfo(), new TeleportInfo(), new TeleportInfo(), new TeleportInfo(),                                        //0x59 -> flayer dungeon lvl 2
            new TeleportInfo(), new TeleportInfo(), new TeleportInfo(), new TeleportInfo(),                                        //0x5a -> swampy pit lvl 3
            new TeleportInfo(), new TeleportInfo(), new TeleportInfo(), new TeleportInfo(),                                        //0x5b -> flayer dungeon lvl 3
            new TeleportInfo(), new TeleportInfo(), new TeleportInfo(), new TeleportInfo(),                                        //0x5c -> sewer lvl 1
            new TeleportInfo(), new TeleportInfo(), new TeleportInfo(), new TeleportInfo(),                                        //0x5d -> sewer lvl 2
            new TeleportInfo(), new TeleportInfo(), new TeleportInfo(), new TeleportInfo(),                                        //0x5e -> ruined temple
            new TeleportInfo(), new TeleportInfo(), new TeleportInfo(), new TeleportInfo(),                                        //0x5f -> disused fane
            new TeleportInfo(), new TeleportInfo(), new TeleportInfo(), new TeleportInfo(),                                        //0x60 -> forgotten reliquary
            new TeleportInfo(), new TeleportInfo(), new TeleportInfo(), new TeleportInfo(),                                        //0x61 -> forgotton temple
            new TeleportInfo(), new TeleportInfo(), new TeleportInfo(), new TeleportInfo(),                                        //0x62 -> ruined fane
            new TeleportInfo(), new TeleportInfo(), new TeleportInfo(), new TeleportInfo(),                                        //0x63 -> disused reliquary

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A3_DURANCE_OF_HATE_LEVEL_2),                    //0x64
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A3_DURANCE_OF_HATE_LEVEL_2),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A3_TRAVINCAL),

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A3_DURANCE_OF_HATE_LEVEL_3),                    //0x65
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A3_DURANCE_OF_HATE_LEVEL_3),
            new TeleportInfo(TeleportTargetType.Object, 324),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A3_DURANCE_OF_HATE_LEVEL_1),

            new TeleportInfo(TeleportTargetType.XY, 17591, 8069),                                        //0x66
            new TeleportInfo(TeleportTargetType.XY, 17591, 8069),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A3_DURANCE_OF_HATE_LEVEL_2),


            ////////////////////////////////////////////////////////////////////
            // Act 4
            ////////////////////////////////////////////////////////////////////

            new TeleportInfo(), new TeleportInfo(), new TeleportInfo(), new TeleportInfo(),                                        //0x67 pandemonium fortress


            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A4_PLAINS_OF_DESPAIR),                        //0x68
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A4_PLAINS_OF_DESPAIR),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A4_THE_PANDEMONIUM_FORTRESS),


            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A4_CITY_OF_THE_DAMNED),                        //0x69
            new TeleportInfo(TeleportTargetType.Monster, 256),
            new TeleportInfo(TeleportTargetType.Object, 238),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A4_OUTER_STEPPES),


            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A4_RIVER_OF_FLAME),                            //0x6a
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A4_RIVER_OF_FLAME),
            new TeleportInfo(TeleportTargetType.Object, 238),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A4_PLAINS_OF_DESPAIR),


            new TeleportInfo(TeleportTargetType.Object, 255, 0, (uint)Map.A4_THE_CHAOS_SANCTUARY),    //0x6b
            new TeleportInfo(TeleportTargetType.Object, 376),
            new TeleportInfo(TeleportTargetType.Object, 238),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A4_CITY_OF_THE_DAMNED),


            new TeleportInfo(TeleportTargetType.Object, 392, 1337),                                        //0x6c
            new TeleportInfo(TeleportTargetType.Object, 255),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A4_RIVER_OF_FLAME),


            ////////////////////////////////////////////////////////////////////
            // Act 5
            ////////////////////////////////////////////////////////////////////

            new TeleportInfo(), new TeleportInfo(), new TeleportInfo(), new TeleportInfo(),                                        //0x6d

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_FRIGID_HIGHLANDS),                        //0x6e
            new TeleportInfo(TeleportTargetType.Monster, 776),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_HARROGATH),

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_ARREAT_PLATEAU),                            //0x6f
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_ARREAT_PLATEAU),
            new TeleportInfo(TeleportTargetType.Object, 496),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_THE_BLOODY_FOOTHILLS),

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_CRYSTALLINE_PASSAGE),                        //0x70
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_CRYSTALLINE_PASSAGE),
            new TeleportInfo(TeleportTargetType.Object, 496),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_FRIGID_HIGHLANDS),

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_GLACIAL_TRAIL),                            //0x71
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_FROZEN_RIVER),
            new TeleportInfo(TeleportTargetType.Object, 511),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_ARREAT_PLATEAU),

            new TeleportInfo(TeleportTargetType.Object, 460),                                        //0x72
            new TeleportInfo(TeleportTargetType.Object, 460),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_CRYSTALLINE_PASSAGE),

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_FROZEN_TUNDRA),                            //0x73
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_FROZEN_TUNDRA),
            new TeleportInfo(TeleportTargetType.Object, 511),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_CRYSTALLINE_PASSAGE),

            new TeleportInfo(), new TeleportInfo(), new TeleportInfo(), new TeleportInfo(),                                        //0x74 -> drifter cavern

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_THE_ANCIENTS_WAY),                        //0x75
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_THE_ANCIENTS_WAY),
            new TeleportInfo(TeleportTargetType.Object, 496),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_ARREAT_PLATEAU),

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_ARREAT_SUMMIT),                            //0x76
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_ARREAT_SUMMIT),
            new TeleportInfo(TeleportTargetType.Object, 511),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_FROZEN_TUNDRA),

            new TeleportInfo(), new TeleportInfo(), new TeleportInfo(), new TeleportInfo(),                                        //0x77 -> icy cellar

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_WORLDSTONE_KEEP_LEVEL_1),                    //0x78
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_WORLDSTONE_KEEP_LEVEL_1),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_THE_ANCIENTS_WAY),

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_HALLS_OF_ANGUISH),                        //0x79
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_HALLS_OF_ANGUISH),
            new TeleportInfo(),
            new TeleportInfo(),

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_HALLS_OF_PAIN),                            //0x7a
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_HALLS_OF_PAIN),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_NIHLATHAKS_TEMPLE),

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_HALLS_OF_VAUGHT),                            //0x7b
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_HALLS_OF_VAUGHT),
            new TeleportInfo(TeleportTargetType.Object, 496),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_HALLS_OF_ANGUISH),

            new TeleportInfo(TeleportTargetType.Object, 462),                                        //0x7c
            new TeleportInfo(TeleportTargetType.Object, 462),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_HALLS_OF_PAIN),

            new TeleportInfo(), new TeleportInfo(), new TeleportInfo(), new TeleportInfo(),                                        //0x7d -> abaddon
            new TeleportInfo(), new TeleportInfo(), new TeleportInfo(), new TeleportInfo(),                                        //0x7e -> pit of acheron
            new TeleportInfo(), new TeleportInfo(), new TeleportInfo(), new TeleportInfo(),                                        //0x7f -> infernal pit

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_WORLDSTONE_KEEP_LEVEL_2),                    //0x80
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_WORLDSTONE_KEEP_LEVEL_2),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_ARREAT_PLATEAU),

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_WORLDSTONE_KEEP_LEVEL_3),                    //0x81
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_WORLDSTONE_KEEP_LEVEL_3),
            new TeleportInfo(TeleportTargetType.Object, 494),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_WORLDSTONE_KEEP_LEVEL_1),

            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_THRONE_OF_DESTRUCTION),                    //0x82
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_THRONE_OF_DESTRUCTION),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_WORLDSTONE_KEEP_LEVEL_2),

            new TeleportInfo(TeleportTargetType.XY, 15091, 5006),                                        //0x83
            new TeleportInfo(TeleportTargetType.XY, 15114, 5069),
            new TeleportInfo(),
            new TeleportInfo(TeleportTargetType.Exit, (uint)Map.A5_WORLDSTONE_KEEP_LEVEL_3),

            new TeleportInfo(), new TeleportInfo(), new TeleportInfo(), new TeleportInfo(),                                        //0x84 -> worldstone keep

            new TeleportInfo(TeleportTargetType.Object, 397),                                        //0x85
            new TeleportInfo(TeleportTargetType.Object, 397),
            new TeleportInfo(),
            new TeleportInfo(),

            new TeleportInfo(), new TeleportInfo(), new TeleportInfo(), new TeleportInfo(),                                        //0x86 -> forgotton sands

            new TeleportInfo(TeleportTargetType.Object, 397),                                        //0x87
            new TeleportInfo(TeleportTargetType.Object, 397),
            new TeleportInfo(),
            new TeleportInfo()
        };
    }
}
