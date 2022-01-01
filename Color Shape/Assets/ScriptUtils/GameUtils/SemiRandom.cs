using ScriptUtils.GameUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptUtils.GameUtils
{
    public class SemiRandom : IRandomizer<int>
    {
        private int asked;
        private int min;
        private int lastBlock;
        public SemiRandom(int min, int max)
        {
            this.min = min;
        }

        public void Reset()
        {
            asked = min - 1;
        }

        public bool IsBlocked(int arg)
        {
            return asked == arg;
        }

        public void addBlock(int arg)
        {
            asked = arg;
            lastBlock = arg;
        }

        public int getLastBlock()
        {
            return lastBlock;
        }

        public void removeBlock(int arg)
        {
            Reset();
        }

        public bool hasNumbersLeft()
        {
            return true;
        }

        public int getNumbersLeft()
        {
            return int.MaxValue;
        }
    }
}