using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public Card CardPrefab;
    public GameObject Dealer;
    public GameObject Player;

    [Min(100)]
    public int ShuffleCount = 100;

    List<Card.Data> cards;

    private void Awake()
    {
        InitCards(); //確認用のコード
    }

    private void Update()
    {//確認用のコード
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DealCards();
        }
    }

    void InitCards()
    {
        cards = new List<Card.Data>(13 * 4);
        var marks = new List<Card.Mark>() {
            Card.Mark.Heart,
            Card.Mark.Diamond,
            Card.Mark.Spade,
            Card.Mark.Crub,
        };

        foreach (var mark in marks)
        {
            for (var num = 1; num <= 13; ++num)
            {
                var card = new Card.Data()
                {
                    Mark = mark,
                    Number = num,
                };
                cards.Add(card);
            }
        }

        ShuffleCards();
    }

    void ShuffleCards()
    {
        //シャッフルする
        var random = new System.Random();
        for (var i = 0; i < ShuffleCount; ++i)
        {
            var index = random.Next(cards.Count);
            var index2 = random.Next(cards.Count);

            //カードの位置を入れ替える。
            var tmp = cards[index];
            cards[index] = cards[index2];
            cards[index2] = tmp;
        }
    }

    Card.Data DealCard()
    {
        if (cards.Count <= 0) return null;

        var card = cards[0];
        cards.Remove(card);
        return card;
    }

    void DealCards()
    {
        foreach (Transform card in Dealer.transform)
        {
            Object.Destroy(card.gameObject);
        }

        foreach (Transform card in Player.transform)
        {
            Object.Destroy(card.gameObject);
        }


        {
            //ディーラーに２枚カードを配る
            var holeCardObj = Object.Instantiate(CardPrefab, Dealer.transform);
            var holeCard = DealCard();
            holeCardObj.SetCard(holeCard.Number, holeCard.Mark, true);

            var upCardObj = Object.Instantiate(CardPrefab, Dealer.transform);
            var upCard = DealCard();
            upCardObj.SetCard(upCard.Number, upCard.Mark, false);
        }

        {
            //プレイヤーにカードを２枚配る
            for (var i = 0; i < 2; ++i)
            {
                var cardObj = Object.Instantiate(CardPrefab, Player.transform);
                var card = DealCard();
                cardObj.SetCard(card.Number, card.Mark, false);
            }
        }
    }
}