using UnityEngine;

public class RoundText : MonoBehaviour
{
    public static RoundText Instance;
    public int currentRound = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AdvanceRound()
    {
        currentRound++;
    }

    public void ResetRounds()
    {
        currentRound = 1;
    }
}
