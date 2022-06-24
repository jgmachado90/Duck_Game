using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class LayerAttribute : PropertyAttribute
    {
        public static List<string> GetLayerList()
        {
            List<string> layerNames =new List<string>();
            for(int i=0;i<=31;i++)
            {
                var layerN= LayerMask.LayerToName(i);
                if (layerN.Length <= 0)
                {
                    layerN = $"Layer {i}";
                }
            
                layerNames.Add(layerN);
            }

            return layerNames;
        }
    }
}
