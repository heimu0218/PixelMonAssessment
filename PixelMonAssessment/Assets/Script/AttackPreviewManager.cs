using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEngine.UI.Image;


public class AttackPreviewManager : MonoBehaviour
{
    
    public Button Btn_Attack;
    public Button Btn_ToggleEnv;

    public GameObject characterTransform;
    public GameObject projectilePoint;
    public GameObject target;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public SkillFXComponentSO SkillFXComponent;

    public enum Mode { Idle, SkillActivation, SkillBuildUp, SkillReady, LaunchAttack, ProjectileHit };


    public Mode CharacterMode;

    GameObject skillActivationGO;
    GameObject skillBuildupGO;
    GameObject attackProjectileGO;
    GameObject LaunchFXGO;
    Collider projectileCollider;

    public GameObject BasicFloor;
    public GameObject SceneElements;

    public TMP_Dropdown SkillColorRampList;

    float waitTime = 0.0f;
    void Start()
    {
        
        Btn_Attack.onClick.AddListener(BeginAttack);
        Btn_ToggleEnv.onClick.AddListener(ToggleEnv);
        CharacterMode = Mode.Idle;

        List<Sprite> ColorRamp = new List<Sprite>();
        foreach (Sprite colorRamp in SkillFXComponent.ColorRamp)
        {
            ColorRamp.Add(colorRamp);
        }
        SkillColorRampList.AddOptions(ColorRamp);

        SkillColorRampList.onValueChanged.AddListener(delegate { UpdateMaterialColorRamp(); });
        UpdateMaterialColorRamp();
    }

    // Update is called once per frame
    void Update()
    {
        if (waitTime <= 0.0f)
        {
            switch (CharacterMode)
            {
                case Mode.SkillActivation:
                    Debug.Log("Activate");
                    SkillBuildUp();
                    break;
                case Mode.SkillBuildUp:
                    Debug.Log("BuildUp");
                    SkillReady();
                    break;
                case Mode.SkillReady:
                    Debug.Log("Projectile");
                    LauchAttack();
                    break;

                case Mode.LaunchAttack:

                    break;

                case Mode.ProjectileHit:
                    CharacterMode = Mode.Idle;
                    break;

                default: CharacterMode=Mode.Idle; 
                            break;
            }
        }

        if (waitTime > 0)
        {
            waitTime-= Time.deltaTime;
        }

        if (CharacterMode == Mode.LaunchAttack)
        {
            projectileCollider.enabled = true;
            var step = SkillFXComponent.ProjectileSpeed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
            attackProjectileGO.transform.position = Vector3.MoveTowards(attackProjectileGO.transform.position, target.transform.position, step);
        }

        if (attackProjectileGO != null && attackProjectileGO.transform.position== target.transform.position)
        {
            CharacterMode = Mode.Idle;
        }
    }

    void BeginAttack()
    {
        target.GetComponent<FXFeedBack>().FeedBackPrefab = SkillFXComponent.HitFeedBackFX;
        target.GetComponent<FXFeedBack>().attackPreviewManager = this;
        ClearExistingFX();

        CharacterMode = Mode.SkillActivation;
        waitTime = SkillFXComponent.SkillActivationDuration;
        skillActivationGO = GameObject.Instantiate(SkillFXComponent.SkillActivationFX, characterTransform.transform);
        skillActivationGO.SetActive(false);
        skillActivationGO.SetActive(true);
        skillActivationGO.GetComponent<ParticleSystem>().Play();
        //AttackHandler.SetActive(true);
    }

    void SkillBuildUp()
    {
        CharacterMode = Mode.SkillBuildUp;
        waitTime = SkillFXComponent.SkillBuildUpDuration;
        skillBuildupGO = GameObject.Instantiate(SkillFXComponent.SkillBuildUpFX, characterTransform.transform);
        skillBuildupGO.SetActive(false);
        skillBuildupGO.SetActive(true);
        skillBuildupGO.GetComponent<ParticleSystem>().Play();
    }

    void SkillReady()
    {
        CharacterMode = Mode.SkillReady;
        waitTime = SkillFXComponent.ProjectileDuration;
        attackProjectileGO = GameObject.Instantiate(SkillFXComponent.Projectile, projectilePoint.transform.position, projectilePoint.transform.rotation);
        attackProjectileGO.SetActive(false);
        attackProjectileGO.SetActive(true);
        projectileCollider = attackProjectileGO.GetComponent<SphereCollider>();
        attackProjectileGO.GetComponent<ParticleSystem>().Play();
    }

    void LauchAttack()
    {
        CharacterMode = Mode.LaunchAttack;
        LaunchFXGO = GameObject.Instantiate(SkillFXComponent.LauchFX, projectilePoint.transform.position, projectilePoint.transform.rotation);
        LaunchFXGO.SetActive(false);
        LaunchFXGO.SetActive(true);
        LaunchFXGO.GetComponent<ParticleSystem>().Play();
        

    }

    void ProjectileHitFeedBack()
    {
        CharacterMode = Mode.ProjectileHit;
    }

    void ClearExistingFX()
    {
        if (attackProjectileGO!=null)
        {
            GameObject.Destroy(attackProjectileGO);
        }
        if (skillBuildupGO != null)
        {
            GameObject.Destroy(skillBuildupGO);
        }
        if (skillActivationGO != null)
        {
            GameObject.Destroy(skillActivationGO);
        }
    }

    void ToggleEnv()
    {
        SceneElements.SetActive(!SceneElements.activeSelf);
        BasicFloor.SetActive(!BasicFloor.activeSelf);
   
    }

    void UpdateMaterialColorRamp()
    {
        foreach (Material material in SkillFXComponent.Materials)
        {
            material.SetTexture("_ColorRamp", SkillFXComponent.ColorRamp[SkillColorRampList.value].texture);
        }
    }
}
