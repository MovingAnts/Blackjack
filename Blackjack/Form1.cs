using System;
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
    public partial class Form1 : Form
    {
        Game newGame;
        UI ui;
        /// <summary>
        /// 构造函数
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        /// <summary>
        /// 开始按钮触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_start_Click(object sender, EventArgs e)
        {
            int playerNum = 0;
            int money = 0;
            bool ifMoneyStandard = true;
            bool ifPlayNumStandard = true;
            Regex rex = new Regex(@"^\d+$");
            ui = new UI(newGame);
            ui.hideControl("playNumError");
            ui.hideControl("moneyError");
            //判断玩家人数是否合法
            if (rex.IsMatch(textbox_playerNum.Text))
            {
                playerNum = int.Parse(textbox_playerNum.Text);
                if (playerNum < 1 || playerNum > 5)
                {
                    ifPlayNumStandard = false;
                }
            }
            else
            {
                ifPlayNumStandard = false;
            }
            //判断金钱是否合法
            if (rex.IsMatch(textbox_money.Text))
            {
                money = int.Parse(textbox_money.Text);
                if (money < 50 || money > 50000)
                {
                    ifMoneyStandard = false;
                }
            }
            else
            {
                ifMoneyStandard = false;
            }
            //金钱、玩家人数合法
            if (ifMoneyStandard == true && ifPlayNumStandard == true)
            {
                newGame = new Game(playerNum, money);
                ui = new UI(newGame);
                ui.startGame();
                
            }
            else
            {
                //UI类画图显示错误信息
                if (ifMoneyStandard == false)
                {
                    ui.showControl("moneyError");
                    ui.WriteText("moneyError", "Invaild");
                }
                if (ifPlayNumStandard == false)
                {
                    ui.showControl("playNumError");
                    ui.WriteText("playNumError", "Invaild");
                }
            }
        }

        /// <summary>
        /// 下注按钮触发事件
        /// </summary>
        public void addBet(int money)
        {
            ui.hideControl("label_betError");
            bool result = newGame.operate(OPERATION.addBet, money);
            if (result==true)
            {
                ui.WriteText("label_currentMoney", "Money:" + (newGame.getCurrentPlayer().Money - newGame.getCurrentPlayer().Bet).ToString());
                ui.WriteText("label_betMoney", (Convert.ToInt32(label_betMoney.Text) + money).ToString());
            }
            else
            {
                ui.showControl("label_betError");
                ui.WriteText("label_betError", "Not Enough");
            }
        }

        /// <summary>
        /// deal按钮触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_deal_Click(object sender, EventArgs e)
        {
            //判断下注是否为0
            ui.hideControl("label_betError");
            if (newGame.getCurrentPlayer().Bet == 0)
            {
                ui.showControl("label_betError");
                ui.WriteText("label_betError", "Invail Bet");
                return;
            }
            bool result = newGame.operate(OPERATION.deal, 0);
            ui.dealGame();
            string control = "label_player" + (newGame.CurrentPlayer + 1) + "Score";
            string text = "Player" + (newGame.CurrentPlayer + 1) + ":";
            //判断玩家是否为黑杰克
            if (result == false)
            {
                text = text + "Blackjack!";
                //庄家的明牌是否为A
                if (newGame.Dealer.Cards[1] % 13 == 0)
                {
                    ui.isCheckIn();
                }else
                {
                    //不为A，直接跳转到下一个玩家
                    newGame.CurrentPlayer++;
                    init();
                }
            }else
            {
                text = text + newGame.getCurrentPlayer().Score.ToString() + "点";
            }    
            ui.hideControl("label_betError");
            ui.WriteText(control, text);
        }

        /// <summary>
        /// hit按钮触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_hit_Click(object sender, EventArgs e)
        {
            bool result = newGame.operate(OPERATION.hit, 0);
            string control = "label_player" + (newGame.CurrentPlayer + 1) + "Score";
            string text = "Player" + (newGame.CurrentPlayer + 1) + ":";

            ui.showCard(newGame.CurrentPlayer + 1, newGame.getCurrentPlayer().Cards.Count);
            //判断是否boom
            if (result == true)
            {
                text = text + newGame.getCurrentPlayer().Score.ToString() + "点";
                //判断是否21点
                if (newGame.getCurrentPlayer().Score == 21)
                {
                    newGame.CurrentPlayer++;
                    init();
                }
            }
            else
            {
                text = text + "boom";
                newGame.CurrentPlayer++;
                init();
            }
            ui.hideControl("label_betError");
            ui.WriteText(control, text);
            ui.hideControl("button_double");
            //显示玩家i的第j张牌
        }

        /// <summary>
        /// stand按钮触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_stand_Click(object sender, EventArgs e)
        {
            newGame.CurrentPlayer++;
            init();
        }

        /// <summary>
        /// double按钮触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_double_Click(object sender, EventArgs e)
        {
            ui.hideControl("label_betError");
            if (!newGame.operate(OPERATION.doubleStake, 0))
            {
                //UI类画图，显示钱数不够信息
                ui.showControl("label_betError");
                ui.WriteText("label_betError", "Not Enough");
            }
            else
            {
                label_currentMoney.Text = "Money:" + (newGame.getCurrentPlayer().Money - newGame.getCurrentPlayer().Bet).ToString();
                label_betMoney.Text = Convert.ToInt32(label_betMoney.Text).ToString();
                //执行hit操作
                bool result = newGame.operate(OPERATION.hit, 0);
                string control = "label_player" + (newGame.CurrentPlayer + 1) + "Score";
                string text = "Player" + (newGame.CurrentPlayer + 1) + ":";

                ui.showCard(newGame.CurrentPlayer + 1, newGame.getCurrentPlayer().Cards.Count);
                //判断是否boom
                if (result == true)
                {
                    text = text + newGame.getCurrentPlayer().Score.ToString() + "点";
                }
                else
                {
                    text = text + "boom";
                }
                //为控件control的Text属性赋值text
                ui.WriteText(control, text);
                newGame.CurrentPlayer++;
                init();
            }
        }

        /// <summary>
        /// 跳转到下一个玩家，初始化界面
        /// </summary>
        private void init()
        {
            while (newGame.getCurrentPlayer()!=null&&newGame.getCurrentPlayer().Money < 5)
            {
                newGame.CurrentPlayer++;
            }
            //跳转到下一个玩家
            ui.initPlayer(); 
            //判断是否结束回合
            if (newGame.isOver())
            {
                //结束回合显示回合结果
                ui.showResult();
            }
            if (newGame.isGameOver())
            {
                ui.showGameResult();
            }
        }

        /// <summary>
        /// checkIn触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_checkIn_Click(object sender, EventArgs e)
        {
            newGame.operate(OPERATION.checkIn, 0);
            newGame.CurrentPlayer++;
            init();
        }

        /// <summary>
        /// notCheckIn触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_notCheckIn_Click(object sender, EventArgs e)
        {
            newGame.operate(OPERATION.notCheckIn, 0);
            newGame.CurrentPlayer++;
            init();
        }

        /// <summary>
        /// 5元砝码触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_5_Click(object sender, EventArgs e)
        {
            addBet(50);
        }

        /// <summary>
        /// 10元砝码触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_10_Click(object sender, EventArgs e)
        {
            addBet(100);
        }

        /// <summary>
        /// 25元砝码触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_25_Click(object sender, EventArgs e)
        {
            addBet(500);
        }

        /// <summary>
        /// 100元砝码触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_100_Click(object sender, EventArgs e)
        {
            addBet(1000);
        }

        /// <summary>
        /// goOn按钮触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_goOn_Click(object sender, EventArgs e)
        {
            newGame.CurrentPlayer = 0;
            newGame.init();
            ui.startGame();
            init();
        }

        /// <summary>
        /// retry按钮触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_retry_Click(object sender, EventArgs e)
        {
            ui.retry();
            Close();
        }

        private void button_5_MouseEnter(object sender, EventArgs e)
        {
            button_5.BackgroundImage = Image.FromFile(System.Windows.Forms.Application.StartupPath + "\\..\\..\\image\\50_2.png", false);
        }

        private void button_5_MouseLeave(object sender, EventArgs e)
        {
            button_5.BackgroundImage = Image.FromFile(System.Windows.Forms.Application.StartupPath + "\\..\\..\\image\\50_1.png", false);
        }

        private void button_10_MouseEnter(object sender, EventArgs e)
        {
            button_10.BackgroundImage = Image.FromFile(System.Windows.Forms.Application.StartupPath + "\\..\\..\\image\\100_2.png", false);
        }

        private void button_10_MouseLeave(object sender, EventArgs e)
        {
            button_10.BackgroundImage = Image.FromFile(System.Windows.Forms.Application.StartupPath + "\\..\\..\\image\\100_1.png", false);
        }

        private void button_25_MouseEnter(object sender, EventArgs e)
        {
            button_25.BackgroundImage = Image.FromFile(System.Windows.Forms.Application.StartupPath + "\\..\\..\\image\\500_2.png", false);
        }

        private void button_25_MouseLeave(object sender, EventArgs e)
        {
            button_25.BackgroundImage = Image.FromFile(System.Windows.Forms.Application.StartupPath + "\\..\\..\\image\\500_1.png", false);
        }

        private void button_100_MouseEnter(object sender, EventArgs e)
        {
            button_100.BackgroundImage = Image.FromFile(System.Windows.Forms.Application.StartupPath + "\\..\\..\\image\\1000_2.png", false);
        }

        private void button_100_MouseLeave(object sender, EventArgs e)
        {
            button_100.BackgroundImage = Image.FromFile(System.Windows.Forms.Application.StartupPath + "\\..\\..\\image\\1000_1.png", false);
        }

        private void button_deal_MouseEnter(object sender, EventArgs e)
        {
            button_deal.BackgroundImage = Image.FromFile(System.Windows.Forms.Application.StartupPath + "\\..\\..\\image\\deal_2.png", false);
        }

        private void button_deal_MouseLeave(object sender, EventArgs e)
        {
            button_deal.BackgroundImage = Image.FromFile(System.Windows.Forms.Application.StartupPath + "\\..\\..\\image\\deal_1.png", false);
        }
    }
}
