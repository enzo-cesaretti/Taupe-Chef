using UnityEngine;

public class Mole : MonoBehaviour
{
    private Vector3 _originalPos = Vector3.zero;

    public bool IsUp { get; private set; } = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _originalPos = transform.position;
        MoleUp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoleUp()
    {
        transform.position = _originalPos + new Vector3(0f, 0.5f, 0f);
        IsUp = true;
    }

    public void MoleDown()
    {
        transform.position = _originalPos;
        IsUp = false;
    }
}
