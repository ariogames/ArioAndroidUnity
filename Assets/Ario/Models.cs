using System;
using System.Collections.Generic;
using System.Linq;

namespace Ario.Models
{

    [System.Serializable]
    public class AchievementList
    {
        [System.Serializable]
        public class Achievement
        {
            public string id;
            public string name;
            public string description;
            public string imageUrl;
            public int totalStep;
            public int currentStep;
            public int xp;
            public bool isUnlock;
        }

        public Achievement[] list = null;
    }

    [System.Serializable]
    public class LeaderboardList { 
        public const int SCORE_ORDER_SMALLER_IS_BETTER = 0;
        public const int SCORE_ORDER_LARGER_IS_BETTER = 1;

        [System.Serializable]
        public class Leaderboard {
            public string id;
            public string name;
            public string iconUrl;
            public int scoreOrder;
        }

        public Leaderboard[] list = null;

    }

    [System.Serializable]
    public class ScoreList {

        public const int COLLECTION_PUBLIC = 0;
        public const int COLLECTION_SOCIAL = 1;
        public const int TIME_STAMP_DAILY = 0;
        public const int TIME_STAMP_WEEKLY = 1;
        public const int TIME_STAMP_ALL_TIME = 2;
        
        [System.Serializable]
        public class Score {
            public string leaderboardId;
            public int rank;
            public int score;
            public string playerName;
            public string playerIconUrl;
            public int collection;
            public int timestamp;
        }

        public Score[] list = null;
    }
}