using System;
using UnityEngine;

namespace Skills.Core
{
    public class PointsModel
    {
        public int TotalPoints { get; private set; }
        public int Points { get; private set; }
        
        public void AddPoints(int points)
        {
            TotalPoints += points;
            Points += points;
            
            ValidatePoints();
        }

        public void ReturnPoints(int points)
        {
            Points += points;
            ValidatePoints();
        }

        public void SpendPoints(int points)
        {
            Points -= points;
            ValidatePoints();
        }
        
        private void ValidatePoints()
        {
            if (Points < 0 )
            {
                throw new ArgumentOutOfRangeException("There are unrecorded points!");
            }
            if (Points > TotalPoints)
            {
                throw new ArgumentOutOfRangeException($"Real total points amount and {nameof(TotalPoints)} are not equal!");
            }
        }
    }
}
