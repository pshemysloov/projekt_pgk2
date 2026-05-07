using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class UI : MonoBehaviour, PlayerControls.IUIInputActions
{
    public Image hpbar;
    public TextMeshProUGUI hptext;
    private PlayerControls playerControls;
    private Animator animator;
    private int maxhp, currenthp;
    

    public void Start()
    {
        animator = GetComponent<Animator>();
        UpdateHealthBar();
    }

    private void OnEnable()
    {
        playerControls = InputManager.Instance.Controls;
        if (playerControls != null)
        playerControls.UIInput.AddCallbacks(this);

    }

    private void OnDisable()
    {
        playerControls.UIInput.RemoveCallbacks(this);
    }

    public void UpdateHealthBar()
    {
        currenthp = GameManager.instance.player.CurrentHealth;
        maxhp = GameManager.instance.player.MaxHealth;

        hptext.text = $"{currenthp}/{maxhp}";

        float completionRatio = (float)currenthp / (float)maxhp;
        hpbar.transform.localScale = new Vector3(completionRatio, 1, 1);
    }

    public void EquipWeapon(WeaponSO weaponSO)
    {
        GameManager.instance.player.weapon.EquipWeapon(weaponSO);
    }

    public void EquipmentToggle()
    {
        animator.SetBool("InventoryOn", !animator.GetBool("InventoryOn"));
        bool isInventoryOn = animator.GetBool("InventoryOn");

        var controls = InputManager.Instance.Controls;


        if (isInventoryOn)
        {
            controls.PlayerInput.Disable();
            controls.UIInput.Enable();
            GameManager.instance.weapon.animator.enabled = false;
        }
        else
        {
            controls.UIInput.Disable();
            controls.PlayerInput.Enable();
            GameManager.instance.weapon.animator.enabled = true;
        }
    }

    public void OnInventoryOff(InputAction.CallbackContext context)
    {
        if (context.performed)
            EquipmentToggle();
    }
}
