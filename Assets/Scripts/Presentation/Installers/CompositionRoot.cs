using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class CompositionRoot : MonoBehaviour
{
    public static CompositionRoot Instance { get; private set; }

    public IEventBus Events { get; private set; }
    public ITime Time { get; private set; }
    public IDamageService Damage { get; private set; }
    public IWeaponFactory WeaponFactory { get; private set; }
    public ITargetingService Targeting { get; private set; }


    [Header("Targeting Settings")]
    [SerializeField] private LayerMask enemyLayer = 1 << 9;
    [SerializeField] private bool useLineOfSight = false;
    
    void Awake() 
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Events = new EventBus();
        Time   = new UnityTimeService();
        Damage = new DamageService(Events);
        Targeting = new UnityPhysicsTargeting(enemyLayer, useLineOfSight);
        WeaponFactory = new WeaponFactory(Time, Damage, Targeting, Events);
    }
}
