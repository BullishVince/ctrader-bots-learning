//  using cAlgo.API;
//  namespace cAlgo
//  {
//      // This sample shows how to use Chart.DrawText method to draw text
//      [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
//      public class ChartTextSample : Indicator
//      {
//          protected override void Initialize()
//          {
//          }
//          public override void Calculate(int index)
//          {
//             for (int iBarIndex = Chart.FirstVisibleBarIndex; iBarIndex <= Chart.LastVisibleBarIndex; iBarIndex++)
//              {
//                  string text;
//                  double y;
//                  Color color;
//                  if (Bars.ClosePrices[iBarIndex] > Bars.OpenPrices[iBarIndex])
//                  {
//                      text = "U";
//                      y = Bars.LowPrices[iBarIndex];
//                      color = Color.Green;
//                  }
//                  else
//                  {
//                      text = "D";
//                      y = Bars.HighPrices[iBarIndex];
//                      color = Color.Red;
//                  }
//                  Chart.DrawText("Text_" + iBarIndex, text, iBarIndex, y, color);
//              }
//          }
//      }
//  }