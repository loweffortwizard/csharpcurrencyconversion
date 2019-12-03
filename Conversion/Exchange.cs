using System;
using System.Collections.Generic;
using System.Text;

namespace Conversion
{
    public class Exchange
    {
        /// <summary>
        /// Class instatination for Exchange object.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="rate"></param>
        /// <param name="date"></param>
        public Exchange(string from, string to, double rate, DateTime date)
        {
            ConFrom = from;
            ConTo = to;
            ConRate = rate;
            ConDate = date;
        }//end exchange

        /// <summary>
        /// Get/set methods for getting and setting conversion rate, to, from and time/date. 
        /// </summary>
      
        public string ConFrom { get; }
        public string ConTo { get; }
        public double ConRate { get; }
        public DateTime ConDate { get; set; }

        /// <summary>
        /// Method for converting amount entered by conversion/exchange rate.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public double Convert(double amount)
        {
            return amount * ConRate;
        }//end convert

    }//end class
}//end name
