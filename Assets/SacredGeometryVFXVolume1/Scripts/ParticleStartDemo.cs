using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleStartDemo : MonoBehaviour
{
    public GameObject PS_FlowerOfLife;
    public ParticleSystem PS_FlowerOfLife_Buff1;
    public ParticleSystem PS_FlowerOfLife_Buff2;
    public ParticleSystem PS_FlowerOfLife_Buff3;

    public GameObject PS_Dodecagram;
    public ParticleSystem PS_Dodecagram_Buff1;
    public ParticleSystem PS_Dodecagram_Buff2;
    public ParticleSystem PS_Dodecagram_Buff3;

    public GameObject PS_SeedOfLife;
    public ParticleSystem PS_SeedOfLife_Buff1;
    public ParticleSystem PS_SeedOfLife_Buff2;
    public ParticleSystem PS_SeefOfLife_Buff3;

    public GameObject PS_VesicaPiscisEye;
    public ParticleSystem PS_VesicaPiscisEye_Buff1;
    public ParticleSystem PS_VesicaPiscisEye_Buff2;
    public ParticleSystem PS_VesicaPiscisEye_Buff3;

    public GameObject PS_HexagonMetatronsAdaptation;
    public ParticleSystem PS_HexagonMetatronsAdaptation_Buff1;
    public ParticleSystem PS_HexagonMetatronsAdaptation_Buff2;
    public ParticleSystem PS_HexagonMetatronsAdaptation_Buff3;

    public GameObject PS_Icosahedron;
    public ParticleSystem PS_Icosahedron_Buff1;
    public ParticleSystem PS_Icosahedron_Buff2;
    public ParticleSystem PS_Icosahedron_Buff3;

    public GameObject PS_HexagonFormation;
    public ParticleSystem PS_HexagonFormation_Buff1;
    public ParticleSystem PS_HexagonFormation_Buff2;
    public ParticleSystem PS_HexagonFormation_Buff3;

    public GameObject PS_MetatronsCube;
    public ParticleSystem PS_MetatronsCube_Buff1;
    public ParticleSystem PS_MetatronsCube_Buff2;
    public ParticleSystem PS_MetatronsCube_Buff3;

    public GameObject PS_PiscisEyeTrinity;
    public ParticleSystem PS_PiscisEyeTrinity_Buff1;
    public ParticleSystem PS_PiscisEyeTrinity_Buff2;
    public ParticleSystem PS_PiscisEyeTrinity_Buff3;

    // Start is called before the first frame update
    void Start()
    {
        PS_FlowerOfLife.SetActive(false);
        PS_Dodecagram.SetActive(false);
        PS_SeedOfLife.SetActive(false);
        PS_VesicaPiscisEye.SetActive(false);
        PS_HexagonMetatronsAdaptation.SetActive(false);
        PS_Icosahedron.SetActive(false);
        PS_HexagonFormation.SetActive(false);
        PS_MetatronsCube.SetActive(false);
        PS_PiscisEyeTrinity.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ToggleFlowerOfLife()
    {
        PS_FlowerOfLife.SetActive(true);
        PS_FlowerOfLife_Buff1.Play();
        PS_FlowerOfLife_Buff2.Play();
        PS_FlowerOfLife_Buff3.Play();
    }

    public void ToggleDodecagram()
    {
        PS_Dodecagram.SetActive(true);
        PS_Dodecagram_Buff1.Play();
        PS_Dodecagram_Buff2.Play();
        PS_Dodecagram_Buff3.Play();
    }

    public void ToggleSeedOfLife()
    {
        PS_SeedOfLife.SetActive(true);
        PS_SeedOfLife_Buff1.Play();
        PS_SeedOfLife_Buff2.Play();
        PS_SeefOfLife_Buff3.Play();
    }

    public void ToggleVesicaPiscisEye()
    {
        PS_VesicaPiscisEye.SetActive(true);
        PS_VesicaPiscisEye_Buff1.Play();
        PS_VesicaPiscisEye_Buff2.Play();
        PS_VesicaPiscisEye_Buff3.Play();

    }

    public void ToggleHexagonMetatronsAdaptation()
    {
        PS_HexagonMetatronsAdaptation.SetActive(true);
        PS_HexagonMetatronsAdaptation_Buff1.Play();
        PS_HexagonMetatronsAdaptation_Buff2.Play();
        PS_HexagonMetatronsAdaptation_Buff3.Play();
    }

    public void ToggleIcosahedron()
    {
        PS_Icosahedron.SetActive(true);
        PS_Icosahedron_Buff1.Play();
        PS_Icosahedron_Buff2.Play();
        PS_Icosahedron_Buff3.Play();
    }

    public void ToggleHexagonFormation()
    {
        PS_HexagonFormation.SetActive(true);
        PS_HexagonFormation_Buff1.Play();
        PS_HexagonFormation_Buff2.Play();
        PS_HexagonFormation_Buff3.Play();
    }

    public void ToggleMetatronsCube()
    {
        PS_MetatronsCube.SetActive(true);
        PS_MetatronsCube_Buff1.Play();
        PS_MetatronsCube_Buff2.Play();
        PS_MetatronsCube_Buff3.Play();
    }

    public void TogglePiscisEyeTrinity()
    {
        PS_PiscisEyeTrinity.SetActive(true);
        PS_PiscisEyeTrinity_Buff1.Play();
        PS_PiscisEyeTrinity_Buff2.Play();
        PS_PiscisEyeTrinity_Buff3.Play();
    }

}
