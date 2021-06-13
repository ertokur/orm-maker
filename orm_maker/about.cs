using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace orm_maker {
    public partial class about : Form {
        public about() {
            InitializeComponent();
        }
        public bool flag = true;
        private void about_Load(object sender, EventArgs e) {
            Label logo = new Label();
            logo.Text = "  ______      __                   __           \n";
            logo.Text += " /_  __/___  / /____  ____________/ /____  __   \n";
            logo.Text += "  / / / __ \\/ //_/ / / / ___/ ___/ //_/ / / /  \n";
            logo.Text += " / / / /_/ / ,< / /_/ / /  (__  ) ,< / /_/ /    \n";
            logo.Text += "/_/  \\____/_/|_|\\__,_/_/  /____/_/|_|\\__, /  \n";
            logo.Text += "\\________________________________________/     \n";
            logo.Text += "ORM-Maker v.1.1 \n";
            if (flag)
                logo.Text += "Made with ❤ by Tokursky, 2021";
            else
                logo.Text += "Сделано Токурским с ❤, 2021";
            logo.Parent = this;
            logo.Location = new Point(10, 10);
            logo.AutoSize = true;
            logo.Font = new Font("Consolas", 12, FontStyle.Regular);
            logo.Show();
        }
    }
}
