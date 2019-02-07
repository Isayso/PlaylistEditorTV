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
    public int LoSpeedWait = 250;

    /// <summary>
    /// The delay in milliseconds between repeats after LoHiChangeTime
    /// </summary>
    public int HiSpeedWait = 50;

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