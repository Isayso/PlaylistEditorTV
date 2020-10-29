//  MIT License
//  Copyright (c) 2018 github/isayso
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation
//  files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy,
//  modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
//  subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlaylistEditor
{
    class NotificationBox
    {
        public static DialogResult Show(string text)
        {
            PopupForm(text, 2000, NotificationMsg.OK);
            return DialogResult.OK;
        }

        public static DialogResult Show(string text, int delay, NotificationMsg message)
        {
            PopupForm(text, delay, message);
            return DialogResult.OK;
        }

        public static async void PopupForm(string label, int delay = 4000, NotificationMsg message = NotificationMsg.OK)
        {
            await PopupDelay(label, delay, message);
        }

        public static async Task PopupDelay(string label, int delay, NotificationMsg message)
        {
            NotificationBoxF box = new NotificationBoxF(label, message);

            box.Show();
            await Task.Delay(delay);

            box.Close();
        }


    }

    public enum NotificationMsg
    {
        OK,
        ERROR,
        DONE
    }
}
