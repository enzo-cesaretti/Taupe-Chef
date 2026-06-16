using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class AppraisialAnvil : MonoBehaviour
{

    [SerializeField] private XRSocketInteractor socketInteractor;
    [SerializeField] private TextMeshProUGUI weaponText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    private GameObject GetAttachedObject()
    {
        Debug.Log($"Has selection: {socketInteractor.hasSelection}");
        if (socketInteractor.hasSelection)
        {
            IXRSelectInteractable interactable = socketInteractor.firstInteractableSelected;

            Debug.Log($"Attached object: {interactable.transform.gameObject.name}");

            return interactable.transform.gameObject;
        }

        return null;
    }

    public void DisplayWeaponStats()
    {
        if (weaponText == null)
        {
            return;
        }

        GameObject attachedObj = GetAttachedObject();

        if (attachedObj != null)
        {
            if (attachedObj.TryGetComponent<Hammer>(out var weapon))
            {
                weaponText.text = $"Damage: {weapon.dmg}";
            }
        }
        else
        {
            weaponText.text = "Place a weapon on the anvil";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
