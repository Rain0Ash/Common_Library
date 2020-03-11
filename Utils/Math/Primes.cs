// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;

namespace Common_Library.Utils
{
    public static partial class PrimeUtils
    {
        private static Int32[] PrimeCapacities { get; set; }

        private static void Init()
        {
            PrimeCapacities = new[]
            {
                //chunk #0
                LargestPrime,

                //chunk #1
                5,
                11,
                23,
                47,
                97,
                197,
                397,
                797,
                1597,
                3203,
                6421,
                12853,
                25717,
                51437,
                102877,
                205759,
                411527,
                823117,
                1646237,
                3292489,
                6584983,
                13169977,
                26339969,
                52679969,
                105359939,
                210719881,
                421439783,
                842879579,
                1685759167,

                //chunk #2
                433,
                877,
                1759,
                3527,
                7057,
                14143,
                28289,
                56591,
                113189,
                226379,
                452759,
                905551,
                1811107,
                3622219,
                7244441,
                14488931,
                28977863,
                57955739,
                115911563,
                231823147,
                463646329,
                927292699,
                1854585413,

                //chunk #3
                953,
                1907,
                3821,
                7643,
                15287,
                30577,
                61169,
                122347,
                244703,
                489407,
                978821,
                1957651,
                3915341,
                7830701,
                15661423,
                31322867,
                62645741,
                125291483,
                250582987,
                501165979,
                1002331963,
                2004663929,

                //chunk #4
                1039,
                2081,
                4177,
                8363,
                16729,
                33461,
                66923,
                133853,
                267713,
                535481,
                1070981,
                2141977,
                4283963,
                8567929,
                17135863,
                34271747,
                68543509,
                137087021,
                274174111,
                548348231,
                1096696463,

                //chunk #5
                31,
                67,
                137,
                277,
                557,
                1117,
                2237,
                4481,
                8963,
                17929,
                35863,
                71741,
                143483,
                286973,
                573953,
                1147921,
                2295859,
                4591721,
                9183457,
                18366923,
                36733847,
                73467739,
                146935499,
                293871013,
                587742049,
                1175484103,

                //chunk #6
                599,
                1201,
                2411,
                4831,
                9677,
                19373,
                38747,
                77509,
                155027,
                310081,
                620171,
                1240361,
                2480729,
                4961459,
                9922933,
                19845871,
                39691759,
                79383533,
                158767069,
                317534141,
                635068283,
                1270136683,

                //chunk #7
                311,
                631,
                1277,
                2557,
                5119,
                10243,
                20507,
                41017,
                82037,
                164089,
                328213,
                656429,
                1312867,
                2625761,
                5251529,
                10503061,
                21006137,
                42012281,
                84024581,
                168049163,
                336098327,
                672196673,
                1344393353,

                //chunk #8
                3,
                7,
                17,
                37,
                79,
                163,
                331,
                673,
                1361,
                2729,
                5471,
                10949,
                21911,
                43853,
                87719,
                175447,
                350899,
                701819,
                1403641,
                2807303,
                5614657,
                11229331,
                22458671,
                44917381,
                89834777,
                179669557,
                359339171,
                718678369,
                1437356741,

                //chunk #9
                43,
                89,
                179,
                359,
                719,
                1439,
                2879,
                5779,
                11579,
                23159,
                46327,
                92657,
                185323,
                370661,
                741337,
                1482707,
                2965421,
                5930887,
                11861791,
                23723597,
                47447201,
                94894427,
                189788857,
                379577741,
                759155483,
                1518310967,

                //chunk #10
                379,
                761,
                1523,
                3049,
                6101,
                12203,
                24407,
                48817,
                97649,
                195311,
                390647,
                781301,
                1562611,
                3125257,
                6250537,
                12501169,
                25002389,
                50004791,
                100009607,
                200019221,
                400038451,
                800076929,
                1600153859
            };
        }
    }
}