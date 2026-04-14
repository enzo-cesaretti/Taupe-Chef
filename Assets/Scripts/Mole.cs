using UnityEngine;
using System.Collections;

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
        StartCoroutine(WaitForAnimation("popUp"));
    }


    private void SquashMole()
    {
        manager?.CancelMoleTimer(this);

        State = MoleState.GoingDown;
        _animator.Play("squash");
        StartCoroutine(WaitForAnimation("squash"));
    }


    public void MoleDown()
    {
        State = MoleState.GoingDown;
        _animator.Play("goDown");
        StartCoroutine(WaitForAnimation("goDown"));
    }


    private IEnumerator WaitForAnimation(string animName)
    {
        float length = _animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(length);

        if (animName == "popUp")
        {
            State = MoleState.Up;
        } else if (animName == "squash" || animName == "goDown")
        {
            State = MoleState.Down;
        }
    }
}
