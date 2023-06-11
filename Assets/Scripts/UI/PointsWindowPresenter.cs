using System;
using System.Collections;
using System.Collections.Generic;
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

        private PointsService _pointsService;
        
        private void Start()
        {
            _pointsService = ReferenceProvider.GetWithGeneration<PointsService>();
            _pointsService.OnAmountChanged += UpdatePointsInfo;
            
            _addPoint.onClick.AddListener(() => AddPoints(1));
            _add10Points.onClick.AddListener(() => AddPoints(10));

            UpdatePointsInfo(0, 0);
        }

        private void AddPoints(int points) => _pointsService.AddPoints(points);

        private void UpdatePointsInfo(int points, int totalPoints) => _pointDisplayer.text = $"{points}/{totalPoints}";
    }
}