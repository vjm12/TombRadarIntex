using Microsoft.ML.OnnxRuntime.Tensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace UserManagement.MVC.Data
{
    public class BurialData
    {
        public float depth { get; set; }
        public float southtohead { get; set; }
        public float westtohead { get; set; }
        public float length { get; set; }
        public float westtofeet { get; set; }
        public float southtofeet { get; set; }
        public float eastwest_W { get; set; }
        public float adultsubadult_C { get; set; }
        public float wrapping_H { get; set; }
        public float wrapping_S { get; set; }
        public float wrapping_W { get; set; }
        public float area_NNW { get; set; }
        public float area_NW { get; set; }
        public float area_SE { get; set; }
        public float area_SW { get; set; }
        public float ageatdeath_C { get; set; }
        public float ageatdeath_I { get; set; }
        public float ageatdeath_IN { get; set; }
        public float ageatdeath_N { get; set; }
        public Tensor<float> AsTensor()
        {
            float[] data = new float[]
            {
                depth, southtohead,  westtohead, length, westtohead, southtofeet, eastwest_W, adultsubadult_C, wrapping_H, wrapping_S, wrapping_W,
                 area_NNW, area_NW, area_SE, area_SW, ageatdeath_C,
                ageatdeath_I, ageatdeath_IN, ageatdeath_N
            };
            int[] dimensions = new int[] { 1, 19 };
            return new DenseTensor<float>(data, dimensions);
        }
    }
}
