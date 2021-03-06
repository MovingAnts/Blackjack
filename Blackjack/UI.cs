﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Blackjack
{
    class UI
    {
        Game newGame;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="newGame"></param>
        public UI(Game newGame)
        {
            this.newGame = newGame;
        }

        /// <summary>
        ///显示第playerIndex位玩家的第cardIndex张牌
        /// </summary>
        /// <param name="playerIndex"></param>
        /// <param name="cardIndex"></param>
        public void showCard(int playerIndex, int cardIndex)
        {
            Application.OpenForms["Form1"].Controls["card_player" + playerIndex + cardIndex].Visible = true;
            Application.OpenForms["Form1"].Controls["card_player" + playerIndex + cardIndex].BackgroundImage = Image.FromFile(System.Windows.Forms.Application.StartupPath + "\\..\\..\\image\\" + (newGame.getCurrentPlayer().Cards[cardIndex - 1] % 52 + 1) + ".jpg", false);
        }

        /// <summary>
        /// 显示控件名为controlName的控件
        /// </summary>
        /// <param name="controlName"></param>
        public void showControl(string controlName)
        {
            Application.OpenForms["Form1"].Controls[controlName].Visible = true;
        }

        /// <summary>
        /// 隐藏控件名为controlName的控件
        /// </summary>
        /// <param name="controlName"></param>
        public void hideControl(string controlName)
        {
            Application.OpenForms["Form1"].Controls[controlName].Visible = false;
        }

        /// <summary>
        /// 使控件名为controlName的控件可用
        /// </summary>
        /// <param name="controlName"></param>
        public void enableControl(string controlName)
        {
            Application.OpenForms["Form1"].Controls[controlName].Enabled = true;
        }

        /// <summary>
        /// 使控件名为controlName的控件不可用
        /// </summary>
        /// <param name="controlName"></param>
        public void unEnableControl(string controlName)
        {
            Application.OpenForms["Form1"].Controls[controlName].Enabled = false;
        }

        /// <summary>
        /// 为控件名为controlName的控件的Text属性赋值text
        /// </summary>
        /// <param name="controlName"></param>
        /// <param name="text"></param>
        public void WriteText(string controlName, string text)
        {
            Application.OpenForms["Form1"].Controls[controlName].Text = text;
        }

        /// <summary>
        /// 在控件名firstControlName的控件下找二级控件secondControlName，为其Text属性赋值text
        /// </summary>
        /// <param name="firstControlName"></param>
        /// <param name="secondControlName"></param>
        /// <param name="text"></param>
        public void WriteSecondText(string firstControlName,string secondControlName,string text)
        {
            Application.OpenForms["Form1"].Controls[firstControlName].Controls[secondControlName].Text = text;
        }

        /// <summary>
        /// 一回合显示游戏结果
        /// </summary>
        public void showResult()
        {
            for(int i = 0; i < newGame.Dealer.Cards.Count; i++)
            {
                showControl("card_dealer" + (i + 1));
                Application.OpenForms["Form1"].Controls["card_dealer"+(i+1)].BackgroundImage = Image.FromFile(System.Windows.Forms.Application.StartupPath + "\\..\\..\\image\\" + (newGame.Dealer.Cards[i] % 52 + 1) + ".jpg", false);
            }
            showControl("label_dealerScore");
            WriteText("label_dealerScore", "Dealer:"+newGame.Dealer.Score.ToString()+"点");
            for (int i = 0; i < newGame.PlayerNum; i++)
            {
                RESULT result = newGame.Player[i].compare(newGame.Dealer.Score);
                WriteSecondText("result_table","label_result" + (i + 1) + 1, "Player"+(i+1));
                WriteSecondText("result_table", "label_result" + (i + 1) + 3, newGame.Player[i].Money.ToString());
                if (result == RESULT.WIN)
                {
                    WriteSecondText("result_table", "label_result" + (i + 1) + 2, "+"+newGame.Player[i].Bet);
                }
                else if(result==RESULT.DRAW)
                {
                    WriteSecondText("result_table", "label_result" + (i + 1) + 2, "0");
                }else
                {
                    WriteSecondText("result_table", "label_result" + (i + 1) + 2, "-" + newGame.Player[i].Bet);
                }
            }
            hideControl("button_10");
            hideControl("button_100");
            hideControl("button_5");
            hideControl("button_25");
            hideControl("button_deal");
            hideControl("label_betError");
            hideControl("label_betMoney");
            showControl("button_goOn");    
            showControl("result_table");
        }

        /// <summary>
        /// 所有玩家全部输光筹码，显示结果
        /// </summary>
        public void showGameResult()
        {
            hideControl("button_goOn");
            showControl("label_gameOver");
            showControl("button_retry");
        }

        /// <summary>
        /// retry按钮触发事件
        /// </summary>
        public void retry()
        {
            //System.Environment.Exit(0);
            System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        /// <summary>
        /// 点击开始按钮ui绘图
        /// </summary>
        public void startGame()
        {
            showControl("button_10");
            showControl("button_100");
            showControl("button_5");
            showControl("button_25");
            showControl("button_deal");
            showControl("label_currentPlayer");
            showControl("picbox_currentPlayer");
            showControl("label_currentMoney");
            showControl("label_betMoney");
            showControl("label_dealerScore");
            showControl("card_dealer1");
            showControl("card_dealer2");
            hideControl("label_betError");
            hideControl("textbox_money");
            hideControl("textbox_playerNum");
            hideControl("label1");
            hideControl("label2");
            hideControl("playNumError");
            hideControl("moneyError");
            hideControl("button_start");
            hideControl("button_goOn");
            hideControl("result_table");
            for(int i = 1; i <= newGame.PlayerNum; i++)
            {
                for(int j = 1; j <= 11; j++)
                {
                    hideControl("card_player" + i + j);
                }
                hideControl("label_player"+i+"Score");
            }
            hideControl("label_dealerScore");
            for(int i = 3; i <= 11; i++)
            {
                hideControl("card_dealer" + i);
            }
            WriteText("label_currentPlayer", "Player" + (newGame.CurrentPlayer + 1));
            WriteText("label_currentMoney", "Money:" + (newGame.getCurrentPlayer().getMoney()));
            Application.OpenForms["Form1"].Controls["card_dealer1"].BackgroundImage = Image.FromFile(System.Windows.Forms.Application.StartupPath + "\\..\\..\\image\\back.jpg", false);
            Application.OpenForms["Form1"].Controls["card_dealer2"].BackgroundImage = Image.FromFile(System.Windows.Forms.Application.StartupPath + "\\..\\..\\image\\" + (newGame.Dealer.Cards[1] % 52 + 1) + ".jpg", false);
            Application.OpenForms["Form1"].Controls["picbox_currentPlayer"].BackgroundImage = Image.FromFile(System.Windows.Forms.Application.StartupPath + "\\..\\..\\image\\Player_" + (newGame.CurrentPlayer+1) + ".png", false);
            Application.OpenForms["Form1"].Width = 896;
            Application.OpenForms["Form1"].Height = 680;
        }

        /// <summary>
        /// 跳转下一位玩家时ui绘图
        /// </summary>
        public void initPlayer()
        {
            WriteText("label_betMoney", "0");
            WriteText("label_betError", "");
            showControl("label_betError");
            enableControl("button_10");
            enableControl("button_100");
            enableControl("button_5");
            enableControl("button_25");
            enableControl("button_deal");
            hideControl("button_double");
            hideControl("button_hit");
            hideControl("button_stand");
            hideControl("button_checkIn");
            hideControl("label_isCheckIn");
            hideControl("button_notCheckIn");
            if (newGame.CurrentPlayer < newGame.PlayerNum)
            {
                WriteText("label_currentPlayer", "Player" + (newGame.CurrentPlayer + 1));
                WriteText("label_currentMoney", "Money:" + (newGame.getCurrentPlayer().getMoney()));
                Application.OpenForms["Form1"].Controls["picbox_currentPlayer"].BackgroundImage = Image.FromFile(System.Windows.Forms.Application.StartupPath + "\\..\\..\\image\\Player_" + (newGame.CurrentPlayer+1) + ".png", false);
            }
            else
            {
                WriteText("label_currentPlayer", "");
                WriteText("label_currentMoney", "");
            }
        }

        /// <summary>
        /// 玩家点击deal按钮绘图
        /// </summary>
        public void dealGame()
        {
            showControl("button_double");
            showControl("button_hit");
            showControl("button_stand");
            unEnableControl("button_5");
            unEnableControl("button_25");
            unEnableControl("button_10");
            unEnableControl("button_100");
            unEnableControl("button_deal");
            string control = "label_player" + (newGame.CurrentPlayer + 1) + "Score";
            showControl(control);
            showCard(newGame.CurrentPlayer + 1, 1);
            showCard(newGame.CurrentPlayer + 1, 2);
        }

        /// <summary>
        /// 是否立即报道时绘图
        /// </summary>
        public void isCheckIn()
        {
            showControl("button_checkIn");
            showControl("button_notCheckIn");
            showControl("label_isCheckIn");
            hideControl("button_hit");
            hideControl("button_double");
            hideControl("button_stand");
        }
    }
}
