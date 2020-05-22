using System;

namespace DFC.App.ContactUs.ViewModels
{
    public abstract class DateViewModel
    {
        protected DateViewModel()
        {
        }

        protected DateViewModel(DateTime dateTime)
        {
            Day = dateTime.Day;
            Month = dateTime.Month;
            Year = dateTime.Year;
            Hour = dateTime.Hour;
            Minute = dateTime.Minute;
        }

        public DateTime? Value
        {
            get
            {
                if (IsValid)
                {
                    if (IncludeTimeValue)
                    {
                        return new DateTime(Year!.Value, Month!.Value, Day!.Value, Hour!.Value, Minute!.Value, 0);
                    }
                    else
                    {
                        return new DateTime(Year!.Value, Month!.Value, Day!.Value);
                    }
                }

                return default;
            }
        }

        public bool IsNull
        {
            get
            {
                if (IncludeTimeValue)
                {
                    if (Day == null || Month == null || Year == null || Hour == null || Minute == null)
                    {
                        return true;
                    }
                }
                else
                {
                    if (Day == null || Month == null || Year == null)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool IsValid
        {
            get
            {
                if (IsNull)
                {
                    return false;
                }

                bool isValid = Year.Value >= 1 && Year.Value <= 9999 &&
                               Month.Value >= 1 && Month.Value <= 12 &&
                               Day.Value >= 1 && Day.Value <= DateTime.DaysInMonth(Year.Value, Month.Value);

                if (isValid && IncludeTimeValue)
                {
                    isValid = Hour.Value >= 0 && Hour.Value < 24 &&
                              Minute.Value >= 0 && Minute.Value < 60;
                }

                return isValid;
            }
        }

        public abstract int? Day { get; set; }

        public abstract int? Month { get; set; }

        public abstract int? Year { get; set; }

        public abstract int? Hour { get; set; }

        public abstract int? Minute { get; set; }

        public abstract bool IncludeTimeValue { get; set; }
    }
}
