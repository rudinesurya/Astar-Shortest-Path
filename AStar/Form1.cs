using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AStar
{
    public partial class Form1 : Form
    {
        Button[,] grid;
        int[,] map;
        Queue<KeyValuePair<int, int>> targets = new Queue<KeyValuePair<int, int>>();
        List<KeyValuePair<int, int>> path;

        public Form1()
        {
            InitializeComponent();
        }

        private void NewMenuOnClick(object sender, EventArgs e)
        {
            NewForm popup = new NewForm();
            DialogResult dialogresult = popup.ShowDialog();
            if (dialogresult == DialogResult.OK)
            {
                Console.WriteLine("You clicked OK");
                Console.WriteLine("row = " + popup.row + " col = " + popup.col);
                CreateBoard(popup.row, popup.col);
            }
            else if (dialogresult == DialogResult.Cancel)
            {
                Console.WriteLine("You clicked either Cancel or X button in the top right corner");
            }
            popup.Dispose();
        }

        private void ClearPreviousGame()
        {
            if (grid == null)
                return;
            
            for (int r = 0; r < grid.GetLength(0); ++r)
            {
                for (int c = 0; c < grid.GetLength(1); ++c)
                {
                    if (Controls.Contains(grid[r, c]))
                    {
                        Controls.Remove(grid[r, c]);
                    }
                }
            }
        }

        private void CreateBoard(int row, int col)
        {
            this.Width = 22 * (col + 1) + 10;
            this.Height = 22 * row + 100;

            ClearPreviousGame();

            grid = new Button[row, col];
            map = new int[row, col];

            for (int r = 0; r < row; ++r)
            {
                for (int c = 0; c < col; ++c)
                {
                    Button btn = new Button();
                    btn.Text = " ";
                    btn.Name = r.ToString() + "," + c.ToString();
                    btn.Size = new System.Drawing.Size(22, 22);
                    btn.Location = new System.Drawing.Point(10 + c * 22, 40 + r * 22);
                    btn.BackColor = Color.White;
                    btn.MouseUp += Btn_MouseUp;
                    grid[r, c] = btn;
                    map[r, c] = 0;

                    Controls.AddRange(new System.Windows.Forms.Control[] { btn, });
                }
            }
        }

        private void Btn_MouseUp(object sender, MouseEventArgs me)
        {
            Button btnClick = sender as Button;

            if (btnClick == null)
            {
                return;
            }

            string[] split = btnClick.Name.Split(new Char[] { ',' });

            int r = System.Convert.ToInt32(split[0]);
            int c = System.Convert.ToInt32(split[1]);

            if (me.Button == MouseButtons.Left)
            {
                Console.WriteLine(btnClick.Name + " left clicked !");

                if (map[r, c] == 0)
                {
                    if (targets.Count < 2)
                        targets.Enqueue(new KeyValuePair<int, int>(r, c));
                    else
                    {
                        KeyValuePair<int,int> temp = targets.Dequeue();
                        grid[temp.Key, temp.Value].BackColor = Color.White;
                        targets.Enqueue(new KeyValuePair<int, int>(r, c));
                    }

                    //Recalculate path
                    var start = targets.First();
                    var end = targets.Last();

                    if (path != null)
                    {
                        for (int i = 2; i < path.Count-1; ++i)
                        {
                            int r_ = path[i].Key;
                            int c_ = path[i].Value;

                            grid[r_, c_].BackColor = Color.White;
                        }
                    }

                    AStar astar = new AStar(map);
                    path = astar.GetPath(start.Key, start.Value, end.Key, end.Value);

                    for (int i = 2; i < path.Count-1; ++i)
                    {
                        int r_ = path[i].Key;
                        int c_ = path[i].Value;

                        grid[r_, c_].BackColor = Color.Yellow;
                    }

                    grid[r, c].BackColor = Color.Green;
                }
            }
            else if (me.Button == MouseButtons.Right)
            {
                Console.WriteLine(btnClick.Name + " right clicked !");

                switch (map[r, c])
                {
                    case 0: //empty tile
                        //turn it into a wall
                        map[r, c] = 1;
                        grid[r, c].BackColor = Color.Black;
                        break;

                    case 1: //wall tile
                        //turn it back to empty
                        map[r, c] = 0;
                        grid[r, c].BackColor = Color.White;
                        break;
                }
            }

            Refresh();
        }
    }
}
