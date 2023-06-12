using Skills.Core;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Skills.UI
{
    public class PointsWindowPresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _pointDisplayer;
        [SerializeField] private Button _addPoint;
        [SerializeField] private Button _add10Points;

        private ServiceReference<PointsService> _pointsService;
        
        private void Start()
        {
            _pointsService.Reference.OnAmountChanged += UpdatePointsInfo;
            
            _addPoint.onClick.AddListener(() => AddPoints(1));
            _add10Points.onClick.AddListener(() => AddPoints(10));

            UpdatePointsInfo(0, 0);
        }

        private void AddPoints(int points) => _pointsService.Reference.AddPoints(points);

        private void UpdatePointsInfo(int points, int totalPoints) => _pointDisplayer.text = $"{points}/{totalPoints}";
    }
}