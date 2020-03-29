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

using System;
using System.Windows.Forms;

/// <summary>
/// A repeating button class.
/// When the mouse is held down on the button it will first wait for FirstDelay milliseconds,
/// then press the button every LoSpeedWait milliseconds until LoHiChangeTime milliseconds,
/// then press the button every HiSpeedWait milliseconds
/// </summary>
class RepeatingButton : Button
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RepeatingButton"/> class.
    /// </summary>
    public RepeatingButton()
    {
        internalTimer = new Timer();
        internalTimer.Interval = FirstDelay;
        internalTimer.Tick += new EventHandler(internalTimer_Tick);
        this.MouseDown += new MouseEventHandler(RepeatingButton_MouseDown);
        this.MouseUp += new MouseEventHandler(RepeatingButton_MouseUp);
    }

    /// <summary>
    /// The delay before first repeat in milliseconds
    /// </summary>
    public int FirstDelay = 400;

    /// <summary>
    /// The delay in milliseconds between repeats before LoHiChangeTime
    /// </summary>
    public int LoSpeedWait = 400;

    /// <summary>
    /// The delay in milliseconds between repeats after LoHiChangeTime
    /// </summary>
    public int HiSpeedWait = 250;

    /// <summary>
    /// The changeover time between slow repeats and fast repeats in milliseconds
    /// </summary>
    public int LoHiChangeTime = 1200;

    private void RepeatingButton_MouseDown(object sender, MouseEventArgs e)
    {
        internalTimer.Tag = DateTime.Now;
        internalTimer.Start();
    }

    private void RepeatingButton_MouseUp(object sender, MouseEventArgs e)
    {
        internalTimer.Stop();
        internalTimer.Interval = FirstDelay;
    }

    private void internalTimer_Tick(object sender, EventArgs e)
    {
        this.OnClick(e);
        TimeSpan elapsed = DateTime.Now - ((DateTime)internalTimer.Tag);
        if (elapsed.TotalMilliseconds < LoHiChangeTime)
        {
            internalTimer.Interval = LoSpeedWait;
        }
        else
        {
            internalTimer.Interval = HiSpeedWait;
        }
    }

    private Timer internalTimer;
}