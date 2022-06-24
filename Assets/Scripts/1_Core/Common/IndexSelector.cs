using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Common
{
    [System.Serializable]
    public class IndexSelector
    {
        public enum Method
        {
            RandomNoRepeat,
            Random,
            Sequential
        }

        [SerializeField] private Method method;
        private int _count;
        private Method _oldMethod;
        
        private readonly List<int> _unusedIndexes;
        
        public int LastValue { get; private set; }

        public IndexSelector()
        {
            _unusedIndexes = new List<int>();
        }
        
        public IndexSelector(Method method, int count)
        {
            Assert.IsTrue(count > 0, "Count must be greater than zero.");
            
            _unusedIndexes = new List<int>();
            this.method = method;
            _oldMethod = method;
            SetCount(count);
        }

        public void SetCount(int count)
        {
            Assert.IsTrue(count > 0, "Count must be greater than zero.");
            _count = count;
            LastValue = -1;
            ResetMethod();
        }
        
        public void SetMethod(Method newMethod)
        {
            method = newMethod;
            ResetMethod();
        }

        private void ResetMethod()
        {
            _oldMethod = method;
            if(method == Method.RandomNoRepeat) ResetShuffle();
        }

        public int GetNext()
        {
            if (method != _oldMethod) SetMethod(method);
            
            switch (method)
            {
                case Method.RandomNoRepeat:
                    LastValue = GetShuffleIndex();
                    break;

                case Method.Random:
                    LastValue = Random.Range(0, _count);
                    break;

                case Method.Sequential:
                    LastValue = ++LastValue % _count;
                    break;
            }

            return LastValue;
        }

        private int GetShuffleIndex()
        {
            if (_unusedIndexes.Count <= 0) ResetShuffle();

            int index;
            
            do index = Random.Range(0, _unusedIndexes.Count);
            while (_unusedIndexes.Count > 1 && index == LastValue);
            
            int value = _unusedIndexes[index];

            _unusedIndexes.RemoveAt(index);

            return value;
        }
        
        private void ResetShuffle()
        {
            _unusedIndexes.Clear();
            
            for (int i = 0; i < _count; i++)
            {
                _unusedIndexes.Add(i);
            }
        }
    }
}