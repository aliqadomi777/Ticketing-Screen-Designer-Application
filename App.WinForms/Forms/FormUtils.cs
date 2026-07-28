using System.Windows.Forms;

namespace App.WinForms
{
    internal class FormUtils
    {
        public static void CenterToForm(Form parent, Form child)
        {
            child.StartPosition = FormStartPosition.Manual;
            child.Left = parent.Left + (parent.Width - child.Width) / 2;
            child.Top = parent.Top + (parent.Height - child.Height) / 2;
        }

    }
}
