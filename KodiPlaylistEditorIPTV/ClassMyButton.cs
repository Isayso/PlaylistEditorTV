using System.Windows.Forms;

namespace PlaylistEditor
{
    class MyButton : Button
    {
        protected override bool ShowFocusCues
        {
            get
            {
                // return base.ShowFocusCues;
                return false;
            }
        }
    }
}
