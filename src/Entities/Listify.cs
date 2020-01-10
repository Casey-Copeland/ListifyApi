using System;
using System.Collections;
using System.Collections.Generic;

namespace ListifyApi.Entities
{
    public class Listify : IReadOnlyList<int>
    {
        private readonly int _minValue;
        private readonly int _maxValue;

        public Listify(int minValue, int maxValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;

            Count = maxValue - minValue;

            if (Count <= 0)
                throw new ArgumentOutOfRangeException($"Class Name: {nameof(Listify)} has a Constructor with the Parameter {nameof(maxValue)} with a value of {maxValue} which is less than the Parameter {nameof(minValue)} with the value of {minValue}");
        }

        public int this[int index] {
            get
            {
                if (index >= 0 && index <= Count)
                    return _minValue + index;
                else
                    throw new IndexOutOfRangeException($"The index {index} is out of the available range: ({_minValue} - {_maxValue}).");
            }
        }

        public int Count { get; private set; }

        public bool Contains(int value)
        {
            return value >= _minValue && value <= _maxValue;
        }

        public IEnumerator<int> GetEnumerator()
        {
            for (var index = _minValue; index < _maxValue; index++)
                yield return index;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
