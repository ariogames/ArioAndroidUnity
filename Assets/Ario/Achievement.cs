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
            public bool isUnlock;
        }

        public Achievement[] list = null;
    }
}