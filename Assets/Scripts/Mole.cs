using UnityEngine;
using System.Collections;


public enum MoleType
{
    Normal,
    Golden
};

public enum MoleState
{
    Down,
    GoingUp,
    Up,
    GoingDown
};


public class Mole : MonoBehaviour
{
    public Animator _animator;



    public int Hp { get; private set; } = 0;
    public int BaseHp { get; private set; } = 2;
    public MoleManager manager;
    public MoleState State { get; private set; } = MoleState.Down;
    public MoleType type = MoleType.Normal;


    private const int NORMALSCORE = 10;
    private const int GOLDENSCORE = 100;


    public void RegisterHit(int dmg)
    {
        Hp -= dmg;
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


    private void AddScore()
    {
        int score = type switch
        {
            MoleType.Normal => NORMALSCORE,
            MoleType.Golden => GOLDENSCORE,
            _ => 0
        };

        ScoreManager.Instance.AddScore(score);
    }

    private void SquashMole()
    {
        AddScore();

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
