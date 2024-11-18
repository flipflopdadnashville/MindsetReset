using UnityEngine;
using System.Collections;

namespace Fraktalia.Core.Samples
{
    public class CirclePattern : PatternGenerator
    {

        public float Expansion;
        public float startangle = 0;

        public override Vector3 Pattern(float Index, float childcount)
        {



            float Angle = (360.0f / childcount) * Index;
            Angle += startangle;
            Vector3 output = new Vector2();
            output.y = 0;
            output.z = Mathf.Sin(Angle * Mathf.PI / 180);
            output.x = Mathf.Cos(Angle * Mathf.PI / 180);
            output *= Expansion / 100;

            return output;
        }
    }
}
