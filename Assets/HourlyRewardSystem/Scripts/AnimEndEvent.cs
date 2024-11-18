namespace GameBench
{
    using UnityEngine;

    public class AnimEndEvent : MonoBehaviour
    {

        public void EndAnim()
        {
            HourlyReward.Instance.RewardUser();
            HourlyReward.Instance.coinAnim.SetActive(true);
        }
        public void RewardUser()
        {
            //HourlyReward.Instance.RewardUser();
            HourlyReward.Instance.coinAnim.SetActive(false);
        }
    }
}