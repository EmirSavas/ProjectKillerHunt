using UnityEngine;

namespace GenericPuzzleMechanics
{
    public static class Mechanics
    {
        public delegate int ChangeIndexValue(int index);

        /// <summary>
        /// Increases And Decreases Index Based On Change Type
        /// </summary>
        /// <param name="value"> Your Index </param>
        /// <param name="minValue"> Included </param>
        /// <param name="maxValue"> Not Included </param>
        /// <param name="changeIndexValue">
        /// Change Types:
        /// IncreaseIndex
        /// DecreaseIndex
        /// </param>
        /// <param name="changeMin"> Should change to <paramref name="minValue"/> when the value is bigger than <paramref name="maxValue"/> </param>
        /// <param name="changeMax"> Should change to <paramref name="maxValue"/> when the value is lower than <paramref name="minValue"/> </param>
        /// <returns></returns>
        public static int ChangeIndex(int value, int minValue, int maxValue, ChangeIndexValue changeIndexValue, bool changeMin = true, bool changeMax = true)
        {
            value = changeIndexValue(value);

            if (value == maxValue)
            {
                value = minValue;
            }

            else if (value < minValue)
            {
                value = maxValue - 1;
            }

            return value;
        }

        public static int IncreaseIndex(int index)
        {
            index++;
            return index;
        }

        public static int DecreaseIndex(int index)
        {
            index--;
            return index;
        }
    }
}
