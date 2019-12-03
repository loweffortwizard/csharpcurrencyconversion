using System;
using System.Collections.Generic;
using System.Text;

namespace Conversion
{
    public class AvgConvert
    {
        /// <summary>
        /// Method for converting the avarage from input, and API's.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="quant"></param>
        /// <returns></returns>
        public static double ConvertAvg(string from, string to, double quant)
        {
            //assign value to vars
            double fix = FixerIO.Convert(from, to, quant);
            double openex = OpenEx.Convert(from, to, quant);

            //add sums together then divide by amount used for avg
            double sum = fix + openex;
            double result = sum / 2;

            //return the result
            return result;
        }//end 
    }//end class
}//end namespace
