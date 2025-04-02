using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public int selectedWeapon = 0;
    private GrenadeThrower grenadeThrower;

    private void Start()
    {
        grenadeThrower = GetComponentInChildren<GrenadeThrower>();
        SelectWeapon();
    }

    private void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= transform.childCount - 1) selectedWeapon = 0;
            else selectedWeapon++;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0) selectedWeapon = transform.childCount - 1;
            else selectedWeapon--;
        }

        if (previousSelectedWeapon != selectedWeapon) SelectWeapon();
    }

    private void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (weapon.name == "ThrowPoint") continue;

            bool isSelected = (i == selectedWeapon);
            weapon.gameObject.SetActive(isSelected);

            if (weapon.name == "Grenade" && grenadeThrower != null)
            {
                grenadeThrower.SetGrenadeActive(isSelected);
                if (!isSelected)
                {
                    grenadeThrower.ResetGrenade();
                }
                else
                {
                    grenadeThrower.SetGrenadeActive(true); // **Bomba seçildiğinde tekrar aç**
                }
            }
            i++;
        }
    }

}
