using UnityEngine;

[CreateAssetMenu(fileName = "SkillFXComponentSO", menuName = "Scriptable Objects/SkillFXComponentSO")]
public class SkillFXComponentSO : ScriptableObject
{
    public GameObject SkillActivationFX;
    public float SkillActivationDuration;
    public GameObject SkillBuildUpFX;
    public float SkillBuildUpDuration;
    public GameObject Projectile;
    public float ProjectileDuration;
    public float ProjectileSpeed;
    public GameObject LauchFX;
    public GameObject HitFeedBackFX;

    public Sprite[] ColorRamp;
    public Material[] Materials;
}
