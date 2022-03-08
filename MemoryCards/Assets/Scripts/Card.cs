/***
 * Created By: Kangjie Ouyang
 * Date Created: 3/4/2022
 * 
 * Last Edited: N/A
 * Last Edited By: N/A
 * 
 * Description: Cards
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public CardState cardState;
    public CardType cardType;
    public CardGame GM;

    void Start()
    {
        cardState = CardState.Unflipped;
        GM = GameObject.FindGameObjectWithTag("CardGame").GetComponent<CardGame>(); //add the GM to the card
    }

    private void OnMouseUp()
    {
        if(cardState.Equals(CardState.Flipped)) //cannot click on an already flipped card to compare
        {
            return;
        }

        if (GM.ReadyToCompareCards) //can only click max 2 cards during "comparison execution"
        {
            return;
        }

        OpenCard();
        GM.AddCardInCardComparison(this);
        GM.CompareCardsInList();
    }

    void OpenCard() //flip the card 180 degrees upon clicking on the card
    {
        transform.eulerAngles = new Vector3(0, 180, 0); 
        cardState = CardState.Flipped;
    }

    public enum CardState 
    {
        Unflipped, Flipped, Match
    }

    public enum CardType 
    {
        Null, Apple, Grape, Kiwi, Lemon, Orange, Peach, Pear, Watermelon
    }
}
