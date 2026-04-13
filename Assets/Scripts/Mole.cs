using UnityEngine;

public class Mole : MonoBehaviour
{
    public Animator _animator;

    public enum MoleState
    {
        Down,
        GoingUp,
        Up,
        GoingDown
    }
    public int Hp { get; private set; } = 0;
    public int BaseHp { get; private set; } = 3;
    public MoleManager manager;

    public MoleState State { get; private set; } = MoleState.Down;

    public void RegisterHit()
    {
        Hp--;
        if (Hp <= 0)
        {
            SquashMole();
        }
        else
        {
            _animator.Play("hit");
        }
    }

    public void MoleUp()
    {
        Hp = BaseHp;
        State = MoleState.GoingUp;
        _animator.Play("popUp");
    }

    public void OnPopUpFinished()
    {
        State = MoleState.Up;
    }


    private void SquashMole()
    {
        manager?.CancelMoleTimer(this);

        State = MoleState.GoingDown;
        _animator.Play("squash");
    }

    public void OnSquashFinished()
    {
        State = MoleState.Down;
    }

    public void MoleDown()
    {
        State = MoleState.GoingDown;
        _animator.Play("goDown");
    }

    public void OnGoDownFinished()
    {
        State = MoleState.Down;
    }
}
