using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleTest
{
    public class FriendRelationTest : TimeChecker
    {

        public enum TotalRelationType : sbyte
        {
            Block = -0x20,
            Hidden = -0x10,
            None = 0,
            Follower = 0x10,
            Follow,
            BothFollow,
            Friend = 0x20,
            Frater = 0x28,
            FamilyKin = 0x30,
            Family = 0x38,
            Brother = 0x40,
            Child = 0x70,
            Parent = 0x78
        }
        public enum PeopleRelationType : sbyte
        {
            None = 0,
            Friend = 0x20,
            Frater = 0x28,
            FamilyKin = 0x30,
            Family = 0x38,
            Brother = 0x40,
            Child = 0x70,
            Parent = 0x78
        }
        [Flags]
        public enum AccessRelationType : byte
        {
            None = 0x00,
            Invisible = MASK_EXCLUDE_TARGET,
            Block = MASK_EXCLUDE_TARGET | MASK_EXCLUDE_ME,
            #region MASK
            MASK_EXCLUDE_TARGET = 0x01,
            MASK_EXCLUDE_ME = 0x02,
            #endregion
        }
        public class AppUserRelation
        {
            public string UserId { get; set; }
            public string TargetUserId { get; set; }
            public string Name { get; set; }
            public TotalRelationType TotalRelation { get; set; }
            public PeopleRelationType PeopleRelation { get; set; }
            public AccessRelationType AccessRelation { get; set; }
            public bool IsFollow { get; set; }
            public bool IsAlarm { get; set; }
            public bool IsSubscribe { get; set; }
            public DateTime CreationTime { get; set; }
            public DateTime ModifiedTime { get; set; }
        }
        public FriendRelationTest()
        {
            string[] NAMES = new string[] { "AAA", "BBB", "CCC", "DDD", "EEE", "FFF", "GGG", "HHH", "III", "JJJ", "KKK", "LLL", "MMM", "NNN", "OOO", "PPP", "QQQ", "RRR", "SSS", "TTT", "UUU", "VVV", "WWW", "XXX", "YYY", "ZZZ" };

            foreach(var user in NAMES)
            {
                var names = NAMES.Where(v => v != user).ToArray();
                for(int i=0; i<names.Length; i++)
                {
                }
            }
        }
    }
}
