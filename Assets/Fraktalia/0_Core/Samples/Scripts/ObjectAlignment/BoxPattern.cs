using UnityEngine;
using System.Collections;
namespace Fraktalia.Core.Samples
{
    public class BoxPattern : PatternGenerator
    {

        public int Rows;
        public float Distance;
        public float Ascension_X = 0;
        public float Ascension_Y = 0;

        private float posX = 0;
        private float posY = 0;

        public override Vector3 Pattern(float Index, float childcount)
        {

            if (Index == 0) posY = 0;
            if (Index % Rows == 0)
            {
                posY += Distance;
                posX = 0;
            }
            posX += Distance;

            float Ascension = Ascension_X * (posX - 1) + Ascension_Y * (posY - 1);

            Vector3 output = new Vector3();
            output.y = Ascension;
            output.z = posY - (Distance * (Rows + 1)) / 2;
            output.x = posX - (Distance * (Rows + 1)) / 2;



            return output;

        }
    }
}