
using UnityEngine;

namespace Utils{
    
    public static class UiUtils{
        public static Vector3[] ArrangePositions(int length, int[] maxHorizontal, bool center, float offsetX, float offsetY,
            float offsetXPostY = 0){
            int horizontalId = 0;
            int maxHorizontalElements = maxHorizontal.Length > 0 ? maxHorizontal[horizontalId] : int.MaxValue;

            int numElements = length - 1;
            float total = offsetX * Mathf.Min(numElements, maxHorizontalElements - 1);

            int x = 0;
            int y = 0;
            float ySumOffset = 0;

            Vector3[] icons = new Vector3[length];

            for (var i = 0; i < icons.Length; i++){
                Vector3 pos = Vector3.zero;
                pos.y += ySumOffset;

                pos.x += offsetX * x;
                pos.x += offsetXPostY * (y % 2);
                if (center)
                    pos.x -= total / 2f;

                icons[i].Set(pos.x, pos.y, pos.z);


                x++;
                if (x >= maxHorizontalElements){
                    numElements -= maxHorizontalElements;
                    if (horizontalId < maxHorizontal.Length - 1){
                        horizontalId++;
                        maxHorizontalElements = maxHorizontal[horizontalId];
                    }

                    if (center)
                        total = offsetX * Mathf.Min(numElements, maxHorizontalElements - 1);
                    x = 0;
                    y++;
                    ySumOffset += offsetY;
                }
            }

            return icons;
        }
    }
}