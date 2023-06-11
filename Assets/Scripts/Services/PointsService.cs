using System;

namespace Skills.Core
{
    public class PointsService : ApplicationService
    {
        private int _totalPoints;
        private int _points;

        /// <summary>
        /// 1st int - current points amount; 2nd int - total points amount
        /// </summary>
        public event Action<int, int> OnAmountChanged;

        public void AddPoints(int points)
        {
            _totalPoints += points;
            _points += points;
            
            HandleAmountChanged();
        }

        public void ReturnPoints(int points)
        {
            _points += points;
            HandleAmountChanged();
        }

        public void SpendPoints(int points)
        {
            _points -= points;
            HandleAmountChanged();
        }

        public bool IsEnoughPoint(int points) => _points >= points;

        private void HandleAmountChanged()
        {
            ValidatePoints();
            OnAmountChanged?.Invoke(_points, _totalPoints);
        }

        private void ValidatePoints()
        {
            if (_points < 0 )
            {
                throw new ArgumentOutOfRangeException("There are unrecorded points!");
            }
            if (_points > _totalPoints)
            {
                throw new ArgumentOutOfRangeException($"Real total points amount and {nameof(_totalPoints)} are not equal!");
            }
        }
    }
}