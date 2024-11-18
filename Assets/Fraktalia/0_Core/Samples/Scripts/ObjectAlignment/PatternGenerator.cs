using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Fraktalia.Core.Samples
{
    public class PatternGenerator : MonoBehaviour
    {

        public bool AutoUpdate = false;
        public bool Apply = false;

        void OnDrawGizmosSelected()
        {
            if (AutoUpdate)
            {
                ApplyPattern();
            }

            if (Apply)
            {
                Apply = false;
                ApplyPattern();
            }
        }

        public void ApplyPattern()
        {
            List<Transform> children = new List<Transform>();
            for (int i = 0; i < transform.childCount; i++)
            {
                children.Add(transform.GetChild(i));
            }

            for (int i = 0; i < children.Count; i++)
            {

                children[i].localPosition = Pattern(i, children.Count);
            }
        }


        public virtual Vector3 Pattern(float Index, float childcount)
        {
            return new Vector3();


        }
    }
}