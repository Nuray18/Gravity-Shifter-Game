using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public int selectedWeapon = 0;

    private void Start()
    {
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
        int weaponIndex = 0;
        foreach (Transform weapon in transform)
        {
            // Eğer child objenin adı "ThrowPoint" ise onu yok say
            if (weapon.name == "ThrowPoint") continue;

            if (weaponIndex == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            weaponIndex++;
        }
    }
}
