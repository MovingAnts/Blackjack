using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    class Dealer
    {
        int score;
        List<int> cards =new List<int>();
        int aNumber = 0;
        int oneNumber = 0;

        public List<int> Cards
        {
            get
            {
                return cards;
            }

            set
            {
                cards = value;
            }
        }

        public int Score
        {
            get
            {
                return score;
            }

            set
            {
                score = value;
            }
        }

        public void init()
        {
            score = 0;
            cards.Clear();
            aNumber = 0;
            oneNumber = 0;
        }

        public bool hit(int card)//false为爆牌
        {
            Cards.Add(card);
            int cardNumber = card % 13;
            if (cardNumber > 0 && cardNumber < 10)
                Score += cardNumber + 1;
            else
                if (cardNumber == 0)
            {
                Score += 11;
                aNumber++;
            }
            else Score += 10;
            if (Score > 21)
            {
                if (aNumber > oneNumber)
                {
                    oneNumber++;
                    Score -= 10;
                    if (Score <= 21)
                        return true;
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool over17()
        {
            return Score >= 17;
        }
    }
}
