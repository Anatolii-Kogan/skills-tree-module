using Skills.Core;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Skills.UI
{
    public class PointsWindowPresenter : BaseMainPresenter
    {
        [SerializeField] private TextMeshProUGUI _pointDisplayer;
        [SerializeField] private Button _addPoint;
        [SerializeField] private Button _add10Points;

        private ServiceReference<PointsService> _pointsService;

        protected override void InitInternal()
        {
            base.InitInternal();
            _pointsService.Reference.OnAmountChanged += UpdatePointsInfo;
            
            _addPoint.onClick.AddListener(AddPoint);
            _add10Points.onClick.AddListener(Add10Points);

            UpdatePointsInfo(0, 0);
        }

        private void AddPoint() => _pointsService.Reference.AddPoints(1);
        private void Add10Points() => _pointsService.Reference.AddPoints(10);

        private void UpdatePointsInfo(int points, int totalPoints) => _pointDisplayer.text = $"{points}/{totalPoints}";
    }
}