using cAlgo.API;
using System.Collections.Generic;

namespace cAlgo
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class SimpleICTMarketStructure : Indicator
    {
        [Parameter("Show Short Term Swing Points", DefaultValue = true)]
        public bool ShowST { get; set; }

        [Parameter("Show Intermediate Term Swing Points", DefaultValue = true)]
        public bool ShowIT { get; set; }

        [Parameter("Show Long Term Swing Points", DefaultValue = true)]
        public bool ShowLT { get; set; }

        [Parameter("Label Color", DefaultValue = "Black")]
        public string LabelColor { get; set; }

        private List<(ChartText text, double price)> stHigh = new List<(ChartText, double)>();
        private List<(ChartText text, double price)> itHigh = new List<(ChartText, double)>();
        private List<(ChartText text, double price)> ltHigh = new List<(ChartText, double)>();

        private List<(ChartText text, double price)> stLow = new List<(ChartText, double)>();
        private List<(ChartText text, double price)> itLow = new List<(ChartText, double)>();
        private List<(ChartText text, double price)> ltLow = new List<(ChartText, double)>();

        protected override void Initialize()
        {
            ictSimpleMarketStructure();
        }

        private void ictSimpleMarketStructure()
        {
            ictSwingStructure(stHigh, itHigh, ltHigh, "high");
            ictSwingStructure(stLow, itLow, ltLow, "low");
        }

        private void ictSwingStructure(List<(ChartText text, double price)> stA,
                                       List<(ChartText text, double price)> itA,
                                       List<(ChartText text, double price)> ltA, 
                                       string type)
        {
            double swing = type == "high" ? Bars.HighPrices.Maximum(3) : Bars.LowPrices.Minimum(3);
            double price = type == "high" ? Bars.HighPrices.Last(1) : Bars.LowPrices.Last(1);
            string lblText = type == "high" ? "▲" : "▼";

            if (swing == -1) // Not a valid swing point
                return;

            DrawLabel(stA, price, ShowST ? "." : null, lblText);

            if (stA.Count > 2 && IsOldSwing(stA, type))
            {
                var it = CopyLabel(stA[stA.Count - 2], ShowIT ? "△" : null);
                AddLabel(itA, it);
            }

            if (itA.Count > 2 && IsOldSwing(itA, type))
            {
                var lt = CopyLabel(itA[itA.Count - 2], ShowLT ? "▲" : null);
                AddLabel(ltA, lt);
            }
        }

        private void DrawLabel(List<(ChartText text, double price)> list, double price, string text, string labelStyle)
        {
            if (string.IsNullOrEmpty(text))
                return;

            var label = Chart.DrawText("Label_" + list.Count, text, Bars.ClosePrices.Count - 1, price, Color.FromName(LabelColor));
            list.Add((label, price));
        }

        private (ChartText text, double price) CopyLabel((ChartText text, double price) labelData, string text)
        {
            if (string.IsNullOrEmpty(text))
                return (null, 0);

            var newLabel = Chart.DrawText("Copy_" + labelData.text.Name, text, Bars.ClosePrices.Count - 1, labelData.price, Color.FromName(LabelColor));
            return (newLabel, labelData.price);
        }

        private void AddLabel(List<(ChartText text, double price)> list, (ChartText text, double price) labelData)
        {
            if (list.Count == 0 || list[list.Count - 1].price != labelData.price)
                list.Add(labelData);
        }

        private bool IsOldSwing(List<(ChartText text, double price)> list, string type)
        {
            if (list.Count < 3)
                return false;

            double y1 = list[list.Count - 1].price;
            double y2 = list[list.Count - 2].price;
            double y3 = list[list.Count - 3].price;

            return (type == "high" && y1 < y2 && y2 > y3) || (type == "low" && y1 > y2 && y2 < y3);
        }

        public override void Calculate(int index)
        {
            throw new System.NotImplementedException();
        }
    }
}
