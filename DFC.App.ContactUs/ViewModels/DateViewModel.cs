using System;

namespace DFC.App.ContactUs.ViewModels
{
    public abstract class DateViewModel
    {
        protected const string RegExForDay = "^(0?[1-9]|[0-2][1-9]|[0-3][01])$";
        protected const string RegExForMonth = "^(0?[1-9]|1[0-2])$";
        protected const string RegExForYear = "^(19|20)[0-9]{2}$";
        protected const string RegExForHour = "^(0?[0-9]|[0-1][0-9]|[0-2][0-3])$";
        protected const string RegExForMinute = "^(0?[0-9]|[0-5][0-9])$";

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

        public bool ContainsNullValueFields
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
                if (ContainsNullValueFields)
                {
                    return false;
                }

                bool isValid = Year!.Value >= 1 && Year!.Value <= 9999 &&
                               Month!.Value >= 1 && Month!.Value <= 12 &&
                               Day!.Value >= 1 && Day!.Value <= DateTime.DaysInMonth(Year!.Value, Month!.Value);

                if (isValid && IncludeTimeValue)
                {
                    isValid = Hour!.Value >= 0 && Hour!.Value < 24 &&
                              Minute!.Value >= 0 && Minute!.Value < 60;
                }

                return isValid;
            }
        }

        public string FirstMissingFieldName
        {
            get
            {
                if (!Day.HasValue)
                {
                    return nameof(Day);
                }

                if (!Month.HasValue)
                {
                    return nameof(Month);
                }

                if (!Year.HasValue)
                {
                    return nameof(Year);
                }

                if (IncludeTimeValue)
                {
                    if (!Hour.HasValue)
                    {
                        return nameof(Hour);
                    }

                    if (!Minute.HasValue)
                    {
                        return nameof(Minute);
                    }
                }

                return string.Empty;
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
