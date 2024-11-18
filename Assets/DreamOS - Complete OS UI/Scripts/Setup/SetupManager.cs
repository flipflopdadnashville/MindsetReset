using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Michsky.DreamOS
{
    public class SetupManager : MonoBehaviour
    {
        // List
        public List<StepItem> steps = new List<StepItem>();

        // Settings
        public int currentPanelIndex = 0;
        public bool enableBackgroundAnim = true;

        private GameObject currentStep;
        private GameObject currentPanel;
        private GameObject nextPanel;
        private GameObject currentBG;
        private GameObject nextBG;

        [HideInInspector] public Animator currentStepAnimator;
        [HideInInspector] public Animator currentPanelAnimator;
        [HideInInspector] public Animator currentBGAnimator;
        [HideInInspector] public Animator nextPanelAnimator;
        [HideInInspector] public Animator nextBGAnimator;

        string panelFadeIn = "Panel In";
        string panelFadeOut = "Panel Out";
        string BGFadeIn = "Panel In";
        string BGFadeOut = "Panel Out";
        string stepFadeIn = "Check";

        [System.Serializable]
        public class StepItem
        {
            public string title = "Step";
            public GameObject indicator;
            public GameObject panel;
            public GameObject background;
        }

        void Start()
        {
            currentPanel = steps[currentPanelIndex].panel;
            currentPanelAnimator = currentPanel.GetComponent<Animator>();

            if (currentPanelAnimator.transform.parent.gameObject.activeSelf == true)
            {
                currentPanelAnimator.Play(panelFadeIn);

                if (enableBackgroundAnim == true)
                {
                    currentBG = steps[currentPanelIndex].background;
                    currentBGAnimator = currentBG.GetComponent<Animator>();
                    currentBGAnimator.Play(BGFadeIn);
                }

                else
                {
                    currentBG = steps[currentPanelIndex].background;
                    currentBGAnimator = currentBG.GetComponent<Animator>();
                    currentBGAnimator.Play(BGFadeIn);
                }
            }
        }

        public void PanelAnim(int newPanel)
        {
            if (newPanel != currentPanelIndex)
            {
                currentPanel = steps[currentPanelIndex].panel;
                currentPanelIndex = newPanel;
                nextPanel = steps[currentPanelIndex].panel;

                currentPanelAnimator = currentPanel.GetComponent<Animator>();
                nextPanelAnimator = nextPanel.GetComponent<Animator>();

                currentPanelAnimator.Play(panelFadeOut);
                nextPanelAnimator.Play(panelFadeIn);

                if (enableBackgroundAnim == true)
                {
                    currentBG = steps[currentPanelIndex].background;
                    currentPanelIndex = newPanel;
                    nextBG = steps[currentPanelIndex].background;

                    currentBGAnimator = currentBG.GetComponent<Animator>();
                    nextBGAnimator = nextBG.GetComponent<Animator>();

                    currentBGAnimator.Play(BGFadeOut);
                    nextBGAnimator.Play(BGFadeIn);
                }
            }
        }

        public void NextPage()
        {
            if (currentPanelIndex <= steps.Count - 2)
            {
                currentPanel = steps[currentPanelIndex].panel;
                currentPanelAnimator = currentPanel.GetComponent<Animator>();
                currentPanelAnimator.Play(panelFadeOut);

                currentStep = steps[currentPanelIndex].indicator;
                currentStepAnimator = currentStep.GetComponent<Animator>();
                currentStepAnimator.Play(stepFadeIn);

                if (enableBackgroundAnim == true)
                {
                    currentBG = steps[currentPanelIndex].background;
                    currentBGAnimator = currentBG.GetComponent<Animator>();
                    currentBGAnimator.Play(BGFadeOut);
                }

                currentPanelIndex += 1;
                nextPanel = steps[currentPanelIndex].panel;

                nextPanelAnimator = nextPanel.GetComponent<Animator>();
                nextPanelAnimator.Play(panelFadeIn);

                if (enableBackgroundAnim == true)
                {
                    nextBG = steps[currentPanelIndex].background;
                    nextBGAnimator = nextBG.GetComponent<Animator>();
                    nextBGAnimator.Play(BGFadeIn);
                }
            }
        }

        public void PrevPage()
        {
            if (currentPanelIndex >= 1)
            {
                currentPanel = steps[currentPanelIndex].panel;
                currentPanelAnimator = currentPanel.GetComponent<Animator>();
                currentPanelAnimator.Play(panelFadeOut);

                if (enableBackgroundAnim == true)
                {
                    currentBG = steps[currentPanelIndex].background;
                    currentBGAnimator = currentBG.GetComponent<Animator>();
                    currentBGAnimator.Play(BGFadeOut);
                }

                currentPanelIndex -= 1;
                nextPanel = steps[currentPanelIndex].panel;

                nextPanelAnimator = nextPanel.GetComponent<Animator>();
                nextPanelAnimator.Play(panelFadeIn);

                if (enableBackgroundAnim == true)
                {
                    nextBG = steps[currentPanelIndex].background;
                    nextBGAnimator = nextBG.GetComponent<Animator>();
                    nextBGAnimator.Play(BGFadeIn);
                }
            }
        }

        public void PlayLastStepAnim()
        {
            currentStep = steps[steps.Count].indicator;
            currentStepAnimator = currentStep.GetComponent<Animator>();
            currentStepAnimator.Play(stepFadeIn);
        }
    }
}