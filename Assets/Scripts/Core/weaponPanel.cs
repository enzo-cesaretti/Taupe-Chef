using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class weaponPanel : MonoBehaviour
{
    public Hammer weapon;


    public void TryBuy()
    {
        if (weapon.Buy())
        {
            gameObject.SetActive(false);
        }
    }
}
