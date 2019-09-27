using System;
using System.Drawing;
using System.Windows.Forms;
using static Square;

namespace TicTacToe{

	internal partial class Opend : Form{

		private TextBox Text_Box = new TextBox(){
			Width = 300,
			Height = 100,
			BackColor = Color.White,
			ForeColor = Color.Black, 
			Location = new Point(50, 150) 
		};

		private Button FirstButton = new Button(){
			Location = new System.Drawing.Point(75, 125),
			Size = new System.Drawing.Size(250, 60),
			Text = "With a computer",
			BackColor = Color.DarkOrchid,
			FlatStyle = FlatStyle.Popup,
			ForeColor = Color.Black,
			Font = new Font("Comic Sans MS", 13f)
		};

		private Button SecondButton = new Button(){
			Location = new System.Drawing.Point(75, 225),
			Size = new System.Drawing.Size(250, 60),
			Text = "With a friend",
			BackColor = Color.DarkOrchid,
			FlatStyle = FlatStyle.Popup,
			ForeColor = Color.Black,
			Font = new Font("Comic Sans MS", 13f)
		};

		private Label Label_ = new Label(){
			AutoSize = false, 
			TextAlign = ContentAlignment.MiddleCenter, 
			Dock = DockStyle.None,
			Width = 350,
			Location = new Point(20, 10),
			Height = 25,
			Font = new Font("Comic Sans MS", 15),
			ForeColor = Color.Magenta
		};
	}
}
