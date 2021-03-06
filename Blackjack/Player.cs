﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    public enum RESULT
    {
        LOSE,
        WIN,
        DRAW
    }
    class Player
    {
        List<int> cards =new List<int>();
        double money;
        int bet=0;
        int score = 0;
        int aNumber = 0;
        int oneNumber=0;
        double blackJact = 0;

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

        public int Bet
        {
            get
            {
                return bet;
            }

            set
            {
                bet = value;
            }
        }

        public double Money
        {
            get
            {
                return money;
            }

            set
            {
                money = value;
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
            bet = 0;
            score = 0;
            aNumber = 0;
            oneNumber = 0;
            blackJact = 0;
            cards.Clear();
        }
        public bool setMoney(int number)
        {
            if (number <= 0)
                return false;
            Money = number;
            return true;
        }
        public bool addBet(int b)//false为不够钱
        {
            if (Bet + b > Money)
                return false;
            else
            {
                Bet += b;
                return true;
            }
        }
        public bool hit(int card)//false为爆牌
        {
            Cards.Add(card);
            int cardNumber = card % 13;
            if(cardNumber>0&&cardNumber<10)
                Score += cardNumber+1;
            else
                if(cardNumber==0)
                {
                    Score+=11;
                    aNumber++;
                }
                else Score+=10;
            if(Score>21)
            {
                if (aNumber > oneNumber)
                {
                    oneNumber++;
                    Score -= 10;
                    if(Score<=21)
                    return true;
                }
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool ifBlackJack()
        {
            if (Score == 21)
                return true;
            else
                return false;
        }
        public void setBlackJack(double multiplying)
        {
            blackJact = multiplying;
        }
        public bool doubleStake()//false为钱不够
        {
            if(Bet*2>Money)
            {
                return false;
            }
            else
            {
                Bet *= 2;
                return true;
            }
        }
        public RESULT compare(int s)//true为玩家赢，false为庄家赢
        {
            if (blackJact == 1 || blackJact == 2||(blackJact==1.5&&s!=21))
            {
                Money += Bet * blackJact;
                return RESULT.WIN;
            }
            if(Score<=21)
            {
                if (s > 21||s<Score)
                {
                    Money += Bet;
                    return RESULT.WIN;
                }
                else
                {
                    if(s==Score)
                    {
                        return RESULT.DRAW;
                    }
                }
            }
            Money -= Bet;
            return RESULT.LOSE;
        }
        public double getMoney()//得到金钱数
        {
            return Money;
        }
    }
}
