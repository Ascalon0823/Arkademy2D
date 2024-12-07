using System;
using UnityEngine.Serialization;

namespace Arkademy.Common
{
    [Serializable]
    public class Progression : Attribute
    {
        public int currProgress;
        public int progressToNext;
        public int nextIncrementalMultiplier;

        public virtual int AddProgress(int progress)
        {
            if (progressToNext == 0)
            {
                return 1;
            }

            currProgress += progress;
            var increase = 0;
            while (currProgress >= progressToNext)
            {
                currProgress -= progressToNext;
                progressToNext += nextIncrementalMultiplier * progressToNext / 100;
                increase++;
            }

            return increase;
        }
    }
}