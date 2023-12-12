using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemTextManager.Launcher
{
    public class ProgressInfo
    {
        public bool OnlyNowStepChange { get; set; }
        public int NowStep { get; set; }
        public int TotalStep { get; set; }
        public int NowFileCnt { get; set; }
        public int TotalFileCnt { get; set; }
        public string DisplayText { get; set; }

        public ProgressInfo()
        {
            OnlyNowStepChange = false;
        }

        public ProgressInfo(bool onlyNowStepChange, int nowStep, int totalStep, int nowFileCnt, int totalFileCnt, string displayText)
        {
            OnlyNowStepChange = onlyNowStepChange;
            NowStep = nowStep;
            TotalStep = totalStep;
            NowFileCnt = nowFileCnt;
            TotalFileCnt = totalFileCnt;
            DisplayText = displayText;
        }   
    }
}
