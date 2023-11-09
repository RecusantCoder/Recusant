using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameShield : Weapon
{
    private float lastShotTime;
    
    protected new int localWeaponlevel;
    protected new int numOfShots = 1;
    protected new float shotFrequency = 1.0f;
    private int numberOfFlameShieldAllowed = 1;
    private int numberOfFlameShieldSpawned = 0;
    
    public int damage = 100;
    public int speed = 0;

    public override void Shoot(Transform firePoint, int weaponLevel)
    {
        WeaponLevels(weaponLevel);

        if (Time.time - lastShotTime > shotFrequency && numberOfFlameShieldSpawned < numberOfFlameShieldAllowed)
        {
            GameObject flameShieldAttack = Instantiate(Resources.Load<GameObject>("PreFabs/Projectiles/FlameShieldAttack"));
            FlameShieldAttack flameShieldScript = flameShieldAttack.GetComponent<FlameShieldAttack>();
            flameShieldScript._damage += damage;
            flameShieldScript.flameShieldSpeed += speed;
            flameShieldAttack.transform.position = firePoint.position;
            lastShotTime = Time.time;
            numberOfFlameShieldSpawned++;
        }
    }

    protected override void WeaponLevels(int weaponLevel)
    {
        if (localWeaponlevel != weaponLevel && weaponLevel <= 10)
        {
            int oldLevel = localWeaponlevel;
            localWeaponlevel = weaponLevel;

            for (int level = oldLevel + 1; level <= localWeaponlevel; level++)
            {
                UpdateWeaponBehavior(level);
            }
        }
    }

    void UpdateWeaponBehavior(int weaponLevel)
    {
        switch (weaponLevel)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                break;
            case 9:
                break;
            case 10:
                break;
            default:
                print ("Default FlameShield.");
                break;
        }
    }
}
