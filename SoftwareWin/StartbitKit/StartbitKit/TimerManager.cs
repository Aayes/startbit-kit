using Multimedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StartbitKit
{
    class TimerManager
    {
        MTimer mTimer;
        public void Test()
        {
            mTimer = new MTimer();
            mTimer.Mode = Multimedia.TimerMode.Periodic;
            mTimer.Period = 1;//ms
            mTimer.Resolution = 1;
            mTimer.Tick += mTimer_Tick;
            //mTimer.Start();
        }

        private void mTimer_Tick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
