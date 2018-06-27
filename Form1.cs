using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Reversi
{
    public partial class Form1 : Form
    {
        Board board;
        bool flag = false;
        private bool IsDrawHelp = true;

        public Form1()
        {
            InitializeComponent();
            panel1.Width = 8*80;
            panel1.Height = 8*80;
            InitBoard();
        }

        private void InitBoard()
        {
            label2.Text = "2";
            label1.Text = "2";
            board = new Board();
            board.Draw(panel1, true, 1);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            board.Draw(e.Graphics, false, 0);
        }

        private void DoBestStep()
        {
            List<int[]> l = board.GetEnableSteps(-1);
            int j = 0;
            int max = -Int32.MaxValue;
            for (int i = 0; i < l.Count; i++)
            {
                Board cp = board.Copy();
                cp.AddFig(l[i][1], l[i][0], -1, true);
                int res = Board.GetBestStep(1, max, Int32.MaxValue, 0, cp, panel1);
                if (max < res)
                {
                    j = i;
                    max = res;
                }
            }
            if (l.Count > j)
                board.AddFig(l[j][1], l[j][0], -1, true);
        }

        private void CompStep()
        {
            do
            {
                DoBestStep();
            } while (board.GetEnableSteps(1).Count == 0 && board.GetEnableSteps(-1).Count > 0);
            board.Draw(panel1, IsDrawHelp, 1);
            int PlayerSteps = board.GetEnableSteps(1).Count;
            if (PlayerSteps == 0)
            {
                if (board.CompFig > board.PlayersFig)
                    MessageBox.Show("ÏÎÐÀÆÅÍÈÅ!!!");
                else
                {
                    if (board.CompFig != board.PlayersFig)
                        MessageBox.Show("ÏÎÁÅÄÀ!!!");
                    else
                        MessageBox.Show("ÍÈ×Üß!!!");
                }
            }
            flag = false;
            ShowRes();
        }

        private void ShowRes()
        {
            label1.Text = board.PlayersFig.ToString();
            label2.Text = board.CompFig.ToString();
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (flag) return;            
            int x = e.X / Board.RectWidth;
            int y = e.Y / Board.RectWidth;
            if (board.AddFig(x, y, 1, true) > 0)
            {
                flag = true;
                board.Draw(panel1, false, -1);
                CompStep();
                ShowRes();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InitBoard();
            board.Draw(panel1, IsDrawHelp, 1);
        }
    }
}