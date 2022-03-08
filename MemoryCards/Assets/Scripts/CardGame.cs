/***
 * Created By: Kangjie Ouyang
 * Date Created: 3/4/2022
 * 
 * Last Edited: N/A
 * Last Edited By: N/A
 * 
 * Description: The actual card dealing and comparing class
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CardGame : MonoBehaviour
{
    [Header("List of Comparison Cards")]
    public List<Card> cardComparison;

    [Header("List of Card Types")]
    public List<Card.CardType> cardsToBePutIn;


    public Transform[] positions;

    [Header("Matched Card Count")]
    public int matchedCardCount = 0;

    public bool level_1 = true;

    int restartCount = 0;

    public GameManager GM;


    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>(); //add the GM to the card
        GenerateRandomCards();
    }

    void SetupCardsToBePutIn() //put enum list into the list
    {
        Array array = Enum.GetValues(typeof(Card.CardType));
        cardsToBePutIn.Clear();
        foreach (var item in array)
        {
            cardsToBePutIn.Add((Card.CardType)item);
        }
        cardsToBePutIn.RemoveAt(0); //remove null
    }

    void GenerateRandomCards() //deal the cards
    {
        if (level_1)
        {
            int positionIdx = 0;
            for (int i = 0; i < 2; i++) //8 cards, 4 each
            {
                SetupCardsToBePutIn(); //get the cards ready
                int maxRandomNumber = 4; //no more than 4

                for (int j = 0; j < maxRandomNumber; maxRandomNumber--)
                {
                    int randomNumber = UnityEngine.Random.Range(0, maxRandomNumber); //get a random nunber between 0 and 3
                    AddNewCard(cardsToBePutIn[randomNumber], positionIdx); //add the new card
                    cardsToBePutIn.RemoveAt(randomNumber); //remove it from the list
                    positionIdx++;
                }
            }
        }
        else
        {
            int positionIdx = 0;
            for (int i = 0; i < 2; i++) //16 cards, 8 each
            {
                SetupCardsToBePutIn(); //get the cards ready
                int maxRandomNumber = cardsToBePutIn.Count; //no more than 8

                for (int j = 0; j < maxRandomNumber; maxRandomNumber--)
                {
                    int randomNumber = UnityEngine.Random.Range(0, maxRandomNumber); //get a random nunber between 0 and 7
                    AddNewCard(cardsToBePutIn[randomNumber], positionIdx); //add the new card
                    cardsToBePutIn.RemoveAt(randomNumber); //remove it from the list
                    positionIdx++;
                }
            }
        }

    }

    void AddNewCard(Card.CardType cardType, int positionIndex) // add a new card based on card type
    {
        if (level_1)
        {
            GameObject card = Instantiate(Resources.Load<GameObject>("Prefabs/Card")); //create a new empty object
            card.GetComponent<Card>().cardType = cardType; //get the card type 
            card.name = "Card_" + cardType.ToString(); //rename it to the card type
            card.transform.position = positions[positionIndex].position;

            GameObject graphic = Instantiate(Resources.Load<GameObject>("Prefabs/pic")); //create a new empty object
            graphic.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Graphics1/" + cardType.ToString()); //add to spirit
            graphic.transform.SetParent(card.transform); //turns into child object of card
            graphic.transform.localPosition = new Vector3(0, 0, 0.1f); //location
            graphic.transform.eulerAngles = new Vector3(0, 180, 0); //180 degrees turn
        }
        else
        {
            GameObject card = Instantiate(Resources.Load<GameObject>("Prefabs/Card")); //create a new empty object
            card.GetComponent<Card>().cardType = cardType; //get the card type 
            card.name = "Card_" + cardType.ToString(); //rename it to the card type
            card.transform.position = positions[positionIndex].position;

            GameObject graphic = Instantiate(Resources.Load<GameObject>("Prefabs/pic")); //create a new empty object
            graphic.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Graphics2/" + cardType.ToString()); //add to spirit
            graphic.transform.SetParent(card.transform); //turns into child object of card
            graphic.transform.localPosition = new Vector3(0, 0, 0.1f); //location
            graphic.transform.eulerAngles = new Vector3(0, 180, 0); //180 degrees turn
        }

    }

    public void AddCardInCardComparison(Card card) //add card to the list
    {
        cardComparison.Add(card);
    }

    public bool ReadyToCompareCards //if list has more than 2 card, then its ready to compare 
    {
        get
        {
            if (cardComparison.Count == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public void CompareCardsInList() //the actual comapre method
    {
        if (ReadyToCompareCards)
        {
            if (cardComparison[0].cardType == cardComparison[1].cardType)
            {
                Debug.Log("same card");
                foreach (var card in cardComparison)
                {
                    card.cardState = Card.CardState.Match;
                }
                ClearCardComparison();
                matchedCardCount += 2;

                if (matchedCardCount >= positions.Length)
                {
                    restartCount++;
                    level_1 = false;
                    GM.nextLevel = true;
                }

                if (restartCount == 1)
                {
                    level_1 = true;
                }
            }
            else
            {
                Debug.Log("differnet card");
                StartCoroutine(MissMatchCards());
            }

        }
    }

    void ClearCardComparison() //clear the card list
    {
        cardComparison.Clear();
    }

    void TurnBackCard() //turn both cards back if not a match
    {
        foreach (var card in cardComparison)
        {
            card.gameObject.transform.eulerAngles = Vector3.zero;
            card.cardState = Card.CardState.Unflipped;
        }
    }

    IEnumerator MissMatchCards() //let the program wait for 1.5 seconds, so we can see the cards flipping
    {
        yield return new WaitForSeconds(1.5f);
        TurnBackCard();
        ClearCardComparison();
    }

}
