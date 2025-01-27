using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationExchange : MonoBehaviour
{
    public Stack<Information> informationStack = new Stack<Information>();

    public void addInformation(Information information)
    {
        informationStack.Push(information);
    }
}
