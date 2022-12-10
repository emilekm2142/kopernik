using UnityEngine;

public class CoutdownTimer
{
    float timeRemaining = 0;
    private bool isTimerCoutdown = false;

    public int secToDafault;

    public void SetDefaultCoutdown(int _secToDafault)
    {
        secToDafault = _secToDafault;
    }
    public void StartTimer()
    {
        isTimerCoutdown = true;
        timeRemaining = secToDafault;
    }

    public bool Update()
    {
        if (isTimerCoutdown)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                return false;
            }
            isTimerCoutdown = false;
            return true;
        }
        return false;
    }

    public bool isInUse() => isTimerCoutdown;
}