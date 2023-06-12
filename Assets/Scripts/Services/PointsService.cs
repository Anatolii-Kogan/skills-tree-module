using System;

namespace Skills.Core
{
    public class PointsService : ApplicationService
    {
        private readonly PointsModel _model;

        /// <summary>
        /// 1st int - current points amount; 2nd int - total points amount
        /// </summary>
        public event Action<int, int> OnAmountChanged;

        public PointsService()
        {
            _model = new PointsModel();
        }

        public void AddPoints(int points)
        {
            _model.AddPoints(points);
            
            HandleAmountChanged();
        }

        public void ReturnPoints(int points)
        {
            _model.ReturnPoints(points);
            HandleAmountChanged();
        }

        public void SpendPoints(int points)
        {
            _model.SpendPoints(points);
            HandleAmountChanged();
        }

        public bool IsEnoughPoint(int points) => _model.Points >= points;

        private void HandleAmountChanged() => OnAmountChanged?.Invoke(_model.Points, _model.TotalPoints);
    }
}