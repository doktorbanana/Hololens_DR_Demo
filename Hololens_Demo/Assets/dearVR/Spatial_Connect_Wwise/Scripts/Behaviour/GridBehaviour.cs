using System;
using UnityEngine;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class GridBehaviour : MonoBehaviour, IGridBehaviour
    {
        [SerializeField] private GameObject gridLinePrefab = default;
        [SerializeField] private Transform dBLinesTransform = default;
        [SerializeField] private Transform distanceLinesTransform = default;
        
        private const int DISTANCE_DIVISION = 5;
        private readonly Vector3 DISTANCE_LABEL_POSITION = new Vector3(-50f, -15f, 0f);

        private const int DB_DIVISION = 4;
        private readonly Vector3 DB_LABEL_POSITION = new Vector3(-65f, 5f, 0f);
        private readonly string[] DB_LABELS = {"-200.0", "-12.0", "-6.0", "-2.5", "0.0"};

        private IDisposable gridPresenter_;
        private readonly IFactory factory_ = new Factory();

        private void Awake()
        {
            var editorRect = GetComponent<RectTransform>().rect;

            CreateDistanceLines();
            CreateDBLines();

            void CreateDistanceLines()
            {
                var interval = editorRect.width / DISTANCE_DIVISION;
                for (var i = 0; i < DISTANCE_DIVISION+1; ++i)
                {
                    var gridLine = Instantiate(gridLinePrefab, distanceLinesTransform);
                    var rectTransform = gridLine.GetComponent<RectTransform>();
                    rectTransform.anchoredPosition3D = new Vector3(interval* i, 0f, 0f);
                    
                    var gridLineBehaviour = gridLine.GetComponent<GridLineBehaviour>();
                    gridLineBehaviour.SetVertices(new Vector3(0f, 0f, -0.1f), new Vector3(0f, editorRect.height, -0.1f));
                    gridLineBehaviour.LabelPosition = DISTANCE_LABEL_POSITION;

                    if(i == 0 || i == DISTANCE_DIVISION)
                        gridLineBehaviour.DisableLine();
                }
            }

            void CreateDBLines()
            {
                var interval = editorRect.height / DB_DIVISION;
                for (var i = 0; i < DB_DIVISION + 1; ++i)
                {
                    var gridLine = Instantiate(gridLinePrefab, dBLinesTransform);
                    var rectTransform = gridLine.GetComponent<RectTransform>();
                    rectTransform.anchoredPosition3D = new Vector3(0f, interval* i, 0f);
                    
                    var gridLineBehaviour = gridLine.GetComponent<GridLineBehaviour>();
                    gridLineBehaviour.SetVertices(new Vector3(0f, 0f, -0.1f), new Vector3(editorRect.width, 0f, -0.1f));
                    gridLineBehaviour.LabelPosition = DB_LABEL_POSITION;
                    gridLineBehaviour.Label = DB_LABELS[i];
                    
                    if(i == 0 || i == DB_DIVISION)
                        gridLineBehaviour.DisableLine();
                }
            }
        }

        public void Init(IPositioningPropertySet positioningPropertySet)
        {
            gridPresenter_ = factory_.CreateGridPresenter(this, positioningPropertySet);
        }

        public uint? MaxDistance
        {
            set
            {
                var numLines = distanceLinesTransform.childCount;

                if (!value.HasValue)
                {
                    distanceLinesTransform.GetChild(numLines-1).GetComponent<GridLineBehaviour>().Label = "N/A";
                    return;
                }
                
                var interval = (float)value.Value /(numLines-1);
                for (var i = 0; i < numLines; ++i)
                    distanceLinesTransform.GetChild(i).GetComponent<GridLineBehaviour>().Label 
                        = (interval* i).ToString("0.0");
            }
        }

        private void OnDestroy()
        {
            gridPresenter_?.Dispose();
        }
    }
}

