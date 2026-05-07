using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // References
    public Player player;
    public WeaponContainer weaponContainer;
    public Weapon weapon;
    public UI ui;
    public NewLevelLoader newLevelLoader;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;

        player = GetComponentInChildren<Player>();
        weaponContainer = GetComponentInChildren<WeaponContainer>();
        weapon = GetComponentInChildren<Weapon>();
        ui = GetComponentInChildren<UI>();
        newLevelLoader = GetComponentInChildren<NewLevelLoader>();
    }
}
