using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ConsoleTest
{
    public class CalcInstallmentSaving
    {

        public CalcInstallmentSaving()
        {
            int principalPerMonth = 500000;
            decimal interestRate = 0.06m;
            int installment = 2 * 12;
            DateTime regist = new DateTime(2022, 02, 24, 9, 0, 0);
            DateTime endDate = new DateTime(2024, 02, 24, 9, 0, 0);
            DateTime befPayDate = regist;

            int payDay = 1;

            int totalPrincipal = 0;
            int totalIntrest = 0;
            Pay(principalPerMonth, interestRate, regist, endDate, ref totalPrincipal, ref totalIntrest);

            for (int session = 2; session <= installment; session++)
            {
                var payDate = befPayDate.AddMonths(1);
                payDate = payDate.AddDays(-(payDate.Day - payDay));

                Console.Write($"{session}\t{payDate:yyyy-MM-dd}\t");
                Pay(principalPerMonth, interestRate, payDate, endDate, ref totalPrincipal, ref totalIntrest);

                befPayDate = payDate;
            }
            Console.WriteLine($"TotalPrincipal: {totalPrincipal:N0}\r\nTotalInterest: {totalIntrest:N0}");
        }

        private void Pay(int principalPerMonth, decimal interestRate, DateTime regist, DateTime end, ref int totalPrincipal, ref int totalIntrest)
        {
            var days = (decimal)(end - regist).TotalDays;
            int interest = (int)(principalPerMonth * (interestRate * (days / 365)));

            totalPrincipal += principalPerMonth;
            totalIntrest += interest;
            Console.WriteLine($"{principalPerMonth}\t{interest}");
        }
    }

}
