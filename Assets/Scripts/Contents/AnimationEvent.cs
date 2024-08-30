using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    private bool palseSound;
    private void Dead()
    {
        Destroy(this.gameObject);
    }
    private void DeadParent()
    {
        if (this.transform.parent != null)
            Destroy(this.transform.parent.gameObject);
        else
            Destroy(this.gameObject);
    }
    private void DonActive()
    {
        this.gameObject.SetActive(false);
    }

    private void PalseSound()
    {
        if (palseSound == true)
            palseSound = false;
    }
    private void FireBallStart()
    {
        if(palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(0, 1f);
            palseSound = true;
        }
    }
    private void FireBallStay()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(1, 1f);
            palseSound = true;
        }
    }
    private void FireBallExit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(2, 1f);
            palseSound = true;
        }
    }
    private void IceBallStart()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(3, 1f);
            palseSound = true;
        }
    }
    private void IceBallStay()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(4, 1f);
            palseSound = true;
        }
    }
    private void IceBallExit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(5, 1f);
            palseSound = true;
        }
    }
    private void IceSpearStart()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(6, 1f);
            palseSound = true;
        }
    }
    private void IceSpearStay()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false) 
        {
            SoundManager.Instance.PlayEffect(7, 1f);
            palseSound = true;
        }
    }
    private void IceSpearExit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(8, 1f);
            palseSound = true;
        }
    }
    private void iceTime()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(9, 1f);
            palseSound = true;
        }
    }
    private void iceBreak()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(10, 1f);
            palseSound = true;
        }
    }
    private void IceWall()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(11, 1f);
            palseSound = true;
        }
    }
    private void IceTrapStart()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(12, 1f);
            palseSound = true;
        }
    }
    private void IceTrapExit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(13, 1f);
            palseSound = true;
        }
    }
    private void IceBite()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(14, 1f);
            palseSound = true;
        }
    }
    private void FireBombStart()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(16, 1f);
            palseSound = true;
        }
    }
    private void FireBombStay()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(17, 1f);
            palseSound = true;
        }
    }
    private void FireBombExit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(18, 1f);
            palseSound = true;
        }
    }
    private void FireBladeStart()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(19, 1f);
            palseSound = true;
        }
    }
    private void FireBladeExit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(20, 1f);
            palseSound = true;
        }
    }
    private void MeteorStart()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(21, 0.5f);
            palseSound = true;
        }
    }
    private void MeteorExit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(2, 0.5f);
            palseSound = true;
        }
    }
    private void FireBracesStay()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(22, 0.25f);
            palseSound = true;
        }
    }
    private void FireViper()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(23, 1f);
            palseSound = true;
        }
    }
    private void FireBuff()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(17, 1f);
            SoundManager.Instance.PlayEffect(22, 1f);
            palseSound = true;
        }
    }
    private void DoubleBerrelExit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(24, 1f);
            palseSound = true;
        }
    }
    private void SpecialBackground()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(25, 0.75f);
            palseSound = true;
        }
    }
    private void SpecialBackgroundExit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.StopEffect(25);
            palseSound = true;
        }
    }
    private void HellFireCharge()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(26, 0.75f);
            SoundManager.Instance.PlayEffect(27, 1f);
            palseSound = true;
        }
    }
    private void HellFireChargeExit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(28, 1f);

            var cam = UnityEngine.Camera.main;
            StartCoroutine(SkillManager.Instance.ZoomInOut(1.2f, cam.orthographicSize / 0.75f, true));
           // StartCoroutine(SkillManager.Instance.SetCamPosition(0.25f, Vector2.zero));
            palseSound = true;
        }
    }
    private void Burn()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(29, 1f);
            palseSound = true;
        }
    }
    private void EarthThrowingStart()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(31, 1f);
            palseSound = true;
        }
    }
    private void EarthThrowingStay()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(32, 1f);
            palseSound = true;
        }
    }
    private void EarthThrowingExit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(33, 1f);
            palseSound = true;
        }
    }
    private void StrongWall()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(34, 1f);
            palseSound = true;
        }
    }
    private void SandHide()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(35, 0.5f);
            palseSound = true;
        }
    }
    private void MudCastStart()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(36, 1f);
            palseSound = true;
        }
    }
    private void MudCastStay()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(37, 1f);
            palseSound = true;
        }
    }
    private void MudCastExit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(38, 1f);
            palseSound = true;
        }
    }
    private void EarthSpikeHit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(39, 1f);
            palseSound = true;
        }
    }
    private void RockBlasterStart()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(40, 1f);
            palseSound = true;
        }
    }
    private void RockBlasterExit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(41, 1f);
            palseSound = true;
        }
    }
    private void RockBlasterLayer()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(42, 1f);
            palseSound = true;
        }
    }
    private void Shock()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(43, 1f);
            palseSound = true;
        }
    }
    private void ShockHit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(44, 1f);
            palseSound = true;
        }
    }
    private void ElecShockStart()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(45, 1f);
            palseSound = true;
        }
    }
    private void ElecShockStay()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(46, 1f);
            palseSound = true;
        }
    }
    private void ElecShockExit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(47, 1f);
            palseSound = true;
        }
    }
    private void ThunderBolt()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(48, 1f);
            palseSound = true;
        }
    }
    private void ElecAura()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(46, 0.1f);
            palseSound = true;
        }
    }
    private void ElectronicSuction()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(49, 1f);
            palseSound = true;
        }
    }
    private void LightingVotexHit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(50, 1f);
            palseSound = true;
        }
    }
    private void ThunderBird()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(51, 1f);
            palseSound = true;
        }
    }
    private void ThunderBirdExit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(52, 1f);
            palseSound = true;
        }
    }
    private void ElecNetStay()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(53, 1f);
            palseSound = true;
        }
    }
    private void ElecNetExit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(54, 1f);
            palseSound = true;
        }
    }
    private void ElecPunchStart()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(55, 1f);
            palseSound = true;
        }
    }
    private void DiskStart()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(56, 1f);
            palseSound = true;
        }
    }
    private void DiskStay()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(57, 1f);
            palseSound = true;
        }
    }
    private void AirBallStart()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(58, 1f);
            palseSound = true;
        }
    }
    private void AirBallStay()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(59, 1f);
            palseSound = true;
        }
    }
    private void AirBallExit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(60, 1f);
            palseSound = true;
        }
    }
    private void AirHit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(61, 1f);
            palseSound = true;
        }
    }
    private void SharpWinds()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(63, 1f);
            palseSound = true;
        }
    }
    private void SonicBoom()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(64, 1f);
            palseSound = true;
        }
    }
    private void WindClone()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(65, 1f);
            palseSound = true;
        }
    }
    private void TornadoStart()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(66, 1f);
            palseSound = true;
        }
    }
    private void TornadoStay()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(64, 0.25f);
            palseSound = true;
        }
    }
    private void TornadoBarriorStay()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(62, 0.25f, true);
            palseSound = true;
        }
    }
    private void TornadoBarriorExit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.StopEffect(62);
            palseSound = true;
        }
    }
    private void WindBlasterStart()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(67, 0.5f, true);
            palseSound = true;
        }
    }
    private void WindBlasterExit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.StopEffect(67);
            palseSound = true;
        }
    }
    private void PoisonNeedle()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(68, 1f);
            palseSound = true;
        }
    }
    private void PoisonHit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(69, 1f);
            palseSound = true;
        }
    }

    private void TailAttack()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false) 
        {
            SoundManager.Instance.PlayEffect(70, 1f);
            palseSound = true;
        }
    }
    private void AcidBallStart()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(71, 1f);
            palseSound = true;
        }
    }
    private void AcidBallStay()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(72, 1f);
            palseSound = true;
        }
    }
    private void PoisonFogHit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(74, 1f);
            palseSound = true;
        }
    }
    private void PoisonPoolStart()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(75, 0.2f);
            palseSound = true;
        }
    }
    private void AcidRumbleStart()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(75, 0.75f);
            palseSound = true;
        }
    }
    private void PoisonPoolExit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.StopEffect(75);
            palseSound = true;
        }
    }
    private void DeadlyPoison()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(76, 1f);
            palseSound = true;
        }
    }
    private void DeadlyPoisonLayer()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(77, 1f);
            palseSound = true;
        }
    }
    private void DeadlyPoisonFog()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(78, 1f);
            palseSound = true;
        }
    }
    private void NormalHit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(79, 1f);
            palseSound = true;
        }
    }
    private void Pogrum()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(80, 1f);
            palseSound = true;
        }
    }
    private void Rush()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(81, 1f);
            palseSound = true;
        }
    }
    private void EnergyStay()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(83, 1f);
            palseSound = true;
        }
    }
    private void EnergyExit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(84, 0.75f);
            palseSound = true;
        }
    }
    private void HitPlant()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(86, 1f);
            palseSound = true;
        }
    }
    private void HitPlantLayer()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(87, 1f);
            palseSound = true;
        }
    }
    private void HitStamp()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(88, 0.5f);
            palseSound = true;
        }
    }
    private void GrassKnot()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(89, 0.75f);
            palseSound = true;
        }
    }
    private void LeafBladeStart()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(91, 1f);
            palseSound = true;
        }
    }
    private void LeafBladeExit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(92, 1f);
            palseSound = true;
        }
    }
    private void WoodBulletStart()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(93, 1f);
            palseSound = true;
        }
    }
    private void WoodBulletStay()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(94, 1f);
            palseSound = true;
        }
    }
    private void WoodBuff()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(95, 1f);
            palseSound = true;
        }
    }
    private void HolyArrowStart()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(96, 1f);
            palseSound = true;
        }
    }
    private void HolyArrowStay()
    {
        if (palseSound == false && SkillManager.Instance.arrowSoundBlock == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(97, 0.5f);
            palseSound = true;
        }
    }
    private void HolyArrowExit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(98, 1f);
            palseSound = true;
        }
    }
    private void Recovery()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(99, 0.75f);
            palseSound = true;
        }
    }
    private void Holy()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(100, 1f);
            palseSound = true;
        }
    }
    private void HolySword()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(101, 1f);
            palseSound = true;
        }
    }
    private void HolyDefense()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(102, 1f);
            palseSound = true;
        }
    }
    private void HolyLightStart()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(103, 1f);
            palseSound = true;
        }
    }
    private void HolyLightExit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(104, 1f);
            palseSound = true;
        }
    }
    private void LazerStay()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(106, 1f);
            palseSound = true;
        }
    }
    private void Knell()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(107, 0.25f);
            palseSound = true;
        }
    }
    private void Easter()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(108, 1f);
            palseSound = true;
        }
    }
    private void DivinPowerStart()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(109, 1f);
            palseSound = true;
        }
    }
    private void Collaboration()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(112, 0.25f, true);
            palseSound = true;
        }
    }
    private void CollaborationExit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.StopEffect(112);
            palseSound = true;
        }
    }
    private void CollaborationHit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(113, 1f);
            palseSound = true;
        }
    }
    private void Star()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(114, 1f);
            palseSound = true;
        }
    }
    private void PowerBalance()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(115, 1f);
            palseSound = true;
        }
    }
    private void LentPower()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(116, 1f);
            palseSound = true;
        }
    }
    private void ShineSkipping()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(119, 1f);
            palseSound = true;
        }
    }
    private void ShadowBallStart()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(120, 1f);
            palseSound = true;
        }
    }
    private void ShadowBallStay()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(121, 0.5f);
            palseSound = true;
        }
    }
    private void DarkHit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(122, 1f);
            palseSound = true;
        }
    }
    private void DemonsEyes()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(123, 1f);
            palseSound = true;
        }
    }

    private void DevilLaugh()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(Random.Range(124, 127), 1f);
            palseSound = true;
        }
    }
    private void MindControl()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(127, 1f);
            palseSound = true;
        }
    }
    private void SoulSlash()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(128, 1f);
            palseSound = true;
        }
    }
    private void Unable()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(129, 1f);
            palseSound = true;
        }
    }
    private void SoulAttackStay()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(130, 1f);
            palseSound = true;
        }
    }
    private void SoulAttackExit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(131, 1f);
            palseSound = true;
        }
    }
    private void TrickRoom()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffectUnTimeScale(133, 1f);
            palseSound = true;
        }
    }
    private void TrickRoomStop()
    {
        if (CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.StopEffect(133);
        }
    }
    private void TrickRoomHit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffectUnTimeScale(48, 1f);
            palseSound = true;
        }
    }
    private void SummmonsMagic()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(135, 1f);
            palseSound = true;
        }
    }
    private void Pandora()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(136, 1f);
            palseSound = true;
        }
    }
    private void TimeBuff()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(137, 1f);
            palseSound = true;
        }
    }
    private void TickTock()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(138, 1f);
            palseSound = true;
        }
    }
    private void DarkLord()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(139, 1f);
            palseSound = true;
        }
    }
    private void DarkHole()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(140, 0.1f, true);
            palseSound = true;
        }
    }
    private void DarkHoleExit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.StopEffect(140);
            palseSound = true;
        }
    }
    private void MythOfSpearStart()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(142, 1f);
            palseSound = true;
        }
    }
    private void MythOfSpearExit()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(143, 1f);
            palseSound = true;
        }
    }
    private void BehimosStart()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(193, 1f);
            SoundManager.Instance.PlayEffect(194, 1f);
            palseSound = true;
        }
    }
    private void SkillLock()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(144, 1f);
            palseSound = true;
        }
    }
    private void ShieldOn()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(145, 1f);
            palseSound = true;
        }
    }
    private void Buff()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(151, 1f);
            palseSound = true;
        }
    }
    private void DeBuff()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(152, 1f);
            palseSound = true;
        }
    }
    private void NoMana()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(153, 1f);
            palseSound = true;
        }
    }
    private void LivingDead()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(154, 1f);
            palseSound = true;
        }
    }
    private void WillPower()
    {
        if (palseSound == false && CombatManager.Instance.isEnd == false)
        {
            SoundManager.Instance.PlayEffect(156, 1f);
            palseSound = true;
        }
    }
    private void ItemBoxStart()
    {
        if (palseSound == false)
        {
            SoundManager.Instance.PlayEffect(180, 1f);
            palseSound = true;
        }
    }
    private void ItemBoxStay()
    {
        if (palseSound == false)
        {
            SoundManager.Instance.PlayEffect(181, 1f);
            palseSound = true;
        }
    }
    private void RewardCharge()
    {
        if (palseSound == false)
        {
            SoundManager.Instance.StopEffect(182);
            SoundManager.Instance.PlayEffect(182, 1f);
            palseSound = true;
        }
    }
    private void FrogJump()
    {
        if (palseSound == false)
        {
            SoundManager.Instance.PlayEffect(192, 1f);
            palseSound = true;
        }
    }
    private void Camera()
    {
        if (palseSound == false)
        {
            SoundManager.Instance.PlayEffect(183, 1f);
            SoundManager.Instance.PlayEffect(184, 1f);
            RewardUI.Instance.skipButton.gameObject.SetActive(false);
            palseSound = true;
        }
    }
    private void Success()
    {
        if (palseSound == false)
        {
            SoundManager.Instance.PlayEffect(186, 1f);
            palseSound = true;
        }
    }
    private void Calander()
    {
        if (palseSound == false && CalendarUI.Instance.blackSound == false)
        {
            SoundManager.Instance.PlayEffect(187, 1f);
            palseSound = true;
        }
    }
    private void VictoryEffect_0()
    {
        if (palseSound == false)
        {
            SoundManager.Instance.PlayEffect(188, 1f);
            palseSound = true;
        }
    }
    private void Swap()
    {
        if (palseSound == false)
        {
            SoundManager.Instance.PlayEffect(173, 1f);
            palseSound = true;
        }
    }
    private void ClosedMonsterButton()
    {
        var buttons = DialougeUI.Instance.monsterBoxButtons;

        buttons[0].interactable = false;
        var text = buttons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0.5019607843137255f);

        buttons[1].interactable = false;
        text = buttons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0.5019607843137255f);

        buttons[2].interactable = false;
        text = buttons[2].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0.5019607843137255f);

        buttons[3].interactable = true;
        text = buttons[3].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);

    }
    private void OpenMonsterButton()
    {
        var buttons = DialougeUI.Instance.monsterBoxButtons;

        buttons[0].interactable = true;
        var text = buttons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);

        buttons[1].interactable = true;
        text = buttons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);

        buttons[2].interactable = true;
        text = buttons[2].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);

        buttons[3].interactable = true;
        text = buttons[3].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
    }
    private void ClosedItemButton()
    {
        var buttons = DialougeUI.Instance.itemBoxButtons;

        buttons[0].interactable = false;
        var text = buttons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0.5019607843137255f);

        buttons[1].interactable = false;
        text = buttons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0.5019607843137255f);

        buttons[2].interactable = true;
        text = buttons[2].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);

    }
    private void OpenItemButton()
    {
        var buttons = DialougeUI.Instance.itemBoxButtons;

        buttons[0].interactable = true;
        var text = buttons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);

        buttons[1].interactable = true;
        text = buttons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);

        buttons[2].interactable = true;
        text = buttons[2].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);

    }
}
