using UnityEngine;
using System.Collections;
using UnityEditor.UI;


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
    private static readonly int HitHash = Animator.StringToHash("hit");
    public Animator _animator;
    public int Hp = 0;
    public MoleManager manager;
    public MoleState State { get; private set; } = MoleState.Down;
    public MoleType type = MoleType.Normal;

    [SerializeField] private Material goldenMaterial;
    [SerializeField] private Material normalMaterial;

    private const int NORMALSCORE = 10;
    private const int GOLDENSCORE = 100;

    [SerializeField] private MeshRenderer moleRenderer;

    public void RegisterHit(int dmg)
    {
        Hp -= dmg;
        Debug.Log($"Mole hit! Remaining HP: {Hp}");
        if (Hp <= 0)
        {
            SquashMole();
        }
        else
        {
            _animator.Play(HitHash, 0, 0f);
        }
    }

    public void MoleUp(bool golden)
    {
        if (golden)
        {
            type = MoleType.Golden;
            Hp = 4;
            moleRenderer.material = goldenMaterial;

        }
        else
        {
            type = MoleType.Normal;
            Hp = 2;
            if (moleRenderer.material != normalMaterial)
            {
                moleRenderer.material = normalMaterial;
            }
        }
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
        CoinManager.Instance.AddCoins(score);
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
