using UnityEngine;

namespace Utils
{
    public class MeshSorting : MonoBehaviour
    {
        public bool useSortingOrder;
        public int sortingOrder;
        public bool useSortingLayer;
        public string sortingLayer = "Default";

        private void OnValidate()
        {
            if (TryGetComponent(out Renderer render))
            {
                if(useSortingLayer)
                    render.sortingLayerID = SortingLayer.NameToID(sortingLayer);
                if(useSortingOrder)
                    render.sortingOrder = sortingOrder;
            }
        }
    }
}
