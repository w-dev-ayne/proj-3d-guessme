using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class RandomGenerator
{
    private int maxNum;
    private List<int> pickedNums = new List<int>();

    public RandomGenerator(int exclusiveMaxNum)
    {
        this.maxNum = exclusiveMaxNum;
    }

    public int GenerateRandomNum()
    {
        float temp = Time.time * 100f;
        Random.InitState((int)temp);
        int random = Random.Range(0, maxNum);

        if (pickedNums.Contains(random))
        {
            return GenerateRandomNum();
        }
        else
        {
            pickedNums.Add(random);
            return random;
        }
    }
}
