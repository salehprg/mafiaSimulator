using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    public float balance = 0;
    public float minBalance = 0;
    public float maxBalance = -1;

    public float DepositBalance(float amount)
    {
        balance += amount;
        if (maxBalance != -1)
            balance = balance > maxBalance ? maxBalance : balance;

        return amount;
    }

    public float WithdrawBalance(float amount)
    {
        balance -= amount;
        balance = balance < minBalance ? minBalance : balance;

        return amount;
    }

}
