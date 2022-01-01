using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptUtils.GameUtils
{
    public class DefaultRandomizerRules : IRandomizer<int>
    {
        private List<int> asked = new List<int>();
        private int nrLeft;
        private int initialNrLeft;
        private int lastBlock;
        public DefaultRandomizerRules(int min, int max)
        {
            nrLeft = (max - min) + 1;
            initialNrLeft = nrLeft;
        }

        public virtual void Reset()
        {
            asked = new List<int>();
            nrLeft = initialNrLeft;
        }

        public bool IsBlocked(int arg)
        {
            return asked.Contains(arg);
        }

        public void addBlock(int arg)
        {
            if (asked.Contains(arg))
                return;
            asked.Add(arg);
            nrLeft--;
            lastBlock = arg;
        }

        public int getLastBlock()
        {
            return lastBlock;
        }

        public int getNumbersLeft()
        {
            return nrLeft;
        }

        public void removeBlock(int arg)
        {
            asked.Remove(arg);
            nrLeft++;
        }

        public bool hasNumbersLeft()
        {
            return nrLeft > 0;
        }
    }
}