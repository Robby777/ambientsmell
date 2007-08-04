using System;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

namespace CSharp
{
    public partial class Scanner : Form
    {
        IVisitor m_visitor;
        public Scanner()
        {
            InitializeComponent();
        }

        private void btnFiles_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;

            m_visitor = new AssemblyVisitor();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                m_visitor.VisitFiles(ofd.FileNames);
            }
        }
    }
}