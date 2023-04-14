using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ConsoleTest
{
    public class CalcLoans
    {

        public CalcLoans()
        {
            DateTime startDay = new DateTime(2021, 07, 21, 11, 0, 0);
            DateTime befCalcStart = startDay;
            int principal = 270_000_000;
            int payed = 0;
            int installments = 360;
            //decimal interestRate = 0.0273m;
            decimal interestRate = 0.0273m;

            int totalInterests = 0;

            DateTime earlyRedemptionLimit = startDay.AddYears(3);
            decimal earlyRedemptionInterestRate = 0.012m;

            int targetPay = 1_000_000;
            int remainTargetPay = 0;

            for (int i = 0; i < installments; i++)
            {
                int session = i + 1;

                var redemptionDay = startDay.AddMonths(session);
                var calcEnd = redemptionDay.AddDays(-1);

                var businessDay = redemptionDay;
                if (businessDay.DayOfWeek == DayOfWeek.Sunday) businessDay = businessDay.AddDays(1);
                else if (businessDay.DayOfWeek == DayOfWeek.Saturday) businessDay = businessDay.AddDays(2);

                decimal rpd = interestRate / 365;
                if ((redemptionDay.Year % 4) == 0 || (befCalcStart.Year % 4) == 0)
                {
                    rpd = interestRate / 366;
                }

                // 조기상환 7회차 이상일때
                if (session >= 7)
                {
                    DateTime earlyRedemptionDate = new DateTime(2022, 2, 3, 11, 0, 0);
                    earlyRedemptionDate = earlyRedemptionDate.AddMonths(session - 7);
                    int earlyPay = 1000000;

                    if (session != 7 && session < 7 + 24)
                    {
                        earlyPay = 500000;
                    }
                    if (session == 7 + 24 + 1)
                    {
                        earlyPay += 13_000_000;
                    }
                    if (session == 9)
                    {
                        earlyPay += 30_000_000;
                    }

                    earlyPay += remainTargetPay;

                    EarlyPay(earlyPay, startDay, befCalcStart, out var earlyRedemptionPay, ref principal, payed, ref totalInterests, out var earlyInterest, earlyRedemptionLimit, earlyRedemptionInterestRate, rpd, earlyRedemptionDate);
                    Console.WriteLine($"{session}\t{earlyRedemptionDate:yyyy-MM-dd}\tEarly\t{earlyRedemptionPay + earlyInterest}\t{earlyRedemptionPay}\t{earlyInterest}");
                }

                // 일반적인 갚음
                payed = Pay(befCalcStart, principal, payed, session, installments, rpd, redemptionDay, out var principalAtPay, out var interest, out var totalPayed);
                totalInterests += interest;

                Console.WriteLine($"{session}\t{redemptionDay:yyyy-MM-dd}\t{businessDay:yyyy-MM-dd}\t{totalPayed}\t{principalAtPay}\t{interest}\t{principal - payed}\t{befCalcStart:yyyy-MM-dd}\t{calcEnd:yyyy-MM-dd}");

                remainTargetPay = Math.Max(0, targetPay - totalPayed);

                befCalcStart = redemptionDay;
            }
        }

        private static void EarlyPay(int earlyPay, DateTime startDay, DateTime befCalcStart, out int earlyRedemptionPay, ref int principal, int payed, ref int totalInterests, out int interest, DateTime earlyRedemptionLimit, decimal earlyRedemptionInterestRate, decimal rpd, DateTime earlyRedemptionDate)
        {
            decimal remainLimitOff = (decimal)(earlyRedemptionLimit - earlyRedemptionDate).Days;
            decimal calcIntrestRate = earlyRedemptionInterestRate * (remainLimitOff / (earlyRedemptionLimit - startDay).Days);
            decimal calcDay = (decimal)(earlyRedemptionDate - befCalcStart).TotalDays;

            earlyRedemptionPay = Math.Min(earlyPay, principal - payed);
            if (earlyRedemptionPay > 0)
            {
                int Fee = Math.Max(0, (int)(earlyRedemptionPay * calcIntrestRate));
                interest = (int)(earlyRedemptionPay * calcDay * rpd);

                principal -= earlyRedemptionPay;
                totalInterests += Fee;
                totalInterests += interest;

                interest += Fee;
            }
            else
            {
                interest = 0;
            }
        }

        private static int Pay(DateTime befCalcStart, int principal, int payed, int session, int installments, decimal rpd, DateTime redemptionDay, out int principalAtPay, out int interest, out int totalPayed)
        {
            decimal calcDay = (decimal)(redemptionDay - befCalcStart).TotalDays;

            principalAtPay = Math.Max(0, (principal - payed) / (installments - (session - 1)));
            if (principalAtPay > 0)
            {
                interest = (int)((principal - payed) * calcDay * rpd);
                totalPayed = principalAtPay + interest;
                payed += principalAtPay;
                return payed;
            }
            else
            {
                totalPayed = 0;
                interest = 0;
            }
            return payed;
        }
    }

}
