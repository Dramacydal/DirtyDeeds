﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itchy.D2Enums
{
    public enum Map
    {
        UNKNOWN = 0x00,

        ///////////////////////////////////////////////////
        // Act 1 Maps
        ///////////////////////////////////////////////////
        A1_ROGUE_ENCAMPMENT = 0x01,
        A1_BLOOD_MOOR = 0x02,
        A1_COLD_PLAINS = 0x03,
        A1_STONY_FIELD = 0x04,
        A1_DARK_WOOD = 0x05,
        A1_BLACK_MARSH = 0x06,
        A1_TAMOE_HIGHLAND = 0x07,
        A1_DEN_OF_EVIL = 0x08,
        A1_CAVE_LEVEL_1 = 0x09, // Cave in Cold plains
        A1_UNDERGROUND_PASSAGE_LEVEL_1 = 0x0a,
        A1_HOLE_LEVEL_1 = 0x0b,
        A1_PIT_LEVEL_1 = 0x0c,
        A1_CAVE_LEVEL_2 = 0x0d, // Cave in Cold plains
        A1_UNDERGROUND_PASSAGE_LEVEL_2 = 0x0e,
        A1_HOLE_LEVEL_2 = 0x0f,
        A1_PIT_LEVEL_2 = 0x10,
        A1_BURIAL_GROUNDS = 0x11,
        A1_CRYPT = 0x12,
        A1_MAUSOLEUM = 0x13,
        A1_FORGOTTEN_TOWER = 0x14,
        A1_TOWER_CELLAR_LEVEL_1 = 0x15,
        A1_TOWER_CELLAR_LEVEL_2 = 0x16,
        A1_TOWER_CELLAR_LEVEL_3 = 0x17,
        A1_TOWER_CELLAR_LEVEL_4 = 0x18,
        A1_TOWER_CELLAR_LEVEL_5 = 0x19,
        A1_MONASTERY_GATE = 0x1a,
        A1_OUTER_CLOISTER = 0x1b,
        A1_BARRACKS = 0x1c,
        A1_JAIL_LEVEL_1 = 0x1d,
        A1_JAIL_LEVEL_2 = 0x1e,
        A1_JAIL_LEVEL_3 = 0x1f,
        A1_INNER_CLOISTER = 0x20,
        A1_CATHEDRAL = 0x21,
        A1_CATACOMBS_LEVEL_1 = 0x22,
        A1_CATACOMBS_LEVEL_2 = 0x23,
        A1_CATACOMBS_LEVEL_3 = 0x24,
        A1_CATACOMBS_LEVEL_4 = 0x25,
        A1_TRISTRAM = 0x26,
        A1_THE_SECRET_COW_LEVEL = 0x27,


        ///////////////////////////////////////////////////
        // Act 2 Maps
        ///////////////////////////////////////////////////
        A2_LUT_GHOLEIN = 0x28,
        A2_ROCKY_WASTE = 0x29,
        A2_DRY_HILLS = 0x2a,
        A2_FAR_OASIS = 0x2b,
        A2_LOST_CITY = 0x2c,
        A2_VALLEY_OF_SNAKES = 0x2d,
        A2_CANYON_OF_THE_MAGI = 0x2e,
        A2_SEWERS_LEVEL_1 = 0x2f,
        A2_SEWERS_LEVEL_2 = 0x30,
        A2_SEWERS_LEVEL_3 = 0x31,
        A2_HAREM_LEVEL_1 = 0x32,
        A2_HAREM_LEVEL_2 = 0x33,
        A2_PALACE_CELLAR_LEVEL_1 = 0x34,
        A2_PALACE_CELLAR_LEVEL_2 = 0x35,
        A2_PALACE_CELLAR_LEVEL_3 = 0x36,
        A2_STONY_TOMB_LEVEL_1 = 0x37,
        A2_HALLS_OF_THE_DEAD_LEVEL_1 = 0x38,
        A2_HALLS_OF_THE_DEAD_LEVEL_2 = 0x39,
        A2_CLAW_VIPER_TEMPLE_LEVEL_1 = 0x3a,
        A2_STONY_TOMB_LEVEL_2 = 0x3b,
        A2_HALLS_OF_THE_DEAD_LEVEL_3 = 0x3c,
        A2_CLAW_VIPER_TEMPLE_LEVEL_2 = 0x3d,
        A2_MAGGOT_LAIR_LEVEL_1 = 0x3e,
        A2_MAGGOT_LAIR_LEVEL_2 = 0x3f,
        A2_MAGGOT_LAIR_LEVEL_3 = 0x40,
        A2_ANCIENT_TUNNELS = 0x41,
        A2_TAL_RASHAS_TOMB_1 = 0x42,
        A2_TAL_RASHAS_TOMB_2 = 0x43,
        A2_TAL_RASHAS_TOMB_3 = 0x44,
        A2_TAL_RASHAS_TOMB_4 = 0x45,
        A2_TAL_RASHAS_TOMB_5 = 0x46,
        A2_TAL_RASHAS_TOMB_6 = 0x47,
        A2_TAL_RASHAS_TOMB_7 = 0x48,
        A2_TAL_RASHAS_CHAMBER = 0x49,
        A2_ARCANE_SANCTUARY = 0x4a,


        ///////////////////////////////////////////////////
        // Act 3 Maps
        ///////////////////////////////////////////////////
        A3_KURAST_DOCKS = 0x4b,
        A3_SPIDER_FOREST = 0x4c,
        A3_GREAT_MARSH = 0x4d,
        A3_FLAYER_JUNGLE = 0x4e,
        A3_LOWER_KURAST = 0x4f,
        A3_KURAST_BAZAAR = 0x50,
        A3_UPPER_KURAST = 0x51,
        A3_KURAST_CAUSEWAY = 0x52,
        A3_TRAVINCAL = 0x53,
        A3_ARCHNID_LAIR = 0x54,
        A3_SPIDER_CAVERN = 0x55,
        A3_SWAMPY_PIT_LEVEL_1 = 0x56,
        A3_SWAMPY_PIT_LEVEL_2 = 0x57,
        A3_FLAYER_DUNGEON_LEVEL_1 = 0x58,
        A3_FLAYER_DUNGEON_LEVEL_2 = 0x59,
        A3_SWAMPY_PIT_LEVEL_3 = 0x5a,
        A3_FLAYER_DUNGEON_LEVEL_3 = 0x5b,
        A3_SEWERS_LEVEL_1 = 0x5c,
        A3_SEWERS_LEVEL_2 = 0x5d,
        A3_RUINED_TEMPLE = 0x5e,
        A3_DISUSED_FANE = 0x5f,
        A3_FORGOTTEN_RELIQUARY = 0x60,
        A3_FORGOTTEN_TEMPLE = 0x61,
        A3_RUINED_FANE = 0x62,
        A3_DISUSED_RELIQUARY = 0x63,
        A3_DURANCE_OF_HATE_LEVEL_1 = 0x64,
        A3_DURANCE_OF_HATE_LEVEL_2 = 0x65,
        A3_DURANCE_OF_HATE_LEVEL_3 = 0x66,

        ///////////////////////////////////////////////////
        // Act 4 Maps
        ///////////////////////////////////////////////////
        A4_THE_PANDEMONIUM_FORTRESS = 0x67,
        A4_OUTER_STEPPES = 0x68,
        A4_PLAINS_OF_DESPAIR = 0x69,
        A4_CITY_OF_THE_DAMNED = 0x6a,
        A4_RIVER_OF_FLAME = 0x6b,
        A4_THE_CHAOS_SANCTUARY = 0x6c,

        ///////////////////////////////////////////////////
        // Act 5 Maps
        ///////////////////////////////////////////////////
        A5_HARROGATH = 0x6d,
        A5_THE_BLOODY_FOOTHILLS = 0x6e,
        A5_FRIGID_HIGHLANDS = 0x6f,
        A5_ARREAT_PLATEAU = 0x70,
        A5_CRYSTALLINE_PASSAGE = 0x71,
        A5_FROZEN_RIVER = 0x72,
        A5_GLACIAL_TRAIL = 0x73,
        A5_DRIFTER_CAVERN = 0x74,
        A5_FROZEN_TUNDRA = 0x75,
        A5_THE_ANCIENTS_WAY = 0x76,
        A5_ICY_CELLAR = 0x77,
        A5_ARREAT_SUMMIT = 0x78,
        A5_NIHLATHAKS_TEMPLE = 0x79,
        A5_HALLS_OF_ANGUISH = 0x7a,
        A5_HALLS_OF_PAIN = 0x7b,
        A5_HALLS_OF_VAUGHT = 0x7c,
        A5_ABADDON = 0x7d,
        A5_PIT_OF_ACHERON = 0x7e,
        A5_INFERNAL_PIT = 0x7f,
        A5_WORLDSTONE_KEEP_LEVEL_1 = 0x80,
        A5_WORLDSTONE_KEEP_LEVEL_2 = 0x81,
        A5_WORLDSTONE_KEEP_LEVEL_3 = 0x82,
        A5_THRONE_OF_DESTRUCTION = 0x83,
        A5_WORLDSTONE_KEEP = 0x84,
        A5_MATRONS_DEN = 0x85,
        A5_FORGOTTEN_SANDS = 0x86,
        A5_FURNACE_OF_PAIN = 0x87,
        A5_TRISTRAM = 0x88,
    }
}