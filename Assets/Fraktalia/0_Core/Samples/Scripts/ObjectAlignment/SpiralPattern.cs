using UnityEngine;
using System.Collections;
namespace Fraktalia.Core.Samples
{
    public class SpiralPattern : PatternGenerator
    {

        public float Expansion;
        public float Curve;
        public float MinDist;
        public float Ascension = 0;


        public override Vector3 Pattern(float Index, float childcount)
        {
            float mindist = MinDist * 0.1f;

            Index++;
            float Angle = (360.0f / childcount) * Index * Curve * 0.1f;
            Vector3 output = new Vector2();
            output.y = Ascension * Index * 0.01f;
            output.z = Mathf.Sin(Angle * Mathf.PI / 180);
            output.x = Mathf.Cos(Angle * Mathf.PI / 180);
            output *= Index * Expansion / 100;
            if (output.sqrMagnitude < mindist * mindist)
            {
                output.Normalize();
                output *= mindist;
            }
            return output;
        }
    }
}