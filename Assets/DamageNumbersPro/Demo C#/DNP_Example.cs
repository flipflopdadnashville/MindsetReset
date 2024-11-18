/* 
 * Hello dear user.
 * 
 * This is a script made to teach you how to use the asset.
 * If you need more help you can check out the documentation.
 * Or message me via discord or email.
 * 
 * Good Luck.
 * - Ekincan
 */

using UnityEngine;
using DamageNumbersPro; //Include DamageNumbersPro Namespace     <-----     [IMPORTANT]

namespace DamageNumbersPro.Demo
{
    public class DNP_Example : MonoBehaviour
    {
        public DamageNumber popupPrefab; //Reference DamageNumber Prefab     <-----     [IMPORTANT]

        //Allows us to reference the spawn target in the inspector.
        public Transform target;

        void Update()
        {
            //Spawns a random number between 1 and 10 on leftclick.
            if(DNP_InputHandler.GetLeftClick())
            {
                SpawnPopup(Mathf.Round(Random.Range(1, 10)));
            }
        }

        public void SpawnPopup(float number)
        {
            DamageNumber newPopup = popupPrefab.Spawn(target.position, number); //Spawn DamageNumber At Target     <-----     [IMPORTANT]


            //You can do any change you want on the DamageNumber returned by the Spawn(...) function.
            //The following code is [OPTIONAL] just to show you some examples.


            //Let's make the popup follow the target.
            newPopup.SetFollowedTarget(target);

            //Let's check if the number is greater than 5.
            if (number > 5)
            {
                //Let's increase the popup's scale.
                newPopup.SetScale(1.3f);

                //Let's add some text to the right of the number.
                newPopup.enableRightText = true;
                newPopup.rightText = "!";

                //Let's change the color of the popup.
                newPopup.SetColor(new Color(1, 0.2f, 0.2f));
            }
            else
            {
                //The following lines reset the changes above.
                //This would be neccesary for pooled popups.
                newPopup.SetScale(1);
                newPopup.enableRightText = false;
                newPopup.SetColor(new Color(1, 0.7f, 0.5f));
            }
        }
    }
}

