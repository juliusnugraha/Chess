using System.Collections.Generic;
using JN.Utils;
using UnityEngine;

namespace JN.Chess
{
    public class SquareHighlightSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _prefabSquareHighlight;
        [SerializeField] private Material _availableMaterial;
        [SerializeField] private Material _opponentMaterial;
        [SerializeField] private Transform _containerHighlight;
        private ObjectPool<SquareHighlight> _poolSquareHighlight;

        void Awake()
        {
            _poolSquareHighlight = new ObjectPool<SquareHighlight>();
            _poolSquareHighlight.Init("highlight", _prefabSquareHighlight, _containerHighlight, 2);
        }

        public void ShowSelection(Dictionary<Vector3, bool> dictMovePosition)
        {
            foreach(var data in dictMovePosition)
            {
                Vector3 newPos = new Vector3(data.Key.x, data.Key.y + 0.1f, data.Key.z);

                SquareHighlight highlight = _poolSquareHighlight.GetNewObject();
                highlight.materialSetter.SetSingleMaterial(data.Value ? _availableMaterial : _opponentMaterial );
                highlight.gameObject.SetActive(true);
                highlight.transform.SetPositionAndRotation(newPos, Quaternion.identity);
            }
        }

        public void ClearSelection()
        {
            _poolSquareHighlight.KillAll();
        }

    }
}