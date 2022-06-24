using System.Collections.Generic;
using UnityEngine;

namespace Common{
    
    public static class ProbabilitySelector{
        private static float GetWeightSum(this List<float> weights){
            float sum = 0;
            foreach (var item in weights){
                sum += item;
            }
            return sum;
        }

        private static float GetProbability(this List<float> weights, int index){
            return weights[index] / weights.GetWeightSum();
        }

        public static int GetRandomIndexByWeights(this List<float> weights){
            int weightId = 0;

            float cumulativeProbability = weights.GetProbability(weightId);

            float random = Random.value;

            while (random > cumulativeProbability && weightId < weights.Count - 1){
                weightId++;
                cumulativeProbability += weights.GetProbability(weightId);
            }

            return weightId;
        }
    }
}
