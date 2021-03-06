﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    public enum OPERATION
    {
        addBet,
        hit,
        doubleStake,
        deal,
        checkIn,
        notCheckIn
    }

    class Game
    {

        Player[] player;
        Dealer dealer;
        int[] card;
        const int cardNum = 208;
        int playerNum;
        int currentPlayer;
        int currentCard;

        public int CurrentPlayer
        {
            get
            {
                return currentPlayer;
            }

            set
            {
                currentPlayer = value;
            }
        }

        internal Player[] Player
        {
            get
            {
                return player;
            }

            set
            {
                player = value;
            }
        }

        internal Dealer Dealer
        {
            get
            {
                return dealer;
            }

            set
            {
                dealer = value;
            }
        }

        public int PlayerNum
        {
            get
            {
                return playerNum;
            }

            set
            {
                playerNum = value;
            }
        }

        /// <summary>
        /// 初始化游戏
        /// </summary>
        public void init()
        {
            dealer.init();
            for (int i = 0; i < PlayerNum; i++)
            {
                player[i].init();
            }
            currentCard = currentCard % cardNum;
            Dealer.hit(card[currentCard++]);
            Dealer.hit(card[currentCard++]);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="playerNum"></param>
        /// <param name="initMoney"></param>
        public Game(int playerNum, int initMoney)
        {
            Player = new Player[playerNum];
            this.PlayerNum = playerNum;
            Dealer = new Dealer();
            card = new int[cardNum];
            currentCard = 0;
            CurrentPlayer = 0;
            for (int i = 0; i < playerNum; i++)
            {
                Player[i] = new Player();
                Player[i].setMoney(initMoney);
            }
            initCard();
        }

        /// <summary>
        /// 将牌打乱
        /// </summary>
        public void initCard()
        {
            int[] result = new int[cardNum];
            for (int i = 0; i < cardNum ; i++)
            {
                result[i] = i;
            }
            Random r = new Random();
            int max = cardNum;//最大的索引位置
            for (int j = 0; j < cardNum; j++)
            {
                //从0~最大的索引位置中取索引
                int tmp = r.Next(0, max);
                //将原数组的tmp位置的值替换成原数组中最后一个值
                card[j] = result[tmp];
                result[tmp] = result[max - 1];
                max--;
            }
            Dealer.hit(card[currentCard++]);
            Dealer.hit(card[currentCard++]);
        }

        /// <summary>
        /// 操作函数，根据不同指令执行对应函数
        /// </summary>
        /// <param name="operatoin"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public bool operate(OPERATION operatoin, int money)
        {
            bool result = true;
            switch (operatoin)
            {
                case OPERATION.addBet: result = Player[CurrentPlayer].addBet(money); break;
                case OPERATION.hit:
                    if (!Player[CurrentPlayer].hit(card[currentCard++]))
                    {
                        result = false;
                    }
                    break;
                case OPERATION.doubleStake: result = Player[CurrentPlayer].doubleStake(); break;
                case OPERATION.deal:
                    Player[CurrentPlayer].hit(card[currentCard++]);
                    Player[CurrentPlayer].hit(card[currentCard++]);
                    if (Player[CurrentPlayer].ifBlackJack())
                    {
                        result = false;
                    }
                    break;
                case OPERATION.checkIn: result = false; Player[CurrentPlayer].setBlackJack(1); break;
                case OPERATION.notCheckIn: result = false; Player[CurrentPlayer].setBlackJack(1.5); break;
            }
            return result;
        }

        /// <summary>
        /// 判断当前玩家是否是黑杰克
        /// </summary>
        /// <returns></returns>
        public bool ifCurrentPlayBlackjack()
        {
            bool ifBlackJack = Player[CurrentPlayer].ifBlackJack();
            if (ifBlackJack)
            {
                Player[CurrentPlayer].setBlackJack(2);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断本回合是否结束
        /// </summary>
        /// <returns></returns>
        public bool isOver()
        {
            if (CurrentPlayer >= PlayerNum)
            {
                while (!Dealer.over17())
                {
                    Dealer.hit(card[currentCard++]);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取当前玩家
        /// </summary>
        /// <returns></returns>
        public Player getCurrentPlayer()
        {
            if (CurrentPlayer < PlayerNum)
            {
                return Player[CurrentPlayer];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 判断游戏是否结束
        /// </summary>
        /// <returns></returns>
        public bool isGameOver()
        {
            bool result = true;
            for(int i = 0; i < playerNum; i++)
            {
                if (player[i].Money >= 5)
                {
                    result = false;
                }
            }
            return result;
        }
    }
}